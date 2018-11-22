using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 電子發票開立註記。
    /// </summary>
    public enum InvoiceState
    {
        /// <summary>
        /// 需要開立電子發票。
        /// </summary>
        Yes = 1,
        /// <summary>
        /// 不需要開立電子發票。
        /// </summary>
        No = 0
    }
}
