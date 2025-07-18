
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
    public class VisionCircle : IVisionTool
    {
        public ROI ROIFindCtrl;
        public VisionCircle(ROI MYROI)
        {
            if (ROIFindCtrl == null)
                ROIFindCtrl = MYROI;
            ToolName = "找圆工具";
            ReName = ToolName;
            visionToolType = VisionToolType.VisionCircle;
            VisionType = VisionType.测量类;

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
            _CenterColumn = 0;
            _CenterRow = 0;
            _Radius = 0;
            _Score = 0;
            errMsg = "Null";
            _IsOk = false;
        }
        public override bool RunVision(HObject nImage, HaGUI m_hWindow_Vision)
        {
            // Local iconic variables 
            ResetResult();
            HTuple hv_FindColCenter = new HTuple(0);
            HTuple hv_FindRowCenter = new HTuple(0);
            HTuple hv_FindRadius = new HTuple(10);
            HTuple hv_FindStartPi = new HTuple(0);
            HTuple hv_FindEndPi = new HTuple(0);
            HTuple dataroi;
            if (ROIFindCtrl!= null)
            {
                if (ROIFindCtrl is ROICircle)
                    dataroi = ((ROICircle)ROIFindCtrl).getModelData();//midR, midC, radius
                else if (ROIFindCtrl is ROICircularArc)
                    dataroi = ((ROICircularArc)ROIFindCtrl).getModelData();//midR, midC, radius, startPhi, extentPhi
                else
                    dataroi = new HTuple();

            }
            else
            {
                return false;
            }
            if (dataroi.Length > 2)
            {
                hv_FindColCenter = dataroi[1];
                hv_FindRowCenter = dataroi[0];
                hv_FindRadius = dataroi[2];
                if (dataroi.Length > 4)
                {
                    hv_FindStartPi = dataroi[3];
                    hv_FindEndPi = dataroi[4];
                }
            }

            HObject ho_Contours, ho_Cross, ho_Contour, ho_Circle, ho_CircleCenterCross;
            // Local control variables 
            HTuple hv_Width = null, hv_Height = null;
            HTuple hv_Index1 = null, hv_Row = null;
            HTuple hv_Col = null, hv_Radius = null, hv_Score = null;
            HTuple hv_Row3 = null, hv_Column3 = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            HOperatorSet.GenEmptyObj(out ho_Contour);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out ho_CircleCenterCross);

            HOperatorSet.GetImageSize(nImage, out hv_Width, out hv_Height);

            //HTuple rows, columns;


            HOperatorSet.CreateMetrologyModel(out MetrologyHandle);
            HOperatorSet.SetMetrologyModelImageSize(MetrologyHandle, hv_Width, hv_Height);

            HOperatorSet.AddMetrologyObjectCircleMeasure(MetrologyHandle, hv_FindRowCenter, hv_FindColCenter, hv_FindRadius, _MeasureLength1, _MeasureLength2, _MeasureSigma, _MeasureThreshold,
                (new HTuple("measure_transition")).TupleConcat("min_score").TupleConcat("measure_select").TupleConcat("start_phi").TupleConcat("end_phi"), (new HTuple(_MeasureTransition)).TupleConcat(_MeasureMinScore).TupleConcat(_MeasureSelect).TupleConcat(hv_FindStartPi).TupleConcat(hv_FindEndPi), out hv_Index1);
            //Apply the metrology model to the reference image and read out the results.
            HOperatorSet.ApplyMetrologyModel(nImage, MetrologyHandle);
            HOperatorSet.GetMetrologyObjectResult(MetrologyHandle, "all", "all", "result_type", "row", out hv_Row);
            HOperatorSet.GetMetrologyObjectResult(MetrologyHandle, "all", "all", "result_type", "column", out hv_Col);
            HOperatorSet.GetMetrologyObjectResult(MetrologyHandle, "all", "all", "result_type", "radius", out hv_Radius);
            HOperatorSet.GetMetrologyObjectResult(MetrologyHandle, "all", "all", "result_type", "score", out hv_Score);

            ho_Contours.Dispose();
            HOperatorSet.GetMetrologyObjectMeasures(out ho_Contours, MetrologyHandle, "all", _MeasureTransition, out hv_Row3, out hv_Column3);
            HOperatorSet.SetColor(m_hWindow_Vision.MyWindow, "blue");
            HOperatorSet.DispObj(ho_Contours, m_hWindow_Vision.MyWindow);

            if (hv_Col.Length > 0)
            {
                _CenterColumn = hv_Col;
                _CenterRow = hv_Row;
                _Radius = hv_Radius;
                ho_Cross.Dispose();
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Row, hv_Col, hv_Radius);
                HOperatorSet.GenCrossContourXld(out ho_Cross, hv_Row3, hv_Column3, 8, 0.785398);
                HOperatorSet.GenCrossContourXld(out ho_CircleCenterCross, hv_Row, hv_Col, 40, 0);
                HOperatorSet.SetColor(m_hWindow_Vision.MyWindow, "green");
                HOperatorSet.DispObj(ho_Cross, m_hWindow_Vision.MyWindow);
                HOperatorSet.DispObj(ho_Circle, m_hWindow_Vision.MyWindow);
                HOperatorSet.DispObj(ho_CircleCenterCross, m_hWindow_Vision.MyWindow);
                HOperatorSet.SetColor(m_hWindow_Vision.MyWindow, "blue");
                HOperatorSet.GenContourPolygonXld(out ho_Contour, hv_Row, hv_Col);
                HOperatorSet.DispObj(ho_Contour, m_hWindow_Vision.MyWindow);
                errMsg = "找圆成功";
                _Score = hv_Score;
                _IsOk = true;
            }
            else
            {
                errMsg = "找圆失败";
                _IsOk = false;
            }

            HOperatorSet.ClearMetrologyModel(MetrologyHandle);
            ho_Contours.Dispose();
            ho_Cross.Dispose();
            ho_Contour.Dispose();

            return _IsOk;
        }
 
        #region 结果

        double _CenterColumn;
        [Category("结果"), Description("圆心X"), XmlIgnore]
        public double CenterColumn
        {
            get { return _CenterColumn; }
        }
        double _CenterRow;
        [Category("结果"), Description("圆心Y"), XmlIgnore]
        public double CenterRow
        {
            get { return _CenterRow; }
        }
        double _Radius;
        [Category("结果"), Description("半径"), XmlIgnore]
        public double Radius
        {
            get { return _Radius; }
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
        int _MeasureLength1 = 40;
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
