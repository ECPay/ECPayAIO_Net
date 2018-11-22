using System;
using System.Collections.Generic;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 裝置類型。
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// 桌機版付費頁面。
        /// </summary>
        PC = 0,
        /// <summary>
        /// 行動裝置版付費頁面。
        /// </summary>
        Mobile = 1,
        /// <summary>
        /// 不給值, 由系統判斷
        /// </summary>
        None = 2
    }
}
