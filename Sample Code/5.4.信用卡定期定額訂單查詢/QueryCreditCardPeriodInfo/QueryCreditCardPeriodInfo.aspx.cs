using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECPay.Payment.Integration;
using System.Runtime.Serialization;
using Newtonsoft.Json;

//信用卡定期定額訂單查詢
namespace QueryCreditCardPeriodInfo
{
    public partial class QueryCreditCardPeriodInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> enErrors = new List<string>();
            PeriodCreditCardTradeInfo oFeedback = null;

            try
            {

                using (AllInOne oPayment = new AllInOne())
                {
                    /* 服務參數 */
                    oPayment.ServiceMethod = HttpMethod.ServerPOST; //介接服務時，呼叫 API 的方法
                    oPayment.ServiceURL = "https://payment-stage.ecpay.com.tw/Cashier/QueryCreditCardPeriodInfo";//要呼叫介接服務的網址
                    oPayment.HashKey = "5294y06JbISpM5x9";//ECPay 提供的 HashKey
                    oPayment.HashIV = "v77hoKGq4kWxNNIS";//ECPay 提供的 HashIV
                    oPayment.MerchantID = "2000132";//ECPay 提供的廠商編號


                    /* 基本參數 */
                    oPayment.Query.MerchantTradeNo = "Luke39692";//廠商的交易編號。
                    
                    /**********************************************************************************************/
                    
                    
                    /* 查詢訂單 */
                    enErrors.AddRange(oPayment.QueryPeriodCreditCardTradeInfo(ref oFeedback));
                }
                // 取回所有資料
                if (enErrors.Count() == 0)
                {
                    /* 查詢後的回傳的基本參數 */
                    string szMerchantID = oFeedback.MerchantID;
                    string szMerchantTradeNo = oFeedback.MerchantTradeNo;
                    string szTradeNo = oFeedback.TradeNo;
                    int nRtnCode = oFeedback.RtnCode;
                    string szPeriodType = oFeedback.PeriodType;
                    int nFrequency = oFeedback.Frequency;
                    int nExecTimes = oFeedback.ExecTimes;
                    int nPeriodAmount = oFeedback.PeriodAmount;
                    int nAmount = oFeedback.amount;
                    long nGwsr = oFeedback.gwsr;
                    string szProcessDate = oFeedback.process_date;
                    string szAuthCode = oFeedback.auth_code;
                    string szCard4No = oFeedback.card4no;
                    string szCard6No = oFeedback.card6no;
                    int nTotalSuccessTimes = oFeedback.TotalSuccessTimes;
                    int nTotalSuccessAmount = oFeedback.TotalSuccessAmount;
                    int szExecStatus = oFeedback.ExecStatus;
                    

                    // 其他資料處理(印出回傳值至頁面)。
                    Response.Write("查詢定期定額訂單結果:" + "<br/><br/>");
                    Response.Write("MerchantID:" + oFeedback.MerchantID + "<br/>");
                    Response.Write("MerchantTradeNo:" + oFeedback.MerchantTradeNo + "<br/>");
                    Response.Write("TradeNo:" + oFeedback.TradeNo + "<br/>");
                    Response.Write("RtnCode:" + oFeedback.RtnCode + "<br/>");
                    Response.Write("PeriodType:" + oFeedback.PeriodType + "<br/>");
                    Response.Write("Frequency:" + oFeedback.Frequency + "<br/>");
                    Response.Write("ExecTimes:" + oFeedback.ExecTimes + "<br/>");
                    Response.Write("PeriodAmount:" + oFeedback.PeriodAmount + "<br/>");
                    Response.Write("amount:" + oFeedback.amount + "<br/>");
                    Response.Write("gwsr:" + oFeedback.gwsr + "<br/>");
                    Response.Write("process_date:" + oFeedback.process_date + "<br/>");
                    Response.Write("auth_code:" + oFeedback.auth_code + "<br/>");
                    Response.Write("card4no:" + oFeedback.card4no + "<br/>");
                    Response.Write("card6no:" + oFeedback.card6no + "<br/>");
                    Response.Write("TotalSuccessTimes:" + oFeedback.TotalSuccessTimes + "<br/>");
                    Response.Write("TotalSuccessAmount:" + oFeedback.TotalSuccessAmount + "<br/>");
                    Response.Write("ExecStatus:" + oFeedback.ExecStatus + "<br/>");
                    string json_data = JsonConvert.SerializeObject(oFeedback.ExecLog);
                    Response.Write("ExecLog:" + json_data + "<br/>");

                }

                
            }
            catch (Exception ex)
            {
                // 例外錯誤處理。
                enErrors.Add(ex.Message);
            }
            finally
            {
                // 顯示錯誤訊息。
                if (enErrors.Count() > 0)
                {
                    string szErrorMessage = String.Join("\\r\\n", enErrors);
                }

            }


        }
    }
}