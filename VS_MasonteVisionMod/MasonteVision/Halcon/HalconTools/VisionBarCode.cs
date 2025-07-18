using HalconDotNet;
using MasonteVision.Halcon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MasonteVision.Halcon.VisionTool
{
    [Serializable]
    public class VisionBarCode : IVisionTool
    {
     
        public ROI ROIFindCtrl;
        public VisionBarCode(ROI MYROI)
        {
            if (ROIFindCtrl == null)
                ROIFindCtrl = MYROI;
            ToolName = "二维码";
            ReName = ToolName;
            visionToolType = VisionToolType.VisionBarCode;
            VisionType = VisionType.扫码功能;
            
        }

        public override void SettingROI(ROI MyROI)
        {
            ROIFindCtrl = MyROI;
        }

        public override void SettingEnter()
        {
            //ToolSettingForm f = new ToolSettingForm();
            //f.Show();
        }
        private void ResetResult()
        {
            _BarCodeString.Clear();

            _BarCodeNum = 0;
            errMsg = "Null";
            _IsOk = false;
        }
        public override bool RunVision(HObject nImage, HaGUI m_hWindow_Vision)
        {
            ResetResult();
            if (nImage == null)
            {
                errMsg = "图片为空，无法进行后续图像处理操作";
                
                return false;
            }
            HTuple dataCodeHandle, resultHandles, decodeDataStrings;
            HObject rectangleROI, imageLaplace,imageSmooth ,symbolXLDs;
            HOperatorSet.GenEmptyObj(out rectangleROI);
            HOperatorSet.GenEmptyObj(out imageLaplace);
            HOperatorSet.GenEmptyObj(out symbolXLDs);
            HOperatorSet.GenEmptyObj(out imageSmooth);
            

            if (ROIFindCtrl != null)
                HOperatorSet.ReduceDomain(nImage, ROIFindCtrl.getRegion(), out rectangleROI);
            else
                rectangleROI = nImage;

            if (_EnableSmooth)
            {
                HOperatorSet.SmoothImage(rectangleROI, out imageSmooth, _SmoothFilter, _SmoolthValue);
            }
            else
                imageSmooth = rectangleROI;

            if (_EnableLaplace)
                HOperatorSet.Laplace(imageSmooth, out imageLaplace, _LaplaceResultType, _LaplaceMaskSize, _LaplaceFilterMask);
            else
                imageLaplace = rectangleROI;
            HOperatorSet.DispObj(imageLaplace, m_hWindow_Vision.MyWindow);

            HOperatorSet.CreateDataCode2dModel(_SymbolType, new HTuple("default_parameters"), _Recognition, out dataCodeHandle);
            HOperatorSet.FindDataCode2d(imageLaplace, out symbolXLDs, dataCodeHandle, new HTuple(), new HTuple(), out resultHandles, out decodeDataStrings);
            _BarCodeNum = decodeDataStrings.Length;

            if (decodeDataStrings.Length > 0)
            {
                HOperatorSet.SetColor(m_hWindow_Vision.MyWindow, "green");
                HOperatorSet.DispObj(symbolXLDs, m_hWindow_Vision.MyWindow);
                _BarCodeString = decodeDataStrings.SArr.ToList();
                _IsOk = true;
                errMsg = "扫码成功";
            }
            else
            {
                HOperatorSet.SetColor(m_hWindow_Vision.MyWindow, "red");
                if (ROIFindCtrl != null)
                    HOperatorSet.DispObj(ROIFindCtrl.getRegion(), m_hWindow_Vision.MyWindow);
                errMsg = "扫码失败";

            }
            HOperatorSet.ClearDataCode2dModel(dataCodeHandle);
            return _IsOk;
        }
       
        #region 结果

        int _BarCodeNum;
        
        public int BarCodeNum
        {
            get { return _BarCodeNum; }
        }
        List<string> _BarCodeString = new List<string>();
        
        public List<string> BarCodeString
        {
            get { return _BarCodeString; }
        }
        string errMsg;
       
        public string ErrMsg
        {
            get { return errMsg; }
        }
        bool _IsOk = false;
        
        public bool IsOK
        {
            get { return _IsOk; }
        }
        #endregion


        #region 参数
        bool _EnableSmooth = false;
       
        public bool EnableSmooth
        {
            get { return _EnableSmooth; }
            set { _EnableSmooth = value; }
        }
        string _SmoothFilter = "deriche1";
        
        public string SmoothFilter
        {
            get { return _SmoothFilter; }
            set { _SmoothFilter = value; }
        }
        double _SmoolthValue = 0.5;
        
        public double SmoolthValue
        {
            get { return _SmoolthValue; }
            set { _SmoolthValue = value; }
        }
        bool _EnableLaplace = false;
        
        public bool EnableLaplace
        {
            get { return _EnableLaplace; }
            set { _EnableLaplace = value; }
        }
        
        string _LaplaceResultType = "absolute";
        
        public string LaplaceResultType
        {
            get { return _LaplaceResultType; }
            set { _LaplaceResultType = value; }
        }
        int _LaplaceMaskSize = 3;
        [Category("参数"), Description("3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35, 37, 39")]
        public int LaplaceMaskSize
        {
            get { return _LaplaceMaskSize; }
            set
            {
                if (value % 2 == 1)
                {
                    _LaplaceMaskSize = value;
                }
                else
                {
                    _LaplaceMaskSize = value / 2 + 1;
                }
            }
        }

        string _LaplaceFilterMask = "n_4";
        
        public string LaplaceFilterMask
        {
            get { return _LaplaceFilterMask; }
            set { _LaplaceFilterMask = value; }
        }

        string _SymbolType = "Data Matrix ECC 200";
       
        public string SymbolType
        {
            get { return _SymbolType; }
            set { _SymbolType = value; }
        }
        string _Recognition = "standard_recognition";
       
        public string Recognition
        {
            get { return _Recognition; }
            set { _Recognition = value; }
        }
        #endregion
    }
}
