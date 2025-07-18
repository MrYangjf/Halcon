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
using System.IO;

namespace MasonteVision.Halcon.VisionTool
{

    [Serializable]
    public class VisionPadPanic : IVisionTool
    {

        #region 参数

        bool _IsSaveCsv = false;
        [Category("File：结果保存"), DisplayName("结果保存"), Description("CSV结果保存")]
        public bool IsSaveCsv
        {
            get { return _IsSaveCsv; }
            set { _IsSaveCsv = value; }
        }

        bool _IsSaveGraphics = true;
        [Category("File：图片保存"), DisplayName("图片保存"), Description("分析后的截图保存")]
        public bool IsSaveGraphicsveCsv
        {
            get { return _IsSaveGraphics; }
            set { _IsSaveGraphics = value; }
        }

        bool _IsSortGraphics = true;
        [Category("File：图片保存"), DisplayName("图片分类"), Description("分析后的分类保存")]
        public bool IsSortGraphics
        {
            get { return _IsSortGraphics; }
            set { _IsSortGraphics = value; }
        }


        int _PadColorSet = 0;
        [Category("Test1：Pad抓取参数"), DisplayName("Pad抓点颜色选择"), Description("不同颜色Pad需要不同模式判断")]
        public int PadColorSet
        {
            get { return _PadColorSet; }
            set { _PadColorSet = value; }
        }

        int _PadOpenSet = 2;
        [Category("Test1：Pad抓取参数"), DisplayName("Pad向内腐蚀圈数"), Description("Pad向内腐蚀圈数")]
        public int PadOpenSet
        {
            get { return _PadOpenSet; }
            set { _PadOpenSet = value; }
        }

        double _Circularity = 0.9;
        [Category("Test1：Pad抓取参数"), DisplayName("Pad抓点最圆度值"), Description("寻找Pad的最小圆度,Max<1")]
        public double Circularity
        {
            get { return _Circularity; }
            set { _Circularity = value; }
        }

        int _ThresholdMinPad = 108;
        [Category("Test1：Pad抓取参数"), DisplayName("Pad抓点最小灰度值"), Description("寻找Pad的ThresholdMin（ThresholdMinGray<=ThresholdMaxGray）")]
        public int ThresholdMinPad
        {
            get { return _ThresholdMinPad; }
            set { _ThresholdMinPad = value; }
        }

        int _ThresholdMaxPad = 255;
        [Category("Test1：Pad抓取参数"), DisplayName("Pad抓点最大灰度值"), Description("寻找Pad的ThresholdMax（ThresholdMinGray<=ThresholdMaxGray）")]
        public int ThresholdMaxPad
        {
            get { return _ThresholdMaxPad; }
            set { _ThresholdMaxPad = value; }
        }

        int _PadMaxArea = 999999;
        [Category("Test1：Pad抓取参数"), DisplayName("Pad抓点最大面积"), Description("寻找Pad的面积过滤最大值")]
        public int PadMaxArea
        {
            get { return _PadMaxArea; }
            set { _PadMaxArea = value; }
        }

        int _PadMinArea = 12500;
        [Category("Test1：Pad抓取参数"), DisplayName("Pad抓点最小面积"), Description("寻找Pad的面积过滤最小值")]
        public int PadMinArea
        {
            get { return _PadMinArea; }
            set { _PadMinArea = value; }
        }
        int _PadMinWidth = 100;
        [Category("Test1：Pad抓取参数"), DisplayName("Pad抓点最小宽度"), Description("寻找Pad的宽度过滤最小值")]
        public int PadMinWidth
        {
            get { return _PadMinWidth; }
            set { _PadMinWidth = value; }
        }
        int _PadMinHeight = 100;
        [Category("Test1：Pad抓取参数"), DisplayName("Pad抓点最小高度"), Description("寻找Pad的高度过滤最小值")]
        public int PadMinHeight
        {
            get { return _PadMinHeight; }
            set { _PadMinHeight = value; }
        }

        int _DifferenceMaxThreshold = 80;
        [Category("Test2：边缘抓取参数"), DisplayName("边缘缺陷暗边缘最大值阈值"), Description("边缘缺陷暗边缘最大值阈值")]
        public int DifferenceMaxThreshold
        {
            get { return _DifferenceMaxThreshold; }
            set { _DifferenceMaxThreshold = value; }
        }

        int _DifferenceMinThreshold = 200;
        [Category("Test2：边缘抓取参数"), DisplayName("边缘缺陷亮边缘最小值阈值"), Description("边缘缺陷亮边缘最小值阈值")]
        public int DifferenceMinThreshold
        {
            get { return _DifferenceMinThreshold; }
            set { _DifferenceMinThreshold = value; }
        }

        int _DifferenceLightMinArea = 10;
        [Category("Test2：边缘抓取参数"), DisplayName("亮边缘缺陷最小面积"), Description("寻找亮边缘缺陷最小面积过滤值")]
        public int DifferenceLightMinArea
        {
            get { return _DifferenceLightMinArea; }
            set { _DifferenceLightMinArea = value; }
        }

        int _DifferenceMinArea = 50;
        [Category("Test2：边缘抓取参数"), DisplayName("边缘缺陷最小面积"), Description("寻找边缘缺陷最小面积过滤值")]
        public int DifferenceMinArea
        {
            get { return _DifferenceMinArea; }
            set { _DifferenceMinArea = value; }
        }

        double _DifferenceRate = 0.4;
        [Category("Test2：边缘抓取参数"), DisplayName("边缘缺陷最小比例"), Description("边缘缺陷最小比例判断长宽类型")]
        public double DifferenceRate
        {
            get { return _DifferenceRate; }
            set { _DifferenceRate = value; }
        }

        double _DotMinPadRate = 0.5;
        [Category("Test3：黑色脏污抓取参数"), DisplayName("小Pad面积收缩比"), Description("小Pad面积收缩比")]
        public double DotMinPadRate
        {
            get { return _DotMinPadRate; }
            set { _DotMinPadRate = value; }
        }

        double _DotMaxPadRate = 2;
        [Category("Test3：黑色脏污抓取参数"), DisplayName("大Pad面积放大比"), Description("大Pad面积放大比")]
        public double DotMaxPadRate
        {
            get { return _DotMaxPadRate; }
            set { _DotMaxPadRate = value; }
        }

        int _DarkOpenValue = 5;
        [Category("Test3：黑色脏污抓取参数"), DisplayName("黑色内开比例"), Description("黑色检测内径区域设定")]
        public int DarkOpenValue
        {
            get { return _DarkOpenValue; }
            set { _DarkOpenValue = value; }
        }

        int _DarkThresholdOffset = 40;
        [Category("Test3.1：黑色脏点抓取参数"), DisplayName("黑色脏点最小灰度值"), Description("寻找黑色脏点的ThresholdMin")]
        public int DarkThresholdOffset
        {
            get { return _DarkThresholdOffset; }
            set { _DarkThresholdOffset = value; }
        }

        int _DarkCircleThresholdOffset = 40;
        [Category("Test3.2：黑色脏团抓取参数"), DisplayName("黑色脏团最小灰度值"), Description("寻找黑色脏团的ThresholdMin")]
        public int DarkCircleThresholdOffset
        {
            get { return _DarkCircleThresholdOffset; }
            set { _DarkCircleThresholdOffset = value; }
        }
        int _DarkPlaneThresholdOffset = 40;
        [Category("Test4.3：瑕疵面抓取参数"), DisplayName("瑕疵面黑瑕疵最小灰度值"), Description("寻找黑瑕疵的ThresholdMin")]
        public int DarkPlaneThresholdOffset
        {
            get { return _DarkPlaneThresholdOffset; }
            set { _DarkPlaneThresholdOffset = value; }
        }

        int _LightThresholdOffset = 40;
        [Category("Test4.1：白色脏点抓取参数"), DisplayName("白色脏点最小灰度值"), Description("寻找白色脏点的ThresholdMin")]
        public int LightThresholdOffset
        {
            get { return _LightThresholdOffset; }
            set { _LightThresholdOffset = value; }
        }
        int _LightCircleThresholdOffset = 40;
        [Category("Test4.2：白色脏团抓取参数"), DisplayName("白色脏团最小灰度值"), Description("寻找白色脏点的ThresholdMin")]
        public int LightCircleThresholdOffset
        {
            get { return _LightCircleThresholdOffset; }
            set { _LightCircleThresholdOffset = value; }
        }
        int _LightPlaneThresholdOffset = 40;
        [Category("Test4.3：瑕疵面抓取参数"), DisplayName("瑕疵面白瑕疵最小灰度值"), Description("寻找白瑕疵的ThresholdMin")]
        public int LightPlaneThresholdOffset
        {
            get { return _LightPlaneThresholdOffset; }
            set { _LightPlaneThresholdOffset = value; }
        }

        bool _IsNullColorCheck = false;
        [Category("Test5：色差检测参数"), DisplayName("是否启用黑白缺损检测"), Description("是否启用黑白缺损检测")]
        public bool IsNullColorCheck
        {
            get { return _IsNullColorCheck; }
            set { _IsNullColorCheck = value; }
        }

        int _NullColorSLightValue = 30;
        [Category("Test5：色差检测参数"), DisplayName("颜色过饱和"), Description("色饱和过盈最大阈值")]
        public int NullColorSLightValue
        {
            get { return _NullColorSLightValue; }
            set { _NullColorSLightValue = value; }
        }


        int _NullColorSDarkValue = 30;
        [Category("Test5：色差检测参数"), DisplayName("颜色不饱和"), Description("色饱和缺损最大阈值")]
        public int NullColorSDarkValue
        {
            get { return _NullColorSDarkValue; }
            set { _NullColorSDarkValue = value; }
        }

        int _ColorOpenValue = 5;
        [Category("Test5：色差检测参数"), DisplayName("色差检测内开比例"), Description("色差检测内开比例")]
        public int ColorOpenValue
        {
            get { return _ColorOpenValue; }
            set { _ColorOpenValue = value; }
        }

        bool _ColorInspect = true;
        [Category("Test5：色差检测参数"), DisplayName("是否检测色差"), Description("是否检测色差")]
        public bool ColorInspect
        {
            get { return _ColorInspect; }
            set { _ColorInspect = value; }
        }


        int _ThresholdMinH = 10;
        [Category("Test5：色差检测参数"), DisplayName("色域H最小灰度值"), Description("寻找色域的ThresholdMin")]
        public int ThresholdMinH
        {
            get { return _ThresholdMinH; }
            set { _ThresholdMinH = value; }
        }
        int _ThresholdMaxH = 255;
        [Category("Test5：色差检测参数"), DisplayName("色域H最大灰度值"), Description("寻找色域的ThresholdMax")]
        public int ThresholdMaxH
        {
            get { return _ThresholdMaxH; }
            set { _ThresholdMaxH = value; }
        }

        int _ThresholdMinS = 120;
        [Category("Test5：色差检测参数"), DisplayName("色饱和S最小灰度值"), Description("寻找色域的ThresholdMin")]
        public int ThresholdMinS
        {
            get { return _ThresholdMinS; }
            set { _ThresholdMinS = value; }
        }
        int _ThresholdMaxS = 255;
        [Category("Test5：色差检测参数"), DisplayName("色饱和S最大灰度值"), Description("寻找色域的ThresholdMax")]
        public int ThresholdMaxS
        {
            get { return _ThresholdMaxS; }
            set { _ThresholdMaxS = value; }
        }

        int _PadColorDifferenceMinArea = 20;
        [Category("Test5：色差检测参数"), DisplayName("色差最小面积"), Description("色差最小面积"), XmlIgnore]
        public int PadColorDifferenceMinArea
        {
            get { return _PadColorDifferenceMinArea; }
            set { _PadColorDifferenceMinArea = value; }
        }

        double _DotAnisometry = 0.8;
        [Category("Test4.3：瑕疵面抓取参数"), DisplayName("瑕疵最小松散度"), Description("瑕疵最小松散度")]
        public double DotAnisometry
        {
            get { return _DotAnisometry; }
            set { _DotAnisometry = value; }
        }

        int _DarkMaxArea = 9999999;
        [Category("Test3.1：黑色脏点抓取参数"), DisplayName("黑色脏点最大面积"), Description("面积过滤最大值")]
        public int DarkMaxArea
        {
            get { return _DarkMaxArea; }
            set { _DarkMaxArea = value; }
        }

        int _DarkDotMinArea = 15;
        [Category("Test3.1：黑色脏点抓取参数"), DisplayName("黑色脏点最小面积"), Description("面积过滤最小值")]
        public int DarkDotMinArea
        {
            get { return _DarkDotMinArea; }
            set { _DarkDotMinArea = value; }
        }

        int _DarkDotMinWidth = 5;
        [Category("Test3.1：黑色脏点抓取参数"), DisplayName("黑色脏点最小宽度"), Description("寻找脏点的宽度过滤最小值")]
        public int DarkDotMinWidth
        {
            get { return _DarkDotMinWidth; }
            set { _DarkDotMinWidth = value; }
        }

        int _DarkDotMinHeight = 5;
        [Category("Test3.1：黑色脏点抓取参数"), DisplayName("黑色脏点最小高度"), Description("寻找脏点的高度过滤最小值")]
        public int DarkDotMinHeight
        {
            get { return _DarkDotMinHeight; }
            set { _DarkDotMinHeight = value; }
        }

        int _DarkCircleMinArea = 15;
        [Category("Test3.2：黑色脏团抓取参数"), DisplayName("黑色脏团最小面积"), Description("面积过滤最小值")]
        public int DarkCircleMinArea
        {
            get { return _DarkCircleMinArea; }
            set { _DarkCircleMinArea = value; }
        }

        int _DarkCircleMinWidth = 5;
        [Category("Test3.2：黑色脏团抓取参数"), DisplayName("黑色脏团最小宽度"), Description("寻找脏团的宽度过滤最小值")]
        public int DarkCircleMinWidth
        {
            get { return _DarkCircleMinWidth; }
            set { _DarkCircleMinWidth = value; }
        }

        int _DarkCircleMinHeight = 5;
        [Category("Test3.2：黑色脏团抓取参数"), DisplayName("黑色脏团最小高度"), Description("寻找脏团的高度过滤最小值")]
        public int DarkCircleMinHeight
        {
            get { return _DarkCircleMinHeight; }
            set { _DarkCircleMinHeight = value; }
        }


        int _WhiteMaxArea = 9999999;
        [Category("Test4.1：白色脏点抓取参数"), DisplayName("白色脏点最大面积"), Description("白色脏点面积过滤最大值")]
        public int WhiteMaxArea
        {
            get { return _WhiteMaxArea; }
            set { _WhiteMaxArea = value; }
        }

        int _WhiteDotMinArea = 15;
        [Category("Test4.1：白色脏点抓取参数"), DisplayName("白色脏点最小面积"), Description("白色脏点面积过滤最小值")]
        public int WhiteDotMinArea
        {
            get { return _WhiteDotMinArea; }
            set { _WhiteDotMinArea = value; }
        }
        int _WhiteDotMinWidth = 5;
        [Category("Test4.1：白色脏点抓取参数"), DisplayName("白色脏点最小宽度"), Description("寻找白色脏点的宽度过滤最小值")]
        public int WhiteDotMinWidth
        {
            get { return _WhiteDotMinWidth; }
            set { _WhiteDotMinWidth = value; }
        }
        int _WhiteDotMinHeight = 5;
        [Category("Test4.1：白色脏点抓取参数"), DisplayName("白色脏点最小高度"), Description("寻找白色脏点的高度过滤最小值")]
        public int WhiteDotMinHeight
        {
            get { return _WhiteDotMinHeight; }
            set { _WhiteDotMinHeight = value; }
        }

        int _WhiteCircleMinArea = 15;
        [Category("Test4.2：白色脏团抓取参数"), DisplayName("白色脏团最小面积"), Description("白色脏团面积过滤最小值")]
        public int WhiteCircleMinArea
        {
            get { return _WhiteCircleMinArea; }
            set { _WhiteCircleMinArea = value; }
        }
        int _WhiteCircleMinWidth = 5;
        [Category("Test4.2：白色脏团抓取参数"), DisplayName("白色脏团最小宽度"), Description("寻找白色脏团的宽度过滤最小值")]
        public int WhiteCircleMinWidth
        {
            get { return _WhiteCircleMinWidth; }
            set { _WhiteCircleMinWidth = value; }
        }
        int _WhiteCircleMinHeight = 5;
        [Category("Test4.2：白色脏团抓取参数"), DisplayName("白色脏团最小高度"), Description("寻找白色脏团的高度过滤最小值")]
        public int WhiteCircleMinHeight
        {
            get { return _WhiteCircleMinHeight; }
            set { _WhiteCircleMinHeight = value; }
        }

        double _WhitePlaneMinPadRate = 0.5;
        [Category("Test4.3：瑕疵面抓取参数"), DisplayName("小pad检测面积缩小比"), Description("小pad检测面积缩小比")]
        public double WhitePlaneMinPadRate
        {
            get { return _WhitePlaneMinPadRate; }
            set { _WhitePlaneMinPadRate = value; }
        }

        double _WhitePlaneMaxPadRate = 2;
        [Category("Test4.3：瑕疵面抓取参数"), DisplayName("大pad检测面积放大比"), Description("大pad检测面积放大比")]
        public double WhitePlaneMaxPadRate
        {
            get { return _WhitePlaneMaxPadRate; }
            set { _WhitePlaneMaxPadRate = value; }
        }

        double _WhitePlaneDil = 1.5;
        [Category("Test4.3：瑕疵面抓取参数"), DisplayName("瑕疵面膨胀系数"), Description("瑕疵面膨胀系数")]
        public double WhitePlaneDil
        {
            get { return _WhitePlaneDil; }
            set { _WhitePlaneDil = value; }
        }

        int _WhitePlaneMinArea = 15;
        [Category("Test4.3：瑕疵面抓取参数"), DisplayName("瑕疵面最小面积"), Description("瑕疵面面积过滤最小值")]
        public int WhitePlaneMinArea
        {
            get { return _WhitePlaneMinArea; }
            set { _WhitePlaneMinArea = value; }
        }
        int _WhitePlaneMinWidth = 5;
        [Category("Test4.3：瑕疵面抓取参数"), DisplayName("瑕疵面最小宽度"), Description("寻找瑕疵面的宽度过滤最小值")]
        public int WhitePlaneMinWidth
        {
            get { return _WhitePlaneMinWidth; }
            set { _WhitePlaneMinWidth = value; }
        }
        int _WhitePlaneMinHeight = 5;
        [Category("Test4.3：瑕疵面抓取参数"), DisplayName("瑕疵面最小高度"), Description("寻找瑕疵面的高度过滤最小值")]
        public int WhitePlaneMinHeight
        {
            get { return _WhitePlaneMinHeight; }
            set { _WhitePlaneMinHeight = value; }
        }

        double _ScratchMinPadRate = 0.5;
        [Category("Test6.1：划痕抓取参数"), DisplayName("小pad检测面积缩小比"), Description("小pad检测面积缩小比")]
        public double ScratchMinPadRate
        {
            get { return _ScratchMinPadRate; }
            set { _ScratchMinPadRate = value; }
        }

        double _ScratchMaxPadRate = 2;
        [Category("Test6.1：划痕抓取参数"), DisplayName("大pad检测面积放大比"), Description("大pad检测面积放大比")]
        public double ScratchMaxPadRate
        {
            get { return _ScratchMaxPadRate; }
            set { _ScratchMaxPadRate = value; }
        }

        double _ScratchOpen = 3.5;
        [Category("Test6.1：划痕抓取参数"), DisplayName("划痕联通域值"), Description("划痕联通域值")]
        public double ScratchOpen
        {
            get { return _ScratchOpen; }
            set { _ScratchOpen = value; }
        }

        int _ScratchOpenValue = 5;
        [Category("Test6.1：划痕抓取参数"), DisplayName("划痕检测内开比例"), Description("划痕检测内开比例")]
        public int ScratchOpenValue
        {
            get { return _ScratchOpenValue; }
            set { _ScratchOpenValue = value; }
        }

        double _ScratchAnisometry = 3;
        [Category("Test6.1：划痕抓取参数"), DisplayName("划痕最小椭度"), Description("寻找划痕最小椭度值")]
        public double ScratchAnisometry
        {
            get { return _ScratchAnisometry; }
            set { _ScratchAnisometry = value; }
        }

        int _ScratchMaxArea = 9999999;
        [Category("Test6.1：划痕抓取参数"), DisplayName("划痕最大面积"), Description("面积过滤最大值")]
        public int ScratchMaxArea
        {
            get { return _ScratchMaxArea; }
            set { _ScratchMaxArea = value; }
        }

        int _ScratchMinArea = 150;
        [Category("Test6.1：划痕抓取参数"), DisplayName("划痕最小面积"), Description("面积过滤最小值")]
        public int ScratchMinArea
        {
            get { return _ScratchMinArea; }
            set { _ScratchMinArea = value; }
        }

        int _ScratchDarkThreshold = 150;
        [Category("Test6.1：划痕抓取参数"), DisplayName("暗划痕最大阈值"), Description("暗划痕最大阈值")]
        public int ScratchDarkThreshold
        {
            get { return _ScratchDarkThreshold; }
            set { _ScratchDarkThreshold = value; }
        }

        bool _ScratchLightEnableOpen = false;
        [Category("Test6.1：划痕抓取参数"), DisplayName("亮划痕膨胀开关"), Description("亮划痕是否允许膨胀计算")]
        public bool ScratchLightEnableOpen
        {
            get { return _ScratchLightEnableOpen; }
            set { _ScratchLightEnableOpen = value; }
        }

        int _ScratchLightThreshold = 190;
        [Category("Test6.1：划痕抓取参数"), DisplayName("亮划痕最小阈值"), Description("亮划痕最小阈值")]
        public int ScratchLightThreshold
        {
            get { return _ScratchLightThreshold; }
            set { _ScratchLightThreshold = value; }
        }

        int _ScratchSoftMaxArea = 9999999;
        [Category("Test6.2：轻微划痕抓取参数"), DisplayName("轻微划痕最大面积"), Description("面积过滤最大值")]
        public int ScratchSoftMaxArea
        {
            get { return _ScratchSoftMaxArea; }
            set { _ScratchSoftMaxArea = value; }
        }

        int _ScratchSoftMinArea = 400;
        [Category("Test6.2：轻微划痕抓取参数"), DisplayName("轻微划痕最小面积"), Description("面积过滤最小值")]
        public int ScratchSoftMinArea
        {
            get { return _ScratchSoftMinArea; }
            set { _ScratchSoftMinArea = value; }
        }

        double _ScratchSoftAnisometry = 4;
        [Category("Test6.2：轻微划痕抓取参数"), DisplayName("轻微划痕最小椭度"), Description("寻找划痕最小椭度值")]
        public double ScratchSoftAnisometry
        {
            get { return _ScratchSoftAnisometry; }
            set { _ScratchSoftAnisometry = value; }
        }

        int _ScratchSoftDarkThreshold = 35;
        [Category("Test6.2：轻微划痕抓取参数"), DisplayName("轻微暗划痕最大阈值"), Description("暗划痕最大阈值")]
        public int ScratchSoftDarkThreshold
        {
            get { return _ScratchSoftDarkThreshold; }
            set { _ScratchSoftDarkThreshold = value; }
        }

        bool _ScratchSoftLightEnableOpen = true;
        [Category("Test6.2：轻微划痕抓取参数"), DisplayName("轻微亮划痕膨胀开关"), Description("亮划痕是否允许膨胀计算")]
        public bool ScratchSoftLightEnableOpen
        {
            get { return _ScratchSoftLightEnableOpen; }
            set { _ScratchSoftLightEnableOpen = value; }
        }

        int _ScratchSoftLightThreshold = 35;
        [Category("Test6.2：轻微划痕抓取参数"), DisplayName("轻微亮划痕最小阈值"), Description("亮划痕最小阈值")]
        public int ScratchSoftLightThreshold
        {
            get { return _ScratchSoftLightThreshold; }
            set { _ScratchSoftLightThreshold = value; }
        }

        double _ScratchPlaneMinPadRate = 0.5;
        [Category("Test6.3：轻微面划痕抓取参数"), DisplayName("小pad检测面积缩小比"), Description("小pad检测面积缩小比")]
        public double ScratchPlaneMinPadRate
        {
            get { return _ScratchPlaneMinPadRate; }
            set { _ScratchPlaneMinPadRate = value; }
        }

        double _ScratchPlaneMaxPadRate = 2;
        [Category("Test6.3：轻微面划痕抓取参数"), DisplayName("大pad检测面积放大比"), Description("大pad检测面积放大比")]
        public double ScratchPlaneMaxPadRate
        {
            get { return _ScratchPlaneMaxPadRate; }
            set { _ScratchPlaneMaxPadRate = value; }
        }

        int _ScratchLengthMin = 60;
        [Category("Test6.3：轻微面划痕抓取参数"), DisplayName("轻微面亮划痕最小长度"), Description("轻微面亮划痕最小长度")]
        public int ScratchLengthMin
        {
            get { return _ScratchLengthMin; }
            set { _ScratchLengthMin = value; }
        }

        int _ScratchDarkLengthMin = 20;
        [Category("Test6.3：轻微面划痕抓取参数"), DisplayName("轻微面暗划痕最小长度"), Description("轻微面暗划痕最小长度")]
        public int ScratchDarkLengthMin
        {
            get { return _ScratchDarkLengthMin; }
            set { _ScratchDarkLengthMin = value; }
        }



        int _ScratchPlaneDarkThreshold = 25;
        [Category("Test6.3：轻微面划痕抓取参数"), DisplayName("轻微面暗划痕最大阈值"), Description("暗划痕最大阈值")]
        public int ScratchPlaneDarkThreshold
        {
            get { return _ScratchPlaneDarkThreshold; }
            set { _ScratchPlaneDarkThreshold = value; }
        }

        int _ScratchPlaneLightThreshold = 25;
        [Category("Test6.3：轻微面划痕抓取参数"), DisplayName("轻微面亮划痕最小阈值"), Description("亮划痕最小阈值")]
        public int ScratchPlaneLightThreshold
        {
            get { return _ScratchPlaneLightThreshold; }
            set { _ScratchPlaneLightThreshold = value; }
        }


        #endregion

        #region 结果
        double _PadWidth = 0;
        [Category("结果"), DisplayName("Pad宽"), Description("Pad宽"), XmlIgnore]
        public double PadWidth
        {
            get { return _PadWidth; }
        }

        double _PadHeight = 0;
        [Category("结果"), DisplayName("Pad高"), Description("Pad高"), XmlIgnore]
        public double PadHeight
        {
            get { return _PadHeight; }
        }

        double _PadArea = 0;
        [Category("结果"), DisplayName("Pad面积"), Description("Pad面积"), XmlIgnore]
        public double PadArea
        {
            get { return _PadArea; }
        }

        int _PadDifference = 0;
        [Category("结果"), DisplayName("边缘缺陷数量"), Description("边缘缺陷数量"), XmlIgnore]
        public int PadDifference
        {
            get { return _PadDifference; }
        }

        int _PadColorDifference = 0;
        [Category("结果"), DisplayName("色差缺陷数量"), Description("色差缺陷数量"), XmlIgnore]
        public int PadColorDifference
        {
            get { return _PadColorDifference; }
        }

        int _PadNullColorDifference = 0;
        [Category("结果"), DisplayName("色缺损缺陷数量"), Description("色缺损缺陷数量"), XmlIgnore]
        public int PadNullColorDifference
        {
            get { return _PadNullColorDifference; }
        }

        int _ScratchCount = 0;
        [Category("结果"), DisplayName("划痕数量"), Description("划痕数量"), XmlIgnore]
        public int ScratchCount
        {
            get { return _ScratchCount; }
        }

        int _WhiteDotCount = 0;
        [Category("结果"), DisplayName("白色脏点数量"), Description("白色脏点数量"), XmlIgnore]
        public int WhiteDotCount
        {
            get { return _WhiteDotCount; }
        }

        int _WhiteCircleCount = 0;
        [Category("结果"), DisplayName("白色脏团数量"), Description("白色脏团数量"), XmlIgnore]
        public int WhiteCircleCount
        {
            get { return _WhiteCircleCount; }
        }

        int _WhitePlaneCount = 0;
        [Category("结果"), DisplayName("白色脏面数量"), Description("白色脏面数量"), XmlIgnore]
        public int WhitePlaneCount
        {
            get { return _WhitePlaneCount; }
        }

        int _DarkDotCount = 0;
        [Category("结果"), DisplayName("黑色脏点数量"), Description("黑色脏点数量"), XmlIgnore]
        public int DarkDotCount
        {
            get { return _DarkDotCount; }
        }

        int _DarkCircleCount = 0;
        [Category("结果"), DisplayName("黑色脏团数量"), Description("黑色脏团数量"), XmlIgnore]
        public int DarkCircleCount
        {
            get { return _DarkCircleCount; }
        }

        int _DarkPlaneCount = 0;
        [Category("结果"), DisplayName("黑色脏面数量"), Description("黑色脏面数量"), XmlIgnore]
        public int DarkPlaneCount
        {
            get { return _DarkPlaneCount; }
        }


        List<double> _CenterColumn = new List<double>();

        List<double> _CenterRow = new List<double>();

        List<double> _BlobArea = new List<double>();

        string errMsg;
        [Category("结果"), DisplayName("运行信息"), Description("运行结果消息"), XmlIgnore]
        public string ErrMsg
        {
            get { return errMsg; }
        }

        #endregion

        #region 判定

        [XmlIgnore]
        bool _IsOk = false;
        [Category("判定"), DisplayName("瑕疵运行结果判定"), Description("工具运行判定"), XmlIgnore]
        public bool IsOK
        {
            get { return _IsOk; }
        }

        bool _IsToolOk = false;
        [Category("判定"), DisplayName("工具运行结果判定"), Description("结果判定"), XmlIgnore]
        public bool IsToolOk
        {
            get { return _IsToolOk; }
        }

        #endregion

        private HObject rectangleROI;

        public ROI ROIFindCtrl;// = new ROIController();
        public VisionPadPanic(ROI MYROI)
        {
            if (ROIFindCtrl == null)
            {
                ROIFindCtrl = MYROI;
            }
            ToolName = "Pad脏污分析";
            ReName = ToolName;
            visionToolType = VisionToolType.VisionPadPanic;
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
            _PadArea = 0;
            _PadHeight = 0;
            _PadWidth = 0;
            _PadDifference = 0;
            _PadColorDifference = 0;
            _PadNullColorDifference = 0;
            _ScratchCount = 0;
            _DarkDotCount = 0;
            _DarkCircleCount = 0;
            _DarkPlaneCount = 0;
            _WhiteDotCount = 0;
            _WhiteCircleCount = 0;
            _WhitePlaneCount = 0;
            _CenterColumn.Clear();
            _CenterRow.Clear();
            _BlobArea.Clear();
            errMsg = string.Empty;
            _IsOk = false;
            _IsToolOk = false;
            RunResult = _IsOk;
            ToolResult = _IsToolOk;
        }

        public override bool RunVision(HObject image, HaGUI m_hWindow_Vision)
        {
            //m_hWindow_Vision.ClearAlliMAGES();
            m_hWindow_Vision.MyHWndControl.clearList();
            m_hWindow_Vision.MyHWndControl.addIconicVar(image);
            m_hWindow_Vision.MyHWndControl.repaint();
            m_hWindow_Vision.ClearAlliMAGES();
            m_hWindow_Vision.iMAGES.Clear();
            m_hWindow_Vision.iMAGES.Add("原图", image);

            ResetResult();

            HObject ho_ThresholdRegions, ho_ThresholdSelectRegion, ho_PadRegion, ho_ClosingCirclePadRegion, ho_RegionDifference, ho_PadImage, ho_PadScratch;
            HObject ho_EmptyImageTool, ho_ImageToolR, ho_ImageToolG, ho_ImageToolB, ho_OpeningCirclePadRegion, ho_ImagaLightDynThreshold, ho_ImagaDarkDynThreshold, ho_AllDynThreshold, ho_ImageR, ho_ImageG, ho_ImageB;
            HObject ho_ImageH, ho_ImageS, ho_ImageV, ho_RegionH, ho_RegionS, ho_RegionV2, ho_RegionV, ho_ColorDifferenceRgion, ho_NullColorRgion, ho_NullColorRgion2;
            HObject ho_ThresholdOffsetRegions, ho_AllLightDynThreshold, ho_AllDarkDynThreshold;
            HObject ImageWithGraphics, EdgeCircle, EdgeCircleDark, EdgeCircleLight, PaintObject;
            HTuple hv_PadCount, hv_PadDifferenceCount, MeanR, MeanG, MeanB, De, hv_ColorDiffCount, hv_NullColorCount, hv_DirtyCount, hv_WhiteDotCount, hv_Raduis, hv_ScratchCount, hv_CenterR, hv_CenterC;
            HTuple AreaValue, CloumnsValue, RowsValue, hv_Pointer;

            HTuple hv_PadArea, hv_PadWidth, hv_PadHeight;
            HTuple hv_PaintRa, hv_PaintRow, hv_PaintCol;

            HTuple BufferWindowHandle;

            HOperatorSet.GenEmptyObj(out ho_ThresholdRegions);
            HOperatorSet.GenEmptyObj(out ho_ThresholdSelectRegion);
            HOperatorSet.GenEmptyObj(out ho_PadRegion);
            HOperatorSet.GenEmptyObj(out ho_PadImage);
            HOperatorSet.GenEmptyObj(out ho_ClosingCirclePadRegion);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ImageR);
            HOperatorSet.GenEmptyObj(out ho_ImageG);
            HOperatorSet.GenEmptyObj(out ho_ImageB);
            HOperatorSet.GenEmptyObj(out ho_EmptyImageTool);
            HOperatorSet.GenEmptyObj(out ho_ImageToolR);
            HOperatorSet.GenEmptyObj(out ho_ImageToolG);
            HOperatorSet.GenEmptyObj(out ho_ImageToolB);
            HOperatorSet.GenEmptyObj(out ho_ImagaLightDynThreshold);
            HOperatorSet.GenEmptyObj(out ho_ImagaDarkDynThreshold);
            HOperatorSet.GenEmptyObj(out ho_AllDynThreshold);
            HOperatorSet.GenEmptyObj(out ho_ImageH);
            HOperatorSet.GenEmptyObj(out ho_ImageS);
            HOperatorSet.GenEmptyObj(out ho_ImageV);
            HOperatorSet.GenEmptyObj(out ho_ColorDifferenceRgion);
            HOperatorSet.GenEmptyObj(out ho_ThresholdOffsetRegions);
            HOperatorSet.GenEmptyObj(out ho_AllLightDynThreshold);
            HOperatorSet.GenEmptyObj(out ho_AllDarkDynThreshold);
            HOperatorSet.GenEmptyObj(out ho_OpeningCirclePadRegion);
            HOperatorSet.GenEmptyObj(out ho_RegionH);
            HOperatorSet.GenEmptyObj(out ho_RegionS);
            HOperatorSet.GenEmptyObj(out ho_NullColorRgion);
            HOperatorSet.GenEmptyObj(out ho_NullColorRgion2);
            HOperatorSet.GenEmptyObj(out ho_RegionV);
            HOperatorSet.GenEmptyObj(out ho_RegionV2);
            HOperatorSet.GenEmptyObj(out ImageWithGraphics);
            HOperatorSet.GenEmptyObj(out EdgeCircle);
            HOperatorSet.GenEmptyObj(out EdgeCircleDark);
            HOperatorSet.GenEmptyObj(out EdgeCircleLight);
            HOperatorSet.GenEmptyObj(out ho_PadScratch);
            HOperatorSet.GenEmptyObj(out PaintObject);

            HOperatorSet.OpenWindow(0, 0, 1919, 1079, 0, "buffer", "", out BufferWindowHandle);
            HOperatorSet.SetDraw(BufferWindowHandle, "margin");
            HOperatorSet.SetColored(BufferWindowHandle, 12);
            HalconUtil.set_display_font(BufferWindowHandle, 30, "mono", "true", "false");
            if (image == null || !image.IsInitialized())
            {
                errMsg = "图片为空，无法进行后续图像处理操作";
                m_hWindow_Vision.MyHWndControl.DispMessage(errMsg, 10, 10, "red");
                HalconUtil.disp_message(BufferWindowHandle, errMsg, "image", 60, 0, "red", "true");
                ToolResult = false;
                return _IsOk;
            }



            if (ROIFindCtrl != null)
                HOperatorSet.ReduceDomain(image, ROIFindCtrl.getRegion(), out rectangleROI);
            else
                rectangleROI = image;

            HOperatorSet.DispObj(rectangleROI, BufferWindowHandle);


            HOperatorSet.Decompose3(rectangleROI, out ho_ImageR, out ho_ImageG, out ho_ImageB);
            m_hWindow_Vision.iMAGES.Add("红色分量图", ho_ImageR);
            m_hWindow_Vision.iMAGES.Add("绿色分量图", ho_ImageG);
            m_hWindow_Vision.iMAGES.Add("蓝色分量图", ho_ImageB);
            switch (_PadColorSet)
            {
                case 0:
                    HOperatorSet.Threshold(rectangleROI, out ho_ThresholdRegions, _ThresholdMinPad, _ThresholdMaxPad);
                    break;
                case 1:
                    HOperatorSet.Threshold(ho_ImageR, out ho_ThresholdRegions, _ThresholdMinPad, _ThresholdMaxPad);
                    break;
                case 2:
                    HOperatorSet.Threshold(ho_ImageG, out ho_ThresholdRegions, _ThresholdMinPad, _ThresholdMaxPad);
                    break;
                case 3:
                    HOperatorSet.Threshold(ho_ImageB, out ho_ThresholdRegions, _ThresholdMinPad, _ThresholdMaxPad);
                    break;
            }
            HOperatorSet.Connection(ho_ThresholdRegions, out ho_ThresholdRegions);
            HOperatorSet.SelectShape(ho_ThresholdRegions, out ho_ThresholdRegions, "area", "and", _PadMinArea, _PadMaxArea);
            HOperatorSet.SelectShape(ho_ThresholdRegions, out ho_ThresholdRegions, "width", "and", _PadMinWidth, 99999);
            HOperatorSet.SelectShape(ho_ThresholdRegions, out ho_ThresholdRegions, "height", "and", _PadMinHeight, 99999);
            HOperatorSet.SelectShape(ho_ThresholdRegions, out ho_ThresholdSelectRegion, "circularity", "and", _Circularity, 1);
            HOperatorSet.FillUp(ho_ThresholdSelectRegion, out ho_ThresholdSelectRegion);
            HOperatorSet.CountObj(ho_ThresholdSelectRegion, out hv_PadCount);
            if (hv_PadCount <= 0)
            {
                errMsg = "没找到Pad \n";
                m_hWindow_Vision.MyHWndControl.DispMessage(errMsg, 10, 10, "red");
                HalconUtil.disp_message(BufferWindowHandle, errMsg, "image", 60, 0, "red", "false");
                _IsToolOk = true;
                ToolResult = _IsToolOk;
            }
            else
            {
                _IsToolOk = true;
                ToolResult = _IsToolOk;
                if (hv_PadCount > 1)
                {
                    HOperatorSet.SortRegion(ho_ThresholdSelectRegion, out ho_PadRegion, "first_point", "true", "column");
                    HOperatorSet.SelectObj(ho_PadRegion, out ho_PadRegion, 1);
                    HOperatorSet.FillUp(ho_PadRegion, out ho_PadRegion);
                }
                else
                {
                    ho_PadRegion = ho_ThresholdSelectRegion;
                    HOperatorSet.FillUp(ho_PadRegion, out ho_PadRegion);
                }

                HOperatorSet.RegionFeatures(ho_PadRegion, "outer_radius", out hv_Raduis);
                HOperatorSet.RegionFeatures(ho_PadRegion, "row", out hv_CenterR);
                HOperatorSet.RegionFeatures(ho_PadRegion, "column", out hv_CenterC);
                HOperatorSet.RegionFeatures(ho_PadRegion, "area", out hv_PadArea);
                HOperatorSet.RegionFeatures(ho_PadRegion, "width", out hv_PadWidth);
                HOperatorSet.RegionFeatures(ho_PadRegion, "height", out hv_PadHeight);
                HOperatorSet.GenCircle(out ho_PadRegion, hv_CenterR, hv_CenterC, hv_Raduis - _PadOpenSet);
                HOperatorSet.ReduceDomain(rectangleROI, ho_PadRegion, out ho_PadImage);

                _PadWidth = hv_PadWidth;
                _PadHeight = hv_PadHeight;
                _PadArea = hv_PadArea;
                HOperatorSet.GenCircle(out ho_OpeningCirclePadRegion, hv_CenterR, hv_CenterC, hv_Raduis - 20);
                m_hWindow_Vision.MyHWndControl.addIconicVar(ho_PadRegion);
                m_hWindow_Vision.iMAGES.Add("Pad分割图", ho_PadImage);
                if (_IsSaveGraphics) HOperatorSet.PaintRegion(ho_PadRegion, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(0)).TupleConcat(255)).TupleConcat(0), "margin");
                HOperatorSet.ClosingCircle(ho_PadRegion, out ho_ClosingCirclePadRegion, 150.5);
                HOperatorSet.Difference(ho_ClosingCirclePadRegion, ho_OpeningCirclePadRegion, out EdgeCircle);
                HOperatorSet.ReduceDomain(rectangleROI, EdgeCircle, out EdgeCircle);
                HOperatorSet.Rgb1ToGray(EdgeCircle, out EdgeCircle);
                HOperatorSet.Threshold(EdgeCircle, out EdgeCircleDark, 0, _DifferenceMaxThreshold);
                HOperatorSet.Threshold(EdgeCircle, out EdgeCircleLight, _DifferenceMinThreshold, 255);
                HOperatorSet.Connection(EdgeCircleDark, out EdgeCircleDark);
                HOperatorSet.Connection(EdgeCircleLight, out EdgeCircleLight);
                HOperatorSet.SelectShape(EdgeCircleDark, out EdgeCircleDark, "area", "and", _DifferenceMinArea, _PadMaxArea);
                HOperatorSet.SelectShape(EdgeCircleLight, out EdgeCircleLight, "area", "and", _DifferenceLightMinArea, _PadMaxArea);
                HOperatorSet.Difference(ho_ClosingCirclePadRegion, ho_PadRegion, out ho_RegionDifference);
                HOperatorSet.Connection(ho_RegionDifference, out ho_RegionDifference);
                HOperatorSet.SelectShape(ho_RegionDifference, out ho_RegionDifference, "area", "and", _DifferenceMinArea, _PadMaxArea);
                HOperatorSet.SelectShape(ho_RegionDifference, out ho_RegionDifference, "circularity", "and", _DifferenceRate, 1);
                HOperatorSet.CountObj(ho_RegionDifference, out hv_PadDifferenceCount);
                HOperatorSet.Union2(ho_RegionDifference, EdgeCircleDark, out ho_RegionDifference);
                HOperatorSet.Union2(ho_RegionDifference, EdgeCircleLight, out ho_RegionDifference);
                HOperatorSet.Connection(ho_RegionDifference, out ho_RegionDifference);
                HOperatorSet.CountObj(ho_RegionDifference, out hv_PadDifferenceCount);
                if (hv_PadDifferenceCount >= 1)
                {
                    _PadDifference = hv_PadDifferenceCount;
                    HOperatorSet.SmallestCircle(ho_RegionDifference, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                    hv_PaintRa = hv_PaintRa + 20;
                    HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                    HOperatorSet.DispObj(PaintObject, BufferWindowHandle);
                    m_hWindow_Vision.iMAGES.Add("边缘缺陷区域", ho_RegionDifference);
                    m_hWindow_Vision.MyHWndControl.addIconicVar(ho_RegionDifference);
                    HOperatorSet.RegionFeatures(ho_RegionDifference, new HTuple("area"), out AreaValue);
                    HOperatorSet.RegionFeatures(ho_RegionDifference, new HTuple("column"), out CloumnsValue);
                    HOperatorSet.RegionFeatures(ho_RegionDifference, new HTuple("row"), out RowsValue);
                    _BlobArea = (AreaValue.DArr).ToList();
                    _CenterColumn = (CloumnsValue.DArr).ToList();
                    _CenterRow = (RowsValue.DArr).ToList();
                    for (int i = 0; i < _PadDifference; i++)
                    {
                        m_hWindow_Vision.MyHWndControl.DispMessage("边缘缺陷:" + (i + 1).ToString(), RowsValue[i], CloumnsValue[i], "red");
                    }
                    errMsg += "存在边缘缺陷 \n";
                    if (_IsSaveGraphics) HOperatorSet.PaintRegion(ho_RegionDifference, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin");

                }

                HOperatorSet.GenImageConst(out ho_EmptyImageTool, "byte", 1920, 1080);
                HOperatorSet.Intensity(ho_PadRegion, ho_ImageR, out MeanR, out De);
                HOperatorSet.Intensity(ho_PadRegion, ho_ImageG, out MeanG, out De);
                HOperatorSet.Intensity(ho_PadRegion, ho_ImageB, out MeanB, out De);
                HOperatorSet.GenImageProto(ho_EmptyImageTool, out ho_ImageToolR, MeanR);
                HOperatorSet.GenImageProto(ho_EmptyImageTool, out ho_ImageToolG, MeanG);
                HOperatorSet.GenImageProto(ho_EmptyImageTool, out ho_ImageToolB, MeanB);
                HOperatorSet.Compose3(ho_ImageToolR, ho_ImageToolG, ho_ImageToolB, out ho_EmptyImageTool);

                //黑点
                HOperatorSet.GenCircle(out ho_OpeningCirclePadRegion, hv_CenterR, hv_CenterC, hv_Raduis - _DarkOpenValue);
                HOperatorSet.DynThreshold(ho_PadImage, ho_EmptyImageTool, out ho_ImagaDarkDynThreshold, _DarkThresholdOffset, "dark");
                HOperatorSet.Intersection(ho_ImagaDarkDynThreshold, ho_OpeningCirclePadRegion, out ho_ImagaDarkDynThreshold);
                HOperatorSet.Connection(ho_ImagaDarkDynThreshold, out ho_ImagaDarkDynThreshold);
                HOperatorSet.SelectShape(ho_ImagaDarkDynThreshold, out ho_ImagaDarkDynThreshold, "area", "and", _DarkDotMinArea, _DarkMaxArea);
                HOperatorSet.SelectShape(ho_ImagaDarkDynThreshold, out ho_ImagaDarkDynThreshold, "width", "and", _DarkDotMinWidth, 99999);
                HOperatorSet.SelectShape(ho_ImagaDarkDynThreshold, out ho_ImagaDarkDynThreshold, "height", "and", _DarkDotMinHeight, 99999);
                HOperatorSet.CountObj(ho_ImagaDarkDynThreshold, out hv_DirtyCount);
                if (hv_DirtyCount >= 1)
                {
                    HOperatorSet.SmallestCircle(ho_ImagaDarkDynThreshold, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                    hv_PaintRa = hv_PaintRa + 20;
                    HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                    m_hWindow_Vision.iMAGES.Add("黑点缺陷区域", ho_ImagaDarkDynThreshold);
                    _DarkDotCount = hv_DirtyCount;
                    m_hWindow_Vision.MyHWndControl.addIconicVar(ho_ImagaDarkDynThreshold);
                    HOperatorSet.DispObj(PaintObject, BufferWindowHandle);
                    HOperatorSet.RegionFeatures(ho_ImagaDarkDynThreshold, new HTuple("area"), out AreaValue);
                    HOperatorSet.RegionFeatures(ho_ImagaDarkDynThreshold, new HTuple("column"), out CloumnsValue);
                    HOperatorSet.RegionFeatures(ho_ImagaDarkDynThreshold, new HTuple("row"), out RowsValue);
                    _BlobArea = (AreaValue.DArr).ToList();
                    _CenterColumn = (CloumnsValue.DArr).ToList();
                    _CenterRow = (RowsValue.DArr).ToList();
                    for (int i = 0; i < _DarkDotCount; i++)
                    {
                        //m_hWindow_Vision.MyHWndControl.DispMessage("黑点缺陷:" + (i + 1).ToString(), RowsValue[i], CloumnsValue[i], "red");
                    }
                    errMsg += "存在黑点缺陷 \n";
                    //HOperatorSet.SmallestCircle(ho_ImagaDarkDynThreshold, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                    //hv_PaintRa = hv_PaintRa + 2;
                    //HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                    //HOperatorSet.PaintRegion(PaintObject, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin");
                    if (_IsSaveGraphics) HOperatorSet.PaintRegion(ho_ImagaDarkDynThreshold, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin");
                }

                //黑团
                if (_DarkDotCount == 0)
                {
                    double MinArea = _DarkCircleMinArea;
                    double MinWidth = _DarkCircleMinWidth;
                    double MinHeight = _DarkCircleMinHeight;
                    if (hv_PadHeight < 200 && _IsNullColorCheck)
                    {
                        MinArea = _DarkCircleMinArea * DotMinPadRate;
                        MinWidth = _DarkCircleMinWidth * DotMinPadRate;
                        MinHeight = _DarkCircleMinHeight * DotMinPadRate;
                    }
                    if (hv_PadHeight > 600 && _IsNullColorCheck)
                    {
                        MinArea = _DarkCircleMinArea * DotMaxPadRate;
                        MinWidth = _DarkCircleMinWidth * DotMaxPadRate;
                        MinHeight = _DarkCircleMinHeight * DotMaxPadRate;
                    }
                    HOperatorSet.DynThreshold(ho_PadImage, ho_EmptyImageTool, out ho_ImagaDarkDynThreshold, _DarkCircleThresholdOffset, "dark");
                    HOperatorSet.Intersection(ho_ImagaDarkDynThreshold, ho_OpeningCirclePadRegion, out ho_ImagaDarkDynThreshold);
                    HOperatorSet.FillUp(ho_ImagaDarkDynThreshold, out ho_ImagaDarkDynThreshold);
                    HOperatorSet.Connection(ho_ImagaDarkDynThreshold, out ho_ImagaDarkDynThreshold);
                    HOperatorSet.SelectShape(ho_ImagaDarkDynThreshold, out ho_ImagaDarkDynThreshold, "area", "and", MinArea, _DarkMaxArea);
                    HOperatorSet.SelectShape(ho_ImagaDarkDynThreshold, out ho_ImagaDarkDynThreshold, "width", "and", MinWidth, 99999);
                    HOperatorSet.SelectShape(ho_ImagaDarkDynThreshold, out ho_ImagaDarkDynThreshold, "height", "and", MinHeight, 99999);
                    HOperatorSet.CountObj(ho_ImagaDarkDynThreshold, out hv_DirtyCount);
                    if (hv_DirtyCount >= 1)
                    {
                        HOperatorSet.SmallestCircle(ho_ImagaDarkDynThreshold, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                        hv_PaintRa = hv_PaintRa + 20;
                        HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                        m_hWindow_Vision.iMAGES.Add("黑团缺陷区域", ho_ImagaDarkDynThreshold);
                        _DarkCircleCount = hv_DirtyCount;
                        m_hWindow_Vision.MyHWndControl.addIconicVar(ho_ImagaDarkDynThreshold);
                        HOperatorSet.DispObj(PaintObject, BufferWindowHandle);
                        HOperatorSet.RegionFeatures(ho_ImagaDarkDynThreshold, new HTuple("area"), out AreaValue);
                        HOperatorSet.RegionFeatures(ho_ImagaDarkDynThreshold, new HTuple("column"), out CloumnsValue);
                        HOperatorSet.RegionFeatures(ho_ImagaDarkDynThreshold, new HTuple("row"), out RowsValue);
                        _BlobArea = (AreaValue.DArr).ToList();
                        _CenterColumn = (CloumnsValue.DArr).ToList();
                        _CenterRow = (RowsValue.DArr).ToList();
                        for (int i = 0; i < _DarkCircleCount; i++)
                        {
                            m_hWindow_Vision.MyHWndControl.DispMessage("黑团缺陷:" + (i + 1).ToString(), RowsValue[i], CloumnsValue[i], "red");
                        }
                        errMsg += "存在黑团缺陷 \n";

                        if (_IsSaveGraphics) HOperatorSet.PaintRegion(ho_ImagaDarkDynThreshold, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin");
                    }
                }

                //白点
                HOperatorSet.DynThreshold(ho_PadImage, ho_EmptyImageTool, out ho_ImagaLightDynThreshold, _LightThresholdOffset, "light");
                HOperatorSet.Intersection(ho_ImagaLightDynThreshold, ho_PadRegion, out ho_ImagaLightDynThreshold);
                HOperatorSet.Connection(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold);
                HOperatorSet.SelectShape(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold, "area", "and", _WhiteDotMinArea, _WhiteMaxArea);
                HOperatorSet.SelectShape(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold, "width", "and", _WhiteDotMinWidth, 99999);
                HOperatorSet.SelectShape(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold, "height", "and", _WhiteDotMinHeight, 99999);
                HOperatorSet.CountObj(ho_ImagaLightDynThreshold, out hv_WhiteDotCount);
                if (hv_WhiteDotCount >= 1)
                {
                    HOperatorSet.SmallestCircle(ho_ImagaLightDynThreshold, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                    hv_PaintRa = hv_PaintRa + 20;
                    HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                    m_hWindow_Vision.iMAGES.Add("白点缺陷区域", ho_ImagaLightDynThreshold);
                    _WhiteDotCount = hv_WhiteDotCount;
                    m_hWindow_Vision.MyHWndControl.addIconicVar(ho_ImagaLightDynThreshold);
                    HOperatorSet.DispObj(PaintObject, BufferWindowHandle);
                    HOperatorSet.RegionFeatures(ho_ImagaLightDynThreshold, new HTuple("area"), out AreaValue);
                    HOperatorSet.RegionFeatures(ho_ImagaLightDynThreshold, new HTuple("column"), out CloumnsValue);
                    HOperatorSet.RegionFeatures(ho_ImagaLightDynThreshold, new HTuple("row"), out RowsValue);
                    _BlobArea = (AreaValue.DArr).ToList();
                    _CenterColumn = (CloumnsValue.DArr).ToList();
                    _CenterRow = (RowsValue.DArr).ToList();
                    for (int i = 0; i < _WhiteDotCount; i++)
                    {
                        m_hWindow_Vision.MyHWndControl.DispMessage("白点缺陷:" + (i + 1).ToString(), RowsValue[i], CloumnsValue[i], "red");
                    }
                    errMsg += "存在白点缺陷 \n";

                    if (_IsSaveGraphics) HOperatorSet.PaintRegion(ho_ImagaLightDynThreshold, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin");
                }

                //白团
                if (_WhiteDotCount == 0)
                {
                    double MinArea = _WhiteCircleMinArea;
                    double MinWidth = _WhiteCircleMinWidth;
                    double MinHeight = _WhiteCircleMinHeight;
                    if (hv_PadHeight < 200 && _IsNullColorCheck)
                    {
                        MinArea = _WhiteCircleMinArea * DotMinPadRate;
                        MinWidth = _WhiteCircleMinWidth * DotMinPadRate;
                        MinHeight = _WhiteCircleMinHeight * DotMinPadRate;
                    }
                    if (hv_PadHeight > 600 && _IsNullColorCheck)
                    {
                        MinArea = _WhiteCircleMinArea * DotMaxPadRate;
                        MinWidth = _WhiteCircleMinWidth * DotMaxPadRate;
                        MinHeight = _WhiteCircleMinHeight * DotMaxPadRate;
                    }
                    HOperatorSet.DynThreshold(ho_PadImage, ho_EmptyImageTool, out ho_ImagaLightDynThreshold, _LightCircleThresholdOffset, "light");
                    HOperatorSet.Intersection(ho_ImagaLightDynThreshold, ho_OpeningCirclePadRegion, out ho_ImagaLightDynThreshold);
                    HOperatorSet.FillUp(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold);
                    HOperatorSet.Connection(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold);
                    HOperatorSet.SelectShape(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold, "area", "and", MinArea, _WhiteMaxArea);
                    HOperatorSet.SelectShape(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold, "width", "and", MinWidth, 99999);
                    HOperatorSet.SelectShape(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold, "height", "and", MinHeight, 99999);
                    HOperatorSet.CountObj(ho_ImagaLightDynThreshold, out hv_WhiteDotCount);
                    if (hv_WhiteDotCount >= 1)
                    {
                        HOperatorSet.SmallestCircle(ho_ImagaLightDynThreshold, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                        hv_PaintRa = hv_PaintRa + 20;
                        HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                        m_hWindow_Vision.iMAGES.Add("白团缺陷区域", ho_ImagaLightDynThreshold);
                        _WhiteCircleCount = hv_WhiteDotCount;
                        m_hWindow_Vision.MyHWndControl.addIconicVar(ho_ImagaLightDynThreshold);
                        HOperatorSet.DispObj(PaintObject, BufferWindowHandle);
                        HOperatorSet.RegionFeatures(ho_ImagaLightDynThreshold, new HTuple("area"), out AreaValue);
                        HOperatorSet.RegionFeatures(ho_ImagaLightDynThreshold, new HTuple("column"), out CloumnsValue);
                        HOperatorSet.RegionFeatures(ho_ImagaLightDynThreshold, new HTuple("row"), out RowsValue);
                        _BlobArea = (AreaValue.DArr).ToList();
                        _CenterColumn = (CloumnsValue.DArr).ToList();
                        _CenterRow = (RowsValue.DArr).ToList();
                        for (int i = 0; i < _WhiteCircleCount; i++)
                        {
                            //m_hWindow_Vision.MyHWndControl.DispMessage("白团缺陷:" + (i + 1).ToString(), RowsValue[i], CloumnsValue[i], "red");
                        }
                        errMsg += "存在白团缺陷 \n";

                        if (_IsSaveGraphics) HOperatorSet.PaintRegion(ho_ImagaLightDynThreshold, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin");
                    }
                }

                //瑕疵面
                if (_WhiteDotCount == 0 && _WhiteCircleCount == 0 && _DarkDotCount == 0 && _DarkCircleCount == 0)
                {
                    double MinArea = _WhitePlaneMinArea;
                    double MinWidth = _WhitePlaneMinWidth;
                    double MinHeight = _WhitePlaneMinHeight;
                    double Dilation = _WhitePlaneDil;
                    if (hv_PadHeight < 200 && _IsNullColorCheck)
                    {
                        MinArea = _WhitePlaneMinArea * WhitePlaneMinPadRate;
                        MinWidth = _WhitePlaneMinWidth * WhitePlaneMinPadRate;
                        MinHeight = _WhitePlaneMinHeight * WhitePlaneMinPadRate;
                        Dilation = 1;
                    }
                    if (hv_PadHeight > 600 && _IsNullColorCheck)
                    {
                        MinArea = _WhitePlaneMinArea * WhitePlaneMaxPadRate;
                        MinWidth = _WhitePlaneMinWidth * WhitePlaneMaxPadRate;
                        MinHeight = _WhitePlaneMinHeight * WhitePlaneMaxPadRate;
                        Dilation = _WhitePlaneDil;
                    }
                    HOperatorSet.DynThreshold(ho_PadImage, ho_EmptyImageTool, out ho_ImagaDarkDynThreshold, _DarkPlaneThresholdOffset, "dark");
                    HOperatorSet.DynThreshold(ho_PadImage, ho_EmptyImageTool, out ho_ImagaLightDynThreshold, _LightPlaneThresholdOffset, "light");
                    HOperatorSet.Intersection(ho_ImagaDarkDynThreshold, ho_OpeningCirclePadRegion, out ho_ImagaDarkDynThreshold);
                    HOperatorSet.Union2(ho_ImagaLightDynThreshold, ho_ImagaDarkDynThreshold, out ho_ImagaLightDynThreshold);
                    HOperatorSet.Intersection(ho_ImagaLightDynThreshold, ho_PadRegion, out ho_ImagaLightDynThreshold);
                    HOperatorSet.DilationCircle(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold, Dilation);
                    HOperatorSet.FillUp(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold);
                    HOperatorSet.Connection(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold);
                    m_hWindow_Vision.iMAGES.Add("面瑕疵纹路", ho_ImagaLightDynThreshold);
                    HOperatorSet.SelectShape(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold, "area", "and", MinArea, _WhiteMaxArea);
                    HOperatorSet.SelectShape(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold, "width", "and", MinWidth, 99999);
                    HOperatorSet.SelectShape(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold, "height", "and", MinHeight, 99999);
                    HOperatorSet.SelectShape(ho_ImagaLightDynThreshold, out ho_ImagaLightDynThreshold, "bulkiness", "and", 0, _DotAnisometry);
                    HOperatorSet.CountObj(ho_ImagaLightDynThreshold, out hv_WhiteDotCount);
                    if (hv_WhiteDotCount >= 1)
                    {
                        HOperatorSet.SmallestCircle(ho_ImagaLightDynThreshold, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                        hv_PaintRa = hv_PaintRa + 20;
                        HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                        m_hWindow_Vision.iMAGES.Add("瑕疵面缺陷区域", ho_ImagaLightDynThreshold);
                        _WhitePlaneCount = hv_WhiteDotCount;
                        m_hWindow_Vision.MyHWndControl.addIconicVar(ho_ImagaLightDynThreshold);
                        HOperatorSet.DispObj(PaintObject, BufferWindowHandle);
                        HOperatorSet.RegionFeatures(ho_ImagaLightDynThreshold, new HTuple("area"), out AreaValue);
                        HOperatorSet.RegionFeatures(ho_ImagaLightDynThreshold, new HTuple("column"), out CloumnsValue);
                        HOperatorSet.RegionFeatures(ho_ImagaLightDynThreshold, new HTuple("row"), out RowsValue);
                        _BlobArea = (AreaValue.DArr).ToList();
                        _CenterColumn = (CloumnsValue.DArr).ToList();
                        _CenterRow = (RowsValue.DArr).ToList();
                        for (int i = 0; i < _WhitePlaneCount; i++)
                        {
                            m_hWindow_Vision.MyHWndControl.DispMessage("瑕疵面缺陷:" + (i + 1).ToString(), RowsValue[i], CloumnsValue[i], "red");
                        }
                        errMsg += "存在瑕疵面缺陷 \n";
                        if (_IsSaveGraphics) HOperatorSet.PaintRegion(ho_ImagaLightDynThreshold, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin");
                    }
                }

                double MinScratchLength = _ScratchMinArea;

                if (hv_PadHeight < 200)
                {
                    MinScratchLength = _ScratchMinArea * _ScratchMinPadRate;
                }
                if (hv_PadHeight > 600)
                {
                    MinScratchLength = _ScratchMinArea * _ScratchMaxPadRate;

                }

                HOperatorSet.GenCircle(out ho_OpeningCirclePadRegion, hv_CenterR, hv_CenterC, hv_Raduis - _ScratchOpenValue);
                HOperatorSet.DynThreshold(ho_PadImage, ho_EmptyImageTool, out ho_AllDarkDynThreshold, _ScratchDarkThreshold, "dark");
                HOperatorSet.Connection(ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold);
                HOperatorSet.SelectShape(ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold, "area", "and", 5, _ScratchMaxArea);
                HOperatorSet.Intersection(ho_OpeningCirclePadRegion, ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold);
                HOperatorSet.DilationCircle(ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold, _ScratchOpen);

                HOperatorSet.DynThreshold(ho_PadImage, ho_EmptyImageTool, out ho_AllLightDynThreshold, _ScratchLightThreshold, "light");
                HOperatorSet.Connection(ho_AllLightDynThreshold, out ho_AllLightDynThreshold);
                HOperatorSet.SelectShape(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, "area", "and", 5, _ScratchMaxArea);
                if (_ScratchLightEnableOpen) HOperatorSet.DilationCircle(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, _ScratchOpen);
                HOperatorSet.Union2(ho_AllDarkDynThreshold, ho_AllLightDynThreshold, out ho_AllDynThreshold);
                HOperatorSet.ErosionCircle(ho_AllDynThreshold, out ho_AllDynThreshold, 1);
                HOperatorSet.Connection(ho_AllDynThreshold, out ho_AllDynThreshold);
                m_hWindow_Vision.iMAGES.Add("划痕可能性纹路", ho_AllDynThreshold);
                HOperatorSet.SelectShape(ho_AllDynThreshold, out ho_AllDynThreshold, "area", "and", MinScratchLength, _ScratchMaxArea);
                HOperatorSet.SelectShape(ho_AllDynThreshold, out ho_AllDynThreshold, "anisometry", "and", _ScratchAnisometry, _ScratchMaxArea);
                HOperatorSet.CountObj(ho_AllDynThreshold, out hv_ScratchCount);
                if (hv_ScratchCount >= 1)
                {
                    HOperatorSet.SmallestCircle(ho_AllDynThreshold, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                    hv_PaintRa = hv_PaintRa + 20;
                    HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                    m_hWindow_Vision.iMAGES.Add("划痕缺陷区域", ho_AllDynThreshold);
                    _ScratchCount = hv_ScratchCount;
                    m_hWindow_Vision.MyHWndControl.addIconicVar(ho_AllDynThreshold);
                    HOperatorSet.DispObj(PaintObject, BufferWindowHandle);
                    HOperatorSet.RegionFeatures(ho_AllDynThreshold, new HTuple("area"), out AreaValue);
                    HOperatorSet.RegionFeatures(ho_AllDynThreshold, new HTuple("column"), out CloumnsValue);
                    HOperatorSet.RegionFeatures(ho_AllDynThreshold, new HTuple("row"), out RowsValue);
                    _BlobArea = (AreaValue.DArr).ToList();
                    _CenterColumn = (CloumnsValue.DArr).ToList();
                    _CenterRow = (RowsValue.DArr).ToList();
                    for (int i = 0; i < _ScratchCount; i++)
                    {
                        m_hWindow_Vision.MyHWndControl.DispMessage("划痕缺陷:" + (i + 1).ToString(), RowsValue[i], CloumnsValue[i], "red");
                    }
                    errMsg += "存在划痕缺陷 \n";

                    if (_IsSaveGraphics) HOperatorSet.PaintRegion(ho_AllDynThreshold, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin");
                }

                if (_ScratchCount == 0)
                {
                    MinScratchLength = _ScratchSoftMinArea;

                    if (hv_PadHeight < 200)
                    {
                        MinScratchLength = _ScratchSoftMinArea * _ScratchMinPadRate;
                    }
                    if (hv_PadHeight > 600)
                    {
                        MinScratchLength = _ScratchSoftMinArea * _ScratchMaxPadRate;

                    }
                    HOperatorSet.DynThreshold(ho_PadImage, ho_EmptyImageTool, out ho_AllDarkDynThreshold, _ScratchSoftDarkThreshold, "dark");
                    HOperatorSet.Connection(ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold);
                    HOperatorSet.SelectShape(ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold, "area", "and", 5, _ScratchSoftMaxArea);
                    HOperatorSet.Intersection(ho_OpeningCirclePadRegion, ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold);
                    HOperatorSet.DilationCircle(ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold, _ScratchOpen);
                    ho_AllDynThreshold = ho_AllDarkDynThreshold;
                    HOperatorSet.Union1(ho_AllDynThreshold, out ho_AllDynThreshold);
                    HOperatorSet.ErosionCircle(ho_AllDynThreshold, out ho_AllDynThreshold, 1);
                    HOperatorSet.Connection(ho_AllDynThreshold, out ho_AllDynThreshold);
                    m_hWindow_Vision.iMAGES.Add("轻微划痕可能性纹路1", ho_AllDynThreshold);
                    HOperatorSet.SelectShape(ho_AllDynThreshold, out ho_AllDynThreshold, "area", "and", MinScratchLength, _ScratchSoftMaxArea);
                    HOperatorSet.SelectShape(ho_AllDynThreshold, out ho_AllDynThreshold, "anisometry", "and", _ScratchSoftAnisometry, _ScratchSoftMaxArea);
                    HOperatorSet.CountObj(ho_AllDynThreshold, out hv_ScratchCount);

                    if (hv_ScratchCount >= 1)
                    {
                        HOperatorSet.SmallestCircle(ho_AllDynThreshold, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                        hv_PaintRa = hv_PaintRa + 20;
                        HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                        m_hWindow_Vision.iMAGES.Add("轻微划痕缺陷区域", ho_AllDynThreshold);
                        _ScratchCount = hv_ScratchCount + _ScratchCount;
                        m_hWindow_Vision.MyHWndControl.addIconicVar(ho_AllDynThreshold);
                        HOperatorSet.DispObj(PaintObject, BufferWindowHandle);
                        HOperatorSet.RegionFeatures(ho_AllDynThreshold, new HTuple("area"), out AreaValue);
                        HOperatorSet.RegionFeatures(ho_AllDynThreshold, new HTuple("column"), out CloumnsValue);
                        HOperatorSet.RegionFeatures(ho_AllDynThreshold, new HTuple("row"), out RowsValue);
                        _BlobArea = (AreaValue.DArr).ToList();
                        _CenterColumn = (CloumnsValue.DArr).ToList();
                        _CenterRow = (RowsValue.DArr).ToList();
                        for (int i = 0; i < hv_ScratchCount.D; i++)
                        {
                            m_hWindow_Vision.MyHWndControl.DispMessage("轻微划痕缺陷:" + (i + 1).ToString(), RowsValue[i], CloumnsValue[i], "red");
                        }
                        errMsg += "存在轻微划痕缺陷 \n";

                        if (_IsSaveGraphics) HOperatorSet.PaintRegion(ho_AllDynThreshold, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin");
                    }
                    else
                    {
                        HOperatorSet.DynThreshold(ho_PadImage, ho_EmptyImageTool, out ho_AllLightDynThreshold, _ScratchSoftLightThreshold, "light");
                        HOperatorSet.Connection(ho_AllLightDynThreshold, out ho_AllLightDynThreshold);
                        HOperatorSet.SelectShape(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, "area", "and", 5, _ScratchSoftMaxArea);
                        if (_ScratchSoftLightEnableOpen) HOperatorSet.DilationCircle(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, _ScratchOpen);
                        ho_AllDynThreshold = ho_AllLightDynThreshold;
                        HOperatorSet.Union1(ho_AllDynThreshold, out ho_AllDynThreshold);
                        HOperatorSet.ErosionCircle(ho_AllDynThreshold, out ho_AllDynThreshold, 1);
                        HOperatorSet.Connection(ho_AllDynThreshold, out ho_AllDynThreshold);
                        m_hWindow_Vision.iMAGES.Add("轻微划痕可能性纹路2", ho_AllDynThreshold);

                        HOperatorSet.SelectShape(ho_AllDynThreshold, out ho_AllDynThreshold, "area", "and", MinScratchLength, _ScratchSoftMaxArea);

                        HOperatorSet.SelectShape(ho_AllDynThreshold, out ho_AllDynThreshold, "anisometry", "and", _ScratchSoftAnisometry, _ScratchSoftMaxArea);
                        HOperatorSet.CountObj(ho_AllDynThreshold, out hv_ScratchCount);

                        if (hv_ScratchCount >= 1)
                        {
                            HOperatorSet.SmallestCircle(ho_AllDynThreshold, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                            hv_PaintRa = hv_PaintRa + 20;
                            HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                            m_hWindow_Vision.iMAGES.Add("轻微划痕缺陷区域", ho_AllDynThreshold);
                            _ScratchCount = hv_ScratchCount + _ScratchCount;
                            m_hWindow_Vision.MyHWndControl.addIconicVar(ho_AllDynThreshold);
                            HOperatorSet.DispObj(PaintObject, BufferWindowHandle);
                            HOperatorSet.RegionFeatures(ho_AllDynThreshold, new HTuple("area"), out AreaValue);
                            HOperatorSet.RegionFeatures(ho_AllDynThreshold, new HTuple("column"), out CloumnsValue);
                            HOperatorSet.RegionFeatures(ho_AllDynThreshold, new HTuple("row"), out RowsValue);
                            _BlobArea = (AreaValue.DArr).ToList();
                            _CenterColumn = (CloumnsValue.DArr).ToList();
                            _CenterRow = (RowsValue.DArr).ToList();
                            for (int i = 0; i < hv_ScratchCount.D; i++)
                            {
                                m_hWindow_Vision.MyHWndControl.DispMessage("轻微划痕缺陷:" + (i + 1).ToString(), RowsValue[i], CloumnsValue[i], "red");
                            }
                            errMsg += "存在轻微划痕缺陷 \n";

                            if (_IsSaveGraphics) HOperatorSet.PaintRegion(ho_AllDynThreshold, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin");
                        }
                    }
                }
                if (_ScratchCount == 0)
                {
                    if (_IsNullColorCheck)
                    {
                        double MinLength = _ScratchLengthMin;
                        double MinDarkLength = _ScratchDarkLengthMin;
                        double ShiftSet = 4;
                        if (hv_PadHeight < 200)
                        {
                            MinLength = _ScratchLengthMin * _ScratchPlaneMinPadRate;
                            MinDarkLength = _ScratchDarkLengthMin * _ScratchPlaneMinPadRate;
                            ShiftSet = 2;
                        }
                        if (hv_PadHeight > 600)
                        {
                            MinLength = _ScratchLengthMin * _ScratchPlaneMaxPadRate;
                            MinDarkLength = _ScratchDarkLengthMin * _ScratchPlaneMaxPadRate;
                            ShiftSet = 8;
                        }


                        HOperatorSet.DynThreshold(ho_PadImage, ho_EmptyImageTool, out ho_AllDarkDynThreshold, _ScratchPlaneDarkThreshold, "dark");
                        HOperatorSet.Connection(ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold);
                        HOperatorSet.SelectShape(ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold, "area", "and", 3, _ScratchSoftMaxArea);
                        HOperatorSet.Intersection(ho_OpeningCirclePadRegion, ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold);

                        HOperatorSet.DynThreshold(ho_PadImage, ho_EmptyImageTool, out ho_AllLightDynThreshold, _ScratchPlaneLightThreshold, "light");
                        HOperatorSet.Connection(ho_AllLightDynThreshold, out ho_AllLightDynThreshold);
                        HOperatorSet.SelectShape(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, "area", "and", 3, _ScratchSoftMaxArea);
                        HOperatorSet.Intersection(ho_OpeningCirclePadRegion, ho_AllLightDynThreshold, out ho_AllLightDynThreshold);

                        HOperatorSet.DilationCircle(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, 3);
                        HOperatorSet.Union1(ho_AllLightDynThreshold, out ho_AllLightDynThreshold);
                        HOperatorSet.Skeleton(ho_AllLightDynThreshold, out ho_AllLightDynThreshold);
                        HOperatorSet.GenContoursSkeletonXld(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, 3, "filter");

                        HOperatorSet.UnionCollinearContoursXld(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, 60, 1, ShiftSet, 0.6, "attr_keep");
                        m_hWindow_Vision.iMAGES.Add("轻微面划痕可能性纹路1", ho_AllLightDynThreshold);
                        HOperatorSet.SelectShapeXld(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, "contlength", "and", MinLength, 9999);
                        HOperatorSet.SelectShapeXld(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, "max_diameter", "and", MinLength, 9999);
                        HOperatorSet.CountObj(ho_AllLightDynThreshold, out hv_ScratchCount);
                        if (hv_ScratchCount >= 1)
                        {
                            HOperatorSet.AreaCenterXld(ho_AllLightDynThreshold, out AreaValue, out RowsValue, out CloumnsValue, out hv_Pointer);
                            HOperatorSet.GenRegionContourXld(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, "filled");
                            HOperatorSet.SmallestCircle(ho_AllLightDynThreshold, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                            hv_PaintRa = hv_PaintRa + 20;
                            HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                            m_hWindow_Vision.iMAGES.Add("轻微面划痕缺陷区域", ho_AllLightDynThreshold);
                            _ScratchCount = hv_ScratchCount + _ScratchCount;
                            m_hWindow_Vision.MyHWndControl.addIconicVar(ho_AllLightDynThreshold);
                            HOperatorSet.DispObj(PaintObject, BufferWindowHandle);


                            _BlobArea = (AreaValue.DArr).ToList();
                            _CenterColumn = (CloumnsValue.DArr).ToList();
                            _CenterRow = (RowsValue.DArr).ToList();
                            for (int i = 0; i < hv_ScratchCount.D; i++)
                            {
                                m_hWindow_Vision.MyHWndControl.DispMessage("轻微面划痕缺陷:" + (i + 1).ToString(), RowsValue[i], CloumnsValue[i], "red");
                            }
                            errMsg += "存在轻微面划痕缺陷 \n";

                            if (_IsSaveGraphics) HOperatorSet.PaintXld(ho_AllLightDynThreshold, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0));
                        }
                        else
                        {
                            HOperatorSet.DilationCircle(ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold, 5);
                            HOperatorSet.ReduceDomain(ho_PadImage, ho_AllDarkDynThreshold, out ho_PadScratch);
                            HOperatorSet.LinesGauss(ho_PadScratch, out ho_AllDarkDynThreshold, 1.04, 8, 24, "dark", "true", "gaussian", "false");
                            m_hWindow_Vision.iMAGES.Add("轻微面划痕可能性纹路2", ho_AllDarkDynThreshold);
                            HOperatorSet.UnionCollinearContoursXld(ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold, 10, 1, ShiftSet, 0.6, "attr_keep");
                            HOperatorSet.SelectShapeXld(ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold, "contlength", "and", MinDarkLength, 9999);
                            HOperatorSet.SelectShapeXld(ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold, "max_diameter", "and", MinDarkLength, 9999);
                            HOperatorSet.CountObj(ho_AllDarkDynThreshold, out hv_ScratchCount);
                            if (hv_ScratchCount >= 1)
                            {
                                HOperatorSet.AreaCenterXld(ho_AllDarkDynThreshold, out AreaValue, out RowsValue, out CloumnsValue, out hv_Pointer);
                                HOperatorSet.GenRegionContourXld(ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold, "filled");
                                HOperatorSet.SmallestCircle(ho_AllDarkDynThreshold, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                                hv_PaintRa = hv_PaintRa + 20;
                                HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                                m_hWindow_Vision.iMAGES.Add("轻微面划痕缺陷区域", ho_AllDarkDynThreshold);
                                _ScratchCount = hv_ScratchCount + _ScratchCount;
                                m_hWindow_Vision.MyHWndControl.addIconicVar(ho_AllDarkDynThreshold);
                                HOperatorSet.DispObj(PaintObject, BufferWindowHandle);

                                _BlobArea = (AreaValue.DArr).ToList();
                                _CenterColumn = (CloumnsValue.DArr).ToList();
                                _CenterRow = (RowsValue.DArr).ToList();
                                for (int i = 0; i < hv_ScratchCount.D; i++)
                                {
                                    m_hWindow_Vision.MyHWndControl.DispMessage("轻微面划痕缺陷:" + (i + 1).ToString(), RowsValue[i], CloumnsValue[i], "red");
                                }
                                errMsg += "存在轻微面划痕缺陷 \n";

                                if (_IsSaveGraphics) HOperatorSet.PaintXld(ho_AllDarkDynThreshold, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0));
                            }
                        }
                    }
                    else
                    {
                        HOperatorSet.DynThreshold(ho_PadImage, ho_EmptyImageTool, out ho_AllDarkDynThreshold, _ScratchPlaneDarkThreshold, "dark");
                        HOperatorSet.Connection(ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold);
                        HOperatorSet.SelectShape(ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold, "area", "and", 3, _ScratchSoftMaxArea);
                        HOperatorSet.Intersection(ho_OpeningCirclePadRegion, ho_AllDarkDynThreshold, out ho_AllDarkDynThreshold);

                        HOperatorSet.DynThreshold(ho_PadImage, ho_EmptyImageTool, out ho_AllLightDynThreshold, _ScratchPlaneLightThreshold, "light");
                        HOperatorSet.Connection(ho_AllLightDynThreshold, out ho_AllLightDynThreshold);
                        HOperatorSet.SelectShape(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, "area", "and", 3, _ScratchSoftMaxArea);
                        HOperatorSet.Intersection(ho_OpeningCirclePadRegion, ho_AllLightDynThreshold, out ho_AllLightDynThreshold);

                        HOperatorSet.DilationCircle(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, 3);
                        HOperatorSet.ReduceDomain(image, ho_AllLightDynThreshold, out ho_PadScratch);
                        HOperatorSet.LinesGauss(ho_PadScratch, out ho_AllLightDynThreshold, 0.9, 5, 15, "light", "true", "gaussian", "false");
                        // HOperatorSet.LinesFacet(ho_PadScratch, out ho_AllLightDynThreshold, 5, 7, 20, "light");
                        HOperatorSet.UnionCollinearContoursXld(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, 5, 1, 3, 0.3, "attr_keep");
                        m_hWindow_Vision.iMAGES.Add("轻微面划痕可能性纹路1", ho_AllLightDynThreshold);
                        HOperatorSet.SelectShapeXld(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, "contlength", "and", _ScratchLengthMin, 9999);
                        HOperatorSet.SelectShapeXld(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, "max_diameter", "and", _ScratchLengthMin, 9999);
                        HOperatorSet.CountObj(ho_AllLightDynThreshold, out hv_ScratchCount);
                        if (hv_ScratchCount >= 1)
                        {
                            HOperatorSet.AreaCenterXld(ho_AllLightDynThreshold, out AreaValue, out RowsValue, out CloumnsValue, out hv_Pointer);
                            HOperatorSet.GenRegionContourXld(ho_AllLightDynThreshold, out ho_AllLightDynThreshold, "filled");
                            HOperatorSet.SmallestCircle(ho_AllLightDynThreshold, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                            hv_PaintRa = hv_PaintRa + 20;
                            HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                            m_hWindow_Vision.iMAGES.Add("轻微面划痕缺陷区域", ho_AllLightDynThreshold);
                            _ScratchCount = hv_ScratchCount + _ScratchCount;
                            m_hWindow_Vision.MyHWndControl.addIconicVar(ho_AllLightDynThreshold);
                            HOperatorSet.DispObj(PaintObject, BufferWindowHandle);


                            _BlobArea = (AreaValue.DArr).ToList();
                            _CenterColumn = (CloumnsValue.DArr).ToList();
                            _CenterRow = (RowsValue.DArr).ToList();
                            for (int i = 0; i < hv_ScratchCount.D; i++)
                            {
                                m_hWindow_Vision.MyHWndControl.DispMessage("轻微面划痕缺陷:" + (i + 1).ToString(), RowsValue[i], CloumnsValue[i], "red");
                            }
                            errMsg += "存在轻微面划痕缺陷 \n";

                            if (_IsSaveGraphics) HOperatorSet.PaintXld(ho_AllLightDynThreshold, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0));
                        }

                    }
                }


                HOperatorSet.TransFromRgb(ho_ImageR, ho_ImageG, ho_ImageB, out ho_ImageH, out ho_ImageS, out ho_ImageV, "hsv");
                HOperatorSet.ReduceDomain(ho_ImageH, ho_PadRegion, out ho_ImageH);
                HOperatorSet.ReduceDomain(ho_ImageS, ho_PadRegion, out ho_ImageS);
                HOperatorSet.ReduceDomain(ho_ImageV, ho_PadRegion, out ho_ImageV);
                m_hWindow_Vision.iMAGES.Add("色域分量图", ho_ImageH);
                m_hWindow_Vision.iMAGES.Add("色饱和分量图", ho_ImageS);
                m_hWindow_Vision.iMAGES.Add("亮度分量图", ho_ImageV);
                HOperatorSet.GenCircle(out ho_OpeningCirclePadRegion, hv_CenterR, hv_CenterC, hv_Raduis - _ColorOpenValue);
                if (_ColorInspect)
                {
                    HOperatorSet.Threshold(ho_ImageH, out ho_RegionH, _ThresholdMinH, _ThresholdMaxH);
                    HOperatorSet.Threshold(ho_ImageS, out ho_RegionS, _ThresholdMinS, _ThresholdMaxS);
                    HOperatorSet.Intersection(ho_RegionH, ho_RegionS, out ho_ColorDifferenceRgion);
                    HOperatorSet.Intersection(ho_OpeningCirclePadRegion, ho_ColorDifferenceRgion, out ho_ColorDifferenceRgion);
                    HOperatorSet.FillUp(ho_ColorDifferenceRgion, out ho_ColorDifferenceRgion);
                    HOperatorSet.Connection(ho_ColorDifferenceRgion, out ho_ColorDifferenceRgion);
                    HOperatorSet.SelectShape(ho_ColorDifferenceRgion, out ho_ColorDifferenceRgion, "area", "and", _PadColorDifferenceMinArea, _PadMaxArea);
                    HOperatorSet.CountObj(ho_ColorDifferenceRgion, out hv_ColorDiffCount);
                    if (hv_ColorDiffCount >= 1)
                    {
                        HOperatorSet.RegionFeatures(ho_ColorDifferenceRgion, new HTuple("area"), out AreaValue);
                        HOperatorSet.RegionFeatures(ho_ColorDifferenceRgion, new HTuple("column"), out CloumnsValue);
                        HOperatorSet.RegionFeatures(ho_ColorDifferenceRgion, new HTuple("row"), out RowsValue);
                        _BlobArea = (AreaValue.DArr).ToList();
                        _CenterColumn = (CloumnsValue.DArr).ToList();
                        _CenterRow = (RowsValue.DArr).ToList();

                        HOperatorSet.SmallestCircle(ho_ColorDifferenceRgion, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                        hv_PaintRa = hv_PaintRa + 20;
                        HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                        m_hWindow_Vision.iMAGES.Add("色差缺陷区域", ho_ColorDifferenceRgion);

                        _PadColorDifference = hv_ColorDiffCount;
                        m_hWindow_Vision.MyHWndControl.addIconicVar(ho_ColorDifferenceRgion);
                        HOperatorSet.DispObj(PaintObject, BufferWindowHandle);
                        for (int n = 0; n < _PadColorDifference; n++)
                        {
                            m_hWindow_Vision.MyHWndControl.DispMessage("色差缺陷:" + (n + 1).ToString(), RowsValue[n], CloumnsValue[n], "red");
                        }
                        errMsg += "存在色差缺陷 \n";

                        if (_IsSaveGraphics) HOperatorSet.PaintRegion(ho_ColorDifferenceRgion, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin");

                    }
                    else
                    {
                        if (_IsNullColorCheck)
                        {
                            HOperatorSet.Intensity(ho_OpeningCirclePadRegion, ho_ImageS, out MeanR, out De);
                            HOperatorSet.GenImageProto(ho_EmptyImageTool, out ho_ImageToolR, MeanR);
                            HOperatorSet.DynThreshold(ho_ImageS, ho_ImageToolR, out ho_NullColorRgion, _NullColorSDarkValue, "dark");
                            HOperatorSet.Intersection(ho_NullColorRgion, ho_OpeningCirclePadRegion, out ho_NullColorRgion);
                            HOperatorSet.FillUp(ho_NullColorRgion, out ho_NullColorRgion);
                            HOperatorSet.Connection(ho_NullColorRgion, out ho_NullColorRgion);
                            HOperatorSet.SelectShape(ho_NullColorRgion, out ho_NullColorRgion, "area", "and", _PadColorDifferenceMinArea, _PadMaxArea);
                            HOperatorSet.CountObj(ho_NullColorRgion, out hv_NullColorCount);
                            if (hv_NullColorCount >= 1)
                            {
                                HOperatorSet.RegionFeatures(ho_NullColorRgion, new HTuple("area"), out AreaValue);
                                HOperatorSet.RegionFeatures(ho_NullColorRgion, new HTuple("column"), out CloumnsValue);
                                HOperatorSet.RegionFeatures(ho_NullColorRgion, new HTuple("row"), out RowsValue);
                                _BlobArea = (AreaValue.DArr).ToList();
                                _CenterColumn = (CloumnsValue.DArr).ToList();
                                _CenterRow = (RowsValue.DArr).ToList();

                                HOperatorSet.SmallestCircle(ho_NullColorRgion, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                                hv_PaintRa = hv_PaintRa + 20;
                                HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                                m_hWindow_Vision.iMAGES.Add("色缺损缺陷区域", ho_NullColorRgion);

                                _PadNullColorDifference = hv_NullColorCount;
                                m_hWindow_Vision.MyHWndControl.addIconicVar(ho_NullColorRgion);
                                HOperatorSet.DispObj(PaintObject, BufferWindowHandle);
                                for (int n = 0; n < _PadNullColorDifference; n++)
                                {
                                    m_hWindow_Vision.MyHWndControl.DispMessage("色缺损缺陷:" + (n + 1).ToString(), RowsValue[n], CloumnsValue[n], "red");
                                }
                                errMsg += "存在色缺损缺陷 \n";

                                if (_IsSaveGraphics) HOperatorSet.PaintRegion(ho_NullColorRgion, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin");
                            }
                            else
                            {
                                HOperatorSet.DynThreshold(ho_ImageS, ho_ImageToolR, out ho_NullColorRgion, _NullColorSLightValue, "light");
                                HOperatorSet.Intersection(ho_NullColorRgion, ho_OpeningCirclePadRegion, out ho_NullColorRgion);
                                HOperatorSet.FillUp(ho_NullColorRgion, out ho_NullColorRgion);
                                HOperatorSet.Connection(ho_NullColorRgion, out ho_NullColorRgion);
                                HOperatorSet.SelectShape(ho_NullColorRgion, out ho_NullColorRgion, "area", "and", _PadColorDifferenceMinArea, _PadMaxArea);
                                HOperatorSet.CountObj(ho_NullColorRgion, out hv_NullColorCount);
                                if (hv_NullColorCount >= 1)
                                {
                                    HOperatorSet.RegionFeatures(ho_NullColorRgion, new HTuple("area"), out AreaValue);
                                    HOperatorSet.RegionFeatures(ho_NullColorRgion, new HTuple("column"), out CloumnsValue);
                                    HOperatorSet.RegionFeatures(ho_NullColorRgion, new HTuple("row"), out RowsValue);
                                    _BlobArea = (AreaValue.DArr).ToList();
                                    _CenterColumn = (CloumnsValue.DArr).ToList();
                                    _CenterRow = (RowsValue.DArr).ToList();

                                    HOperatorSet.SmallestCircle(ho_NullColorRgion, out hv_PaintRow, out hv_PaintCol, out hv_PaintRa);
                                    hv_PaintRa = hv_PaintRa + 20;
                                    HOperatorSet.GenCircle(out PaintObject, hv_PaintRow, hv_PaintCol, hv_PaintRa);
                                    m_hWindow_Vision.iMAGES.Add("色缺损缺陷区域", ho_NullColorRgion);

                                    _PadNullColorDifference = hv_NullColorCount;
                                    m_hWindow_Vision.MyHWndControl.addIconicVar(ho_NullColorRgion);
                                    HOperatorSet.DispObj(PaintObject, BufferWindowHandle);
                                    for (int n = 0; n < _PadNullColorDifference; n++)
                                    {
                                        m_hWindow_Vision.MyHWndControl.DispMessage("色缺损缺陷:" + (n + 1).ToString(), RowsValue[n], CloumnsValue[n], "red");
                                    }
                                    errMsg += "存在色缺损缺陷 \n";

                                    if (_IsSaveGraphics) HOperatorSet.PaintRegion(ho_NullColorRgion, ImageWithGraphics, out ImageWithGraphics, ((new HTuple(255)).TupleConcat(0)).TupleConcat(0), "margin");
                                }
                            }
                        }
                    }

                }
                if (!string.IsNullOrEmpty(errMsg))
                {
                    m_hWindow_Vision.MyHWndControl.DispMessage(errMsg, 10, 10, "red");
                    HalconUtil.disp_message(BufferWindowHandle, errMsg, "image", 60, 0, "red", "true");
                }
                else
                {
                    m_hWindow_Vision.MyHWndControl.DispMessage("未找到瑕疵", 10, 10, "green");
                    HalconUtil.disp_message(BufferWindowHandle, errMsg, "image", 60, 0, "green", "true");
                }
                int iRet = _PadDifference + _DarkDotCount + _DarkPlaneCount + _DarkCircleCount + _WhiteDotCount + _WhiteCircleCount + _WhitePlaneCount + _PadColorDifference + _ScratchCount + _PadNullColorDifference;
                if (iRet > 0) _IsOk = false;
                else _IsOk = true;
                RunResult = _IsOk;
            }
            if (_IsSaveCsv && !string.IsNullOrEmpty(m_hWindow_Vision.MyOpenPath))
            {
                string Cols = "File,TestTime,HavePad,Edge,DarkDot,DarkCircle,DarkPlane,WhiteDot,WhiteCircle,WhitePlane,IOXColor,NullColor,Scratch,ToolResult,Pass \n";
                string result = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14} \n", m_hWindow_Vision.MyOpenPath, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:ffff"), hv_PadCount.D, _PadDifference, _DarkDotCount, _DarkCircleCount, _DarkPlaneCount, _WhiteDotCount, _WhiteCircleCount, _WhitePlaneCount, _PadColorDifference, _PadNullColorDifference, _ScratchCount, _IsToolOk, _IsOk);
                SaveStrCSV(m_hWindow_Vision.GUIName, result, Cols);
            }
            string SortOriPicPath;
            string SortOriPicFile;
            string GraphicsPath;
            string GraphicsStr;
            string[] GraphicsStringArray = m_hWindow_Vision.MyOpenPath.Split('\\');
            string FileName = GraphicsStringArray[GraphicsStringArray.Length - 1].Replace(".jpg", "");
            if (_IsSaveGraphics && ImageWithGraphics != null && !string.IsNullOrEmpty(m_hWindow_Vision.MyOpenPath))
            {
                if (_IsOk)
                {
                    if (!_IsSortGraphics)
                    {
                        GraphicsStringArray[GraphicsStringArray.Length - 1] = "OK\\";
                        GraphicsPath = string.Join("\\", GraphicsStringArray);
                        GraphicsStr = GraphicsPath + FileName + "_OK_Graphics.bmp";
                        if (!Directory.Exists(GraphicsPath)) Directory.CreateDirectory(GraphicsPath);
                        HOperatorSet.WriteImage(ImageWithGraphics, "bmp", 0, GraphicsStr);
                    }
                    else
                    {
                        SortOriPicPath = string.Format("D:\\Image\\{0}_AOI_Test\\OK\\", DateTime.Today.ToString("yyyyMMdd"));
                        if (!Directory.Exists(SortOriPicPath)) Directory.CreateDirectory(SortOriPicPath);
                        SortOriPicFile = SortOriPicPath + FileName + ".jpg";
                        File.Copy(m_hWindow_Vision.MyOpenPath, SortOriPicFile, true);
                    }
                }
                else
                {
                    if (!_IsSortGraphics)
                    {
                        GraphicsStringArray[GraphicsStringArray.Length - 1] = "NG\\";
                        GraphicsPath = string.Join("\\", GraphicsStringArray);
                        GraphicsStr = GraphicsPath + FileName + "_NG_Graphics.bmp";

                        if (!Directory.Exists(GraphicsPath)) Directory.CreateDirectory(GraphicsPath);
                        HOperatorSet.WriteImage(ImageWithGraphics, "bmp", 0, GraphicsStr);
                    }
                    else
                    {
                        SortOriPicPath = string.Format("D:\\Image\\{0}_AOI_Test\\NG\\", DateTime.Today.ToString("yyyyMMdd"));
                        if (!Directory.Exists(SortOriPicPath)) Directory.CreateDirectory(SortOriPicPath);
                        SortOriPicFile = SortOriPicPath + FileName + ".jpg";
                        File.Copy(m_hWindow_Vision.MyOpenPath, SortOriPicFile, true);
                    }
                }
            }
            if (!_IsOk && ImageWithGraphics != null)
            {
                try
                {
                    GraphicsStringArray[GraphicsStringArray.Length - 2] = "NG";
                    GraphicsStringArray[GraphicsStringArray.Length - 1] = "";
                    GraphicsPath = string.Join("\\", GraphicsStringArray).Trim();
                    if (!Directory.Exists(GraphicsPath)) Directory.CreateDirectory(GraphicsPath);
                    string NGOriginPic = GraphicsPath + FileName + ".jpg";
                    HalconUtil.disp_message(BufferWindowHandle, FileName, "image", 1, 0, "red", "true");
                    HOperatorSet.DumpWindowImage(out ImageWithGraphics, BufferWindowHandle);
                    HOperatorSet.WriteImage(ImageWithGraphics, "jpeg 100", 0, NGOriginPic);
                    ImageWithGraphics.Dispose();
                    //File.Copy(m_hWindow_Vision.MyOpenPath, NGOriginPic, true);

                }
                catch (Exception e)
                {

                }
            }

            HOperatorSet.CloseWindow(BufferWindowHandle);

            m_hWindow_Vision.GetAlliMAGES();
            return _IsOk;
        }

        public void SaveStrCSV(string GuiName, string MystrReault, string MystrColumns)
        {
            try
            {
                if (GuiName == null) GuiName = "NoName";
                string FilePath = Application.StartupPath + "\\INI\\CSV_Files\\";
                string FileName = string.Format(FilePath + "Result_{0}_{1}.csv", GuiName, DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);

                    File.AppendAllText(FileName, MystrColumns);

                    File.AppendAllText(FileName, MystrReault);
                    return;
                }
                if (!File.Exists(FileName))
                {
                    File.AppendAllText(FileName, MystrColumns);

                    File.AppendAllText(FileName, MystrReault);
                    return;
                }
                File.AppendAllText(FileName, MystrReault);
            }
            catch { }
        }

        // Procedures 
        // External procedures 
        // Chapter: Classification / Misc
        // Short Description: Auxiliary procedure for get_custom_features and get_features. 
        public void append_names_or_groups(HTuple hv_Mode, HTuple hv_Name, HTuple hv_Groups,
            HTuple hv_CurrentName, HTuple hv_AccumulatedResults, out HTuple hv_ExtendedResults)
        {

            // Local iconic variables 

            // Local control variables 

            HTuple hv_FirstOccurrence = new HTuple(), hv_BelongsToGroup = new HTuple();
            // Initialize local and output iconic variables 
            //
            //Auxiliary procedure used only by get_features and get_custom_features
            //
            hv_ExtendedResults = hv_AccumulatedResults.Clone();
            if ((int)(new HTuple(hv_Mode.TupleEqual("get_names"))) != 0)
            {
                hv_FirstOccurrence = (new HTuple((new HTuple(hv_AccumulatedResults.TupleLength()
                    )).TupleEqual(0))).TupleOr(new HTuple(((hv_AccumulatedResults.TupleFind(
                    hv_Name))).TupleEqual(-1)));
                hv_BelongsToGroup = (new HTuple(((((hv_Name.TupleConcat(hv_Groups))).TupleFind(
                    hv_CurrentName))).TupleNotEqual(-1))).TupleOr(new HTuple(hv_CurrentName.TupleEqual(
                    "all")));
                if ((int)(hv_FirstOccurrence.TupleAnd(hv_BelongsToGroup)) != 0)
                {
                    //Output in 'get_names' mode is the name of the feature
                    hv_ExtendedResults = new HTuple();
                    hv_ExtendedResults = hv_ExtendedResults.TupleConcat(hv_AccumulatedResults);
                    hv_ExtendedResults = hv_ExtendedResults.TupleConcat(hv_Name);
                }
            }
            else if ((int)(new HTuple(hv_Mode.TupleEqual("get_groups"))) != 0)
            {
                hv_ExtendedResults = new HTuple();
                hv_ExtendedResults = hv_ExtendedResults.TupleConcat(hv_AccumulatedResults);
                hv_ExtendedResults = hv_ExtendedResults.TupleConcat(hv_Groups);
            }

            return;
        }

        // Chapter: Classification / Misc
        // Short Description: Auxiliary procedure for get_custom_features and get_features. 
        public void append_length_or_values(HTuple hv_Mode, HTuple hv_Feature, HTuple hv_AccumulatedResults,
            out HTuple hv_ExtendedResults)
        {



            // Local iconic variables 
            // Initialize local and output iconic variables 
            hv_ExtendedResults = new HTuple();
            //
            //Auxiliary procedure used only by get_features and get_custom_features
            //
            if ((int)(new HTuple(hv_Mode.TupleEqual("get_lengths"))) != 0)
            {
                //Output in 'get_lengths' mode is the length of the feature
                hv_ExtendedResults = new HTuple();
                hv_ExtendedResults = hv_ExtendedResults.TupleConcat(hv_AccumulatedResults);
                hv_ExtendedResults = hv_ExtendedResults.TupleConcat(new HTuple(hv_Feature.TupleLength()
                    ));
            }
            else if ((int)(new HTuple(hv_Mode.TupleEqual("calculate"))) != 0)
            {
                //Output in 'calculate' mode is the feature vector
                hv_ExtendedResults = new HTuple();
                hv_ExtendedResults = hv_ExtendedResults.TupleConcat(hv_AccumulatedResults);
                hv_ExtendedResults = hv_ExtendedResults.TupleConcat(hv_Feature);
            }
            else
            {
                hv_ExtendedResults = hv_AccumulatedResults.Clone();
            }

            return;
        }

        // Chapter: Classification / Misc
        // Short Description: Calculate color intensity features. 
        public void calc_feature_color_intensity(HObject ho_Region, HObject ho_Image,
            HTuple hv_ColorSpace, HTuple hv_Mode, out HTuple hv_Feature)
        {




            // Local iconic variables 

            HObject ho_R, ho_G, ho_B, ho_I1 = null, ho_I2 = null;
            HObject ho_I3 = null;

            // Local control variables 

            HTuple hv_Channels = null, hv_Mean1 = new HTuple();
            HTuple hv_Deviation1 = new HTuple(), hv_Mean2 = new HTuple();
            HTuple hv_Deviation2 = new HTuple(), hv_Mean3 = new HTuple();
            HTuple hv_Deviation3 = new HTuple(), hv_Tmp1 = new HTuple();
            HTuple hv_Tmp2 = new HTuple(), hv_Tmp3 = new HTuple();
            HTuple hv_NumRegions = null, hv_Index = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_R);
            HOperatorSet.GenEmptyObj(out ho_G);
            HOperatorSet.GenEmptyObj(out ho_B);
            HOperatorSet.GenEmptyObj(out ho_I1);
            HOperatorSet.GenEmptyObj(out ho_I2);
            HOperatorSet.GenEmptyObj(out ho_I3);
            hv_Feature = new HTuple();
            try
            {
                //
                //Calculate color features
                //
                //Transform an RGB image into the given ColorSpace
                //and calculate the mean gray value and the deviation
                //for all three channels.
                //
                HOperatorSet.CountChannels(ho_Image, out hv_Channels);
                if ((int)(new HTuple(hv_Channels.TupleNotEqual(3))) != 0)
                {
                    throw new HalconException((((("Error when calculating feature " + hv_ColorSpace) + "_") + hv_Mode)).TupleConcat(
                        "Please use a 3-channel RGB image or remove color feature from the list."));
                }
                ho_R.Dispose(); ho_G.Dispose(); ho_B.Dispose();
                HOperatorSet.Decompose3(ho_Image, out ho_R, out ho_G, out ho_B);
                if ((int)(new HTuple(hv_ColorSpace.TupleEqual("rgb"))) != 0)
                {
                    HOperatorSet.Intensity(ho_Region, ho_R, out hv_Mean1, out hv_Deviation1);
                    HOperatorSet.Intensity(ho_Region, ho_G, out hv_Mean2, out hv_Deviation2);
                    HOperatorSet.Intensity(ho_Region, ho_B, out hv_Mean3, out hv_Deviation3);
                }
                else
                {
                    ho_I1.Dispose(); ho_I2.Dispose(); ho_I3.Dispose();
                    HOperatorSet.TransFromRgb(ho_R, ho_G, ho_B, out ho_I1, out ho_I2, out ho_I3,
                        hv_ColorSpace);
                    HOperatorSet.Intensity(ho_Region, ho_I1, out hv_Mean1, out hv_Deviation1);
                    HOperatorSet.Intensity(ho_Region, ho_I2, out hv_Mean2, out hv_Deviation2);
                    HOperatorSet.Intensity(ho_Region, ho_I3, out hv_Mean3, out hv_Deviation3);
                }
                if ((int)(new HTuple(hv_Mode.TupleEqual("mean"))) != 0)
                {
                    hv_Tmp1 = hv_Mean1.Clone();
                    hv_Tmp2 = hv_Mean2.Clone();
                    hv_Tmp3 = hv_Mean3.Clone();
                }
                else if ((int)(new HTuple(hv_Mode.TupleEqual("deviation"))) != 0)
                {
                    hv_Tmp1 = hv_Deviation1.Clone();
                    hv_Tmp2 = hv_Deviation2.Clone();
                    hv_Tmp3 = hv_Deviation3.Clone();
                }
                HOperatorSet.CountObj(ho_Region, out hv_NumRegions);
                if ((int)(new HTuple(hv_NumRegions.TupleGreater(0))) != 0)
                {
                    hv_Index = HTuple.TupleGenSequence(0, (3 * hv_NumRegions) - 1, 3);
                    if (hv_Feature == null)
                        hv_Feature = new HTuple();
                    hv_Feature[hv_Index] = hv_Tmp1;
                    if (hv_Feature == null)
                        hv_Feature = new HTuple();
                    hv_Feature[1 + hv_Index] = hv_Tmp2;
                    if (hv_Feature == null)
                        hv_Feature = new HTuple();
                    hv_Feature[2 + hv_Index] = hv_Tmp3;
                }
                else
                {
                    hv_Feature = new HTuple();
                }
                ho_R.Dispose();
                ho_G.Dispose();
                ho_B.Dispose();
                ho_I1.Dispose();
                ho_I2.Dispose();
                ho_I3.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_R.Dispose();
                ho_G.Dispose();
                ho_B.Dispose();
                ho_I1.Dispose();
                ho_I2.Dispose();
                ho_I3.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Classification / Misc
        // Short Description: Calculate edge density. 
        public void calc_feature_edge_density(HObject ho_Region, HObject ho_Image, out HTuple hv_Feature)
        {



            // Local iconic variables 

            HObject ho_RegionUnion, ho_ImageReduced, ho_EdgeAmplitude = null;

            // Local control variables 

            HTuple hv_Area = null, hv_Row = null, hv_Column = null;
            HTuple hv_Width = null, hv_Height = null, hv_AreaGray = new HTuple();
            HTuple hv_ZeroIndex = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_EdgeAmplitude);
            hv_Feature = new HTuple();
            try
            {
                //
                //Calculate the edge density, i.e.
                //the ratio of the edge amplitudes to the area of the region.
                //
                ho_RegionUnion.Dispose();
                HOperatorSet.Union1(ho_Region, out ho_RegionUnion);
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(ho_Image, ho_RegionUnion, out ho_ImageReduced);
                HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row, out hv_Column);
                HOperatorSet.GetImageSize(ho_ImageReduced, out hv_Width, out hv_Height);
                if ((int)((new HTuple(hv_Width.TupleGreater(1))).TupleAnd(new HTuple(hv_Height.TupleGreater(
                    1)))) != 0)
                {
                    ho_EdgeAmplitude.Dispose();
                    HOperatorSet.SobelAmp(ho_ImageReduced, out ho_EdgeAmplitude, "sum_abs", 3);
                    HOperatorSet.AreaCenterGray(ho_Region, ho_EdgeAmplitude, out hv_AreaGray,
                        out hv_Row, out hv_Column);
                    hv_ZeroIndex = hv_Area.TupleFind(0);
                    if ((int)(new HTuple(hv_ZeroIndex.TupleNotEqual(-1))) != 0)
                    {
                        if (hv_Area == null)
                            hv_Area = new HTuple();
                        hv_Area[hv_ZeroIndex] = 1;
                        if (hv_AreaGray == null)
                            hv_AreaGray = new HTuple();
                        hv_AreaGray[hv_ZeroIndex] = 0;
                    }
                    hv_Feature = hv_AreaGray / hv_Area;
                }
                else
                {
                    hv_Feature = HTuple.TupleGenConst(new HTuple(hv_Area.TupleLength()), 0.0);
                }
                ho_RegionUnion.Dispose();
                ho_ImageReduced.Dispose();
                ho_EdgeAmplitude.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionUnion.Dispose();
                ho_ImageReduced.Dispose();
                ho_EdgeAmplitude.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Classification / Misc
        // Short Description: Calculate edge density histogram feature. 
        public void calc_feature_edge_density_histogram(HObject ho_Region, HObject ho_Image,
            HTuple hv_NumBins, out HTuple hv_Feature)
        {




            // Local iconic variables 

            HObject ho_Channel1 = null, ho_EdgeAmplitude = null;
            HObject ho_RegionSelected = null;

            // Local control variables 

            HTuple hv_ImageWidth = null, hv_ImageHeight = null;
            HTuple hv_NumRegions = null, hv_J = new HTuple(), hv_Area = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Histo = new HTuple(), hv_BinSize = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Channel1);
            HOperatorSet.GenEmptyObj(out ho_EdgeAmplitude);
            HOperatorSet.GenEmptyObj(out ho_RegionSelected);
            try
            {
                //
                //Calculate the edge density histogram, i.e.
                //the ratio of the edge amplitude histogram to the area of the region.
                //
                hv_Feature = new HTuple();
                HOperatorSet.GetImageSize(ho_Image, out hv_ImageWidth, out hv_ImageHeight);
                HOperatorSet.CountObj(ho_Region, out hv_NumRegions);
                if ((int)((new HTuple(hv_ImageWidth.TupleGreater(1))).TupleAnd(new HTuple(hv_ImageHeight.TupleGreater(
                    1)))) != 0)
                {
                    ho_Channel1.Dispose();
                    HOperatorSet.AccessChannel(ho_Image, out ho_Channel1, 1);
                    ho_EdgeAmplitude.Dispose();
                    HOperatorSet.SobelAmp(ho_Channel1, out ho_EdgeAmplitude, "sum_abs", 3);
                    HTuple end_val10 = hv_NumRegions;
                    HTuple step_val10 = 1;
                    for (hv_J = 1; hv_J.Continue(end_val10, step_val10); hv_J = hv_J.TupleAdd(step_val10))
                    {
                        ho_RegionSelected.Dispose();
                        HOperatorSet.SelectObj(ho_Region, out ho_RegionSelected, hv_J);
                        HOperatorSet.AreaCenter(ho_RegionSelected, out hv_Area, out hv_Row, out hv_Column);
                        if ((int)(new HTuple(hv_Area.TupleGreater(0))) != 0)
                        {
                            HOperatorSet.GrayHistoRange(ho_RegionSelected, ho_EdgeAmplitude, 0, 255,
                                hv_NumBins, out hv_Histo, out hv_BinSize);
                            hv_Feature = hv_Feature.TupleConcat((hv_Histo.TupleReal()) / (hv_Histo.TupleSum()
                                ));
                        }
                        else
                        {
                            hv_Feature = ((hv_Feature.TupleConcat(1.0))).TupleConcat(HTuple.TupleGenConst(
                                hv_NumBins - 1, 0.0));
                        }
                    }
                }
                else
                {
                    hv_Feature = HTuple.TupleGenConst(hv_NumRegions * hv_NumBins, 0.0);
                }
                ho_Channel1.Dispose();
                ho_EdgeAmplitude.Dispose();
                ho_RegionSelected.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Channel1.Dispose();
                ho_EdgeAmplitude.Dispose();
                ho_RegionSelected.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Classification / Misc
        // Short Description: Calculate the gradient direction histogram. 
        public void calc_feature_grad_dir_histo(HObject ho_Region, HObject ho_Image, HTuple hv_NumBins,
            out HTuple hv_Feature)
        {




            // Local iconic variables 

            HObject ho_Channel1, ho_RegionSelected = null;
            HObject ho_ImageReduced = null, ho_EdgeAmplitude = null, ho_EdgeDirection = null;

            // Local control variables 

            HTuple hv_NumRegions = null, hv_Index = null;
            HTuple hv_Histo = new HTuple(), hv_BinSize = new HTuple();
            HTuple hv_Sum = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Channel1);
            HOperatorSet.GenEmptyObj(out ho_RegionSelected);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_EdgeAmplitude);
            HOperatorSet.GenEmptyObj(out ho_EdgeDirection);
            try
            {
                //
                //Calculate gradient direction histogram
                //
                ho_Channel1.Dispose();
                HOperatorSet.AccessChannel(ho_Image, out ho_Channel1, 1);
                HOperatorSet.CountObj(ho_Region, out hv_NumRegions);
                hv_Feature = new HTuple();
                HTuple end_val6 = hv_NumRegions;
                HTuple step_val6 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val6, step_val6); hv_Index = hv_Index.TupleAdd(step_val6))
                {
                    ho_RegionSelected.Dispose();
                    HOperatorSet.SelectObj(ho_Region, out ho_RegionSelected, hv_Index);
                    ho_ImageReduced.Dispose();
                    HOperatorSet.ReduceDomain(ho_Channel1, ho_RegionSelected, out ho_ImageReduced
                        );
                    ho_EdgeAmplitude.Dispose(); ho_EdgeDirection.Dispose();
                    HOperatorSet.SobelDir(ho_ImageReduced, out ho_EdgeAmplitude, out ho_EdgeDirection,
                        "sum_abs_binomial", 3);
                    HOperatorSet.GrayHistoRange(ho_RegionSelected, ho_EdgeDirection, 0, 179,
                        hv_NumBins, out hv_Histo, out hv_BinSize);
                    hv_Sum = hv_Histo.TupleSum();
                    if ((int)(new HTuple(hv_Sum.TupleNotEqual(0))) != 0)
                    {
                        hv_Feature = hv_Feature.TupleConcat((hv_Histo.TupleReal()) / hv_Sum);
                    }
                    else
                    {
                        hv_Feature = hv_Feature.TupleConcat(hv_Histo);
                    }
                }
                ho_Channel1.Dispose();
                ho_RegionSelected.Dispose();
                ho_ImageReduced.Dispose();
                ho_EdgeAmplitude.Dispose();
                ho_EdgeDirection.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_Channel1.Dispose();
                ho_RegionSelected.Dispose();
                ho_ImageReduced.Dispose();
                ho_EdgeAmplitude.Dispose();
                ho_EdgeDirection.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Classification / Misc
        // Short Description: Calculate gray-value projections and their histograms. 
        public void calc_feature_gray_proj(HObject ho_Region, HObject ho_Image, HTuple hv_Mode,
            HTuple hv_Size, out HTuple hv_Feature)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_RegionTmp = null, ho_RegionMoved = null;
            HObject ho_ImageTmp = null;

            // Local control variables 

            HTuple hv_NumRegions = null, hv_Index = null;
            HTuple hv_RowsTmp = new HTuple(), hv_ColumnsTmp = new HTuple();
            HTuple hv_HorProjectionFilledUp = new HTuple(), hv_VertProjectionFilledUp = new HTuple();
            HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_ScaleHeight = new HTuple(), hv_ScaleWidth = new HTuple();
            HTuple hv_HorProjection = new HTuple(), hv_VertProjection = new HTuple();
            HTuple hv_HorProjectionFilledUpFront = new HTuple(), hv_VertProjectionFilledUpFront = new HTuple();
            HTuple hv_Histo = new HTuple(), hv_BinSize = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_RegionTmp);
            HOperatorSet.GenEmptyObj(out ho_RegionMoved);
            HOperatorSet.GenEmptyObj(out ho_ImageTmp);
            try
            {
                //
                //Calculate gray-value projections and their histograms
                //
                HOperatorSet.CountObj(ho_Region, out hv_NumRegions);
                hv_Feature = new HTuple();
                //
                HTuple end_val6 = hv_NumRegions;
                HTuple step_val6 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val6, step_val6); hv_Index = hv_Index.TupleAdd(step_val6))
                {
                    ho_RegionTmp.Dispose();
                    HOperatorSet.SelectObj(ho_Region, out ho_RegionTmp, hv_Index);
                    //Test empty region
                    HOperatorSet.GetRegionPoints(ho_RegionTmp, out hv_RowsTmp, out hv_ColumnsTmp);
                    if ((int)(new HTuple((new HTuple(hv_RowsTmp.TupleLength())).TupleEqual(0))) != 0)
                    {
                        hv_HorProjectionFilledUp = HTuple.TupleGenConst(hv_Size, -1.0);
                        hv_VertProjectionFilledUp = HTuple.TupleGenConst(hv_Size, -1.0);
                    }
                    else
                    {
                        //Zoom image and region to Size x Size pixels
                        HOperatorSet.SmallestRectangle1(ho_RegionTmp, out hv_Row1, out hv_Column1,
                            out hv_Row2, out hv_Column2);
                        ho_RegionMoved.Dispose();
                        HOperatorSet.MoveRegion(ho_RegionTmp, out ho_RegionMoved, -hv_Row1, -hv_Column1);
                        ho_ImageTmp.Dispose();
                        HOperatorSet.CropRectangle1(ho_Image, out ho_ImageTmp, hv_Row1, hv_Column1,
                            hv_Row2, hv_Column2);
                        hv_ScaleHeight = (hv_Size.TupleReal()) / ((hv_Row2 - hv_Row1) + 1);
                        hv_ScaleWidth = (hv_Size.TupleReal()) / ((hv_Column2 - hv_Column1) + 1);
                        {
                            HObject ExpTmpOutVar_0;
                            HOperatorSet.ZoomImageFactor(ho_ImageTmp, out ExpTmpOutVar_0, hv_ScaleWidth,
                                hv_ScaleHeight, "constant");
                            ho_ImageTmp.Dispose();
                            ho_ImageTmp = ExpTmpOutVar_0;
                        }
                        ho_RegionTmp.Dispose();
                        HOperatorSet.ZoomRegion(ho_RegionMoved, out ho_RegionTmp, hv_ScaleWidth,
                            hv_ScaleHeight);
                        //Calculate gray value projection
                        HOperatorSet.GrayProjections(ho_RegionTmp, ho_ImageTmp, "simple", out hv_HorProjection,
                            out hv_VertProjection);
                        //Fill up projection in case the zoomed region is smaller than
                        //Size x Size pixels due to interpolation effects
                        HOperatorSet.SmallestRectangle1(ho_RegionTmp, out hv_Row1, out hv_Column1,
                            out hv_Row2, out hv_Column2);
                        hv_HorProjectionFilledUpFront = new HTuple();
                        hv_HorProjectionFilledUpFront = hv_HorProjectionFilledUpFront.TupleConcat(HTuple.TupleGenConst(
                            (new HTuple(0)).TupleMax2(hv_Row1), -1.0));
                        hv_HorProjectionFilledUpFront = hv_HorProjectionFilledUpFront.TupleConcat(hv_HorProjection);
                        hv_HorProjectionFilledUp = new HTuple();
                        hv_HorProjectionFilledUp = hv_HorProjectionFilledUp.TupleConcat(hv_HorProjectionFilledUpFront);
                        hv_HorProjectionFilledUp = hv_HorProjectionFilledUp.TupleConcat(HTuple.TupleGenConst(
                            hv_Size - (new HTuple(hv_HorProjectionFilledUpFront.TupleLength())), -1.0));
                        hv_VertProjectionFilledUpFront = new HTuple();
                        hv_VertProjectionFilledUpFront = hv_VertProjectionFilledUpFront.TupleConcat(HTuple.TupleGenConst(
                            (new HTuple(0)).TupleMax2(hv_Column1), -1.0));
                        hv_VertProjectionFilledUpFront = hv_VertProjectionFilledUpFront.TupleConcat(hv_VertProjection);
                        hv_VertProjectionFilledUp = new HTuple();
                        hv_VertProjectionFilledUp = hv_VertProjectionFilledUp.TupleConcat(hv_VertProjectionFilledUpFront);
                        hv_VertProjectionFilledUp = hv_VertProjectionFilledUp.TupleConcat(HTuple.TupleGenConst(
                            hv_Size - (new HTuple(hv_VertProjectionFilledUpFront.TupleLength())),
                            -1.0));
                    }
                    if ((int)(new HTuple(hv_Mode.TupleEqual("hor"))) != 0)
                    {
                        hv_Feature = hv_Feature.TupleConcat(hv_HorProjectionFilledUp);
                    }
                    else if ((int)(new HTuple(hv_Mode.TupleEqual("vert"))) != 0)
                    {
                        hv_Feature = hv_Feature.TupleConcat(hv_VertProjectionFilledUp);
                    }
                    else if ((int)(new HTuple(hv_Mode.TupleEqual("hor_histo"))) != 0)
                    {
                        HOperatorSet.TupleHistoRange(hv_HorProjectionFilledUp, 0, 255, hv_Size,
                            out hv_Histo, out hv_BinSize);
                        hv_Feature = hv_Feature.TupleConcat(hv_Histo);
                    }
                    else if ((int)(new HTuple(hv_Mode.TupleEqual("vert_histo"))) != 0)
                    {
                        HOperatorSet.TupleHistoRange(hv_VertProjectionFilledUp, 0, 255, hv_Size,
                            out hv_Histo, out hv_BinSize);
                        hv_Feature = hv_Feature.TupleConcat(hv_Histo);
                    }
                }
                ho_RegionTmp.Dispose();
                ho_RegionMoved.Dispose();
                ho_ImageTmp.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionTmp.Dispose();
                ho_RegionMoved.Dispose();
                ho_ImageTmp.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Classification / Misc
        // Short Description: Auxiliary procedure for get_features. 
        public void append_names_or_groups_pyramid(HTuple hv_Mode, HTuple hv_Groups, HTuple hv_CurrentName,
            HTuple hv_Names, HTuple hv_NameRegExp, HTuple hv_AccumulatedResults, out HTuple hv_ExtendedResults)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_BelongsToGroup = new HTuple(), hv_TmpNames = new HTuple();
            HTuple hv_J = new HTuple(), hv_FirstOccurrence = new HTuple();
            HTuple hv_Names_COPY_INP_TMP = hv_Names.Clone();

            // Initialize local and output iconic variables 
            //
            //Auxiliary procedure used only by get_features and get_custom_features
            //
            hv_ExtendedResults = hv_AccumulatedResults.Clone();
            if ((int)(new HTuple(hv_Mode.TupleEqual("get_names"))) != 0)
            {
                hv_BelongsToGroup = (new HTuple(((hv_Groups.TupleFind(hv_CurrentName))).TupleNotEqual(
                    -1))).TupleOr(new HTuple(hv_CurrentName.TupleEqual("all")));
                if ((int)(hv_CurrentName.TupleRegexpTest(hv_NameRegExp)) != 0)
                {
                    hv_Names_COPY_INP_TMP = hv_CurrentName.Clone();
                }
                else if ((int)(hv_BelongsToGroup.TupleNot()) != 0)
                {
                    hv_Names_COPY_INP_TMP = new HTuple();
                }
                hv_TmpNames = new HTuple();
                for (hv_J = 0; (int)hv_J <= (int)((new HTuple(hv_Names_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_J = (int)hv_J + 1)
                {
                    hv_FirstOccurrence = (new HTuple((new HTuple(hv_AccumulatedResults.TupleLength()
                        )).TupleEqual(0))).TupleOr(new HTuple(((hv_AccumulatedResults.TupleFind(
                        hv_Names_COPY_INP_TMP.TupleSelect(hv_J)))).TupleEqual(-1)));
                    if ((int)(hv_FirstOccurrence) != 0)
                    {
                        //Output in 'get_names' mode is the name of the feature
                        hv_TmpNames = hv_TmpNames.TupleConcat(hv_Names_COPY_INP_TMP.TupleSelect(
                            hv_J));
                    }
                }
                hv_ExtendedResults = new HTuple();
                hv_ExtendedResults = hv_ExtendedResults.TupleConcat(hv_AccumulatedResults);
                hv_ExtendedResults = hv_ExtendedResults.TupleConcat(hv_TmpNames);
            }
            else if ((int)(new HTuple(hv_Mode.TupleEqual("get_groups"))) != 0)
            {
                hv_ExtendedResults = new HTuple();
                hv_ExtendedResults = hv_ExtendedResults.TupleConcat(hv_AccumulatedResults);
                hv_ExtendedResults = hv_ExtendedResults.TupleConcat(hv_Groups);
            }

            return;
        }

        // Chapter: Classification / Misc
        // Short Description: Calculate a feature on different image pyramid levels. 
        public void calc_feature_pyramid(HObject ho_Region, HObject ho_Image, HTuple hv_FeatureName,
            HTuple hv_NumLevels, out HTuple hv_Feature)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_ImageZoom = null, ho_RegionZoom = null;

            // Local control variables 

            HTuple hv_Zoom = null, hv_NumRegions = null;
            HTuple hv_I = new HTuple(), hv_Features = new HTuple();
            HTuple hv_FeatureLength = new HTuple(), hv_Step = new HTuple();
            HTuple hv_Indices = new HTuple(), hv_J = new HTuple();
            HTuple hv_Start = new HTuple(), hv_End = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageZoom);
            HOperatorSet.GenEmptyObj(out ho_RegionZoom);
            try
            {
                //
                //Calculate a feature for different pyramid levels
                //
                hv_Zoom = 0.5;
                hv_Feature = new HTuple();
                HOperatorSet.CountObj(ho_Region, out hv_NumRegions);
                if ((int)(new HTuple(hv_NumRegions.TupleGreater(0))) != 0)
                {
                    HTuple end_val7 = hv_NumLevels;
                    HTuple step_val7 = 1;
                    for (hv_I = 1; hv_I.Continue(end_val7, step_val7); hv_I = hv_I.TupleAdd(step_val7))
                    {
                        if ((int)(new HTuple(hv_I.TupleGreater(1))) != 0)
                        {
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ZoomImageFactor(ho_ImageZoom, out ExpTmpOutVar_0, hv_Zoom,
                                    hv_Zoom, "constant");
                                ho_ImageZoom.Dispose();
                                ho_ImageZoom = ExpTmpOutVar_0;
                            }
                            {
                                HObject ExpTmpOutVar_0;
                                HOperatorSet.ZoomRegion(ho_RegionZoom, out ExpTmpOutVar_0, hv_Zoom, hv_Zoom);
                                ho_RegionZoom.Dispose();
                                ho_RegionZoom = ExpTmpOutVar_0;
                            }
                            calculate_features(ho_RegionZoom, ho_ImageZoom, hv_FeatureName, out hv_Features);
                        }
                        else
                        {
                            ho_ImageZoom.Dispose();
                            HOperatorSet.CopyObj(ho_Image, out ho_ImageZoom, 1, 1);
                            ho_RegionZoom.Dispose();
                            HOperatorSet.CopyObj(ho_Region, out ho_RegionZoom, 1, hv_NumRegions);
                            calculate_features(ho_RegionZoom, ho_ImageZoom, hv_FeatureName, out hv_Features);
                            hv_FeatureLength = (new HTuple(hv_Features.TupleLength())) / hv_NumRegions;
                            hv_Step = hv_NumLevels * hv_FeatureLength;
                        }
                        hv_Indices = new HTuple();
                        HTuple end_val20 = hv_NumRegions - 1;
                        HTuple step_val20 = 1;
                        for (hv_J = 0; hv_J.Continue(end_val20, step_val20); hv_J = hv_J.TupleAdd(step_val20))
                        {
                            hv_Start = (hv_J * hv_Step) + ((hv_I - 1) * hv_FeatureLength);
                            hv_End = (hv_Start + hv_FeatureLength) - 1;
                            hv_Indices = hv_Indices.TupleConcat(HTuple.TupleGenSequence(hv_Start,
                                hv_End, 1));
                        }
                        if (hv_Feature == null)
                            hv_Feature = new HTuple();
                        hv_Feature[hv_Indices] = hv_Features;
                    }
                }
                ho_ImageZoom.Dispose();
                ho_RegionZoom.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_ImageZoom.Dispose();
                ho_RegionZoom.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Classification / Misc
        // Short Description: Calculate one or more features of a given image and/or region. 
        public void calculate_features(HObject ho_Region, HObject ho_Image, HTuple hv_FeatureNames,
            out HTuple hv_Features)
        {



            // Initialize local and output iconic variables 
            //
            //Calculate features given in FeatureNames
            //for the input regions in Region
            //(if needed supported by the underlying
            //gray-value or color image Image).
            //
            get_features(ho_Region, ho_Image, hv_FeatureNames, "calculate", out hv_Features);

            return;
        }

        // Chapter: Classification / Misc
        // Short Description: Calculate gray-value projections of polar-transformed image regions. 
        public void calc_feature_polar_gray_proj(HObject ho_Region, HObject ho_Image,
            HTuple hv_Mode, HTuple hv_Width, HTuple hv_Height, out HTuple hv_Features)
        {




            // Local iconic variables 

            HObject ho_RegionSelected = null, ho_PolarTransImage = null;
            HObject ho_EdgeAmplitude = null, ho_ImageAbs = null;

            // Local control variables 

            HTuple hv_NumRegions = null, hv_Index = null;
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Radius = new HTuple(), hv_HorProjection = new HTuple();
            HTuple hv_VertProjection = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_RegionSelected);
            HOperatorSet.GenEmptyObj(out ho_PolarTransImage);
            HOperatorSet.GenEmptyObj(out ho_EdgeAmplitude);
            HOperatorSet.GenEmptyObj(out ho_ImageAbs);
            try
            {
                //
                //Calculate gray-value projections of
                //polar-transformed image regions.
                //
                HOperatorSet.CountObj(ho_Region, out hv_NumRegions);
                hv_Features = new HTuple();
                HTuple end_val6 = hv_NumRegions;
                HTuple step_val6 = 1;
                for (hv_Index = 1; hv_Index.Continue(end_val6, step_val6); hv_Index = hv_Index.TupleAdd(step_val6))
                {
                    ho_RegionSelected.Dispose();
                    HOperatorSet.SelectObj(ho_Region, out ho_RegionSelected, hv_Index);
                    HOperatorSet.SmallestCircle(ho_RegionSelected, out hv_Row, out hv_Column,
                        out hv_Radius);
                    ho_PolarTransImage.Dispose();
                    HOperatorSet.PolarTransImageExt(ho_Image, out ho_PolarTransImage, hv_Row,
                        hv_Column, 0, (new HTuple(360)).TupleRad(), 0, ((hv_Radius.TupleConcat(
                        1))).TupleMax(), hv_Width, hv_Height, "bilinear");
                    //
                    if ((int)(new HTuple(hv_Mode.TupleEqual("hor_gray"))) != 0)
                    {
                        HOperatorSet.GrayProjections(ho_PolarTransImage, ho_PolarTransImage, "simple",
                            out hv_HorProjection, out hv_VertProjection);
                        hv_Features = hv_Features.TupleConcat(hv_HorProjection);
                    }
                    else if ((int)(new HTuple(hv_Mode.TupleEqual("vert_gray"))) != 0)
                    {
                        HOperatorSet.GrayProjections(ho_PolarTransImage, ho_PolarTransImage, "simple",
                            out hv_HorProjection, out hv_VertProjection);
                        hv_Features = hv_Features.TupleConcat(hv_VertProjection);
                    }
                    else if ((int)(new HTuple(hv_Mode.TupleEqual("hor_sobel_amp"))) != 0)
                    {
                        ho_EdgeAmplitude.Dispose();
                        HOperatorSet.SobelAmp(ho_PolarTransImage, out ho_EdgeAmplitude, "sum_abs",
                            3);
                        ho_ImageAbs.Dispose();
                        HOperatorSet.AbsImage(ho_EdgeAmplitude, out ho_ImageAbs);
                        HOperatorSet.GrayProjections(ho_ImageAbs, ho_ImageAbs, "simple", out hv_HorProjection,
                            out hv_VertProjection);
                        hv_Features = hv_Features.TupleConcat(hv_HorProjection);
                    }
                    else if ((int)(new HTuple(hv_Mode.TupleEqual("vert_sobel_amp"))) != 0)
                    {
                        ho_EdgeAmplitude.Dispose();
                        HOperatorSet.SobelAmp(ho_PolarTransImage, out ho_EdgeAmplitude, "sum_abs",
                            3);
                        ho_ImageAbs.Dispose();
                        HOperatorSet.AbsImage(ho_EdgeAmplitude, out ho_ImageAbs);
                        HOperatorSet.GrayProjections(ho_ImageAbs, ho_ImageAbs, "simple", out hv_HorProjection,
                            out hv_VertProjection);
                        hv_Features = hv_Features.TupleConcat(hv_VertProjection);
                    }
                    else if ((int)(new HTuple(hv_Mode.TupleEqual("hor_sobel_x"))) != 0)
                    {
                        ho_EdgeAmplitude.Dispose();
                        HOperatorSet.SobelAmp(ho_PolarTransImage, out ho_EdgeAmplitude, "x_binomial",
                            3);
                        ho_ImageAbs.Dispose();
                        HOperatorSet.AbsImage(ho_EdgeAmplitude, out ho_ImageAbs);
                        HOperatorSet.GrayProjections(ho_ImageAbs, ho_ImageAbs, "simple", out hv_HorProjection,
                            out hv_VertProjection);
                        hv_Features = hv_Features.TupleConcat(hv_HorProjection);
                    }
                    else if ((int)(new HTuple(hv_Mode.TupleEqual("vert_sobel_x"))) != 0)
                    {
                        ho_EdgeAmplitude.Dispose();
                        HOperatorSet.SobelAmp(ho_PolarTransImage, out ho_EdgeAmplitude, "x_binomial",
                            3);
                        ho_ImageAbs.Dispose();
                        HOperatorSet.AbsImage(ho_EdgeAmplitude, out ho_ImageAbs);
                        HOperatorSet.GrayProjections(ho_ImageAbs, ho_ImageAbs, "simple", out hv_HorProjection,
                            out hv_VertProjection);
                        hv_Features = hv_Features.TupleConcat(hv_VertProjection);
                    }
                    else if ((int)(new HTuple(hv_Mode.TupleEqual("hor_sobel_y"))) != 0)
                    {
                        ho_EdgeAmplitude.Dispose();
                        HOperatorSet.SobelAmp(ho_PolarTransImage, out ho_EdgeAmplitude, "y_binomial",
                            3);
                        ho_ImageAbs.Dispose();
                        HOperatorSet.AbsImage(ho_EdgeAmplitude, out ho_ImageAbs);
                        HOperatorSet.GrayProjections(ho_ImageAbs, ho_ImageAbs, "simple", out hv_HorProjection,
                            out hv_VertProjection);
                        hv_Features = hv_Features.TupleConcat(hv_HorProjection);
                    }
                    else if ((int)(new HTuple(hv_Mode.TupleEqual("vert_sobel_y"))) != 0)
                    {
                        ho_EdgeAmplitude.Dispose();
                        HOperatorSet.SobelAmp(ho_PolarTransImage, out ho_EdgeAmplitude, "y_binomial",
                            3);
                        ho_ImageAbs.Dispose();
                        HOperatorSet.AbsImage(ho_EdgeAmplitude, out ho_ImageAbs);
                        HOperatorSet.GrayProjections(ho_ImageAbs, ho_ImageAbs, "simple", out hv_HorProjection,
                            out hv_VertProjection);
                        hv_Features = hv_Features.TupleConcat(hv_VertProjection);
                    }
                    else
                    {
                        throw new HalconException(("Unknown Mode: " + hv_Mode) + " in calc_feature_polar_proj");
                    }
                }
                ho_RegionSelected.Dispose();
                ho_PolarTransImage.Dispose();
                ho_EdgeAmplitude.Dispose();
                ho_ImageAbs.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionSelected.Dispose();
                ho_PolarTransImage.Dispose();
                ho_EdgeAmplitude.Dispose();
                ho_ImageAbs.Dispose();

                throw HDevExpDefaultException;
            }
        }

        // Chapter: Classification / Misc
        // Short Description: This procedure contains all relevant information about the supported features. 
        public void get_features(HObject ho_Region, HObject ho_Image, HTuple hv_Namelist,
            HTuple hv_Mode, out HTuple hv_Output)
        {




            // Local iconic variables 

            // Local control variables 

            HTuple hv_EmptyRegionResult = null, hv_AccumulatedResults = null;
            HTuple hv_CustomResults = null, hv_NumRegions = null, hv_ImageWidth = null;
            HTuple hv_ImageHeight = null, hv_I = null, hv_CurrentName = new HTuple();
            HTuple hv_Name = new HTuple(), hv_Groups = new HTuple();
            HTuple hv_Feature = new HTuple(), hv_ExpDefaultCtrlDummyVar = new HTuple();
            HTuple hv_ExtendedResults = new HTuple(), hv_Row1 = new HTuple();
            HTuple hv_Column1 = new HTuple(), hv_Row2 = new HTuple();
            HTuple hv_Column2 = new HTuple(), hv_Ra = new HTuple();
            HTuple hv_Rb = new HTuple(), hv_Phi = new HTuple(), hv_Distance = new HTuple();
            HTuple hv_Sigma = new HTuple(), hv_Roundness = new HTuple();
            HTuple hv_Sides = new HTuple(), hv_NumConnected = new HTuple();
            HTuple hv_NumHoles = new HTuple(), hv_Diameter = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Anisometry = new HTuple(), hv_Bulkiness = new HTuple();
            HTuple hv_StructureFactor = new HTuple(), hv_Length1 = new HTuple();
            HTuple hv_Length2 = new HTuple(), hv_ContLength = new HTuple();
            HTuple hv_AreaHoles = new HTuple(), hv_Area = new HTuple();
            HTuple hv_Min = new HTuple(), hv_Max = new HTuple(), hv_Range = new HTuple();
            HTuple hv_Mean = new HTuple(), hv_Deviation = new HTuple();
            HTuple hv_Entropy = new HTuple(), hv_Anisotropy = new HTuple();
            HTuple hv_Size = new HTuple(), hv_NumBins = new HTuple();
            HTuple hv_NameRegExp = new HTuple(), hv_Names = new HTuple();
            HTuple hv_NumPyramids = new HTuple(), hv_Energy = new HTuple();
            HTuple hv_Correlation = new HTuple(), hv_Homogeneity = new HTuple();
            HTuple hv_Contrast = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
            HTuple hv_Projection = new HTuple(), hv_Start = new HTuple();
            HTuple hv_Histo = new HTuple(), hv_BinSize = new HTuple();
            // Initialize local and output iconic variables 
            //*********************************************************
            //Feature procedure
            //Contains the names, properties and calculation of
            //all supproted features.
            //It consists of similar blocks for each feature.
            //
            //If you like to add your own features, please use
            //the external procedure get_custom_features.hdvp
            //in the HALCON procedures/templates directory.
            //*********************************************************
            //
            //Insert location of your custom procedure here
            //
            HOperatorSet.GetSystem("empty_region_result", out hv_EmptyRegionResult);
            HOperatorSet.SetSystem("empty_region_result", "true");
            hv_AccumulatedResults = new HTuple();
            hv_CustomResults = new HTuple();
            HOperatorSet.CountObj(ho_Region, out hv_NumRegions);
            HOperatorSet.GetImageSize(ho_Image, out hv_ImageWidth, out hv_ImageHeight);
            //
            for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Namelist.TupleLength())) - 1); hv_I = (int)hv_I + 1)
            {
                hv_CurrentName = hv_Namelist.TupleSelect(hv_I);
                //
                get_custom_features(ho_Region, ho_Image, hv_CurrentName, hv_Mode, out hv_CustomResults);
                hv_AccumulatedResults = hv_AccumulatedResults.TupleConcat(hv_CustomResults);
                //
                //
                //************************************
                //HALCON REGION FEATURES
                //************************************
                //
                //************************************
                //BASIC
                //************************************
                //** area ***
                hv_Name = "area";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.AreaCenter(ho_Region, out hv_Feature, out hv_ExpDefaultCtrlDummyVar,
                        out hv_ExpDefaultCtrlDummyVar);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** width ***
                hv_Name = "width";
                hv_Groups = "region";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.SmallestRectangle1(ho_Region, out hv_Row1, out hv_Column1, out hv_Row2,
                        out hv_Column2);
                    hv_Feature = (hv_Column2 - hv_Column1) + 1;
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** height ***
                hv_Name = "height";
                hv_Groups = "region";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.SmallestRectangle1(ho_Region, out hv_Row1, out hv_Column1, out hv_Row2,
                        out hv_Column2);
                    hv_Feature = (hv_Row2 - hv_Row1) + 1;
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** ra ***
                hv_Name = "ra";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.EllipticAxis(ho_Region, out hv_Ra, out hv_Rb, out hv_Phi);
                    hv_Feature = hv_Ra.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** rb ***
                hv_Name = "rb";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.EllipticAxis(ho_Region, out hv_Ra, out hv_Rb, out hv_Phi);
                    hv_Feature = hv_Rb.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** phi ***
                hv_Name = "phi";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.EllipticAxis(ho_Region, out hv_Ra, out hv_Rb, out hv_Phi);
                    hv_Feature = hv_Phi.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** roundness ***
                hv_Name = "roundness";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.Roundness(ho_Region, out hv_Distance, out hv_Sigma, out hv_Roundness,
                        out hv_Sides);
                    hv_Feature = hv_Roundness.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** num_sides ***
                hv_Name = "num_sides";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.Roundness(ho_Region, out hv_Distance, out hv_Sigma, out hv_Roundness,
                        out hv_Sides);
                    hv_Feature = hv_Sides.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** num_connected ***
                hv_Name = "num_connected";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.ConnectAndHoles(ho_Region, out hv_NumConnected, out hv_NumHoles);
                    hv_Feature = hv_NumConnected.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** num_holes ***
                hv_Name = "num_holes";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.ConnectAndHoles(ho_Region, out hv_NumConnected, out hv_NumHoles);
                    hv_Feature = hv_NumHoles.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** area_holes ***
                hv_Name = "area_holes";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.AreaHoles(ho_Region, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** max_diameter ***
                hv_Name = "max_diameter";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.DiameterRegion(ho_Region, out hv_Row1, out hv_Column1, out hv_Row2,
                        out hv_Column2, out hv_Diameter);
                    hv_Feature = hv_Diameter.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** orientation ***
                hv_Name = "orientation";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.OrientationRegion(ho_Region, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //SHAPE
                //************************************
                //
                //************************************
                //** outer_radius ***
                hv_Name = "outer_radius";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.SmallestCircle(ho_Region, out hv_Row, out hv_Column, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** inner_radius ***
                hv_Name = "inner_radius";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.InnerCircle(ho_Region, out hv_Row, out hv_Column, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** inner_width ***
                hv_Name = "inner_width";
                hv_Groups = "region";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.InnerRectangle1(ho_Region, out hv_Row1, out hv_Column1, out hv_Row2,
                        out hv_Column2);
                    hv_Feature = (hv_Column2 - hv_Column1) + 1;
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** inner_height ***
                hv_Name = "inner_height";
                hv_Groups = "region";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.InnerRectangle1(ho_Region, out hv_Row1, out hv_Column1, out hv_Row2,
                        out hv_Column2);
                    hv_Feature = (hv_Row2 - hv_Row1) + 1;
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** circularity ***
                hv_Name = "circularity";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.Circularity(ho_Region, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** compactness ***
                hv_Name = "compactness";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.Compactness(ho_Region, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** convexity ***
                hv_Name = "convexity";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.Convexity(ho_Region, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** rectangularity ***
                hv_Name = "rectangularity";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.Rectangularity(ho_Region, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** anisometry ***
                hv_Name = "anisometry";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.Eccentricity(ho_Region, out hv_Anisometry, out hv_Bulkiness,
                        out hv_StructureFactor);
                    hv_Feature = hv_Anisometry.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** bulkiness ***
                hv_Name = "bulkiness";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.Eccentricity(ho_Region, out hv_Anisometry, out hv_Bulkiness,
                        out hv_StructureFactor);
                    hv_Feature = hv_Bulkiness.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** struct_factor ***
                hv_Name = "struct_factor";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.Eccentricity(ho_Region, out hv_Anisometry, out hv_Bulkiness,
                        out hv_StructureFactor);
                    hv_Feature = hv_StructureFactor.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** dist_mean ***
                hv_Name = "dist_mean";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.Roundness(ho_Region, out hv_Distance, out hv_Sigma, out hv_Roundness,
                        out hv_Sides);
                    hv_Feature = hv_Distance.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** dist_deviation ***
                hv_Name = "dist_deviation";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.Roundness(ho_Region, out hv_Distance, out hv_Sigma, out hv_Roundness,
                        out hv_Sides);
                    hv_Feature = hv_Sigma.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** euler_number ***
                hv_Name = "euler_number";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.EulerNumber(ho_Region, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** rect2_phi ***
                hv_Name = "rect2_phi";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.SmallestRectangle2(ho_Region, out hv_Row, out hv_Column, out hv_Phi,
                        out hv_Length1, out hv_Length2);
                    hv_Feature = hv_Phi.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** rect2_len1 ***
                hv_Name = "rect2_len1";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.SmallestRectangle2(ho_Region, out hv_Row, out hv_Column, out hv_Phi,
                        out hv_Length1, out hv_Length2);
                    hv_Feature = hv_Length1.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** rect2_len2 ***
                hv_Name = "rect2_len2";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.SmallestRectangle2(ho_Region, out hv_Row, out hv_Column, out hv_Phi,
                        out hv_Length1, out hv_Length2);
                    hv_Feature = hv_Length2.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** contlength ***
                hv_Name = "contlength";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.Contlength(ho_Region, out hv_ContLength);
                    hv_Feature = hv_ContLength.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //REGION FEATURES
                //************************************
                //MISC
                //************************************
                //** porosity ***
                hv_Name = "porosity";
                hv_Groups = new HTuple();
                hv_Groups[0] = "region";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.AreaHoles(ho_Region, out hv_AreaHoles);
                    HOperatorSet.AreaCenter(ho_Region, out hv_Area, out hv_Row, out hv_Column);
                    if ((int)(new HTuple(hv_Area.TupleEqual(0))) != 0)
                    {
                        hv_Feature = 0.0;
                    }
                    else
                    {
                        hv_Feature = (hv_AreaHoles.TupleReal()) / (hv_Area + hv_AreaHoles);
                    }
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //HALCON GRAY VALUE FEATURES
                //************************************
                //BASIC
                //************************************
                //
                //** gray_area ***
                hv_Name = "gray_area";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "rot_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.AreaCenterGray(ho_Region, ho_Image, out hv_Area, out hv_Row,
                        out hv_Column);
                    hv_Feature = hv_Area.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** gray_ra ***
                hv_Name = "gray_ra";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "rot_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.EllipticAxisGray(ho_Region, ho_Image, out hv_Ra, out hv_Rb,
                        out hv_Phi);
                    hv_Feature = hv_Ra.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** gray_rb ***
                hv_Name = "gray_rb";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "rot_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.EllipticAxisGray(ho_Region, ho_Image, out hv_Ra, out hv_Rb,
                        out hv_Phi);
                    hv_Feature = hv_Rb.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** gray_phi ***
                hv_Name = "gray_phi";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "scale_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.EllipticAxisGray(ho_Region, ho_Image, out hv_Ra, out hv_Rb,
                        out hv_Phi);
                    hv_Feature = hv_Phi.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** gray_min ***
                hv_Name = "gray_min";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.MinMaxGray(ho_Region, ho_Image, 0, out hv_Min, out hv_Max, out hv_Range);
                    hv_Feature = hv_Min.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** gray_max ***
                hv_Name = "gray_max";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.MinMaxGray(ho_Region, ho_Image, 0, out hv_Min, out hv_Max, out hv_Range);
                    hv_Feature = hv_Max.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** gray_range ***
                hv_Name = "gray_range";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.MinMaxGray(ho_Region, ho_Image, 0, out hv_Min, out hv_Max, out hv_Range);
                    hv_Feature = hv_Range.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //TEXTURE
                //************************************
                //
                //************************************
                //** gray_mean ***
                hv_Name = "gray_mean";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                hv_Groups[2] = "rot_invar";
                hv_Groups[3] = "scale_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.Intensity(ho_Region, ho_Image, out hv_Mean, out hv_Deviation);
                    hv_Feature = hv_Mean.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** gray_deviation ***
                hv_Name = "gray_deviation";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                hv_Groups[2] = "rot_invar";
                hv_Groups[3] = "scale_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.Intensity(ho_Region, ho_Image, out hv_Mean, out hv_Deviation);
                    hv_Feature = hv_Deviation.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** gray_plane_deviation ***
                hv_Name = "gray_plane_deviation";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                hv_Groups[2] = "rot_invar";
                hv_Groups[3] = "scale_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.PlaneDeviation(ho_Region, ho_Image, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** gray_anisotropy ***
                hv_Name = "gray_anisotropy";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                hv_Groups[2] = "rot_invar";
                hv_Groups[3] = "scale_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.EntropyGray(ho_Region, ho_Image, out hv_Entropy, out hv_Anisotropy);
                    hv_Feature = hv_Anisotropy.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** gray_entropy ***
                hv_Name = "gray_entropy";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                hv_Groups[2] = "rot_invar";
                hv_Groups[3] = "scale_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    HOperatorSet.EntropyGray(ho_Region, ho_Image, out hv_Entropy, out hv_Anisotropy);
                    hv_Feature = hv_Entropy.Clone();
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** gray_hor_proj ***
                hv_Name = "gray_hor_proj";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                hv_Groups[2] = "scale_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    hv_Size = 20;
                    calc_feature_gray_proj(ho_Region, ho_Image, "hor", hv_Size, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** gray_vert_proj ***
                hv_Name = "gray_vert_proj";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                hv_Groups[2] = "scale_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    hv_Size = 20;
                    calc_feature_gray_proj(ho_Region, ho_Image, "vert", hv_Size, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** gray_hor_proj_histo ***
                hv_Name = "gray_hor_proj_histo";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                hv_Groups[2] = "scale_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    hv_Size = 20;
                    calc_feature_gray_proj(ho_Region, ho_Image, "hor_histo", hv_Size, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** gray_vert_proj_histo ***
                hv_Name = "gray_vert_proj_histo";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                hv_Groups[2] = "scale_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    hv_Size = 20;
                    calc_feature_gray_proj(ho_Region, ho_Image, "vert_histo", hv_Size, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** grad_dir_histo ***
                hv_Name = "grad_dir_histo";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    hv_NumBins = 20;
                    calc_feature_grad_dir_histo(ho_Region, ho_Image, hv_NumBins, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** edge_density ***
                hv_Name = "edge_density";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                hv_Groups[2] = "rot_invar";
                hv_Groups[3] = "scale_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    calc_feature_edge_density(ho_Region, ho_Image, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** edge_density_histogram ***
                hv_Name = "edge_density_histogram";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                hv_Groups[2] = "rot_invar";
                hv_Groups[3] = "scale_invar";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    hv_NumBins = 4;
                    calc_feature_edge_density_histogram(ho_Region, ho_Image, hv_NumBins, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** edge_density_pyramid ***
                hv_NameRegExp = "edge_density_pyramid_([234])";
                hv_Names = new HTuple("edge_density_pyramid_") + HTuple.TupleGenSequence(2, 4, 1);
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                hv_Groups[2] = "rot_invar";
                hv_Groups[3] = "scale_invar";
                //****************
                if ((int)(hv_CurrentName.TupleRegexpTest(hv_NameRegExp)) != 0)
                {
                    //** Calculate feature ***
                    hv_NumPyramids = ((hv_CurrentName.TupleRegexpMatch(hv_NameRegExp))).TupleNumber()
                        ;
                    calc_feature_pyramid(ho_Region, ho_Image, "edge_density", hv_NumPyramids,
                        out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups_pyramid(hv_Mode, hv_Groups, hv_CurrentName, hv_Names,
                    hv_NameRegExp, hv_AccumulatedResults, out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** edge_density_histogram_pyramid ***
                hv_NameRegExp = "edge_density_histogram_pyramid_([234])";
                hv_Names = new HTuple("edge_density_histogram_pyramid_") + HTuple.TupleGenSequence(
                    2, 4, 1);
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                hv_Groups[2] = "rot_invar";
                hv_Groups[3] = "scale_invar";
                //****************
                if ((int)(hv_CurrentName.TupleRegexpTest(hv_NameRegExp)) != 0)
                {
                    //** Calculate feature ***
                    hv_NumPyramids = ((hv_CurrentName.TupleRegexpMatch(hv_NameRegExp))).TupleNumber()
                        ;
                    calc_feature_pyramid(ho_Region, ho_Image, "edge_density_histogram", hv_NumPyramids,
                        out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups_pyramid(hv_Mode, hv_Groups, hv_CurrentName, hv_Names,
                    hv_NameRegExp, hv_AccumulatedResults, out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //** cooc ***
                hv_Name = "cooc";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                //****************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    hv_Feature = new HTuple();
                    HOperatorSet.CoocFeatureImage(ho_Region, ho_Image, 6, 0, out hv_Energy, out hv_Correlation,
                        out hv_Homogeneity, out hv_Contrast);
                    if ((int)(new HTuple(hv_NumRegions.TupleGreater(0))) != 0)
                    {
                        hv_Index = HTuple.TupleGenSequence(0, (4 * hv_NumRegions) - 1, 4);
                        if (hv_Feature == null)
                            hv_Feature = new HTuple();
                        hv_Feature[hv_Index] = hv_Energy;
                        if (hv_Feature == null)
                            hv_Feature = new HTuple();
                        hv_Feature[1 + hv_Index] = hv_Correlation;
                        if (hv_Feature == null)
                            hv_Feature = new HTuple();
                        hv_Feature[2 + hv_Index] = hv_Homogeneity;
                        if (hv_Feature == null)
                            hv_Feature = new HTuple();
                        hv_Feature[3 + hv_Index] = hv_Contrast;
                    }
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** cooc_pyramid ***
                hv_NameRegExp = "cooc_pyramid_([234])";
                hv_Names = new HTuple("cooc_pyramid_") + HTuple.TupleGenSequence(2, 4, 1);
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "texture";
                //****************
                if ((int)(hv_CurrentName.TupleRegexpTest(hv_NameRegExp)) != 0)
                {
                    //** Calculate feature ***
                    hv_NumPyramids = ((hv_CurrentName.TupleRegexpMatch(hv_NameRegExp))).TupleNumber()
                        ;
                    calc_feature_pyramid(ho_Region, ho_Image, "cooc", hv_NumPyramids, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups_pyramid(hv_Mode, hv_Groups, hv_CurrentName, hv_Names,
                    hv_NameRegExp, hv_AccumulatedResults, out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //
                //************************************
                //
                //************************************
                //POLAR TRANSFORM FEATURES
                //************************************
                //
                //************************************
                //** polar_gray_proj ***
                hv_Name = "polar_gray_proj";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    hv_Width = 100;
                    hv_Height = 40;
                    calc_feature_polar_gray_proj(ho_Region, ho_Image, "hor_gray", hv_Width, hv_Height,
                        out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** polar_grad_proj ***
                hv_Name = "polar_grad_proj";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    hv_Width = 100;
                    hv_Height = 40;
                    calc_feature_polar_gray_proj(ho_Region, ho_Image, "hor_sobel_amp", hv_Width,
                        hv_Height, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** polar_grad_x_proj ***
                hv_Name = "polar_grad_x_proj";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    hv_Width = 100;
                    hv_Height = 40;
                    calc_feature_polar_gray_proj(ho_Region, ho_Image, "hor_sobel_x", hv_Width,
                        hv_Height, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** polar_grad_y_proj ***
                hv_Name = "polar_grad_y_proj";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    hv_Width = 100;
                    hv_Height = 40;
                    calc_feature_polar_gray_proj(ho_Region, ho_Image, "hor_sobel_y", hv_Width,
                        hv_Height, out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** polar_gray_proj_histo ***
                hv_Name = "polar_gray_proj_histo";
                hv_Groups = new HTuple();
                hv_Groups[0] = "gray";
                hv_Groups[1] = "rot_invar";
                hv_Groups[2] = "scale_invar";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    hv_Width = 100;
                    hv_Height = 40;
                    calc_feature_polar_gray_proj(ho_Region, ho_Image, "vert_gray", hv_Width,
                        hv_Height, out hv_Projection);
                    hv_NumBins = 20;
                    hv_Feature = new HTuple();
                    HTuple end_val1093 = hv_NumRegions;
                    HTuple step_val1093 = 1;
                    for (hv_Index = 1; hv_Index.Continue(end_val1093, step_val1093); hv_Index = hv_Index.TupleAdd(step_val1093))
                    {
                        hv_Start = (hv_Index - 1) * hv_Width;
                        HOperatorSet.TupleHistoRange(hv_Projection.TupleSelectRange(hv_Start, (hv_Start + hv_Width) - 1),
                            0, 255, hv_NumBins, out hv_Histo, out hv_BinSize);
                        hv_Feature = hv_Feature.TupleConcat(hv_Histo);
                    }
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //COLOR FEATURES
                //************************************
                //
                //************************************
                //** cielab_mean ***
                hv_Name = "cielab_mean";
                hv_Groups = "color";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    calc_feature_color_intensity(ho_Region, ho_Image, "cielab", "mean", out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** cielab_dev ***
                hv_Name = "cielab_dev";
                hv_Groups = "color";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    calc_feature_color_intensity(ho_Region, ho_Image, "cielab", "deviation",
                        out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** hls_mean ***
                hv_Name = "hls_mean";
                hv_Groups = "color";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    calc_feature_color_intensity(ho_Region, ho_Image, "hls", "mean", out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** hls_dev ***
                hv_Name = "hls_dev";
                hv_Groups = "color";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    calc_feature_color_intensity(ho_Region, ho_Image, "hls", "deviation", out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** rgb_mean ***
                hv_Name = "rgb_mean";
                hv_Groups = "color";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    calc_feature_color_intensity(ho_Region, ho_Image, "rgb", "mean", out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
                //************************************
                //
                //************************************
                //** rgb_dev ***
                hv_Name = "rgb_dev";
                hv_Groups = "color";
                //*************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //** Calculate feature ***
                    calc_feature_color_intensity(ho_Region, ho_Image, "rgb", "deviation", out hv_Feature);
                    //*************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_AccumulatedResults, out hv_ExtendedResults);
                    hv_AccumulatedResults = hv_ExtendedResults.Clone();
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_AccumulatedResults,
                    out hv_ExtendedResults);
                hv_AccumulatedResults = hv_ExtendedResults.Clone();
            }
            hv_Output = hv_AccumulatedResults.Clone();
            HOperatorSet.SetSystem("empty_region_result", hv_EmptyRegionResult);

            return;
        }

        // Chapter: Classification / Misc
        // Short Description: Describe and calculate user-defined features to be used in conjunction with the calculate_feature_set procedure library. 
        public void get_custom_features(HObject ho_Region, HObject ho_Image, HTuple hv_CurrentName,
            HTuple hv_Mode, out HTuple hv_Output)
        {




            // Local iconic variables 

            HObject ho_RegionSelected = null, ho_Contours = null;
            HObject ho_ContoursSelected = null, ho_ContoursSplit = null;

            // Local control variables 

            HTuple hv_TmpResults = null, hv_Name = null;
            HTuple hv_Groups = null, hv_Feature = new HTuple(), hv_NumRegions = new HTuple();
            HTuple hv_I = new HTuple(), hv_NumContours = new HTuple();
            HTuple hv_NumLines = new HTuple(), hv_J = new HTuple();
            HTuple hv_NumSplit = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_RegionSelected);
            HOperatorSet.GenEmptyObj(out ho_Contours);
            HOperatorSet.GenEmptyObj(out ho_ContoursSelected);
            HOperatorSet.GenEmptyObj(out ho_ContoursSplit);
            try
            {
                //
                //This procedure can be used to extend the functionality
                //of the calculate_feature_set procedure library by
                //user-defined features.
                //
                //Instructions:
                //
                //1. Find the template block at the beginning the procedure
                //(marked by comments) and duplicate it.
                //
                //2. In the copy edit the two marked areas as follows:
                //
                //2.1. Feature name and groups:
                //Assign a unique identifier for your feature to the variable "Name".
                //Then, assign the groups that you want your feature to belong to
                //to the variable "Groups".
                //
                //2.2. Feature calculation:
                //Enter the code that calculates your feature and
                //assign the result to the variable "Feature".
                //
                //3. Test
                //Use the "test_feature" procedure to check,
                //if the feature is calculated correctly.
                //If the procedure throws an exception,
                //maybe the order of the feature vector is wrong
                //(See note below).
                //
                //4. Integration
                //- Save your modified procedure get_custom_features.hdvp
                //  to a location of your choice.
                //  (We recommend not to overwrite the template.)
                //- Make sure, that your version of get_custom_procedures
                //  is included in the procedure directories of HDevelop.
                //  (Choose Procedures -> Manage Procedures -> Directories -> Add from the HDevelop menu bar.)
                //
                //Note:
                //The current implementation supports region arrays as input.
                //In that case, multi-dimensional feature vectors are simply concatenated.
                //Example: The feature 'center' has two dimensions [Row,Column].
                //If an array of three regions is passed, the correct order of the "Feature" variable is
                //[Row1, Column1, Row2, Column2, Row3, Column3].
                //
                hv_TmpResults = new HTuple();
                //************************************************
                //************************************************
                //**** Copy the following template block     *****
                //**** and edit the two marked code sections *****
                //**** to add user-defined features          *****
                //************************************************
                //************************************************
                //
                //***************************************
                //*********** TEMPLATE BLOCK ************
                //***************************************
                //
                //********************************************************************
                //** Section 1:
                //** Enter unique feature name and groups to which it belongs here ***
                hv_Name = "custom_feature_numlines";
                hv_Groups = "custom";
                //** Enter unique feature name and groups above this line ************
                //********************************************************************
                if ((int)(new HTuple(hv_Name.TupleEqual(hv_CurrentName))) != 0)
                {
                    //******************************************************
                    //** Section 2:
                    //** Enter code to calculate feature here **************
                    hv_Feature = new HTuple();
                    HOperatorSet.CountObj(ho_Region, out hv_NumRegions);
                    HTuple end_val69 = hv_NumRegions;
                    HTuple step_val69 = 1;
                    for (hv_I = 1; hv_I.Continue(end_val69, step_val69); hv_I = hv_I.TupleAdd(step_val69))
                    {
                        ho_RegionSelected.Dispose();
                        HOperatorSet.SelectObj(ho_Region, out ho_RegionSelected, hv_I);
                        ho_Contours.Dispose();
                        HOperatorSet.GenContourRegionXld(ho_RegionSelected, out ho_Contours, "border");
                        HOperatorSet.CountObj(ho_Contours, out hv_NumContours);
                        hv_NumLines = 0;
                        HTuple end_val74 = hv_NumContours;
                        HTuple step_val74 = 1;
                        for (hv_J = 1; hv_J.Continue(end_val74, step_val74); hv_J = hv_J.TupleAdd(step_val74))
                        {
                            ho_ContoursSelected.Dispose();
                            HOperatorSet.SelectObj(ho_Contours, out ho_ContoursSelected, hv_J);
                            ho_ContoursSplit.Dispose();
                            HOperatorSet.SegmentContoursXld(ho_ContoursSelected, out ho_ContoursSplit,
                                "lines", 5, 2, 1);
                            HOperatorSet.CountObj(ho_ContoursSplit, out hv_NumSplit);
                            hv_NumLines = hv_NumLines + hv_NumSplit;
                        }
                        hv_Feature = hv_Feature.TupleConcat(hv_NumLines);
                    }
                    //** Enter code to calculate feature above this line ***
                    //******************************************************
                    append_length_or_values(hv_Mode, hv_Feature, hv_TmpResults, out hv_TmpResults);
                }
                append_names_or_groups(hv_Mode, hv_Name, hv_Groups, hv_CurrentName, hv_TmpResults,
                    out hv_TmpResults);
                //
                //************************************
                //****** END OF TEMPLATE BLOCK *******
                //************************************
                //
                hv_Output = hv_TmpResults.Clone();
                ho_RegionSelected.Dispose();
                ho_Contours.Dispose();
                ho_ContoursSelected.Dispose();
                ho_ContoursSplit.Dispose();

                return;
            }
            catch (HalconException HDevExpDefaultException)
            {
                ho_RegionSelected.Dispose();
                ho_Contours.Dispose();
                ho_ContoursSelected.Dispose();
                ho_ContoursSplit.Dispose();

                throw HDevExpDefaultException;
            }
        }

        public void calculate_lines_gauss_parameters(HTuple hv_MaxLineWidth, HTuple hv_Contrast,
      out HTuple hv_Sigma, out HTuple hv_Low, out HTuple hv_High)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_ContrastHigh = null, hv_ContrastLow = new HTuple();
            HTuple hv_HalfWidth = null, hv_Help = null;
            HTuple hv_MaxLineWidth_COPY_INP_TMP = hv_MaxLineWidth.Clone();

            // Initialize local and output iconic variables 
            //Check control parameters
            if ((int)(new HTuple((new HTuple(hv_MaxLineWidth_COPY_INP_TMP.TupleLength())).TupleNotEqual(
                1))) != 0)
            {
                throw new HalconException("Wrong number of values of control parameter: 1");
            }
            if ((int)(((hv_MaxLineWidth_COPY_INP_TMP.TupleIsNumber())).TupleNot()) != 0)
            {
                throw new HalconException("Wrong type of control parameter: 1");
            }
            if ((int)(new HTuple(hv_MaxLineWidth_COPY_INP_TMP.TupleLessEqual(0))) != 0)
            {
                throw new HalconException("Wrong value of control parameter: 1");
            }
            if ((int)((new HTuple((new HTuple(hv_Contrast.TupleLength())).TupleNotEqual(1))).TupleAnd(
                new HTuple((new HTuple(hv_Contrast.TupleLength())).TupleNotEqual(2)))) != 0)
            {
                throw new HalconException("Wrong number of values of control parameter: 2");
            }
            if ((int)(new HTuple(((((hv_Contrast.TupleIsNumber())).TupleMin())).TupleEqual(
                0))) != 0)
            {
                throw new HalconException("Wrong type of control parameter: 2");
            }
            //Set and check ContrastHigh
            hv_ContrastHigh = hv_Contrast.TupleSelect(0);
            if ((int)(new HTuple(hv_ContrastHigh.TupleLess(0))) != 0)
            {
                throw new HalconException("Wrong value of control parameter: 2");
            }
            //Set or derive ContrastLow
            if ((int)(new HTuple((new HTuple(hv_Contrast.TupleLength())).TupleEqual(2))) != 0)
            {
                hv_ContrastLow = hv_Contrast.TupleSelect(1);
            }
            else
            {
                hv_ContrastLow = hv_ContrastHigh / 3.0;
            }
            //Check ContrastLow
            if ((int)(new HTuple(hv_ContrastLow.TupleLess(0))) != 0)
            {
                throw new HalconException("Wrong value of control parameter: 2");
            }
            if ((int)(new HTuple(hv_ContrastLow.TupleGreater(hv_ContrastHigh))) != 0)
            {
                throw new HalconException("Wrong value of control parameter: 2");
            }
            //
            //Calculate the parameters Sigma, Low, and High for lines_gauss
            if ((int)(new HTuple(hv_MaxLineWidth_COPY_INP_TMP.TupleLess((new HTuple(3.0)).TupleSqrt()
                ))) != 0)
            {
                //Note that LineWidthMax < sqrt(3.0) would result in a Sigma < 0.5,
                //which does not make any sense, because the corresponding smoothing
                //filter mask would be of size 1x1.
                //To avoid this, LineWidthMax is restricted to values greater or equal
                //to sqrt(3.0) and the contrast values are adapted to reflect the fact
                //that lines that are thinner than sqrt(3.0) pixels have a lower contrast
                //in the smoothed image (compared to lines that are sqrt(3.0) pixels wide).
                hv_ContrastLow = (hv_ContrastLow * hv_MaxLineWidth_COPY_INP_TMP) / ((new HTuple(3.0)).TupleSqrt()
                    );
                hv_ContrastHigh = (hv_ContrastHigh * hv_MaxLineWidth_COPY_INP_TMP) / ((new HTuple(3.0)).TupleSqrt()
                    );
                hv_MaxLineWidth_COPY_INP_TMP = (new HTuple(3.0)).TupleSqrt();
            }
            //Convert LineWidthMax and the given contrast values into the input parameters
            //Sigma, Low, and High required by lines_gauss
            hv_HalfWidth = hv_MaxLineWidth_COPY_INP_TMP / 2.0;
            hv_Sigma = hv_HalfWidth / ((new HTuple(3.0)).TupleSqrt());
            hv_Help = ((-2.0 * hv_HalfWidth) / (((new HTuple(6.283185307178)).TupleSqrt()) * (hv_Sigma.TuplePow(
                3.0)))) * (((-0.5 * (((hv_HalfWidth / hv_Sigma)).TuplePow(2.0)))).TupleExp());
            hv_High = ((hv_ContrastHigh * hv_Help)).TupleFabs();
            hv_Low = ((hv_ContrastLow * hv_Help)).TupleFabs();

            return;
        }
    }
}
