using Wells.WellsFramework.Controls;
using Wells.WellsFramework.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Wells.WellsFramework.Forms;

namespace Wells.WellsFramework
{
    public partial class WellsMetroMessageBoxControl : Form
    {
        public WellsMetroMessageBoxControl()
        {
            InitializeComponent();

            _properties = new WellsMetroMessageBoxProperties(this);

            StylizeButton(metroButton1);
            StylizeButton(metroButton2);
            StylizeButton(metroButton3);

            metroButton1.Click += new EventHandler(button_Click);
            metroButton2.Click += new EventHandler(button_Click);
            metroButton3.Click += new EventHandler(button_Click);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Color _defaultColor = Color.FromArgb(57, 179, 215);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Color _errorColor = Color.FromArgb(210, 50, 45);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Color _warningColor = Color.FromArgb(237, 156, 40);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Color _success = Color.FromArgb(0, 170, 173);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Color _question = Color.FromArgb(71, 164, 71);


        /// <summary>
        /// Gets the top body section of the control. 
        /// </summary>
        public Panel Body
        {
            get { return panelbody; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private WellsMetroMessageBoxProperties _properties = null;

        /// <summary>
        /// Gets the message box display properties.
        /// </summary>
        public WellsMetroMessageBoxProperties Properties
        { get { return _properties; } }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DialogResult _result = DialogResult.None;

        /// <summary>
        /// Gets the dialog result that the user have chosen.
        /// </summary>
        public DialogResult Result
        {
            get { return _result; }
        }

        /// <summary>
        /// Arranges the apperance of the message box overlay.
        /// </summary>
        public void ArrangeApperance()
        {
            titleLabel.Text = _properties.Title;
            messageLabel.Text = "\n" + _properties.Message;

            switch (_properties.Icon)
            {
                case MessageBoxIcon.Exclamation:
                    panelbody.BackColor = _warningColor;
                    break;
                case MessageBoxIcon.Error:
                    panelbody.BackColor = _errorColor;
                    break;
                default: break;
            }

            switch (_properties.Buttons)
            {
                case MessageBoxButtons.OK:
                    EnableButton(metroButton1);

                    metroButton1.Text = "&" + clsWellsLanguage.getString(2);
                    metroButton1.Location = metroButton3.Location;
                
                    metroButton1.Tag = DialogResult.OK;

                    EnableButton(metroButton2, false);
                    EnableButton(metroButton3, false);
                    break;
                case MessageBoxButtons.OKCancel:
                    EnableButton(metroButton1);

                    metroButton1.Text = "&" + clsWellsLanguage.getString(2);
                    metroButton1.Location = metroButton2.Location;
                    metroButton1.Tag = DialogResult.OK;

                    //metroButton1.FlatStyle = FlatStyle.Flat;
                    //metroButton1.FlatAppearance.BorderColor = Color.White;
                    //metroButton1.ForeColor = Color.White;
                    //metroButton1.BackColor = _errorColor;

                    EnableButton(metroButton2);

                    metroButton2.Text = "&" + clsWellsLanguage.getString(3);
                    metroButton2.Location = metroButton3.Location;
                    metroButton2.Tag = DialogResult.Cancel;

                    EnableButton(metroButton3, false);
                    break;
                case MessageBoxButtons.RetryCancel:
                    EnableButton(metroButton1);

                    metroButton1.Text = "&" + clsWellsLanguage.getString(4);
                    metroButton1.Location = metroButton2.Location;
                    metroButton1.Tag = DialogResult.Retry;

                    EnableButton(metroButton2);

                    metroButton2.Text = "&" + clsWellsLanguage.getString(3);
                    metroButton2.Location = metroButton3.Location;
                    metroButton2.Tag = DialogResult.Cancel;

                    EnableButton(metroButton3, false);
                    break;
                case MessageBoxButtons.YesNo:
                    EnableButton(metroButton1);

                    metroButton1.Text = "&" + clsWellsLanguage.getString(5);
                    metroButton1.Location = metroButton2.Location;
                    metroButton1.Tag = DialogResult.Yes;

                    EnableButton(metroButton2);

                    metroButton2.Text = "&" + clsWellsLanguage.getString(7);
                    metroButton2.Location = metroButton3.Location;
                    metroButton2.Tag = DialogResult.No;

                    EnableButton(metroButton3, false);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    EnableButton(metroButton1);

                    metroButton1.Text = "&" + clsWellsLanguage.getString(5);
                    metroButton1.Tag = DialogResult.Yes;

                    EnableButton(metroButton2);

                    metroButton2.Text = "&" + clsWellsLanguage.getString(7);
                    metroButton2.Tag = DialogResult.No;

                    EnableButton(metroButton3);

                    metroButton3.Text = "&" + clsWellsLanguage.getString(3);
                    metroButton3.Tag = DialogResult.Cancel;

                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    EnableButton(metroButton1);

                    metroButton1.Text = "&" + clsWellsLanguage.getString(8);
                    metroButton1.Tag = DialogResult.Abort;

                    EnableButton(metroButton2);

                    metroButton2.Text = "&" + clsWellsLanguage.getString(4);
                    metroButton2.Tag = DialogResult.Retry;

                    EnableButton(metroButton3);

                    metroButton3.Text = "&"+clsWellsLanguage.getString(9);
                    metroButton3.Tag = DialogResult.Ignore;

                    break;
                default : break;
            }

            switch (_properties.Icon)
            {
                case  MessageBoxIcon.Error:
                    panelbody.BackColor = _errorColor; break;
                case MessageBoxIcon.Warning:
                    panelbody.BackColor = _warningColor; break;
                case MessageBoxIcon.Information:
                    panelbody.BackColor = _defaultColor;                    
                     break;
                case MessageBoxIcon.Question:
                    panelbody.BackColor = _question; break;
                default:
                    panelbody.BackColor = _success; break;
            }
        }

        private void EnableButton(Button button)
        { EnableButton(button, true); }

        private void EnableButton(Button button, bool enabled)
        {
            button.Enabled = enabled; button.Visible = enabled;
        }

        /// <summary>
        /// Sets the default focused button.
        /// </summary>
        public void SetDefaultButton()
        {
            switch (_properties.DefaultButton)
            {
                case MessageBoxDefaultButton.Button1:
                    if (metroButton1 != null)
                    {
                        if (metroButton1.Enabled) metroButton1.Focus();
                    }
                    break;
                case MessageBoxDefaultButton.Button2:
                    if (metroButton2 != null)
                    {
                        if (metroButton2.Enabled) metroButton2.Focus();
                    }
                    break;
                case MessageBoxDefaultButton.Button3:
                    if (metroButton3 != null)
                    {
                        if (metroButton3.Enabled) metroButton3.Focus();
                    }
                    break;  
                default: break;
            }
        }

        private void button_MouseClick(object sender, MouseEventArgs e)
        {
            //Button button = (Button)sender;
            //button.BackColor = WellsMetroPaint.BackColor.Button.Press(Wells.WellsFramework.WellsMetroThemeStyle.Light);
            //button.FlatAppearance.BorderColor = WellsMetroPaint.BorderColor.Button.Press(Wells.WellsFramework.WellsMetroThemeStyle.Light);
            //button.ForeColor = WellsMetroPaint.ForeColor.Button.Press(Wells.WellsFramework.WellsMetroThemeStyle.Light);
        }

        private void button_MouseEnter(object sender, EventArgs e)
        { StylizeButton((Button)sender, true); }

        private void button_MouseLeave(object sender, EventArgs e)
        { StylizeButton((Button)sender); }

        private void StylizeButton(Button button)
        { StylizeButton(button, false); }

        private void StylizeButton(Button button, bool hovered)
        {
            button.Cursor = Cursors.Hand;

            button.MouseClick -= button_MouseClick;
            button.MouseClick += button_MouseClick;
            
            button.MouseEnter -= button_MouseEnter;
            button.MouseEnter += button_MouseEnter;

            button.MouseLeave -= button_MouseLeave;
            button.MouseLeave += button_MouseLeave;

            //if (hovered)
            //{
            //    button.FlatAppearance.BorderColor = WellsMetroPaint.BorderColor.Button.Hover(Wells.WellsFramework.WellsMetroThemeStyle.Light);
            //    button.ForeColor = WellsMetroPaint.ForeColor.Button.Hover(Wells.WellsFramework.WellsMetroThemeStyle.Light);
            //}
            //else
            //{
            //    button.BackColor = WellsMetroPaint.BackColor.Button.Normal(Wells.WellsFramework.WellsMetroThemeStyle.Light);
            //    button.FlatAppearance.BorderColor = Color.SlateGray;
            //    //button.FlatAppearance.BorderColor = Color.Red;
            //    button.FlatAppearance.MouseOverBackColor = WellsMetroPaint.BorderColor.Button.Hover(Wells.WellsFramework.WellsMetroThemeStyle.Light);
            //    button.ForeColor = WellsMetroPaint.ForeColor.Button.Normal(Wells.WellsFramework.WellsMetroThemeStyle.Light);
            //    button.FlatAppearance.BorderSize = 1;
            //}
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (!button.Enabled) return;
            _result = (DialogResult)button.Tag;
            Hide(); 
        }

        private void WellsMetroMessageBoxControl_Load(object sender, EventArgs e)
        {
            TopMost = false;
            TopLevel = true;
            BringToFront();
            TopMost = true;
        }
    }
}
