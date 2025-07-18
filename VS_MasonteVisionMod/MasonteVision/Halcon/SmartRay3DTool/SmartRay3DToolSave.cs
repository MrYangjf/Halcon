using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using MasonteVision.Halcon;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using MasonteDataProcess.FileProcess;
using HalconDotNet;

namespace MasonteVision.Halcon.SmartRay3DTool
{
    public static class SmartRay3DToolSave
    {
        public static HTuple hv_HomMat2DIdentity;
        public static HObject Zmap;
        static XMLFile myXMLfile = new XMLFile(Application.StartupPath + "\\INI\\SmartRayTool.XML");
        public static void Save(CreatAffineModel Tool)
        {
            Tool.SaveModel(Application.StartupPath + "\\INI");
            string ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<CreatAffineModel>(Tool);
            myXMLfile.WriteValue("SmartRay3DToolSave", "CreatAffineModel", "ParamJson", ParamJson);
        }

        public static void Save(FitSurface Tool)
        {
            string ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<FitSurface>(Tool);
            myXMLfile.WriteValue("SmartRay3DToolSave", "FitSurface", "ParamJson", ParamJson);
        }

        public static void Save(PointDist Tool)
        {
            string ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<PointDist>(Tool);
            myXMLfile.WriteValue("SmartRay3DToolSave", "PointDist", "ParamJson", ParamJson);
        }

        public static CreatAffineModel LoadCreatAffineModel()
        {
            CreatAffineModel creatAffineModel;
            if (!myXMLfile.ReadXML()) return null;
            string ParamJson = myXMLfile.GetStringValue("SmartRay3DToolSave", "CreatAffineModel", "ParamJson");
            creatAffineModel = NewtonsoftJsonExtension.DeserializeObjectFromJson_NJ<CreatAffineModel>(ParamJson);
            creatAffineModel.ReadModel(Application.StartupPath + "\\INI\\SmartRayToolMatch.dat");
            return creatAffineModel;
        }

        public static FitSurface LoadFitSurface()
        {
            if (!myXMLfile.ReadXML()) return null;
            string ParamJson = myXMLfile.GetStringValue("SmartRay3DToolSave", "FitSurface", "ParamJson");
            return NewtonsoftJsonExtension.DeserializeObjectFromJson_NJ<FitSurface>(ParamJson);
        }

        public static PointDist LoadPointDist()
        {
            if (!myXMLfile.ReadXML()) return null;
            string ParamJson = myXMLfile.GetStringValue("SmartRay3DToolSave", "PointDist", "ParamJson");
            return NewtonsoftJsonExtension.DeserializeObjectFromJson_NJ<PointDist>(ParamJson);
        }

        public static void Run3DtestFlow(CreatAffineModel my3DMatchTool, FitSurface my3DFitTool, PointDist my3DPointDistTool,HaGUI mHaGUI)
        {
            if (my3DMatchTool == null) return;
            my3DMatchTool.RunVision(mHaGUI.MyImage, mHaGUI, my3DMatchTool.ROIFindCtrl);
            if (my3DFitTool == null) return;
            my3DFitTool.RunVision(mHaGUI.MyImage, mHaGUI,out Zmap);
            if (my3DPointDistTool == null) return;
            my3DPointDistTool.RunVision(Zmap, mHaGUI, my3DFitTool.Beta, my3DFitTool.Alpha, my3DFitTool.Bias, my3DFitTool.Down);
            mHaGUI.MyHWndControl.DispMessage("高度数据为" + my3DPointDistTool.Distance.ToString("0.0000"), 10, 10, "green");
        }

    }
}
