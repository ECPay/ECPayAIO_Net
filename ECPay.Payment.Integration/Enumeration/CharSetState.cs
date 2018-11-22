using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 編碼設定狀態
    /// </summary>
    public enum CharSetState : byte
    {
        /// <summary>
        /// 預設-試使用端系統決定
        /// </summary>
        Default = 0,
        Big5 =1 ,
        UTF8 = 2
    }
}

