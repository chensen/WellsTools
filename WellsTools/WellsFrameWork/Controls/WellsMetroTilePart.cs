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
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

using Wells.WellsFramework.Components;
using Wells.WellsFramework.Interfaces;

namespace Wells.WellsFramework.Controls
{
    #region Enums

    public enum WellsMetroTilePartContentType
    {
        Text,
        Image,
        Html
    }

    #endregion

    [ToolboxItem(false)]
    public class WellsMetroTilePart : Control, IWellsMetroControl
    {
        #region Interface

        private WellsMetroColorStyle metroStyle = WellsMetroColorStyle.Default;
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(WellsMetroColorStyle.Default)]
        public WellsMetroColorStyle Style
        {
            get
            {
                if (DesignMode || metroStyle != WellsMetroColorStyle.Default)
                {
                    return metroStyle;
                }

                if (StyleManager != null && metroStyle == WellsMetroColorStyle.Default)
                {
                    return StyleManager.Style;
                }
                if (StyleManager == null && metroStyle == WellsMetroColorStyle.Default)
                {
                    return WellsMetroDefaults.Style;
                }

                return metroStyle;
            }
            set { metroStyle = value; }
        }

        private WellsMetroThemeStyle metroTheme = WellsMetroThemeStyle.Default;
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(WellsMetroThemeStyle.Default)]
        public WellsMetroThemeStyle Theme
        {
            get
            {
                if (DesignMode || metroTheme != WellsMetroThemeStyle.Default)
                {
                    return metroTheme;
                }

                if (StyleManager != null && metroTheme == WellsMetroThemeStyle.Default)
                {
                    return StyleManager.Theme;
                }
                if (StyleManager == null && metroTheme == WellsMetroThemeStyle.Default)
                {
                    return WellsMetroDefaults.Theme;
                }

                return metroTheme;
            }
            set { metroTheme = value; }
        }

        private WellsMetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WellsMetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set { metroStyleManager = value; }
        }

        #endregion

        #region Fields

        private WellsMetroTilePartContentType partContentType = WellsMetroTilePartContentType.Text;
        [DefaultValue(WellsMetroTilePartContentType.Text)]
        [Category(WellsMetroDefaults.PropertyCategory.Behaviour)]
        public WellsMetroTilePartContentType ContentType 
        {
            get { return partContentType; }
            set { partContentType = value; }
        }

        private string partHtmlContent = "";
        [DefaultValue("")]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public string HtmlContent
        {
            get { return partHtmlContent; }
            set { partHtmlContent = value; }
        }

        #endregion

        #region Constructor

        public WellsMetroTilePart()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Dock = DockStyle.Fill;
        }

        #endregion

        #region Paint Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (partContentType == WellsMetroTilePartContentType.Html)
            {
                Wells.WellsFramework.Drawing.Html.HtmlRenderer.Render(e.Graphics, partHtmlContent, ClientRectangle, true);
            }
        }

        #endregion
    }
}
