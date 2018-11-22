using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 使用記憶信用卡註記。
    /// </summary>
    public enum BindingCardType
    {
        /// <summary>
        /// 使用記憶信用卡。
        /// </summary>
        Yes = 1,
        /// <summary>
        /// 不使用記憶信用卡。
        /// </summary>
        No = 0
    }
}
