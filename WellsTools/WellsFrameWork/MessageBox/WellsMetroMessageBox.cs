using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Wells.WellsFramework.Forms;
using Wells.WellsFramework.Interfaces;

namespace Wells.WellsFramework
{
    /// <summary>
    /// Metro-styled message notification.
    /// </summary>
    public static class WellsMetroMessageBox
    {
        public static WellsMetroMessageBoxControl _control = null;
        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message)
        { return Show(owner, message, "Notification"); }

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title)
        { return Show(owner, message, title, MessageBoxButtons.OK); }

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title, MessageBoxButtons buttons)
        { return Show(owner, message, title, buttons, MessageBoxIcon.None); }

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title, MessageBoxButtons buttons, MessageBoxIcon icon)
        { return Show(owner, message, title, buttons, icon, MessageBoxDefaultButton.Button1); }

        /// <summary>
        /// Shows a metro-styles message notification into the specified owner window.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <param name="defaultbutton"></param>
        /// <returns></returns>
        public static DialogResult Show(IWin32Window owner, String message, String title, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultbutton)
        {
            DialogResult result = DialogResult.None;
            Form form = (owner == null) ? null : ((Form)owner);
            if (icon != MessageBoxIcon.Hand)
            {
                if (icon != MessageBoxIcon.Question)
                {
                    if (icon != MessageBoxIcon.Exclamation)
                    {
                        SystemSounds.Asterisk.Play();
                    }
                    else
                    {
                        SystemSounds.Exclamation.Play();
                    }
                }
                else
                {
                    SystemSounds.Beep.Play();
                }
            }
            else
            {
                SystemSounds.Hand.Play();
            }
            if (_control != null)
                _control = null;
            _control = new WellsMetroMessageBoxControl();
            _control.BackColor = ((form == null) ? System.Drawing.Color.CadetBlue : form.BackColor);
            _control.Properties.Buttons = buttons;
            _control.Properties.DefaultButton = defaultbutton;
            _control.Properties.Icon = icon;
            _control.Properties.Message = message;
            _control.Properties.Title = title;
            _control.Padding = new Padding(0, 0, 0, 0);
            _control.ShowInTaskbar = false;
            _control.Size = ((form == null) ? new System.Drawing.Size(500, 270) : new System.Drawing.Size(form.Size.Width - 16, _control.Height));
            int x = Convert.ToInt32(Math.Ceiling((decimal)(Screen.PrimaryScreen.WorkingArea.Size.Width / 2 - _control.Size.Width / 2)));
            int y = Convert.ToInt32(Math.Ceiling((decimal)(Screen.PrimaryScreen.WorkingArea.Size.Height / 2 - _control.Size.Height / 2)));
            _control.Location = ((form == null) ? new System.Drawing.Point(x, y) : new System.Drawing.Point(form.Location.X + 8, form.Location.Y + (form.Height - _control.Height) / 2));
            _control.ArrangeApperance();
            Convert.ToInt32(Math.Floor((double)_control.Size.Height * 0.28));
            _control.ShowDialog();
            _control.BringToFront();
            _control.SetDefaultButton();
            Action<WellsMetroMessageBoxControl> action = new Action<WellsMetroMessageBoxControl>(ModalState);
            IAsyncResult asyncResult = action.BeginInvoke(_control, null, action);
            bool flag = false;
            try
            {
                while (!asyncResult.IsCompleted)
                {
                    Thread.Sleep(1);
                    Application.DoEvents();
                }
            }
            catch
            {
                flag = true;
                if (!asyncResult.IsCompleted)
                {
                    try
                    {
                        asyncResult = null;
                    }
                    catch
                    {
                    }
                }
            }
            if (!flag)
            {
                result = _control.Result;
                _control.Dispose();
                _control = null;
            }
            return result;
        }

        private static void ModalState(WellsMetroMessageBoxControl control)
        {
            while (control.Visible)
            { }
        }

        public static void CloseMsgForm()
        {
            if (_control != null && _control.Visible)
            {
                _control.Close();
            }
        }
    }
}


//namespace Wells.WellsFramework
//{
//    /// <summary>
//    /// Metro-styled message notification.
//    /// </summary>
//    public static class WellsMetroMessageBox
//    {
//        /// <summary>
//        /// Shows a metro-styles message notification into the specified owner window.
//        /// </summary>
//        /// <param name="owner"></param>
//        /// <param name="message"></param>
//        /// <returns></returns>
//        public static DialogResult Show(IWin32Window owner, String message)
//        { return Show(owner, message, "Notification"); }

//        /// <summary>
//        /// Shows a metro-styles message notification into the specified owner window.
//        /// </summary>
//        /// <param name="owner"></param>
//        /// <param name="message"></param>
//        /// <param name="title"></param>
//        /// <returns></returns>
//        public static DialogResult Show(IWin32Window owner, String message, String title)
//        { return Show(owner, message, title, MessageBoxButtons.OK); }

//        /// <summary>
//        /// Shows a metro-styles message notification into the specified owner window.
//        /// </summary>
//        /// <param name="owner"></param>
//        /// <param name="message"></param>
//        /// <param name="title"></param>
//        /// <param name="buttons"></param>
//        /// <returns></returns>
//        public static DialogResult Show(IWin32Window owner, String message, String title, MessageBoxButtons buttons)
//        { return Show(owner, message, title, buttons, MessageBoxIcon.None); }

//        /// <summary>
//        /// Shows a metro-styles message notification into the specified owner window.
//        /// </summary>
//        /// <param name="owner"></param>
//        /// <param name="message"></param>
//        /// <param name="title"></param>
//        /// <param name="buttons"></param>
//        /// <param name="icon"></param>
//        /// <returns></returns>
//        public static DialogResult Show(IWin32Window owner, String message, String title, MessageBoxButtons buttons, MessageBoxIcon icon)
//        { return Show(owner, message, title, buttons, icon, MessageBoxDefaultButton.Button1); }

//        /// <summary>
//        /// Shows a metro-styles message notification into the specified owner window.
//        /// </summary>
//        /// <param name="owner"></param>
//        /// <param name="message"></param>
//        /// <param name="title"></param>
//        /// <param name="buttons"></param>
//        /// <param name="icon"></param>
//        /// <param name="defaultbutton"></param>
//        /// <returns></returns>
//        public static DialogResult Show(IWin32Window owner, String message, String title, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultbutton)
//        {
//            DialogResult _result = DialogResult.None;

//            if (owner != null)
//            {
//                Form _owner = (Form)owner;

//                //int _minWidth = 500;
//                //int _minHeight = 350;

//                //if (_owner.Size.Width < _minWidth ||
//                //    _owner.Size.Height < _minHeight)
//                //{
//                //    if (_owner.Size.Width < _minWidth && _owner.Size.Height < _minHeight) {
//                //            _owner.Size = new Size(_minWidth, _minHeight);
//                //    }
//                //    else
//                //    {
//                //        if (_owner.Size.Width < _minWidth) _owner.Size = new Size(_minWidth, _owner.Size.Height);
//                //        else _owner.Size = new Size(_owner.Size.Width, _minHeight);
//                //    }

//                //    int x = Convert.ToInt32(Math.Ceiling((decimal)(Screen.PrimaryScreen.WorkingArea.Size.Width / 2) - (_owner.Size.Width / 2)));
//                //    int y = Convert.ToInt32(Math.Ceiling((decimal)(Screen.PrimaryScreen.WorkingArea.Size.Height / 2) - (_owner.Size.Height / 2)));
//                //    _owner.Location = new Point(x, y);
//                //}

//                switch (icon)
//                {
//                    case MessageBoxIcon.Error:
//                        SystemSounds.Hand.Play(); break;
//                    case MessageBoxIcon.Exclamation:
//                        SystemSounds.Exclamation.Play(); break;
//                    case MessageBoxIcon.Question:
//                        SystemSounds.Beep.Play(); break;
//                    default:
//                        SystemSounds.Asterisk.Play(); break;
//                }

//                WellsMetroMessageBoxControl _control = new WellsMetroMessageBoxControl();
//                _control.BackColor = _owner.BackColor;
//                _control.Properties.Buttons = buttons;
//                _control.Properties.DefaultButton = defaultbutton;
//                _control.Properties.Icon = icon;
//                _control.Properties.Message = message;
//                _control.Properties.Title = title;
//                _control.Padding = new Padding(0, 0, 0, 0);
//                _control.ControlBox = false;
//                _control.ShowInTaskbar = false;  



//                //_owner.Controls.Add(_control);
//                //if (_owner is IMetroForm)
//                //{
//                //    //if (((MetroForm)_owner).DisplayHeader)
//                //    //{
//                //    //    _offset += 30;
//                //    //}
//                //    _control.Theme = ((MetroForm)_owner).Theme;
//                //    _control.Style = ((MetroForm)_owner).Style;
//                //}

//                _control.Size = new Size(_owner.Size.Width, _control.Height);
//                _control.Location = new Point(_owner.Location.X, _owner.Location.Y + (_owner.Height - _control.Height) / 2);
//                _control.ArrangeApperance();
//                int _overlaySizes = Convert.ToInt32(Math.Floor(_control.Size.Height * 0.28));
//                //_control.OverlayPanelTop.Size = new Size(_control.Size.Width, _overlaySizes - 30);
//                //_control.OverlayPanelBottom.Size = new Size(_control.Size.Width, _overlaySizes);

//                _control.ShowDialog();
//                _control.BringToFront();
//                _control.SetDefaultButton();

//                Action<WellsMetroMessageBoxControl> _delegate = new Action<WellsMetroMessageBoxControl>(ModalState);
//                IAsyncResult _asyncresult = _delegate.BeginInvoke(_control, null, _delegate);
//                bool _cancelled = false;

//                try
//                {
//                    while (!_asyncresult.IsCompleted)
//                    { Thread.Sleep(1); Application.DoEvents(); }
//                }
//                catch 
//                {
//                    _cancelled = true;

//                    if (!_asyncresult.IsCompleted)
//                    {
//                        try { _asyncresult = null; }
//                        catch { }
//                    }

//                    _delegate = null;
//                }

//                if (!_cancelled)
//                {
//                    _result = _control.Result;
//                    //_owner.Controls.Remove(_control);
//                    _control.Dispose(); _control = null;
//                }

//            }
//            else
//            {
//                //Form _owner = (Form)owner;

//                //int _minWidth = 500;
//                //int _minHeight = 350;

//                //if (_owner.Size.Width < _minWidth ||
//                //    _owner.Size.Height < _minHeight)
//                //{
//                //    if (_owner.Size.Width < _minWidth && _owner.Size.Height < _minHeight) {
//                //            _owner.Size = new Size(_minWidth, _minHeight);
//                //    }
//                //    else
//                //    {
//                //        if (_owner.Size.Width < _minWidth) _owner.Size = new Size(_minWidth, _owner.Size.Height);
//                //        else _owner.Size = new Size(_owner.Size.Width, _minHeight);
//                //    }

//                //    int x = Convert.ToInt32(Math.Ceiling((decimal)(Screen.PrimaryScreen.WorkingArea.Size.Width / 2) - (_owner.Size.Width / 2)));
//                //    int y = Convert.ToInt32(Math.Ceiling((decimal)(Screen.PrimaryScreen.WorkingArea.Size.Height / 2) - (_owner.Size.Height / 2)));
//                //    _owner.Location = new Point(x, y);
//                //}

//                switch (icon)
//                {
//                    case MessageBoxIcon.Error:
//                        SystemSounds.Hand.Play(); break;
//                    case MessageBoxIcon.Exclamation:
//                        SystemSounds.Exclamation.Play(); break;
//                    case MessageBoxIcon.Question:
//                        SystemSounds.Beep.Play(); break;
//                    default:
//                        SystemSounds.Asterisk.Play(); break;
//                }

//                WellsMetroMessageBoxControl _control = new WellsMetroMessageBoxControl();
//                _control.BackColor = Color.Aqua;
//                _control.Properties.Buttons = buttons;
//                _control.Properties.DefaultButton = defaultbutton;
//                _control.Properties.Icon = icon;
//                _control.Properties.Message = message;
//                _control.Properties.Title = title;
//                _control.Padding = new Padding(0, 0, 0, 0);
//                _control.ControlBox = false;
//                _control.ShowInTaskbar = false;



//                //_owner.Controls.Add(_control);
//                //if (_owner is IMetroForm)
//                //{
//                //    //if (((MetroForm)_owner).DisplayHeader)
//                //    //{
//                //    //    _offset += 30;
//                //    //}
//                //    _control.Theme = ((MetroForm)_owner).Theme;
//                //    _control.Style = ((MetroForm)_owner).Style;
//                //}
//                int ScreenWidth = Screen.PrimaryScreen.WorkingArea.Size.Width;
//                int ScreenHeight = Screen.PrimaryScreen.WorkingArea.Size.Height;
//                _control.Size = new Size(ScreenWidth/3, ScreenHeight/3);
//                int x = Convert.ToInt32(Math.Ceiling((decimal)(Screen.PrimaryScreen.WorkingArea.Size.Width / 2) - (_control.Size.Width / 2)));
//                int y = Convert.ToInt32(Math.Ceiling((decimal)(Screen.PrimaryScreen.WorkingArea.Size.Height / 2) - (_control.Size.Height / 2)));
//                _control.Location = new Point(x,y);
//                _control.ArrangeApperance();
//                int _overlaySizes = Convert.ToInt32(Math.Floor(_control.Size.Height * 0.28));
//                //_control.OverlayPanelTop.Size = new Size(_control.Size.Width, _overlaySizes - 30);
//                //_control.OverlayPanelBottom.Size = new Size(_control.Size.Width, _overlaySizes);

//                _control.ShowDialog();
//                _control.BringToFront();
//                _control.SetDefaultButton();

//                Action<WellsMetroMessageBoxControl> _delegate = new Action<WellsMetroMessageBoxControl>(ModalState);
//                IAsyncResult _asyncresult = _delegate.BeginInvoke(_control, null, _delegate);
//                bool _cancelled = false;

//                try
//                {
//                    while (!_asyncresult.IsCompleted)
//                    { Thread.Sleep(1); Application.DoEvents(); }
//                }
//                catch
//                {
//                    _cancelled = true;

//                    if (!_asyncresult.IsCompleted)
//                    {
//                        try { _asyncresult = null; }
//                        catch { }
//                    }

//                    _delegate = null;
//                }

//                if (!_cancelled)
//                {
//                    _result = _control.Result;
//                    //_owner.Controls.Remove(_control);
//                    _control.Dispose(); _control = null;
//                }
//            }

//            return _result;
//        }

//        private static void ModalState(WellsMetroMessageBoxControl control)
//        {
//            while (control.Visible)
//            { }
//        }

//    }
//}
