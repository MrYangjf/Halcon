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

namespace HalconVision.VisionTool
{
    public partial class ToolSettingForm : Form
    {
        IVisionTool mVisionTool;
        public ToolSettingForm(IVisionTool nVisionTool)
        {
            InitializeComponent();
            mVisionTool = nVisionTool;
            propertyGrid1.SelectedObject = mVisionTool;
            this.Text = string.Format("视觉设置_{0}", mVisionTool.ReName);
            if (mVisionTool.InPutImage != null)
                if (mVisionTool.InPutImage.IsInitialized())
                    viewDisplayCtrl1.LoadImage(new HImage(mVisionTool.InPutImage));
        }
        private void btRuntest_Click(object sender, EventArgs e)
        {
            //mVisionTool.RunVision();
        }
    }
}
