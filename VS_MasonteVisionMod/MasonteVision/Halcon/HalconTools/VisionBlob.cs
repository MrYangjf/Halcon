using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasonteVision.Halcon;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace MasonteVision.Halcon.VisionTool
{
    [Serializable]
    public class VisionBlob : IVisionTool
    {
        
        #region 参数
        int _ThresholdMinGray = 128;
        [Category("参数"), DisplayName("最小灰度值"), Description("Lower threshold for the gray values.ThresholdMinGray<=ThresholdMaxGray")]
        public int ThresholdMinGray
        {
            get { return _ThresholdMinGray; }
            set { _ThresholdMinGray = value; }
        }
        int _ThresholdMaxGray = 255;
        [Category("参数"), DisplayName("最大灰度值"), Description("Upper threshold for the gray values.ThresholdMinGray<=ThresholdMaxGray")]
        public int ThresholdMaxGray
        {
            get { return _ThresholdMaxGray; }
            set { _ThresholdMaxGray = value; }
        }
        int _MarkMaxArea = 999999;
        [Category("参数"), DisplayName("最大面积"), Description("面积过滤最大值")]
        public int MarkMaxArea
        {
            get { return _MarkMaxArea; }
            set { _MarkMaxArea = value; }
        }
        int _MarkMinArea = 99;
        [Category("参数"), DisplayName("最小面积"), Description("面积过滤最小值")]
        public int MarkMinArea
        {
            get { return _MarkMinArea; }
            set { _MarkMinArea = value; }
        }
        double _CircularityMin = 0.8;
        [Category("参数"), DisplayName("圆度最小值"), Description("圆度过滤最小值≥0")]
        public double CircularityMin
        {
            get { return _CircularityMin; }
            set { _CircularityMin = value; }
        }
        double _CircularityMax = 1;
        [Category("参数"), DisplayName("圆度最大值"), Description("圆度过滤最大值≤1")]
        public double CircularityMax
        {
            get { return _CircularityMax; }
            set { _CircularityMax = value; }
        }
        double _ClosingRadius = 3.5;
        [Category("参数"), DisplayName("闭运算半径"), Description("闭运算半径0.5 ≤ Radius ≤ 511.5")]
        public double ClosingRadius
        {
            get { return _ClosingRadius; }
            set { _ClosingRadius = value; }
        }

        bool _FillUp = false;
        [Category("参数"), DisplayName("空洞填充"), Description("是否填充空洞")]
        public bool FillUp
        {
            get { return _FillUp; }
            set { _FillUp = value; }
        }
        #endregion
        #region 结果

        int _BlobCount = 0;
        [Category("结果"), DisplayName("Blob数量"), Description("Blob数量"), XmlIgnore]
        public int BlobCount
        {
            get { return _BlobCount; }
        }

        List<double> _CenterColumn = new List<double>();
        [Category("结果"), DisplayName("中心点X坐标"), Description("中心点X坐标"), XmlIgnore]
        public List<double> CenterColumn
        {
            get { return _CenterColumn; }
        }
        [XmlIgnore]
        List<double> _CenterRow = new List<double>();
        [Category("结果"), DisplayName("中心点Y坐标"), Description("中心点Y坐标"), XmlIgnore]
        public List<double> CenterRow
        {
            get { return _CenterRow; }
        }
        [XmlIgnore]
        List<double> _BlobArea = new List<double>();
        [Category("结果"), DisplayName("Blob面积"), Description("Blob面积"), XmlIgnore]
        public List<double> BlobArea
        {
            get { return _BlobArea; }
        }
        [XmlIgnore]
        string errMsg;
        [Category("结果"), DisplayName("运行信息"), Description("运行结果消息"), XmlIgnore]
        public string ErrMsg
        {
            get { return errMsg; }
        }
        [XmlIgnore]
        bool _IsOk = false;
        [Category("结果"), DisplayName("运行结果"), Description("运行结果"), XmlIgnore]
        public bool IsOK
        {
            get { return _IsOk; }
        }
        #endregion
        #region 判定

        #endregion

        public HObject rectangleROI;
        public ROI ROIFindCtrl;// = new ROIController();
        public VisionBlob(ROI MYROI)
        {
            if (ROIFindCtrl == null)
            {
                ROIFindCtrl = MYROI;
            }
            ToolName = "Blob分析";
            ReName = ToolName;
            visionToolType = VisionToolType.VisionBlob;
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
            _BlobCount = 0;
            _CenterColumn.Clear();
            _CenterRow.Clear();
            _BlobArea.Clear();
            errMsg = "Null";
            _IsOk = false;
        }
        public override bool RunVision(HObject image, HaGUI m_hWindow_Vision)
        {
            HObject /* ho_ImageInvert,*/ ho_Regions, ho_ConnectedRegions, ho_SelectedAreaRegions, ho_SelectedCirRegions, ho_Contours, ho_cross;
            HTuple hv_BlobCount;
            ResetResult();
            //m_hWindow_Vision.MyHWndControl.RemoveIconicVar();
            if (image == null || !image.IsInitialized())
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
            ho_cross = new HObject();

            HOperatorSet.SetColor(m_hWindow_Vision.MyWindow, "green");
            HOperatorSet.SetDraw(m_hWindow_Vision.MyWindow, "margin");
            HObject regionClosing, regionFillUp;
            HOperatorSet.Threshold(rectangleROI, out ho_Regions, _ThresholdMinGray, _ThresholdMaxGray);
            HOperatorSet.ClosingCircle(ho_Regions, out regionClosing, _ClosingRadius);
            HOperatorSet.Connection(regionClosing, out ho_ConnectedRegions);
            if (FillUp)
                HOperatorSet.FillUp(ho_ConnectedRegions, out regionFillUp);
            else
            {
                regionFillUp = ho_ConnectedRegions;
            }
            HOperatorSet.SelectShape(regionFillUp, out ho_SelectedAreaRegions, "area", "and", _MarkMinArea, _MarkMaxArea);
            HOperatorSet.SelectShape(ho_SelectedAreaRegions, out ho_SelectedCirRegions, "circularity", "and", _CircularityMin, _CircularityMax);
            
            HOperatorSet.GenContourRegionXld(ho_SelectedCirRegions, out ho_Contours, "border");
            //HOperatorSet.DispObj(ho_SelectedCirRegions, m_hWindow_Vision.ViewHwindow);
            m_hWindow_Vision.MyHWndControl.addIconicVar(ho_Contours.Clone());

            HOperatorSet.CountObj(ho_Contours, out hv_BlobCount);
            _BlobCount = hv_BlobCount.I;
            HTuple AreaValue, CloumnsValue, RowsValue;
            if (_BlobCount > 0)
            {
                HOperatorSet.RegionFeatures(ho_SelectedCirRegions, new HTuple("area"), out AreaValue);
                HOperatorSet.RegionFeatures(ho_SelectedCirRegions, new HTuple("column"), out CloumnsValue);
                HOperatorSet.RegionFeatures(ho_SelectedCirRegions, new HTuple("row"), out RowsValue);
                HOperatorSet.GenCrossContourXld(out ho_cross, RowsValue, CloumnsValue, 100, 0);
                m_hWindow_Vision.MyHWndControl.addIconicVar(ho_cross.Clone());
                _BlobArea = (AreaValue.DArr).ToList();
                _CenterColumn = (CloumnsValue.DArr).ToList();
                _CenterRow = (RowsValue.DArr).ToList();
                _IsOk = true;
            }
            errMsg = string.Format("找到{0}个满足面积在{1}-{2}，圆度大于{3}的区域", _BlobCount, _MarkMinArea, _MarkMaxArea, _CircularityMin);
            //OnFinished(this, new VisionEventArgs(0, true));
            ho_Regions.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedAreaRegions.Dispose();
            ho_SelectedCirRegions.Dispose();
            ho_Contours.Dispose();
            regionClosing.Dispose();
            regionFillUp.Dispose();
            ho_cross.Dispose();
            return _IsOk;
        }


        //public override bool RunVision(HObject image, ViewDisplayCtrl m_hWindow_Vision)
        //{
        //    HObject rectangleROI,/* ho_ImageInvert,*/ ho_Regions, ho_ConnectedRegions, ho_SelectedAreaRegions, ho_SelectedCirRegions, ho_Contours;
        //    HTuple hv_BlobCount;
        //    ResetResult();
        //    if (image == null)
        //    {
        //        errMsg = "图片为空，无法进行后续图像处理操作";
        //        OnFinished(this, new VisionEventArgs(0, false));
        //        return false;
        //    }
        //    if (ROIFindCtrl.GetRegion() != null)
        //        HOperatorSet.ReduceDomain(image, ROIFindCtrl.GetRegion(), out rectangleROI);
        //    else
        //        rectangleROI = image;

        //    HOperatorSet.SetColor(m_hWindow_Vision.ViewHwindow, "green");
        //    HOperatorSet.SetDraw(m_hWindow_Vision.ViewHwindow, "margin");
        //    //HOperatorSet.InvertImage(rectangleROI, out ho_ImageInvert);
        //    HObject regionClosing, regionFillUp;
        //    HOperatorSet.Threshold(rectangleROI, out ho_Regions, _ThresholdMinGray, _ThresholdMaxGray);
        //    HOperatorSet.ClosingCircle(ho_Regions, out regionClosing, _ClosingRadius);
        //    HOperatorSet.Connection(regionClosing, out ho_ConnectedRegions);
        //    HOperatorSet.FillUp(ho_ConnectedRegions, out regionFillUp);

        //    HOperatorSet.SelectShape(regionFillUp, out ho_SelectedAreaRegions, "area", "and", _MarkMinArea, _MarkMaxArea);
        //    HOperatorSet.SelectShape(ho_SelectedAreaRegions, out ho_SelectedCirRegions, "circularity", "and", _CircularityMin, _CircularityMax);
        //    HOperatorSet.GenContourRegionXld(ho_SelectedCirRegions, out ho_Contours, "border");
        //    HOperatorSet.DispObj(ho_SelectedCirRegions, m_hWindow_Vision.ViewHwindow);
        //    HOperatorSet.CountObj(ho_Contours, out hv_BlobCount);
        //    _BlobCount = hv_BlobCount.I;
        //    HTuple AreaValue, CloumnsValue, RowsValue;
        //    if (hv_BlobCount > 0)
        //    {
        //        HOperatorSet.RegionFeatures(ho_SelectedCirRegions, new HTuple("area"), out AreaValue);
        //        HOperatorSet.RegionFeatures(ho_SelectedCirRegions, new HTuple("column"), out CloumnsValue);
        //        HOperatorSet.RegionFeatures(ho_SelectedCirRegions, new HTuple("row"), out RowsValue);
        //        _BlobArea = (AreaValue.DArr).ToList();
        //        _CenterColumn = (CloumnsValue.DArr).ToList();
        //        _CenterRow = (RowsValue.DArr).ToList();
        //    }
        //    errMsg = string.Format("找到{0}个满足面积在{1}-{2}，圆度大于{3}的区域", hv_BlobCount, _MarkMinArea, _MarkMaxArea, _CircularityMin);
        //    if (hv_BlobCount.I != 1)
        //    {
        //        OnFinished(this, new VisionEventArgs(0, false));
        //    }
        //    else
        //    {
        //        _BlobArea.Clear();
        //        _CenterColumn.Clear();
        //        _CenterRow.Clear();

        //        HTuple hv_Row, hv_Column, hv_StartPhi, hv_EndPhi, hv_Radius, hv_PointOrder;
        //        //HOperatorSet.SelectObj(ho_ContoursSplit, out ho_ObjectSelected, 1);
        //        HOperatorSet.FitCircleContourXld(ho_Contours, "atukey", -1, 0, 0, 3, 2, out hv_Row, out hv_Column, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
        //        HOperatorSet.SetColor(m_hWindow_Vision.ViewHwindow, "green");
        //        HOperatorSet.SetDraw(m_hWindow_Vision.ViewHwindow, "margin");
        //        //HOperatorSet.DispCross(hWindow, hv_Row, hv_Column, 2 * hv_Radius, 0);
        //        HObject circleRegion = new HObject();
        //        HOperatorSet.GenCircle(out circleRegion, hv_Row, hv_Column, hv_Radius);
        //        HOperatorSet.DispObj(circleRegion, m_hWindow_Vision.ViewHwindow);
        //        _CenterColumn.Add(hv_Column);
        //        _CenterRow.Add(hv_Row);
        //        _BlobArea.Add(Math.PI * Math.Pow(hv_Radius, 2));

        //        _IsOk = true;
        //        HObject Cross = new HObject();
        //        Cross.GenEmptyObj();
        //        HOperatorSet.GenCrossContourXld(out Cross, hv_Row, hv_Column, 200, 0);
        //        //HOperatorSet.DispCross(m_hWindow_Vision.ViewHwindow,hv_Row, hv_Column, 100, 0);
        //        m_hWindow_Vision.mView.addIconicVar(circleRegion.Clone());
        //        //Application.DoEvents();
        //        m_hWindow_Vision.mView.addIconicVar(Cross.Clone());
        //        //Application.DoEvents();
        //        circleRegion.Dispose();
        //        Cross.Dispose();
        //    }
        //    ho_Regions.Dispose();
        //    ho_ConnectedRegions.Dispose();
        //    ho_SelectedAreaRegions.Dispose();
        //    ho_SelectedCirRegions.Dispose();
        //    ho_Contours.Dispose();
        //    regionClosing.Dispose();
        //    regionFillUp.Dispose();
        //    return _IsOk;
        //}

        //public override bool RunVision()
        //{
        //    return RunVision(InPutImage, hWindow);
        //}
       
    }
}
//当我们想要提取Region时，图像处理后，往往存在几个类似的Region，此时，需要根据Region的一些特殊特征，来选择指定的Region。

//求Region指定特征值：region_features(Regions : : Features : Value)

//根据特征值选择区域：select_shape(Regions : SelectedRegions : Features, Operation, Min, Max : )
/*特征	英	译	备注
area	Area of the object	对象的面积	 
row	    Row index of the center	中心点的行坐标	 
column	Column index of the center	中心点的列坐标	 
width	Width of the region	区域的宽度	 
height	Height of the region	区域的高度	 
row1	Row index of upper left corner	左上角行坐标	 
column1	Column index of upper left corner	左上角列坐标	 
row2	Row index of lower right corner	右下角行坐标	 
column2	Column index of lower right corner	右下角列坐标	 
circularity	Circularity	圆度	0~1
compactness	Compactness	紧密度	0~1
contlength	Total length of contour	轮廓线总长	 
convexity	Convexity	凸性	 
rectangularity	Rectangularity	矩形度	0~1
ra	Main radius of the equivalent ellipse	等效椭圆长轴半径长度	 
rb	Secondary radius of the equivalent ellipse	等效椭圆短轴半径长度	 
phi	Orientation of the equivalent ellipse	等效椭圆方向	 
anisometry	Anisometry	椭圆参数，Ra/Rb长轴与短轴的比值	 
bulkiness	Bulkiness	椭圆参数，蓬松度π*Ra*Rb/A	 
struct_factor	Structur Factor 	椭圆参数，Anisometry*Bulkiness-1	 
outer_radius	Radius of smallest surrounding circle	最小外接圆半径	 
inner_radius	Radius of largest inner circle	最大内接圆半径	 
inner_width	Width of the largest axis-parallel rectangle that fits into the region	最大内接矩形宽度	 
inner_height	Height of the largest axis-parallel rectangle that fits into the region	最大内接矩形高度	 
dist_mean	Mean distance from the region border to the center	区域边界到中心的平均距离	 
dist_deviation	Deviation of the distance from the region border from the center	区域边界到中心距离的偏差	 
roundness	Roundness	圆度，与circularity计算方法不同	 
num_sides	Number of polygon sides	多边形边数	 
connect_num	Number of connection components	连通数	 
holes_num	Number of holes	区域内洞数	 
area_holes	Area of the holes of the object	所有洞的面积	 
max_diameter	Maximum diameter of the region	最大直径	 
orientation	Orientation of the region	区域方向	 
euler_number	Euler number	欧拉数，即连通数和洞数的差	 
rect2_phi	Orientation of the smallest surrounding rectangle	最小外接矩形的方向	 
rect2_len1	Half the length of the smallest surrounding rectangle	最小外接矩形长度的一半？？	smallest_rectangle2
rect2_len2	Half the width of the smallest surrounding rectangle	最小外接矩形宽度的一半	 
moments_m11	Geometric moments of the region	几何矩	 
moments_m20	Geometric moments of the region	几何矩	 
moments_m02	Geometric moments of the region	几何矩	 
moments_ia	Geometric moments of the region	几何矩	 
moments_ib	Geometric moments of the region	几何矩	 
moments_m11_invar	Geometric moments of the region	几何矩	 
moments_m20_invar	Geometric moments of the region	几何矩	 
moments_m02_invar	Geometric moments of the region	几何矩	 
moments_phi1	Geometric moments of the region	几何矩	 
moments_phi2	Geometric moments of the region	几何矩	 
moments_m21	Geometric moments of the region	几何矩	 
moments_m12	Geometric moments of the region	几何矩	 
moments_m03	Geometric moments of the region	几何矩	 
moments_m30	Geometric moments of the region	几何矩	 
moments_m21_invar	Geometric moments of the region	几何矩	 
moments_m12_invar	Geometric moments of the region	几何矩	 
moments_m03_invar	Geometric moments of the region	几何矩	 
moments_m30_invar	Geometric moments of the region	几何矩	 
moments_i1	Geometric moments of the region	几何矩	 
moments_i2	Geometric moments of the region	几何矩	 
moments_i3	Geometric moments of the region	几何矩	 
moments_i4	Geometric moments of the region	几何矩	 
moments_psi1	Geometric moments of the region	几何矩	 
moments_psi2	Geometric moments of the region	几何矩	 
moments_psi3	Geometric moments of the region	几何矩	 
moments_psi4	Geometric moments of the region	几何矩	 */
