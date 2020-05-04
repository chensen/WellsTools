/**
 * WellsFramework - Modern UI for WinForms
 * 
 * The MIT License (MIT)
 * Copyright (c) 2011 Sven Walter, http://github.com/viperneo
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing.Text;
using System;

namespace Wells.WellsFramework
{
    public enum WellsMetroLabelSize
    {
        Small,
        Medium,
        Tall
    }

    public enum WellsMetroLabelWeight
    {
        Light,
        Regular,
        Bold
    }

    public enum WellsMetroTileTextSize
    {
        Small,
        Medium,
        Tall
    }

    public enum WellsMetroTileTextWeight
    {
        Light,
        Regular,
        Bold
    }

    public enum WellsMetroLinkSize
    {
        Small,
        Medium,
        Tall
    }

    public enum WellsMetroLinkWeight
    {
        Light,
        Regular,
        Bold
    }

    public enum WellsMetroComboBoxSize
    {
        Small,
        Medium,
        Tall
    }

    public enum WellsMetroComboBoxWeight
    {
        Light,
        Regular,
        Bold
    }

    public enum WellsMetroTextBoxSize
    {
        Small,
        Medium,
        Tall
    }

    public enum WellsMetroTextBoxWeight
    {
        Light,
        Regular,
        Bold
    }

    public enum WellsMetroProgressBarSize
    {
        Small,
        Medium,
        Tall
    }

    public enum WellsMetroProgressBarWeight
    {
        Light,
        Regular,
        Bold
    }

    public enum WellsMetroTabControlSize
    {
        Small,
        Medium,
        Tall
    }

    public enum WellsMetroTabControlWeight
    {
        Light,
        Regular,
        Bold
    }

    public enum WellsMetroCheckBoxSize
    {
        Small,
        Medium,
        Tall
    }

    public enum WellsMetroCheckBoxWeight
    {
        Light,
        Regular,
        Bold
    }

    public enum WellsMetroButtonSize
    {
        Small,
        Medium,
        Tall
    }

    public enum WellsMetroButtonWeight
    {
        Light,
        Regular,
        Bold
    }

    public static class WellsMetroFonts
    {

        #region Font Resolver

        internal interface IWellsMetroFontResolver
        {
            Font ResolveFont(string familyName, float emSize, FontStyle fontStyle, GraphicsUnit unit);
        }

        private class DefaultFontResolver : IWellsMetroFontResolver
        {
            public Font ResolveFont(string familyName, float emSize, FontStyle fontStyle, GraphicsUnit unit)
            {
                return new Font(familyName, emSize, fontStyle, unit);
            }
        }

        private static IWellsMetroFontResolver FontResolver;

        static WellsMetroFonts()
        {
            try
            {
                Type t = Type.GetType(AssemblyRef.MetroFrameworkFontResolver);
                if (t != null)
                {
                    FontResolver = (IWellsMetroFontResolver)Activator.CreateInstance(t);
                    if (FontResolver != null)
                    {
                        //Debug.WriteLine("'" + AssemblyRef.MetroFrameworkFontResolver + "' loaded.", "MetroFonts");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // ignore
                //Debug.WriteLine(ex.Message, "WellsMetroFonts");
            }
            FontResolver = new DefaultFontResolver();
        }

        #endregion

        public static Font DefaultLight(float size)
        {
            return FontResolver.ResolveFont("微软雅黑", size, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        public static Font Default(float size)
        {
            return FontResolver.ResolveFont("微软雅黑", size, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        public static Font DefaultBold(float size)
        {
            return FontResolver.ResolveFont("微软雅黑", size, FontStyle.Bold, GraphicsUnit.Pixel);
        }

        public static Font Title
        {
            get { return DefaultLight(24f); }
        }

        public static Font Subtitle
        {
            get { return Default(14f); }
        }

        public static Font Tile(WellsMetroTileTextSize labelSize, WellsMetroTileTextWeight labelWeight)
        {
            if (labelSize == WellsMetroTileTextSize.Small)
            {
                if (labelWeight == WellsMetroTileTextWeight.Light)
                    return DefaultLight(12f);
                if (labelWeight == WellsMetroTileTextWeight.Regular)
                    return Default(12f);
                if (labelWeight == WellsMetroTileTextWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (labelSize == WellsMetroTileTextSize.Medium)
            {
                if (labelWeight == WellsMetroTileTextWeight.Light)
                    return DefaultLight(14f);
                if (labelWeight == WellsMetroTileTextWeight.Regular)
                    return Default(14f);
                if (labelWeight == WellsMetroTileTextWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (labelSize == WellsMetroTileTextSize.Tall)
            {
                if (labelWeight == WellsMetroTileTextWeight.Light)
                    return DefaultLight(18f);
                if (labelWeight == WellsMetroTileTextWeight.Regular)
                    return Default(18f);
                if (labelWeight == WellsMetroTileTextWeight.Bold)
                    return DefaultBold(18f);
            }

            return DefaultLight(14f);
        }

        public static Font TileCount
        {
            get { return Default(44f); }
        }

        public static Font Link(WellsMetroLinkSize linkSize, WellsMetroLinkWeight linkWeight)
        {
            if (linkSize == WellsMetroLinkSize.Small)
            {
                if (linkWeight == WellsMetroLinkWeight.Light)
                    return DefaultLight(12f);
                if (linkWeight == WellsMetroLinkWeight.Regular)
                    return Default(12f);
                if (linkWeight == WellsMetroLinkWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (linkSize == WellsMetroLinkSize.Medium)
            {
                if (linkWeight == WellsMetroLinkWeight.Light)
                    return DefaultLight(14f);
                if (linkWeight == WellsMetroLinkWeight.Regular)
                    return Default(14f);
                if (linkWeight == WellsMetroLinkWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (linkSize == WellsMetroLinkSize.Tall)
            {
                if (linkWeight == WellsMetroLinkWeight.Light)
                    return DefaultLight(18f);
                if (linkWeight == WellsMetroLinkWeight.Regular)
                    return Default(18f);
                if (linkWeight == WellsMetroLinkWeight.Bold)
                    return DefaultBold(18f);
            }

            return Default(12f);
        }

        public static Font ComboBox(WellsMetroComboBoxSize linkSize, WellsMetroComboBoxWeight linkWeight)
        {
            if (linkSize == WellsMetroComboBoxSize.Small)
            {
                if (linkWeight == WellsMetroComboBoxWeight.Light)
                    return DefaultLight(12f);
                if (linkWeight == WellsMetroComboBoxWeight.Regular)
                    return Default(12f);
                if (linkWeight == WellsMetroComboBoxWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (linkSize == WellsMetroComboBoxSize.Medium)
            {
                if (linkWeight == WellsMetroComboBoxWeight.Light)
                    return DefaultLight(14f);
                if (linkWeight == WellsMetroComboBoxWeight.Regular)
                    return Default(14f);
                if (linkWeight == WellsMetroComboBoxWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (linkSize == WellsMetroComboBoxSize.Tall)
            {
                if (linkWeight == WellsMetroComboBoxWeight.Light)
                    return DefaultLight(18f);
                if (linkWeight == WellsMetroComboBoxWeight.Regular)
                    return Default(18f);
                if (linkWeight == WellsMetroComboBoxWeight.Bold)
                    return DefaultBold(18f);
            }

            return Default(12f);
        }

        public static Font Label(WellsMetroLabelSize labelSize, WellsMetroLabelWeight labelWeight)
        {
            if (labelSize == WellsMetroLabelSize.Small)
            {
                if (labelWeight == WellsMetroLabelWeight.Light)
                    return DefaultLight(12f);
                if (labelWeight == WellsMetroLabelWeight.Regular)
                    return Default(12f);
                if (labelWeight == WellsMetroLabelWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (labelSize == WellsMetroLabelSize.Medium)
            {
                if (labelWeight == WellsMetroLabelWeight.Light)
                    return DefaultLight(14f);
                if (labelWeight == WellsMetroLabelWeight.Regular)
                    return Default(14f);
                if (labelWeight == WellsMetroLabelWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (labelSize == WellsMetroLabelSize.Tall)
            {
                if (labelWeight == WellsMetroLabelWeight.Light)
                    return DefaultLight(18f);
                if (labelWeight == WellsMetroLabelWeight.Regular)
                    return Default(18f);
                if (labelWeight == WellsMetroLabelWeight.Bold)
                    return DefaultBold(18f);
            }

            return DefaultLight(14f);
        }

        public static Font TextBox(WellsMetroTextBoxSize linkSize, WellsMetroTextBoxWeight linkWeight)
        {
            if (linkSize == WellsMetroTextBoxSize.Small)
            {
                if (linkWeight == WellsMetroTextBoxWeight.Light)
                    return DefaultLight(12f);
                if (linkWeight == WellsMetroTextBoxWeight.Regular)
                    return Default(12f);
                if (linkWeight == WellsMetroTextBoxWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (linkSize == WellsMetroTextBoxSize.Medium)
            {
                if (linkWeight == WellsMetroTextBoxWeight.Light)
                    return DefaultLight(14f);
                if (linkWeight == WellsMetroTextBoxWeight.Regular)
                    return Default(14f);
                if (linkWeight == WellsMetroTextBoxWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (linkSize == WellsMetroTextBoxSize.Tall)
            {
                if (linkWeight == WellsMetroTextBoxWeight.Light)
                    return DefaultLight(18f);
                if (linkWeight == WellsMetroTextBoxWeight.Regular)
                    return Default(18f);
                if (linkWeight == WellsMetroTextBoxWeight.Bold)
                    return DefaultBold(18f);
            }

            return Default(12f);
        }

        public static Font ProgressBar(WellsMetroProgressBarSize labelSize, WellsMetroProgressBarWeight labelWeight)
        {
            if (labelSize == WellsMetroProgressBarSize.Small)
            {
                if (labelWeight == WellsMetroProgressBarWeight.Light)
                    return DefaultLight(12f);
                if (labelWeight == WellsMetroProgressBarWeight.Regular)
                    return Default(12f);
                if (labelWeight == WellsMetroProgressBarWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (labelSize == WellsMetroProgressBarSize.Medium)
            {
                if (labelWeight == WellsMetroProgressBarWeight.Light)
                    return DefaultLight(14f);
                if (labelWeight == WellsMetroProgressBarWeight.Regular)
                    return Default(14f);
                if (labelWeight == WellsMetroProgressBarWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (labelSize == WellsMetroProgressBarSize.Tall)
            {
                if (labelWeight == WellsMetroProgressBarWeight.Light)
                    return DefaultLight(18f);
                if (labelWeight == WellsMetroProgressBarWeight.Regular)
                    return Default(18f);
                if (labelWeight == WellsMetroProgressBarWeight.Bold)
                    return DefaultBold(18f);
            }

            return DefaultLight(14f);
        }

        public static Font TabControl(WellsMetroTabControlSize labelSize, WellsMetroTabControlWeight labelWeight)
        {
            if (labelSize == WellsMetroTabControlSize.Small)
            {
                if (labelWeight == WellsMetroTabControlWeight.Light)
                    return DefaultLight(12f);
                if (labelWeight == WellsMetroTabControlWeight.Regular)
                    return Default(12f);
                if (labelWeight == WellsMetroTabControlWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (labelSize == WellsMetroTabControlSize.Medium)
            {
                if (labelWeight == WellsMetroTabControlWeight.Light)
                    return DefaultLight(14f);
                if (labelWeight == WellsMetroTabControlWeight.Regular)
                    return Default(14f);
                if (labelWeight == WellsMetroTabControlWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (labelSize == WellsMetroTabControlSize.Tall)
            {
                if (labelWeight == WellsMetroTabControlWeight.Light)
                    return DefaultLight(18f);
                if (labelWeight == WellsMetroTabControlWeight.Regular)
                    return Default(18f);
                if (labelWeight == WellsMetroTabControlWeight.Bold)
                    return DefaultBold(18f);
            }

            return DefaultLight(14f);
        }

        public static Font CheckBox(WellsMetroCheckBoxSize linkSize, WellsMetroCheckBoxWeight linkWeight)
        {
            if (linkSize == WellsMetroCheckBoxSize.Small)
            {
                if (linkWeight == WellsMetroCheckBoxWeight.Light)
                    return DefaultLight(12f);
                if (linkWeight == WellsMetroCheckBoxWeight.Regular)
                    return Default(12f);
                if (linkWeight == WellsMetroCheckBoxWeight.Bold)
                    return DefaultBold(12f);
            }
            else if (linkSize == WellsMetroCheckBoxSize.Medium)
            {
                if (linkWeight == WellsMetroCheckBoxWeight.Light)
                    return DefaultLight(14f);
                if (linkWeight == WellsMetroCheckBoxWeight.Regular)
                    return Default(14f);
                if (linkWeight == WellsMetroCheckBoxWeight.Bold)
                    return DefaultBold(14f);
            }
            else if (linkSize == WellsMetroCheckBoxSize.Tall)
            {
                if (linkWeight == WellsMetroCheckBoxWeight.Light)
                    return DefaultLight(18f);
                if (linkWeight == WellsMetroCheckBoxWeight.Regular)
                    return Default(18f);
                if (linkWeight == WellsMetroCheckBoxWeight.Bold)
                    return DefaultBold(18f);
            }

            return Default(12f);
        }

        public static Font Button(WellsMetroButtonSize linkSize, WellsMetroButtonWeight linkWeight)
        {
            if (linkSize == WellsMetroButtonSize.Small)
            {
                if (linkWeight == WellsMetroButtonWeight.Light)
                    return DefaultLight(11f);
                if (linkWeight == WellsMetroButtonWeight.Regular)
                    return Default(11f);
                if (linkWeight == WellsMetroButtonWeight.Bold)
                    return DefaultBold(11f);
            }
            else if (linkSize == WellsMetroButtonSize.Medium)
            {
                if (linkWeight == WellsMetroButtonWeight.Light)
                    return DefaultLight(13f);
                if (linkWeight == WellsMetroButtonWeight.Regular)
                    return Default(13f);
                if (linkWeight == WellsMetroButtonWeight.Bold)
                    return DefaultBold(13f);
            }
            else if (linkSize == WellsMetroButtonSize.Tall)
            {
                if (linkWeight == WellsMetroButtonWeight.Light)
                    return DefaultLight(16f);
                if (linkWeight == WellsMetroButtonWeight.Regular)
                    return Default(16f);
                if (linkWeight == WellsMetroButtonWeight.Bold)
                    return DefaultBold(16f);
            }

            return Default(11f);
        }
    }
}
