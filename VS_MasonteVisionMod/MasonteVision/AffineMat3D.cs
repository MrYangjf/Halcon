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
using MasonteDataProcess.FileProcess;

namespace MasonteVision.HalconMat3D
{
    [Serializable]
    public class AffineMat3D
    {
        HTuple Px, Py, Pz, Qx, Qy, Qz, HomMate3DModel, Height;
        HTuple SerializedItemHandle, FileHandle;

        /// <summary>
        /// 输入坐标
        /// </summary>
        /// <param name="InPutx">输入X</param>
        /// <param name="InPuty">输入Y</param>
        /// <param name="InPutz">输入激光传感器高度Z</param>
        /// <param name="ConstHeight">输入设定的相机高度</param>
        public AffineMat3D(double[] InPutx, double[] InPuty, double[] InPutz, double ConstHeight)
        {
            Qx = InPutx;
            Qy = InPuty;
            Qz = InPutz;

            Height = ConstHeight;

            Px = Qx;
            Py = Qy;
            HOperatorSet.TupleGenConst(Qz.Length, Height, out Pz);
        }

        /// <summary>
        /// 求高度
        /// </summary>
        /// <param name="X">输入X</param>
        /// <param name="Y">输入Y</param>
        /// <param name="Z">得到激光传感器需要到达的高度</param>
        public void RunVision(double X, double Y, out double Z)
        {
            HTuple NeedX, NeedY, NeedHeight;
            HOperatorSet.AffineTransPoint3d(HomMate3DModel, X, Y, Height, out NeedX, out NeedY, out NeedHeight);
            Z = NeedHeight;
        }

        /// <summary>
        /// 建立3D矩阵
        /// </summary>
        public void Create3dModel()
        {
            HOperatorSet.HomMat3dIdentity(out HomMate3DModel);
            HOperatorSet.VectorToHomMat3d("similarity", Px, Py, Pz, Qx, Qy, Qz, out HomMate3DModel);

        }

        /// <summary>
        /// 保存矩阵
        /// </summary>
        /// <param name="SavePath"></param>
        public void SaveModel(string SavePath)
        {
            HOperatorSet.SerializeHomMat3d(HomMate3DModel, out SerializedItemHandle);
            HOperatorSet.OpenFile(SavePath + "HomMat3D.mat", "output_binary", out FileHandle);
            HOperatorSet.FwriteSerializedItem(FileHandle, SerializedItemHandle);
            HOperatorSet.CloseFile(FileHandle);
        }

        /// <summary>
        /// 读取矩阵
        /// </summary>
        /// <param name="LoadPath"></param>
        public void ReadModel(string LoadPath)
        {
            HOperatorSet.OpenFile(LoadPath + "HomMat3D.mat", "output_binary", out FileHandle);
            HOperatorSet.FreadSerializedItem(FileHandle, out SerializedItemHandle);
            HOperatorSet.DeserializeHomMat3d(SerializedItemHandle, out HomMate3DModel);
            HOperatorSet.CloseFile(FileHandle);
        }

    }


}