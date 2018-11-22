using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 是否要延遲撥款。
    /// </summary>
    public enum HoldTradeType
    {
        /// <summary>
        /// 要延遲撥款。
        /// </summary>
        Yes = 1,
        /// <summary>
        /// 不要要延遲撥款。
        /// </summary>
        No = 0
    }
}
