using System;
using System.Collections.Generic;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 通知種類。
    /// </summary>
    public enum NotifiedType
    {
        /// <summary>
        /// 退回部分金額。
        /// </summary>
        RefundPartAmount = 1,
        /// <summary>
        /// 退回全部金額。
        /// </summary>
        RefundAllAmount = 2,
        /// <summary>
        /// 換貨後重新出貨。
        /// </summary>
        Redelivery = 3,
        /// <summary>
        /// 商品無異常。
        /// </summary>
        GoodsNoAbnormality = 4
    }
}
