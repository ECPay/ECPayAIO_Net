using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECPay.Payment.Integration;
using System.Collections;

//信用卡關帳／退刷／取消／放棄
namespace DoAction
{
    public partial class DoAction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> enErrors = new List<string>();
            Hashtable htFeedback = null;
            try
            {
                using (AllInOne oPayment = new AllInOne())
                {
                    /* 服務參數 */
                    oPayment.ServiceMethod = HttpMethod.ServerPOST;//介接服務時，呼叫 API 的方法
                    oPayment.ServiceURL = "https://payment.ecpay.com.tw/CreditDetail/DoAction";//要呼叫介接服務的網址
                    oPayment.HashKey = "";//ECPay 提供的 HashKey
                    oPayment.HashIV = "";//ECPay 提供的 HashIV
                    oPayment.MerchantID = "";//ECPay 提供的廠商編號
                    /* 基本參數 */
                    oPayment.Action.MerchantTradeNo = "";//廠商交易編號
                    oPayment.Action.TradeNo = "";//請保存 ECPay 的交易編號與 MerchantTradeNo 的關連
                    oPayment.Action.Action = ActionType.C;//針對訂單做處理的動作
                    oPayment.Action.TotalAmount = Decimal.Parse("1000");//訂單總金額
                    //oPayment.Action.PlatformID = "";//特約合作平台商代號
                                 
                    enErrors.AddRange(oPayment.DoAction(ref htFeedback));
                }
                if (enErrors.Count() == 0)
                {
                    /* 執行後的回傳的基本參數 */
                    string szMerchantID = String.Empty;
                    string szMerchantTradeNo = String.Empty;
                    string szTradeNo = String.Empty;
                    string szRtnCode = String.Empty;
                    string szRtnMsg = String.Empty;
                    // 取得資料於畫面
                    foreach (string szKey in htFeedback.Keys)
                    {
                        switch (szKey)
                        {
                            /* 執行後的回傳的基本參數 */
                            case "MerchantID": szMerchantID = htFeedback[szKey].ToString(); break;
                            case "MerchantTradeNo": szMerchantTradeNo = htFeedback[szKey].ToString(); break;
                            case "TradeNo": szTradeNo = htFeedback[szKey].ToString(); break;
                            case "RtnCode": szRtnCode = htFeedback[szKey].ToString(); break;
                            case "RtnMsg": szRtnMsg = htFeedback[szKey].ToString(); break;
                            default: break;
                        }
                    }
                    // 其他資料處理。


                    //Response.Write("MerchantID:" + szMerchantID + "<br />");
                    Response.Write("TradeNo:" + szTradeNo + "<br />");
                    Response.Write("RtnCode:" + szRtnCode + "<br />");
                    Response.Write("RtnMsg:" + szRtnMsg);


                }
                else
                {
                    // 其他資料處理(印出錯誤訊息)。

                    Response.Write(enErrors[0]);


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