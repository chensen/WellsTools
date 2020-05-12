using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wells.Controls.VisionInspect
{
    public class clsImage : ICloneable
    {
        public byte[] ImgBuffer;
        public int ImgBufferSize { get { return Width * Height * (Color ? 3 : 1); } }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Color { get; set; }

        public clsImage()
        {
            Width = 0;
            Height = 0;
            Color = false;
            ImgBuffer = null;
        }

        public object Clone()
        {
            clsImage instance = new clsImage();
            instance.Width = this.Width;
            instance.Height = this.Height;
            instance.Color = this.Color;
            if (this.ImgBuffer != null)
            {
                instance.ImgBuffer = new byte[this.ImgBuffer.Length];
                Array.Copy(this.ImgBuffer, instance.ImgBuffer, this.ImgBuffer.Length);
            }
            else
            {
                instance.ImgBuffer = null;
            }
            return instance;
        }

        public void clear()
        {
            ImgBuffer = null;
        }

        public bool isInitialized()
        {
            if (ImgBuffer == null)
                return false;
            if (Width <= 0 || Height <= 0)
                return false;
            return true;
        }
    }
}
