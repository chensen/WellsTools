using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Wells.Tools
{
    public class clsStopWatch
    {
        double m_dStartTime = 0.0;    ///< 开始时间
        double m_dStopTime = 0.0;     ///< 停止时间 


        public clsStopWatch()
        {
            start();
        }

        /// <summary>
        /// 开始计数
        /// </summary>
        public void start()
        {
            m_dStartTime = Stopwatch.GetTimestamp();
        }

        /// <summary>
        /// 停止计数
        /// </summary>
        /// <returns>时间差单位ms</returns>
        public double stop()
        {
            m_dStopTime = Stopwatch.GetTimestamp();
            double theElapsedTime = getElapsedTime();

            m_dStartTime = m_dStopTime;
            return theElapsedTime;
        }


        /// <summary>
        /// 获取时间差
        /// </summary>
        /// <returns>时间差单位ms</returns>
        public double getElapsedTime()
        {
            m_dStopTime = Stopwatch.GetTimestamp();
            double dTimeElapsed = (m_dStopTime - m_dStartTime) * 1000.0;

            return dTimeElapsed / Stopwatch.Frequency;
        }
    }
}
