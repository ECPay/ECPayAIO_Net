using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECPay.Payment.Integration;

//廠商下載對帳媒體檔
namespace TradeNoAio
{
    public partial class TradeNoAio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            List<string> enErrors = new List<string>();
            string szFilePath = "D:\\test\\test.txt";//需指定路徑及檔名
            try
            {
                using (AllInOne oPayment = new AllInOne())
                {
                    /* 服務參數 */
                    oPayment.ServiceMethod = HttpMethod.ServerPOST;//介接服務時，呼叫 API 的方法
                    oPayment.ServiceURL = "https://vendor-stage.ecpay.com.tw/PaymentMedia/TradeNoAio";//要呼叫介接服務的網址
                    oPayment.HashKey = "5294y06JbISpM5x9";//ECPay 提供的 HashKey
                    oPayment.HashIV = "v77hoKGq4kWxNNIS";//ECPay 提供的 HashIV
                    oPayment.MerchantID = "2000132";//ECPay 提供的廠商編號
                    /* 基本參數 */
                    oPayment.TradeFile.DateType = TradeDateType.Order;//日期類別
                    oPayment.TradeFile.BeginDate = "2016-01-17";//開始日期
                    oPayment.TradeFile.EndDate = "2016-11-18";//結束日期
                    oPayment.TradeFile.PaymentType = PaymentMethod.ALL;//付款方式
                    oPayment.TradeFile.PlatformStatus = PlatformState.ALL;//訂單類型
                    oPayment.TradeFile.PaymentStatus = PaymentState.ALL;//付款狀態
                    oPayment.TradeFile.AllocateStatus = AllocateState.ALL;//撥款狀態
                    oPayment.TradeFile.NewFormatedMedia = false;//特約合作平台商代號
                    
                    
                    
                    enErrors.AddRange(oPayment.TradeNoAio(szFilePath));
                }
                if (enErrors.Count() == 0)
                {
                    // 其他資料處理。
                    Response.Write("程式執行成功!<br/>對帳媒體檔請至" + szFilePath + "查看。");

                }
                else
                {
                    // 其他資料處理。
                    Response.Write("Error");
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