//StructsBody.cs�����еĶ��壬��ӳ��DataType.h�ж������Ӧ�Ľṹ���ص�������ö������
using System;
using System.Runtime.InteropServices;
using System.Text;

#region-����ͷ��ز����ṹ
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
[Serializable]
public struct Read_Param
{
    public uint exposure;//�ع�ʱ�䵥λus
    public uint gain;    //����
    public uint rgain;   //������
    public uint bgain;   //������
    public uint contrast;//�Աȶ�
    public uint hdr;     //������
    public uint sharp;   //������
    public uint saturation;//���Ͷ�
    public uint bestAE;  //�������
    public uint Hflip;   //ˮƽ��ת
    public uint Vflip;   //��ֱ��ת
    public uint BWmode;  //�ع�ģʽ
    public uint Temperature;//����
    public uint AEstate; //�Զ��ع�״̬
    public uint AWBstate;//�Զ���ƽ��״̬
    public uint resolution;//����
    public uint LanIP;//����
    public uint WirelanIP;//����
    public uint bWifi;//����

};
#endregion



#region//����
public enum SPWF_RESOLUTION_TYPE
{
    SPWF_CAMERA_SMALL = 0,
    SPWF_CAMERA_NORMAL = 1,
    SPWF_CAMERA_LAGER = 2,
    SPWF_CAMERA_4K = 3

};
#endregion





#region // ����ͼ���ʽ����
public enum SPWF_IMG_TYPE
{
    SPWF_IMG_BMP = 0,
    SPWF_IMG_JPG = 1,
    SPWF_IMG_TIF = 2,
    SPWF_IMG_PNG = 3
};
#endregion



#region	����ֵ����
public struct CapReturnValul
{
    public const uint ResSuccess = (uint)0x0000;	                // ���سɹ�
    public const uint ResNullHandleErr = (uint)0x0001;		        // ��Ч���
    public const uint ResNullPointerErr = (uint)0x0002;		        // ָ��Ϊ��
    public const uint ResFileOpenErr = (uint)0x0003;		        // �ļ�����/��ʧ��
    public const uint ResNoDeviceErr = (uint)0x0004;		        // û�п����豸
    public const uint ResInvalidParameterErr = (uint)0x0005;		// �ڴ���䲻��
    public const uint ResOutOfMemoryErr = (uint)0x0006;		        // û�п���Ԥ��
    public const uint ResNoPreviewRunningErr = (uint)0x0007;		// Ԥ��û�п���
    public const uint ResOSVersionErr = (uint)0x0008;
    public const uint ResUsbNotAvailableErr = (uint)0x0009;
    public const uint ResNotSupportedErr = (uint)0x000a;
    public const uint ResNoSerialString = (uint)0x000b;
    public const uint ResVerificationErr = (uint)0x000c;
    public const uint ResTimeoutErr = (uint)0x000d;
    public const uint ResScaleModeErr = (uint)0x000f;
    public const uint ResUnknownErr = (uint)0x00ff;

    public const uint ResDisplayWndExist = (uint)0x0011;		// Ӧ�ùر�Ԥ������
    public const uint ResAllocated = (uint)0x0012;		        // �ڴ��Ѿ�����
    public const uint ResAllocateFail = (uint)0x0013;		    // �ڴ����ʧ��
    public const uint ResReadError = (uint)0x0014;              // USB��ȡʧ��
    public const uint ResWriteError = (uint)0x0015;		        // USB�����ʧ��
    public const uint ResUsbOpen = (uint)0x0016;                // USB�˿��Ѿ���
    public const uint ResCreateStreamErr = (uint)0x0017;		// ����avi��ʧ��
    public const uint ResSetStreamFormatErr = (uint)0x0018;		// ����AVI����ʽʧ��

};
#endregion


