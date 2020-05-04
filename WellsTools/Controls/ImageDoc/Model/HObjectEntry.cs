using System;
using hvppleDotNet;
using System.Collections;

namespace Wells.Controls.ImageDoc
{

	/// <summary>
	/// This class is an auxiliary class, which is used to 
	/// link a graphical context to an HALCON object. The graphical 
	/// context is described by a hashtable, which contains a list of
	/// graphical modes (e.g GC_COLOR, GC_LINEWIDTH and GC_PAINT) 
	/// and their corresponding values (e.g "blue", "4", "3D-plot"). These
	/// graphical states are applied to the window before displaying the
	/// object.
	/// </summary>
	public class HObjectEntry
	{
		/// <summary>Hashlist defining the graphical context for HObj</summary>
		public Hashtable	gContext;

		/// <summary>HALCON object</summary>
		public HObject		HObj;

        public HWndMessage Message;

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

        public HObjectEntry(HWndMessage message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Clears the entries of the class members Hobj and gContext
        /// </summary>
        public void clear()
		{
            if (gContext != null)
                gContext.Clear();
            if (HObj != null)
                HObj.Dispose();
		}

	}//end of class
}//end of namespace
