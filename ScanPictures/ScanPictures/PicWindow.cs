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
    public partial class PicWindow : Form
    {
        int Index = 0;

        public PicWindow(int index)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            Index = index;
            pictureBox1.Image = Image.FromFile(Form1.VarClass.pathList[Index]);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (Index != 0) pictureBox1.Image = Image.FromFile(Form1.VarClass.pathList[--Index]);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (Index < Form1.VarClass.pathList.Count - 1) pictureBox1.Image = Image.FromFile(Form1.VarClass.pathList[++Index]);

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Image myImage = pictureBox1.Image;
            Bitmap myBitmap = new Bitmap(myImage, myImage.Width * 2, myImage.Height * 2);
            pictureBox1.Image = myBitmap;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Image myImage = pictureBox1.Image;
            Bitmap myBitmap = new Bitmap(myImage, myImage.Width / 2, myImage.Height / 2);
            pictureBox1.Image = myBitmap;
        }

        private void PicWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            {
                if (Index < Form1.VarClass.pathList.Count - 1) pictureBox1.Image = Image.FromFile(Form1.VarClass.pathList[++Index]);
            }

            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
            {
                if (Index < Form1.VarClass.pathList.Count - 1) pictureBox1.Image = Image.FromFile(Form1.VarClass.pathList[++Index]);
            }

        }
    }
}
