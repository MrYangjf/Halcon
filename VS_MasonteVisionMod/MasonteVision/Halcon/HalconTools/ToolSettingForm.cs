using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MasonteVision.Halcon.VisionTool;

namespace MasonteVision.Halcon
{

    public partial class ToolSettingForm : Form
    {
        int FlowID;
        IVisionTool mVisionTool;
        public HaGUI myHaGUI = new HaGUI();

        VisionBarCode myVBarCode;
        VisionBlob myVBlob;
        VisionCircle myVCircle;
        VisionLine myVLine;
        VisionTemplate myVTemplate;
        VisionClarity myVClarity;
        VisionPadPanic myVPadPanic;

        public ToolSettingForm(HObject myImage, ROI Roi, ToolsController toolsController, int ROIID)
        {
            InitializeComponent();
            FlowID = ROIID;
            comboBox1.SelectedIndex = 0;
            panel_iMAGE.Controls.Add(myHaGUI);
            myHaGUI.Dock = DockStyle.Fill;
            myHaGUI.UpdateImage(myImage);
            myHaGUI.MyROIController.ROIList.Add(Roi);
            myHaGUI.MyROIController.paintData(myHaGUI.MyWindow);
            myHaGUI.MyToolsController = toolsController;
            myHaGUI.SetROIMoveLecel(2);
            ShowToolsController();
            if (myHaGUI.MyToolsController.VisionToolList.Count > 0) ChangeTool((IVisionTool)myHaGUI.MyToolsController.VisionToolList[0]);
            ShowEdit();
        }
        private void btRuntest_Click(object sender, EventArgs e)
        {
            if (mVisionTool == null) return;
            myHaGUI.MyHWndControl.clearList();
            myHaGUI.MyHWndControl.addIconicVar(myHaGUI.MyImage);
            mVisionTool.RunVision(myHaGUI.MyImage, myHaGUI);
            myHaGUI.MyHWndControl.repaint();
            ShowToolsController();
            ShowEdit();
            myHaGUI.MyHWndControl.setViewState(HWndCtrl.MODE_VIEW_MOVE);
        }

        public void ChangeTool(IVisionTool nVisionTool)
        {
            mVisionTool = nVisionTool;
        }


        void ShowToolsController()
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < myHaGUI.MyToolsController.VisionToolList.Count; i++)
            {
                if (((IVisionTool)myHaGUI.MyToolsController.VisionToolList[i]).IndexID == FlowID.ToString())
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = ((IVisionTool)myHaGUI.MyToolsController.VisionToolList[i]).ReName;
                }
            }
        }

        public void ShowEdit()
        {
            if (mVisionTool == null)
            {
                myVPadPanic = new VisionPadPanic((ROI)myHaGUI.MyROIController.ROIList[0]);
                myVPadPanic.IndexID = FlowID.ToString();
                myHaGUI.MyToolsController.AddVisionTool(myVPadPanic);
                ChangeTool(myVPadPanic);
                ShowEdit();
                ShowToolsController();
            }
            propertyGrid1.SelectedObject = mVisionTool;
            this.Text = string.Format("视觉设置_{0}", mVisionTool.ReName);

        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            int TempID = -1;
            int TrueID = 0;
            int RoundID = 0;
            if (myHaGUI.MyToolsController.VisionToolList.Count > 0)
            {
                for (int i = 0; i < myHaGUI.MyToolsController.VisionToolList.Count; i++)
                {

                    if (((IVisionTool)myHaGUI.MyToolsController.VisionToolList[i]).IndexID == FlowID.ToString())
                    {
                        TempID++;
                        if (index == TempID)
                        {
                            TrueID = RoundID;
                        }
                    }
                    RoundID++;
                }
                ChangeTool((IVisionTool)myHaGUI.MyToolsController.VisionToolList[TrueID]);
                ShowEdit();
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            if (mVisionTool == null)
            {
                MessageBox.Show("工具为空!");
                return;
            }
            if (mVisionTool.ToolName != "模板匹配")
            {
                MessageBox.Show("工具不支持创建模板,工具必须是模板匹配!");
                return;
            }
            ((VisionTemplate)mVisionTool).CreateShapeModel(myHaGUI.MyImage, myHaGUI);
            ((VisionTemplate)mVisionTool).SaveModel();
        }

        private void button_SaveTool_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            string i = comboBox1.SelectedItem.ToString();
            switch (i)
            {
                case "Pad脏污工具":
                    myVPadPanic = new VisionPadPanic((ROI)myHaGUI.MyROIController.ROIList[0]);
                    myVPadPanic.IndexID = FlowID.ToString();
                    myHaGUI.MyToolsController.AddVisionTool(myVPadPanic);
                    ChangeTool(myVPadPanic);
                    ShowEdit();
                    ShowToolsController();
                    break;
                case "抓线工具":
                    myVLine = new VisionLine((ROI)myHaGUI.MyROIController.ROIList[0]);
                    myVLine.IndexID = FlowID.ToString();
                    myHaGUI.MyToolsController.AddVisionTool(myVLine);
                    ChangeTool(myVLine);
                    ShowEdit();
                    ShowToolsController();
                    break;
                case "模板工具":
                    myVTemplate = new VisionTemplate((ROI)myHaGUI.MyROIController.ROIList[0]);
                    myVTemplate.IndexID = FlowID.ToString();
                    myHaGUI.MyToolsController.AddVisionTool(myVTemplate);
                    ChangeTool(myVTemplate);
                    ShowEdit();
                    ShowToolsController();
                    break;
                case "清晰度工具":
                    myVClarity = new VisionClarity((ROI)myHaGUI.MyROIController.ROIList[0]);
                    myVClarity.IndexID = FlowID.ToString();
                    myHaGUI.MyToolsController.AddVisionTool(myVClarity);
                    ChangeTool(myVClarity);
                    ShowEdit();
                    ShowToolsController();
                    break;
                case "读码工具":
                    myVBarCode = new VisionBarCode((ROI)myHaGUI.MyROIController.ROIList[0]);
                    myVBarCode.IndexID = FlowID.ToString();
                    myHaGUI.MyToolsController.AddVisionTool(myVBarCode);
                    ChangeTool(myVBarCode);
                    ShowEdit();
                    ShowToolsController();
                    break;
                case "Blob工具":
                    myVBlob = new VisionBlob((ROI)myHaGUI.MyROIController.ROIList[0]);
                    myVBlob.IndexID = FlowID.ToString();
                    myHaGUI.MyToolsController.AddVisionTool(myVBlob);
                    ChangeTool(myVBlob);
                    ShowEdit();
                    ShowToolsController();
                    break;
                case "抓圆工具":
                    myVCircle = new VisionCircle((ROI)myHaGUI.MyROIController.ROIList[0]);
                    myVCircle.IndexID = FlowID.ToString();
                    myHaGUI.MyToolsController.AddVisionTool(myVCircle);
                    ChangeTool(myVBlob);
                    ShowEdit();
                    ShowToolsController();
                    break;

            }
        }
    }
}
