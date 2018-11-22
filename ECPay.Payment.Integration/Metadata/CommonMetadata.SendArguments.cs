using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Linq.Expressions;
using System.Reflection;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 共用的介接參數的類別。
    /// </summary>
    public abstract partial class CommonMetadata
    {
        /// <summary>
        /// 介接的基本資料傳遞成員類別。
        /// </summary>
        public abstract class SendArguments
        {
            internal string _ItemName = String.Empty;
            internal string _ItemURL = String.Empty;
            /// <summary>
            /// 廠商交易編號。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string MerchantTradeNo { get; set; }
            /// <summary>
            /// 廠商交易時間。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public DateTime MerchantTradeDate { get; set; }
            /// <summary>
            /// 交易類型(程式自動設定)。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string PaymentType { get; protected set; }
            /// <summary>
            /// 交易金額。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public decimal TotalAmount { get; set; }
            /// <summary>
            /// 交易描述。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string TradeDesc { get; set; }
            /// <summary>
            /// 商品名稱(ReadOnly)。
            /// </summary>
            public string ItemName { get { return this._ItemName; } }
            /// <summary>
            /// 商品列表(帶入購買商品的資訊，系統會自動轉為 ItemName 所需格式)。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public ItemCollection Items { get; set; }
            /// <summary>
            /// 商品銷售的網址(ReadOnly)。
            /// </summary>
            [RegularExpression(@"^(?:http|https|ftp)://[a-zA-Z0-9\.\-]+(?:\:\d{1,5})?(?:[A-Za-z0-9\.\;\:\@\&\=\+\$\,\?/_]|%u[0-9A-Fa-f]{4}|%[0-9A-Fa-f]{2})*$", ErrorMessage = "{0} is not correct URL.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string ItemURL { get; set; }
            /// <summary>
            /// 備註欄位。
            /// </summary>
            [StringLength(100, ErrorMessage = "{0} max langth as {1}.")]
            public string Remark { get; set; }
            /// <summary>
            /// 付款完成通知的回傳網址。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            [RegularExpression(@"^(?:http|https|ftp)://[a-zA-Z0-9\.\-]+(?:\:\d{1,5})?(?:[A-Za-z0-9\.\;\:\@\&\=\+\$\,\?/_]|%u[0-9A-Fa-f]{4}|%[0-9A-Fa-f]{2})*$", ErrorMessage = "{0} is not correct URL.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string ReturnURL { get; set; }
            /// <summary>
            /// 用戶端返回廠商站台的網址(※頁面導回的時候，不會帶付款結果到此網址，只是將頁面導回而已)。
            /// </summary>
            [RegularExpression(@"^(?:http|https|ftp)://[a-zA-Z0-9\.\-]+(?:\:\d{1,5})?(?:[A-Za-z0-9\.\;\:\@\&\=\+\$\,\?/_]|%u[0-9A-Fa-f]{4}|%[0-9A-Fa-f]{2})*$", ErrorMessage = "{0} is not correct URL.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string ClientBackURL { get; set; }

            /// <summary>
            /// 介接的基本資料傳遞成員類別建構式。
            /// </summary>
            public SendArguments()
            {
                this.Items = new ItemCollection();
            }
        }
    }
}
