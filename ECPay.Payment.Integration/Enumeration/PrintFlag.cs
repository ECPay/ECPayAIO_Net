using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 是否列印。
    /// </summary>
    public enum PrintFlag
    {
        /// <summary>
        /// 無(當 InvoiceMark=No 時，為該值)。
        /// </summary>
        None = 9,
        /// <summary>
        /// 要列印。
        /// </summary>
        Yes = 1,
        /// <summary>
        /// 不列印。
        /// </summary>
        No = 0
    }
}
