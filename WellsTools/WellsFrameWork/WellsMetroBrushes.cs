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
using System.Drawing;
using System.Collections.Generic;

namespace Wells.WellsFramework
{
    public sealed class WellsMetroBrushes
    {
        private static Dictionary<string, SolidBrush> metroBrushes = new Dictionary<string, SolidBrush>();
        private static SolidBrush GetSaveBrush(string key, Color color)
        {
            lock (metroBrushes)
            {
                if (!metroBrushes.ContainsKey(key))
                    metroBrushes.Add(key, new SolidBrush(color));

                return metroBrushes[key].Clone() as SolidBrush;
            }
        }

        public static SolidBrush Black
        {
            get
            {
                return GetSaveBrush("Black", WellsMetroColors.Black);
            }
        }

        public static SolidBrush White
        {
            get
            {
                return GetSaveBrush("White", WellsMetroColors.White);
            }
        }

        public static SolidBrush Silver
        {
            get
            {
                return GetSaveBrush("Silver", WellsMetroColors.Silver);
            }
        }

        public static SolidBrush Blue
        {
            get
            {
                return GetSaveBrush("Blue", WellsMetroColors.Blue);
            }
        }

        public static SolidBrush Green
        {
            get
            {
                return GetSaveBrush("Green", WellsMetroColors.Green);
            }
        }

        public static SolidBrush Lime
        {
            get
            {
                return GetSaveBrush("Lime", WellsMetroColors.Lime);
            }
        }

        public static SolidBrush Teal
        {
            get
            {
                return GetSaveBrush("Teal", WellsMetroColors.Teal);
            }
        }

        public static SolidBrush Orange
        {
            get
            {
                return GetSaveBrush("Orange", WellsMetroColors.Orange);
            }
        }

        public static SolidBrush Brown
        {
            get
            {
                return GetSaveBrush("Brown", WellsMetroColors.Brown);
            }
        }

        public static SolidBrush Pink
        {
            get
            {
                return GetSaveBrush("Pink", WellsMetroColors.Pink);
            }
        }

        public static SolidBrush Magenta
        {
            get
            {
                return GetSaveBrush("Magenta", WellsMetroColors.Magenta);
            }
        }

        public static SolidBrush Purple
        {
            get
            {
                return GetSaveBrush("Purple", WellsMetroColors.Purple);
            }
        }

        public static SolidBrush Red
        {
            get
            {
                return GetSaveBrush("Red", WellsMetroColors.Red);
            }
        }

        public static SolidBrush Yellow
        {
            get
            {
                return GetSaveBrush("Yellow", WellsMetroColors.Yellow);
            }
        }
    }
}
