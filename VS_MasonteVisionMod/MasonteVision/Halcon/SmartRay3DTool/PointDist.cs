using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using MasonteVision.Halcon;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace MasonteVision.Halcon.SmartRay3DTool
{
    [Serializable]
    public class PointDist
    {
        #region 参数
        bool _isFollow;
        [Category("定位控制"), Description("是否跟随定位器")]
        public bool IsFollow
        {
            get { return _isFollow; }
            set { _isFollow = value; }
        }
        #endregion

        #region 结果
        double _distance;
        [Category("结果"), Description("距离"), XmlIgnore]
        public double Distance
        {
            get { return _distance; }
        }

        #endregion


        public ROI ROIFindCtrl;
        HObject _hRegion;


        public PointDist(ROI roi)
        {
            ROIFindCtrl = roi;
            _hRegion = ROIFindCtrl.getRegion();
        }

        private void get_dist_p2plane(HObject ho_Z, HTuple hv_RoiNewRow, HTuple hv_RoiNewCol,
            HTuple hv_Beta, HTuple hv_Alpha, HTuple hv_Bias, HTuple hv_Down, out HTuple hv_Distance)
        {

            // Local iconic variables 

            // Local control variables 

            HTuple hv_ZDepth = null, hv_GreaterV = null;
            HTuple hv_GC = null, hv_tt = new HTuple(), hv_Sorted = null;
            HTuple hv_Length = null, hv_Mean = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GetGrayval(ho_Z, hv_RoiNewRow, hv_RoiNewCol, out hv_ZDepth);
            //
            hv_Distance = ((((hv_Beta * hv_RoiNewCol) + (hv_Alpha * hv_RoiNewRow)) - hv_ZDepth) + hv_Bias) / hv_Down;
            //

            //hv_GreaterV = hv_Distance.TupleGreaterElem(30);
            //hv_GC = hv_GreaterV.TupleFind(1);
            //if ((int)(new HTuple(((hv_GC.TupleSelect(0))).TupleGreater(-1))) != 0)
            //{
            //    hv_tt = 0;
            //}
            //LowV := Distance[<]0
            //LC := Low
            //tuple_abs (Distance, Abs)
            //tuple_min (Distance, Min)
            //tuple_max (Distance, Max)

            //tuple_sum (Distance, Sum)
            //Sum := Sum - Min - Max

            //Distance := Sum / (|Distance| - 2)

            //tuple_sort (Distance, Sorted)
            //tuple_length (Sorted, Length)
            //leftIdx := Length/4
            //rightIdx := Length*3/4
            //tuple_select_range (Sorted, leftIdx, rightIdx, Selected)

            //tuple_mean (Selected, Mean1)
            //tuple_abs (Mean1, Abs)



            //HOperatorSet.TupleSort(hv_Distance, out hv_Sorted);
            //HOperatorSet.TupleLength(hv_Distance, out hv_Length);
            //HOperatorSet.TupleSelectRange(hv_Sorted, hv_Length / 3, hv_Length - (hv_Length / 3),
            //    out hv_Distance);
            //HOperatorSet.TupleMean(hv_Distance, out hv_Mean);

            //HOperatorSet.TupleAbs(hv_Mean, out hv_Distance);

            //tuple_mean (Distance, Distance)

            return;
        }



        private void RotateRoi(ref HObject HRegion)
        {
            if (_isFollow) HOperatorSet.AffineTransRegion(HRegion, out HRegion, SmartRay3DToolSave.hv_HomMat2DIdentity, "nearest_neighbor");
        }

        public void RunVision(HObject ZImage, HaGUI m_hWindow_Vision, double X, double Y, double beta, double alpha, double bias, double down)
        {
            HTuple hTuple;
            get_dist_p2plane(ZImage, Y, X, beta, alpha, bias, down, out hTuple);
            _distance = hTuple;
        }

        public void RunVision(HObject ZImage, HaGUI m_hWindow_Vision, ROI HROI, double beta, double alpha, double bias, double down)
        {
            HObject HRegion = HROI.getRegion();
            RotateRoi(ref HRegion);
            HTuple hTuple, hv_area, hv_Row, hv_Col;
            HOperatorSet.AreaCenter(HRegion, out hv_area, out hv_Row, out hv_Col);
            get_dist_p2plane(ZImage, hv_Row, hv_Col, beta, alpha, bias, down, out hTuple);
            _distance = hTuple;
        }

        public void RunVision(HObject ZImage, HaGUI m_hWindow_Vision, double beta, double alpha, double bias, double down)
        {
            RotateRoi(ref _hRegion);
            HTuple hTuple, hv_area, hv_Row, hv_Col;
            HOperatorSet.AreaCenter(_hRegion, out hv_area, out hv_Row, out hv_Col);
            get_dist_p2plane(ZImage, hv_Row, hv_Col, beta, alpha, bias, down, out hTuple);
            _distance = hTuple;
        }

        public void RunVision(HObject ZImage, HaGUI m_hWindow_Vision, ROI HROI, HTuple MinGray, HTuple MaxGray, double beta, double alpha, double bias, double down)
        {
            HObject HRegion = HROI.getRegion();
            RotateRoi(ref HRegion);
            HTuple hTuple, hv_area, hv_Row, hv_Col, hv_area1, hv_row1, hv_col1;
            HObject Hregions, ho_ConnectedRegions, hselectregion;
            HOperatorSet.Threshold(HRegion, out Hregions, MinGray, MaxGray);
            HOperatorSet.Connection(Hregions, out ho_ConnectedRegions);
            HOperatorSet.AreaCenter(Hregions, out hv_area, out hv_Row, out hv_Col);
            HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out hselectregion, "max_area", 70);
            HOperatorSet.AreaCenter(hselectregion, out hv_area1, out hv_row1, out hv_col1);
            get_dist_p2plane(ZImage, hv_row1, hv_col1, beta, alpha, bias, down, out hTuple);
            _distance = hTuple;
        }

        public void RunVision(HObject ZImage, HaGUI m_hWindow_Vision, HTuple MinGray, HTuple MaxGray, double beta, double alpha, double bias, double down)
        {
            RotateRoi(ref _hRegion);
            HTuple hTuple, hv_area, hv_Row, hv_Col, hv_area1, hv_row1, hv_col1;
            HObject Hregions, ho_ConnectedRegions, hselectregion;
            HOperatorSet.Threshold(_hRegion, out Hregions, MinGray, MaxGray);
            HOperatorSet.Connection(Hregions, out ho_ConnectedRegions);
            HOperatorSet.AreaCenter(Hregions, out hv_area, out hv_Row, out hv_Col);
            HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out hselectregion, "max_area", 70);
            HOperatorSet.AreaCenter(hselectregion, out hv_area1, out hv_row1, out hv_col1);
            get_dist_p2plane(ZImage, hv_row1, hv_col1, beta, alpha, bias, down, out hTuple);
            _distance = hTuple;
        }

    }
}
