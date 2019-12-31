using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECPay.Payment.Integration;
using System.Collections;

//訂單查詢
namespace QueryTradeInfo
{
    public partial class QueryTradeInfo : System.Web.UI.Page
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
                    oPayment.ServiceMethod = HttpMethod.ServerPOST; //介接服務時，呼叫 API 的方法
                    oPayment.ServiceURL = "https://payment-stage.ecpay.com.tw/Cashier/QueryTradeInfo";//要呼叫介接服務的網址
                    oPayment.HashKey = "5294y06JbISpM5x9";//ECPay 提供的 HashKey
                    oPayment.HashIV = "v77hoKGq4kWxNNIS";//ECPay 提供的 HashIV
                    oPayment.MerchantID = "2000132";//ECPay 提供的廠商編號

                    /* 基本參數 */
                    oPayment.Query.MerchantTradeNo = "Luke4640";//廠商的交易編號
                    //oPayment.Query.PlatformID = "";//特約合作平台商代號
                    
                    /**********************************************************************************/
                    
                    /* 查詢訂單 */
                    enErrors.AddRange(oPayment.QueryTradeInfo(ref htFeedback));
                }
                // 取回所有資料
                if (enErrors.Count() == 0)
                {
                    /* 查詢後的回傳的基本參數 */
                    string szMerchantID = String.Empty;
                    string szMerchantTradeNo = String.Empty;
                    string szTradeNo = String.Empty;
                    string szTradeAmt = String.Empty;
                    string szPaymentDate = String.Empty;
                    string szPaymentType = String.Empty;
                    string szHandlingCharge = String.Empty;
                    string szPaymentTypeChargeFee = String.Empty;
                    string szTradeDate = String.Empty;
                    string szTradeStatus = String.Empty;
                    string szItemName = String.Empty;
                    /* 使用 WebATM 交易時，回傳的額外參數 */
                    string szWebATMAccBank = String.Empty;
                    string szWebATMAccNo = String.Empty;
                    string szWebATMBankName = String.Empty;
                    /* 使用 ATM 交易時，回傳的額外參數 */
                    string szATMAccBank = String.Empty;
                    string szATMAccNo = String.Empty;
                    /* 使用 CVS 或 BARCODE 交易時，回傳的額外參數 */
                    string szPaymentNo = String.Empty;
                    string szPayFrom = String.Empty;
                    /* 使用 Alipay 交易時，回傳的額外參數 */
                    string szAlipayID = String.Empty;
                    string szAlipayTradeNo = String.Empty;
                    /* 使用 Tenpay 交易時，回傳的額外參數 */
                    string szTenpayTradeNo = String.Empty;
                    /* 使用 Credit 交易時，回傳的額外參數 */
                    string szGwsr = String.Empty;
                    string szProcessDate = String.Empty;
                    string szAuthCode = String.Empty;
                    string szAmount = String.Empty;
                    string szStage = String.Empty;
                    string szStast = String.Empty;
                    string szStaed = String.Empty;
                    string szECI = String.Empty;
                    string szCard4No = String.Empty;
                    string szCard6No = String.Empty;
                    string szRedDan = String.Empty;
                    string szRedDeAmt = String.Empty;
                    string szRedOkAmt = String.Empty;
                    string szRedYet = String.Empty;
                    string szPeriodType = String.Empty;
                    string szFrequency = String.Empty;
                    string szExecTimes = String.Empty;
                    string szPeriodAmount = String.Empty;
                    string szTotalSuccessTimes = String.Empty;
                    string szTotalSuccessAmount = String.Empty;
                    string szCustomField1 = string.Empty;
                    string szCustomField2 = string.Empty;
                    string szCustomField3 = string.Empty;
                    string szCustomField4 = string.Empty;
                    string szStoreID=string.Empty;

                    // 取得資料於畫面
                    foreach (string szKey in htFeedback.Keys)
                    {
                        switch (szKey)
                        {
                            /* 查詢後的回傳的基本參數 */
                            case "MerchantID": szMerchantID = htFeedback[szKey].ToString(); break;
                            case "MerchantTradeNo": szMerchantTradeNo = htFeedback[szKey].ToString(); break;
                            case "TradeNo": szTradeNo = htFeedback[szKey].ToString(); break;
                            case "TradeAmt": szTradeAmt = htFeedback[szKey].ToString(); break;
                            case "PaymentDate": szPaymentDate = htFeedback[szKey].ToString(); break;
                            case "PaymentType": szPaymentType = htFeedback[szKey].ToString(); break;
                            case "HandlingCharge": szHandlingCharge = htFeedback[szKey].ToString(); break;
                            case "PaymentTypeChargeFee": szPaymentTypeChargeFee = htFeedback[szKey].ToString(); break;
                            case "TradeDate": szTradeDate = htFeedback[szKey].ToString(); break;
                            case "TradeStatus": szTradeStatus = htFeedback[szKey].ToString(); break;
                            case "ItemName": szItemName = htFeedback[szKey].ToString(); break;
                            /* 使用 WebATM 交易時回傳的參數 */
                            case "WebATMAccBank": szWebATMAccBank = htFeedback[szKey].ToString(); break;
                            case "WebATMAccNo": szWebATMAccNo = htFeedback[szKey].ToString(); break;
                            /* 使用 ATM 交易時回傳的參數 */
                            case "ATMAccBank": szATMAccBank = htFeedback[szKey].ToString(); break;
                            case "ATMAccNo": szATMAccNo = htFeedback[szKey].ToString(); break;
                            /* 使用 CVS 或 BARCODE 交易時回傳的參數 */
                            case "PaymentNo": szPaymentNo = htFeedback[szKey].ToString(); break;
                            case "PayFrom": szPayFrom = htFeedback[szKey].ToString(); break;
                            /* 使用 Alipay 交易時回傳的參數 */
                            case "AlipayID": szAlipayID = htFeedback[szKey].ToString(); break;
                            case "AlipayTradeNo": szAlipayTradeNo = htFeedback[szKey].ToString(); break;
                            /* 使用 Tenpay 交易時回傳的參數 */
                            case "TenpayTradeNo": szTenpayTradeNo = htFeedback[szKey].ToString(); break;
                            /* 使用 Credit 交易時回傳的參數 */
                            case "gwsr": szGwsr = htFeedback[szKey].ToString(); break;
                            case "process_date": szProcessDate = htFeedback[szKey].ToString(); break;
                            case "auth_code": szAuthCode = htFeedback[szKey].ToString(); break;
                            case "amount": szAmount = htFeedback[szKey].ToString(); break;
                            case "stage": szStage = htFeedback[szKey].ToString(); break;
                            case "stast": szStast = htFeedback[szKey].ToString(); break;
                            case "staed": szStaed = htFeedback[szKey].ToString(); break;
                            case "eci": szECI = htFeedback[szKey].ToString(); break;
                            case "card4no": szCard4No = htFeedback[szKey].ToString(); break;
                            case "card6no": szCard6No = htFeedback[szKey].ToString(); break;
                            case "red_dan": szRedDan = htFeedback[szKey].ToString(); break;
                            case "red_de_amt": szRedDeAmt = htFeedback[szKey].ToString(); break;
                            case "red_ok_amt": szRedOkAmt = htFeedback[szKey].ToString(); break;
                            case "red_yet": szRedYet = htFeedback[szKey].ToString(); break;
                            case "PeriodType": szPeriodType = htFeedback[szKey].ToString(); break;
                            case "Frequency": szFrequency = htFeedback[szKey].ToString(); break;
                            case "ExecTimes": szExecTimes = htFeedback[szKey].ToString(); break;
                            case "PeriodAmount": szPeriodAmount = htFeedback[szKey].ToString(); break;
                            case "TotalSuccessTimes": szTotalSuccessTimes = htFeedback[szKey].ToString(); break;
                            case "TotalSuccessAmount": szTotalSuccessAmount = htFeedback[szKey].ToString(); break;
                            case "CustomField1": szCustomField1=htFeedback[szKey].ToString(); break;
                            case "CustomField2": szCustomField2=htFeedback[szKey].ToString(); break;
                            case "CustomField3": szCustomField3=htFeedback[szKey].ToString(); break;
                            case "CustomField4": szCustomField4=htFeedback[szKey].ToString(); break;
                            case "StoreID":szStoreID=htFeedback[szKey].ToString(); break;
                            default: break;
                        }

                    }
                    // 其他資料處理(將回傳值印在頁面)。
                    string output = String.Format(@"MerchantID={0}&MerchantTradeNo={1}&TradeNo={2}&TradeAmt={3}&PaymentDate={4}&PaymentType={5}&HandlingCharge={6}&PaymentTypeChargeFee={7}&TradeDate={8}&TradeStatus={9}&ItemName={10}&AlipayID={11}&AlipayTradeNo{12}&amount={13}
                    &ATMAccBank={14}&ATMAccNo={15}&auth_code={16}&card4no={17}&card6no={18}&CustomField1={19}&CustomField2={20}&CustomField3={21}&CustomField4={22}&eci={23}&ExecTimes={24}&Frequency={25}&gwsr={26}&PayFrom={27}&PaymentNo={28}&PeriodAmount={29}&PeriodType={30}&process_date={31}
                    &red_dan={32}&red_de_amt={33}&red_ok_amt={34}&red_yet={35}&staed={36}&stage={37}&StoreID={38}&TenpayTradeNo={39}&TotalSuccessAmount={40}&TotalSuccessTimes={41}&WebATMAccBank={42}&WebATMAccNo={43}&WebATMBankName={44}&stast={45}"
                        , szMerchantID, szMerchantTradeNo, szTradeNo, szTradeAmt, szPaymentDate, szPaymentType, szHandlingCharge, szPaymentTypeChargeFee, szTradeDate, szTradeStatus, szItemName,szAlipayID,szAlipayTradeNo,szAmount
                        , szATMAccBank, szATMAccNo, szAuthCode, szCard4No, szCard6No, szCustomField1, szCustomField2, szCustomField3, szCustomField4,szECI,szExecTimes,szFrequency,szGwsr,szPayFrom,szPaymentNo,szPeriodAmount,szPeriodType,szProcessDate
                        , szRedDan, szRedDeAmt, szRedOkAmt, szRedYet, szStaed, szStage, szStoreID, szTenpayTradeNo, szTotalSuccessAmount, szTotalSuccessTimes, szWebATMAccBank, szWebATMAccNo, szWebATMBankName, szStast);

                    this.Response.Clear();
                    this.Response.Write(output);
                    this.Response.Flush();
                    this.Response.End();

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