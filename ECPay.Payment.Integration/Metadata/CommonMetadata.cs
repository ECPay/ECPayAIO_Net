using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 共用的介接參數的類別。
    /// </summary>
    public abstract partial class CommonMetadata
    {
        /// <summary>
        /// 介接服務的網址。
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        [RegularExpression(@"^(?:http|https|ftp)://[a-zA-Z0-9\.\-]+(?:\:\d{1,5})?(?:[A-Za-z0-9\.\;\:\@\&\=\+\$\,\?/_]|%u[0-9A-Fa-f]{4}|%[0-9A-Fa-f]{2})*$", ErrorMessage = "{0} is not correct URL.")]
        [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
        public string ServiceURL { get; set; }
        /// <summary>
        /// 介接服務的方法(預設: POST, 除 QueryTradeInfo 方法提供 SOAP 呼叫 WebService 外，其餘皆使用 POST)。
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        public HttpMethod ServiceMethod { get; set; }
        /// <summary>
        /// 介接的 HashKey。
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        public string HashKey { get; set; }
        /// <summary>
        /// 介接的 HashIV。
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        public string HashIV { get; set; }
        /// <summary>
        /// 廠商編號。
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(10, ErrorMessage = "{0} max langth as {1}.")]
        public string MerchantID { get; set; }
        /// <summary>
        /// 共用的介接參數的建構式。
        /// </summary>
        public CommonMetadata()
        {

        }
    }
}
