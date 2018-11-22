using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ECPay.Payment.Integration.SPCheckOut
{
    /// <summary>
    /// SP_Token返回模組
    /// </summary>
    public class TradeSPToken
    {
        /// <summary>
        /// 返回代碼
        /// </summary>
        public string RtnCode { get; set; }

        /// <summary>
        /// 返回訊息
        /// </summary>
        public string RtnMsg { get; set; }

        /// <summary>
        /// 返回SPToken
        /// </summary>
        public string SPToken { get; set; }

        /// <summary>
        /// 廠商編號
        /// </summary>
        public string MerchantID { get; set; }

        /// <summary>
        /// 交易流水號
        /// </summary>
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// CheckMacValue檢查碼
        /// </summary>
        public string CheckMacValue { get; set; }
    }
}
