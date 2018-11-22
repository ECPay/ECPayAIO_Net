using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 報關方式。
    /// </summary>
    public enum CustomsClearance
    {
        /// <summary>
        /// 無(當 InvoiceMark=No 或 TaxType=ZeroTaxRate 時，為該值)。
        /// </summary>
        None = 0,
        /// <summary>
        /// 經海關出口(當 InvoiceMark=Yes 而且 TaxType=ZeroTaxRate 時，可設定該值)。
        /// </summary>
        CustomsExport = 1,
        /// <summary>
        /// 非經海關出口(當 InvoiceMark=Yes 而且 TaxType=ZeroTaxRate 時，可設定該值)。
        /// </summary>
        ECACustomsExport = 2
    }
}
