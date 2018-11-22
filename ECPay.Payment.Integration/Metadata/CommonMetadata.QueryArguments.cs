using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 全功能介接參數的類別。
    /// </summary>
    public abstract partial class CommonMetadata
    {
        /// <summary>
        /// 介接訂單查詢的資料傳遞成員類別。
        /// </summary>
        public abstract class QueryArguments
        {
            /// <summary>
            /// 廠商交易編號。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string MerchantTradeNo { get; set; }
            /// <summary>
            /// 廠商驗證時間(自動產生)。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public int TimeStamp { get; private set; }
            /// <summary>
            /// 訂單查詢介接參數的建構式。
            /// </summary>
            public QueryArguments()
            {
                this.TimeStamp = Convert.ToInt32((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
            }
        }
    }
}
