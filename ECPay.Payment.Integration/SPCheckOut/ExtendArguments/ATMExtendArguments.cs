using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration.SPCheckOut.ExtendArguments
{
    /// <summary>
    /// ATM付款方式延伸參數
    /// </summary>
    public class ATMExtendArguments
    {
        /// <summary>
        /// 允許繳費有效天數
        /// </summary>
        public int ExpireDate { get; set; }

        /// <summary>
        /// Server端回傳付款相關資訊
        /// </summary>
        public string PaymentInfoURL { get; set; }

        public ATMExtendArguments() 
        {
            ExpireDate = 3;
        }
    }
}
