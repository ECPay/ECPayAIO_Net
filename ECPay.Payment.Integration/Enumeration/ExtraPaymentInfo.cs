using System;
using System.Collections.Generic;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 額外付款資訊。
    /// </summary>
    public enum ExtraPaymentInfo
    {
        /// <summary>
        /// 需要額外付款資訊。
        /// </summary>
        Yes = 1,
        /// <summary>
        /// 不需要額外付款資訊。
        /// </summary>
        No = 0
    }
}
