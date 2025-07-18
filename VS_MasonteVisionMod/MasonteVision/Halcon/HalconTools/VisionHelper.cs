using HalconDotNet;
using MasonteVision.Halcon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace MasonteVision.Halcon.VisionTool
{
    public static class VisionHelper
    {

        public static HTuple HomMate2DModel;

        public static HObject RunAffineFllowROI(HRegion InputRegion, HaGUI m_hWindow_Vision)
        {
            HRegion FollowRegion; HObject RegionFollowed;
            FollowRegion = InputRegion;
            HOperatorSet.AffineTransRegion(FollowRegion, out RegionFollowed, HomMate2DModel, "nearest_neighbor");
            HOperatorSet.SetDraw(m_hWindow_Vision.MyWindow, "margin");
            HOperatorSet.DispObj(RegionFollowed, m_hWindow_Vision.MyWindow);
            return RegionFollowed;
        }

    }
}
