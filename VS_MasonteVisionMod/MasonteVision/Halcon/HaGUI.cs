using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MasonteVision.Halcon.VisionTool;
using HalconDotNet;
using System.Threading;
using System.IO;

namespace MasonteVision.Halcon
{
    [Serializable]
    public partial class HaGUI : UserControl
    {

        #region 定义

        public Dictionary<string, HObject> iMAGES = new Dictionary<string, HObject>();

        public List<string> CanContCamList = new List<string>();
        private HObject TempImage;
        public HObject MyImage; //图像
        public HWindow MyWindow; //显示窗口
        public HWndCtrl MyHWndControl; //可视化操作
        public ROIController MyROIController;
        public ToolsController MyToolsController;
        ToolSettingForm toolSettingForm;
        public ROICircle MyROICircle;
        public ROICircularArc MyROICircleArc;
        public ROIRectangle1 MyROIRectangle1;
        public ROIRectangle2 MyROIRectangle2;
        public ROILine MyROILine;
        public string GUIName;

        private bool _isEdit;
        public bool IsEdit
        {
            get { return _isEdit; }
            set { _isEdit = value; }
        }

        private bool _test2dMode = true;
        public bool Test2dMode
        {
            get { return _test2dMode; }
            set { _test2dMode = value; }
        }

        private bool _isROIAllownMove = false;
        public bool IsROIAllownMove
        {
            get { return _isROIAllownMove; }
            set { _isROIAllownMove = value; }
        }

        public string MySavePath;  //保存图像路径

        public string MyOpenPath; //打开图像路径

        #endregion

        public HaGUI()
        {
            InitializeComponent();

            MyImage = new HObject();
            MyHWndControl = new HWndCtrl(hWindowControl);
            MyROIController = new ROIController();
            MyHWndControl.useROIController(MyROIController);
            MyToolsController = new ToolsController();
            MyWindow = hWindowControl.HalconWindow;
            MySavePath = "";
            MyOpenPath = "";
            ShowMode(false);
            SetROIMoveLecel(2);
        }

        #region 设置图像

        //打开图像


        public void Display(HObject image)
        {
            try
            {
                UpdateImage(image);
            }
            catch
            {

            }

        }

        #endregion

        #region  图像窗口更新

        public bool InitHaGUIWindows(HObject Image)
        {
            try
            {
                if (Image == null || Image.CountObj() <= 0) return false;
                MyHWndControl.clearList();
                MyROIController.ROIList.Clear();
                MyHWndControl.addIconicVar(new HImage(Image));
                MyHWndControl.repaint();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void UpdateImage(HObject Image)      //显示图像
        {
            try
            {
                if (Image != null || Image.CountObj() <= 0)
                {
                    MyImage.Dispose();
                    MyHWndControl.clearList();
                    MyHWndControl.addIconicVar(new HImage(Image));
                    MyHWndControl.repaint();
                    MyImage = Image.Clone();
                }
            }
            catch
            {

            }

        }
        public void UpdateRegion(HObject HRegion)     //显示区域
        {
            MyHWndControl.addIconicVar(new HObject(HRegion));
            MyHWndControl.repaint();

        }



        #endregion

        #region         图像相关  

        public void OpenImage()
        {
            openFileDialog1.InitialDirectory = "E:\\";
            openFileDialog1.Filter = "图片|*.bmp;*.png;*.tif;*.tiff;*.gif;*.jpg;*.jpeg;*.jp2;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima|(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                MyOpenPath = openFileDialog1.FileName;
                HOperatorSet.GenEmptyObj(out MyImage);
                HOperatorSet.GenEmptyObj(out TempImage);

                HOperatorSet.ReadImage(out TempImage, MyOpenPath);
                MyImage = TempImage.Clone();
                TempImage.Dispose();
                MyHWndControl.clearList();
                MyHWndControl.addIconicVar(new HImage(MyImage));
                MyHWndControl.repaint();
            }
        }
        public void OpenImage(string OpenFile)
        {
            MyOpenPath = OpenFile;
            HOperatorSet.GenEmptyObj(out MyImage);
            HOperatorSet.GenEmptyObj(out TempImage);
            HOperatorSet.ReadImage(out TempImage, MyOpenPath);
            MyImage = TempImage.Clone();
            TempImage.Dispose();
            MyHWndControl.clearList();
            MyHWndControl.addIconicVar(new HImage(MyImage));
            MyHWndControl.repaint();

            //MyHWndControl.zoomByGUIHandle(100);
        }

        public void SaveImageWithGraphics(string SavePath)
        {
            if (MyImage.IsInitialized() && MyImage.CountObj() > 0)
            {
                string name = SavePath + "_test";
                HObject ImageWithGraphics;
                HOperatorSet.DumpWindowImage(out ImageWithGraphics, MyWindow);
                HOperatorSet.WriteImage(ImageWithGraphics, "bmp", 0, SavePath + ".bmp");
                //hWindowControl.CreateGraphics();
            }
        }


        public void SaveImage(string SavePath)
        {
            if (MyImage.IsInitialized() && MyImage.CountObj() > 0)
            {

                HOperatorSet.WriteImage(MyImage, "bmp", 0, SavePath);

                //hWindowControl.CreateGraphics();

            }
        }

        public void SaveImage()
        {
            if (MyImage.IsInitialized() && MyImage.CountObj() > 0)
            {
                string savepath = "";
                saveFileDialog1.Filter = "图片|*.bmp;|(*.*)|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    savepath = saveFileDialog1.FileName;
                }

                if (savepath != "")
                {
                    HOperatorSet.WriteImage(MyImage, "bmp", 0, savepath);
                }

                //hWindowControl.CreateGraphics();

            }
        }

        public void FitImage()
        {
            MyHWndControl.resetWindow();
            MyHWndControl.zoomByGUIHandle(100);
        }

        public void ShowMode(bool IsEdit)
        {
            _isEdit = IsEdit;
            if (_isEdit)
            {
                if (_test2dMode)
                {
                    toolStripButton_DrawCircle.Visible = true;
                    toolStripButton_DrawCircleArr.Visible = true;
                    toolStripButton_DrawLine.Visible = true;
                    toolStripButton_DrawRectangle1.Visible = true;
                    toolStripButton_DrawRectangle2.Visible = true;
                }
                else
                {
                    toolStripButton_DrawCircle.Visible = false;
                    toolStripButton_DrawCircleArr.Visible = false;
                    toolStripButton_DrawLine.Visible = false;
                    toolStripButton_DrawRectangle1.Visible = false;
                    toolStripButton_DrawRectangle2.Visible = false;
                }
                toolStrip1.Visible = true;
                MyHWndControl.repaint();
            }
            else
            {
                toolStrip1.Visible = false;
                MyHWndControl.repaint();
            }
        }

        public void SetROIMoveLecel(int i)
        {
            if (i == 1) _isROIAllownMove = true;
            else _isROIAllownMove = false;
            MyHWndControl.setmoveLevel(i);
        }

        public void SetROIDisp(int i)
        {
            if (i == 0) MyROIController.setROISign(ROIController.MODE_ROI_NEG);
            else MyROIController.setROISign(ROIController.MODE_ROI_POS);
        }

        public void GetAlliMAGES()
        {
            try
            {
                toolStripComboBox1.Items.Clear();
                foreach (KeyValuePair<string, HObject> iMAGE in iMAGES)
                {
                    toolStripComboBox1.Items.Add(iMAGE.Key);
                }
            }
            catch
            {

            }
        }

        public void ClearAlliMAGES()
        {
            try
            {
                foreach (KeyValuePair<string, HObject> iMAGE in iMAGES)
                {
                    if(iMAGE.Key != "原图") iMAGE.Value.Dispose();
                }
            }
            catch
            {

            }
        }


        #endregion

        private void hWindowControl_HMouseMove(object sender, HMouseEventArgs e)
        {

            if (MyImage != null && MyImage.IsInitialized() && MyImage.CountObj() > 0)
            {
                lbl_X.Text = e.X.ToString("0.000");
                lbl_Y.Text = e.Y.ToString("0.000");
                try
                {
                    if (e.X >= 0 && e.Y >= 0)
                    {
                        HTuple w, h;
                        HOperatorSet.GetImageSize(MyImage, out w, out h);
                        if (e.X <= w && e.Y <= h)
                        {
                            HTuple mValue;
                            HOperatorSet.GetGrayval(MyImage, e.Y, e.X, out mValue);
                            lbl_Gray.Text = mValue.ToString();
                        }
                        toolStripLabel_ROIid.Text = MyROIController.activeROIidx.ToString();
                    }
                }
                catch
                {
                }
            }
        }

        private void hWindowControl_HMouseDown(object sender, HMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && MyROIController.activeROIidx != -1)
            {
                //UpdateRegion(MyROIRectangle1.getRegion());
                //if (_isEdit && _test2dMode)
                //{ hWindowControl.ContextMenuStrip = contextMenuStrip1; }
            }
            else
            {
                //hWindowControl.ContextMenuStrip = null;
            }
        }

        private void toolStripButton_Move_Click(object sender, EventArgs e)
        {
            if (toolStripButton_Move.CheckState == CheckState.Unchecked)
            {
                toolStripButton_Move.CheckState = CheckState.Checked;
                if (0 != MyHWndControl.getListCount())
                {
                    MyHWndControl.setViewState(HWndCtrl.MODE_VIEW_MOVE);
                }
            }
            else
            {
                toolStripButton_Move.CheckState = CheckState.Unchecked;
                if (0 != MyHWndControl.getListCount())
                {
                    MyHWndControl.setViewState(HWndCtrl.MODE_VIEW_NONE);
                }
            }
        }

        private void toolStripButton_OpenImage_Click(object sender, EventArgs e)
        {
            OpenImage();
        }

        private void toolStripButton_SaveImage_Click(object sender, EventArgs e)
        {
            SaveImage();
        }

        private void toolStripButton_Fit_Click(object sender, EventArgs e)
        {
            FitImage();
        }

        private void toolStripButton_DrawCircle_Click(object sender, EventArgs e)
        {
            MyROICircle = new ROICircle();
            MyROIController.setROIShape(MyROICircle);
        }

        private void toolStripButton_DrawCircleArr_Click(object sender, EventArgs e)
        {
            MyROICircleArc = new ROICircularArc();
            MyROIController.setROIShape(MyROICircleArc);
        }

        private void toolStripButton_DrawRectangle1_Click(object sender, EventArgs e)
        {
            DrawRectangle1();
        }

        public void DrawRectangle1()
        {
            MyROIRectangle1 = new ROIRectangle1();
            MyROIController.setROIShape(MyROIRectangle1);
        }

        public void DrawRectangle1(double row1, double col1, double row2, double col2)
        {
            try
            {
                if (MyImage == null || MyImage.CountObj() <= 0) return;
                MyROIRectangle1 = new ROIRectangle1();
                MyROIRectangle1.ROW1 = row1;
                MyROIRectangle1.ROW2 = row2;
                MyROIRectangle1.COL1 = col1;
                MyROIRectangle1.COL2 = col2;
                MyROIController.ROIList.Add(MyROIRectangle1);
                MyHWndControl.repaint();
            }
            catch
            {

            }
        }

        private void toolStripButton_DrawRectangle2_Click(object sender, EventArgs e)
        {
            MyROIRectangle2 = new ROIRectangle2();
            MyROIController.setROIShape(MyROIRectangle2);
        }

        private void toolStripButton_DrawLine_Click(object sender, EventArgs e)
        {
            MyROILine = new ROILine();
            MyROIController.setROIShape(MyROILine);
        }

        private void 删除ROI_Click(object sender, EventArgs e)
        {
            MyROIController.removeActive();
        }

        public void ShowVisionEidtTool(int activeROIidx)
        {
            int IDindex = activeROIidx;
            toolSettingForm = new ToolSettingForm(MyImage, ((ROI)MyROIController.ROIList[IDindex]), MyToolsController, IDindex);
            toolSettingForm.FormClosed += ToolSettingForm_FormClosed;
            toolSettingForm.Show();
        }

        private void 编辑工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int IDindex = MyROIController.activeROIidx;
            if (IDindex == -1) return;
            toolSettingForm = new ToolSettingForm(MyImage, ((ROI)MyROIController.ROIList[IDindex]), MyToolsController, IDindex);
            toolSettingForm.FormClosed += ToolSettingForm_FormClosed;
            toolSettingForm.Show();
        }

        private void ToolSettingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Button_SaveTool_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            MyToolsController = toolSettingForm.myHaGUI.MyToolsController;
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string TkeyStr = toolStripComboBox1.SelectedItem.ToString();
            foreach (KeyValuePair<string, HObject> iMAGE in iMAGES)
            {
                if (iMAGE.Key == TkeyStr && TkeyStr != "色差缺陷区域" && TkeyStr != "边缘缺陷区域"
                    && TkeyStr != "黑点缺陷区域" && TkeyStr != "黑面缺陷区域" && TkeyStr != "黑团缺陷区域"
                    && TkeyStr != "白点缺陷区域" && TkeyStr != "白面缺陷区域" && TkeyStr != "白团缺陷区域"
                    && TkeyStr != "色缺损缺陷区域" && TkeyStr != "划痕缺陷区域" && TkeyStr != "划痕可能性纹路" && TkeyStr != "面瑕疵纹路" && TkeyStr != "瑕疵面缺陷区域"
                    && TkeyStr != "轻微划痕可能性纹路1" && TkeyStr != "轻微划痕可能性纹路2" && TkeyStr != "轻微划痕缺陷区域"
                    && TkeyStr != "轻微面划痕可能性纹路1" && TkeyStr != "轻微面划痕可能性纹路2" && TkeyStr != "轻微面划痕缺陷区域")
                {
                    MyHWndControl.clearList();
                    MyHWndControl.addIconicVar(new HImage(iMAGE.Value));
                    MyHWndControl.repaint();
                }
                else
                {
                    try
                    {
                        MyHWndControl.clearList();
                        MyHWndControl.addIconicVar(new HImage(iMAGES["Pad分割图"]));
                        if (iMAGES.ContainsKey(TkeyStr)) MyHWndControl.addIconicVar(iMAGES[TkeyStr]);
                        MyHWndControl.repaint();
                    }
                    catch
                    {
                        //MyHWndControl.clearList();
                        //MyHWndControl.addIconicVar(new HImage(iMAGES["原图"]));
                        //MyHWndControl.repaint();
                    }

                }
            }
        }
    }

}
