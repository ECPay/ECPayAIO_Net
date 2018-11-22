using System;
using System.Collections.Generic;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 定期定額的週期種類。
    /// </summary>
    public enum PeriodType
    {
        /// <summary>
        /// 無。
        /// </summary>
        None = 0,
        /// <summary>
        /// 年。
        /// </summary>
        Year = 1,
        /// <summary>
        /// 月。
        /// </summary>
        Month = 2,
        /// <summary>
        /// 日。
        /// </summary>
        Day = 3
    }
}
