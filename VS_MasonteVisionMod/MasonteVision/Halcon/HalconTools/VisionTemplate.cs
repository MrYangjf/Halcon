
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
    [Serializable]
    public class VisionTemplate : IVisionTool
    {

        public ROI ROIFindCtrl;
        HTuple m_ModelID_xld;
        public VisionTemplate(ROI MyRoi)
        {

            if (ROIFindCtrl == null)
                ROIFindCtrl = MyRoi;

            ToolName = "模板匹配";
            ReName = ToolName;
            visionToolType = VisionToolType.VisionTemplate;
            VisionType = VisionType.模板匹配;
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
        public override bool RunVision(HObject hImage, HaGUI m_hWindow_Vision)
        {
            ResetResult();

            if (!_IsCreateModel)
            {

                return false;
            }
            if (_BorderShapeModel)
                HOperatorSet.SetSystem("border_shape_models", "true");
            else
                HOperatorSet.SetSystem("border_shape_models", "false");

            HObject ModelContours, ho_ModelContoursTrans, cross, imageReduced;
            HOperatorSet.GenEmptyObj(out ModelContours);
            HOperatorSet.GenEmptyObj(out ho_ModelContoursTrans);
            HOperatorSet.GenEmptyObj(out cross);
            HOperatorSet.GenEmptyObj(out imageReduced);
            HTuple hv_Row, hv_Column, hv_Angle, hv_Score;
            HRegion FindRegion = ROIFindCtrl.getRegion();

            try
            {
                if (FindRegion != null)
                    HOperatorSet.ReduceDomain(hImage, FindRegion, out imageReduced);
                else
                    imageReduced = hImage;
                //模板匹配
                HOperatorSet.FindShapeModel(imageReduced, m_ModelID_xld, new HTuple(_AngleStart), new HTuple(_AngleExtent),
                    new HTuple(_MinScore), new HTuple(_NumMatches), new HTuple(_MaxOverLap), new HTuple(_subPixel), new HTuple(_NumLevels), new HTuple(_Greediness), out hv_Row,
                    out hv_Column, out hv_Angle, out hv_Score);

                _TemplateCount = hv_Column.TupleLength();
                //检测是否找到模板
                if (_TemplateCount > 0)
                {
                    if (_TemplateCount == 1)
                        HOperatorSet.SetColor(m_hWindow_Vision.MyWindow, "green");
                    else
                        HOperatorSet.SetColor(m_hWindow_Vision.MyWindow, "red");
                    //绘制找到的模板
                    for (int i = 0; i < _TemplateCount; i++)
                    {
                        HOperatorSet.GetShapeModelContours(out ModelContours, m_ModelID_xld, 1);
                        //根据匹配的中心和角度，计算变换矩阵
                        HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row[i], hv_Column[i], hv_Angle[i], out VisionHelper.HomMate2DModel);
                        //将轮廓经过变换矩阵映射到原始位置
                        HOperatorSet.AffineTransContourXld(ModelContours, out ho_ModelContoursTrans, VisionHelper.HomMate2DModel);
                        //在匹配物体的中心画十字
                        HOperatorSet.GenCrossContourXld(out cross, hv_Row[i], hv_Column[i], new HTuple(60), hv_Angle[i]);
                        HOperatorSet.SetColor(m_hWindow_Vision.MyWindow, "green");
                        HOperatorSet.DispObj(ho_ModelContoursTrans, m_hWindow_Vision.MyWindow);
                        HOperatorSet.DispObj(cross, m_hWindow_Vision.MyWindow);
                    }
                    _CenterRow = hv_Row[0];
                    _CenterColumn = hv_Column[0];
                    _CenterAngle = hv_Angle[0];
                    _Score = hv_Score[0];
                    errMsg = "模板匹配成功";
                    _IsOk = true;
                }
                else
                {
                    errMsg = "模板匹配失败";
                    _IsOk = false;
                }
            }
            catch (HalconException ex)
            {
                errMsg = ex.Message;
                _IsOk = false;

                return false;
            }
            ModelContours.Dispose();
            ho_ModelContoursTrans.Dispose();
            cross.Dispose();
            imageReduced.Dispose();
            return _IsOk;
        }

        private void ResetResult()
        {
            _CenterRow = 0;
            _CenterColumn = 0;
            _CenterAngle = 0;
            _Score = 0;
            errMsg = "Null";
            _IsOk = false;
        }
        //创建模板
        public bool CreateShapeModel(HObject hImage, HaGUI m_hWindow_Vision)
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
                CreatRegion = ROIFindCtrl.getRegion();

                if (CreatRegion != null)
                    HOperatorSet.ReduceDomain(hImage, CreatRegion, out ImageReduce);
                else
                { ImageReduce = hImage; }
                HOperatorSet.SetDraw(m_hWindow_Vision.MyWindow, "margin");

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
                    HOperatorSet.SetColor(m_hWindow_Vision.MyWindow, "green");
                    HOperatorSet.DispObj(ho_ModelContoursTrans, m_hWindow_Vision.MyWindow);
                    HOperatorSet.SetColor(m_hWindow_Vision.MyWindow, "blue");
                    HOperatorSet.DispObj(cross, m_hWindow_Vision.MyWindow);
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
        public bool SaveModel()
        {
            try
            {
                if (0 == m_ModelID_xld.Length)
                {
                    return false;
                }
                if (!Directory.Exists(_VisionConfigPath))
                { Directory.CreateDirectory(_VisionConfigPath); }
                HOperatorSet.WriteShapeModel(m_ModelID_xld, _VisionConfigPath + "\\" + _ModelName);
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
                    HOperatorSet.ReadShapeModel(LoadPath + "\\" + _ModelName, out m_ModelID_xld);
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
        int _NumMatches = 1;
        /// <summary>
        ///Number of instances of the model to be found (or 0 for all matches).
        ///Default value: 1
        ///Suggested values: 0, 1, 2, 3, 4, 5, 10, 20
        /// </summary>
        [Category("搜索参数"), Description("匹配的数量(0表示所有);Suggested values: 0, 1, 2, 3, 4, 5, 10, 20")]
        public int NumMatches
        {
            get { return _NumMatches; }
            set { _NumMatches = value; }
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

        public string Optimization
        {
            get { return _Optimization; }
            set { _Optimization = value; }
        }

        string _Metric = "use_polarity";

        public string Metric
        {
            get { return _Metric; }
            set { _Metric = value; }
        }

        string _Contrast = "auto";

        public string Contrast
        {
            get { return _Contrast; }
            set { _Contrast = value; }
        }
        string _MinContrast = "auto";

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
        string _ModelName = "Template1";
        [Category("模板参数"), Description("模板保存名称")]
        public string ModelName
        {
            get { return _ModelName; }
            set
            {
                _ModelName = value;
            }
        }

        bool _BorderShapeModel = false;
        [Category("运行参数"), Description("false:模板必须在ROI内;true:模板可以部分在ROI外面")]
        public bool BorderShapeModel
        {
            get { return _BorderShapeModel; }
            set { _BorderShapeModel = value; }
        }
        #endregion

        #region 基准点坐标
        [Category("模板基准点"), Description("基准点X坐标"), DisplayName("基准点X坐标")]
        public double StandardX
        {
            get; set;
        }
        [Category("模板基准点"), Description("基准点Y坐标"), DisplayName("基准点Y坐标")]
        public double StandardY
        {
            get; set;
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
