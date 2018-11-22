using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 課稅類型。
    /// </summary>
    public enum TaxationType
    {
        /// <summary>
        /// 無(當 InvoiceMark=No 時，為該值)。
        /// </summary>
        None = 0,
        /// <summary>
        /// 應稅(當 InvoiceMark=Yes 時，才可設定該值)。
        /// </summary>
        Taxable = 1,
        /// <summary>
        /// 零稅率(當 InvoiceMark=Yes 時，才可設定該值)。
        /// </summary>
        ZeroTaxRate = 2,
        /// <summary>
        /// 免稅(當 InvoiceMark=Yes 時，才可設定該值)。
        /// </summary>
        DutyFree = 3,
        /// <summary>
        /// 混合應稅(當 InvoiceMark=Yes 時，才可設定該值)。
        /// </summary>
        MixedTaxable = 9
    }
}
