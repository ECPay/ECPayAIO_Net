using System;
using System.Collections.Generic;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 付款方式。
    /// </summary>
    public enum PaymentMethod
    {
        /// <summary>
        /// 不指定付款方式。
        /// </summary>
        ALL = 0,
        /// <summary>
        /// 信用卡付費。
        /// </summary>
        Credit = 1,
        /// <summary>
        /// 網路 ATM。
        /// </summary>
        WebATM = 2,
        /// <summary>
        /// 自動櫃員機。
        /// </summary>
        ATM = 3,
        /// <summary>
        /// 超商代碼。
        /// </summary>
        CVS = 4,
        /// <summary>
        /// 超商條碼。
        /// </summary>
        BARCODE = 5,
        /// <summary>
        /// 支付寶。
        /// </summary>
        Alipay = 6,
        /// <summary>
        /// 財付通。
        /// </summary>
        Tenpay = 7,
        /// <summary>
        /// 儲值消費。
        /// </summary>
        TopUpUsed = 8,
        ///// <summary>
        ///// 全家立即儲。
        ///// </summary>
        //APPBARCODE = 9,
        ///// <summary>
        ///// Account Link。
        ///// </summary>
        //AccountLink = 10,

        /// <summary>
        /// GooglePay
        /// </summary>
        GooglePay = 11
    }
}
