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
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Wells.WellsFramework.Controls;
using Wells.WellsFramework.Interfaces;

namespace Wells.WellsFramework.Components
{
    [Designer(typeof(Design.Components.WellsMetroStyleManagerDesigner), typeof(System.ComponentModel.Design.IRootDesigner))]
    //[Designer("Wells.WellsFramework.Design.Components.WellsMetroStyleManagerDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    public sealed class WellsMetroStyleManager : Component, ICloneable, ISupportInitialize
    {
        #region Fields

        private readonly IContainer parentContainer;

        private WellsMetroColorStyle metroStyle = WellsMetroDefaults.Style;
        [DefaultValue(WellsMetroDefaults.Style)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroColorStyle Style
        {
            get { return metroStyle; }
            set
            {
                if (value == WellsMetroColorStyle.Default)
                {
                    value = WellsMetroDefaults.Style;
                }

                metroStyle = value;

                if (!isInitializing)
                {
                    Update();
                }
            }
        }

        private WellsMetroThemeStyle metroTheme = WellsMetroDefaults.Theme;
        [DefaultValue(WellsMetroDefaults.Theme)]
        [Category(WellsMetroDefaults.PropertyCategory.Appearance)]
        public WellsMetroThemeStyle Theme
        {
            get { return metroTheme; }
            set
            {
                if (value == WellsMetroThemeStyle.Default)
                {
                    value = WellsMetroDefaults.Theme;
                }

                metroTheme = value;

                if (!isInitializing)
                {
                    Update();
                }
            }
        }

        private ContainerControl owner;
        public ContainerControl Owner
        {
            get { return owner; }
            set
            {
                if (owner != null)
                {
                    owner.ControlAdded -= ControlAdded;
                }

                owner = value;

                if (value != null)
                {
                    owner.ControlAdded += ControlAdded;

                    if (!isInitializing)
                    {
                        UpdateControl(value);
                    }
                }
            }
        }

        #endregion

        #region Constructor

        public WellsMetroStyleManager()
        {
        
        }

        public WellsMetroStyleManager(IContainer parentContainer)
            : this()
        {
            if (parentContainer != null)
            {
                this.parentContainer = parentContainer;
                this.parentContainer.Add(this);
            }
        }

        #endregion

        #region ICloneable

        public object Clone()
        {
            WellsMetroStyleManager newStyleManager = new WellsMetroStyleManager();
            newStyleManager.metroTheme = Theme;
            newStyleManager.metroStyle = Style;
            return newStyleManager;
        }

        public object Clone(ContainerControl owner)
        {
            WellsMetroStyleManager clonedManager = Clone() as WellsMetroStyleManager;

            if (owner is IWellsMetroForm)
            {
                clonedManager.Owner = owner;
            }

            return clonedManager;
        }

        #endregion

        #region ISupportInitialize

        private bool isInitializing;

        void ISupportInitialize.BeginInit()
        {
            isInitializing = true;
        }

        void ISupportInitialize.EndInit()
        {
            isInitializing = false;
            Update();
        }

        #endregion

        #region Management Methods

        private void ControlAdded(object sender, ControlEventArgs e)
        {
            if (!isInitializing)
            {
                UpdateControl(e.Control);
            }
        }

        public void Update()
        {
            if (owner != null)
            {
                UpdateControl(owner);
            }

            if (parentContainer == null || parentContainer.Components == null)
            {
                return;
            }

            foreach (Object obj in parentContainer.Components)
            {
                if (obj is IWellsMetroComponent)
                {
                    ApplyTheme((IWellsMetroComponent)obj);
                }

                if (obj.GetType() == typeof(WellsMetroContextMenu))
                {
                    ApplyTheme((WellsMetroContextMenu)obj);
                }
            }
        }

        private void UpdateControl(Control ctrl)
        {
            if (ctrl == null)
            {
                return;
            }

            IWellsMetroControl metroControl = ctrl as IWellsMetroControl;
            if (metroControl != null)
            {
                ApplyTheme(metroControl);
            }

            IWellsMetroComponent metroComponent = ctrl as IWellsMetroComponent;
            if (metroComponent != null)
            {
                ApplyTheme(metroComponent);
            }

            TabControl tabControl = ctrl as TabControl;
            if (tabControl != null)
            {
                foreach (TabPage tp in ((TabControl)ctrl).TabPages)
                {
                    UpdateControl(tp);
                }
            }

            if (ctrl.Controls != null)
            {
                foreach (Control child in ctrl.Controls)
                {
                    UpdateControl(child);
                }
            }

            if (ctrl.ContextMenuStrip != null)
            {
                UpdateControl(ctrl.ContextMenuStrip);
            }

            ctrl.Refresh();
        }

        private void ApplyTheme(IWellsMetroControl control)
        {
            control.StyleManager = this;
        }

        private void ApplyTheme(IWellsMetroComponent component)
        {
            component.StyleManager = this;
        }

        #endregion
    }
}
