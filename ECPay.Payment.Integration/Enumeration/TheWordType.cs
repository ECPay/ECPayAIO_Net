using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 字軌類型。
    /// </summary>
    public enum TheWordType
    {
        /// <summary>
        /// 無(當 InvoiceMark=No 時，為該值)。
        /// </summary>
        None = 0,
        /// <summary>
        /// 一般稅額。
        /// </summary>
        General = 7,
        /// <summary>
        /// 特種稅額。
        /// </summary>
        Special = 8

    }
}
