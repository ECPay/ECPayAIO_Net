using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ECPay.Payment.Integration
{
    public partial class AllInEscrowMetadata
    {
        /// <summary>
        /// 出貨通知介接的資料傳遞成員類別。
        /// </summary>
        public class DeliveryArguments
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
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string TradeNo { get; set; }
            /// <summary>
            /// 出貨狀態。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public ShippingState ShippingState { get; set; }
            /// <summary>
            /// 出貨時間。
            /// </summary>
            public DateTime? ShippingDate { get; set; }
            /// <summary>
            /// 中文編碼格式。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(10, ErrorMessage = "{0} max langth as {1}.")]
            public string EncodeChartset { get; set; }
            /// <summary>
            /// 備註欄位。
            /// </summary>
            [StringLength(100, ErrorMessage = "{0} max langth as {1}.")]
            public string Remark { get; set; }

            /// <summary>
            /// 出貨通知介接參數的建構式。
            /// </summary>
            public DeliveryArguments()
            {
                this.EncodeChartset = "utf-8";
            }
         }
    }
}
