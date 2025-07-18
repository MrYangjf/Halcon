using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MasonteVision.Halcon;
using MasonteVision.Halcon.SmartRay3DTool;
using System.IO;
using HalconDotNet;
using MasonteVision.PCB_AOI_ZY_CAM;
using MasonteDataProcess.FileProcess;
using System.Threading;

namespace MasonteVision
{
    public partial class MainForm : UserControl
    {

        string[] _ImageSavePath = new string[4];
        public string[] ImageSavePath//图像保存路径
        {
            get { return _ImageSavePath; }
        }

        public string PCBtestName = "PCB161206-V03";//pcb板类型
        public string[] CodeStr = new string[4];//当前板Code
        public int DutIndex = 0;//当前小板的序列号
        public int DotIndex = 0;//当前点的序列号
        public int ColorSelect = 0;//颜色参数排序
        private Thread[] RunningThread = new Thread[4];
        bool ChangeParam;
        bool StopRunFiles;
        public bool IsInitCamGUI;//内存初始化
        public bool[] IsInitZYCam = new bool[4];//相机初始化OK
        public bool[] IsViewer = new bool[4];//相机预览开启
        public bool[] IsCamBusing = new bool[4];//相机程序工作中
        public bool[] IsCamRunFinished = new bool[4];//相机分析完成
        public bool[] IsCamOnline = new bool[4];//相机在线
        public bool[] IsToolRunOK = new bool[4];//工具运行OK
        public bool[] IsRunPass = new bool[4];//运行结果
        public bool[] IsLoadParamters = new bool[4];
        private bool[] StartRunning = new bool[4];
        DateTime[] StartTime = new DateTime[4];
        DateTime[] EndTime = new DateTime[4];
        int[] OutTimeStep = new int[4];

        public string CameraRunErrorMsg;//运行错误信息提示
        private string Trigger;
        string logpath = Application.StartupPath + "\\LOG\\";
        string GenralPath;
        bool mTest2DMode;
        bool IsReadParam;//updata使用
        int Cam2DUsingSelectIndex = -1;
        HaGUI[] mHaGUI = new HaGUI[4];
        HalconModelAndFlow[] my2DhalconFlow = new HalconModelAndFlow[4];
        ParamsClass[] myCamParaClass = new ParamsClass[4];
        PictureBox[] ViewWindows = new PictureBox[4];
        LOGFile mLog = new LOGFile();

        public bool Init2DUI()
        {
            if (IsInitCamGUI) return false;
            int nRet = DllFunction.WF_InitRes(4, SPWF_RESOLUTION_TYPE.SPWF_CAMERA_4K);
            if (nRet != 0)
            {
                MessageBox.Show("内存申请失败！");
                DllFunction.WF_UnInitRes();
            }
            for (int i = 0; i < 4; i++)
            {
                mHaGUI[i] = new HaGUI();
                mHaGUI[i].Test2dMode = true;
                mHaGUI[i].Dock = DockStyle.Fill;
                mHaGUI[i].GUIName = string.Format("Cam{0}", i);
                tableLayoutPanel1.Controls.Add(mHaGUI[i]);
                my2DhalconFlow[i] = new HalconModelAndFlow(PCBtestName, i, ColorSelect);
                ViewWindows[i] = new PictureBox();
                ViewWindows[i].BackgroundImageLayout = ImageLayout.Stretch;
                ViewWindows[i].SizeMode = PictureBoxSizeMode.StretchImage;
                ViewWindows[i].BackColor = Color.Gray;
                ViewWindows[i].Dock = DockStyle.Fill;
                ViewPanel.Controls.Add(ViewWindows[i]);
                myCamParaClass[i] = my2DhalconFlow[i].LoadZYCameraConn();
                my2DhalconFlow[i].LoadAllParamFromXML(5);
            }
            tableLayoutPanel1.BringToFront();
            mHaGUI[0].toolStrip2.DoubleClick += MainForm_UI0_DoubleClick;
            mHaGUI[1].toolStrip2.DoubleClick += MainForm_UI1_DoubleClick;
            mHaGUI[2].toolStrip2.DoubleClick += MainForm_UI2_DoubleClick;
            mHaGUI[3].toolStrip2.DoubleClick += MainForm_UI3_DoubleClick;
            ViewWindows[0].DoubleClick += MainForm_DoubleClick0;
            ViewWindows[1].DoubleClick += MainForm_DoubleClick1;
            ViewWindows[2].DoubleClick += MainForm_DoubleClick2;
            ViewWindows[3].DoubleClick += MainForm_DoubleClick3;
            IsInitCamGUI = true;
            return IsInitCamGUI;
        }

        public bool Init2DUI(string PCBName, int Colorset)
        {
            ColorSelect = Colorset;
            label_ColorSet.Text = ColorSelect.ToString();
            PCBtestName = PCBName;
            if (IsInitCamGUI) return false;
            int nRet = DllFunction.WF_InitRes(4, SPWF_RESOLUTION_TYPE.SPWF_CAMERA_4K);
            if (nRet != 0)
            {
                MessageBox.Show("内存申请失败！");
                DllFunction.WF_UnInitRes();
            }
            for (int i = 0; i < 4; i++)
            {
                mHaGUI[i] = new HaGUI();
                mHaGUI[i].Test2dMode = true;
                mHaGUI[i].Dock = DockStyle.Fill;
                mHaGUI[i].GUIName = string.Format("Cam{0}", i);
                ViewWindows[i] = new PictureBox();
                ViewWindows[i].BackgroundImageLayout = ImageLayout.Stretch;
                ViewWindows[i].SizeMode = PictureBoxSizeMode.StretchImage;
                ViewWindows[i].BackColor = Color.Gray;
                ViewWindows[i].Dock = DockStyle.Fill;
                tableLayoutPanel1.Controls.Add(mHaGUI[i]);
                ViewPanel.Controls.Add(ViewWindows[i]);
                my2DhalconFlow[i] = new HalconModelAndFlow(PCBName, i, ColorSelect);
                myCamParaClass[i] = my2DhalconFlow[i].LoadZYCameraConn();
                my2DhalconFlow[i].LoadAllParamFromXML(5);
            }
            tableLayoutPanel1.BringToFront();
            mHaGUI[0].toolStrip2.DoubleClick += MainForm_UI0_DoubleClick;
            mHaGUI[1].toolStrip2.DoubleClick += MainForm_UI1_DoubleClick;
            mHaGUI[2].toolStrip2.DoubleClick += MainForm_UI2_DoubleClick;
            mHaGUI[3].toolStrip2.DoubleClick += MainForm_UI3_DoubleClick;
            ViewWindows[0].DoubleClick += MainForm_DoubleClick0;
            ViewWindows[1].DoubleClick += MainForm_DoubleClick1;
            ViewWindows[2].DoubleClick += MainForm_DoubleClick2;
            ViewWindows[3].DoubleClick += MainForm_DoubleClick3;

            ViewWindows[0].SizeChanged += MainForm_SizeChanged0;
            ViewWindows[1].SizeChanged += MainForm_SizeChanged1;
            ViewWindows[2].SizeChanged += MainForm_SizeChanged2;
            ViewWindows[3].SizeChanged += MainForm_SizeChanged3;

            IsInitCamGUI = true;
            return IsInitCamGUI;
        }
        private void MainForm_SizeChanged0(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            DllFunction.WF_SetVideoWindowPos(0, 0, 0, ViewWindows[0].Width, ViewWindows[0].Height);
        }
        private void MainForm_SizeChanged1(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            DllFunction.WF_SetVideoWindowPos(1, 0, 0, ViewWindows[1].Width, ViewWindows[1].Height);
        }
        private void MainForm_SizeChanged2(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            DllFunction.WF_SetVideoWindowPos(2, 0, 0, ViewWindows[2].Width, ViewWindows[2].Height);
        }
        private void MainForm_SizeChanged3(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            DllFunction.WF_SetVideoWindowPos(3, 0, 0, ViewWindows[3].Width, ViewWindows[3].Height);
        }
        public bool ReloadFlowDependNameAndColor(string PCBName, int Colorset)
        {
            IsInitCamGUI = false;
            ColorSelect = Colorset;
            PCBtestName = PCBName;
            for (int i = 0; i < 4; i++)
            {
                my2DhalconFlow[i] = new HalconModelAndFlow(PCBName, i, ColorSelect);
                myCamParaClass[i] = my2DhalconFlow[i].LoadZYCameraConn();
            }
            IsInitCamGUI = true;
            return IsInitCamGUI;
        }

        public bool ReloadFlowDependNameAndColorIndex(int CameraIndex, string PCBName, int Colorset)
        {
            IsInitCamGUI = false;
            ColorSelect = Colorset;
            PCBtestName = PCBName;

            my2DhalconFlow[CameraIndex] = new HalconModelAndFlow(PCBName, CameraIndex, ColorSelect);
            myCamParaClass[CameraIndex] = my2DhalconFlow[CameraIndex].LoadZYCameraConn();

            IsInitCamGUI = true;
            return IsInitCamGUI;
        }

        private void CreatNewPCBTestFlowIndex(int Cam, String NewTestName)
        {
            string OldPath = string.Format(Application.StartupPath + "\\INI\\{0}\\{1}\\", PCBtestName, Cam);
            string Path = string.Format(Application.StartupPath + "\\INI\\{0}\\{1}\\", NewTestName, Cam);
            if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
            DirectoryInfo BackUpInfo = new DirectoryInfo(OldPath);
            FileInfo[] BackUpFiles = BackUpInfo.GetFiles();
            for (int i = 0; i < BackUpFiles.Length; i++)
            {
                BackUpFiles[i].CopyTo(Path + BackUpFiles[i].Name, true);
            }
            ReloadFlowDependNameAndColorIndex(Cam, NewTestName, ColorSelect);
        }

        public void CreatNewPCBTestFlow(string NewTestName)
        {
            CreatNewPCBTestFlowIndex(0, NewTestName);
            CreatNewPCBTestFlowIndex(1, NewTestName);
            CreatNewPCBTestFlowIndex(2, NewTestName);
            CreatNewPCBTestFlowIndex(3, NewTestName);
        }

        public static long IpToInt(string ip)
        {
            string[] items = ip.Split('.');
            return long.Parse(items[3]) << 24
                        | long.Parse(items[2]) << 16
                        | long.Parse(items[1]) << 8
                        | long.Parse(items[0]);
        }

        public void InitPCB_AOI_CAM()
        {
            int nRet = 0;
            if (!IsInitCamGUI)
            {
                nRet = DllFunction.WF_InitRes(4, SPWF_RESOLUTION_TYPE.SPWF_CAMERA_4K);
                if (nRet != 0)
                {
                    MessageBox.Show("内存申请失败！");
                    DllFunction.WF_UnInitRes();
                }
            }
            for (int i = 0; i < 4; i++)
            {
                IsInitZYCam[i] = false;

                myCamParaClass[i].m_iPort = myCamParaClass[i].MAXPORT;

                string ip = myCamParaClass[i].CameraIP;
                long ipInit = IpToInt(ip);
                uint ulIPAddr = (uint)ipInit;
                nRet = DllFunction.WF_InitCamera(i, ulIPAddr, (ushort)myCamParaClass[i].m_iPort);
                if (nRet != 0)
                {
                    MessageBox.Show("内存申请失败！");
                    DllFunction.WF_UnInitRes();
                }

                int nret = DllFunction.WF_GetAllParam(i, ref myCamParaClass[i].m_Param);
                if (nRet != 0)
                {
                    MessageBox.Show("获取参数失败！");
                }
                else
                {
                    IsInitZYCam[i] = true;
                }
            }
        }

        public void StartGrab2DCamera()
        {
            for (int i = 0; i < 4; i++)
            {
                if (IsViewer[i]) break;

                DllFunction.WF_SetNewHwnd(i, ViewWindows[i].Handle);

                DllFunction.WF_SetVideoWindowPos(i, 0, 0, ViewWindows[i].Width, ViewWindows[i].Height);

                DllFunction.WF_LiveStart(i);

                IsViewer[i] = true;

                IsCamOnline[i] = true;
            }
        }

        public void StopGrab2DCamera()
        {
            for (int i = 0; i < 4; i++)
            {
                DllFunction.WF_LiveStop(i);
                IsViewer[i] = false;
            }
        }

        public void Dispose2DCamera()
        {
            for (int i = 0; i < 4; i++)
            {
                DllFunction.WF_LiveStop(i);
                IsViewer[i] = false;
            }
            DllFunction.WF_UnInitRes();
        }

        public void InitPCB_AOI_CAMIndex()
        {
            int nRet;

            IsInitZYCam[Cam2DUsingSelectIndex] = false;

            myCamParaClass[Cam2DUsingSelectIndex].m_iPort = myCamParaClass[Cam2DUsingSelectIndex].MAXPORT;

            string ip = myCamParaClass[Cam2DUsingSelectIndex].CameraIP;
            long ipInit = IpToInt(ip);
            uint ulIPAddr = (uint)ipInit;
            nRet = DllFunction.WF_InitCamera(Cam2DUsingSelectIndex, ulIPAddr, (ushort)myCamParaClass[Cam2DUsingSelectIndex].m_iPort);
            if (nRet != 0)
            {
                MessageBox.Show("内存申请失败！");
                DllFunction.WF_UnInitRes();
            }

            myCamParaClass[Cam2DUsingSelectIndex].m_Param.AEstate = 0;

            int nret = DllFunction.WF_GetAllParam(Cam2DUsingSelectIndex, ref myCamParaClass[Cam2DUsingSelectIndex].m_Param);
            if (nRet != 0)
            {
                MessageBox.Show("获取参数失败！");
            }
            else
            {
                IsInitZYCam[Cam2DUsingSelectIndex] = true;
            }

        }

        public void StartGrab2DCameraIndex(int Index)
        {
            if (IsViewer[Index]) return;

            DllFunction.WF_SetNewHwnd(Index, ViewWindows[Index].Handle);

            DllFunction.WF_SetVideoWindowPos(Index, 0, 0, ViewWindows[Index].Width, ViewWindows[Index].Height);

            DllFunction.WF_LiveStart(Index);

            //int nret = DllFunction.WF_GetAllParam(Cam2DUsingSelectIndex, ref myCamParaClass[Cam2DUsingSelectIndex].m_Param);

            IsViewer[Index] = true;

        }

        public void StopGrab2DCameraIndex(int Index)
        {

            DllFunction.WF_LiveStop(Index);
            IsViewer[Index] = false;
            IsReadParam = false;
        }

        public void StartGrab2DCameraIndex()
        {

            DllFunction.WF_SetNewHwnd(Cam2DUsingSelectIndex, ViewWindows[Cam2DUsingSelectIndex].Handle);

            DllFunction.WF_SetVideoWindowPos(Cam2DUsingSelectIndex, 0, 0, ViewWindows[Cam2DUsingSelectIndex].Width, ViewWindows[Cam2DUsingSelectIndex].Height);

            DllFunction.WF_LiveStart(Cam2DUsingSelectIndex);

            //int nret = DllFunction.WF_GetAllParam(Cam2DUsingSelectIndex, ref myCamParaClass[Cam2DUsingSelectIndex].m_Param);

            IsViewer[Cam2DUsingSelectIndex] = true;

        }

        public void StopGrab2DCameraIndex()
        {
            DllFunction.WF_LiveStop(Cam2DUsingSelectIndex);
            IsViewer[Cam2DUsingSelectIndex] = false;
            IsReadParam = false;
        }

        public void Dispose2DCameraIndex()
        {
            DllFunction.WF_LiveStop(Cam2DUsingSelectIndex);
            IsViewer[Cam2DUsingSelectIndex] = false;
        }

        public void UpdateParam(ParamsClass pread)
        {
            IsReadParam = false;

            trackBar1.SetRange(pread.EXPOSURE_MIN, pread.EXPOSURE_MAX);
            trackBar2.SetRange(pread.BESTAE_MIN, pread.BESTAE_MAX);
            trackBar3.SetRange(pread.GLOBALGAIN_MIN, pread.GLOBALGAIN_MAX);
            trackBar4.SetRange(pread.RGAIN_MIN, pread.RGAIN_MAX);
            trackBar5.SetRange(pread.BGAIN_MIN, pread.BGAIN_MAX);
            trackBar6.SetRange(pread.SATURATION_MIN, pread.SATURATION_MAX);
            trackBar7.SetRange(pread.CONTRAST_MIN, pread.CONTRAST_MAX);
            trackBar8.SetRange(pread.SHARPEN_MIN, pread.SHARPEN_MAX);
            trackBar9.SetRange(pread.HDR_MIN, pread.HDR_MAX);

            trackBar2.Value = (int)pread.m_Param.bestAE;
            trackBar3.Value = (int)pread.m_Param.gain;
            trackBar4.Value = (int)pread.m_Param.rgain;
            trackBar5.Value = (int)pread.m_Param.bgain;
            trackBar6.Value = (int)pread.m_Param.saturation;
            trackBar7.Value = (int)pread.m_Param.contrast;
            trackBar8.Value = (int)pread.m_Param.sharp;
            trackBar9.Value = (int)pread.m_Param.hdr;

            if (pread.m_Param.Hflip == 1)
            {
                checkBox2.Checked = true;
            }
            else
            {
                checkBox2.Checked = false;
            }

            if (pread.m_Param.Vflip == 1)
            {
                checkBox3.Checked = true;
            }
            else
            {
                checkBox3.Checked = false;
            }

            if (pread.m_Param.Temperature == 1)
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }



            checkBox4.Checked = false;

            trackBar1.SetRange(myCamParaClass[Cam2DUsingSelectIndex].EXPOSURE_MIN, myCamParaClass[Cam2DUsingSelectIndex].EXPOSURE_MAX);
            //int nvalue = GetExposureValue(pread.exposure);
            int nvalue = (int)pread.m_Param.exposure;
            trackBar1.Value = (int)nvalue;
            ExposureParam(nvalue);


            if (pread.m_Param.AEstate == 1)
            {
                checkBox5.Checked = true;
            }
            else
            {
                checkBox5.Checked = false;
            }

            IsReadParam = true;

        }

        public void ExposureParam(int nValue)
        {
            ulong lExpTime = 0;
            ulong lValue = 0;

            if (myCamParaClass[Cam2DUsingSelectIndex].m_nCameraType == 1)
            {
                //lExpTime = (ulong)(nValue*3 + 32);

                double temp = nValue;
                label19.Text = temp.ToString();
            }
            else
            {
                //lExpTime = (ulong)(7+ nValue*3);

                double temp = nValue;
                label19.Text = temp.ToString();
            }

            DllFunction.WF_SetExposure(Cam2DUsingSelectIndex, (uint)nValue);
        }

        public void LoadTestParameters(string PCBName, int CameraIndex, int ColorSelect)
        {
            IsLoadParamters[CameraIndex] = false;
            my2DhalconFlow[CameraIndex] = new HalconModelAndFlow(PCBName, CameraIndex, ColorSelect);
            my2DhalconFlow[CameraIndex].LoadHalconFlow(mHaGUI[CameraIndex]);
            my2DhalconFlow[CameraIndex].LoadAllParamFromXML(5);
            IsLoadParamters[CameraIndex] = true;
        }

        public void CreatTestParameters(string PCBName, int CameraIndex, int ColorSelect)
        {
            IsLoadParamters[CameraIndex] = false;
            my2DhalconFlow[CameraIndex] = new HalconModelAndFlow(PCBName, CameraIndex, ColorSelect, true);
            my2DhalconFlow[CameraIndex].LoadHalconFlow(mHaGUI[CameraIndex]);
            IsLoadParamters[CameraIndex] = true;
        }

        public void LoadTestParameters(int CameraIndex, int ColorSelect)
        {
            IsLoadParamters[CameraIndex] = false;
            my2DhalconFlow[CameraIndex] = new HalconModelAndFlow(PCBtestName, CameraIndex, ColorSelect);
            my2DhalconFlow[CameraIndex].LoadHalconFlow(mHaGUI[CameraIndex]);
            my2DhalconFlow[CameraIndex].LoadAllParamFromXML(5);
            IsLoadParamters[CameraIndex] = true;
        }

        public void ChangeTestParameters(int CameraIndex, int ColorSelect)
        {
            IsLoadParamters[CameraIndex] = false;
            mHaGUI[CameraIndex].MyToolsController.VisionToolList.Clear();
            mHaGUI[CameraIndex].MyToolsController.AddVisionTool(my2DhalconFlow[CameraIndex].MyAllParam[ColorSelect]);
            IsLoadParamters[CameraIndex] = true;
        }

        public void SaveTestParameters(string PCBName, int CameraIndex, int ColorSelect)
        {
            my2DhalconFlow[CameraIndex] = new HalconModelAndFlow(PCBName, CameraIndex, ColorSelect);
            my2DhalconFlow[CameraIndex].SaveMyHalconFlow(mHaGUI[CameraIndex]);
            my2DhalconFlow[CameraIndex].LoadHalconFlow(mHaGUI[CameraIndex]);
        }

        public void SaveTestParameters(int CameraIndex, int ColorSelect)
        {
            string Item = my2DhalconFlow[CameraIndex].Item;
            my2DhalconFlow[CameraIndex] = new HalconModelAndFlow(Item, CameraIndex, ColorSelect);
            my2DhalconFlow[CameraIndex].SaveMyHalconFlow(mHaGUI[CameraIndex]);
            my2DhalconFlow[CameraIndex].LoadHalconFlow(mHaGUI[CameraIndex]);
        }

        public void LoadTestParameters(int CameraIndex)
        {
            int ColorSelect = my2DhalconFlow[CameraIndex].SaveParamIndex;
            string Item = my2DhalconFlow[CameraIndex].Item;
            my2DhalconFlow[CameraIndex] = new HalconModelAndFlow(Item, CameraIndex, ColorSelect);
            my2DhalconFlow[CameraIndex].LoadHalconFlow(mHaGUI[CameraIndex]);
        }

        public void SaveTestParameters(int CameraIndex)
        {
            int ColorSelect = my2DhalconFlow[CameraIndex].SaveParamIndex;
            string Item = my2DhalconFlow[CameraIndex].Item;
            my2DhalconFlow[CameraIndex] = new HalconModelAndFlow(Item, CameraIndex, ColorSelect);
            my2DhalconFlow[CameraIndex].SaveMyHalconFlow(mHaGUI[CameraIndex]);
            my2DhalconFlow[CameraIndex].LoadHalconFlow(mHaGUI[CameraIndex]);
        }

        string ConvertToLogMsgFormat(string Key, string Msg)
        {
            Msg = string.Format("[" + DateTime.Now.ToString("HHmmss:ffff") + "] {0}:{1}", Key, Msg);
            return Msg;
        }

        public bool TriggerTest(string[] GivenPcbCode, int GivenDutIndex, int GivenDotIndex, int ColorSet, string Path, bool IsDelete)
        {
            if (IsCamBusing.Contains(true) || IsLoadParamters.Contains(false))
            {
                if (IsCamBusing.Contains(true)) mLog.SaveLog(ConvertToLogMsgFormat("Operation", "相机程序已经启动等待结束后再开启！"), logpath);
                if (IsCamBusing.Contains(true)) mLog.SaveLog(ConvertToLogMsgFormat("Operation", "相机程序参数未加载！"), logpath);
                return false;
            }


            CodeStr = GivenPcbCode;
            DutIndex = GivenDutIndex;
            DotIndex = GivenDotIndex;
            ChangeParam = false;
            Trigger = Path + DateTime.Now.ToString("yyyyMMdd") + "\\";
            if (ColorSelect != ColorSet)
            {
                ColorSelect = ColorSet;
                ChangeParam = true;
            }
            mLog.SaveLog(ConvertToLogMsgFormat("Operation", "启动分析程序"), logpath);
            CameraRunErrorMsg = "";

            if (!IsInitCamGUI)
            {
                CameraRunErrorMsg += "资源未初始化，无法启动分析程序！";
                mLog.SaveLog(ConvertToLogMsgFormat("error", CameraRunErrorMsg), logpath);
                return false;
            }

            for (int i = 0; i < 4; i++)
            {
                if (!IsInitZYCam[i])
                {
                    CameraRunErrorMsg += string.Format("相机{0}资源未初始化，无法启动分析！", (i + 1).ToString());
                    mLog.SaveLog(ConvertToLogMsgFormat("error", CameraRunErrorMsg), logpath);
                    IsCamRunFinished[i] = true;
                    IsCamBusing[i] = false;
                    return false;
                }
                else
                {
                    IsCamRunFinished[i] = false;
                    IsCamBusing[i] = true;
                    OutTimeStep[i] = 0;
                }
            }

            string logcampath0 = logpath + "Cam0\\";
            try
            {
                if (ChangeParam)
                {
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", "0号相机 切换相机配置"), logcampath0);
                    ChangeTestParameters(0, ColorSelect);
                }
                mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("0号相机对 Code:{0},Dut:{1},Dot:{2} 启动分析", CodeStr[0], DutIndex, DotIndex)), logcampath0);

                if (HaGUILoadImage(0, Trigger, logcampath0))
                {
                    IsCamBusing[0] = false;
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("0号相机 存图为{0}", _ImageSavePath[0])), logcampath0);

                    my2DhalconFlow[0].RunHalconFlow(mHaGUI[0].MyImage, mHaGUI[0]);
                    IsToolRunOK[0] = ((IVisionTool)mHaGUI[0].MyToolsController.VisionToolList[0]).ToolResult;
                    IsRunPass[0] = ((IVisionTool)mHaGUI[0].MyToolsController.VisionToolList[0]).RunResult;
                    IsCamRunFinished[0] = true;
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", "0号相机 分析完成"), logcampath0);

                }
                else
                {
                    IsCamBusing[0] = false;
                    IsCamRunFinished[0] = true;
                    mLog.SaveLog(ConvertToLogMsgFormat("Error", "0号相机加载图片失败"), logcampath0);
                }

                if (!IsDelete) File.Delete(_ImageSavePath[0]);
            }
            catch (Exception e)
            {
                IsCamBusing[0] = false;
                IsCamRunFinished[0] = true;
                mLog.SaveLog(ConvertToLogMsgFormat("Error", "0号相机报警:" + e.ToString()), logcampath0);
            }

            string logcampath1 = logpath + "Cam1\\";
            try
            {
                if (ChangeParam)
                {
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", "1号相机 切换相机配置"), logcampath1);
                    ChangeTestParameters(1, ColorSelect);
                }
                mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("1号相机对 Code:{0},Dut:{1},Dot:{2} 启动分析", CodeStr[1], DutIndex, DotIndex)), logcampath1);
                if (HaGUILoadImage(1, Trigger, logcampath1))
                {
                    IsCamBusing[1] = false;
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("1号相机 存图为{0}", _ImageSavePath[1])), logcampath1);
                    my2DhalconFlow[1].RunHalconFlow(mHaGUI[1].MyImage, mHaGUI[1]);
                    IsToolRunOK[1] = ((IVisionTool)mHaGUI[1].MyToolsController.VisionToolList[0]).ToolResult;
                    IsRunPass[1] = ((IVisionTool)mHaGUI[1].MyToolsController.VisionToolList[0]).RunResult;
                    IsCamRunFinished[1] = true;
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", "1号相机 分析完成"), logcampath1);
                }
                else
                {
                    IsCamBusing[1] = false;
                    IsCamRunFinished[1] = true;
                    mLog.SaveLog(ConvertToLogMsgFormat("Error", "1号相机加载图片失败"), logcampath1);
                }
                if (!IsDelete) File.Delete(_ImageSavePath[1]);
            }
            catch (Exception e)
            {
                IsCamBusing[1] = false;
                IsCamRunFinished[1] = true;
                mLog.SaveLog(ConvertToLogMsgFormat("Error", "1号相机报警:" + e.ToString()), logcampath1);
            }

            string logcampath2 = logpath + "Cam2\\";
            try
            {
                if (ChangeParam)
                {
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", "2号相机 切换相机配置"), logcampath2);
                    ChangeTestParameters(2, ColorSelect);
                }
                mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("2号相机对 Code:{0},Dut:{1},Dot:{2} 启动分析", CodeStr[2], DutIndex, DotIndex)), logcampath2);
                if (HaGUILoadImage(2, Trigger, logcampath2))
                {
                    IsCamBusing[2] = false;
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("2号相机 存图为{0}", _ImageSavePath[2])), logcampath2);
                    my2DhalconFlow[2].RunHalconFlow(mHaGUI[2].MyImage, mHaGUI[2]);
                    IsToolRunOK[2] = ((IVisionTool)mHaGUI[2].MyToolsController.VisionToolList[0]).ToolResult;
                    IsRunPass[2] = ((IVisionTool)mHaGUI[2].MyToolsController.VisionToolList[0]).RunResult;
                    IsCamRunFinished[2] = true;
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", "2号相机 分析完成"), logcampath2);
                }
                else
                {
                    IsCamBusing[2] = false;
                    IsCamRunFinished[2] = true;
                    mLog.SaveLog(ConvertToLogMsgFormat("Error", "2号相机加载图片失败"), logcampath2);
                }
                if (!IsDelete) File.Delete(_ImageSavePath[2]);
            }
            catch (Exception e)
            {
                IsCamBusing[2] = false;
                IsCamRunFinished[2] = true;
                mLog.SaveLog(ConvertToLogMsgFormat("Error", "2号相机报警:" + e.ToString()), logcampath2);
            }

            string logcampath3 = logpath + "Cam3\\";
            try
            {
                if (ChangeParam)
                {
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", "3号相机 切换相机配置"), logcampath3);
                    ChangeTestParameters(3, ColorSelect);
                }
                mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("3号相机对 Code:{0},Dut:{1},Dot:{2} 启动分析", CodeStr[3], DutIndex, DotIndex)), logcampath3);
                if (HaGUILoadImage(3, Trigger, logcampath3))
                {
                    IsCamBusing[3] = false;
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("3号相机 存图为{0}", _ImageSavePath[3])), logcampath3);
                    my2DhalconFlow[3].RunHalconFlow(mHaGUI[3].MyImage, mHaGUI[3]);
                    IsToolRunOK[3] = ((IVisionTool)mHaGUI[3].MyToolsController.VisionToolList[0]).ToolResult;
                    IsRunPass[3] = ((IVisionTool)mHaGUI[3].MyToolsController.VisionToolList[0]).RunResult;
                    IsCamRunFinished[3] = true;
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", "3号相机 分析完成"), logcampath3);
                }
                else
                {
                    IsCamBusing[3] = false;
                    IsCamRunFinished[3] = true;
                    mLog.SaveLog(ConvertToLogMsgFormat("Error", "3号相机加载图片失败"), logcampath3);
                }
                if (!IsDelete) File.Delete(_ImageSavePath[3]);
            }
            catch (Exception e)
            {
                IsCamBusing[3] = false;
                IsCamRunFinished[3] = true;
                mLog.SaveLog(ConvertToLogMsgFormat("Error", "3号相机报警:" + e.ToString()), logcampath3);
            }


            return true;
        }

        public void CheckStoreFreeSpace()
        {
            DirectoryInfo theFolder = new DirectoryInfo("E:\\Image");
            long freeSpace = new long();
            string HardDiskName = "E:\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == HardDiskName)
                {
                    freeSpace = drive.TotalFreeSpace / (1024 * 1024);
                }
            }
            freeSpace = freeSpace / 1024;
            if (freeSpace < 6)
            {
                DirectoryInfo[] dirInfo = theFolder.GetDirectories();

                //遍历文件夹

                foreach (DirectoryInfo NextFolder in dirInfo)
                {
                    string CreatTime = NextFolder.CreationTime.ToString("yyyyMMdd");
                    if (CreatTime != DateTime.Today.ToString("yyyyMMdd")) NextFolder.Delete(true);
                }
            }
        }

        void CameraRunFunc0()
        {
            while (true)
            {
                if (StartRunning[0])
                {
                    StartRunning[0] = false;
                    IsCamRunFinished[0] = false;

                    string logcampath0 = logpath + "Cam0\\";
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", "启动分析线程0"), logcampath0);
                    try
                    {
                        if (ChangeParam)
                        {
                            mLog.SaveLog(ConvertToLogMsgFormat("Operation", "0号相机 切换相机配置"), logcampath0);
                            LoadTestParameters(0, ColorSelect);
                        }
                        mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("0号相机对 Code:{0},Dut:{1},Dot:{2} 启动分析", CodeStr[0], DutIndex, DotIndex)), logcampath0);

                        if (HaGUILoadImage(0, Trigger, logcampath0))
                        {
                            IsCamBusing[0] = false;
                            mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("0号相机 存图为{0}", _ImageSavePath[0])), logcampath0);

                            my2DhalconFlow[0].RunHalconFlow(mHaGUI[0].MyImage, mHaGUI[0]);
                            IsToolRunOK[0] = ((IVisionTool)mHaGUI[0].MyToolsController.VisionToolList[0]).ToolResult;
                            IsRunPass[0] = ((IVisionTool)mHaGUI[0].MyToolsController.VisionToolList[0]).RunResult;
                            IsCamRunFinished[0] = true;
                            mLog.SaveLog(ConvertToLogMsgFormat("Operation", "0号相机 分析完成"), logcampath0);
                        }
                        else
                        {
                            IsCamBusing[0] = false;
                            IsCamRunFinished[0] = true;
                            mLog.SaveLog(ConvertToLogMsgFormat("Error", "0号相机加载图片失败"), logcampath0);
                        }
                    }
                    catch (Exception e)
                    {
                        IsCamBusing[0] = false;
                        IsCamRunFinished[0] = true;
                        mLog.SaveLog(ConvertToLogMsgFormat("Error", "0号相机报警:" + e.ToString()), logcampath0);
                    }
                }
                if (StartRunning[1])
                {
                    StartRunning[1] = false;
                    IsCamRunFinished[1] = false;


                    string logcampath1 = logpath + "Cam1\\";
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", "启动分析线程1"), logcampath1);
                    try
                    {
                        if (ChangeParam)
                        {
                            mLog.SaveLog(ConvertToLogMsgFormat("Operation", "1号相机 切换相机配置"), logcampath1);
                            LoadTestParameters(1, ColorSelect);
                        }
                        mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("1号相机对 Code:{0},Dut:{1},Dot:{2} 启动分析", CodeStr[1], DutIndex, DotIndex)), logcampath1);
                        if (HaGUILoadImage(1, Trigger, logcampath1))
                        {
                            IsCamBusing[1] = false;
                            mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("1号相机 存图为{0}", _ImageSavePath[1])), logcampath1);
                            my2DhalconFlow[1].RunHalconFlow(mHaGUI[1].MyImage, mHaGUI[1]);
                            IsToolRunOK[1] = ((IVisionTool)mHaGUI[1].MyToolsController.VisionToolList[0]).ToolResult;
                            IsRunPass[1] = ((IVisionTool)mHaGUI[1].MyToolsController.VisionToolList[0]).RunResult;
                            IsCamRunFinished[1] = true;
                            mLog.SaveLog(ConvertToLogMsgFormat("Operation", "1号相机 分析完成"), logcampath1);
                        }
                        else
                        {
                            IsCamBusing[1] = false;
                            IsCamRunFinished[1] = true;
                            mLog.SaveLog(ConvertToLogMsgFormat("Error", "1号相机加载图片失败"), logcampath1);
                        }
                    }
                    catch (Exception e)
                    {
                        IsCamBusing[1] = false;
                        IsCamRunFinished[1] = true;
                        mLog.SaveLog(ConvertToLogMsgFormat("Error", "1号相机报警:" + e.ToString()), logcampath1);
                    }
                }
                if (StartRunning[2])
                {
                    StartRunning[2] = false;
                    IsCamRunFinished[2] = false;


                    string logcampath2 = logpath + "Cam2\\";
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", "启动分析线程2"), logcampath2);
                    try
                    {
                        if (ChangeParam)
                        {
                            mLog.SaveLog(ConvertToLogMsgFormat("Operation", "2号相机 切换相机配置"), logcampath2);
                            LoadTestParameters(2, ColorSelect);
                        }
                        mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("2号相机对 Code:{0},Dut:{1},Dot:{2} 启动分析", CodeStr[2], DutIndex, DotIndex)), logcampath2);
                        if (HaGUILoadImage(2, Trigger, logcampath2))
                        {
                            IsCamBusing[2] = false;
                            mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("2号相机 存图为{0}", _ImageSavePath[2])), logcampath2);
                            my2DhalconFlow[2].RunHalconFlow(mHaGUI[2].MyImage, mHaGUI[2]);
                            IsToolRunOK[2] = ((IVisionTool)mHaGUI[2].MyToolsController.VisionToolList[0]).ToolResult;
                            IsRunPass[2] = ((IVisionTool)mHaGUI[2].MyToolsController.VisionToolList[0]).RunResult;
                            IsCamRunFinished[2] = true;
                            mLog.SaveLog(ConvertToLogMsgFormat("Operation", "2号相机 分析完成"), logcampath2);
                        }
                        else
                        {
                            IsCamBusing[2] = false;
                            IsCamRunFinished[2] = true;
                            mLog.SaveLog(ConvertToLogMsgFormat("Error", "2号相机加载图片失败"), logcampath2);
                        }
                    }
                    catch (Exception e)
                    {
                        IsCamBusing[2] = false;
                        IsCamRunFinished[2] = true;
                        mLog.SaveLog(ConvertToLogMsgFormat("Error", "2号相机报警:" + e.ToString()), logcampath2);
                    }
                }
                if (StartRunning[3])
                {
                    StartRunning[3] = false;
                    IsCamRunFinished[3] = false;


                    string logcampath3 = logpath + "Cam3\\";
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", "启动分析线程3"), logcampath3);
                    try
                    {
                        if (ChangeParam)
                        {
                            mLog.SaveLog(ConvertToLogMsgFormat("Operation", "3号相机 切换相机配置"), logcampath3);
                            LoadTestParameters(3, ColorSelect);
                        }
                        mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("3号相机对 Code:{0},Dut:{1},Dot:{2} 启动分析", CodeStr[3], DutIndex, DotIndex)), logcampath3);
                        if (HaGUILoadImage(3, Trigger, logcampath3))
                        {
                            IsCamBusing[3] = false;
                            mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("3号相机 存图为{0}", _ImageSavePath[3])), logcampath3);
                            my2DhalconFlow[3].RunHalconFlow(mHaGUI[3].MyImage, mHaGUI[3]);
                            IsToolRunOK[3] = ((IVisionTool)mHaGUI[3].MyToolsController.VisionToolList[0]).ToolResult;
                            IsRunPass[3] = ((IVisionTool)mHaGUI[3].MyToolsController.VisionToolList[0]).RunResult;
                            IsCamRunFinished[3] = true;
                            mLog.SaveLog(ConvertToLogMsgFormat("Operation", "3号相机 分析完成"), logcampath3);
                        }
                        else
                        {
                            IsCamBusing[3] = false;
                            IsCamRunFinished[3] = true;
                            mLog.SaveLog(ConvertToLogMsgFormat("Error", "3号相机加载图片失败"), logcampath3);
                        }
                    }
                    catch (Exception e)
                    {
                        IsCamBusing[3] = false;
                        IsCamRunFinished[3] = true;
                        mLog.SaveLog(ConvertToLogMsgFormat("Error", "3号相机报警:" + e.ToString()), logcampath3);
                    }
                }
                Thread.Sleep(100);
            }
        }

        void CameraRunFunc1()
        {
            while (true)
            {
                if (StartRunning[1])
                {
                    StartRunning[1] = false;
                    IsCamRunFinished[1] = false;
                    IsCamBusing[1] = true;

                    string logcampath1 = logpath + "Cam1\\";
                    mLog.SaveLog(ConvertToLogMsgFormat("Operation", "启动分析线程1"), logcampath1);
                    try
                    {
                        if (ChangeParam)
                        {
                            mLog.SaveLog(ConvertToLogMsgFormat("Operation", "1号相机 切换相机配置"), logcampath1);
                            LoadTestParameters(1, ColorSelect);
                        }
                        mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("1号相机对 Code:{0},Dut:{1},Dot:{2} 启动分析", CodeStr[1], DutIndex, DotIndex)), logcampath1);
                        if (HaGUILoadImage(1, Trigger, logcampath1))
                        {
                            IsCamBusing[1] = false;
                            mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("1号相机 存图为{0}", _ImageSavePath[1])), logcampath1);
                            my2DhalconFlow[1].RunHalconFlow(mHaGUI[1].MyImage, mHaGUI[1]);
                            IsToolRunOK[1] = ((IVisionTool)mHaGUI[1].MyToolsController.VisionToolList[0]).ToolResult;
                            IsRunPass[1] = ((IVisionTool)mHaGUI[1].MyToolsController.VisionToolList[0]).RunResult;
                            IsCamRunFinished[1] = true;
                            mLog.SaveLog(ConvertToLogMsgFormat("Operation", "1号相机 分析完成"), logcampath1);
                        }
                        else
                        {
                            IsCamBusing[1] = false;
                            IsCamRunFinished[1] = true;
                            mLog.SaveLog(ConvertToLogMsgFormat("Error", "1号相机加载图片失败"), logcampath1);
                        }
                    }
                    catch (Exception e)
                    {
                        IsCamBusing[1] = false;
                        IsCamRunFinished[1] = true;
                        mLog.SaveLog(ConvertToLogMsgFormat("Error", "1号相机报警:" + e.ToString()), logcampath1);
                    }
                }
                Thread.Sleep(300);
            }
        }

        void CameraRunFunc2()
        {
            while (true)
            {

                Thread.Sleep(300);
            }
        }

        void CameraRunFunc3()
        {
            while (true)
            {
                for (int i = 0; i < 4; i++)
                {
                    switch (OutTimeStep[i])
                    {
                        case 0://判断相机启动了
                            if (IsCamBusing[i])
                            {
                                StartTime[i] = DateTime.Now;
                                EndTime[i] = DateTime.Now;
                                OutTimeStep[i] = 10;
                            }
                            else
                            {
                                OutTimeStep[i] = 999;
                            }
                            break;
                        case 10://判断在分析中或运行中
                            EndTime[i] = DateTime.Now;
                            if (IsCamBusing[i])
                            {
                                if ((EndTime[i] - StartTime[i]).TotalSeconds > 5)
                                {
                                    IsCamRunFinished[i] = true;
                                    IsCamBusing[i] = false;
                                    CameraRunErrorMsg += String.Format("{0}号相机取图超时！", (i).ToString());
                                    mLog.SaveLog(ConvertToLogMsgFormat("error", CameraRunErrorMsg), logpath);
                                    OutTimeStep[i] = 999;
                                    IsCamOnline[i] = false;
                                }
                            }
                            else
                            {
                                StartTime[i] = DateTime.Now;
                                EndTime[i] = DateTime.Now;
                                OutTimeStep[i] = 20;
                            }
                            break;
                        case 20:
                            EndTime[i] = DateTime.Now;
                            if (!IsCamRunFinished[i])
                            {
                                if ((EndTime[i] - StartTime[i]).TotalSeconds > 5)
                                {
                                    IsCamRunFinished[i] = true;
                                    IsCamBusing[i] = false;
                                    CameraRunErrorMsg += String.Format("{0}号相机分析超时！", (i).ToString());
                                    mLog.SaveLog(ConvertToLogMsgFormat("error", CameraRunErrorMsg), logpath);
                                    OutTimeStep[i] = 999;
                                }
                            }
                            break;
                        case 999:
                            //等待启动
                            break;

                    }
                }
                Thread.Sleep(1000);
            }
        }

        private void RunFiles()
        {
            StopRunFiles = false;

            List<string> FileName = new List<string>();

            FolderBrowserDialog filesDialog = new FolderBrowserDialog();

            if (filesDialog.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo theFolder = new DirectoryInfo(filesDialog.SelectedPath);

                FileInfo[] FirstfileInfo = theFolder.GetFiles();
                foreach (FileInfo FirstNextFile in FirstfileInfo) //遍历文件
                {
                    if (FirstNextFile.Name.ToLower().EndsWith("bmp") || FirstNextFile.Name.ToLower().EndsWith("jpg") || FirstNextFile.Name.ToLower().EndsWith("png"))
                    {
                        FileName.Add(FirstNextFile.FullName);
                    }
                }

                DirectoryInfo[] dirInfo = theFolder.GetDirectories();

                //遍历文件夹

                foreach (DirectoryInfo NextFolder in dirInfo)
                {
                    FileInfo[] fileInfo = NextFolder.GetFiles();

                    foreach (FileInfo NextFile in fileInfo) //遍历文件

                    {
                        if (NextFile.Name.ToLower().EndsWith("bmp") || NextFile.Name.ToLower().EndsWith("jpg") || NextFile.Name.ToLower().EndsWith("png"))
                        {
                            FileName.Add(NextFile.FullName);
                        }
                    }

                }

                Task FilesTest = new Task(() =>
                  {
                      int Count = 0;
                      while (Count < FileName.Count)
                      {
                          if (StopRunFiles) break;
                          mHaGUI[Cam2DUsingSelectIndex].OpenImage(FileName[Count]);
                          try
                          {
                              my2DhalconFlow[Cam2DUsingSelectIndex].RunHalconFlow(mHaGUI[Cam2DUsingSelectIndex].MyImage, mHaGUI[Cam2DUsingSelectIndex]);

                              if (!((IVisionTool)mHaGUI[Cam2DUsingSelectIndex].MyToolsController.VisionToolList[0]).ToolResult) File.AppendAllText(theFolder + "MissFileList.txt", FileName[Count] + "\t");
                          }
                          catch(Exception ex)
                          {
                              File.AppendAllText(theFolder + "MissFileList.txt", FileName[Count] + "\t");
                          }
                          Count++;

                      }
                  });
                FilesTest.Start();
            }
        }

        private void RunFilesForYUXIN()
        {
            StopRunFiles = false;

            List<string> FileName = new List<string>();
            List<string> BojinseFileName = new List<string>();

            FolderBrowserDialog filesDialog = new FolderBrowserDialog();

            if (filesDialog.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo theFolder = new DirectoryInfo(filesDialog.SelectedPath);

                DirectoryInfo[] dirInfo = theFolder.GetDirectories();

                string JinSe, BoJinSe;

                DirectoryInfo JinSeDirectory, BoJinSeDirectory;

                FileInfo[] JinsefileInfo, BoJinsefileInfo;

                //遍历文件夹

                foreach (DirectoryInfo NextFolder in dirInfo)
                {

                    JinSe = NextFolder.FullName + "\\NG\\黄金色\\";
                    BoJinSe = NextFolder.FullName + "\\NG\\铂金色\\";
                    try
                    {
                        JinSeDirectory = new DirectoryInfo(JinSe);
                        JinsefileInfo = JinSeDirectory.GetFiles();
                        foreach (FileInfo FirstNextFile in JinsefileInfo) //遍历文件
                        {
                            if (FirstNextFile.Name.ToLower().EndsWith("bmp") || FirstNextFile.Name.ToLower().EndsWith("jpg") || FirstNextFile.Name.ToLower().EndsWith("png"))
                            {
                                FileName.Add(FirstNextFile.FullName);
                            }
                        }
                    }
                    catch
                    {
                        File.AppendAllText(theFolder + "MissDirectoryList.txt", JinSe + "\n");
                    }
                    try
                    {
                        BoJinSeDirectory = new DirectoryInfo(BoJinSe);

                        BoJinsefileInfo = BoJinSeDirectory.GetFiles();

                        foreach (FileInfo SencondNextFile in BoJinsefileInfo) //遍历文件
                        {
                            if (SencondNextFile.Name.ToLower().EndsWith("bmp") || SencondNextFile.Name.ToLower().EndsWith("jpg") || SencondNextFile.Name.ToLower().EndsWith("png"))
                            {
                                BojinseFileName.Add(SencondNextFile.FullName);
                            }
                        }
                    }
                    catch
                    {
                        File.AppendAllText(theFolder + "MissDirectoryList.txt", BoJinSe + "\n");
                    }

                }

                Task FilesTest = new Task(() =>
                {
                    int Count = 0;
                    ChangeTestParameters(Cam2DUsingSelectIndex, 0);
                    while (Count < FileName.Count)
                    {
                        if (StopRunFiles) break;
                        mHaGUI[Cam2DUsingSelectIndex].OpenImage(FileName[Count]);
                        try
                        {
                            my2DhalconFlow[Cam2DUsingSelectIndex].RunHalconFlow(mHaGUI[Cam2DUsingSelectIndex].MyImage, mHaGUI[Cam2DUsingSelectIndex]);

                            if (!((IVisionTool)mHaGUI[Cam2DUsingSelectIndex].MyToolsController.VisionToolList[0]).ToolResult) File.AppendAllText(theFolder + "MissFileList.txt", FileName[Count] + "\n");
                        }
                        catch
                        {
                            File.AppendAllText(theFolder + "MissFileList.txt", FileName[Count] + "\n");
                        }
                        Count++;

                    }
                    ChangeTestParameters(Cam2DUsingSelectIndex, 1);
                    int BoCount = 0;
                    while (BoCount < BojinseFileName.Count)
                    {
                        if (StopRunFiles) break;
                        mHaGUI[Cam2DUsingSelectIndex].OpenImage(BojinseFileName[BoCount]);
                        try
                        {
                            my2DhalconFlow[Cam2DUsingSelectIndex].RunHalconFlow(mHaGUI[Cam2DUsingSelectIndex].MyImage, mHaGUI[Cam2DUsingSelectIndex]);

                            if (!((IVisionTool)mHaGUI[Cam2DUsingSelectIndex].MyToolsController.VisionToolList[0]).ToolResult) File.AppendAllText(theFolder + "MissFileList.txt", BojinseFileName[BoCount] + "\n");
                        }
                        catch
                        {
                            File.AppendAllText(theFolder + "MissFileList.txt", BojinseFileName[BoCount] + "\n");
                        }
                        BoCount++;

                    }
                });
                FilesTest.Start();

            }
        }

        public void SetMode(bool TestMode)
        {
            mTest2DMode = TestMode;

        }

        void SetParamSetControl(bool b)
        {

            trackBar1.Enabled = true;
            trackBar2.Enabled = b;
            trackBar3.Enabled = b;
            trackBar4.Enabled = true;
            trackBar5.Enabled = true;
            trackBar6.Enabled = b;
            trackBar7.Enabled = b;
            trackBar8.Enabled = b;
            trackBar9.Enabled = b;
            checkBox2.Enabled = b;
            checkBox3.Enabled = b;
            checkBox4.Enabled = b;
            checkBox5.Enabled = b;
            checkBox1.Enabled = b;

            //btn_Connect2DCamera.Enabled = false;
            btn_2DCameraGrab.Enabled = b;
            btn_2DCameraFrezz.Enabled = b;
            btn_2DCameraStop.Enabled = b;

        }

        void GroupDefultStatus()
        {
            SetMode(true);
            CheckBox_Edit.Checked = false;
            CheckBox_ROIMoveMode.Checked = false;
            comboBox_CamSelect.SelectedIndex = 0;

            tabControl1.TabPages.Remove(tabPage2);
            tabControl1.TabPages.Remove(tabPage3);

            SetParamSetControl(true);
        }

        public void HideEditUI(bool isHide)
        {
            splitContainer1.Panel2Collapsed = isHide;
        }

        public MainForm()
        {
            InitializeComponent();

            GroupDefultStatus();

            //RunningThread[0] = new Thread(CameraRunFunc0);
            //RunningThread[1] = new Thread(CameraRunFunc1);
            //RunningThread[2] = new Thread(CameraRunFunc2);
            RunningThread[3] = new Thread(CameraRunFunc3);
            //RunningThread[0].IsBackground = true;
            //RunningThread[1].IsBackground = true;
            //RunningThread[2].IsBackground = true;
            RunningThread[3].IsBackground = true;
            //RunningThread[0].Start();
            //RunningThread[1].Start();
            //RunningThread[2].Start();
            RunningThread[3].Start();
        }

        ~MainForm()
        {
            Dispose2DCamera();
        }

        private void MainForm_UI0_DoubleClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (mHaGUI[0].Parent == tableLayoutPanel1)
            {
                mHaGUI[0].Parent = Showpanel;
                mHaGUI[0].BringToFront();
            }
            else
            {
                tableLayoutPanel1.Controls.Add(mHaGUI[0], 0, 0);
                tableLayoutPanel1.BringToFront();
            }

        }

        private void MainForm_UI1_DoubleClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (mHaGUI[1].Parent == tableLayoutPanel1)
            {
                mHaGUI[1].Parent = Showpanel;
                mHaGUI[1].BringToFront();
            }
            else
            {
                tableLayoutPanel1.Controls.Add(mHaGUI[1], 1, 0);
                tableLayoutPanel1.BringToFront();
            }
        }

        private void MainForm_UI2_DoubleClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (mHaGUI[2].Parent == tableLayoutPanel1)
            {
                mHaGUI[2].Parent = Showpanel;
                mHaGUI[2].BringToFront();
            }
            else
            {
                tableLayoutPanel1.Controls.Add(mHaGUI[2], 0, 1);
                tableLayoutPanel1.BringToFront();
            }
        }

        private void MainForm_UI3_DoubleClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (mHaGUI[3].Parent == tableLayoutPanel1)
            {
                mHaGUI[3].Parent = Showpanel;
                mHaGUI[3].BringToFront();
            }
            else
            {
                tableLayoutPanel1.Controls.Add(mHaGUI[3], 1, 1);
                tableLayoutPanel1.BringToFront();
            }
        }

        private void MainForm_DoubleClick0(object sender, EventArgs e)
        {
            if (ViewWindows[0].Parent == ViewPanel)
            {
                ViewWindows[0].Parent = tabPage4;
                ViewWindows[0].BringToFront();
            }
            else
            {
                ViewPanel.Controls.Add(ViewWindows[0], 0, 0);
                ViewPanel.BringToFront();
            }
        }
        private void MainForm_DoubleClick1(object sender, EventArgs e)
        {
            if (ViewWindows[1].Parent == ViewPanel)
            {
                ViewWindows[1].Parent = tabPage4;
                ViewWindows[1].BringToFront();
            }
            else
            {
                ViewPanel.Controls.Add(ViewWindows[1], 1, 0);
                ViewPanel.BringToFront();
            }
        }
        private void MainForm_DoubleClick2(object sender, EventArgs e)
        {
            if (ViewWindows[2].Parent == ViewPanel)
            {
                ViewWindows[2].Parent = tabPage4;
                ViewWindows[2].BringToFront();
            }
            else
            {
                ViewPanel.Controls.Add(ViewWindows[2], 0, 1);
                ViewPanel.BringToFront();
            }
        }
        private void MainForm_DoubleClick3(object sender, EventArgs e)
        {
            if (ViewWindows[3].Parent == ViewPanel)
            {
                ViewWindows[3].Parent = tabPage4;
                ViewWindows[3].BringToFront();
            }
            else
            {
                ViewPanel.Controls.Add(ViewWindows[3], 1, 1);
                ViewPanel.BringToFront();
            }
        }

        private void CheckBox_Click(object sender, EventArgs e)
        {
            try
            {
                CheckBox ClickSender = sender as CheckBox;
                switch (ClickSender.Name)
                {
                    case "CheckBox_Edit":
                        if (!IsInitCamGUI) break;
                        mHaGUI[0].ShowMode(ClickSender.Checked);
                        mHaGUI[1].ShowMode(ClickSender.Checked);
                        mHaGUI[2].ShowMode(ClickSender.Checked);
                        mHaGUI[3].ShowMode(ClickSender.Checked);
                        if (!ClickSender.Checked)
                        {
                            tabControl1.TabPages.Remove(tabPage2);
                            //if (!mTest2DMode) tabControl1.TabPages.Remove(tabPage3);
                        }
                        else
                        {
                            tabControl1.TabPages.Add(tabPage2);
                            //if (!mTest2DMode) tabControl1.TabPages.Add(tabPage3);
                        }
                        break;
                    case "CheckBox_ROIMoveMode":
                        if (ClickSender.Checked)
                        {
                            mHaGUI[0].SetROIMoveLecel(1);
                            mHaGUI[1].SetROIMoveLecel(1);
                            mHaGUI[2].SetROIMoveLecel(1);
                            mHaGUI[3].SetROIMoveLecel(1);
                        }
                        else
                        {
                            mHaGUI[0].SetROIMoveLecel(2);
                            mHaGUI[1].SetROIMoveLecel(1);
                            mHaGUI[2].SetROIMoveLecel(1);
                            mHaGUI[3].SetROIMoveLecel(1);
                        }
                        break;
                    case "chb_EnableTriggerMode":

                        break;
                }
            }
            catch
            {

            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            Button ClickSender = sender as Button;
            try
            {
                if (ClickSender == null) return;
                switch (ClickSender.Name)
                {
                    case "btn_2DCameraParamSave":
                        if (Cam2DUsingSelectIndex == -1) break;
                        my2DhalconFlow[Cam2DUsingSelectIndex].SaveZYCameraConn(myCamParaClass[Cam2DUsingSelectIndex]);
                        break;
                    case "btn_LoadImage":
                        HaGUILoadLocalImage(Cam2DUsingSelectIndex, "Test");
                        break;
                    case "btn_Connect2DCamera":
                        InitPCB_AOI_CAMIndex();
                        break;
                    case "btn_2DCameraGrab":
                        StartGrab2DCameraIndex();
                        break;
                    case "btn_2DCameraFrezz":
                        StopGrab2DCameraIndex();
                        break;
                    case "btn_2DCameraStop":
                        Dispose2DCameraIndex();
                        break;
                    case "btn_2DCameraSave":
                        SaveCameraParamIndex();
                        break;
                    case "btn_SaveAllParam":
                        my2DhalconFlow[0].LoadAllParamFromXML(5);
                        my2DhalconFlow[1].LoadAllParamFromXML(5);
                        my2DhalconFlow[2].LoadAllParamFromXML(5);
                        my2DhalconFlow[3].LoadAllParamFromXML(5);
                        ParamBackUp(0);
                        ParamBackUp(1);
                        ParamBackUp(2);
                        ParamBackUp(3);
                        break;
                    case "btn_SaveOther2DtestFlow":
                        if (Cam2DUsingSelectIndex == -1) break;
                        SaveFileDialog mySaveDialog = new SaveFileDialog();
                        mySaveDialog.InitialDirectory = string.Format(Application.StartupPath + "\\INI\\{1}\\{0}\\", Cam2DUsingSelectIndex, PCBtestName);
                        DialogResult Re = mySaveDialog.ShowDialog();
                        if (Re == DialogResult.OK) my2DhalconFlow[Cam2DUsingSelectIndex].SaveMyHalconFlow(mySaveDialog.FileName, mHaGUI[Cam2DUsingSelectIndex]);
                        my2DhalconFlow[Cam2DUsingSelectIndex].LoadAllParamFromXML(5);
                        break;
                    case "btn_LoadOther2DtestFlow":
                        if (Cam2DUsingSelectIndex == -1) break;
                        OpenFileDialog myOpenFileDialog = new OpenFileDialog();
                        myOpenFileDialog.InitialDirectory = string.Format(Application.StartupPath + "\\INI\\{1}\\{0}\\", Cam2DUsingSelectIndex, PCBtestName);
                        DialogResult OpenRe = myOpenFileDialog.ShowDialog();
                        if (OpenRe == DialogResult.OK)
                        {
                            string ParamName = myOpenFileDialog.SafeFileName;
                            try
                            {
                                string[] NewSet = ParamName.Split('m');
                                int NewColorSet;
                                if (int.TryParse(NewSet[1].Split('.')[0], out NewColorSet) && NewSet[0] == "HalconPara")
                                {
                                    ColorSelect = NewColorSet;
                                    label_ColorSet.Text = ColorSelect.ToString();
                                    LoadTestParameters(Cam2DUsingSelectIndex, NewColorSet);
                                }
                            }
                            catch
                            {
                                MessageBox.Show("文件类型错误!");
                            }
                        }
                        break;
                    case "btn_2DCameraRunTest":
                        if (Cam2DUsingSelectIndex == -1) break;
                        my2DhalconFlow[Cam2DUsingSelectIndex].RunHalconFlow(mHaGUI[Cam2DUsingSelectIndex].MyImage, mHaGUI[Cam2DUsingSelectIndex]);
                        break;
                    case "btn_Load2DtestFlow":
                        if (Cam2DUsingSelectIndex == -1) break;
                        my2DhalconFlow[Cam2DUsingSelectIndex].LoadHalconFlow(mHaGUI[Cam2DUsingSelectIndex]);
                        break;
                    case "btn_Save2DtestFlow":
                        if (Cam2DUsingSelectIndex == -1) break;
                        {
                            string FileName = string.Format(Application.StartupPath + "\\INI\\{1}\\{0}\\HalconParam{2}.xml", Cam2DUsingSelectIndex, PCBtestName, ColorSelect);
                            my2DhalconFlow[Cam2DUsingSelectIndex].SaveMyHalconFlow(FileName, mHaGUI[Cam2DUsingSelectIndex]);
                        }
                        break;
                    case "btn_2DCameraRunFiles":
                        RunFiles();
                        break;
                    case "btn_Show4Param":
                        Halcon.VisionTool.VisionPadPanic[] ParamShow = new Halcon.VisionTool.VisionPadPanic[4]
                        { my2DhalconFlow[0].MyAllParam[ColorSelect], my2DhalconFlow[1].MyAllParam[ColorSelect], my2DhalconFlow[2].MyAllParam[ColorSelect], my2DhalconFlow[3].MyAllParam[ColorSelect] };
                        ParamContrast ShowForm = new ParamContrast();
                        ShowForm.LoadParam(ParamShow);
                        ShowForm.Show();
                        break;
                    case "btn_ShowVisionEdit":
                        mHaGUI[Cam2DUsingSelectIndex].ShowVisionEidtTool(0);
                        break;
                    case "btn_CameraStopRunFiles":
                        StopRunFiles = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                mLog.SaveLog(ConvertToLogMsgFormat("Error", ex.Message));
            }
        }
        void ParamBackUp(int Cam)
        {
            string OldPath = string.Format(Application.StartupPath + "\\INI\\{0}\\{1}\\", PCBtestName, Cam);
            string Path = string.Format(Application.StartupPath + "\\INI\\{0}_Backup{2}\\{1}\\", PCBtestName, Cam, DateTime.Now.ToString("yyyyMMdd"));
            if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
            DirectoryInfo BackUpInfo = new DirectoryInfo(OldPath);
            FileInfo[] BackUpFiles = BackUpInfo.GetFiles();
            for (int i = 0; i < BackUpFiles.Length; i++)
            {
                BackUpFiles[i].CopyTo(Path + BackUpFiles[i].Name, true);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                mTest2DMode = true;
                SetMode(mTest2DMode);
            }
            else
            {
                //if(tabControl1.SelectedIndex == 2) SetMode(mTest2DMode);
                mTest2DMode = false;
            }

        }

        private void comboBox_CamSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox_CamSelect.SelectedItem.ToString())
            {
                case "相机1":
                    Cam2DUsingSelectIndex = 0;
                    if (IsInitZYCam[Cam2DUsingSelectIndex])
                    {
                        DllFunction.WF_GetAllParam(Cam2DUsingSelectIndex, ref myCamParaClass[Cam2DUsingSelectIndex].m_Param);
                        UpdateParam(myCamParaClass[Cam2DUsingSelectIndex]);
                    }
                    break;
                case "相机2":
                    Cam2DUsingSelectIndex = 1;
                    if (IsInitZYCam[Cam2DUsingSelectIndex])
                    {
                        DllFunction.WF_GetAllParam(Cam2DUsingSelectIndex, ref myCamParaClass[Cam2DUsingSelectIndex].m_Param);
                        UpdateParam(myCamParaClass[Cam2DUsingSelectIndex]);
                    }
                    break;
                case "相机3":
                    Cam2DUsingSelectIndex = 2;
                    if (IsInitZYCam[Cam2DUsingSelectIndex])
                    {
                        DllFunction.WF_GetAllParam(Cam2DUsingSelectIndex, ref myCamParaClass[Cam2DUsingSelectIndex].m_Param);
                        UpdateParam(myCamParaClass[Cam2DUsingSelectIndex]);
                    }
                    break;
                case "相机4":
                    Cam2DUsingSelectIndex = 3;
                    if (IsInitZYCam[Cam2DUsingSelectIndex])
                    {
                        DllFunction.WF_GetAllParam(Cam2DUsingSelectIndex, ref myCamParaClass[Cam2DUsingSelectIndex].m_Param);
                        UpdateParam(myCamParaClass[Cam2DUsingSelectIndex]);
                    }
                    break;
            }
        }

        void SaveImageOnLocal(int CameraIndex, string CodeStr)
        {
            if (!Directory.Exists(Application.StartupPath + "\\TempImage\\")) Directory.CreateDirectory(Application.StartupPath + "\\TempImage\\");
            DllFunction.WF_GetFrameFromCamera(CameraIndex, Application.StartupPath + "\\TempImage\\" + CodeStr + ".jpg");
        }

        void HaGUILoadLocalImage(int CameraIndex, string CodeStr)
        {
            SaveImageOnLocal(CameraIndex, CodeStr);
            mHaGUI[CameraIndex].OpenImage(Application.StartupPath + "\\TempImage\\" + CodeStr + ".jpg");
        }

        public bool SaveImage(int CameraIndex, string Path, string LogPath)
        {
            bool ReValue = true;
            GenralPath = Path + CodeStr[CameraIndex] + "\\" + DutIndex + "\\";
            _ImageSavePath[CameraIndex] = GenralPath + "CamStation" + CameraIndex + "_Pcs" + DutIndex + "_PAD" + DotIndex + DateTime.Now.ToString("_HHmmssff") + ".jpg";
            mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("{0}号相机图片路径为:{1}", CameraIndex, _ImageSavePath[CameraIndex])), LogPath);
            if (!Directory.Exists(GenralPath))
            {
                Directory.CreateDirectory(GenralPath);

            }
            int iRet = DllFunction.WF_GetFrameFromCamera(CameraIndex, _ImageSavePath[CameraIndex]);
            if (iRet != 0)
            {
                mLog.SaveLog(ConvertToLogMsgFormat("Operation", string.Format("{0}号相机取图片失败", CameraIndex)), LogPath);
                ReValue = false;
            }
            return ReValue;
        }

        public bool HaGUILoadImage(int CameraIndex, string Path, string LogPath)
        {
            if (!SaveImage(CameraIndex, Path, LogPath)) return false;
            mHaGUI[CameraIndex].OpenImage(_ImageSavePath[CameraIndex]);
            return true;
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            TrackBar ClickSender = sender as TrackBar;
            if (!IsReadParam) return;
            switch (ClickSender.Name)
            {
                case "trackBar1":
                    ExposureParam(trackBar1.Value);
                    break;
                case "trackBar2":
                    label17.Text = trackBar2.Value.ToString();
                    DllFunction.WF_SetBestAe(Cam2DUsingSelectIndex, (uint)trackBar2.Value);
                    break;
                case "trackBar3":
                    label18.Text = trackBar3.Value.ToString();
                    DllFunction.WF_SetGain_A(Cam2DUsingSelectIndex, (uint)trackBar3.Value);
                    break;
                case "trackBar4":
                    label20.Text = trackBar4.Value.ToString();
                    DllFunction.WF_SetGainR(Cam2DUsingSelectIndex, (uint)trackBar4.Value);
                    break;
                case "trackBar5":
                    label21.Text = trackBar5.Value.ToString();
                    DllFunction.WF_SetGainB(Cam2DUsingSelectIndex, (uint)trackBar5.Value);
                    break;
                case "trackBar6":
                    label22.Text = trackBar6.Value.ToString();
                    DllFunction.WF_SetSaturation(Cam2DUsingSelectIndex, trackBar6.Value);
                    break;
                case "trackBar7":
                    label23.Text = trackBar7.Value.ToString();
                    DllFunction.WF_SetContrast(Cam2DUsingSelectIndex, (uint)trackBar7.Value);
                    break;
                case "trackBar8":
                    label24.Text = trackBar8.Value.ToString();
                    DllFunction.WF_SetSharpness(Cam2DUsingSelectIndex, trackBar8.Value);
                    break;
                case "trackBar9":
                    label25.Text = trackBar9.Value.ToString();
                    DllFunction.WF_SetHDR(Cam2DUsingSelectIndex, trackBar9.Value);
                    break;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (IsReadParam) DllFunction.WF_SetDoAE(Cam2DUsingSelectIndex, (char)0, checkBox5.Checked, IntPtr.Zero, IntPtr.Zero);
        }

        void SaveCameraParamIndex()
        {
            StopGrab2DCamera();
            DllFunction.WF_UnInitRes();
            IsInitCamGUI = false;
            InitPCB_AOI_CAM();
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            comboBox_CamSelect.SelectedIndex = (int)numericUpDown.Value;
        }

        private void CheckBox_Edit_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
