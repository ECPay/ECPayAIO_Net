using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 定期定額回傳的介接參數的類別。
    /// </summary>
    public class PeriodCreditCardTradeInfo
    {
        /// <summary>
        /// 廠商編號。
        /// </summary>
        public string MerchantID { get; set; }
        /// <summary>
        /// 廠商交易編號。
        /// </summary>
        public string MerchantTradeNo { get; set; }
        /// <summary>
        /// AllPay 的交易編號。
        /// </summary>
        public string TradeNo { get; set; }
        /// <summary>
        /// 交易狀態(1: 授權成功，其餘為失敗，失敗代碼請參考交易訊息代碼一覽表)
        /// </summary>
        public int RtnCode { get; set; }
        /// <summary>
        /// 定期定額的週期種類。
        /// </summary>
        public string PeriodType { get; set; }
        /// <summary>
        /// 定期定額的執行頻率。
        /// </summary>
        public int Frequency { get; set; }
        /// <summary>
        /// 定期定額的執行次數。
        /// </summary>
        public int ExecTimes { get; set; }
        /// <summary>
        /// 定期定額每次授權金額。
        /// </summary>
        public int PeriodAmount { get; set; }
        /// <summary>
        /// 授權金額。
        /// </summary>
        public int amount { get; set; }
        /// <summary>
        /// 授權交易單號。
        /// </summary>
        public long gwsr { get; set; }
        /// <summary>
        /// 授權成功處理時間。
        /// </summary>
        public string process_date { get; set; }
        /// <summary>
        /// 授權碼。
        /// </summary>
        public string auth_code { get; set; }
        /// <summary>
        /// 卡片的末4碼。
        /// </summary>
        public string card4no { get; set; }
        /// <summary>
        /// 卡片的末6碼。
        /// </summary>
        public string card6no { get; set; }
        /// <summary>
        /// 已成功授權次數合計。
        /// </summary>
        public int TotalSuccessTimes { get; set; }
        /// <summary>
        /// 已成功授權總金額。
        /// </summary>
        public int TotalSuccessAmount { get; set; }
        /// <summary>
        /// 每次授權明細。
        /// </summary>
        public PeriodCreditCardTradeInfoLog[] ExecLog { get; set; }

        public int ExecStatus { get; set; }
        /// <summary>
        /// 定期定額回傳的介接參數的類別建構式。
        /// </summary>
        public PeriodCreditCardTradeInfo()
        {

        }
    }

    /// <summary>
    /// 每次授權明細的介接參數的類別。
    /// </summary>
    public class PeriodCreditCardTradeInfoLog
    {
        /// <summary>
        /// 交易狀態(1: 授權成功，其餘為失敗，失敗代碼請參考交易訊息代碼一覽表)
        /// </summary>
        public int RtnCode { get; set; }
        /// <summary>
        /// 授權金額。
        /// </summary>
        public int amount { get; set; }
        /// <summary>
        /// 授權交易單號。
        /// </summary>
        public long gwsr { get; set; }
        /// <summary>
        /// 授權成功處理時間。
        /// </summary>
        public string process_date { get; set; }
        /// <summary>
        /// 授權碼。
        /// </summary>
        public string auth_code { get; set; }
        /// <summary>
        /// 每次授權明細的介接參數的類別建構式。
        /// </summary>
        public PeriodCreditCardTradeInfoLog()
        {
        }
    }
}
