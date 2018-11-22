using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 發票捐贈類型。
    /// </summary>
    public enum DonatedInvoice
    {
        /// <summary>
        /// 無(當 InvoiceMark=No 時，為該值)。
        /// </summary>
        None = 0,
        /// <summary>
        /// 捐贈發票。
        /// </summary>
        Yes = 1,
        /// <summary>
        /// 不捐贈發票。
        /// </summary>
        No = 2
    }
}
