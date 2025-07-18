using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasonteVision.PCB_AOI_ZY_CAM
{

    public class ParamsClass
    {
        public Read_Param m_Param;

        public string CameraIP = "192.168.100.25";

        public int m_iCurIndex = 0;
        public int MAXPORT = 8554;
        public int m_iPort = 0;
        public float WIDTH = 16;
        public float HEIGHT = 9;
        public int m_nCameraType = 0;

        public int EXPOSURE_MIN = 0;
        public int EXPOSURE_MAX = 1108;
        public int EXPOSURE_MAX_2KZ = 5525;
        public int EXPOSURE_LONG_MAX = 1108;
        public int GLOBALGAIN_MIN = 0;
        public int GLOBALGAIN_MAX = 80;
        public int GLOBALGAIN_MAX_2KZ = 7000;
        public int RGAIN_MIN = 0;
        public int RGAIN_MAX = 511;
        public int BGAIN_MIN = 0;
        public int BGAIN_MAX = 511;
        public int CONTRAST_MIN = 0;
        public int CONTRAST_MAX = 100;
        public int CONTRAST_MIN_2KZ = 0;
        public int CONTRAST_MAX_2KZ = 255;
        public int HDR_MIN = 0;
        public int HDR_MAX = 16;
        public int HDR_MAX_2KZ = 255;
        public int SHARPEN_MIN = 0;
        public int SHARPEN_MAX = 160;
        public int SATURATION_MIN = 0;
        public int SATURATION_MAX = 16;
        public int BESTAE_MIN = 20;
        public int BESTAE_MAX = 200;

        public bool m_bInit = false;
        public bool m_bLongExp = false;
        public bool m_bAE = false;
        public bool m_bAWB = false;
        public bool m_bplayse = false;
        public bool m_bclose = false;
        public bool m_bplay = false;
    }
}
