using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wells.Controls.VisionInspect
{
    public class clsImage : ICloneable
    {
        public byte[] ImgBuffer;
        public int ImgBufferSize { get; set; }
        public int Width { get; set; }
        public int Stride { get; set; }
        public int Height { get; set; }
        public int Chanels { get; set; }
        public bool Color { get; set; }

        public clsImage()
        {
            Width = 0;
            Height = 0;
            Stride = 0;
            Color = false;
            Chanels = 1;
            ImgBufferSize = 0;
            ImgBuffer = null;
        }

        public object Clone()
        {
            clsImage instance = new clsImage();
            instance.Width = this.Width;
            instance.Height = this.Height;
            instance.Stride = this.Stride;
            instance.Chanels = this.Chanels;
            instance.Color = this.Color;
            instance.ImgBufferSize = this.ImgBufferSize;
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
            if (Width <= 0 || Height <= 0 || ImgBufferSize <= 0 || Stride <= 0)
                return false;
            if (ImgBufferSize != Height * Stride)
                return false;
            return true;
        }
    }
}
