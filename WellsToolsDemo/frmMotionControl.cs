using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace dhVision
{
    public partial class frmMotionControl : CCWin.Skin_Color
    {
        public frmMotionControl()
        {
            InitializeComponent();

            loadAlarm();
        }


        /// <summary>
        /// 程序要写入plc的值，从而控制各种动作
        /// 0-拍照位置X 1-拍照位置Y 2-【=1请求移动到拍照位置进行拍照 =2 纯移动】 3-收到拍摄全图指令 4-设备启动 5-设备停止 6-急停 7-解除警报 8-伺服ON 9-轨道原点回归 
        /// 10-镭射头原点回归 11-光源开 12-光源关 13-手动进料 14-手动出料 15-轨道气缸上升 16-轨道气缸下降 17-轨道宽度缩小 18-轨道宽度放大  19-倒板，重新夹板
        /// 20-设置速度  21-设置轨道宽度 22-不移动，相机拍照 23-移至拍摄原点 24-手动执行镭雕主程序 25-轨道宽度点动减小  26-轨道宽度点动增大 27-打标开始flag 28-打标结束flag
        /// 29-报警，指示灯和蜂鸣器信号 30-读取轨道宽度 31-轨道宽度[读取回来的值] 32-左挡板上升 33-左挡板下降 34-右挡板上升 35-右挡板下降 
        /// 36-解除安全门 37-安全门使能 38-发送相机收到图像信号到PLC
        /// </summary>
        public static int[] iPlc_status_write = new int[60];


        private string[] strPlcParams = new string[] { };
        public void initilizeParams(string[] strMparams)
        {
            #region  *** 初始化  ***
            try
            {
                strPlcParams = new string[strMparams.Length];
                Array.Copy(strMparams, strPlcParams, strMparams.Length);

                List<string> sListNew = new List<string>(strPlcParams);
                while (sListNew.Count < 38)
                {
                    sListNew.Add("1");
                }
                strPlcParams = sListNew.ToArray();

                skinCb_motion_Plctype.SelectedIndex = int.Parse(strPlcParams[0]);
                skinCb_motion_substation.Text = strPlcParams[1];
                skinCb_motion_comm.SelectedIndex = int.Parse(strPlcParams[2]);
                skinCb_motion_cpu.SelectedIndex = int.Parse(strPlcParams[3]);
                skinTxtB_motion_IP.Text = strPlcParams[4];

                skinNumUD_pcbWidth.Value = decimal.Parse(strPlcParams[5]);
                skinNumUD_pcbHeight.Value = decimal.Parse(strPlcParams[6]);
                skinNumUD_scale.Value = decimal.Parse(strPlcParams[7]);

                if (strPlcParams.Length > 16) skinNumUD_speed.Value = decimal.Parse(strPlcParams[16]);
                if (strPlcParams.Length > 17) skinNumUD_TrailWidth.Value = decimal.Parse(strPlcParams[17]);
                if (strPlcParams.Length > 18) skinNumUD_Grab0PLCX.Value = decimal.Parse(strPlcParams[18]);
                if (strPlcParams.Length > 19) skinNumUD_Grab0PLCY.Value = decimal.Parse(strPlcParams[19]);
                if (strPlcParams.Length > 20) skinChkb_PLC_OrientationX.Checked = strPlcParams[20] == "1" ? true : false;
                if (strPlcParams.Length > 21) skinChkb_PLC_OrientationY.Checked = strPlcParams[21] == "1" ? true : false;
                if (strPlcParams.Length > 22) skinNumUD_LaserPositionX.Value = decimal.Parse(strPlcParams[22]);
                if (strPlcParams.Length > 23) skinNumUD_LaserPositionY.Value = decimal.Parse(strPlcParams[23]);
            }
            catch (Exception exc)
            {
                dhDll.frmMsg.Log("初始化运动控制界面参数出错:" + exc.Message, 1, 2, 2);
            }
            #endregion
        }

        private void frmMotionControl_Load(object sender, EventArgs e)
        {
            skinCb__Width_dot.SelectedIndex = 0;
            timerAuthority.Enabled = true;
        }


        private void frmMotionControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }



        private void skinBbtn_CaptureAllPcb_Click(object sender, EventArgs e)
        {
            #region  *** 采集全图  ***

            if (iError_Laser_Old[0] != 0)
            {
                MessageBox.Show("请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (iPlc_status_write[3] == 0)
            {
                Parameter.configPublic.listPubParams[16][5] = skinNumUD_pcbWidth.Value.ToString();
                Parameter.configPublic.listPubParams[16][6] = skinNumUD_pcbHeight.Value.ToString();
                Parameter.configPublic.listPubParams[16][7] = skinNumUD_scale.Value.ToString();

                iPlc_status_write[3] = 1;
            }
            else
            {
                MessageBox.Show("正在采集全图照片,请稍后!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            #endregion
        }

        private void skinBbtn_SaveParams_Click(object sender, EventArgs e)
        {
            #region  ****** 参数保存 ******

            if (Parameter.configPublic.listPubParams[16].Length < 26)
            {
                List<string> ltemple = new List<string>(Parameter.configPublic.listPubParams[16]);
                while (ltemple.Count < 36) ltemple.Add("0");
                Parameter.configPublic.listPubParams[16] = ltemple.ToArray();
            }
            Parameter.configPublic.listPubParams[16][0] = skinCb_motion_Plctype.SelectedIndex.ToString();
            Parameter.configPublic.listPubParams[16][1] = skinCb_motion_substation.Text;
            Parameter.configPublic.listPubParams[16][2] = skinCb_motion_comm.SelectedIndex.ToString();
            Parameter.configPublic.listPubParams[16][3] = skinCb_motion_cpu.SelectedIndex.ToString();
            Parameter.configPublic.listPubParams[16][4] = skinTxtB_motion_IP.Text;

            Parameter.configPublic.listPubParams[16][5]=skinNumUD_pcbWidth.Value.ToString();
            Parameter.configPublic.listPubParams[16][6] = skinNumUD_pcbHeight.Value.ToString();
            Parameter.configPublic.listPubParams[16][7] = skinNumUD_scale.Value.ToString();

            Parameter.configPublic.listPubParams[16][16] = skinNumUD_speed.Value.ToString();
            Parameter.configPublic.listPubParams[16][17] = skinNumUD_TrailWidth.Value.ToString();
            Parameter.configPublic.listPubParams[16][18] = skinNumUD_Grab0PLCX.Value.ToString();
            Parameter.configPublic.listPubParams[16][19] = skinNumUD_Grab0PLCY.Value.ToString();
            Parameter.configPublic.listPubParams[16][20] = skinChkb_PLC_OrientationX.Checked ? "1" : "0";
            Parameter.configPublic.listPubParams[16][21] = skinChkb_PLC_OrientationY.Checked ? "1" : "0";
            Parameter.configPublic.listPubParams[16][22] = skinNumUD_LaserPositionX.Value.ToString();
            Parameter.configPublic.listPubParams[16][23] = skinNumUD_LaserPositionY.Value.ToString();

            Parameter.ConfigSave(999, -1);

            MessageBox.Show("保存参数成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            #endregion
        }


        /// <summary>
        /// 对应Alarm.txt
        /// </summary>
        List<string> strAlarm = new List<string>();
        /// <summary>
        /// 对应Alarm1.txt
        /// </summary>
        Dictionary<int, string> dicAlarm1 = new Dictionary<int, string>();
        /// <summary>
        /// 对应Alarm2.txt
        /// </summary>
        Dictionary<int, string> dicAlarm2 = new Dictionary<int, string>();

        private void loadAlarm()
        {
            #region  *** 加载alarm 文件  报警代码  ***

            string strFile = Application.StartupPath + "\\Alarm\\Alarm.txt";
            if (System.IO.File.Exists(strFile))
            {
                #region  *** Alarm.txt文件存在  ***

                FileStream fs = new FileStream(strFile, FileMode.Open, FileAccess.Read);
                StreamReader sReader = new StreamReader(fs, Encoding.Default);
                string strReader;
                while (sReader.Peek() != -1)
                {
                    strReader = sReader.ReadLine();
                    strAlarm.Add(strReader);
                }
                sReader.Close();

                #endregion
            }

            strFile = Application.StartupPath + "\\Alarm\\Alarm1.txt";
            if (System.IO.File.Exists(strFile))
            {
                #region  *** Alarm1.txt文件存在  ***

                FileStream fs = new FileStream(strFile, FileMode.Open, FileAccess.Read);
                StreamReader sReader = new StreamReader(fs, Encoding.Default);
                string strReader;
                while (sReader.Peek() != -1)
                {
                    strReader = sReader.ReadLine();

                    string[] strDic = strReader.Split('=');
                    int ieIndex = -1;
                    int.TryParse(strDic[0], out ieIndex);

                    if (!dicAlarm1.ContainsKey(ieIndex))
                    {
                        dicAlarm1.Add(ieIndex, strDic[1]);
                    }
                }
                sReader.Close();

                #endregion
            }

            strFile = Application.StartupPath + "\\Alarm\\Alarm2.txt";
            if (System.IO.File.Exists(strFile))
            {
                #region  *** Alarm2.txt文件存在  ***

                FileStream fs = new FileStream(strFile, FileMode.Open, FileAccess.Read);
                StreamReader sReader = new StreamReader(fs, Encoding.Default);
                string strReader;
                while (sReader.Peek() != -1)
                {
                    strReader = sReader.ReadLine();

                    string[] strDic = strReader.Split('=');
                    int ieIndex = -1;
                    int.TryParse(strDic[0], out ieIndex);

                    if (!dicAlarm2.ContainsKey(ieIndex))
                    {
                        dicAlarm2.Add(ieIndex, strDic[1]);
                    }
                }
                sReader.Close();

                #endregion
            }


            #endregion
        }

        /// <summary>
        ///每次更新错误之前 保存的临时错误代码： 镭雕故障代码 =0表示正常 =其他表示故障
        /// </summary>
        private int[] iError_Laser_Old = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1 };

        private string[] strErrors_Old = new string[] { " ", " ", " ", " ", " ", " ", " ", " ", " " };

        System.Threading.ThreadStart processZS = null;
        System.Threading.Thread threadProcess = null;
        public void handleError(int[] iErrorNumber, bool bAutoRun)
        {
            if (threadProcess == null || !threadProcess.IsAlive)
            {
                processZS = delegate { handleErrorThread(iErrorNumber, bAutoRun); };
                threadProcess = new System.Threading.Thread(processZS);
                threadProcess.Start();
            }
        }


        int iupdateTimes = 0;
        /// <summary>
        /// 0-代表正常 1 2 3 等代表错误代码
        /// </summary>
        /// <param name="iErrorNumber"></param>
        public void handleErrorThread(int[] iErrorNumber, bool bAutoRun)
        {
            #region  *** 显示错误信息  每50ms刷新一次  ***

            try
            {
                if (bAutoRun)
                {
                    if (lblAutoRun.Text != "自动模式") lblAutoRun.Text = "自动模式";
                }
                else
                {
                    if (lblAutoRun.Text != "手动模式") lblAutoRun.Text = "手动模式";
                }

                //计算是否有错误
                bool bhaveError = false;
                for (int igg = 0; igg < iErrorNumber.Length; igg++)
                {
                    if (iErrorNumber[igg] != 0)
                    {
                        bhaveError = true;
                        break;
                    }
                }

                //是否更新界面
                bool bupdateUI = false;

                string[] strErrors = new string[iErrorNumber.Length];
                if (bhaveError)
                {
                    for (int igg = 0; igg < iErrorNumber.Length; igg++)
                    {
                        if (iError_Laser_Old[igg] == iErrorNumber[igg])
                        {
                            strErrors[igg] = strErrors_Old[igg];
                        }
                        else
                        {
                            if (igg == 0)
                            {
                                string valueString = Convert.ToString(iErrorNumber[igg], 2);
                                if (valueString.Length > 16) valueString = valueString.Substring(valueString.Length - 16, 16);
                                if (valueString.Length < 16) valueString = valueString.PadLeft(16, '0');
                                char[] chars = valueString.ToCharArray();
                                Array.Reverse(chars);
                                string strStatus = new string(chars);

                                for (int ihh = 0; ihh < strStatus.Length; ihh++)
                                {
                                    if (strStatus[ihh] == '1')
                                    {
                                        string strtemp = "设备异常:" + ihh.ToString();
                                        if (strAlarm.Count > ihh) strtemp = strAlarm[ihh];

                                        strErrors[igg] += strtemp + " ";
                                    }
                                }
                            }
                            else if (igg == 1)
                            {
                                int ieIndex = iErrorNumber[igg];
                                if (dicAlarm1.ContainsKey(ieIndex))
                                {
                                    strErrors[igg] = dicAlarm1[ieIndex];
                                }
                                else
                                {
                                    strErrors[igg] = "未知错误" + ieIndex.ToString();
                                }
                            }
                            else if (igg == 2)
                            {
                                int ieIndex = iErrorNumber[igg];
                                if (dicAlarm2.ContainsKey(ieIndex))
                                {
                                    strErrors[igg] = dicAlarm2[ieIndex];
                                }
                                else
                                {
                                    strErrors[igg] = "未知错误" + ieIndex.ToString();
                                }
                            }


                        }
                    }

                    //如果当前错误内容和之前不一样
                    for (int igg = 0; igg < strErrors.Length; igg++)
                    {
                        if (strErrors[igg] != strErrors_Old[igg])
                        {
                            bupdateUI = true;
                            break;
                        }
                    }
                    if (bupdateUI)
                    {
                        if (lblError.ForeColor != Color.Red) lblError.ForeColor = Color.Red;
                        lblError.Text = Parameter.transFromStringArray2String(strErrors, " ");
                        iupdateTimes++; lblCounts.Text = iupdateTimes.ToString();
                    }
                }
                else
                {
                    #region  *** 如果没有错误  ***

                    //如果old错误 不为0 表示之前有错误 需要刷新界面
                    for (int igg = 0; igg < iError_Laser_Old.Length; igg++)
                    {
                        if (iError_Laser_Old[igg] != 0)
                        {
                            bupdateUI = true;
                            break;
                        }
                    }
                    if (bupdateUI)
                    {
                        lblError.ForeColor = Color.White;
                        lblError.Text = "设备运转正常";
                        iupdateTimes++; lblCounts.Text = iupdateTimes.ToString();
                    }

                    #endregion
                }

                iError_Laser_Old = new int[iErrorNumber.Length];
                Array.Copy(iErrorNumber, iError_Laser_Old, iErrorNumber.Length);
                strErrors_Old = new string[strErrors.Length];
                Array.Copy(strErrors, strErrors_Old, strErrors.Length);
            }
            catch (Exception exc)
            {
                dhDll.frmMsg.Log("运动控制界面显示错误信息出错:" + exc.Message, 0, 2, 0);
            }
            #endregion
        }

        private void skinBtn_alarmClear_Click(object sender, EventArgs e)
        {
            iPlc_status_write[7] = 1;
        }

        private void skinBtn_Start_Click(object sender, EventArgs e)
        {
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //iPlc_status_write[4] = 1;
        }

        private void skinBtn_Stop_Click(object sender, EventArgs e)
        {
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

           // iPlc_status_write[5] = 1;
        }

        private void skinBtn_Emergency_Click(object sender, EventArgs e)
        {
            iPlc_status_write[6] = 1;

        }

        private void skinBtn_ServoOn_Click(object sender, EventArgs e)
        {
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //iPlc_status_write[8] = 1;
        }

        private void skinBtn_TrailZero_Click(object sender, EventArgs e)
        {
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[9] = 1;
        }

        private void skinBtn_LaserZero_Click(object sender, EventArgs e)
        {
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[10] = 1;
        }

        private void skinBtn_Lighton_Click(object sender, EventArgs e)
        {
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[11] = 1;
        }

        private void skinBtn_Lighoff_Click(object sender, EventArgs e)
        {
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[12] = 1;
        }

        private void skinBtn_ObjectIn_Click(object sender, EventArgs e)
        {
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[13] = 1;
        }

        private void skinBtn_ObjectOut_Click(object sender, EventArgs e)
        {
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[14] = 1;
        }

        private void skinBtn_CylinderUp_Click(object sender, EventArgs e)
        {
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[15] = 1;
        }

        private void skinBtn_CylinderDown_Click(object sender, EventArgs e)
        {
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[16] = 1;
        }

        private void skinBtn_LoadBoard_Click(object sender, EventArgs e)
        {
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[19] = 1;
        }

        private void skinBtn_SetSpeed_Click(object sender, EventArgs e)
        {
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[20] = (int)skinNumUD_speed.Value * 10;

            //iPlc_status_write[0] = 300000; iPlc_status_write[1] = 300000;
            //iPlc_status_write[2] = 1;
        }


        private void skinBtn_triggercamera_Click(object sender, EventArgs e)
        {
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[22] = 1;
        }

        private void frmMotionControl_Activated(object sender, EventArgs e)
        {
            this.Invalidate();
            //dhDll.frmMsg.Log("111", 0, 1, 0);
        }

        private void skinBbtn_ExeLaserProcess_Click(object sender, EventArgs e)
        {
            //手动执行打标
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[24] = 1;
        }

        private void skinBbtn_move2Original_Click(object sender, EventArgs e)
        {
            //移动到拍摄原点，设备左下角
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[23] = 1;
        }


        private void skinBtn_WidthSmall_Click(object sender, EventArgs e)
        {
            //连续 轨道宽度减小
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (iPlc_status_write[17] == 0) iPlc_status_write[17] = 1;
            else iPlc_status_write[17] = 18;
        }

        private void skinBtn_WIdthBig_Click(object sender, EventArgs e)
        {
            //连续 轨道宽度增大
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (iPlc_status_write[18] == 0) iPlc_status_write[18] = 1;
            else iPlc_status_write[18] = 18;
        }

        private void skinBtn_WidthSmall_dot_Click(object sender, EventArgs e)
        {
            //点动 轨道宽度减小
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int iwiddth = skinCb__Width_dot.SelectedIndex == 0 ? 25 : 125 * skinCb__Width_dot.SelectedIndex;

            iPlc_status_write[25] = iwiddth;
        }

        private void skinBtn_WIdthBig_dot_Click(object sender, EventArgs e)
        {
            //点动 轨道宽度增大
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int iwiddth = skinCb__Width_dot.SelectedIndex == 0 ? 25 : 125 * skinCb__Width_dot.SelectedIndex;
            iPlc_status_write[26] = iwiddth;
        }

        private void skinBtn_SetTrailWidth_Click(object sender, EventArgs e)
        {
            //设置轨道宽度
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[21] = (4160 - (int)(skinNumUD_TrailWidth.Value * 10)) * 25;
        }

        private void skinBtn_startflag_Click(object sender, EventArgs e)
        {
            //打标开始flag
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[27] = 1;
        }

        private void skinBtn_endflag_Click(object sender, EventArgs e)
        {
            //打标结束flag
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[28] = 1;
        }


        private void skinBtn_ReadTrailWidth_Click(object sender, EventArgs e)
        {
            //打标结束flag
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            iPlc_status_write[31] = 0;
            iPlc_status_write[30] = 1;
            int iretryCount = 0;
            while (iPlc_status_write[31] == 0)
            {
                iretryCount++;
                if (iretryCount > 300) break;
                dhDll.clsTimeDelay.Delay(15);
            }
            if (iPlc_status_write[31] == 0)
            {

            }
            else
            {
                skinNumUD_TrailWidth.Value = (decimal)iPlc_status_write[31] /100;
            }
      
            
        }

        private void skinBtn_DangbanLeftUp_Click(object sender, EventArgs e)
        {
            //左挡板上升
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            iPlc_status_write[32] = 1;
        }

        private void skinBtn_DangbanLeftDown_Click(object sender, EventArgs e)
        {
            //左挡板下降
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            iPlc_status_write[33] = 1;
        }

        private void skinBtn_DangbanRightUp_Click(object sender, EventArgs e)
        {
            //右挡板上升
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            iPlc_status_write[34] = 1;
        }

        private void skinBtn_DangbanRightDown_Click(object sender, EventArgs e)
        {
            //右挡板下降
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            iPlc_status_write[35] = 1;
        }

        private void skinBtn_SafedoorOff_Click(object sender, EventArgs e)
        {
            //解除安全门
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            iPlc_status_write[36] = 1;
        }

        private void skinBtn_SafedoorOn_Click(object sender, EventArgs e)
        {
            //安全门使能
            if (iError_Laser_Old[0] != 0)
            {
                MetroFramework.MetroMessageBox.Show(this, Environment.NewLine + "请先解除故障!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            iPlc_status_write[37] = 1;
        }

        private void timerAuthority_Tick(object sender, EventArgs e)
        {
            #region  *** 根据用户设定对应的权限  ***

            if (!this.Visible) return;

            if (classApp.strUserInfo != null && classApp.strAuthority != null)
            {
                string[] strAuthority = classApp.strAuthority;
                if (strAuthority.Length > 22 && strAuthority[22] == "1")//plc选择
                {
                    if (!skinGroupBox21.Enabled) skinGroupBox21.Enabled = true;
                }
                else
                {
                    if (skinGroupBox21.Enabled) skinGroupBox21.Enabled = false;
                }

                if (strAuthority.Length > 23 && strAuthority[23] == "1")//基板参数
                {
                    if (!skinGroupBox24.Enabled) skinGroupBox24.Enabled = true;
                }
                else
                {
                    if (skinGroupBox24.Enabled) skinGroupBox24.Enabled = false;
                }

                if (strAuthority.Length > 24 && strAuthority[24] == "1")//警报急停
                {
                    if (!skinGroupBox4.Enabled) skinGroupBox4.Enabled = true;
                }
                else
                {
                    if (skinGroupBox4.Enabled) skinGroupBox4.Enabled = false;
                }

                if (strAuthority.Length > 25 && strAuthority[25] == "1")//保存参数
                {
                    if (!skinGroupBox3.Enabled) skinGroupBox3.Enabled = true;
                    if (!skinGroupBox5.Enabled) skinGroupBox5.Enabled = true;
                }
                else
                {
                    if (skinGroupBox3.Enabled) skinGroupBox3.Enabled = false;
                    if (skinGroupBox5.Enabled) skinGroupBox5.Enabled = false;
                }

                if (strAuthority.Length > 26 && strAuthority[26] == "1")//控制逻辑
                {
                    if (!skinGroupBox1.Enabled) skinGroupBox1.Enabled = true;
                }
                else
                {
                    if (skinGroupBox1.Enabled) skinGroupBox1.Enabled = false;
                }

                if (strAuthority.Length > 27 && strAuthority[27] == "1")//控制动作
                {
                    if (!skinGroupBox2.Enabled) skinGroupBox2.Enabled = true;
                }
                else
                {
                    if (skinGroupBox2.Enabled) skinGroupBox2.Enabled = false;
                }

                if (strAuthority.Length > 28 && strAuthority[28] == "1")//轨道宽度
                {
                    if (!skinGroupBox6.Enabled) skinGroupBox6.Enabled = true;
                    if (!skinGroupBox7.Enabled) skinGroupBox7.Enabled = true;
                    if (!skinGroupBox8.Enabled) skinGroupBox8.Enabled = true;
                }
                else
                {
                    if (skinGroupBox6.Enabled) skinGroupBox6.Enabled = false;
                    if (skinGroupBox7.Enabled) skinGroupBox7.Enabled = false;
                    if (skinGroupBox8.Enabled) skinGroupBox8.Enabled = false;
                }
            }
            #endregion
        }


       





    }
}
