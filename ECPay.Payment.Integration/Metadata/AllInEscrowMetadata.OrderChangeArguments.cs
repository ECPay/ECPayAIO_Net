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
        public class OrderChangeArguments
        {
            /// <summary>
            /// 廠商交易編號。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string MerchantTradeNo { get; set; }
            /// <summary>
            /// AllPay 交易編號。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string TradeNo { get; set; }
            /// <summary>
            /// 通知種類。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public NotifiedType InfoType { get; set; }
            /// <summary>
            /// 退款金額。
            /// </summary>
            public decimal? RefundAmount { get; set; }
            /// <summary>
            /// 備註欄位。
            /// </summary>
            [StringLength(100, ErrorMessage = "{0} max langth as {1}.")]
            public string Remark { get; set; }

            /// <summary>
            /// 出貨通知介接參數的建構式。
            /// </summary>
            public OrderChangeArguments()
            {

            }
         }
    }
}
