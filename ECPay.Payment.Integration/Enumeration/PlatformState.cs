using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 訂單類型。
    /// </summary>
    public enum PlatformState
    {
        /// <summary>
        /// 全部。
        /// </summary>
        ALL = 0,
        /// <summary>
        /// 一般。
        /// </summary>
        General = 1,
        /// <summary>
        /// 平台商。
        /// </summary>
        Platform = 2
    }
}
