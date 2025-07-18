using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasonteVision
{
    public partial class FormTest : Form
    {

        string PCB_Name = "test";//pcb 板类名
        string[] PCB_Code = new string[4] { "test1", "test2", "test2", "test2" };//每个PCB板扫描的Code
        int PCB_DUT_Index = 0;//pcb板的小板序列号
        int PCB_DOT_Index = 0;//pcb板的点序列号
        string DefultImageStorePath = "D:\\Image";//图像保存路径

        int ColorSelect = 0;//点按照不同类别进行参数保存

        MainForm CamMainForm;
        public FormTest()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CamMainForm.HideEditUI(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CamMainForm.HideEditUI(false);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            CamMainForm = new MainForm();
            CamMainForm.Dock = DockStyle.Fill;
            CamMainForm.Parent = panel1;
            CamMainForm.BringToFront();
            CamMainForm.Init2DUI(PCB_Name, ColorSelect);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CamMainForm.InitPCB_AOI_CAM();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CamMainForm.LoadTestParameters(PCB_Name, 0, ColorSelect);
            CamMainForm.LoadTestParameters(PCB_Name, 1, ColorSelect);
            CamMainForm.LoadTestParameters(PCB_Name, 2, ColorSelect);
            CamMainForm.LoadTestParameters(PCB_Name, 3, ColorSelect);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CamMainForm.SaveTestParameters(PCB_Name, 0, ColorSelect);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int step = 0;
            Task FilesTest = new Task(() =>
            {
                while (true)
                {
                    switch (step)
                    {
                        case 0:
                            //检测状态，方便界面显示
                            if (!CamMainForm.IsInitCamGUI) label1.Text = "资源内存初始化成功！";
                            if (CamMainForm.IsLoadParamters.Contains(false)) label2.Text = "未完全加载参数！";//未加载参数
                            if (!CamMainForm.IsInitZYCam[0]) label3.Text = "1号相机未初始化成功！";
                            if (CamMainForm.IsViewer.Contains(false)) label4.Text = "未开启预览！";
                            if (CamMainForm.IsCamBusing[0]) label5.Text = "1号相机正在运行中！";
                            step = 10;
                            break;
                        case 10:
                            //开启预览
                            CamMainForm.StartGrab2DCamera();
                            step = 20;
                            break;
                        case 20:
                            //触发分析
                            if (CamMainForm.TriggerTest(PCB_Code, PCB_DUT_Index, PCB_DOT_Index, ColorSelect, DefultImageStorePath, true)) step = 30;
                            break;
                        case 30:
                            //等待相机取图结束即可移动相机
                            if (CamMainForm.IsCamBusing.Contains(true)) break;
                            step = 40;
                            break;
                        case 40:
                            //等待相机分析结束才可以触发下一次分析
                            if (CamMainForm.IsCamRunFinished.Contains(true)) break;
                            step = 50;
                            break;
                        case 50:
                            //拿图片和结果
                            string ErrorMsg = CamMainForm.CameraRunErrorMsg;
                            string MyFilePath = CamMainForm.ImageSavePath[0];
                            bool IsToolRunOK = CamMainForm.IsToolRunOK[0];//0号相机工具运行是否成功
                            bool IsRunPASS = CamMainForm.IsRunPass[0];//0号相机工具运行判断点是否PASS

                            break;
                    }
                }
            });
            FilesTest.Start();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CamMainForm.ReloadFlowDependNameAndColor(PCB_Name, ColorSelect);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            CamMainForm.SaveTestParameters(0, ColorSelect + 1);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            CamMainForm.StartGrab2DCamera();
        }
    }
}
