using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Wells.Tools
{
    /// <summary>
    /// 窗口渐隐显示和退出
    /// </summary>
    public class clsFormShowFadeout
    {
        Timer _timer = new Timer();
        Form _form = new Form();
        double dou = 0;

        public void getShow(Form form)
        {
            dou = 0.1;
            _form = form;
            _form.Opacity = 0;
            _form.Show();
            _timer.Start();
         _timer.Tick +=new EventHandler(_timer_Tick);
        }

        private void _timer_Tick(object sender,EventArgs e)
        {
            _form.Opacity += dou;
            if(_form.Opacity==1)
            {
                _timer.Stop();
                _timer.Dispose();
            }
            else if (_form.Opacity == 0)
            {
                _timer.Stop();
                _timer.Dispose();
                _form.Close();
            }
        }

        public void getClose(Form form)
        {
            dou = -0.1;
            _form = form;
            _timer.Start();
            _timer.Tick +=new EventHandler(_timer_Tick);
        }
    }
}
