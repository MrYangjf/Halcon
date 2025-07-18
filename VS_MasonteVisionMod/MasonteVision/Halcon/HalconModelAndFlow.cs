using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MasonteDataProcess.FileProcess;
using System.Windows.Forms;
using HalconDotNet;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using MasonteVision.Halcon.VisionTool;
using MasonteVision.PCB_AOI_ZY_CAM;

namespace MasonteVision.Halcon
{


    /// <summary>
    /// Newtonsoft.Json的扩展方法类
    /// </summary>
    public static class NewtonsoftJsonExtension
    {
        /// <summary>
        /// 将Json字符串反序列化为对象实例——Newtonsoft.Json
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonString">Json字符串</param>
        /// <returns>对象实例</returns>
        public static T DeserializeObjectFromJson_NJ<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// 将对象实例序列化为Json字符串——Newtonsoft.Json
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象实例</param>
        /// <returns>Json字符串</returns>
        public static string SerializeObjectToJson_NJ<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将序列化的json字符串内容写入Json文件，并且保存
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="jsonConents">Json内容</param>
        public static void WriteJsonFile(string path, string jsonConents)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.WriteLine(jsonConents);
                }
            }
        }

        /// <summary>
        /// 获取到本地的Json文件并且解析返回对应的json字符串
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns>Json内容</returns>
        public static string GetJsonFile(string filepath)
        {
            string json = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            return json;
        }
    }



    public class HalconModelAndFlow
    {
        /// <summary>
        /// 保存方式
        /// </summary>
        /// <param name="TestPadName">项目名字</param>
        /// <param name="GUIIndex">GUI顺序</param>
        /// <param name="ParamIndex">点色序列</param>
        public HalconModelAndFlow(string TestPadName, int GUIIndex, int ParamIndex)
        {
            Item = TestPadName;
            GuiIndex = GUIIndex;
            SaveParamIndex = ParamIndex;

            string Path = string.Format(Application.StartupPath + "\\INI\\{1}\\{0}\\", GuiIndex, Item);

            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            myXMLfile = new XMLFile(string.Format(Application.StartupPath + "\\INI\\{2}\\{0}\\HalconParam{1}.XML", GuiIndex, SaveParamIndex, Item));
            myCamXMLfile = new XMLFile(string.Format(Application.StartupPath + "\\INI\\{1}\\{0}\\Cam.XML", GuiIndex, Item));
        }

        public HalconModelAndFlow(string TestPadName, int GUIIndex, int ParamIndex, bool isCover)
        {
            string TempItem = Item;
            int TempGuiIndex = GuiIndex;
            int TempSaveParamIndex = SaveParamIndex;

            Item = TestPadName;
            GuiIndex = GUIIndex;
            SaveParamIndex = ParamIndex;

            string oldpath= string.Format(Application.StartupPath + "\\INI\\{1}\\{0}\\", TempGuiIndex, TempItem);
            string Path = string.Format(Application.StartupPath + "\\INI\\{1}\\{0}\\", GuiIndex, Item);
            myXMLfile = new XMLFile(string.Format(Application.StartupPath + "\\INI\\{2}\\{0}\\HalconParam{1}.XML", GuiIndex, SaveParamIndex, Item));
            myCamXMLfile = new XMLFile(string.Format(Application.StartupPath + "\\INI\\{1}\\{0}\\Cam.XML", GuiIndex, Item));

            if (Directory.Exists(Path)) return;
            Directory.CreateDirectory(Path);
            DirectoryInfo myNewDirectory = new DirectoryInfo(oldpath);
            for (int i = 0; i < myNewDirectory.GetFiles().Length; i++)
            {
                myNewDirectory.GetFiles()[i].CopyTo(Path + myNewDirectory.GetFiles()[i].Name, isCover);
            }
        }

        public HalconModelAndFlow(int GUIIndex, int ParamIndex)
        {
            GuiIndex = GUIIndex;
            SaveParamIndex = ParamIndex;

            string Path = string.Format(Application.StartupPath + "\\INI\\{1}\\{0}\\", GuiIndex, Item);

            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            myXMLfile = new XMLFile(string.Format(Application.StartupPath + "\\INI\\{2}\\{0}\\HalconParam{1}.XML", GuiIndex, SaveParamIndex, Item));
            myCamXMLfile = new XMLFile(string.Format(Application.StartupPath + "\\INI\\{1}\\{0}\\Cam.XML", GuiIndex, Item));
        }



        public const int OKFlag = 0;
        public const int NGFlag = 1;
        public const int ERRORFlag = -1;

        public int GuiIndex = -1;
        public int SaveParamIndex = -1;
        public VisionPadPanic[] MyAllParam;
 
        XMLFile[] myAllParamXMLfile;

        XMLFile myXMLfile;
        XMLFile myCamXMLfile;

        public string Item;


        public void  LoadAllParamFromXML(int AllParamCount)
        {
            myAllParamXMLfile = new XMLFile[AllParamCount];
            MyAllParam = new VisionPadPanic[AllParamCount];
            for(int m=0;m< AllParamCount;m++)
            {
                myAllParamXMLfile[m] = new XMLFile(string.Format(Application.StartupPath + "\\INI\\{2}\\{0}\\HalconParam{1}.XML", GuiIndex, m, Item));

                if (myAllParamXMLfile[m].ReadXML())
                {
                    for (int i = 0; i < myAllParamXMLfile[m]._xmlDoc.SelectSingleNode("ROIList").ChildNodes.Count; i++)
                    {
                        string name = myAllParamXMLfile[m]._xmlDoc.SelectSingleNode("ROIList").ChildNodes[i].Name;
                        string t = name.Substring(0, 3);
                      
                        if (t == "Too")
                        {
                            string Type = myAllParamXMLfile[m].GetStringValue("ROIList", name, "type");
                            string XMLstr = "";
                            switch (Type)
                            {
                                case "Pad脏污分析":
                                    XMLstr = myAllParamXMLfile[m].GetStringValue("ROIList", name, "ParamJson");
                                    MyAllParam[m] = NewtonsoftJsonExtension.DeserializeObjectFromJson_NJ<VisionPadPanic>(XMLstr);
                                    break;
                            }
                        }

                    }
                    for (int i = 0; i < myAllParamXMLfile[m]._xmlDoc.SelectSingleNode("ROIList").ChildNodes.Count; i++)
                    {
                        string name = myAllParamXMLfile[m]._xmlDoc.SelectSingleNode("ROIList").ChildNodes[i].Name;
                        string t = name.Substring(0, 3);

                        if (t == "ROI")
                        {
                            string Type = myAllParamXMLfile[m].GetStringValue("ROIList", "ROI" + i.ToString(), "Type");
                            switch (Type)
                            {
                                case "ROICircle":
                                    ROICircle myROICircle = new ROICircle();
                                    myROICircle.MIDC = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC");
                                    myROICircle.MIDR = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR");
                                    myROICircle.COL = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "COL");
                                    myROICircle.ROW = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW");
                                    myROICircle.RADIUS = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "RADIUS");
                                    MyAllParam[m].ROIFindCtrl = myROICircle;
                                    break;
                                case "ROILine":
                                    ROILine myROILine = new ROILine();
                                    myROILine.MIDC = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "midC");
                                    myROILine.MIDR = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "midR");
                                    myROILine.COL1 = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "COL1");
                                    myROILine.ROW1 = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW1");
                                    myROILine.COL2 = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "COL2");
                                    myROILine.ROW2 = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW2");
                                    MyAllParam[m].ROIFindCtrl = myROILine;
                                    break;
                                case "ROICircularArc":
                                    ROICircularArc myROICircularArc = new ROICircularArc();
                                    myROICircularArc.MIDC = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC");
                                    myROICircularArc.MIDR = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR");
                                    myROICircularArc.RADIUS = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "RADIUS");
                                    myROICircularArc.StartR = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "StartR");
                                    myROICircularArc.StartC = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "StartC");
                                    myROICircularArc.SIZER = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "SIZER");
                                    myROICircularArc.SIZEC = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "SIZEC");
                                    myROICircularArc.StartPhi = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "StartPhi");
                                    myROICircularArc.ExtentPhi = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "ExtentPhi");
                                    MyAllParam[m].ROIFindCtrl = myROICircularArc;
                                    break;
                                case "ROIRectangle1":
                                    ROIRectangle1 myROIRectangle1 = new ROIRectangle1();
                                    myROIRectangle1.MIDC = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC");
                                    myROIRectangle1.MIDR = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR");
                                    myROIRectangle1.COL1 = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "COL1");
                                    myROIRectangle1.ROW1 = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW1");
                                    myROIRectangle1.COL2 = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "COL2");
                                    myROIRectangle1.ROW2 = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW2");
                                    MyAllParam[m].ROIFindCtrl = myROIRectangle1;
                                    break;
                                case "ROIRectangle2":
                                    ROIRectangle2 myROIRectangle2 = new ROIRectangle2();
                                    myROIRectangle2.MIDC = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC");
                                    myROIRectangle2.MIDR = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR");
                                    myROIRectangle2.LENGTH1 = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "LENGTH1");
                                    myROIRectangle2.LENGTH2 = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "LENGTH2");
                                    myROIRectangle2.Phi = myAllParamXMLfile[m].GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "Phi");
                                    MyAllParam[m].ROIFindCtrl = myROIRectangle2;
                                    break;
                            }
                        }

                    }
                }
            }
        }

        public void SaveMyHalconFlow(string XMLfile, HaGUI myHaGUI)
        {
            XMLFile myNewXMLfile = new XMLFile(XMLfile);
            myNewXMLfile.ClearXML();
            for (int i = 0; i < myHaGUI.MyROIController.ROIList.Count; i++)
            {
                int Numhandeles = ((ROI)myHaGUI.MyROIController.ROIList[i]).getNumHandles();
                switch (Numhandeles)
                {
                    case 2:
                        ROICircle myROICircle = new ROICircle();
                        myROICircle = ((ROICircle)myHaGUI.MyROIController.ROIList[i]);
                        myNewXMLfile.WriteValue("ROIList", "ROIList", "ROI" + i.ToString(), "Type", "ROICircle");
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "index", i);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "Numhandeles", Numhandeles);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC", myROICircle.MIDC);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR", myROICircle.MIDR);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "COL", myROICircle.COL);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW", myROICircle.ROW);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "RADIUS", myROICircle.RADIUS);
                        break;
                    case 3:
                        ROILine myROILine = new ROILine();
                        myROILine = ((ROILine)myHaGUI.MyROIController.ROIList[i]);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Type", "ROILine");
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "index", i);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "Numhandeles", Numhandeles);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "midC", myROILine.MIDC);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "midR", myROILine.MIDR);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "COL1", myROILine.COL1);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW1", myROILine.ROW1);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "COL2", myROILine.COL2);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW2", myROILine.ROW2);
                        break;
                    case 4:
                        ROICircularArc myROICircleArr = new ROICircularArc();
                        myROICircleArr = ((ROICircularArc)myHaGUI.MyROIController.ROIList[i]);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Type", "ROICircularArc");
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "index", i);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "Numhandeles", Numhandeles);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC", myROICircleArr.MIDC);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR", myROICircleArr.MIDR);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "StartR", myROICircleArr.StartR);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "StartC", myROICircleArr.StartC);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "SIZER", myROICircleArr.SIZER);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "SIZEC", myROICircleArr.SIZEC);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "StartPhi", myROICircleArr.StartPhi);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "ExtentPhi", myROICircleArr.ExtentPhi);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "RADIUS", myROICircleArr.RADIUS);
                        break;
                    case 5:
                        ROIRectangle1 MyROIRectangle1 = new ROIRectangle1();
                        MyROIRectangle1 = ((ROIRectangle1)myHaGUI.MyROIController.ROIList[i]);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Type", "ROIRectangle1");
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "index", i);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "Numhandeles", Numhandeles);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC", MyROIRectangle1.MIDC);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR", MyROIRectangle1.MIDR);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW1", MyROIRectangle1.ROW1);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW2", MyROIRectangle1.ROW2);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "COL1", MyROIRectangle1.COL1);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "COL2", MyROIRectangle1.COL2);
                        break;
                    case 6:
                        ROIRectangle2 MyROIRectangle2 = new ROIRectangle2();
                        MyROIRectangle2 = ((ROIRectangle2)myHaGUI.MyROIController.ROIList[i]);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Type", "ROIRectangle2");
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "index", i);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "Numhandeles", Numhandeles);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC", MyROIRectangle2.MIDC);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR", MyROIRectangle2.MIDR);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "LENGTH1", MyROIRectangle2.LENGTH1);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "LENGTH2", MyROIRectangle2.LENGTH2);
                        myNewXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "Phi", MyROIRectangle2.Phi);
                        break;
                }
            }
            for (int i = 0; i < myHaGUI.MyToolsController.VisionToolList.Count; i++)
            {
                IVisionTool myTool = (IVisionTool)myHaGUI.MyToolsController.VisionToolList[i];
                string ToolsName = myTool.ToolName;
                string ParamJson = "";
                switch (ToolsName)
                {
                    case "找直线":
                        VisionLine myLineTool = (VisionLine)myHaGUI.MyToolsController.VisionToolList[i];
                        ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<VisionLine>(myLineTool);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "type", myLineTool.ToolName);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "FlowID", myLineTool.IndexID);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "ParamJson", ParamJson);
                        break;
                    case "Blob分析":
                        VisionBlob myBlobTool = (VisionBlob)myHaGUI.MyToolsController.VisionToolList[i];
                        ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<VisionBlob>(myBlobTool);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "type", myBlobTool.ToolName);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "FlowID", myBlobTool.IndexID);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "ParamJson", ParamJson);
                        break;
                    case "Pad脏污分析":
                        VisionPadPanic myVisionPadTool = (VisionPadPanic)myHaGUI.MyToolsController.VisionToolList[i];
                        ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<VisionPadPanic>(myVisionPadTool);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "type", myVisionPadTool.ToolName);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "FlowID", myVisionPadTool.IndexID);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "ParamJson", ParamJson);
                        break;
                    case "找圆工具":
                        VisionCircle myCircleTool = (VisionCircle)myHaGUI.MyToolsController.VisionToolList[i];
                        ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<VisionCircle>(myCircleTool);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "type", myCircleTool.ToolName);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "FlowID", myCircleTool.IndexID);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "ParamJson", ParamJson);
                        break;
                    case "二维码":

                        VisionBarCode myBarCodeTool = (VisionBarCode)myHaGUI.MyToolsController.VisionToolList[i];
                        ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<VisionBarCode>(myBarCodeTool);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "type", myBarCodeTool.ToolName);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "FlowID", myBarCodeTool.IndexID);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "ParamJson", ParamJson);
                        break;
                    case "模板匹配":

                        VisionTemplate myTemplateTool = (VisionTemplate)myHaGUI.MyToolsController.VisionToolList[i];
                        ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<VisionTemplate>(myTemplateTool);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "type", myTemplateTool.ToolName);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "FlowID", myTemplateTool.IndexID);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "ParamJson", ParamJson);
                        break;
                    case "清晰度分析":
                        VisionClarity mClarity = (VisionClarity)myHaGUI.MyToolsController.VisionToolList[i];
                        ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<VisionClarity>(mClarity);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "type", mClarity.ToolName);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "FlowID", mClarity.IndexID);
                        myNewXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "ParamJson", ParamJson);
                        break;
                }
            }
        }

        public void SaveMyHalconFlow(HaGUI myHaGUI)
        {
            myXMLfile.ClearXML();
            for (int i = 0; i < myHaGUI.MyROIController.ROIList.Count; i++)
            {
                int Numhandeles = ((ROI)myHaGUI.MyROIController.ROIList[i]).getNumHandles();
                switch (Numhandeles)
                {
                    case 2:
                        ROICircle myROICircle = new ROICircle();
                        myROICircle = ((ROICircle)myHaGUI.MyROIController.ROIList[i]);
                        myXMLfile.WriteValue("ROIList", "ROIList", "ROI" + i.ToString(), "Type", "ROICircle");
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "index", i);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "Numhandeles", Numhandeles);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC", myROICircle.MIDC);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR", myROICircle.MIDR);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "COL", myROICircle.COL);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW", myROICircle.ROW);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "RADIUS", myROICircle.RADIUS);
                        break;
                    case 3:
                        ROILine myROILine = new ROILine();
                        myROILine = ((ROILine)myHaGUI.MyROIController.ROIList[i]);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Type", "ROILine");
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "index", i);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "Numhandeles", Numhandeles);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "midC", myROILine.MIDC);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "midR", myROILine.MIDR);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "COL1", myROILine.COL1);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW1", myROILine.ROW1);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "COL2", myROILine.COL2);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW2", myROILine.ROW2);
                        break;
                    case 4:
                        ROICircularArc myROICircleArr = new ROICircularArc();
                        myROICircleArr = ((ROICircularArc)myHaGUI.MyROIController.ROIList[i]);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Type", "ROICircularArc");
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "index", i);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "Numhandeles", Numhandeles);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC", myROICircleArr.MIDC);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR", myROICircleArr.MIDR);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "StartR", myROICircleArr.StartR);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "StartC", myROICircleArr.StartC);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "SIZER", myROICircleArr.SIZER);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "SIZEC", myROICircleArr.SIZEC);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "StartPhi", myROICircleArr.StartPhi);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "ExtentPhi", myROICircleArr.ExtentPhi);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "RADIUS", myROICircleArr.RADIUS);
                        break;
                    case 5:
                        ROIRectangle1 MyROIRectangle1 = new ROIRectangle1();
                        MyROIRectangle1 = ((ROIRectangle1)myHaGUI.MyROIController.ROIList[i]);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Type", "ROIRectangle1");
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "index", i);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "Numhandeles", Numhandeles);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC", MyROIRectangle1.MIDC);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR", MyROIRectangle1.MIDR);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW1", MyROIRectangle1.ROW1);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW2", MyROIRectangle1.ROW2);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "COL1", MyROIRectangle1.COL1);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "COL2", MyROIRectangle1.COL2);
                        break;
                    case 6:
                        ROIRectangle2 MyROIRectangle2 = new ROIRectangle2();
                        MyROIRectangle2 = ((ROIRectangle2)myHaGUI.MyROIController.ROIList[i]);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Type", "ROIRectangle2");
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "index", i);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Properties", "Numhandeles", Numhandeles);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC", MyROIRectangle2.MIDC);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR", MyROIRectangle2.MIDR);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "LENGTH1", MyROIRectangle2.LENGTH1);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "LENGTH2", MyROIRectangle2.LENGTH2);
                        myXMLfile.WriteValue("ROIList", "ROI" + i.ToString(), "Coord", "Phi", MyROIRectangle2.Phi);
                        break;
                }
            }
            for (int i = 0; i < myHaGUI.MyToolsController.VisionToolList.Count; i++)
            {
                IVisionTool myTool = (IVisionTool)myHaGUI.MyToolsController.VisionToolList[i];
                string ToolsName = myTool.ToolName;
                string ParamJson = "";
                switch (ToolsName)
                {
                    case "找直线":
                        VisionLine myLineTool = (VisionLine)myHaGUI.MyToolsController.VisionToolList[i];
                        ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<VisionLine>(myLineTool);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "type", myLineTool.ToolName);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "FlowID", myLineTool.IndexID);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "ParamJson", ParamJson);
                        break;
                    case "Blob分析":
                        VisionBlob myBlobTool = (VisionBlob)myHaGUI.MyToolsController.VisionToolList[i];
                        ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<VisionBlob>(myBlobTool);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "type", myBlobTool.ToolName);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "FlowID", myBlobTool.IndexID);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "ParamJson", ParamJson);
                        break;
                    case "Pad脏污分析":
                        VisionPadPanic myVisionPadTool = (VisionPadPanic)myHaGUI.MyToolsController.VisionToolList[i];
                        ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<VisionPadPanic>(myVisionPadTool);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "type", myVisionPadTool.ToolName);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "FlowID", myVisionPadTool.IndexID);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "ParamJson", ParamJson);
                        break;
                    case "找圆工具":
                        VisionCircle myCircleTool = (VisionCircle)myHaGUI.MyToolsController.VisionToolList[i];
                        ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<VisionCircle>(myCircleTool);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "type", myCircleTool.ToolName);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "FlowID", myCircleTool.IndexID);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "ParamJson", ParamJson);
                        break;
                    case "二维码":

                        VisionBarCode myBarCodeTool = (VisionBarCode)myHaGUI.MyToolsController.VisionToolList[i];
                        ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<VisionBarCode>(myBarCodeTool);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "type", myBarCodeTool.ToolName);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "FlowID", myBarCodeTool.IndexID);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "ParamJson", ParamJson);
                        break;
                    case "模板匹配":

                        VisionTemplate myTemplateTool = (VisionTemplate)myHaGUI.MyToolsController.VisionToolList[i];
                        ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<VisionTemplate>(myTemplateTool);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "type", myTemplateTool.ToolName);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "FlowID", myTemplateTool.IndexID);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "ParamJson", ParamJson);
                        break;
                    case "清晰度分析":
                        VisionClarity mClarity = (VisionClarity)myHaGUI.MyToolsController.VisionToolList[i];
                        ParamJson = NewtonsoftJsonExtension.SerializeObjectToJson_NJ<VisionClarity>(mClarity);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "type", mClarity.ToolName);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "FlowID", mClarity.IndexID);
                        myXMLfile.WriteValue("ROIList", "Tools" + i.ToString(), "ParamJson", ParamJson);
                        break;
                }
            }
        }

        public void LoadHalconFlow(HaGUI myHaGUI)
        {
            if (!myXMLfile.ReadXML()) return;
            myHaGUI.MyROIController.reset();
            myHaGUI.MyHWndControl.repaint();
            myHaGUI.MyToolsController.VisionToolList.Clear();
            for (int i = 0; i < myXMLfile._xmlDoc.SelectSingleNode("ROIList").ChildNodes.Count; i++)
            {
                string name = myXMLfile._xmlDoc.SelectSingleNode("ROIList").ChildNodes[i].Name;
                string t = name.Substring(0, 3);
                if (t == "ROI")
                {
                    string Type = myXMLfile.GetStringValue("ROIList", "ROI" + i.ToString(), "Type");
                    switch (Type)
                    {
                        case "ROICircle":
                            ROICircle myROICircle = new ROICircle();
                            myROICircle.MIDC = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC");
                            myROICircle.MIDR = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR");
                            myROICircle.COL = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "COL");
                            myROICircle.ROW = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW");
                            myROICircle.RADIUS = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "RADIUS");
                            myHaGUI.MyROIController.ROIList.Add(myROICircle);
                            myHaGUI.MyROIController.paintData(myHaGUI.MyWindow);
                            break;
                        case "ROILine":
                            ROILine myROILine = new ROILine();
                            myROILine.MIDC = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "midC");
                            myROILine.MIDR = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "midR");
                            myROILine.COL1 = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "COL1");
                            myROILine.ROW1 = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW1");
                            myROILine.COL2 = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "COL2");
                            myROILine.ROW2 = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW2");
                            myHaGUI.MyROIController.ROIList.Add(myROILine);
                            myHaGUI.MyROIController.paintData(myHaGUI.MyWindow);
                            break;
                        case "ROICircularArc":
                            ROICircularArc myROICircularArc = new ROICircularArc();
                            myROICircularArc.MIDC = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC");
                            myROICircularArc.MIDR = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR");
                            myROICircularArc.RADIUS = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "RADIUS");
                            myROICircularArc.StartR = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "StartR");
                            myROICircularArc.StartC = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "StartC");
                            myROICircularArc.SIZER = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "SIZER");
                            myROICircularArc.SIZEC = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "SIZEC");
                            myROICircularArc.StartPhi = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "StartPhi");
                            myROICircularArc.ExtentPhi = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "ExtentPhi");
                            myHaGUI.MyROIController.ROIList.Add(myROICircularArc);
                            myHaGUI.MyROIController.paintData(myHaGUI.MyWindow);
                            break;
                        case "ROIRectangle1":
                            ROIRectangle1 myROIRectangle1 = new ROIRectangle1();
                            myROIRectangle1.MIDC = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC");
                            myROIRectangle1.MIDR = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR");
                            myROIRectangle1.COL1 = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "COL1");
                            myROIRectangle1.ROW1 = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW1");
                            myROIRectangle1.COL2 = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "COL2");
                            myROIRectangle1.ROW2 = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "ROW2");
                            myHaGUI.MyROIController.ROIList.Add(myROIRectangle1);
                            myHaGUI.MyROIController.paintData(myHaGUI.MyWindow);
                            break;
                        case "ROIRectangle2":
                            ROIRectangle2 myROIRectangle2 = new ROIRectangle2();
                            myROIRectangle2.MIDC = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDC");
                            myROIRectangle2.MIDR = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "MIDR");
                            myROIRectangle2.LENGTH1 = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "LENGTH1");
                            myROIRectangle2.LENGTH2 = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "LENGTH2");
                            myROIRectangle2.Phi = myXMLfile.GetDoubleValue("ROIList", "ROI" + i.ToString(), "Coord", "Phi");
                            myHaGUI.MyROIController.ROIList.Add(myROIRectangle2);
                            myHaGUI.MyROIController.paintData(myHaGUI.MyWindow);
                            break;
                    }
                }
                if (t == "Too")
                {
                    string Type = myXMLfile.GetStringValue("ROIList", name, "type");
                    string XMLstr = "";
                    switch (Type)
                    {
                        case "找直线":
                            XMLstr = myXMLfile.GetStringValue("ROIList", name, "ParamJson");
                            VisionLine myLineTool = NewtonsoftJsonExtension.DeserializeObjectFromJson_NJ<VisionLine>(XMLstr);
                            myLineTool.SettingROI((ROI)myHaGUI.MyROIController.ROIList[int.Parse(myLineTool.IndexID)]);
                            myHaGUI.MyToolsController.AddVisionTool(myLineTool);
                            break;
                        case "Blob分析":
                            XMLstr = myXMLfile.GetStringValue("ROIList", name, "ParamJson");
                            VisionBlob myBlobTool = NewtonsoftJsonExtension.DeserializeObjectFromJson_NJ<VisionBlob>(XMLstr);
                            myBlobTool.SettingROI((ROI)myHaGUI.MyROIController.ROIList[int.Parse(myBlobTool.IndexID)]);
                            myHaGUI.MyToolsController.AddVisionTool(myBlobTool);
                            break;
                        case "Pad脏污分析":
                            XMLstr = myXMLfile.GetStringValue("ROIList", name, "ParamJson");
                            VisionPadPanic myPadTool = NewtonsoftJsonExtension.DeserializeObjectFromJson_NJ<VisionPadPanic>(XMLstr);
                            myPadTool.SettingROI((ROI)myHaGUI.MyROIController.ROIList[int.Parse(myPadTool.IndexID)]);
                            myHaGUI.MyToolsController.AddVisionTool(myPadTool);
                            break;
                        case "找圆工具":
                            XMLstr = myXMLfile.GetStringValue("ROIList", name, "ParamJson");
                            VisionCircle myCircleTool = NewtonsoftJsonExtension.DeserializeObjectFromJson_NJ<VisionCircle>(XMLstr);
                            myCircleTool.SettingROI((ROI)myHaGUI.MyROIController.ROIList[int.Parse(myCircleTool.IndexID)]);
                            myHaGUI.MyToolsController.AddVisionTool(myCircleTool);
                            break;
                        case "二维码":
                            XMLstr = myXMLfile.GetStringValue("ROIList", name, "ParamJson");
                            VisionBarCode myBarcodeTool = NewtonsoftJsonExtension.DeserializeObjectFromJson_NJ<VisionBarCode>(XMLstr);
                            myBarcodeTool.SettingROI((ROI)myHaGUI.MyROIController.ROIList[int.Parse(myBarcodeTool.IndexID)]);
                            myHaGUI.MyToolsController.AddVisionTool(myBarcodeTool);
                            break;
                        case "模板匹配":
                            XMLstr = myXMLfile.GetStringValue("ROIList", name, "ParamJson");
                            VisionTemplate myTemplateTool = NewtonsoftJsonExtension.DeserializeObjectFromJson_NJ<VisionTemplate>(XMLstr);
                            myTemplateTool.SettingROI((ROI)myHaGUI.MyROIController.ROIList[int.Parse(myTemplateTool.IndexID)]);
                            myHaGUI.MyToolsController.AddVisionTool(myTemplateTool);
                            break;
                        case "清晰度分析":
                            XMLstr = myXMLfile.GetStringValue("ROIList", name, "ParamJson");
                            VisionClarity myClarity = NewtonsoftJsonExtension.DeserializeObjectFromJson_NJ<VisionClarity>(XMLstr);
                            myClarity.SettingROI((ROI)myHaGUI.MyROIController.ROIList[int.Parse(myClarity.IndexID)]);
                            myHaGUI.MyToolsController.AddVisionTool(myClarity);
                            break;
                    }
                }
            }
            myHaGUI.MyHWndControl.repaint();
        }

        public void RunHalconFlow(HObject myImage, HaGUI myHaGUI)
        {
            if (myHaGUI.MyROIController.ROIList.Count <= 0) return;
            if (myHaGUI.MyToolsController.VisionToolList.Count <= 0) return;
            myHaGUI.MyHWndControl.clearHMessageList();
            for (int i = 0; i < myHaGUI.MyROIController.ROIList.Count; i++)
            {
                if (((IVisionTool)myHaGUI.MyToolsController.VisionToolList[i]).IndexID == i.ToString())
                {
                    ((IVisionTool)myHaGUI.MyToolsController.VisionToolList[i]).RunVision(myImage, myHaGUI);
                }
            }
            myHaGUI.MyHWndControl.repaint();
        }

        public void SaveHalconCameraConn(HaCamConnect Cam)
        {
            myCamXMLfile.ClearXML();
            myCamXMLfile.WriteValue("HaGUI", "CameraConnect", "CameraSn", Cam.BingdingSn);
        }

        public string LoadHalconCameraConn()
        {
            try
            {
                return myCamXMLfile.GetStringValue("HaGUI", "CameraConnect", "CameraSn");
            }
            catch
            {
                return null;
            }
        }

        public void SaveZYCameraConn(ParamsClass Cam)
        {
            myCamXMLfile.ClearXML();
            myCamXMLfile.WriteValue("ZY", "CameraConnect", "IPadr", Cam.CameraIP);

            myCamXMLfile.WriteValue("ZY", "CameraConnect", "bestAE", Cam.m_Param.bestAE);
            myCamXMLfile.WriteValue("ZY", "CameraConnect", "gain", Cam.m_Param.gain);
            myCamXMLfile.WriteValue("ZY", "CameraConnect", "rgain", Cam.m_Param.rgain);
            myCamXMLfile.WriteValue("ZY", "CameraConnect", "bgain", Cam.m_Param.bgain);
            myCamXMLfile.WriteValue("ZY", "CameraConnect", "saturation", Cam.m_Param.saturation);
            myCamXMLfile.WriteValue("ZY", "CameraConnect", "contrast", Cam.m_Param.contrast);
            myCamXMLfile.WriteValue("ZY", "CameraConnect", "sharp", Cam.m_Param.sharp);
            myCamXMLfile.WriteValue("ZY", "CameraConnect", "hdr", Cam.m_Param.hdr);

            myCamXMLfile.WriteValue("ZY", "CameraConnect", "AEstate", Cam.m_Param.AEstate);

        }

        public ParamsClass LoadZYCameraConn()
        {
            ParamsClass Cam = new ParamsClass();
            try
            {
                if (!myCamXMLfile.IsExisting()) return Cam;
                Cam.CameraIP = myCamXMLfile.GetStringValue("ZY", "CameraConnect", "IPadr");
                Cam.m_Param.bestAE = uint.Parse(myCamXMLfile.GetStringValue("ZY", "CameraConnect", "bestAE"));
                Cam.m_Param.gain = uint.Parse(myCamXMLfile.GetStringValue("ZY", "CameraConnect", "gain"));
                Cam.m_Param.rgain = uint.Parse(myCamXMLfile.GetStringValue("ZY", "CameraConnect", "rgain"));
                Cam.m_Param.bgain = uint.Parse(myCamXMLfile.GetStringValue("ZY", "CameraConnect", "bgain"));
                Cam.m_Param.saturation = uint.Parse(myCamXMLfile.GetStringValue("ZY", "CameraConnect", "saturation"));
                Cam.m_Param.contrast = uint.Parse(myCamXMLfile.GetStringValue("ZY", "CameraConnect", "contrast"));
                Cam.m_Param.sharp = uint.Parse(myCamXMLfile.GetStringValue("ZY", "CameraConnect", "sharp"));
                Cam.m_Param.hdr = uint.Parse(myCamXMLfile.GetStringValue("ZY", "CameraConnect", "hdr"));
                Cam.m_Param.AEstate = uint.Parse(myCamXMLfile.GetStringValue("ZY", "CameraConnect", "AEstate"));
                return Cam;
            }
            catch
            {
                return Cam;
            }
        }

        public static void SaveHalconDataSpecial<T>(T src, string savePath, out int flagResult)
        {

            flagResult = OKFlag;
            string[] typesOfData = new string[] { "HTuple", "HObject", "MatrixID", "ModelID", "HTuple[]" };
            string[] serializeMethod = new string[] { "SerializeTuple", "SerializeObject", "SerializeMatrix", "SerializeShapeModel" };

            // Get the File Name 
            string FilePath = "";
            string FileName = "";
            FilePath = savePath.Substring(0, savePath.LastIndexOf("\\") + 1);
            FileName = savePath.Substring(savePath.LastIndexOf("\\") + 1, (savePath.LastIndexOf(".")) - (savePath.LastIndexOf("\\") + 1));
            if ("" == FileName)
            {
                flagResult = NGFlag;
                return;
            }
            // Set the File to Save the Paras Info , main info is the type and order of the para
            string SavePathDescription = FilePath + FileName + "Describe.txt";

            // open the file , 参数文件 和  参数类型描述文件
            HTuple hv_FileHandle;   // 使用Halcon方式打开文件， 如果用  Stream streamPara = File.OpenWrite(savePath); 打开， 在需要使用Halcon 时没法用
            HOperatorSet.OpenFile(savePath, "output_binary", out hv_FileHandle);
            Stream streamParaDiscrip = File.OpenWrite(SavePathDescription);      // 存储的数据全是string类型， 用Stream方便

            try
            {
                /** use the reflection **/
                Type type = typeof(T);
                FieldInfo[] fInfo = type.GetFields();
                foreach (FieldInfo fCurrent in fInfo)
                {
                    //获取数据
                    Object obj = (fCurrent.GetValue(src));   // 数据值

                    // 
                    int indType = 0;
                    var vName = (fCurrent.FieldType).Name; // 变量类型名称
                    HTuple hv_strVName = new HTuple(vName);
                    if (hv_strVName.S == typesOfData[0])
                    {
                        indType = 0;
                        string varName = fCurrent.ToString();         // 变量完整名字，包括名称空间
                        if (varName.IndexOf(typesOfData[2]) >= 0)   // 包含specailNameMatrixID标志
                        {
                            indType = 2;
                        }
                        else if (varName.IndexOf(typesOfData[3]) >= 0)
                        {
                            indType = 3;
                        }
                    }
                    else if (hv_strVName.S == typesOfData[1])
                    {
                        indType = 1;
                    }
                    else if (hv_strVName.S == typesOfData[4])
                    {
                        indType = 4;
                    }

                    // 描述文件 ， Serialize the name of data type 
                    new HTuple(typesOfData[indType]).Serialize(streamParaDiscrip);  // save the para type
                    int NElement = 0;
                    if (4 == indType)  // 如果是数组长度则获取长度
                    {
                        MethodInfo methodGetLength = (fCurrent.FieldType).GetMethod("get_Length");
                        Object objTmp = methodGetLength.Invoke(obj, new Object[] { });
                        int.TryParse(objTmp.ToString(), out NElement);

                        new HTuple(NElement).Serialize(streamParaDiscrip);
                    }

                    // 序列化数据， Serialize para 

                    if (null != obj)
                    {
                        if (indType == 4)
                        {
                            for (int nTmp = 0; nTmp < NElement; nTmp++)
                            {
                                Object[] ParameterItemHandleTmp = new Object[1];
                                ParameterItemHandleTmp[0] = nTmp;
                                MethodInfo methodGetEle = (fCurrent.FieldType).GetMethod("Get");
                                Object objTmp = methodGetEle.Invoke(obj, ParameterItemHandleTmp); // 获取 单个元素

                                Object[] ParameterItemHandle = new Object[2];
                                Object[] ParameterWriteItem = new Object[2];
                                ParameterItemHandle[0] = objTmp;       // 输入matrixID 
                                                                       // ParameterTmp[1] = Parameter[0];  // 输出 ItemHandle， 该参数是 out 类型， 不能直接指定
                                ParameterWriteItem[0] = hv_FileHandle;           // 输入文件                   
                                Type tmpAllType = typeof(HOperatorSet);
                                MethodInfo methodSerialize = (tmpAllType).GetMethod(serializeMethod[0]);
                                MethodInfo methodFwriteSerializedItem = (tmpAllType).GetMethod("FwriteSerializedItem");
                                methodSerialize.Invoke(null, ParameterItemHandle); // 获取 ItemHandle
                                ParameterWriteItem[1] = ParameterItemHandle[1];            // 输入 ItemHandle
                                methodFwriteSerializedItem.Invoke(null, ParameterWriteItem); // 序列化 ItemHandle
                            }


                        }
                        else
                        {
                            Object[] ParameterItemHandle = new Object[2];
                            Object[] ParameterWriteItem = new Object[2];
                            ParameterItemHandle[0] = obj;       // 输入matrixID 
                                                                // ParameterTmp[1] = Parameter[0];  // 输出 ItemHandle， 该参数是 out 类型， 不能直接指定
                            ParameterWriteItem[0] = hv_FileHandle;           // 输入文件                   
                            Type tmpAllType = typeof(HOperatorSet);
                            MethodInfo methodSerialize = (tmpAllType).GetMethod(serializeMethod[indType]);
                            MethodInfo methodFwriteSerializedItem = (tmpAllType).GetMethod("FwriteSerializedItem");

                            methodSerialize.Invoke(null, ParameterItemHandle); // 获取 ItemHandle
                            ParameterWriteItem[1] = ParameterItemHandle[1];            // 输入 ItemHandle
                            methodFwriteSerializedItem.Invoke(null, ParameterWriteItem); // 序列化 ItemHandle
                        }


                    }
                    else
                    {
                        // close file
                        streamParaDiscrip.Close();

                        flagResult = NGFlag;
                        return;
                    }



                }

                // close file
                streamParaDiscrip.Close();
                HOperatorSet.CloseFile(hv_FileHandle);
                MessageBox.Show("保存成功!");
            }
            catch (HalconException ex)
            {
                flagResult = NGFlag;
            }



        }

        /// </summary>
        /// Load the HalconData which was saved by SaveHalconData  . !!!! Attention :input src must be class , struct will not work but do not know why
        /// <summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dst"></param>
        /// <param name="savePath"></param>
        /// <param name="flagResult"></param>
        public static void LoadHalconDataSpecial<T>(ref T dst, string savePath, out int flagResult)
        {

            flagResult = OKFlag;
            string[] typesOfData = new string[] { "HTuple", "HObject", "MatrixID", "ModelID", "HTuple[]" };
            string[] deserializeMethod = new string[] { "DeserializeTuple", "DeserializeObject", "DeserializeMatrix", "DeserializeShapeModel" };

            // Get the File Name 
            string FilePath = "";
            string FileName = "";
            FilePath = savePath.Substring(0, savePath.LastIndexOf("\\") + 1);
            FileName = savePath.Substring(savePath.LastIndexOf("\\") + 1, (savePath.LastIndexOf(".")) - (savePath.LastIndexOf("\\") + 1));
            if ("" == FileName)
            {
                flagResult = NGFlag;
                return;
            }
            // Set the File to Save the Paras Info , main info is the type and order of the para
            string SavePathDescription = FilePath + FileName + "Describe.txt";
            if (!(new FileInfo(SavePathDescription)).Exists)
            {
                flagResult = NGFlag;
                return;
            }

            // open the file , 参数文件 和  参数类型描述文件
            HTuple hv_FileHandle;   // 使用Halcon方式打开文件， 如果用  Stream streamPara = File.OpenWrite(savePath); 打开， 在需要使用Halcon 时没法用
            HOperatorSet.OpenFile(savePath, "input_binary", out hv_FileHandle);
            Stream streamParaDiscrip = File.OpenRead(SavePathDescription);


            /** use the reflection **/
            try
            {
                Type type = typeof(T);
                FieldInfo[] fInfo = type.GetFields();
                bool bBreak = false; // 表明描述文件 Discrip 已经到达末尾， 暂时用于 类中新增加变量的情况
                foreach (FieldInfo fCurrent in fInfo)
                {
                    // Serialize the name of data type 
                    var vName = (fCurrent.FieldType).Name;
                    HTuple hv_strVName = new HTuple(vName);

                    hv_strVName = HTuple.Deserialize(streamParaDiscrip);  // get the para type
                    if (streamParaDiscrip.Position == streamParaDiscrip.Length)
                    {
                        bBreak = true; // 表明描述文件 Discrip 已经到达末尾
                    }

                    for (int indType = 0; indType < typesOfData.Length; indType++)
                    {
                        if (hv_strVName.S == typesOfData[indType])
                        {
                            // 获取ITem
                            Type tmpAllType = typeof(HOperatorSet);
                            Object[] ParameterItemHandle = new Object[2];
                            ParameterItemHandle[0] = hv_FileHandle;                      // 输入FileHandle
                            MethodInfo methodFreadSerializedItem = (tmpAllType).GetMethod("FreadSerializedItem");
                            methodFreadSerializedItem.Invoke(null, ParameterItemHandle); // 获取 ItemHandle

                            // 反序列化
                            Object[] ParameterDesrialize = new Object[2];
                            MethodInfo methodDeSerialize = (tmpAllType).GetMethod(deserializeMethod[indType]);
                            if (1 == indType) // 注意， HObject 类型的 反序列化参数顺序 与 其他不一致， HObject 输出都是放在前面的
                            {

                                ParameterDesrialize[1] = ParameterItemHandle[1];
                                methodDeSerialize.Invoke(null, ParameterDesrialize); // 反序列化                               
                                fCurrent.SetValue(dst, ParameterDesrialize[0]);
                            }
                            else
                            {

                                ParameterDesrialize[0] = ParameterItemHandle[1];
                                methodDeSerialize.Invoke(null, ParameterDesrialize); // 反序列化                               
                                fCurrent.SetValue(dst, ParameterDesrialize[1]);

                            }

                            break;
                        }
                    }

                    if (bBreak)
                    {
                        break;
                    }
                }

                // close file
                streamParaDiscrip.Close();
                HOperatorSet.CloseFile(hv_FileHandle);
            }
            catch (HalconException ex)
            {
                flagResult = NGFlag;
            }



        }


        public static void SaveData(IVisionTool nIVTool, string path)
        {
            FileStream filesave = new FileStream(path, FileMode.Create);
            BinaryFormatter BF = new BinaryFormatter();
            BF.Serialize(filesave, nIVTool);
            filesave.Close();
        }

        public static void LoadData(ref IVisionTool nIVTool, string path)
        {
            FileStream filesave = new FileStream(path, FileMode.Open);
            BinaryFormatter BF = new BinaryFormatter();
            nIVTool = BF.Deserialize(filesave) as IVisionTool;
            filesave.Close();
        }
    }

}
