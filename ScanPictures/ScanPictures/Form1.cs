using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ScanPictures
{

    public partial class Form1 : Form
    {
        public static class VarClass
        {
            private static string iniPath = Application.StartupPath + "\\" + "Config.ini";
            public static string NetPath = "";
            public static string NetUser = "";
            public static string NetPassword = "";
            public static ImageList imageList = new ImageList();
            public static List<string> pathList = new List<string>();

            public static void WriteINI()
            {
                if (!File.Exists(iniPath)) File.Create(iniPath);
                try
                {
                    File.WriteAllText(iniPath, VarClass.NetPath + "\n");
                    File.AppendAllText(iniPath, VarClass.NetUser + "\n");
                    File.AppendAllText(iniPath, VarClass.NetPassword + "\n");
                }
                catch
                {

                }
            }

            public static void ReadINI()
            {
                if (File.Exists(iniPath))
                {
                    string[] INIConfig = File.ReadAllText(iniPath).Split('\n');
                    try
                    {
                        VarClass.NetPath = INIConfig[0];
                        VarClass.NetUser = INIConfig[1];
                        VarClass.NetPassword = INIConfig[2];
                    }
                    catch
                    {

                    }
                }

            }

        }

        public NetSendFile.SendFile SendFile = new NetSendFile.SendFile();
        private object _lockSendFile = new object();
        private string PCBsn;
        private string PathScan;
        private string MyStorePath;


        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            VarClass.ReadINI();
            if (string.IsNullOrEmpty(VarClass.NetPath) && string.IsNullOrEmpty(VarClass.NetPath) && string.IsNullOrEmpty(VarClass.NetPath))
            {
                MessageBox.Show("请配置正确的路径！");
            }
            else
            {
                SendFile.ConnectState(VarClass.NetPath, VarClass.NetUser, VarClass.NetPassword);

            }
            ReadINI();
            //ShowImages(Application.StartupPath + "\\Check\\NG");
        }
        public void SendPicture()
        {
            Task.Factory.StartNew(() => { SendPictureSon(); });
        }

        private void SendPictureSon()
        {
            lock (_lockSendFile)
            {
                if (!string.IsNullOrEmpty(PathScan))
                {
                    SendFile.CopyFileAndDir(PathScan, MyStorePath);
                }

            }
        }

        private void ReadINI()
        {
            tbLANPath.Text = Form1.VarClass.NetPath;
            tbLANUserName.Text = Form1.VarClass.NetUser;
            tbLANPassword.Text = Form1.VarClass.NetPassword;
        }

        /// <summary>
        /// C# imagelist listview显示图片
        /// </summary>
        private void ShowImages(string filePath)
        {
            VarClass.imageList.ImageSize = new Size(160, 160);
            VarClass.imageList.Images.Clear();
            listView1.Clear();
            listView1.View = View.LargeIcon;
            listView1.LargeImageList = VarClass.imageList;
            DirectoryInfo di = new DirectoryInfo(filePath);
            FileInfo[] afi = di.GetFiles("*.*");

            string temp;
            int j = 0;
            VarClass.pathList.Clear();
            for (int i = 0; i < afi.Length; i++)
            {
                VarClass.pathList.Add(afi[i].FullName);
                temp = afi[i].Name.ToLower();
                if (temp.EndsWith(".jpg"))
                {
                    AddImg(ref afi[i], ref j, ".jpg");
                }
                else if (temp.EndsWith(".jpeg"))
                {
                    AddImg(ref afi[i], ref j, ".jpeg");
                }
                else if (temp.EndsWith(".gif"))
                {
                    AddImg(ref afi[i], ref j, ".gif");
                }
                else if (temp.EndsWith(".png"))
                {
                    AddImg(ref afi[i], ref j, ".png");
                }
                else if (temp.EndsWith(".bmp"))
                {
                    AddImg(ref afi[i], ref j, ".bmp");
                }
                else if (temp.EndsWith(".ico"))
                {
                    AddImg(ref afi[i], ref j, ".ico");
                }
            }
        }

        private void AddImg(ref FileInfo fi, ref int j, string ex)
        {
            VarClass.imageList.Images.Add(Image.FromFile(fi.FullName));
            listView1.Items.Add(fi.Name.Replace(ex, ""), j);
            j++;
        }



        private void NetConnect()
        {
            if (string.IsNullOrEmpty(VarClass.NetPath) && string.IsNullOrEmpty(VarClass.NetPath) && string.IsNullOrEmpty(VarClass.NetPath))
            {
                MessageBox.Show("请配置正确的路径！");
            }
            else
            {
                if (SendFile.ConnectState(VarClass.NetPath, VarClass.NetUser, VarClass.NetPassword))
                    MessageBox.Show("网络连接成功！");
                else
                    MessageBox.Show("网络连接失败！");
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            PCBsn = textBox1.Text;
            if (string.IsNullOrEmpty(PCBsn))
            {
                MessageBox.Show("PCB序列为空！");
            }
            else
            {
                if (!SendFile.IsConnect) NetConnect();
                if (SendFile.IsConnect)
                {
                    MyStorePath = Application.StartupPath + "\\Check\\" + PCBsn;
                    PathScan = VarClass.NetPath + "\\" + PCBsn + "\\" + "NG\\";
                    SendPictureSon();
                    ShowImages(MyStorePath);
                }
            }
        }

        private void toolStripConfigButton_Click(object sender, EventArgs e)
        {
            FormConfig myConfig = new FormConfig();
            myConfig.Show();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {

            int index = listView1.SelectedItems[0].Index;
            PicWindow win = new PicWindow(index);
            win.ShowDialog();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DirectoryInfo myDirectoryinfo = new DirectoryInfo(Application.StartupPath + "\\Check\\");
                DirectoryInfo[] Directorys = myDirectoryinfo.GetDirectories();
                foreach(DirectoryInfo New in Directorys)
                {
                    New.Delete(true);
                }
            }
            catch
            {

            }
        }
        private void ChageINI()
        {
            Form1.VarClass.NetPath = tbLANPath.Text;
            Form1.VarClass.NetUser = tbLANUserName.Text;
            Form1.VarClass.NetPassword = tbLANPassword.Text;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Form1.VarClass.WriteINI();
        }

        private void btn_changeINI_Click(object sender, EventArgs e)
        {
            ChageINI();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            tbLANPath.Enabled = checkBox1.Checked;
            tbLANUserName.Enabled = checkBox1.Checked;
            tbLANPassword.Enabled = checkBox1.Checked;
            btnConnect.Enabled = checkBox1.Checked;
            btn_changeINI.Enabled = checkBox1.Checked;
        }
    }
}
