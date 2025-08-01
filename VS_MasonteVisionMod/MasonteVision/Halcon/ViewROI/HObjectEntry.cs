using System;
using HalconDotNet;
using System.Collections;



namespace MasonteVision.Halcon
{
	[Serializable]
	/// <summary>
	/// This class is an auxiliary class, which is used to 
	/// link a graphical context to an HALCON object. The graphical 
	/// context is described by a hashtable, which contains a list of
	/// graphical modes (e.g GC_COLOR, GC_LINEWIDTH and GC_PAINT) 
	/// and their corresponding values (e.g "blue", "4", "3D-plot"). These
	/// graphical states are applied to the window before displaying the
	/// object.
	/// （该类是一个辅助类，用于将图形context链接到HALCON object。
	/// 图形context由哈希表描述，哈希表包含图形模式列表（例如GC_COLOR、GC_LINEWIDTH和GC_PAINT）及其相应值（例如“blue”、“4”、“3D plot”）
	/// 这些图形状态在显示对象之前应用于窗口。）
	/// </summary>
	public class HObjectEntry
	{
		/// <summary>Hashlist defining the graphical context for HObj</summary>
		public Hashtable	gContext;

		/// <summary>HALCON object</summary>
		public HObject		HObj;



		/// <summary>Constructor</summary>
		/// <param name="obj">
		/// HALCON object that is linked to the graphical context gc. 
		/// </param>
		/// <param name="gc"> 
		/// Hashlist of graphical states that are applied before the object
		/// is displayed. 
		/// </param>
		public HObjectEntry(HObject obj, Hashtable gc)
		{
			gContext = gc;
			HObj = obj;
		}

		/// <summary>
		/// Clears the entries of the class members Hobj and gContext
		/// </summary>
		public void clear()
		{
			gContext.Clear();
			HObj.Dispose();
		}

	}//end of class
}//end of namespace
