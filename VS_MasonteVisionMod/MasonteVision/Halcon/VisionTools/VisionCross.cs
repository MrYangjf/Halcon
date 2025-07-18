using AllInterface;
using HalconDotNet;
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
    public class VisionCross : IVisionTool
    {
        public event VisionHandle OnRunFinished;

        public VisionCross()
        {
            Line1 = new VisionLine();
            Line2 = new VisionLine();
            ROIFindCtrl1 = Line1.ROIFindCtrl;
            ROIFindCtrl2 = Line2.ROIFindCtrl;
            ToolName = "直线交点";
            ReName = ToolName;
            visionToolType = VisionToolType.VisionCross;
            VisionType = VisionType.精准定位;
            IcoImage = global::HalconVision.Properties.Resources.camera;
        }
        public override void SettingEnter()
        {
            ToolSettingForm f = new ToolSettingForm(this);
            f.Show();
        }
        public override bool RunVision(HObject image, ViewDisplayCtrl m_hWindow_Vision)
        {
            ResetResult();
            HTuple hv_row, hv_cloumn, hv_angle, isOverLapping;
            if (Line1.RunVision(image, m_hWindow_Vision))
            {
                if (Line2.RunVision(image, m_hWindow_Vision))
                {
                    HOperatorSet.AngleLx(Line1.BeginRow, Line1.BeginColumn, Line1.EndRow, Line1.EndColumn, out hv_angle);
                    HOperatorSet.IntersectionLines(Line1.BeginRow, Line1.BeginColumn, Line1.EndRow, Line1.EndColumn, Line2.BeginRow, Line2.BeginColumn, Line2.EndRow, Line2.EndColumn, out hv_row, out hv_cloumn, out isOverLapping);
                    if (!isOverLapping)
                    {
                        HObject cross;
                        _CrossAngle = hv_angle;
                        _CrossColumn = hv_cloumn;
                        _CrossRow = hv_row;

                        HOperatorSet.GenCrossContourXld(out cross, hv_row, hv_cloumn, new HTuple(100), hv_angle);
                        HOperatorSet.DispObj(cross, m_hWindow_Vision.ViewHwindow);
                        _IsOk = true;
                    }
                }
            }
            OnFinished(this, new VisionEventArgs(0, _IsOk));
            return false;
        }
        private void ResetResult()
        {
            errMsg = "Null";
            _IsOk = false;
        }
        private void OnFinished(object sender, VisionEventArgs e)
        {
            if (OnRunFinished != null)
                OnRunFinished(sender, e);
        }
        public VisionLine GetActiveLine(int index)
        {
            if (index == 1)
                return Line1;
            else
                return Line2;
        }
        #region 参数
        public ROIController ROIFindCtrl1;
        public ROIController ROIFindCtrl2;
        public VisionLine Line1;
        public VisionLine Line2;
        #endregion
        #region 结果
        double _CrossColumn = 0;
        [Category("结果"), Description("交点X坐标"), XmlIgnore]
        public double CrossColumn
        {
            get { return _CrossColumn; }
        }
        double _CrossRow = 0;
        [Category("结果"), Description("交点Y坐标"), XmlIgnore]
        public double CrossRow
        {
            get { return _CrossRow; }
        }
        double _CrossAngle = 0;
        [Category("结果"), Description("交点角度"), XmlIgnore]
        public double CrossAngle
        {
            get { return _CrossAngle; }
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
    }
}
