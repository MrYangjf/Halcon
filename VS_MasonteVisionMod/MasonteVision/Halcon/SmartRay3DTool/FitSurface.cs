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
    public class FitSurface
    {
        public ROICircle[] myCircle = new ROICircle[4];


        #region 参数

        bool _isFollow;
        [Category("定位控制"), Description("是否跟随定位器")]
        public bool IsFollow
        {
            get { return _isFollow; }
            set { _isFollow = value; }
        }

        int _thresholdMinGay = 0;
        [Category("二值化参数"), Description("灰度下阈值")]
        public int ThresholdMinGay
        {
            get { return _thresholdMinGay; }
            set { _thresholdMinGay = value; }
        }


        int _thresholdMaxGay = 128;
        [Category("二值化参数"), Description("灰度上阈值")]
        public int ThresholdMaxGay
        {
            get { return _thresholdMaxGay; }
            set { _thresholdMaxGay = value; }
        }

        double _scaleMult = 0.00135;
        [Category("缩放参数"), Description("Mult")]
        public double ScaleMult
        {
            get { return _scaleMult; }
            set { _scaleMult = value; }
        }

        double _radius = 30;
        [Category("求平参数"), Description("特征点半径")]
        public double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        double[] _row = { 350, 150, 150, 150 };
        [Category("求平参数"), Description("特征点Row")]
        public double[] Row
        {
            get { return _row; }
            set { _row = value; }
        }

        double[] _column = { 350, 150, 150, 150 };
        [Category("求平参数"), Description("特征点Column")]
        public double[] Column
        {
            get { return _column; }
            set { _column = value; }
        }

        #endregion

        #region 结果

        double _alpha;
        [Category("结果"), Description("平面Alpha"), XmlIgnore]
        public double Alpha
        {
            get { return _alpha; }
        }

        double _beta;
        [Category("结果"), Description("平面Beta"), XmlIgnore]
        public double Beta
        {
            get { return _beta; }
        }

        double _bias;
        [Category("结果"), Description("平面Bias"), XmlIgnore]
        public double Bias
        {
            get { return _bias; }
        }

        double _down;
        [Category("结果"), Description("平面Down"), XmlIgnore]
        public double Down
        {
            get { return _down; }
        }

        #endregion

        public FitSurface()
        {

        }

        private void RotateRoi(ref HObject HRegion)
        {
            if (_isFollow) HOperatorSet.AffineTransRegion(HRegion, out HRegion, SmartRay3DToolSave.hv_HomMat2DIdentity, "nearest_neighbor");

        }

        private void RotateCalPoints( ref HObject hObject)
        {
            HOperatorSet.GenEmptyObj(out hObject);
            for (int i = 0; i < 4; i++)
            {
                HObject HRrgion = myCircle[i].getRegion();
                RotateRoi(ref HRrgion);
                hObject.InsertObj(HRrgion, i);
            }
        }

        public void AddSurfacePoint(HaGUI m_hWindow_Vision)
        {

            for (int i = 0; i < 4; i++)
            {
                myCircle[i] = new ROICircle();
                myCircle[i].MIDC = 200;
                myCircle[i].MIDR = 200;
                myCircle[i].RADIUS = 150;
                m_hWindow_Vision.MyROIController.ROIList.Add(myCircle[i]);
            }
        }

        public void ShowSurfacePoint(HaGUI m_hWindow_Vision)
        {
            for (int i = 0; i < 4; i++)
            {
                m_hWindow_Vision.MyROIController.ROIList.Add(myCircle[i]);
            }
        }

        public void EntryMarkPoint(double Radius, HaGUI m_hWindow_Vision)
        {
            _radius = Radius;
            for (int i = 0; i < 4; i++)
            {
                myCircle[i] = (ROICircle)m_hWindow_Vision.MyROIController.ROIList[i];
                _row[0] = myCircle[0].ROW;
                _column[0] = myCircle[0].COL;

            }

        }

        public void EntryMarkPoint(HaGUI m_hWindow_Vision)
        {
            for (int i = 0; i < 4; i++)
            {
                myCircle[i] = (ROICircle)m_hWindow_Vision.MyROIController.ROIList[i];
                _row[i] = myCircle[i].MIDR;
                _column[i] = myCircle[i].MIDC;
                _radius = myCircle[i].RADIUS;
            }

        }

        public void RunVision(HObject hImage, HaGUI m_hWindow_Vision, out HObject Zmap)
        {
            HObject ThresholdRegion, ImageConvert, SurfaceCircles, RegionIntersection, RegionIntersectionUnion;
            HOperatorSet.GenEmptyObj(out SurfaceCircles);
            HTuple hv_Area, hv_BMRow, hv_BMCol;
            HTuple hv_Alpha, hv_Beta, hv_Gamma;
            HTuple hv_Down, hv_Bias;

            HOperatorSet.Threshold(hImage, out ThresholdRegion, _thresholdMinGay, _thresholdMaxGay);
            HOperatorSet.ConvertImageType(hImage, out ImageConvert, "real");
            HOperatorSet.ScaleImage(ImageConvert, out Zmap, _scaleMult, 0);
            EntryMarkPoint(m_hWindow_Vision);

            HTuple hv_Row = _row;
            HTuple hv_Col = _column;
            HTuple hv_RR = _radius;

            if (_isFollow) RotateCalPoints(ref SurfaceCircles);
            else HOperatorSet.GenCircle(out SurfaceCircles, hv_Row, hv_Col, ((((hv_RR.TupleConcat(
         hv_RR))).TupleConcat(hv_RR))).TupleConcat(hv_RR));
            HOperatorSet.Intersection(ThresholdRegion, SurfaceCircles, out RegionIntersection);
            HOperatorSet.AreaCenter(RegionIntersection, out hv_Area, out hv_BMRow, out hv_BMCol);
            HOperatorSet.Union1(RegionIntersection, out RegionIntersectionUnion);
            HOperatorSet.FitSurfaceFirstOrder(RegionIntersectionUnion, Zmap, "regression", 15, 2,
                out hv_Alpha, out hv_Beta, out hv_Gamma);

            hv_Down = ((((hv_Alpha * hv_Alpha) + (hv_Beta * hv_Beta)) + 1.0)).TupleSqrt();
            hv_Bias = (((-hv_Alpha) * hv_BMRow) - (hv_Beta * hv_BMCol)) + hv_Gamma;
            
            _alpha = hv_Alpha;
            _beta = hv_Beta;
            _down = hv_Down;
            _bias = hv_Bias;

        }
    }
}
