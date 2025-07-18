using AllInterface;
using HalconDotNet;
using NBCommon;
using NBCommon.Controls.ViewDisplay;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HalconVision.VisionTool
{
    [Serializable]
    public class VisionBarCode : IVisionTool
    {
        public  event VisionHandle OnRunFinished;
        public ROIController ROIFindCtrl;
        public VisionBarCode()
        {
            if (ROIFindCtrl == null)
                ROIFindCtrl = new ROIController();
            ToolName = "二维码";
            ReName = ToolName;
            visionToolType = VisionToolType.VisionBarCode;
            VisionType = VisionType.扫码功能;
            IcoImage = global::HalconVision.Properties.Resources.camera;
        }
        public override void SettingEnter()
        {
            ToolSettingForm f = new ToolSettingForm(this);
            f.Show();
        }
        private void ResetResult()
        {
            _BarCodeString.Clear();
            _BarCodeNum = 0;
            errMsg = "Null";
            _IsOk = false;
        }
        public override bool RunVision(HObject nImage, ViewDisplayCtrl m_hWindow_Vision)
        {
            ResetResult();
            if (nImage == null)
            {
                errMsg = "图片为空，无法进行后续图像处理操作";
                OnFinished(this, new VisionEventArgs(0, false));
                return false;
            }
            HTuple dataCodeHandle, resultHandles, decodeDataStrings;
            HObject rectangleROI, imageLaplace,imageSmooth ,symbolXLDs;
            HOperatorSet.GenEmptyObj(out rectangleROI);
            HOperatorSet.GenEmptyObj(out imageLaplace);
            HOperatorSet.GenEmptyObj(out symbolXLDs);
            HOperatorSet.GenEmptyObj(out imageSmooth);
            

            if (ROIFindCtrl.GetRegion() != null)
                HOperatorSet.ReduceDomain(nImage, ROIFindCtrl.GetRegion(), out rectangleROI);
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
            HOperatorSet.DispObj(imageLaplace, m_hWindow_Vision.ViewHwindow);

            HOperatorSet.CreateDataCode2dModel(_SymbolType, new HTuple("default_parameters"), _Recognition, out dataCodeHandle);
            HOperatorSet.FindDataCode2d(imageLaplace, out symbolXLDs, dataCodeHandle, new HTuple(), new HTuple(), out resultHandles, out decodeDataStrings);
            _BarCodeNum = decodeDataStrings.Length;

            if (decodeDataStrings.Length > 0)
            {
                HOperatorSet.SetColor(m_hWindow_Vision.ViewHwindow, "green");
                HOperatorSet.DispObj(symbolXLDs, m_hWindow_Vision.ViewHwindow);
                _BarCodeString = decodeDataStrings.SArr.ToList();
                _IsOk = true;
                errMsg = "扫码成功";
            }
            else
            {
                HOperatorSet.SetColor(m_hWindow_Vision.ViewHwindow, "red");
                if (ROIFindCtrl.GetRegion() != null)
                    HOperatorSet.DispObj(ROIFindCtrl.GetRegion(), m_hWindow_Vision.ViewHwindow);
                errMsg = "扫码失败";

            }
            HOperatorSet.ClearDataCode2dModel(dataCodeHandle);
            return _IsOk;
        }
        private void OnFinished(object sender, VisionEventArgs e)
        {
            if (OnRunFinished != null)
                OnRunFinished(sender, e);
        }
        #region 结果

        int _BarCodeNum;
        [Category("结果"), Description("个数"),DisplayName("码个数"),XmlIgnore]
        public int BarCodeNum
        {
            get { return _BarCodeNum; }
        }
        List<string> _BarCodeString = new List<string>();
        [Category("结果"), Description("数据"), XmlIgnore]
        public List<string> BarCodeString
        {
            get { return _BarCodeString; }
        }
        string errMsg;
        [Category("结果"), Description("运行结果消息"), XmlIgnore]
        public string ErrMsg
        {
            get { return errMsg; }
        }
        bool _IsOk = false;
        [Category("结果"), Description("运行结果"), XmlIgnore]
        public bool IsOK
        {
            get { return _IsOk; }
        }
        #endregion


        #region 参数
        bool _EnableSmooth = false;
        [Category("参数"), Description("是否启用羽化图像预处理")]
        public bool EnableSmooth
        {
            get { return _EnableSmooth; }
            set { _EnableSmooth = value; }
        }
        string _SmoothFilter = "deriche1";
        [Category("参数"), TypeConverter(typeof(SmoothFilterItem)), Description("羽化滤镜")]
        public string SmoothFilter
        {
            get { return _SmoothFilter; }
            set { _SmoothFilter = value; }
        }
        double _SmoolthValue = 0.5;
        [Category("参数"), Description("羽化值")]
        public double SmoolthValue
        {
            get { return _SmoolthValue; }
            set { _SmoolthValue = value; }
        }
        bool _EnableLaplace = false;
        [Category("参数"), Description("是否启用Laplace图像预处理")]
        public bool EnableLaplace
        {
            get { return _EnableLaplace; }
            set { _EnableLaplace = value; }
        }
        
        string _LaplaceResultType = "absolute";
        [Category("参数"), TypeConverter(typeof(ResultTypeItem)), Description("Laplace类型")]
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
        [Category("参数"), TypeConverter(typeof(FilterMaskItem)), Description("联通参数")]
        public string LaplaceFilterMask
        {
            get { return _LaplaceFilterMask; }
            set { _LaplaceFilterMask = value; }
        }

        string _SymbolType = "Data Matrix ECC 200";
        [Category("参数"), TypeConverter(typeof(SymbolTypeItem)), Description("Type of the 2D data code.")]
        public string SymbolType
        {
            get { return _SymbolType; }
            set { _SymbolType = value; }
        }
        string _Recognition = "standard_recognition";
        [Category("参数"), TypeConverter(typeof(RecognitionItem)), Description("识别精细度")]
        public string Recognition
        {
            get { return _Recognition; }
            set { _Recognition = value; }
        }
        #endregion
    }
}
