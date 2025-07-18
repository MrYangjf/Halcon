using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Serialization;
using HalconDotNet;

namespace MasonteVision.Halcon.VisionTool
{
    [Serializable]
    public class VisionClarity : IVisionTool
    {

        #region 参数
        int _Method = 1;
        [Category("参数"), DisplayName("计算方式"), Description("1 = 'Deviation'；2 = 'laplace'；3 = 'energy'；4 = 'Brenner'，5 = 'Tenegrad'")]
        public int Method
        {
            get { return _Method; }
            set { _Method = value; }
        }

        #endregion
        #region 结果

        [XmlIgnore]
        double _RetValue = 0;
        [Category("结果"), DisplayName("清晰度结果"), Description("能量值"), XmlIgnore]
        public double RetValue
        {
            get { return _RetValue; }
        }

        [XmlIgnore]
        string errMsg;
        [Category("结果"), DisplayName("运行信息"), Description("运行结果消息"), XmlIgnore]
        public string ErrMsg
        {
            get { return errMsg; }
        }
        #endregion
        #region 判定

        #endregion

        public HObject rectangleROI;
        public ROI ROIFindCtrl;// = new ROIController();
        public VisionClarity(ROI MYROI)
        {
            if (ROIFindCtrl == null)
            {
                ROIFindCtrl = MYROI;
            }
            ToolName = "清晰度分析";
            ReName = ToolName;
            visionToolType = VisionToolType.VisionClarity;
            VisionType = VisionType.形态分析;
            //IcoImage = global::HalconVision.Properties.Resources.camera;
        }
        public override void SettingROI(ROI MyROI)
        {
            ROIFindCtrl = MyROI;
        }

        public override void SettingEnter()
        {
            //ToolSettingForm f = new ToolSettingForm();
            //f.ShowDialog();
        }
        private void ResetResult()
        {
            _RetValue = 0;
        }
        public override bool RunVision(HObject image, HaGUI m_hWindow_Vision)
        {
            HObject ho_Image = null;
            HObject ho_ImageMean = null, ho_ImageSub = null;
            HObject ho_ImageResult = null, ho_ImageLaplace4 = null, ho_ImageLaplace8 = null;
            HObject ho_ImageResult1 = null, ho_ImagePart00 = null, ho_ImagePart01 = null;
            HObject ho_ImagePart10 = null, ho_ImageSub1 = null, ho_ImageSub2 = null;
            HObject ho_ImageResult2 = null, ho_ImagePart20 = null, ho_EdgeAmplitude = null;
            HObject ho_Region1 = null, ho_BinImage = null, ho_ImageResult4 = null;

            // Local control variables 

            HTuple hv_Width = null, hv_Height = null;
            HTuple hv_Value = new HTuple(), hv_Deviation = new HTuple();
            HTuple hv_Min = new HTuple(), hv_Max = new HTuple(), hv_Range = new HTuple();

            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_ImageMean);
            HOperatorSet.GenEmptyObj(out ho_ImageSub);
            HOperatorSet.GenEmptyObj(out ho_ImageResult);
            HOperatorSet.GenEmptyObj(out ho_ImageLaplace4);
            HOperatorSet.GenEmptyObj(out ho_ImageLaplace8);
            HOperatorSet.GenEmptyObj(out ho_ImageResult1);
            HOperatorSet.GenEmptyObj(out ho_ImagePart00);
            HOperatorSet.GenEmptyObj(out ho_ImagePart01);
            HOperatorSet.GenEmptyObj(out ho_ImagePart10);
            HOperatorSet.GenEmptyObj(out ho_ImageSub1);
            HOperatorSet.GenEmptyObj(out ho_ImageSub2);
            HOperatorSet.GenEmptyObj(out ho_ImageResult2);
            HOperatorSet.GenEmptyObj(out ho_ImagePart20);
            HOperatorSet.GenEmptyObj(out ho_EdgeAmplitude);
            HOperatorSet.GenEmptyObj(out ho_Region1);
            HOperatorSet.GenEmptyObj(out ho_BinImage);
            HOperatorSet.GenEmptyObj(out ho_ImageResult4);

            ResetResult();
            //m_hWindow_Vision.MyHWndControl.RemoveIconicVar();
            if (image == null)
            {
                errMsg = "图片为空，无法进行后续图像处理操作";

                return false;
            }
            else
            {
                //hWindow.LoadImage(image);
            }
            if (ROIFindCtrl != null)
                HOperatorSet.ReduceDomain(image, ROIFindCtrl.getRegion(), out rectangleROI);
            else
                rectangleROI = image;
            HOperatorSet.GetImageSize(image, out hv_Width, out hv_Height);

            HOperatorSet.SetColor(m_hWindow_Vision.MyWindow, "green");
            HOperatorSet.SetDraw(m_hWindow_Vision.MyWindow, "margin");

            switch(_Method)
            {
                case 1:
                    ho_ImageMean.Dispose();
                    HOperatorSet.RegionToMean(image, image, out ho_ImageMean);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_ImageMean, out ExpTmpOutVar_0, "real");
                        ho_ImageMean.Dispose();
                        ho_ImageMean = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(image, out ExpTmpOutVar_0, "real");
                        ho_Image.Dispose();
                        ho_Image = ExpTmpOutVar_0;
                    }
                    ho_ImageSub.Dispose();
                    HOperatorSet.SubImage(ho_Image, ho_ImageMean, out ho_ImageSub, 1, 0);
                    ho_ImageResult.Dispose();
                    HOperatorSet.MultImage(ho_ImageSub, ho_ImageSub, out ho_ImageResult, 1, 0);
                    HOperatorSet.Intensity(ho_ImageResult, ho_ImageResult, out hv_Value, out hv_Deviation);
                    break;
                case 2:
                    ho_ImageLaplace4.Dispose();
                    HOperatorSet.Laplace(image, out ho_ImageLaplace4, "signed", 3, "n_4");
                    ho_ImageLaplace8.Dispose();
                    HOperatorSet.Laplace(image, out ho_ImageLaplace8, "signed", 3, "n_8");
                    ho_ImageResult1.Dispose();
                    HOperatorSet.AddImage(ho_ImageLaplace4, ho_ImageLaplace4, out ho_ImageResult1,
                        1, 0);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.AddImage(ho_ImageLaplace4, ho_ImageResult1, out ExpTmpOutVar_0,
                            1, 0);
                        ho_ImageResult1.Dispose();
                        ho_ImageResult1 = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.AddImage(ho_ImageLaplace8, ho_ImageResult1, out ExpTmpOutVar_0,
                            1, 0);
                        ho_ImageResult1.Dispose();
                        ho_ImageResult1 = ExpTmpOutVar_0;
                    }
                    ho_ImageResult.Dispose();
                    HOperatorSet.MultImage(ho_ImageResult1, ho_ImageResult1, out ho_ImageResult,
                        1, 0);
                    HOperatorSet.Intensity(ho_ImageResult, ho_ImageResult, out hv_Value, out hv_Deviation);
                    break;
                case 3:
                    ho_ImagePart00.Dispose();
                    HOperatorSet.CropPart(image, out ho_ImagePart00, 0, 0, hv_Width - 1, hv_Height - 1);
                    ho_ImagePart01.Dispose();
                    HOperatorSet.CropPart(image, out ho_ImagePart01, 0, 1, hv_Width - 1, hv_Height - 1);
                    ho_ImagePart10.Dispose();
                    HOperatorSet.CropPart(image, out ho_ImagePart10, 1, 0, hv_Width - 1, hv_Height - 1);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_ImagePart00, out ExpTmpOutVar_0, "real");
                        ho_ImagePart00.Dispose();
                        ho_ImagePart00 = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_ImagePart10, out ExpTmpOutVar_0, "real");
                        ho_ImagePart10.Dispose();
                        ho_ImagePart10 = ExpTmpOutVar_0;
                    }
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_ImagePart01, out ExpTmpOutVar_0, "real");
                        ho_ImagePart01.Dispose();
                        ho_ImagePart01 = ExpTmpOutVar_0;
                    }
                    ho_ImageSub1.Dispose();
                    HOperatorSet.SubImage(ho_ImagePart10, ho_ImagePart00, out ho_ImageSub1, 1,
                        0);
                    ho_ImageResult1.Dispose();
                    HOperatorSet.MultImage(ho_ImageSub1, ho_ImageSub1, out ho_ImageResult1, 1,
                        0);
                    ho_ImageSub2.Dispose();
                    HOperatorSet.SubImage(ho_ImagePart01, ho_ImagePart00, out ho_ImageSub2, 1,
                        0);
                    ho_ImageResult2.Dispose();
                    HOperatorSet.MultImage(ho_ImageSub2, ho_ImageSub2, out ho_ImageResult2, 1,
                        0);
                    ho_ImageResult.Dispose();
                    HOperatorSet.AddImage(ho_ImageResult1, ho_ImageResult2, out ho_ImageResult,
                        1, 0);
                    HOperatorSet.Intensity(ho_ImageResult, ho_ImageResult, out hv_Value, out hv_Deviation);
                    break;
                case 4:
                    ho_ImagePart00.Dispose();
                    HOperatorSet.CropPart(image, out ho_ImagePart00, 0, 0, hv_Width, hv_Height - 2);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_ImagePart00, out ExpTmpOutVar_0, "real");
                        ho_ImagePart00.Dispose();
                        ho_ImagePart00 = ExpTmpOutVar_0;
                    }
                    ho_ImagePart20.Dispose();
                    HOperatorSet.CropPart(image, out ho_ImagePart20, 2, 0, hv_Width, hv_Height - 2);
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.ConvertImageType(ho_ImagePart20, out ExpTmpOutVar_0, "real");
                        ho_ImagePart20.Dispose();
                        ho_ImagePart20 = ExpTmpOutVar_0;
                    }
                    ho_ImageSub.Dispose();
                    HOperatorSet.SubImage(ho_ImagePart20, ho_ImagePart00, out ho_ImageSub, 1, 0);
                    ho_ImageResult.Dispose();
                    HOperatorSet.MultImage(ho_ImageSub, ho_ImageSub, out ho_ImageResult, 1, 0);
                    HOperatorSet.Intensity(ho_ImageResult, ho_ImageResult, out hv_Value, out hv_Deviation);
                    break;
                case 5:
                    ho_EdgeAmplitude.Dispose();
                    HOperatorSet.SobelAmp(image, out ho_EdgeAmplitude, "sum_sqrt", 3);
                    HOperatorSet.MinMaxGray(ho_EdgeAmplitude, ho_EdgeAmplitude, 0, out hv_Min,
                        out hv_Max, out hv_Range);
                    ho_Region1.Dispose();
                    HOperatorSet.Threshold(ho_EdgeAmplitude, out ho_Region1, 11.8, 255);
                    ho_BinImage.Dispose();
                    HOperatorSet.RegionToBin(ho_Region1, out ho_BinImage, 1, 0, hv_Width, hv_Height);
                    ho_ImageResult4.Dispose();
                    HOperatorSet.MultImage(ho_EdgeAmplitude, ho_BinImage, out ho_ImageResult4,
                        1, 0);
                    ho_ImageResult.Dispose();
                    HOperatorSet.MultImage(ho_ImageResult4, ho_ImageResult4, out ho_ImageResult,
                        1, 0);
                    HOperatorSet.Intensity(ho_ImageResult, ho_ImageResult, out hv_Value, out hv_Deviation);
                    break;
            }

            _RetValue = hv_Deviation.D;
           

            return true;
        }

        public override void ShowResultText(HaGUI m_hWindow_Vision)
        {
            HTuple StringShow = "清晰度值结果为:" + _RetValue;
            m_hWindow_Vision.MyHWndControl.DispMessage(StringShow, 10, 10, "green");
            
        }
    }

}
