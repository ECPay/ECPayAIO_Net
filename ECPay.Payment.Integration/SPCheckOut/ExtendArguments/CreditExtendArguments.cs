using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration.SPCheckOut.ExtendArguments
{


    public class CreditPayOffExtendArguments : CreditCard
    {
        /// <summary>
        /// 信用卡是否使用紅利折抵。
        /// </summary>
        public string Redeem { get; set; }

        public CreditPayOffExtendArguments() 
        {
            this.Redeem = "Y";
        }
    }

    /// <summary>
    /// 信用卡分期付款
    /// </summary>
    public class CreditInstallmentExtendArguments : CreditCard
    {
        /// <summary>
        /// 刷卡分期期數。
        /// </summary>
        public string CreditInstallment { get; set; }
    }

    /// <summary>
    /// 信用卡定期定額
    /// </summary>
    public class CreditRSPExtendArguments : CreditCard
    {
        /// <summary>
        /// 每次授權金額
        /// </summary>
        public int PeriodAmount { get; set; }

        /// <summary>
        /// 週期種類
        /// </summary>
        public string PeriodType { get; set; }

        /// <summary>
        /// 執行頻率
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// 執行次數
        /// </summary>
        public int ExecTimes { get; set; }

        /// <summary>
        /// 定期定額的執行結果回應URL
        /// </summary>
        public string PeriodReturnURL { get; set; }
    }

    public class CreditCard 
    {
        /// <summary>
        /// 記憶卡號
        /// </summary>
        public int BindingCard { get; set; }
        /// <summary>
        /// 記憶卡號識別碼
        /// </summary>
        public string MerchantMemberID { get; set; }
    } 
}
