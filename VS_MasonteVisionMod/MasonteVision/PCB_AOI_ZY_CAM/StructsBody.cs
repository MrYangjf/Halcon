//StructsBody.cs该类中的定义，是映射DataType.h中定义的相应的结构，回调函数及枚举类型
using System;
using System.Runtime.InteropServices;
using System.Text;

#region-摄像头相关参数结构
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
[Serializable]
public struct Read_Param
{
    public uint exposure;//曝光时间单位us
    public uint gain;    //增益
    public uint rgain;   //红增益
    public uint bgain;   //绿增益
    public uint contrast;//对比度
    public uint hdr;     //消反光
    public uint sharp;   //清晰度
    public uint saturation;//饱和度
    public uint bestAE;  //最佳亮度
    public uint Hflip;   //水平翻转
    public uint Vflip;   //垂直翻转
    public uint BWmode;  //曝光模式
    public uint Temperature;//保留
    public uint AEstate; //自动曝光状态
    public uint AWBstate;//自动白平衡状态
    public uint resolution;//保留
    public uint LanIP;//保留
    public uint WirelanIP;//保留
    public uint bWifi;//保留

};
#endregion



#region//类型
public enum SPWF_RESOLUTION_TYPE
{
    SPWF_CAMERA_SMALL = 0,
    SPWF_CAMERA_NORMAL = 1,
    SPWF_CAMERA_LAGER = 2,
    SPWF_CAMERA_4K = 3

};
#endregion





#region // 保存图像格式设置
public enum SPWF_IMG_TYPE
{
    SPWF_IMG_BMP = 0,
    SPWF_IMG_JPG = 1,
    SPWF_IMG_TIF = 2,
    SPWF_IMG_PNG = 3
};
#endregion



#region	返回值定义
public struct CapReturnValul
{
    public const uint ResSuccess = (uint)0x0000;	                // 返回成功
    public const uint ResNullHandleErr = (uint)0x0001;		        // 无效句柄
    public const uint ResNullPointerErr = (uint)0x0002;		        // 指针为空
    public const uint ResFileOpenErr = (uint)0x0003;		        // 文件创建/打开失败
    public const uint ResNoDeviceErr = (uint)0x0004;		        // 没有可用设备
    public const uint ResInvalidParameterErr = (uint)0x0005;		// 内存分配不足
    public const uint ResOutOfMemoryErr = (uint)0x0006;		        // 没有开启预览
    public const uint ResNoPreviewRunningErr = (uint)0x0007;		// 预览没有开启
    public const uint ResOSVersionErr = (uint)0x0008;
    public const uint ResUsbNotAvailableErr = (uint)0x0009;
    public const uint ResNotSupportedErr = (uint)0x000a;
    public const uint ResNoSerialString = (uint)0x000b;
    public const uint ResVerificationErr = (uint)0x000c;
    public const uint ResTimeoutErr = (uint)0x000d;
    public const uint ResScaleModeErr = (uint)0x000f;
    public const uint ResUnknownErr = (uint)0x00ff;

    public const uint ResDisplayWndExist = (uint)0x0011;		// 应该关闭预览窗口
    public const uint ResAllocated = (uint)0x0012;		        // 内存已经分配
    public const uint ResAllocateFail = (uint)0x0013;		    // 内存分配失败
    public const uint ResReadError = (uint)0x0014;              // USB读取失败
    public const uint ResWriteError = (uint)0x0015;		        // USB命令发出失败
    public const uint ResUsbOpen = (uint)0x0016;                // USB端口已经打开
    public const uint ResCreateStreamErr = (uint)0x0017;		// 创建avi流失败
    public const uint ResSetStreamFormatErr = (uint)0x0018;		// 设置AVI流格式失败

};
#endregion


