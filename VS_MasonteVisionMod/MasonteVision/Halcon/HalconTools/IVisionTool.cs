
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MasonteVision.Halcon.VisionTool;
using System.Drawing;
using System.ComponentModel;

namespace MasonteVision.Halcon
{
    public enum VisionToolType
    {
        IVisionTool,
        VisionBlob,
        VisionBarCode,
        VisionCircle,
        VisionCross,
        VisionLine,
        VisionTemplate,
        VisionTemplateNCC,
        VisionTemplateScaled,
        VisionTemplateOCR,
        VisionClarity,
        VisionCam,
        VisionPadPanic,
        VisionDataExchange
    };
    public enum VisionType
    {
        未分类工具 = -1,
        图像采集 = 0,
        图像预处理 = 1,
        测量类 = 2,
        形态分析 = 3,
        模板匹配 = 4,
        数据处理 = 5,
        显示相关 = 6,
        精准定位 = 7,
        扫码功能 = 8
    };
    [XmlInclude(typeof(IVisionTool)),
     XmlInclude(typeof(VisionBlob)),
     XmlInclude(typeof(VisionBarCode)),
     XmlInclude(typeof(VisionCircle)),
     XmlInclude(typeof(VisionCross)),
     XmlInclude(typeof(VisionLine)),
     XmlInclude(typeof(VisionTemplate)),
     XmlInclude(typeof(VisionTemplateNCC)),
     XmlInclude(typeof(VisionTemplateScaled)),
     XmlInclude(typeof(VisionTemplateOCR)),
     ]

    [Serializable]
    public class IVisionTool
    {
        public IVisionTool()
        {
            //hWindow = new HaGUI();
        }
        [DisplayName("工具名称"), Category("工具属性"), Browsable(false)]
        public string ToolName { get; set; } = "视觉工具";
        [DisplayName("名称"), Category("工具属性"), Browsable(true)]
        public string ReName { get; set; } = "视觉工具";
        [DisplayName("流程序号"), Category("工具属性"), Browsable(false)]
        public string IndexID { get; set; } = "";
        //[DisplayName("图标"), Category("工具属性"), Browsable(false), XmlIgnore]
        //public Image IcoImage { get; set; } = Properties.Resources.settings1;
        [DisplayName("工具类型"), Category("工具属性"), Browsable(false)]
        public VisionToolType visionToolType { get; set; } = VisionToolType.IVisionTool;
        [DisplayName("工具分类"), Category("工具属性"), Browsable(false)]
        public VisionType VisionType { get; set; } = VisionType.未分类工具;
        public virtual void SettingEnter() { }

        public virtual void SettingROI(ROI MyROI) { }
        public virtual bool RunVision(HObject Image, HaGUI hWindow, int ROIindex) { return false; }

        public virtual bool RunVision(HObject Image, HaGUI hWindow) { return false; }
        public virtual bool RunVision(HObject Image) { return false; }

        public virtual void ShowResultText(HaGUI hWindow) { }

        [DisplayName("是否禁用"), Category("控制"), Browsable(true), Description("是否在流程中禁用运行此工具")]
        public bool ToolDisable { get; } = false;

        [DisplayName("综合判定"), Category("控制"), Browsable(true), Description("工具结果的综合判定")]
        public bool ToolResult { get; set; } = false;
        [DisplayName("综合判定"), Category("控制"), Browsable(true), Description("运行结果的综合判定")]
        public bool RunResult { get; set; } = false;


        //[XmlIgnore]


    }
}
