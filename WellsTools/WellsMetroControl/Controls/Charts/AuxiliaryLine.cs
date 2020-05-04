using System;
using System.Drawing;

namespace Wells.WellsMetroControl.Controls
{
    /// <summary>
    /// Class AuxiliaryLine.
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
	internal class AuxiliaryLine : IDisposable
	{
		private bool disposedValue = false;

		public float Value
		{
			get;
			set;
		}

		public float PaintValue
		{
			get;
			set;
		}

		public float PaintValueBackUp
		{
			get;
			set;
		}

		public Color LineColor
		{
			get;
			set;
		}

		public Pen PenDash
		{
			get;
			set;
		}

		public Pen PenSolid
		{
			get;
			set;
		}

		public float LineThickness
		{
			get;
			set;
		}

		public Brush LineTextBrush
		{
			get;
			set;
		}

		public bool IsLeftFrame
		{
			get;
			set;
		}

        private bool isDashStyle = true;

        public bool IsDashStyle
        {
            get { return isDashStyle; }
            set { isDashStyle = value; }
        }		


		public Pen GetPen()
		{
			return IsDashStyle ? PenDash : PenSolid;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
                    if(PenDash==null)
					PenDash.Dispose();
                    if(PenSolid==null)
					PenSolid.Dispose();
                    if(LineTextBrush==null)
					LineTextBrush.Dispose();
				}
				disposedValue = true;
			}
		}

        public string Tip { get; set; }

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
