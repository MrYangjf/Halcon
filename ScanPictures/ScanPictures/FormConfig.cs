using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScanPictures
{
    public partial class FormConfig : Form
    {
        public FormConfig()
        {
            InitializeComponent();
            tbLANPath.Text = Form1.VarClass.NetPath;
            tbLANUserName.Text = Form1.VarClass.NetUser;
            tbLANPassword.Text = Form1.VarClass.NetPassword;

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Form1.VarClass.NetPath = tbLANPath.Text;
            Form1.VarClass.NetUser = tbLANUserName.Text;
            Form1.VarClass.NetPassword = tbLANPassword.Text;
            Form1.VarClass.WriteINI();
            this.Close();
        }
    }
}
