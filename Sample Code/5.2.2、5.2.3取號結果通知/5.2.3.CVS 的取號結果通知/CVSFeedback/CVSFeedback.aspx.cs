using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECPay.Payment.Integration;
using System.Collections;

//CVS的取號結果通知
namespace CVSFeedback
{
    public partial class CVSFeedback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            List<string> enErrors = new List<string>();
            Hashtable htFeedback = null;

            try
            {
                using (AllInOne oPayment = new AllInOne())
                {
                    oPayment.HashKey = "5294y06JbISpM5x9";//ECPay 提供的 HashKey
                    oPayment.HashIV = "v77hoKGq4kWxNNIS";//ECPay 提供的 HashIV
                 /* 取回付款結果 */
                  enErrors.AddRange(oPayment.CheckOutFeedback(ref htFeedback));
                 }
                 // 取回所有資料
                 if (enErrors.Count() == 0)
                 {
                     /* 支付後的回傳的基本參數 */
                     string szMerchantID = String.Empty;
                     string szMerchantTradeNo = String.Empty;
                     string szPaymentType = String.Empty;
                     string szRtnCode = String.Empty;
                     string szRtnMsg = String.Empty;
                     string szTradeAmt = String.Empty;
                     string szTradeDate = String.Empty;
                     string szTradeNo = String.Empty;
                     /* 使用 CVS 交易時，回傳的額外參數 */
                     string szPaymentNo = String.Empty;
                     string szExpireDate = String.Empty;
                     string szBarcode1 = String.Empty;
                     string szBarcode2 = String.Empty;
                     string szBarcode3 = String.Empty;
                     // 取得資料於畫面
                     foreach (string szKey in htFeedback.Keys)
                     {
                        switch (szKey)
                        {
                             /* 使用 CVS 交易時回傳的參數 */
                             case "MerchantID": szMerchantID = htFeedback[szKey].ToString(); break;
                             case "MerchantTradeNo": szMerchantTradeNo = htFeedback[szKey].ToString(); break;
                             case "RtnCode": szRtnCode = htFeedback[szKey].ToString(); break;
                             case "RtnMsg": szRtnMsg = htFeedback[szKey].ToString(); break;
                             case "TradeNo": szTradeNo = htFeedback[szKey].ToString(); break;
                             case "TradeAmt": szTradeAmt = htFeedback[szKey].ToString(); break;
                             case "PaymentType": szPaymentType = htFeedback[szKey].ToString(); break;
                             case "TradeDate": szTradeDate = htFeedback[szKey].ToString(); break;
                             case "PaymentNo": szPaymentNo = htFeedback[szKey].ToString(); break;
                             case "ExpireDate": szExpireDate = htFeedback[szKey].ToString(); break;
                             case "Barcode1": szBarcode1 = htFeedback[szKey].ToString(); break;
                             case "Barcode2": szBarcode2 = htFeedback[szKey].ToString(); break;
                             case "Barcode3": szBarcode3 = htFeedback[szKey].ToString(); break;
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
             // 例外錯誤處理。
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