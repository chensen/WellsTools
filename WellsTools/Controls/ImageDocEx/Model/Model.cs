using hvppleDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Wells.Controls.ImageDocEx
{
    [Serializable]
    public class Model : INotifyPropertyChanged
    {
        private string color = "green";
        private string drawMode = "margin";
        private int lineWidth = 1;
        private string lineStyle = "line";
        private string type = "";

        public string Color
        {
            get { return color; }
            set
            {
                if (value == color) return;

                color = value;
                NotifyPropertyChange("Color");
            }
        }

        public string DrawMode
        {
            get { return drawMode; }
            set
            {
                if (value == drawMode) return;

                drawMode = value;
                NotifyPropertyChange("DrawMode");
            }
        }

        public int LineWidth
        {
            get { return lineWidth; }
            set
            {
                if (value == lineWidth) return;

                lineWidth = value;
                NotifyPropertyChange("LineWidth");
            }
        }

        public string LineStyle
        {
            get { return lineStyle; }
            set
            {
                if (value == lineStyle) return;

                lineStyle = value;
                NotifyPropertyChange("LineStyle");
            }
        }

        public string Type
        {
            get { return type; }
            set
            {
                if (value == type) return;

                type = value;
                NotifyPropertyChange("Type");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public virtual void draw(HWindow window) { }
    }
}
