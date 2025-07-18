using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;
using MvCamCtrl.NET;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Collections.ObjectModel;

namespace MasonteVision.Halcon
{
    public delegate void HalconGUIUpdateEventHandler(HObject Image);
    public delegate void RunCallBackEventHandler(HObject Image);
    public class HaCamConnect
    {
        private ChoseCamera mchose;
        private int cameraIndex = -1;
        private MyCamera m_pMyCamera = new MyCamera();
        bool m_bGrabbing;

        public HalconGUIUpdateEventHandler HalconGUIUpdate;
        public RunCallBackEventHandler HUpdateImage;
        public HaGUI MyHaGUI;
        public string BingdingSn;
        public MyCamera.MV_CC_DEVICE_INFO_LIST m_pDeviceList;

        /// <summary>
        /// USB接口设备信息
        /// </summary>
        public List<string> USBDevices = new List<string>();
        /// <summary>
        /// 网口接口设备信息
        /// </summary>
        public List<string> GigeDevices = new List<string>();

        void KillMvsLogServer()
        {
            Process[] MyProcess;
            MyProcess = Process.GetProcesses();
            for (int i = 0; i < MyProcess.Length; i++)
            {
                //if (MyProcess[i].ProcessName == "MvLogServer_20190108(32 位)")
                //{
                //    MyProcess[i].Kill();
                //}
                //if (MyProcess[i].ProcessName == "MVDLogServer")
                //{
                //    MyProcess[i].Kill();
                //}
            }
        }

        public HaCamConnect()
        {
            KillMvsLogServer();
            MyHaGUI = new HaGUI();
            HalconGUIUpdate = new HalconGUIUpdateEventHandler(MyHaGUI.Display);
            DeviceListAcq();
            MyHaGUI.SetCameraSn.Click += SetCameraSn_Click;
        }

        public HaCamConnect(HaGUI RunningGUI)
        {
            KillMvsLogServer();
            MyHaGUI = RunningGUI;
            HalconGUIUpdate = new HalconGUIUpdateEventHandler(MyHaGUI.Display);
            DeviceListAcq();
            MyHaGUI.SetCameraSn.Click += SetCameraSn_Click;
        }

        ~HaCamConnect()
        {
            CloseDevice();
        }

        public bool ConnectCameraBySN(string CameraSN)
        {
            cameraIndex = GetIndexBySN(CameraSN);
            return DeviceOpen();
        }

        public bool ConnectCameraByIndex(int Index)
        {
            cameraIndex = Index;
            return DeviceOpen();
        }

        public bool ConnectDefultCamera()
        {
            if (cameraIndex == -1) cameraIndex = GetIndexBySN(BingdingSn);
            return DeviceOpen();
        }

        private int GetIndexBySN(string SN)
        {
            int index = -1;
            for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
            {
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));

                    if (SN == gigeInfo.chSerialNumber)
                    {
                        index = i;
                    }

                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stUsb3VInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    if (SN == usbInfo.chSerialNumber)
                    {
                        index = i;
                    }
                }
            }
            return index;
        }

        private void SetCameraSn_Click(object sender, EventArgs e)
        {
            mchose = new ChoseCamera();
            for (int i = 0; i < GigeDevices.Count; i++)
            {
                //string CamSN = GigeDevices[i].Split('(')[1].Split(')')[0];
                mchose.comboBox1.Items.Add(GigeDevices[i]);
            }
            for (int i = 0; i < USBDevices.Count; i++)
            {
                mchose.comboBox1.Items.Add(USBDevices[i]);
            }
            mchose.Show();
            mchose.button1.Click += Button1_Click; ;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            string CamSN = mchose.comboBox1.SelectedItem.ToString().Split('(')[1].Split(')')[0];
            cameraIndex = GetIndexBySN(CamSN);
            mchose.Close();
        }

        void OnRunCallBack(HObject o)
        {
            if (HUpdateImage != null)
            {
                HUpdateImage(o);
            }
        }

        private void DeviceListAcq()
        {
            int nRet;
            // ch:创建设备列表 || en: Create device list
            System.GC.Collect();
            nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_pDeviceList);
            if (MyCamera.MV_OK != nRet)
            {
                MessageBox.Show("Enum Devices Fail");
                return;
            }

            for (int i = 0; i < m_pDeviceList.nDeviceNum; i++)
            {
                MyCamera.MV_CC_DEVICE_INFO device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));
                if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
                    MyCamera.MV_GIGE_DEVICE_INFO gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                    if (gigeInfo.chUserDefinedName != "")
                    {
                        GigeDevices.Add("GEV: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")");
                    }
                    else
                    {
                        GigeDevices.Add("GEV: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")");
                    }

                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stUsb3VInfo, 0);
                    MyCamera.MV_USB3_DEVICE_INFO usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    if (usbInfo.chUserDefinedName != "")
                    {
                        USBDevices.Add("U3V: " + usbInfo.chUserDefinedName + " (" + usbInfo.chSerialNumber + ")");
                    }
                    else
                    {
                        USBDevices.Add("U3V: " + usbInfo.chManufacturerName + " " + usbInfo.chModelName + " (" + usbInfo.chSerialNumber + ")");
                    }
                }
            }

        }

        private bool DeviceOpen()
        {
            if (m_pDeviceList.nDeviceNum == 0 || cameraIndex == -1)
            {
                MessageBox.Show("No device,please select");
                return false;
            }
            int nRet = -1;

            //ch:获取选择的设备信息 | en:Get selected device information
            MyCamera.MV_CC_DEVICE_INFO device =
                (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_pDeviceList.pDeviceInfo[cameraIndex],
                                                              typeof(MyCamera.MV_CC_DEVICE_INFO));

            nRet = m_pMyCamera.MV_CC_CreateDevice_NET(ref device);
            if (MyCamera.MV_OK != nRet)
            {
                return false;
            }

            // ch:打开设备 | en:Open device
            nRet = m_pMyCamera.MV_CC_OpenDevice_NET();
            if (MyCamera.MV_OK != nRet)
            {
                MessageBox.Show("Open Device Fail");
                return false;
            }

            // ch:设置触发模式为off || en:set trigger mode as off
            m_pMyCamera.MV_CC_SetEnumValue_NET("AcquisitionMode", 2);
            m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerMode", 0);

            return true;

        }

        /// <summary>
        /// 设置触发模式
        /// </summary>
        /// <param name="Mode">1开启触发模式；0关闭</param>
        /// <param name="TriggerSource"> 1 - Line1;2 - Line2; 3 - Line3;4 - Counter;7 - Software;</param>
        /// <returns></returns>
        public bool SetTriggerMode(int Mode, uint TriggerSource)
        {
            int nRet = MyCamera.MV_OK;
            if (Mode == 0)
            {
                nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerSource", 0);
            }
            else
            {
                nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerMode", 1);
            }
            if (nRet != MyCamera.MV_OK) return false;
            // ch: 触发源选择:0 - Line0 || en :TriggerMode select;
            //           1 - Line1;
            //           2 - Line2;
            //           3 - Line3;
            //           4 - Counter;
            //           7 - Software;
            if (Mode != 0)
            {
                nRet = m_pMyCamera.MV_CC_SetEnumValue_NET("TriggerSource", TriggerSource);
            }
            if (nRet != MyCamera.MV_OK) return false;
            else return true;
        }

        public void StartGrab()
        {
            // ch:开启抓图 | en:start grab
            int nRet = m_pMyCamera.MV_CC_StartGrabbing_NET();
            if (MyCamera.MV_OK != nRet)
            {
                MessageBox.Show("Start Grabbing Fail");
                return;
            }
            m_bGrabbing = true;

            Thread hReceiveImageThreadHandle = new Thread(ReceiveImageWorkThread);
            hReceiveImageThreadHandle.Start(m_pMyCamera);

        }

        public bool StopGrab()
        {
            // ch:停止抓图 || en:Stop grab image
            int nRet = m_pMyCamera.MV_CC_StopGrabbing_NET();
            m_bGrabbing = false;
            if (nRet != MyCamera.MV_OK) return false;
            else return true;

            // ch: 控件操作 || en: Control operation
        }

        public void CloseDevice()
        {
            if (m_bGrabbing)
            {
                m_bGrabbing = false;
                // ch:停止抓图 || en:Stop grab image
                m_pMyCamera.MV_CC_StopGrabbing_NET();

                // ch: 控件操作 || en: Control operation
            }

            // ch:关闭设备 || en: Close device
            m_pMyCamera.MV_CC_CloseDevice_NET();
            m_pMyCamera.MV_CC_DestroyDevice_NET();

            m_bGrabbing = false;
        }

        public void TriggerExec()
        {
            // ch: 触发命令 || en: Trigger command
            int nRet = m_pMyCamera.MV_CC_SetCommandValue_NET("TriggerSoftware");
            if (MyCamera.MV_OK != nRet)
            {
                MessageBox.Show("Trigger Fail");
            }
        }

        public bool SaveCameraProperties()
        {
            int nRet = m_pMyCamera.MV_CC_FeatureSave_NET("CameraFile");
            if (MyCamera.MV_OK != nRet) return false;
            else return true;
        }

        public bool LoadCameraProperties()
        {
            int nRet = m_pMyCamera.MV_CC_FeatureLoad_NET("CameraFile");
            if (MyCamera.MV_OK != nRet) return false;
            else return true;
        }

        public string GetParamValue(string strParamName)
        {
            MyCamera.MVCC_FLOATVALUE stParam = new MyCamera.MVCC_FLOATVALUE();
            int nRet = m_pMyCamera.MV_CC_GetFloatValue_NET(strParamName, ref stParam);
            return stParam.fCurValue.ToString("F1");

        }

        public bool SetParamValue(string strParamName, string strParamValue)
        {
            try
            {
                float.Parse(strParamValue);
            }
            catch
            {
                return false;
            }

            int nRet = m_pMyCamera.MV_CC_SetFloatValue_NET(strParamName, float.Parse(strParamValue));
            if (nRet != MyCamera.MV_OK) return false;
            else return true;

        }

        public void ReceiveImageWorkThread(object obj)
        {
            int nRet = MyCamera.MV_OK;
            MyCamera device = obj as MyCamera;
            MyCamera.MV_FRAME_OUT stFrameOut = new MyCamera.MV_FRAME_OUT();

            IntPtr pImageBuf = IntPtr.Zero;
            int nImageBufSize = 0;

            HObject Hobj = new HObject();
            IntPtr pTemp = IntPtr.Zero;

            while (m_bGrabbing)
            {
                nRet = device.MV_CC_GetImageBuffer_NET(ref stFrameOut, 1000);

                if (MyCamera.MV_OK == nRet)
                {
                    if (IsColorPixelFormat(stFrameOut.stFrameInfo.enPixelType))
                    {
                        if (stFrameOut.stFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed)
                        {
                            pTemp = stFrameOut.pBufAddr;
                        }
                        else
                        {
                            if (IntPtr.Zero == pImageBuf || nImageBufSize < (stFrameOut.stFrameInfo.nWidth * stFrameOut.stFrameInfo.nHeight * 3))
                            {
                                if (pImageBuf != IntPtr.Zero)
                                {
                                    Marshal.FreeHGlobal(pImageBuf);
                                    pImageBuf = IntPtr.Zero;
                                }

                                pImageBuf = Marshal.AllocHGlobal((int)stFrameOut.stFrameInfo.nWidth * stFrameOut.stFrameInfo.nHeight * 3);
                                if (IntPtr.Zero == pImageBuf)
                                {
                                    break;
                                }
                                nImageBufSize = stFrameOut.stFrameInfo.nWidth * stFrameOut.stFrameInfo.nHeight * 3;
                            }

                            MyCamera.MV_PIXEL_CONVERT_PARAM stPixelConvertParam = new MyCamera.MV_PIXEL_CONVERT_PARAM();

                            stPixelConvertParam.pSrcData = stFrameOut.pBufAddr;//源数据
                            stPixelConvertParam.nWidth = stFrameOut.stFrameInfo.nWidth;//图像宽度
                            stPixelConvertParam.nHeight = stFrameOut.stFrameInfo.nHeight;//图像高度
                            stPixelConvertParam.enSrcPixelType = stFrameOut.stFrameInfo.enPixelType;//源数据的格式
                            stPixelConvertParam.nSrcDataLen = stFrameOut.stFrameInfo.nFrameLen;

                            stPixelConvertParam.nDstBufferSize = (uint)nImageBufSize;
                            stPixelConvertParam.pDstBuffer = pImageBuf;//转换后的数据
                            stPixelConvertParam.enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed;
                            nRet = device.MV_CC_ConvertPixelType_NET(ref stPixelConvertParam);//格式转换
                            if (MyCamera.MV_OK != nRet)
                            {
                                break;
                            }
                            pTemp = pImageBuf;
                        }

                        try
                        {
                            HOperatorSet.GenImageInterleaved(out Hobj, (HTuple)pTemp, (HTuple)"rgb", (HTuple)stFrameOut.stFrameInfo.nWidth, (HTuple)stFrameOut.stFrameInfo.nHeight, -1, "byte", 0, 0, 0, 0, -1, 0);
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            break;
                        }
                    }
                    else if (IsMonoPixelFormat(stFrameOut.stFrameInfo.enPixelType))
                    {
                        if (stFrameOut.stFrameInfo.enPixelType == MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8)
                        {
                            pTemp = stFrameOut.pBufAddr;
                        }
                        else
                        {
                            if (IntPtr.Zero == pImageBuf || nImageBufSize < (stFrameOut.stFrameInfo.nWidth * stFrameOut.stFrameInfo.nHeight))
                            {
                                if (pImageBuf != IntPtr.Zero)
                                {
                                    Marshal.FreeHGlobal(pImageBuf);
                                    pImageBuf = IntPtr.Zero;
                                }

                                pImageBuf = Marshal.AllocHGlobal((int)stFrameOut.stFrameInfo.nWidth * stFrameOut.stFrameInfo.nHeight);
                                if (IntPtr.Zero == pImageBuf)
                                {
                                    break;
                                }
                                nImageBufSize = stFrameOut.stFrameInfo.nWidth * stFrameOut.stFrameInfo.nHeight;
                            }

                            MyCamera.MV_PIXEL_CONVERT_PARAM stPixelConvertParam = new MyCamera.MV_PIXEL_CONVERT_PARAM();

                            stPixelConvertParam.pSrcData = stFrameOut.pBufAddr;//源数据
                            stPixelConvertParam.nWidth = stFrameOut.stFrameInfo.nWidth;//图像宽度
                            stPixelConvertParam.nHeight = stFrameOut.stFrameInfo.nHeight;//图像高度
                            stPixelConvertParam.enSrcPixelType = stFrameOut.stFrameInfo.enPixelType;//源数据的格式
                            stPixelConvertParam.nSrcDataLen = stFrameOut.stFrameInfo.nFrameLen;

                            stPixelConvertParam.nDstBufferSize = (uint)nImageBufSize;
                            stPixelConvertParam.pDstBuffer = pImageBuf;//转换后的数据
                            stPixelConvertParam.enDstPixelType = MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8;
                            nRet = device.MV_CC_ConvertPixelType_NET(ref stPixelConvertParam);//格式转换
                            if (MyCamera.MV_OK != nRet)
                            {
                                break;
                            }
                            pTemp = pImageBuf;
                        }
                        try
                        {
                            HOperatorSet.GenImage1Extern(out Hobj, "byte", stFrameOut.stFrameInfo.nWidth, stFrameOut.stFrameInfo.nHeight, pTemp, IntPtr.Zero);
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            break;
                        }
                    }
                    else
                    {
                        device.MV_CC_FreeImageBuffer_NET(ref stFrameOut);
                        continue;
                    }
                    //HalconDisplay(m_Window, Hobj, stFrameOut.stFrameInfo.nHeight, stFrameOut.stFrameInfo.nWidth);
                    if (Hobj != null)
                    {
                        if (MyHaGUI != null) HalconGUIUpdate(Hobj);
                        OnRunCallBack(Hobj);
                    }
                    device.MV_CC_FreeImageBuffer_NET(ref stFrameOut);
                }
                else
                {
                    continue;
                }
                System.GC.Collect();
            }

            if (pImageBuf != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(pImageBuf);
                pImageBuf = IntPtr.Zero;
            }
            
        }

        private bool IsMonoPixelFormat(MyCamera.MvGvspPixelType enType)
        {
            switch (enType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsColorPixelFormat(MyCamera.MvGvspPixelType enType)
        {
            switch (enType)
            {
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_RGB8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BGR8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_RGBA8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BGRA8_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_YUV422_YUYV_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG8:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR10_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGB12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerBG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerRG12_Packed:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12:
                case MyCamera.MvGvspPixelType.PixelType_Gvsp_BayerGR12_Packed:
                    return true;
                default:
                    return false;
            }
        }

    }
}
