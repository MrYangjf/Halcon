//DllFunction.cs该文件中方法映射RZAPI.h中方法
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MasonteVision.PCB_AOI_ZY_CAM
{
    #region----------------声明两个委托,相当于VC里面的回调函数------------------
    public delegate void DL_AUTOCALLBACK(uint dw1, uint dw2, IntPtr lpContext);

    /// <summary>
    /// 帧回调函数	lpParam指向帧数据的指针, 
    /// 			lpPoint 保留, 
    /// 			lpContext在设置帧回调函数时传递的上下文
    /// </summary>
    public delegate void DL_FRAMECALLBACK(IntPtr lpParam1, IntPtr lpPoint, IntPtr lpContext);

    #endregion

    #region 对应dll中的函数原型转换
    /// <summary>
    /// 函数原型
    /// </summary>
    public class DllFunction
    {
        /// <summary>
        ///函数:	WF_InitRes
        ///功能:设置初始化资源函数
        ///参数:	WF_RESOLUTION_TYPE 分为小，正常，大，4K四种模式
        ///nCount 表示当前模式开放几块资源，这里对应的是同时使用的相机个数
        ///说明:   目前SP-4KCH-III推荐使用WF_CAMERA_4K类型进行设置
        ///       
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_InitRes(int nCount, SPWF_RESOLUTION_TYPE nType);

        /// <summary>
        ///函数:	WF_InitCamera
        ///功能:	设置初始化资源函数
        ///参数:   ulIPAddr 表示当前初始化相机的IP地址
        ///usPort 表示当前初始化相机的端口号 这里默认为8554
        ///nIndex 指定相机申请资源使用的序号，默认从0开始，这里与WF_InitRes中的nCount相关。
        ///说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_InitCamera(int nIndex, uint ulIPAddr, ushort usPort);

        

        /// <summary>
        /// 函数:	WF_SetNewHwnd
        /// 功能:	设置窗口显示
        /// 参数:   nIndex:         相机索引号，用来区分不同的相机
        ///         hwnd：窗口句柄
        ///  说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetNewHwnd(int nIndex, IntPtr hwnd);


        /// <summary>
        /// 函数:	WF_SetVideoWindowPos
        /// 功能:	设置窗口显示的大小以及位置
        /// 参数:   nIndex:         相机索引号，用来区分不同的相机
        /// StartX：				水平偏移位置
		/// 	startY：				垂直偏移位置
		///	nWidth：				宽
        ///	nHeight：				高         
        /// 说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetVideoWindowPos(int nIndex, int StartX, int startY, int nWidth, int nHeight);


        /// <summary>
        /// 函数:	WF_LiveStart
        /// 功能:	开启视频
        /// 参数:   nIndex:         相机索引号，用来区分不同的相机
        /// 说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_LiveStart(int nIndex);


        /// <summary>
        /// 函数:	WF_LiveStop
        /// 功能:	关闭视频
        /// 参数:    nIndex:         相机索引号，用来区分不同的相机
        /// 说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_LiveStop(int nIndex);

        /// <summary>
        ///  函数:	WF_ChangeIP
        ///  功能:	修改相机IP，调用前，建议先停止预览，在调用，然后再开启预览
        /// 参数:   nIndex:         相机索引号，用来区分不同的相机
        ///    ulIPAddr：      相机IP地址，例如192.168.1.10，则为0xA01A8C0
        /// 说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_ChangeIP(int nIndex, uint ulIPAddr);

        /// <summary>
        /// 函数:	WF_UnInitRes
        /// 功能:	反初始化全部资源
        /// 参数:   
        /// 说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_UnInitRes();

        /// <summary>
        /// 函数:	WF_SetExposure
        /// 功能:	设置曝光
        /// 参数:   nIndex:         相机索引号，用来区分不同的相机
        ///         dwEx：相机曝光时间单位为us
        ///  说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        /*unsafe*/
        public static extern int WF_SetExposure(int nIndex, uint dwEx);

        /// <summary>
        /// 函数:	WF_SetBestAe
        ///  功能:	设置最佳亮度
        /// 参数:    nIndex:         相机索引号，用来区分不同的相机
        ///         dwBae：相机最佳亮度
        /// 说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetBestAe(int nIndex, uint dwBae);


        /// <summary>
        /// 函数:	WF_SetGain_A
        /// 功能:	设置增益
        /// 参数:   nIndex:         相机索引号，用来区分不同的相机
        ///         dwGain：相机增益
        /// 说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetGain_A(int nIndex, uint dwGain);

        /// <summary>
        /// 函数:	WF_SetGainR
        /// 功能:	设置红增益
        /// 参数:   nIndex:         相机索引号，用来区分不同的相机
        ///         dwR：相机红增益
        /// 说明:
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetGainR(int nIndex, uint dwR);

        /// <summary>
        ///函数:	WF_SetGainB
        ///功能:	nIndex:         相机索引号，用来区分不同的相机
        ///参数:   FrameCB 帧回调函数
        ///		dwB：相机蓝增益
        ///说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetGainB(int nIndex, uint dwB);

        /// <summary>
        ///函数:	WF_SetSaturation
        ///功能:	设置饱和度
        ///参数:  nIndex:         相机索引号，用来区分不同的相机
        /// nSaturation：相机饱和度
        ///说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetSaturation(int nIndex, int nSaturation);

        /// <summary>
        /// 函数:	WF_SetContrast
        /// 功能:	设置视频窗口滚动偏移
        /// 参数:   nIndex:         相机索引号，用来区分不同的相机
        /// 	dwContrast：相机对比度
        /// 说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetContrast(int nIndex, uint dwContrast);

        /// <summary>
        ///函数:	WF_SetSharpness
        ///功能:	设置清晰度
        ///参数:  nIndex:         相机索引号，用来区分不同的相机
        ///         nSharp：相机清晰度
        ///说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetSharpness(int nIndex, int nSharp);

        /// <summary>
        ///函数:	WF_SetHDR
        ///功能:	设置消反光
        ///参数:   nIndex:         相机索引号，用来区分不同的相机
        ///         nHDR：相机消反光
        ///说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetHDR(int nIndex, int nHDR);

        /// <summary>
        ///函数:	WF_SetFrameCallBack
        ///功能:	设置帧回调函数
        ///参数:   nIndex:         相机索引号，用来区分不同的相机
        ///FrameCB：       回调函数指针
        ///lpContext：     设备上下文指针
        ///说明:   通过该函数设置对于每帧数据的处理的回调函数。
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetFrameCallBack(int nIndex, DL_FRAMECALLBACK FrameCB, IntPtr lpContext);

        /// <summary>
        ///函数:	WF_GetFrame
        ///功能:	获取一帧数据到指定的内存中
        ///参数:   nIndex:         相机索引号，用来区分不同的相机
        ///    pData：         内存地址首指针，数据格式为BGR排列。  
        ///说明:     用户在此处必须分配足够大的内存，否则很有可能内存溢出
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_GetFrame(int nIndex, IntPtr pData);

        /// <summary>
        ///函数:	WF_GetFrameToImage
        ///功能:	将一帧图像数据保存为指定格式的图片
        ///参数:   nIndex:         相机索引号，用来区分不同的相机
        /// lpszFileName：  设置保存文件的路径及文件名
        /// ImgType：       保存的图片类型
        /// pData：         保留参数，设为NULL
        /// dwParam：       保留参数，设为0
        /// 
        ///说明:   支持的图片类型参见WF_IMG_TYPE定义
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_GetFrameToImage(int nIndex, string lpszFileName, SPWF_IMG_TYPE ImgType, IntPtr pData, uint dwParam);

      
        /// <summary>
        ///函数:	WF_SetDoAWB
        ///功能:	开启白平衡，单次白平衡，默认4s后关闭
        ///参数:   nIndex:         相机索引号，用来区分不同的相机
        ///				btTarget：     保留传入0即可
        ///				bAWB：         需要设置为true
        ///				pfAWBCallback：保留传入NULL
        ///				lpContext：			保留传入NULL
        ///说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetDoAWB(int nIndex, char btTarget, bool bAWB, IntPtr pfAWBCallback, IntPtr lpContext);

        /// <summary>
        ///函数:	WF_SetDoAE
        ///功能:	开启自动曝光
        ///参数:    nIndex:         相机索引号，用来区分不同的相机
        /// 		btTarget：     保留传入0即可
        /// 		bAE：         true为开启，false为关闭
        /// 		pfAECallback：保留传入NULL
        /// 	lpContext：			保留传入NULL
        ///说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetDoAE(int nIndex, char btTarget, bool bAE, IntPtr pfAECallback, IntPtr lpContext);

        /// <summary>
        ///函数:	WF_GetCameraFrameRate
        ///功能:	获取相机当前帧率
        ///参数:   nIndex:         相机索引号，用来区分不同的相机
        ///说明:   fr：						返回相机帧率
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_GetCameraFrameRate(int nIndex, ref float fr);

        /// <summary>
        /// 函数:	WF_SetHFlip
        /// 功能:	设置水平翻转模式
        /// 参数:   nIndex:         相机索引号，用来区分不同的相机
        ///         dwHFlip：						0为关闭，1为开启
        /// 说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetHFlip(int nIndex, uint dwHFlip);

        /// <summary>
        /// 函数:	WF_SetVFlip
        /// 功能:	设置垂直翻转模式
        /// 参数:  nIndex:         相机索引号，用来区分不同的相机
        /// dwVFlip：						0为关闭，1为开启
        /// 说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetVFlip(int nIndex, uint dwVFlip);

        /// <summary>
        /// 函数:	WF_SetTemperature
        /// 功能:	黑白模式设置
        /// 参数:  nIndex:         相机索引号，用来区分不同的相机
        /// dwVFlip：						0为关闭，1为开启
        /// 说明:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetTemperature(int nIndex, uint dwBWMode);


        /// <summary>
        ///函数:	WF_SetBWMode
        ///功能:	设置长短曝光模式
        ///参数:   nIndex:         相机索引号，用来区分不同的相机
        ///         dwBWMode：						0为短曝光模式，1为长曝光模式
        ///说明:   
        /// <summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetBWMode(int nIndex, uint dwBWMode);

        /// <summary>
        /// 函数:	WF_GetAllParam
        /// 功能:	获取相机内所有参数
        /// 参数:   nIndex:         相机索引号，用来区分不同的相机
        /// 说明:   pReadParam：		相机内参数，曝光，增益等，具体详见Read_Param结构体
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_GetAllParam(int nIndex, ref Read_Param pReadParam);
       

        /// <summary>
        ///        /*==============================================================
        ///	函数:	WF_SetResolution
        ///	功能:	设置相机分辨率
        ///	参数:   nIndex:         相机索引号，用来区分不同的相机
        /// nType：		0为1080P10帧，1为1080P20帧
        ///--------------------------------------------------------------*/
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetResolution(int nIndex, uint nType);
        /// <summary>
        ///        /*==============================================================
        ///	函数:	WF_ReceiveFile
        ///	功能:	接收相机端图片
        ///	参数:   nIndex:         相机索引号，用来区分不同的相机
        /// bStart：		开启后，库中会开启一个线程等待接收相机端拍摄的图片。
        /// strFileName：设置图片存储路径。
        /// 
        ///--------------------------------------------------------------*/
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_ReceiveFile(int nIndex, bool bStart, string strFileName);
        /// <summary>
        ///        /*==============================================================
        ///	函数:	WF_GetFrameFromCamera
        ///	功能:	请求一张相机端的图片，图片大小与相机端设置保持一致
        ///	参数:   nIndex:         相机索引号，用来区分不同的相机
        ///         lpszFileName：	保存路径及名称。
        ///         目前仅支持jpg格式
        ///--------------------------------------------------------------*/
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_GetFrameFromCamera(int nIndex, string lpszFileName);

        /// <summary>
        ///        /*==============================================================
        ///	函数:	WF_GetFrameFromCameraBmp(目前只针对2K_V相机做后缀名的修改用)
        ///	功能:	请求一张相机端的图片，图片大小与相机端设置保持一致
        ///	参数:   nIndex:         相机索引号，用来区分不同的相机
        ///         lpszFileName：	保存路径及名称。
        ///         目前仅支持jpg格式
        ///--------------------------------------------------------------*/
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_GetFrameFromCameraBmp(int nIndex, string lpszFileName);

        /// <summary>
        ///        /*==============================================================
        ///	函数:	WF_BroadcastCamera
        ///	功能:	发起广播寻找相机，这里仅支持同一网段内查找
        ///	参数:   
        ///--------------------------------------------------------------*/
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_BroadcastCamera();

        /// <summary>
        ///        /*==============================================================
        ///	函数:	WF_BroadcastResult
        ///	功能:	查询广播后结果
        ///	参数:   nCount： 查询到的相机个数
        ///         ulIPAddrarm：  查询到的相机IP地址  
        ///         说明: 这里默认最大为10个相机。
        ///--------------------------------------------------------------*/
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_BroadcastResult( ref uint nCount, ref uint ulIPAddrarm);
        
       
    }
    #endregion
}