using System;
using System.Collections.Generic;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 信用卡訂單處理動作。
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 關帳。
        /// </summary>
        C = 0,
        /// <summary>
        /// 退刷。
        /// </summary>
        R = 1,
        /// <summary>
        /// 取消。
        /// </summary>
        E = 2,
        /// <summary>
        /// 放棄。
        /// </summary>
        N = 3
    }
}
