using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECPay.Payment.Integration;
using System.Collections;


//Credit定期定額授權成功通知
namespace PeriodFeedback
{
    public partial class PeriodFeedback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> enErrors = new List<string>();
            Hashtable htFeedback = null;

            try
                {
                    using (AllInOne oPayment = new AllInOne())
                    {
                        oPayment.HashKey = "";//ECPay 提供的 HashKey
                        oPayment.HashIV = "";//ECPay 提供的 HashIV
                         /* 取回付款結果 */
                        enErrors.AddRange(oPayment.CheckOutFeedback(ref htFeedback));
                     }
                     // 取回所有資料
                     if (enErrors.Count() == 0)
                     {
                         /* 支付後的回傳的基本參數 */
                         string szMerchantID = String.Empty;
                         string szMerchantTradeNo = String.Empty;
                         string szRtnCode = String.Empty;
                         string szRtnMsg = String.Empty;
                         /* 使用定期定額交易時，回傳的額外參數 */
                         string szPeriodType = String.Empty;
                         string szFrequency = String.Empty;
                         string szExecTimes = String.Empty;
                         string szAmount = String.Empty;
                         string szGwsr = String.Empty;
                         string szProcessDate = String.Empty;
                         string szAuthCode = String.Empty;
                         string szFirstAuthAmount = String.Empty;
                         string szTotalSuccessTimes = String.Empty;
                         // 取得資料於畫面
                         foreach (string szKey in htFeedback.Keys)
                         {
                         switch (szKey)
                         {
                         /* 使用定期定額交易時回傳的參數 */
                             case "MerchantID": szMerchantID = htFeedback[szKey].ToString(); break;
                             case "MerchantTradeNo": szMerchantTradeNo = htFeedback[szKey].ToString();
                             break;
                             case "RtnCode": szRtnCode = htFeedback[szKey].ToString(); break;
                             case "RtnMsg": szRtnMsg = htFeedback[szKey].ToString(); break;
                             case "PeriodType": szPeriodType = htFeedback[szKey].ToString(); break;
                             case "Frequency": szFrequency = htFeedback[szKey].ToString(); break;
                             case "ExecTimes": szExecTimes = htFeedback[szKey].ToString(); break;
                             case "Amount": szAmount = htFeedback[szKey].ToString(); break;
                             case "Gwsr": szGwsr = htFeedback[szKey].ToString(); break;
                             case "ProcessDate": szProcessDate = htFeedback[szKey].ToString(); break;
                             case "AuthCode": szAuthCode = htFeedback[szKey].ToString(); break;
                             case "FirstAuthAmount": szFirstAuthAmount = htFeedback[szKey].ToString();
                             break;
                             case "TotalSuccessTimes": szTotalSuccessTimes = htFeedback[szKey].ToString();
                             break;
                             default: break;
                         }
                     }
                     // 其他資料處理。
                     
                     } else {
                     // 其他資料處理。
                    
                      }
                    }
                    catch (Exception ex)
                    {
                     enErrors.Add(ex.Message);
                    }
                    finally
                    {
                     this.Response.Clear();
                     // 回覆成功訊息。
                     if (enErrors.Count() == 0)
                     this.Response.Write("1|OK");
                    // 回覆錯誤訊息。
                     else
                     this.Response.Write(String.Format("0|{0}", String.Join("\\r\\n", enErrors)));
                     this.Response.Flush();
                     this.Response.End();
                    }




        }
    }
}