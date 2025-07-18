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
    public partial class ParamContrast : Form
    {

        public ParamContrast()
        {
            InitializeComponent();
        }

        public  void LoadParam(VisionPadPanic[] ClassParam)
        {
            try
            {
                propertyGrid1.SelectedObject = ClassParam[0];
                propertyGrid2.SelectedObject = ClassParam[1];
                propertyGrid3.SelectedObject = ClassParam[2];
                propertyGrid4.SelectedObject = ClassParam[3];
            }
            catch
            {

            }
        }
    }
}
