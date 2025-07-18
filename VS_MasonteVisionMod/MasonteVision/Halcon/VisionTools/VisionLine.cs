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
    public class VisionLine : IVisionTool
    {
        public event VisionHandle OnRunFinished;
        public ROIController ROIFindCtrl;
        public VisionLine()
        {
            if (ROIFindCtrl == null)
                ROIFindCtrl = new ROIController();
            ToolName = "找直线";
            ReName = ToolName;
            visionToolType = VisionToolType.VisionLine;
            VisionType = VisionType.精准定位;
            IcoImage = global::HalconVision.Properties.Resources.camera;
        }
        public override void SettingEnter()
        {
            ToolSettingForm f = new ToolSettingForm(this);
            f.Show();
        }
        private void ResetResult()
        {
            _BeginColumn = 0;
            _BeginRow = 0;
            _EndColumn = 0;
            _EndRow = 0;
            _Score = 0;
            errMsg = "Null";
            _IsOk = false;
        }
        public override bool RunVision(HObject nImage, ViewDisplayCtrl m_hWindow_Vision)
        {
            // Local iconic variables 
            ResetResult();
            HTuple hv_FindColBegin = new HTuple(0);
            HTuple hv_FindRowBegin = new HTuple(0);
            HTuple hv_FindColEnd = new HTuple(0);
            HTuple hv_FindRowEnd = new HTuple(0);
            HTuple dataroi;
            if (ROIFindCtrl.ROIList.Count > 0)
                dataroi = ((ROILine)ROIFindCtrl.ROIList[0]).getModelData();//row1, col1, row2, col2
            else
            {
                return false;
            }
            if (dataroi.Length == 4)
            {
                hv_FindColBegin = dataroi[1];
                hv_FindRowBegin = dataroi[0];
                hv_FindColEnd = dataroi[3];
                hv_FindRowEnd = dataroi[2];
            }
            HObject ho_Contours, ho_Cross, ho_Contour;
            // Local control variables 
            HTuple hv_Width = null, hv_Height = null;
            HTuple hv_Index1 = null, hv_RowBegin = null;
            HTuple hv_ColBegin = null, hv_RowEnd = null, hv_ColEnd = null, hv_Score = null;
            HTuple hv_Row3 = null, hv_Column3 = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Contour);

            HOperatorSet.GetImageSize(nImage, out hv_Width, out hv_Height);

            //HTuple rows, columns;


            HOperatorSet.CreateMetrologyModel(out MetrologyHandle);
            HOperatorSet.SetMetrologyModelImageSize(MetrologyHandle, hv_Width, hv_Height);

            HOperatorSet.AddMetrologyObjectLineMeasure(MetrologyHandle, hv_FindRowBegin, hv_FindColBegin, hv_FindRowEnd, hv_FindColEnd, _MeasureLength1, _MeasureLength2, _MeasureSigma, _MeasureThreshold,
                (new HTuple("measure_transition")).TupleConcat("min_score").TupleConcat("measure_select"), (new HTuple(_MeasureTransition)).TupleConcat(_MeasureMinScore).TupleConcat(_MeasureSelect), out hv_Index1);
            //Apply the metrology model to the reference image and read out the results.
            HOperatorSet.ApplyMetrologyModel(nImage, MetrologyHandle);
            HOperatorSet.GetMetrologyObjectResult(MetrologyHandle, "all", "all", "result_type", "row_begin", out hv_RowBegin);
            HOperatorSet.GetMetrologyObjectResult(MetrologyHandle, "all", "all", "result_type", "column_begin", out hv_ColBegin);
            HOperatorSet.GetMetrologyObjectResult(MetrologyHandle, "all", "all", "result_type", "row_end", out hv_RowEnd);
            HOperatorSet.GetMetrologyObjectResult(MetrologyHandle, "all", "all", "result_type", "column_end", out hv_ColEnd);
            HOperatorSet.GetMetrologyObjectResult(MetrologyHandle, "all", "all", "result_type", "score", out hv_Score);

            if (hv_ColBegin.Length == 1)
            {
                _BeginColumn = hv_ColBegin;
                _BeginRow = hv_RowBegin;
                _EndColumn = hv_ColEnd;
                _EndRow = hv_RowEnd;

            }
            else
            {
                errMsg = "找线失败";
                OnFinished(this, new VisionEventArgs(0, false));
                _IsOk = false;
            }

            ho_Contours.Dispose();
            //HOperatorSet.GetMetrologyObjectMeasures(out ho_Contours, MetrologyHandle, "all", "positive", out hv_Row3, out hv_Column3);
            HOperatorSet.GetMetrologyObjectMeasures(out ho_Contours, MetrologyHandle, "all", _MeasureTransition, out hv_Row3, out hv_Column3);
            ho_Cross.Dispose();
            HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row3, hv_Column3, 8, 0.785398);
            HOperatorSet.SetColor(m_hWindow_Vision.ViewHwindow, "blue");
            HOperatorSet.DispObj(ho_Contours, m_hWindow_Vision.ViewHwindow);
            HOperatorSet.SetColor(m_hWindow_Vision.ViewHwindow, "green");
            HOperatorSet.DispObj(ho_Cross, m_hWindow_Vision.ViewHwindow);

            HOperatorSet.TupleConcat(hv_ColBegin, hv_ColEnd, out hv_ColBegin);
            HOperatorSet.TupleConcat(hv_RowBegin, hv_RowEnd, out hv_RowBegin);
            ho_Contour.Dispose();
            if (hv_ColBegin.Length == 2)
            {
                HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_RowBegin, hv_ColBegin);
                HOperatorSet.DispObj(ho_Contour, m_hWindow_Vision.ViewHwindow);
                errMsg = "找线成功";
                _Score = hv_Score;
                _IsOk = true;
            }
            HOperatorSet.ClearMetrologyModel(MetrologyHandle);
            ho_Contours.Dispose();
            ho_Cross.Dispose();
            ho_Contour.Dispose();
            OnFinished(this, new VisionEventArgs(0, true));
            return _IsOk;
        }
        private void OnFinished(object sender, VisionEventArgs e)
        {
            if (OnRunFinished != null)
                OnRunFinished(sender, e);
        }
        #region 结果

        double _BeginColumn;
        [Category("结果"), Description("起点X"), XmlIgnore]
        public double BeginColumn
        {
            get { return _BeginColumn; }
        }
        double _BeginRow;
        [Category("结果"), Description("起点Y"), XmlIgnore]
        public double BeginRow
        {
            get { return _BeginRow; }
        }
        double _EndColumn;
        [Category("结果"), Description("终点X"), XmlIgnore]
        public double EndColumn
        {
            get { return _EndColumn; }
        }
        double _EndRow;
        [Category("结果"), Description("终点Y"), XmlIgnore]
        public double EndRow
        {
            get { return _EndRow; }
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

        #region 参数

        //MetrologyHandle (input_control, state is modified)  metrology_model → (integer)
        //Handle of the metrology model.
        private HTuple MetrologyHandle;

        //RowBegin (input_control)  line.begin.y(-array) → (real / integer)
        //Row (or Y) coordinate of the start of the line.

        //ColumnBegin (input_control)  line.begin.x(-array) → (real / integer)
        //Column (or X) coordinate of the start of the line.

        //RowEnd (input_control)  line.end.y(-array) → (real / integer)
        //Row (or Y) coordinate of the end of the line.

        //ColumnEnd (input_control)  line.end.x(-array) → (real / integer)
        //Column (or X) coordinate of the end of the line.

        //MeasureLength1 (input_control)  number → (real / integer)
        //Half length of the measure regions perpendicular to the boundary.
        //Default value: 20.0
        //Suggested values: 10.0, 20.0, 30.0
        //Typical range of values: 1.0 ≤ MeasureLength1
        //Minimum increment: 1.0
        //Recommended increment: 10.0
        int _MeasureLength1 = 20;
        [Category("参数"), Description("测量区域高度1.0 ≤ MeasureLength1")]
        public int MeasureLength1
        {
            get { return _MeasureLength1; }
            set { _MeasureLength1 = value; }
        }
        //MeasureLength2 (input_control)  number → (real / integer)
        //Half length of the measure regions tangetial to the boundary.
        //Default value: 5.0
        //Suggested values: 3.0, 5.0, 10.0
        //Typical range of values: 1.0 ≤ MeasureLength2
        //Minimum increment: 1.0
        //Recommended increment: 10.0
        int _MeasureLength2 = 1;
        [Category("参数"), Description("测量区域宽度1.0 ≤ MeasureLength2")]
        public int MeasureLength2
        {
            get { return _MeasureLength2; }
            set { _MeasureLength2 = value; }
        }
        double _MeasureSigma = 1;
        //MeasureSigma (input_control)  number → (real / integer)
        //Sigma of the Gaussian function for the smoothing.
        //Default value: 1.0
        //Suggested values: 0.4, 0.6, 0.8, 1.0, 1.5, 2.0, 3.0, 4.0, 5.0, 7.0, 10.0
        //Typical range of values: 0.4 ≤ MeasureSigma
        //Minimum increment: 0.01
        //Recommended increment: 0.1
        //Restriction: (0.4 <= MeasureSigma) && (MeasureSigma <= 100)
        [Category("参数"), Description("平滑(0.4 <= MeasureSigma) && (MeasureSigma <= 100)")]
        public double MeasureSigma
        {
            get { return _MeasureSigma; }
            set { _MeasureSigma = value; }
        }
        int _MeasureThreshold = 40;
        //MeasureThreshold (input_control)  number → (real / integer)
        //Minimum edge amplitude.
        //Default value: 30.0
        //Suggested values: 5.0, 10.0, 20.0, 30.0, 40.0, 50.0, 60.0, 70.0, 90.0, 110.0
        //Typical range of values: 1 ≤ MeasureThreshold ≤ 255 (lin)
        //Minimum increment: 0.5
        //Recommended increment: 2
        [Category("参数"), Description("边沿阈值 1 ≤ MeasureThreshold ≤ 255")]
        public int MeasureThreshold
        {
            get { return _MeasureThreshold; }
            set { _MeasureThreshold = value; }
        }
        string _MeasureTransition = "all";
        [Category("参数"), TypeConverter(typeof(TransitionItem)), Description("极性")]
        public string MeasureTransition
        {
            get { return _MeasureTransition; }
            set { _MeasureTransition = value; }
        }
        double _MeasureMinScore = 0.6;
        [Category("参数"), Description("最小分数")]
        public double MeasureMinScore
        {
            get { return _MeasureMinScore; }
            set { _MeasureMinScore = value; }
        }
        string _MeasureSelect = "all";
        [Category("参数"), TypeConverter(typeof(SelectItem)), Description("端点选择")]
        public string MeasureSelect
        {
            get { return _MeasureSelect; }
            set { _MeasureSelect = value; }
        }
        //GenParamName (input_control)  attribute.name(-array) → (string)
        //Names of the generic parameters.
        //Default value: []
        //List of values: 'distance_threshold', 'instances_outside_measure_regions', 'max_num_iterations', 'measure_distance', 'measure_interpolation', 'measure_select', 'measure_transition', 'min_score', 'num_instances', 'num_measures', 'rand_seed'

        //GenParamValue (input_control)  attribute.value(-array) → (real / integer / string)
        //Values of the generic parameters.
        //Default value: []
        //Suggested values: 1, 2, 3, 4, 5, 10, 20, 'all', 'true', 'false', 'first', 'last', 'positive', 'negative', 'uniform', 'nearest_neighbor', 'bilinear', 'bicubic'
        #endregion
    }
}
