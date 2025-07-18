using AllInterface;
using HalconDotNet;
using NBCommon.Controls.ViewDisplay;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NBCommon;
using System.Windows.Forms;

namespace HalconVision.VisionTool
{
    [Serializable]
    public class VisionTemplateOCR : IVisionTool
    {
        public event VisionHandle OnRunFinished;

        public ROIController ROICreateCtrl;
        public ROIController ROIFollowCtrl;
        public ROIController ROIFollowCtrl2;
        public ROIController ROIFollowCtrl3;
        public ROIController ROIFindCtrl;
        HTuple m_ModelID_xld;
        public VisionTemplateOCR()
        {
            if (ROICreateCtrl == null)
                ROICreateCtrl = new ROIController();
            if (ROIFindCtrl == null)
                ROIFindCtrl = new ROIController();
            if (ROIFollowCtrl == null)
                ROIFollowCtrl = new ROIController();
            if (ROIFollowCtrl2 == null)
                ROIFollowCtrl2 = new ROIController();
            if (ROIFollowCtrl3 == null)
                ROIFollowCtrl3 = new ROIController();

            ToolName = "文字识别OCR";
            ReName = ToolName;
            visionToolType = VisionToolType.VisionTemplateOCR;
            VisionType = VisionType.扫码功能;
            IcoImage = global::HalconVision.Properties.Resources.camera;
        }
        public override void SettingEnter()
        {
            ToolSettingForm f = new ToolSettingForm(this);
            f.Show();
        }
        public override bool RunVision(HObject hImage, ViewDisplayCtrl m_hWindow_Vision)
        {
            ResetResult();
            
            if (!_IsCreateModel)
            {
                WriteLog.GetInstance.LogMsg("模板未创建..");
                return false;
            }
            if (_BorderShapeModel)
                HOperatorSet.SetSystem("border_shape_models", "true");
            else
                HOperatorSet.SetSystem("border_shape_models", "false");
            HTuple HomMate2DModel;
            HObject ModelContours, ho_ModelContoursTrans, cross, imageReduced, RegionFollowed, OCRReduced, ImageEmphasize, ImageInvert;
            HOperatorSet.GenEmptyObj(out ModelContours);
            HOperatorSet.GenEmptyObj(out ho_ModelContoursTrans);
            HOperatorSet.GenEmptyObj(out cross);
            HOperatorSet.GenEmptyObj(out imageReduced);
            HOperatorSet.GenEmptyObj(out RegionFollowed);
            HOperatorSet.GenEmptyObj(out OCRReduced);
            HOperatorSet.GenEmptyObj(out ImageEmphasize);
            HOperatorSet.GenEmptyObj(out ImageInvert);
            HTuple hv_Row, hv_Column, hv_Angle, hv_Score;
            HRegion FindRegion = ROIFindCtrl.GetRegion();
            HRegion CreatRegion = ROICreateCtrl.GetRegion();
            HRegion  FollowRegion = ROIFollowCtrl.GetRegion();
            HRegion FollowRegion2 = ROIFollowCtrl2.GetRegion();
            HRegion FollowRegion3 = ROIFollowCtrl3.GetRegion();
            HTuple RowModel, CloumnModel, hv_HomMat2DIdentity = null;
            CreatRegion.AreaCenter(out RowModel, out CloumnModel);
            HObject RegionThreshold, RegionDilation, RegionErosion, RegionConnection, RegionPartitioned, RegionSelectedArea, RegionSelectedHight, RegionSelectedWidth, SortRegions;
            HOperatorSet.GenEmptyObj(out RegionThreshold);
            HOperatorSet.GenEmptyObj(out RegionDilation);
            HOperatorSet.GenEmptyObj(out RegionErosion);
            HOperatorSet.GenEmptyObj(out RegionConnection);
            HOperatorSet.GenEmptyObj(out RegionPartitioned);
            HOperatorSet.GenEmptyObj(out RegionSelectedArea);
            HOperatorSet.GenEmptyObj(out RegionSelectedHight);
            HOperatorSet.GenEmptyObj(out RegionSelectedWidth);
            HOperatorSet.GenEmptyObj(out SortRegions);
            HTuple Checkarea,Checkrow,Checkcolumn,OCRHandle, hv_Class, hv_sum;
            HTuple OCR_Tuple = new HTuple(); 
            try
            {
                if (FindRegion != null)
                    HOperatorSet.ReduceDomain(hImage, FindRegion, out imageReduced);
                else
                    imageReduced = hImage;
                //模板匹配
                HOperatorSet.FindShapeModel(imageReduced, m_ModelID_xld, new HTuple(_AngleStart), new HTuple(_AngleExtent),
                    new HTuple(_MinScore), new HTuple(1), new HTuple(_MaxOverLap), new HTuple(_subPixel), new HTuple(_NumLevels), new HTuple(_Greediness), out hv_Row,
                    out hv_Column, out hv_Angle, out hv_Score);

                _TemplateCount = hv_Column.TupleLength();
                //检测是否找到模板
                if (_TemplateCount > 0)
                {
                        HOperatorSet.SetColor(m_hWindow_Vision.ViewHwindow, "green");
                        HOperatorSet.GetShapeModelContours(out ModelContours, m_ModelID_xld, 1);
                        //根据匹配的中心和角度，计算变换矩阵
                        HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row, hv_Column, hv_Angle, out HomMate2DModel);
                        //将轮廓经过变换矩阵映射到原始位置
                        HOperatorSet.AffineTransContourXld(ModelContours, out ho_ModelContoursTrans, HomMate2DModel);
                        //在匹配物体的中心画十字
                        HOperatorSet.GenCrossContourXld(out cross, hv_Row, hv_Column, new HTuple(60), hv_Angle);                   
                        HOperatorSet.DispObj(ho_ModelContoursTrans, m_hWindow_Vision.ViewHwindow);
                        HOperatorSet.DispObj(cross, m_hWindow_Vision.ViewHwindow);
                        HOperatorSet.SetColor(m_hWindow_Vision.ViewHwindow, "blue");
                        HOperatorSet.VectorAngleToRigid(RowModel, CloumnModel, 0, hv_Row, hv_Column, hv_Angle, out hv_HomMat2DIdentity);
                        for (int i = 0; i < 3; i++)
                        {
                            if (i == 1) FollowRegion = FollowRegion2;
                            if (i == 2) FollowRegion = FollowRegion3;  
                            HOperatorSet.AffineTransRegion(FollowRegion, out RegionFollowed, hv_HomMat2DIdentity, "nearest_neighbor");
                            HOperatorSet.SetDraw(m_hWindow_Vision.ViewHwindow, "margin");
                            HOperatorSet.DispObj(RegionFollowed, m_hWindow_Vision.ViewHwindow);
                            HOperatorSet.ReduceDomain(hImage, RegionFollowed, out OCRReduced);
                            HOperatorSet.Threshold(OCRReduced, out RegionThreshold, new HTuple(_BackgroundThresholdMin), new HTuple(_BackgroundThresholdMax));
                            HOperatorSet.AreaCenter(RegionThreshold, out Checkarea, out Checkrow, out Checkcolumn);
                            HOperatorSet.Emphasize(OCRReduced, out ImageEmphasize, 2592, 1944, 1.2);
                            if (Checkarea > 30000)
                            {
                                HOperatorSet.InvertImage(ImageEmphasize, out ImageInvert);
                                HOperatorSet.Threshold(ImageInvert, out RegionThreshold, new HTuple(_BlackThresholdMin), new HTuple(_BlackThresholdMax));
                                RegionErosion=RegionThreshold;
                            }
                            else
                            {
                                HOperatorSet.Threshold(ImageEmphasize, out RegionThreshold, new HTuple(_WriteThresholdMin), new HTuple(_WriteThresholdMax));
                                HOperatorSet.DilationCircle(RegionThreshold, out RegionDilation, 2);
                                HOperatorSet.ErosionRectangle1(RegionDilation, out RegionErosion, 0.5, 0.5);
                            }
                            HOperatorSet.Connection(RegionErosion, out RegionConnection);
                            HOperatorSet.PartitionRectangle(RegionConnection, out RegionPartitioned, new HTuple(_PartWidth), new HTuple(_PartHight));
                            HOperatorSet.SelectShape(RegionPartitioned, out RegionSelectedArea, "area", "and", new HTuple(_AreaMin), new HTuple(_AreaMax));
                            HOperatorSet.SelectShape(RegionSelectedArea, out RegionSelectedWidth, "width", "and", new HTuple(_WidthMin), new HTuple(_WidthMax));
                            HOperatorSet.SelectShape(RegionSelectedWidth, out RegionSelectedHight, "height", "and", new HTuple(_HightMin), new HTuple(_HightMax));
                            HOperatorSet.SortRegion(RegionSelectedHight, out SortRegions, "character", "true", "row");
                            HOperatorSet.ReadOcrClassSvm(_VisionConfigPath + "OCR.omc", out OCRHandle);
                            HOperatorSet.DoOcrMultiClassSvm(SortRegions, ImageEmphasize, OCRHandle, out hv_Class);
                            HOperatorSet.TupleSum(hv_Class, out hv_sum);
                            if (hv_sum.ToString().Contains("..")) hv_sum = hv_sum.ToString().Replace("..",":");
                            OCR_Tuple[i] = hv_sum;
                        }                     
                    _CenterRow = hv_Row;
                    _CenterColumn = hv_Column;
                    _CenterAngle = hv_Angle;
                    _Score = hv_Score;
                    errMsg = "模板匹配成功";
                    _OCRResult = OCR_Tuple.ToString();
                    _IsOk = true;
                }
                else
                {
                    errMsg = "模板匹配失败";
                    _OCRResult = "not found";
                    _IsOk = false;
                }
            }
            catch (HalconException ex)
            {
                errMsg = ex.Message;
                _IsOk = false;
                OnFinished(this, new VisionEventArgs(0, false));
                return false;
            }
            ModelContours.Dispose();
            ho_ModelContoursTrans.Dispose();
            cross.Dispose();
            imageReduced.Dispose();
            RegionFollowed.Dispose();
            OCRReduced.Dispose();
            ImageEmphasize.Dispose();
            ImageInvert.Dispose();
            RegionThreshold.Dispose();
            RegionDilation.Dispose();
            RegionErosion.Dispose();
            RegionConnection.Dispose();
            RegionPartitioned.Dispose();
            RegionSelectedArea.Dispose();
            RegionSelectedHight.Dispose();
            RegionSelectedWidth.Dispose();
            SortRegions.Dispose();
            OnFinished(this, new VisionEventArgs(0, _IsOk));
            return _IsOk;
        }

        private void ResetResult()
        {
            _CenterRow = 0;
            _CenterColumn = 0;
            _CenterAngle = 0;
            _Score = 0;
            errMsg = "Null";
            _OCRResult = "";
            _IsOk = false;
        }
        //创建模板
        public bool CreateShapeModel(HObject hImage, HTuple m_hWindow_Vision)
        {
            _IsCreateModel = false;
            HObject ImageReduce;//模板区域图像
            HObject modelContours;
            HRegion CreatRegion;
            if (_BorderShapeModel)
                HOperatorSet.SetSystem("border_shape_models", "true");
            else
                HOperatorSet.SetSystem("border_shape_models", "false");
            try
            {
                CreatRegion = ROICreateCtrl.GetRegion();
                if (CreatRegion != null)
                    HOperatorSet.ReduceDomain(hImage, CreatRegion, out ImageReduce);
                else
                { ImageReduce = hImage; }
                HOperatorSet.SetDraw(m_hWindow_Vision, "margin");

                //创建模板及匹配模板
                HTuple HomMate2DModel;
                if (_Contrast == "auto" || _MinContrast == "auto")
                {
                    _Contrast = "auto"; _MinContrast = "auto";
                    HOperatorSet.CreateShapeModel(ImageReduce, new HTuple("auto"), new HTuple(-3.14), new HTuple(3.14), new HTuple("auto"),
                        new HTuple(_Optimization), new HTuple(_Metric), new HTuple(_Contrast), new HTuple(_MinContrast), out m_ModelID_xld);
                }
                else
                    HOperatorSet.CreateShapeModel(ImageReduce, new HTuple("auto"), new HTuple(-3.14), new HTuple(3.14), new HTuple("auto"),
                   new HTuple(_Optimization), new HTuple(_Metric), new HTuple(Convert.ToInt32(_Contrast)), new HTuple(Convert.ToInt32(_MinContrast)), out m_ModelID_xld);

                HTuple hv_Row, hv_Column, hv_Angle, hv_Score;
                HOperatorSet.FindShapeModel(hImage, m_ModelID_xld, new HTuple(_AngleStart), new HTuple(_AngleExtent),
                   new HTuple(_MinScore), new HTuple(1), new HTuple(_MaxOverLap), new HTuple(_subPixel), new HTuple(_NumLevels), new HTuple(_Greediness), out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                HTuple Length1;
                HOperatorSet.TupleLength(hv_Score, out Length1);
                if (Length1 != 0)
                {
                    HObject cross, ho_ModelContoursTrans;
                    //画出十字心
                    HOperatorSet.GenCrossContourXld(out cross, hv_Row, hv_Column, new HTuple(60), hv_Angle);
                    HOperatorSet.VectorAngleToRigid(new HTuple(0), new HTuple(0), new HTuple(0), hv_Row, hv_Column, hv_Angle, out HomMate2DModel);
                    HOperatorSet.GetShapeModelContours(out modelContours, m_ModelID_xld, 1);
                    HOperatorSet.AffineTransContourXld(modelContours, out ho_ModelContoursTrans, HomMate2DModel);
                    //绘制轮廓及中心
                    HOperatorSet.SetColor(m_hWindow_Vision, "green");
                    HOperatorSet.DispObj(ho_ModelContoursTrans, m_hWindow_Vision);
                    HOperatorSet.SetColor(m_hWindow_Vision, "blue");
                    HOperatorSet.DispObj(cross, m_hWindow_Vision);
                    _CenterRow = hv_Row;
                    _CenterColumn = hv_Column;
                    _CenterAngle = hv_Angle;
                    _Score = hv_Score;
                    errMsg = "创建模板成功";
                    _IsCreateModel = true;
                    _IsOk = true;
                    return true;
                }
                else
                {
                    errMsg = "创建模板失败";
                    return false;
                }
            }
            catch (HalconException ex)
            {
                errMsg = "创建模板失败:" + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 保存模板的路径
        /// </summary>
        /// <param name="SavePath">路径，不含文件名</param>
        /// <returns></returns>
        public bool SaveModel(string SavePath)
        {
            try
            {
                if (0 == m_ModelID_xld.Length)
                {
                    return false;
                }
                HOperatorSet.WriteShapeModel(m_ModelID_xld, SavePath +"\\"+ _ModelName);
                _IsCreateModel = true;
                return true;
            }
            catch (HalconException ex)
            {
                _IsCreateModel = false;
                return false;
            }

        }
        public bool ReadModel(string LoadPath)
        {
            try
            {
                if (File.Exists(LoadPath + "\\" + _ModelName))
                {
                    HOperatorSet.ReadShapeModel(LoadPath + "\\"+_ModelName, out m_ModelID_xld);
                    _IsCreateModel = true;
                    return true;
                }
                else
                    _IsCreateModel = false;
            }
            catch (HalconException ex)
            {
                _IsCreateModel = false;
                return false;
            }
            return false;
        }
          public void DelModel(string nPath)
        {
            if (File.Exists(nPath + _ModelName))
                File.Delete(nPath + _ModelName);
        }
        private void OnFinished(object sender, VisionEventArgs e)
        {
            if (OnRunFinished != null)
                OnRunFinished(sender, e);
        }

        #region 参数
        int _NumLevels = 1;
        [Category("搜索参数"), Description("金字塔级别;values: 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10")]
        public int NumLevels
        {
            get { return _NumLevels; }
            set { _NumLevels = value; }
        }
        double _AngleStart = -0.39;
        [Category("搜索参数"), Description("起始角度")]
        public double AngleStart
        {
            get { return _AngleStart * 180 / Math.PI; }
            set { _AngleStart = value * Math.PI / 180; }
        }
        double _AngleExtent = 1.57;
        [Category("搜索参数"), Description("角度范围>0")]
        public double AngleExtent
        {
            get { return _AngleExtent * 180 / Math.PI; }
            set { _AngleExtent = value * Math.PI / 180; }
        }
        double _AngleStep = 0.0175;

        [Category("搜索参数"), Description("步长（分辨率）(AngleStep >= 0) && (AngleStep <= (180 / 16)")]
        public double AngleStep
        {
            get { return _AngleStep * 180 / Math.PI; }
            set { _AngleStep = value * Math.PI / 180; }
        }

        double _MinScore = 0.8;
        [Category("搜索参数"), Description("最小分数;0 ≤ MinScore ≤ 1;最小增量: 0.01;推荐增量: 0.05")]
        public double MinScore
        {
            get { return _MinScore; }
            set
            {
                if (value > 1) value = 1;
                if (value < 0) value = 0;
                _MinScore = value;
            }
        }

        double _MaxOverLap = 0.5;
        /// <summary>
        /// Maximum overlap of the instances of the model to be found.
        ///Default value: 0.5
        ///Suggested values: 0.0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0
        ///Typical range of values: 0 ≤ MaxOverlap ≤ 1
        ///最小增量: 0.01
        ///推荐增量: 0.05
        /// </summary>
        [Category("搜索参数"), Description("重叠率;0 ≤ MaxOverlap ≤ 1")]
        public double MaxOverlap
        {
            get { return _MaxOverLap; }
            set { _MaxOverLap = value; }
        }
        string _subPixel = "least_squares";
        /// <summary>
        /// Subpixel accuracy if not equal to 'none'.
        ///Default value: 'least_squares'
        ///List of values: 'interpolation', 'least_squares', 'least_squares_high', 
        ///'least_squares_very_high', 'max_deformation 1', 'max_deformation 2', 
        ///'max_deformation 3', 'max_deformation 4', 'max_deformation 5', 'max_deformation 6', 'none'
        /// </summary>
        [Category("搜索参数"), TypeConverter(typeof(SubPixelItem)), Description("亚像素精度;")]
        public string SubPixel
        {
            get { return _subPixel; }
            set { _subPixel = value; }
        }
        double _Greediness = 0.85;
        /// <summary>
        /// “Greediness” of the search heuristic (0: safe but slow; 1: fast but matches may be missed).
        ///Default value: 0.9
        ///Suggested values: 0.0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0
        ///Typical range of values: 0 ≤ Greediness ≤ 1
        ///最小增量: 0.01
        ///推荐增量: 0.05
        /// </summary>
        [Category("搜索参数"), Description("搜寻模板定位加速:0 ≤ Greediness ≤ 1;0: 慢但是安全; 1: 快但是匹配数量可能丢失;建议0.7-0.9")]
        public double Greediness
        {
            get { return _Greediness; }
            set { _Greediness = value; }
        }
        string _Optimization = "auto";
        [Category("模板参数"), TypeConverter(typeof(OptimizationItem)), Description("优化算法;")]
        public string Optimization
        {
            get { return _Optimization; }
            set { _Optimization = value; }
        }

        string _Metric = "use_polarity";
        [Category("模板参数"), TypeConverter(typeof(MetricItem)), Description("极性模式")]
        public string Metric
        {
            get { return _Metric; }
            set { _Metric = value; }
        }

        string _Contrast = "auto";
        [Category("模板参数"), TypeConverter(typeof(ContrastItem)), Description("对比度")]
        public string Contrast
        {
            get { return _Contrast; }
            set { _Contrast = value; }
        }
        string _MinContrast = "auto";
        [Category("模板参数"), TypeConverter(typeof(MinContrastItem)), Description("最小对比度")]
        public string MinContrast
        {
            get { return _MinContrast; }
            set { _MinContrast = value; }
        }
        bool _IsCreateModel = false;
        [Category("模板参数"), Description("是否已创建模板")]
        public bool IsCreateModel
        {
            get { return _IsCreateModel; }
            set
            {
                if (value)
                {
                    if (!ReadModel(_VisionConfigPath))
                    {
                        value = false;
                    }
                }
                else
                {
                    //DelModel(_VisionConfigPath);
                }
                _IsCreateModel = value;
            }
        }
        string _ModelName = "TemplateOCR";
        [Category("模板参数"), Description("模板保存名称")]
        public string ModelName
        {
            get { return _ModelName; }
            set
            {
                _ModelName = value;
            }
        }
        double _BackgroundThresholdMin = 0;
        [Category("OCR参数"), Description("阈值最小值范围>0")]
        public double BackgroundThresholdMin
        {
            get { return _BackgroundThresholdMin; }
            set { _BackgroundThresholdMin = value; }
        }
        double _BackgroundThresholdMax = 50;
        [Category("OCR参数"), Description("阈值最大值范围<255")]
        public double BackgroundThresholdMax
        {
            get { return _BackgroundThresholdMax; }
            set { _BackgroundThresholdMax = value; }
        }
        double _BlackThresholdMin = 100;
        [Category("OCR参数"), Description("阈值最小值范围>0")]
        public double BlackThresholdMin
        {
            get { return _BlackThresholdMin; }
            set { _BlackThresholdMin = value; }
        }
        double _BlackThresholdMax = 200;
        [Category("OCR参数"), Description("阈值最大值范围<255")]
        public double BlackThresholdMax
        {
            get { return _BlackThresholdMax; }
            set { _BlackThresholdMax = value; }
        }
        double _WriteThresholdMin = 0;
        [Category("OCR参数"), Description("阈值最小值范围>0")]
        public double WriteThresholdMin
        {
            get { return _WriteThresholdMin; }
            set { _WriteThresholdMin = value; }
        }
        double _WriteThresholdMax = 50;
        [Category("OCR参数"), Description("阈值最大值范围<255")]
        public double WriteThresholdMax
        {
            get { return _WriteThresholdMax; }
            set { _WriteThresholdMax = value; }
        }
        double _PartWidth = 50;
        [Category("OCR参数"), Description("面积最小值范围>0")]
        public double PartWidth
        {
            get { return _PartWidth; }
            set { _PartWidth = value; }
        }
        double _PartHight = 80;
        [Category("OCR参数"), Description("面积最小大值范围<99999")]
        public double PartHight
        {
            get { return _PartHight; }
            set { _PartHight = value; }
        }
        double _AreaMin = 20;
        [Category("OCR参数"), Description("面积最小值范围>0")]
        public double AreaMin
        {
            get { return _AreaMin; }
            set { _AreaMin = value; }
        }
        double _AreaMax = 2000;
        [Category("OCR参数"), Description("面积最小大值范围<99999")]
        public double AreaMax
        {
            get { return _AreaMax; }
            set { _AreaMax = value; }
        }
        double _HightMin = 10;
        [Category("OCR参数"), Description("高度最小值范围>0")]
        public double HightMin
        {
            get { return _HightMin; }
            set { _HightMin = value; }
        }
        double _HightMax = 500;
        [Category("OCR参数"), Description("高度最大值范围<99999")]
        public double HightMax
        {
            get { return _HightMax; }
            set { _HightMax = value; }
        }
        double _WidthMin = 10;
        [Category("OCR参数"), Description("宽度最小值范围>0")]
        public double WidthMin
        {
            get { return _WidthMin; }
            set { _WidthMin = value; }
        }
        double _WidthMax = 500;
        [Category("OCR参数"), Description("宽度最大值范围<99999")]
        public double WidthMax
        {
            get { return _WidthMax; }
            set { _WidthMax = value; }
        }

        bool _BorderShapeModel = false;
        [Category("运行参数"), Description("false:模板必须在ROI内;true:模板可以部分在ROI外面")]
        public bool BorderShapeModel
        {
            get { return _BorderShapeModel; }
            set { _BorderShapeModel = value; }
        }
        #endregion
        #region 结果
        int _TemplateCount;
        [Category("结果"), Description("匹配数量"), XmlIgnore]
        public int TemplateCount
        {
            get { return _TemplateCount; }
        }

        double _CenterColumn;
        [Category("结果"), Description("中心点X"), XmlIgnore]
        public double CenterColumn
        {
            get { return _CenterColumn; }
        }
        double _CenterRow;
        [Category("结果"), Description("中心点Y"), XmlIgnore]
        public double CenterRow
        {
            get { return _CenterRow; }
        }
        double _CenterAngle;
        [Category("结果"), Description("角度"), XmlIgnore]
        public double CenterAngle
        {
            get { return _CenterAngle; }
        }
        double _Score;
        [Category("结果"), Description("得分"), XmlIgnore]
        public double Score
        {
            get { return _Score; }
        }
        string _OCRResult;
        [Category("结果"), Description("字符识别结果"), XmlIgnore]
        public string OCRResult
        {
            get { return _OCRResult; }
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
        private string _VisionConfigPath = Application.StartupPath + "\\VisionConfig\\";
    }
}
