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
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Wells.WellsFramework.Drawing
{
    public class WellsMetroPaintEventArgs : EventArgs
    {
        public Color BackColor { get; private set; }
        public Color ForeColor { get; private set; }
        public Graphics Graphics { get; private set; }

        public WellsMetroPaintEventArgs(Color backColor, Color foreColor, Graphics g)
        {
            BackColor = backColor;
            ForeColor = foreColor;
            Graphics = g;
        }
    }

    public sealed class WellsMetroPaint
    {
        public sealed class BorderColor
        {
            public static Color Form(WellsMetroThemeStyle theme)
            {
                if (theme == WellsMetroThemeStyle.Dark)
                    return Color.FromArgb(68, 68, 68);

                return Color.FromArgb(204, 204, 204);
            }

            public static class Button
            {
                public static Color Normal(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Hover(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(102, 102, 102);
                }

                public static Color Press(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(238, 238, 238);

                    return Color.FromArgb(51, 51, 51);
                }

                public static Color Disabled(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(109, 109, 109);

                    return Color.FromArgb(155, 155, 155);
                }
            }

            public static class CheckBox
            {
                public static Color Normal(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Hover(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(204, 204, 204);

                    return Color.FromArgb(51, 51, 51);
                }

                public static Color Press(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Disabled(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(85, 85, 85);

                    return Color.FromArgb(204, 204, 204);
                }
            }

            public static class ComboBox
            {
                public static Color Normal(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Hover(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(204, 204, 204);

                    return Color.FromArgb(51, 51, 51);
                }

                public static Color Press(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Disabled(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(85, 85, 85);

                    return Color.FromArgb(204, 204, 204);
                }
            }

            public static class ProgressBar
            {
                public static Color Normal(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Hover(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Press(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Disabled(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(109, 109, 109);

                    return Color.FromArgb(155, 155, 155);
                }
            }

            public static class TabControl
            {
                public static Color Normal(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Hover(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Press(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Disabled(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(109, 109, 109);

                    return Color.FromArgb(155, 155, 155);
                }
            }
        }

        public sealed class BackColor
        {
            public static Color Form(WellsMetroThemeStyle theme)
            {
                if (theme == WellsMetroThemeStyle.Dark)
                    return Color.FromArgb(17, 17, 17);

                return Color.FromArgb(255, 255, 255);
            }

            public sealed class Button
            {
                public static Color Normal(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(34, 34, 34);

                    return Color.FromArgb(238, 238, 238);
                }

                public static Color Hover(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(102, 102, 102);
                }

                public static Color Press(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(238, 238, 238);

                    return Color.FromArgb(51, 51, 51);
                }

                public static Color Disabled(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(80, 80, 80);

                    return Color.FromArgb(204, 204, 204);
                }
            }

            public sealed class TrackBar
            {
                public sealed class Thumb
                {
                    public static Color Normal(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(153, 153, 153);

                        return Color.FromArgb(102, 102, 102);
                    }

                    public static Color Hover(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(204, 204, 204);

                        return Color.FromArgb(17, 17, 17);
                    }

                    public static Color Press(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(204, 204, 204);

                        return Color.FromArgb(17, 17, 17);
                    }

                    public static Color Disabled(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(85, 85, 85);

                        return Color.FromArgb(179, 179, 179);
                    }
                }

                public sealed class Bar
                {
                    public static Color Normal(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(204, 204, 204);
                    }

                    public static Color Hover(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(204, 204, 204);
                    }

                    public static Color Press(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(204, 204, 204);
                    }

                    public static Color Disabled(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(34, 34, 34);

                        return Color.FromArgb(230, 230, 230);
                    }
                }
            }

            public sealed class ScrollBar
            {
                public sealed class Thumb
                {
                    public static Color Normal(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(221, 221, 221);
                    }

                    public static Color Hover(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(204, 204, 204);

                        return Color.FromArgb(17, 17, 17);
                    }

                    public static Color Press(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(204, 204, 204);

                        return Color.FromArgb(17, 17, 17);
                    }

                    public static Color Disabled(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(221, 221, 221);
                    }
                }

                public sealed class Bar
                {
                    public static Color Normal(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Hover(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Press(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Disabled(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }
                }
            }

            public sealed class ProgressBar
            {
                public sealed class Bar
                {
                    public static Color Normal(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Hover(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Press(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Disabled(WellsMetroThemeStyle theme)
                    {
                        if (theme == WellsMetroThemeStyle.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(221, 221, 221);
                    }
                }
            }
        }

        public sealed class ForeColor
        {
            public sealed class Button
            {
                public static Color Normal(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(204, 204, 204);

                    return Color.FromArgb(0, 0, 0);
                }

                public static Color Hover(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(17, 17, 17);

                    return Color.FromArgb(255, 255, 255);
                }

                public static Color Press(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(17, 17, 17);

                    return Color.FromArgb(255, 255, 255);
                }

                public static Color Disabled(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(109, 109, 109);

                    return Color.FromArgb(136, 136, 136);
                }
            }

            public static Color Title(WellsMetroThemeStyle theme)
            {
                if (theme == WellsMetroThemeStyle.Dark)
                    return Color.FromArgb(255, 255, 255);

                return Color.FromArgb(0, 0, 0);
            }

            public sealed class Tile
            {
                public static Color Normal(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(255, 255, 255);

                    return Color.FromArgb(255, 255, 255);
                }

                public static Color Hover(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(255, 255, 255);

                    return Color.FromArgb(255, 255, 255);
                }

                public static Color Press(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(255, 255, 255);

                    return Color.FromArgb(255, 255, 255);
                }

                public static Color Disabled(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(209, 209, 209);

                    return Color.FromArgb(209, 209, 209);
                }
            }

            public sealed class Link
            {
                public static Color Normal(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(0, 0, 0);
                }

                public static Color Hover(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(93, 93, 93);

                    return Color.FromArgb(128, 128, 128);
                }

                public static Color Press(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(93, 93, 93);

                    return Color.FromArgb(128, 128, 128);
                }

                public static Color Disabled(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(51, 51, 51);

                    return Color.FromArgb(209, 209, 209);
                }
            }

            public sealed class Label
            {
                public static Color Normal(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(0, 0, 0);
                }

                public static Color Disabled(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(51, 51, 51);

                    return Color.FromArgb(209, 209, 209);
                }
            }

            public sealed class CheckBox
            {
                public static Color Normal(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(17, 17, 17);
                }

                public static Color Hover(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Press(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Disabled(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(93, 93, 93);

                    return Color.FromArgb(136, 136, 136);
                }
            }

            public sealed class ComboBox
            {
                public static Color Normal(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Hover(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(17, 17, 17);
                }

                public static Color Press(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Disabled(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(93, 93, 93);

                    return Color.FromArgb(136, 136, 136);
                }
            }

            public sealed class ProgressBar
            {
                public static Color Normal(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(0, 0, 0);
                }

                public static Color Disabled(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(51, 51, 51);

                    return Color.FromArgb(209, 209, 209);
                }
            }

            public sealed class TabControl
            {
                public static Color Normal(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(0, 0, 0);
                }

                public static Color Disabled(WellsMetroThemeStyle theme)
                {
                    if (theme == WellsMetroThemeStyle.Dark)
                        return Color.FromArgb(51, 51, 51);

                    return Color.FromArgb(209, 209, 209);
                }
            }
        }

        #region Helper Methods

        public static Color GetStyleColor(WellsMetroColorStyle style)
        {
            switch (style)
            {
                case WellsMetroColorStyle.Black:
                    return WellsMetroColors.Black;

                case WellsMetroColorStyle.White:
                    return WellsMetroColors.White;

                case WellsMetroColorStyle.Silver:
                    return WellsMetroColors.Silver;

                case WellsMetroColorStyle.Blue:
                    return WellsMetroColors.Blue;

                case WellsMetroColorStyle.Green:
                    return WellsMetroColors.Green;

                case WellsMetroColorStyle.Lime:
                    return WellsMetroColors.Lime;

                case WellsMetroColorStyle.Teal:
                    return WellsMetroColors.Teal;

                case WellsMetroColorStyle.Orange:
                    return WellsMetroColors.Orange;

                case WellsMetroColorStyle.Brown:
                    return WellsMetroColors.Brown;

                case WellsMetroColorStyle.Pink:
                    return WellsMetroColors.Pink;

                case WellsMetroColorStyle.Magenta:
                    return WellsMetroColors.Magenta;

                case WellsMetroColorStyle.Purple:
                    return WellsMetroColors.Purple;

                case WellsMetroColorStyle.Red:
                    return WellsMetroColors.Red;

                case WellsMetroColorStyle.Yellow:
                    return WellsMetroColors.Yellow;

                default:
                    return WellsMetroColors.Blue;
            }
        }

        public static SolidBrush GetStyleBrush(WellsMetroColorStyle style)
        {
            switch (style)
            {
                case WellsMetroColorStyle.Black:
                    return WellsMetroBrushes.Black;

                case WellsMetroColorStyle.White:
                    return WellsMetroBrushes.White;

                case WellsMetroColorStyle.Silver:
                    return WellsMetroBrushes.Silver;

                case WellsMetroColorStyle.Blue:
                    return WellsMetroBrushes.Blue;

                case WellsMetroColorStyle.Green:
                    return WellsMetroBrushes.Green;

                case WellsMetroColorStyle.Lime:
                    return WellsMetroBrushes.Lime;

                case WellsMetroColorStyle.Teal:
                    return WellsMetroBrushes.Teal;

                case WellsMetroColorStyle.Orange:
                    return WellsMetroBrushes.Orange;

                case WellsMetroColorStyle.Brown:
                    return WellsMetroBrushes.Brown;

                case WellsMetroColorStyle.Pink:
                    return WellsMetroBrushes.Pink;

                case WellsMetroColorStyle.Magenta:
                    return WellsMetroBrushes.Magenta;

                case WellsMetroColorStyle.Purple:
                    return WellsMetroBrushes.Purple;

                case WellsMetroColorStyle.Red:
                    return WellsMetroBrushes.Red;

                case WellsMetroColorStyle.Yellow:
                    return WellsMetroBrushes.Yellow;

                default:
                    return WellsMetroBrushes.Blue;
            }
        }

        public static Pen GetStylePen(WellsMetroColorStyle style)
        {
            switch (style)
            {
                case WellsMetroColorStyle.Black:
                    return WellsMetroPens.Black;

                case WellsMetroColorStyle.White:
                    return WellsMetroPens.White;
                
                case WellsMetroColorStyle.Silver:
                    return WellsMetroPens.Silver;

                case WellsMetroColorStyle.Blue:
                    return WellsMetroPens.Blue;

                case WellsMetroColorStyle.Green:
                    return WellsMetroPens.Green;

                case WellsMetroColorStyle.Lime:
                    return WellsMetroPens.Lime;

                case WellsMetroColorStyle.Teal:
                    return WellsMetroPens.Teal;

                case WellsMetroColorStyle.Orange:
                    return WellsMetroPens.Orange;

                case WellsMetroColorStyle.Brown:
                    return WellsMetroPens.Brown;

                case WellsMetroColorStyle.Pink:
                    return WellsMetroPens.Pink;

                case WellsMetroColorStyle.Magenta:
                    return WellsMetroPens.Magenta;

                case WellsMetroColorStyle.Purple:
                    return WellsMetroPens.Purple;

                case WellsMetroColorStyle.Red:
                    return WellsMetroPens.Red;

                case WellsMetroColorStyle.Yellow:
                    return WellsMetroPens.Yellow;

                default:
                    return WellsMetroPens.Blue;
            }
        }

        public static StringFormat GetStringFormat(ContentAlignment textAlign)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Trimming = StringTrimming.EllipsisCharacter;

            switch (textAlign)
            {
                case ContentAlignment.TopLeft:
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopCenter:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;

                case ContentAlignment.MiddleLeft:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleCenter:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleRight:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;

                case ContentAlignment.BottomLeft:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomCenter:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomRight:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
            }

            return stringFormat;
        }

        public static TextFormatFlags GetTextFormatFlags(ContentAlignment textAlign)
        {
            return GetTextFormatFlags(textAlign, false);
        }

        public static TextFormatFlags GetTextFormatFlags(ContentAlignment textAlign,bool WrapToLine)
        {
            TextFormatFlags controlFlags = TextFormatFlags.Default;

            switch (WrapToLine)
            {
                case true:
                    controlFlags = TextFormatFlags.WordBreak;
                    break;
                case false:
                    controlFlags = TextFormatFlags.EndEllipsis;
                    break;
            }
            switch (textAlign)
            {
                case ContentAlignment.TopLeft:
                    controlFlags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;
                case ContentAlignment.TopCenter:
                    controlFlags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.TopRight:
                    controlFlags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;

                case ContentAlignment.MiddleLeft:
                    controlFlags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case ContentAlignment.MiddleCenter:
                    controlFlags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.MiddleRight:
                    controlFlags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;

                case ContentAlignment.BottomLeft:
                    controlFlags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;
                case ContentAlignment.BottomCenter:
                    controlFlags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.BottomRight:
                    controlFlags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;
            }

            return controlFlags;
        }

        #endregion
    }
}
