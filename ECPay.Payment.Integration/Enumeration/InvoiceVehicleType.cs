using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 電子發票載具類型。
    /// </summary>
    public enum InvoiceVehicleType
    {
        /// <summary>
        /// 無。
        /// </summary>
        None = 0,
        /// <summary>
        /// 綠界科技會員。
        /// </summary>
        Member = 1,
        /// <summary>
        /// 自然人憑證。
        /// </summary>
        NaturalPersonEvidence = 2,
        /// <summary>
        /// 手機條碼。
        /// </summary>
        PhoneBarcode = 3
    }
}
