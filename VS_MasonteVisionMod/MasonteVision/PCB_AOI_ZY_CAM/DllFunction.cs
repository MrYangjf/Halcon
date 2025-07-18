//DllFunction.cs���ļ��з���ӳ��RZAPI.h�з���
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MasonteVision.PCB_AOI_ZY_CAM
{
    #region----------------��������ί��,�൱��VC����Ļص�����------------------
    public delegate void DL_AUTOCALLBACK(uint dw1, uint dw2, IntPtr lpContext);

    /// <summary>
    /// ֡�ص�����	lpParamָ��֡���ݵ�ָ��, 
    /// 			lpPoint ����, 
    /// 			lpContext������֡�ص�����ʱ���ݵ�������
    /// </summary>
    public delegate void DL_FRAMECALLBACK(IntPtr lpParam1, IntPtr lpPoint, IntPtr lpContext);

    #endregion

    #region ��Ӧdll�еĺ���ԭ��ת��
    /// <summary>
    /// ����ԭ��
    /// </summary>
    public class DllFunction
    {
        /// <summary>
        ///����:	WF_InitRes
        ///����:���ó�ʼ����Դ����
        ///����:	WF_RESOLUTION_TYPE ��ΪС����������4K����ģʽ
        ///nCount ��ʾ��ǰģʽ���ż�����Դ�������Ӧ����ͬʱʹ�õ��������
        ///˵��:   ĿǰSP-4KCH-III�Ƽ�ʹ��WF_CAMERA_4K���ͽ�������
        ///       
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_InitRes(int nCount, SPWF_RESOLUTION_TYPE nType);

        /// <summary>
        ///����:	WF_InitCamera
        ///����:	���ó�ʼ����Դ����
        ///����:   ulIPAddr ��ʾ��ǰ��ʼ�������IP��ַ
        ///usPort ��ʾ��ǰ��ʼ������Ķ˿ں� ����Ĭ��Ϊ8554
        ///nIndex ָ�����������Դʹ�õ���ţ�Ĭ�ϴ�0��ʼ��������WF_InitRes�е�nCount��ء�
        ///˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_InitCamera(int nIndex, uint ulIPAddr, ushort usPort);

        

        /// <summary>
        /// ����:	WF_SetNewHwnd
        /// ����:	���ô�����ʾ
        /// ����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        ///         hwnd�����ھ��
        ///  ˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetNewHwnd(int nIndex, IntPtr hwnd);


        /// <summary>
        /// ����:	WF_SetVideoWindowPos
        /// ����:	���ô�����ʾ�Ĵ�С�Լ�λ��
        /// ����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        /// StartX��				ˮƽƫ��λ��
		/// 	startY��				��ֱƫ��λ��
		///	nWidth��				��
        ///	nHeight��				��         
        /// ˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetVideoWindowPos(int nIndex, int StartX, int startY, int nWidth, int nHeight);


        /// <summary>
        /// ����:	WF_LiveStart
        /// ����:	������Ƶ
        /// ����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        /// ˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_LiveStart(int nIndex);


        /// <summary>
        /// ����:	WF_LiveStop
        /// ����:	�ر���Ƶ
        /// ����:    nIndex:         ��������ţ��������ֲ�ͬ�����
        /// ˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_LiveStop(int nIndex);

        /// <summary>
        ///  ����:	WF_ChangeIP
        ///  ����:	�޸����IP������ǰ��������ֹͣԤ�����ڵ��ã�Ȼ���ٿ���Ԥ��
        /// ����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        ///    ulIPAddr��      ���IP��ַ������192.168.1.10����Ϊ0xA01A8C0
        /// ˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_ChangeIP(int nIndex, uint ulIPAddr);

        /// <summary>
        /// ����:	WF_UnInitRes
        /// ����:	����ʼ��ȫ����Դ
        /// ����:   
        /// ˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_UnInitRes();

        /// <summary>
        /// ����:	WF_SetExposure
        /// ����:	�����ع�
        /// ����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        ///         dwEx������ع�ʱ�䵥λΪus
        ///  ˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        /*unsafe*/
        public static extern int WF_SetExposure(int nIndex, uint dwEx);

        /// <summary>
        /// ����:	WF_SetBestAe
        ///  ����:	�����������
        /// ����:    nIndex:         ��������ţ��������ֲ�ͬ�����
        ///         dwBae������������
        /// ˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetBestAe(int nIndex, uint dwBae);


        /// <summary>
        /// ����:	WF_SetGain_A
        /// ����:	��������
        /// ����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        ///         dwGain���������
        /// ˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetGain_A(int nIndex, uint dwGain);

        /// <summary>
        /// ����:	WF_SetGainR
        /// ����:	���ú�����
        /// ����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        ///         dwR�����������
        /// ˵��:
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetGainR(int nIndex, uint dwR);

        /// <summary>
        ///����:	WF_SetGainB
        ///����:	nIndex:         ��������ţ��������ֲ�ͬ�����
        ///����:   FrameCB ֡�ص�����
        ///		dwB�����������
        ///˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetGainB(int nIndex, uint dwB);

        /// <summary>
        ///����:	WF_SetSaturation
        ///����:	���ñ��Ͷ�
        ///����:  nIndex:         ��������ţ��������ֲ�ͬ�����
        /// nSaturation��������Ͷ�
        ///˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetSaturation(int nIndex, int nSaturation);

        /// <summary>
        /// ����:	WF_SetContrast
        /// ����:	������Ƶ���ڹ���ƫ��
        /// ����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        /// 	dwContrast������Աȶ�
        /// ˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetContrast(int nIndex, uint dwContrast);

        /// <summary>
        ///����:	WF_SetSharpness
        ///����:	����������
        ///����:  nIndex:         ��������ţ��������ֲ�ͬ�����
        ///         nSharp�����������
        ///˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetSharpness(int nIndex, int nSharp);

        /// <summary>
        ///����:	WF_SetHDR
        ///����:	����������
        ///����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        ///         nHDR�����������
        ///˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetHDR(int nIndex, int nHDR);

        /// <summary>
        ///����:	WF_SetFrameCallBack
        ///����:	����֡�ص�����
        ///����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        ///FrameCB��       �ص�����ָ��
        ///lpContext��     �豸������ָ��
        ///˵��:   ͨ���ú������ö���ÿ֡���ݵĴ���Ļص�������
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetFrameCallBack(int nIndex, DL_FRAMECALLBACK FrameCB, IntPtr lpContext);

        /// <summary>
        ///����:	WF_GetFrame
        ///����:	��ȡһ֡���ݵ�ָ�����ڴ���
        ///����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        ///    pData��         �ڴ��ַ��ָ�룬���ݸ�ʽΪBGR���С�  
        ///˵��:     �û��ڴ˴���������㹻����ڴ棬������п����ڴ����
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_GetFrame(int nIndex, IntPtr pData);

        /// <summary>
        ///����:	WF_GetFrameToImage
        ///����:	��һ֡ͼ�����ݱ���Ϊָ����ʽ��ͼƬ
        ///����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        /// lpszFileName��  ���ñ����ļ���·�����ļ���
        /// ImgType��       �����ͼƬ����
        /// pData��         ������������ΪNULL
        /// dwParam��       ������������Ϊ0
        /// 
        ///˵��:   ֧�ֵ�ͼƬ���Ͳμ�WF_IMG_TYPE����
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_GetFrameToImage(int nIndex, string lpszFileName, SPWF_IMG_TYPE ImgType, IntPtr pData, uint dwParam);

      
        /// <summary>
        ///����:	WF_SetDoAWB
        ///����:	������ƽ�⣬���ΰ�ƽ�⣬Ĭ��4s��ر�
        ///����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        ///				btTarget��     ��������0����
        ///				bAWB��         ��Ҫ����Ϊtrue
        ///				pfAWBCallback����������NULL
        ///				lpContext��			��������NULL
        ///˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetDoAWB(int nIndex, char btTarget, bool bAWB, IntPtr pfAWBCallback, IntPtr lpContext);

        /// <summary>
        ///����:	WF_SetDoAE
        ///����:	�����Զ��ع�
        ///����:    nIndex:         ��������ţ��������ֲ�ͬ�����
        /// 		btTarget��     ��������0����
        /// 		bAE��         trueΪ������falseΪ�ر�
        /// 		pfAECallback����������NULL
        /// 	lpContext��			��������NULL
        ///˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetDoAE(int nIndex, char btTarget, bool bAE, IntPtr pfAECallback, IntPtr lpContext);

        /// <summary>
        ///����:	WF_GetCameraFrameRate
        ///����:	��ȡ�����ǰ֡��
        ///����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        ///˵��:   fr��						�������֡��
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_GetCameraFrameRate(int nIndex, ref float fr);

        /// <summary>
        /// ����:	WF_SetHFlip
        /// ����:	����ˮƽ��תģʽ
        /// ����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        ///         dwHFlip��						0Ϊ�رգ�1Ϊ����
        /// ˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetHFlip(int nIndex, uint dwHFlip);

        /// <summary>
        /// ����:	WF_SetVFlip
        /// ����:	���ô�ֱ��תģʽ
        /// ����:  nIndex:         ��������ţ��������ֲ�ͬ�����
        /// dwVFlip��						0Ϊ�رգ�1Ϊ����
        /// ˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetVFlip(int nIndex, uint dwVFlip);

        /// <summary>
        /// ����:	WF_SetTemperature
        /// ����:	�ڰ�ģʽ����
        /// ����:  nIndex:         ��������ţ��������ֲ�ͬ�����
        /// dwVFlip��						0Ϊ�رգ�1Ϊ����
        /// ˵��:   
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetTemperature(int nIndex, uint dwBWMode);


        /// <summary>
        ///����:	WF_SetBWMode
        ///����:	���ó����ع�ģʽ
        ///����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        ///         dwBWMode��						0Ϊ���ع�ģʽ��1Ϊ���ع�ģʽ
        ///˵��:   
        /// <summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetBWMode(int nIndex, uint dwBWMode);

        /// <summary>
        /// ����:	WF_GetAllParam
        /// ����:	��ȡ��������в���
        /// ����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        /// ˵��:   pReadParam��		����ڲ������ع⣬����ȣ��������Read_Param�ṹ��
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_GetAllParam(int nIndex, ref Read_Param pReadParam);
       

        /// <summary>
        ///        /*==============================================================
        ///	����:	WF_SetResolution
        ///	����:	��������ֱ���
        ///	����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        /// nType��		0Ϊ1080P10֡��1Ϊ1080P20֡
        ///--------------------------------------------------------------*/
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_SetResolution(int nIndex, uint nType);
        /// <summary>
        ///        /*==============================================================
        ///	����:	WF_ReceiveFile
        ///	����:	���������ͼƬ
        ///	����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        /// bStart��		�����󣬿��лῪ��һ���̵߳ȴ���������������ͼƬ��
        /// strFileName������ͼƬ�洢·����
        /// 
        ///--------------------------------------------------------------*/
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_ReceiveFile(int nIndex, bool bStart, string strFileName);
        /// <summary>
        ///        /*==============================================================
        ///	����:	WF_GetFrameFromCamera
        ///	����:	����һ������˵�ͼƬ��ͼƬ��С����������ñ���һ��
        ///	����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        ///         lpszFileName��	����·�������ơ�
        ///         Ŀǰ��֧��jpg��ʽ
        ///--------------------------------------------------------------*/
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_GetFrameFromCamera(int nIndex, string lpszFileName);

        /// <summary>
        ///        /*==============================================================
        ///	����:	WF_GetFrameFromCameraBmp(Ŀǰֻ���2K_V�������׺�����޸���)
        ///	����:	����һ������˵�ͼƬ��ͼƬ��С����������ñ���һ��
        ///	����:   nIndex:         ��������ţ��������ֲ�ͬ�����
        ///         lpszFileName��	����·�������ơ�
        ///         Ŀǰ��֧��jpg��ʽ
        ///--------------------------------------------------------------*/
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_GetFrameFromCameraBmp(int nIndex, string lpszFileName);

        /// <summary>
        ///        /*==============================================================
        ///	����:	WF_BroadcastCamera
        ///	����:	����㲥Ѱ������������֧��ͬһ�����ڲ���
        ///	����:   
        ///--------------------------------------------------------------*/
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_BroadcastCamera();

        /// <summary>
        ///        /*==============================================================
        ///	����:	WF_BroadcastResult
        ///	����:	��ѯ�㲥����
        ///	����:   nCount�� ��ѯ�����������
        ///         ulIPAddrarm��  ��ѯ�������IP��ַ  
        ///         ˵��: ����Ĭ�����Ϊ10�������
        ///--------------------------------------------------------------*/
        /// </summary>
        [DllImport("SPWFu.dll", CharSet = CharSet.Auto)]
        public static extern int WF_BroadcastResult( ref uint nCount, ref uint ulIPAddrarm);
        
       
    }
    #endregion
}