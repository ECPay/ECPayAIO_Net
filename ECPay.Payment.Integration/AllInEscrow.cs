using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web;
using System.IO;
using System.Runtime.InteropServices;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 綠界科技履約保證介接模組。
    /// </summary>
    public class AllInEscrow : AllInEscrowMetadata, IDisposable
    {
        #region - 屬性欄位成員

        /// <summary>
        /// 取得目前頁面。
        /// </summary>
        private Page CurrentPage
        {
            get
            {
                if (null != HttpContext.Current)
                    return HttpContext.Current.Handler as Page;
                else
                    return null;
            }
        }

        #endregion

        #region - 履約保證介接函式庫的建構式

        /// <summary>
        /// 履約保證介接的建構式。
        /// </summary>
        public AllInEscrow() : base()
        {
        }

        #endregion

        #region - 提供介接的函數方法

        /// <summary>
        /// 訂單產生(HttpPOST, 不支援 Ajax 非同步)。
        /// </summary>
        /// <returns>錯誤訊息的集合字串。</returns>
        /// <remarks>
        /// 該方法自動產生 Http POST 頁面，將參數送至 AllInEscrow.ServiceURL 位址，
        /// 需於 Web 站台或 Web 專案中撰寫以下程式碼才能正常送出訂單資料。
        /// </remarks>
        /// <example>
        /// <code>
        /// /*
        ///  *  產生訂單的範例程式碼。
        ///  */
        /// List&lt;string&gt; enErrors = null;
        ///
        /// try
        /// {
        ///     using (AllInEscrow oPayment = new AllInEscrow())
        ///     {
        ///         /* 服務參數 */
        ///         oPayment.ServiceMethod = HttpMethod.HttpPOST;
        ///         oPayment.ServiceURL = "&lt;&lt;AllPay Service URL&gt;&gt;";
        ///         oPayment.HashKey = "&lt;&lt;Hash Key&gt;&gt;";
        ///         oPayment.HashIV = "&lt;&lt;Hash IV&gt;&gt;";
        ///         oPayment.MerchantID = "&lt;&lt;Merchant ID&gt;&gt;";
        ///         /* 基本參數 */
        ///         oPayment.Send.ReturnURL = "&lt;&lt;Return URL&gt;&gt;";
        ///         oPayment.Send.ClientBackURL = "&lt;&lt;Client Back URL&gt;&gt;";
        ///
        ///         oPayment.Send.MerchantTradeNo = "&lt;&lt;Merchant Trade No&gt;&gt;";
        ///         oPayment.Send.MerchantTradeDate = DateTime.Parse("&lt;&lt;Merchant Trade Date&gt;&gt;");
        ///         oPayment.Send.TotalAmount = Decimal.Parse("&lt;&lt;Total Amount&gt;&gt;");
        ///         oPayment.Send.TradeDesc = "&lt;&lt;Trade Desc&gt;&gt;";
        ///         oPayment.Send.Currency = "&lt;&lt;Currency&gt;&gt;";
        ///         oPayment.Send.EncodeChartset = "&lt;&lt;Encode Chartset&gt;&gt;";
        ///         oPayment.Send.UseAllpayAddress = "&lt;&lt;Use Allpay Address&gt;&gt;";
        ///         oPayment.Send.CreditInstallment = Int32.Parse("&lt;&lt;Credit Installment&gt;&gt;");
        ///         oPayment.Send.InstallmentAmount = Decimal.Parse("&lt;&lt;Installment Amount&gt;&gt;");
        ///         oPayment.Send.Redeem = "&lt;&lt;Redeem&gt;&gt;";
        ///         oPayment.Send.ShippingDate = "&lt;&lt;Shipping Date&gt;&gt;";
        ///         oPayment.Send.ConsiderHour = Int32.Parse("&lt;&lt;Consider Hour&gt;&gt;");
        ///         oPayment.Send.Remark = "&lt;&lt;Remark&gt;&gt;";
        ///         // 加入選購商品資料。
        ///         oPayment.Send.Items.Add(new Item() { Name = "&lt;&lt;Product Name&gt;&gt;", Price = Decimal.Parse("&lt;&lt;Unit Price&gt;&gt;"), Currency = "&lt;&lt;Currency&gt;&gt;", Quantity = Int32.Parse("&lt;&lt;Quantity&gt;&gt;"), URL = "&lt;&lt;Product Detail URL&gt;&gt;" });
        ///         oPayment.Send.Items.Add(new Item() { Name = "&lt;&lt;Product Name&gt;&gt;", Price = Decimal.Parse("&lt;&lt;Unit Price&gt;&gt;"), Currency = "&lt;&lt;Currency&gt;&gt;", Quantity = Int32.Parse("&lt;&lt;Quantity&gt;&gt;"), URL = "&lt;&lt;Product Detail URL&gt;&gt;" });
        ///         oPayment.Send.Items.Add(new Item() { Name = "&lt;&lt;Product Name&gt;&gt;", Price = Decimal.Parse("&lt;&lt;Unit Price&gt;&gt;"), Currency = "&lt;&lt;Currency&gt;&gt;", Quantity = Int32.Parse("&lt;&lt;Quantity&gt;&gt;"), URL = "&lt;&lt;Product Detail URL&gt;&gt;" });
        ///
        ///         enErrors.AddRange(oPayment.CheckOut());
        ///     }
        /// }
        /// catch (Exception ex)
        /// {
        ///     // 例外錯誤處理。
        ///     enErrors.Add(ex.Message);
        /// }
        /// finally
        /// {
        ///     // 顯示錯誤訊息。
        ///     if (enErrors.Count() &gt; 0)
        ///         ScriptManager.RegisterStartupScript(this, typeof(Page), "_MESSAGE", String.Format("alert(\"{0}\");", String.Join("\\r\\n", enErrors)), true);
        /// }
        /// </code>
        /// </example>
        public IEnumerable<string> CheckOut()
        {
            return this.CheckOut("_self");
        }
        /// <summary>
        /// 訂單產生(HttpPOST, 不支援 Ajax 非同步)。
        /// </summary>
        /// <param name="target">目標視窗</param>
        /// <returns>錯誤訊息的集合字串。</returns>
        /// <remarks>
        /// 該方法自動產生 Http POST 頁面，將參數送至 AllInEscrow.ServiceURL 位址，
        /// 需於 Web 站台或 Web 專案中撰寫以下程式碼才能正常送出訂單資料。
        /// </remarks>
        /// <example>
        /// <code>
        /// /*
        ///  *  產生訂單的範例程式碼。
        ///  */
        /// List&lt;string&gt; enErrors = null;
        ///
        /// try
        /// {
        ///     using (AllInEscrow oPayment = new AllInEscrow())
        ///     {
        ///         /* 服務參數 */
        ///         oPayment.ServiceMethod = HttpMethod.HttpPOST;
        ///         oPayment.ServiceURL = "&lt;&lt;AllPay Service URL&gt;&gt;";
        ///         oPayment.HashKey = "&lt;&lt;Hash Key&gt;&gt;";
        ///         oPayment.HashIV = "&lt;&lt;Hash IV&gt;&gt;";
        ///         oPayment.MerchantID = "&lt;&lt;Merchant ID&gt;&gt;";
        ///         /* 基本參數 */
        ///         oPayment.Send.ReturnURL = "&lt;&lt;Return URL&gt;&gt;";
        ///         oPayment.Send.ClientBackURL = "&lt;&lt;Client Back URL&gt;&gt;";
        ///
        ///         oPayment.Send.MerchantTradeNo = "&lt;&lt;Merchant Trade No&gt;&gt;";
        ///         oPayment.Send.MerchantTradeDate = DateTime.Parse("&lt;&lt;Merchant Trade Date&gt;&gt;");
        ///         oPayment.Send.TotalAmount = Decimal.Parse("&lt;&lt;Total Amount&gt;&gt;");
        ///         oPayment.Send.TradeDesc = "&lt;&lt;Trade Desc&gt;&gt;";
        ///         oPayment.Send.Currency = "&lt;&lt;Currency&gt;&gt;";
        ///         oPayment.Send.EncodeChartset = "&lt;&lt;Encode Chartset&gt;&gt;";
        ///         oPayment.Send.UseAllpayAddress = "&lt;&lt;Use Allpay Address&gt;&gt;";
        ///         oPayment.Send.CreditInstallment = Int32.Parse("&lt;&lt;Credit Installment&gt;&gt;");
        ///         oPayment.Send.InstallmentAmount = Decimal.Parse("&lt;&lt;Installment Amount&gt;&gt;");
        ///         oPayment.Send.Redeem = "&lt;&lt;Redeem&gt;&gt;";
        ///         oPayment.Send.ShippingDate = "&lt;&lt;Shipping Date&gt;&gt;";
        ///         oPayment.Send.ConsiderHour = Int32.Parse("&lt;&lt;Consider Hour&gt;&gt;");
        ///         oPayment.Send.Remark = "&lt;&lt;Remark&gt;&gt;";
        ///         // 加入選購商品資料。
        ///         oPayment.Send.Items.Add(new Item() { Name = "&lt;&lt;Product Name&gt;&gt;", Price = Decimal.Parse("&lt;&lt;Unit Price&gt;&gt;"), Currency = "&lt;&lt;Currency&gt;&gt;", Quantity = Int32.Parse("&lt;&lt;Quantity&gt;&gt;"), URL = "&lt;&lt;Product Detail URL&gt;&gt;" });
        ///         oPayment.Send.Items.Add(new Item() { Name = "&lt;&lt;Product Name&gt;&gt;", Price = Decimal.Parse("&lt;&lt;Unit Price&gt;&gt;"), Currency = "&lt;&lt;Currency&gt;&gt;", Quantity = Int32.Parse("&lt;&lt;Quantity&gt;&gt;"), URL = "&lt;&lt;Product Detail URL&gt;&gt;" });
        ///         oPayment.Send.Items.Add(new Item() { Name = "&lt;&lt;Product Name&gt;&gt;", Price = Decimal.Parse("&lt;&lt;Unit Price&gt;&gt;"), Currency = "&lt;&lt;Currency&gt;&gt;", Quantity = Int32.Parse("&lt;&lt;Quantity&gt;&gt;"), URL = "&lt;&lt;Product Detail URL&gt;&gt;" });
        ///
        ///         enErrors.AddRange(oPayment.CheckOut("newForm"));
        ///     }
        /// }
        /// catch (Exception ex)
        /// {
        ///     // 例外錯誤處理。
        ///     enErrors.Add(ex.Message);
        /// }
        /// finally
        /// {
        ///     // 顯示錯誤訊息。
        ///     if (enErrors.Count() &gt; 0)
        ///         ScriptManager.RegisterStartupScript(this, typeof(Page), "_MESSAGE", String.Format("alert(\"{0}\");", String.Join("\\r\\n", enErrors)), true);
        /// }
        /// </code>
        /// </example>
        public IEnumerable<string> CheckOut(string target)
        {
            List<string> errList = new List<string>();
            // 驗證服務參數。
            errList.AddRange(ServerValidator.Validate(this));
            // 驗證基本參數。
            errList.AddRange(ServerValidator.Validate(this.Send));

            if (errList.Count == 0)
            {
                // 清除畫面控制項。
                this.ClearPageControls();
                // 產生傳遞的參數與檢查碼。
                string szParameters = String.Empty;
                string szCheckMacValue = String.Empty;

                if (ServiceMethod == HttpMethod.HttpPOST)
                {
                    szParameters += this.RenderControlAndParamenter("ClientBackURL", this.Send.ClientBackURL);
                    szParameters += this.RenderControlAndParamenter("ConsiderHour", this.Send.ConsiderHour);
                    szParameters += this.RenderControlAndParamenter("CreditInstallment", this.Send.CreditInstallment);
                    szParameters += this.RenderControlAndParamenter("Currency", this.Send.Currency);
                    szParameters += this.RenderControlAndParamenter("EncodeChartset", this.Send.EncodeChartset);
                    szParameters += this.RenderControlAndParamenter("InstallmentAmount", this.Send.InstallmentAmount);
                    szParameters += this.RenderControlAndParamenter("ItemName", this.Send.ItemName);
                    szParameters += this.RenderControlAndParamenter("ItemURL", this.Send.ItemURL);
                    szParameters += this.RenderControlAndParamenter("MerchantID", this.MerchantID);
                    szParameters += this.RenderControlAndParamenter("MerchantTradeDate", this.Send.MerchantTradeDate);
                    szParameters += this.RenderControlAndParamenter("MerchantTradeNo", this.Send.MerchantTradeNo);
                    szParameters += this.RenderControlAndParamenter("PaymentType", this.Send.PaymentType);
                    szParameters += this.RenderControlAndParamenter("Redeem", (this.Send.Redeem ? "Y" : String.Empty));
                    szParameters += this.RenderControlAndParamenter("Remark", this.Send.Remark);
                    szParameters += this.RenderControlAndParamenter("ReturnURL", this.Send.ReturnURL);
                    szParameters += this.RenderControlAndParamenter("ShippingDate", this.Send.ShippingDate);
                    szParameters += this.RenderControlAndParamenter("TotalAmount", this.Send.TotalAmount);
                    szParameters += this.RenderControlAndParamenter("TradeDesc", this.Send.TradeDesc);
                    szParameters += this.RenderControlAndParamenter("UseAllpayAddress", (this.Send.UseAllpayAddress ? "1" : "0"));
                    // 產生檢查碼。
                    szCheckMacValue = this.BuildCheckMacValue(szParameters);
                    // 繪製 MD5 檢查碼控制項。
                    szParameters += this.RenderControlAndParamenter("CheckMacValue", szCheckMacValue);
                    // 紀錄記錄檔
                    Logger.WriteLine(String.Format("INFO   {0}  OUTPUT  AllInEscrow.CheckOut: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szParameters));

                    if (null != this.CurrentPage)
                    {
                        HtmlGenericControl htmJQuery = new HtmlGenericControl("script");

                        htmJQuery.Attributes.Add("type", "text/javascript");
                        htmJQuery.Attributes.Add("src", this.CurrentPage.ClientScript.GetWebResourceUrl(this.GetType(), "ECPay.Payment.Integration.Resources.jquery-1.4.1.min.js"));

                        HtmlGenericControl htmScript = new HtmlGenericControl("script");

                        htmScript.Attributes.Add("type", "text/javascript");
                        htmScript.InnerHtml = String.Empty;
                        htmScript.InnerHtml += "\r\n//<![CDATA[";
                        htmScript.InnerHtml += "\r\n    $(document).ready(function () {";
                        htmScript.InnerHtml += "\r\n        $(\".__parameter\").each(function (i) {";
                        htmScript.InnerHtml += "\r\n            this.name = this.id;";
                        htmScript.InnerHtml += "\r\n        });";
                        htmScript.InnerHtml += "\r\n";
                        htmScript.InnerHtml += "\r\n        $(\"input[type=hidden]:not(.__parameter)\").each(function (i) {";
                        htmScript.InnerHtml += "\r\n            $(this).remove();";
                        htmScript.InnerHtml += "\r\n        });";
                        htmScript.InnerHtml += "\r\n";
                        htmScript.InnerHtml += String.Format("\r\n        $(\"form\").attr(\"target\", \"{0}\");", target);
                        htmScript.InnerHtml += "\r\n        $(\"form\").submit();";
                        htmScript.InnerHtml += "\r\n    });";
                        htmScript.InnerHtml += "\r\n//]]>";
                        htmScript.InnerHtml += "\r\n";

                        this.CurrentPage.Header.Controls.Clear();
                        this.CurrentPage.Header.Controls.Add(htmJQuery);
                        this.CurrentPage.Header.Controls.Add(htmScript);
                        this.CurrentPage.Form.Action = this.ServiceURL;
                    }
                }
                else
                {
                    errList.Add("No service for HttpGET, ServerPOST and HttpSOAP.");
                }
            }

            return errList;
        }
        /// <summary>
        /// 取回訂單產生結果。
        /// </summary>
        /// <param name="feedback">回傳的參數資料。</param>
        /// <returns>錯誤訊息的集合字串。</returns>
        /// <remarks>
        /// 該方法對應到產生訂單方法中 AllInEscrow.Send.ReturnURL 的位址，
        /// 需於 Web 站台或 Web 專案中指定的位址(頁面)撰寫以下程式碼才能正常取回處理結果。
        /// </remarks>
        /// <example>
        /// <code>
        /// /*
        ///  *  接收訂單資料產生完成的範例程式碼。
        ///  */
        /// List&lt;string&gt; enErrors = new List&lt;string&gt;();
        /// Hashtable htFeedback = null;
        ///
        /// try
        /// {
        ///     using (AllInEscrow oPayment = new AllInEscrow())
        ///     {
        ///         oPayment.HashKey = "&lt;&lt;Hash Key&gt;&gt;";
        ///         oPayment.HashIV = "&lt;&lt;Hash IV&gt;&gt;";
        ///         /* 取回付款結果 */
        ///         enErrors.AddRange(oPayment.CheckOutFeedback(ref htFeedback));
        ///     }
        ///     // 取回所有資料
        ///     if (enErrors.Count() == 0)
        ///     {
        ///         /* 支付後的回傳的基本參數 */
        ///         string szMerchantID = String.Empty;
        ///         string szMerchantTradeNo = String.Empty;
        ///         string szRtnCode = String.Empty;
        ///         string szRtnMsg = String.Empty;
        ///         string szTradeNo = String.Empty;
        ///         string szTradeAmt = String.Empty;
        ///         string szPaymentDate = String.Empty;
        ///         string szPaymentType = String.Empty;
        ///         string szPaymentTypeChargeFee = String.Empty;
        ///         string szTradeDate = String.Empty;
        ///         string szUserAddress = String.Empty; // Only for Alipay
        ///         string szSimulatePaid = String.Empty;
        ///         // 取得資料於畫面
        ///         foreach (string szKey in htFeedback.Keys)
        ///         {
        ///             switch (szKey)
        ///             {
        ///                 /* 支付後的回傳的基本參數 */
        ///                 case "MerchantID": szMerchantID = this.Request.Form[szKey]; break;
        ///                 case "MerchantTradeNo": szMerchantTradeNo = this.Request.Form[szKey]; break;
        ///                 case "PaymentDate": szPaymentDate = this.Request.Form[szKey]; break;
        ///                 case "PaymentType": szPaymentType = this.Request.Form[szKey]; break;
        ///                 case "PaymentTypeChargeFee": szPaymentTypeChargeFee = this.Request.Form[szKey]; break;
        ///                 case "RtnCode": szRtnCode = this.Request.Form[szKey]; break;
        ///                 case "RtnMsg": szRtnMsg = this.Request.Form[szKey]; break;
        ///                 case "SimulatePaid": szSimulatePaid = this.Request.Form[szKey]; break;
        ///                 case "TradeAmt": szTradeAmt = this.Request.Form[szKey]; break;
        ///                 case "TradeDate": szTradeDate = this.Request.Form[szKey]; break;
        ///                 case "TradeNo": szTradeNo = this.Request.Form[szKey]; break;
        ///                 case "UserAddress": szUserAddress = this.Request.Form[szKey]; break;
        ///                 default: break;
        ///             }
        ///         }
        ///         // 其他資料處理。
        ///         ......
        ///     }
        /// }
        /// catch (Exception ex)
        /// {
        ///     enErrors.Add(ex.Message);
        /// }
        /// finally
        /// {
        ///     this.Response.Clear();
        ///     // 回覆成功訊息。
        ///     if (enErrors.Count() == 0)
        ///         this.Response.Write("1|OK");
        ///     // 回覆錯誤訊息。
        ///     else
        ///         this.Response.Write(String.Format("0|{0}", String.Join("\\r\\n", enErrors)));
        ///     this.Response.Flush();
        ///     this.Response.End();
        /// }
        /// </code>
        /// </example>
        public IEnumerable<string> CheckOutFeedback(ref Hashtable feedback)
        {
            string szParameters = String.Empty;
            string szCheckMacValue = String.Empty;

            List<string> errList = new List<string>();

            if (null == feedback) feedback = new Hashtable();

            if (null != this.CurrentPage)
            {
                Array.Sort(this.CurrentPage.Request.Form.AllKeys);

                foreach (string szKey in this.CurrentPage.Request.Form.AllKeys)
                {
                    string szValue = this.CurrentPage.Request.Form[szKey];

                    if (szKey != "CheckMacValue")
                    {
                        feedback.Add(szKey, szValue);

                        szParameters += String.Format("&{0}={1}", szKey, szValue);
                    }
                    else
                    {
                        szCheckMacValue = szValue;
                    }
                }
                // 紀錄記錄檔
                Logger.WriteLine(String.Format("INFO   {0}  INPUT   AllInEscrow.CheckOutFeedback: {1}&CheckMacValue={2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szParameters, szCheckMacValue));
                // 比對驗證檢查碼。
                errList.AddRange(this.CompareCheckMacValue(szParameters, szCheckMacValue));
            }

            return errList;
        }
        /// <summary>
        /// 出貨通知(ServerPOST)。
        /// </summary>
        /// <param name="feedback">回傳的參數資料。</param>
        /// <returns>錯誤訊息的集合字串。</returns>
        /// <remarks>
        /// 該方法會以 Server POST 的方式送出參數至 AllInEscrow.ServiceURL 位址，
        /// 網路需申請 ACL(Access Control List) 才能正常送出出貨通知資料的給 AllPay。
        /// </remarks>
        /// <example>
        /// <code>
        /// /*
        ///  *  出貨通知的範例程式碼。
        ///  */
        /// List&lt;string&gt; enErrors = new List&lt;string&gt;();
        /// Hashtable htFeedback = null;
        ///
        /// try
        /// {
        ///     using (AllInEscrow oPayment = new AllInEscrow())
        ///     {
        ///         /* 服務參數 */
        ///         oPayment.ServiceMethod = HttpMethod.ServerPOST;
        ///         oPayment.ServiceURL = "&lt;&lt;AllPay Service URL&gt;&gt;";
        ///         oPayment.HashKey = "&lt;&lt;Hash Key&gt;&gt;";
        ///         oPayment.HashIV = "&lt;&lt;Hash IV&gt;&gt;";
        ///         oPayment.MerchantID = "&lt;&lt;Merchant ID&gt;&gt;";
        ///         /* 基本參數 */
        ///         oPayment.Delivery.MerchantTradeNo = "&lt;&lt;Merchant Trade No&gt;&gt;";
        ///         oPayment.Delivery.TradeNo = "&lt;&lt;Trade No&gt;&gt;";
        ///         oPayment.Delivery.ShippingState = (ShippingState)Enum.Parse(typeof(ShippingState), "&lt;&lt;Shipping State&gt;&gt;");
        ///         oPayment.Delivery.ShippingDate = DateTime.Parse("&lt;&lt;Shipping Date&gt;&gt;");
        ///         oPayment.Delivery.EncodeChartset = "&lt;&lt;Encode Chartset&gt;&gt;";
        ///         oPayment.Delivery.Remark = "&lt;&lt;Remark&gt;&gt;";
        ///
        ///         enErrors.AddRange(oPayment.DeliveryNotify(ref htFeedback));
        ///     }
        ///
        ///     if (enErrors.Count() == 0)
        ///     {
        ///         // 其他資料處理。
        ///         ......
        ///     }
        /// }
        /// catch (Exception ex)
        /// {
        ///     // 例外錯誤處理。
        ///     enErrors.Add(ex.Message);
        /// }
        /// finally
        /// {
        ///     // 顯示錯誤訊息。
        ///     if (enErrors.Count() > 0)
        ///         ScriptManager.RegisterStartupScript(this, typeof(Page), "_MESSAGE", String.Format("alert(\"{0}\");", String.Join("\\r\\n", enErrors)), true);
        /// }
        /// </code>
        /// </example>
        public IEnumerable<string> DeliveryNotify(ref Hashtable feedback)
        {
            string szFeedback = String.Empty;
            string szParameters = String.Empty;
            string szCheckMacValue = String.Empty;

            List<string> errList = new List<string>();

            if (null == feedback) feedback = new Hashtable();
            // 驗證服務參數。
            errList.AddRange(ServerValidator.Validate(this));
            // 驗證基本參數。
            errList.AddRange(ServerValidator.Validate(this.Delivery));

            if (errList.Count == 0)
            {
                // 產生傳遞的參數與檢查碼。
                szParameters += this.RenderControlAndParamenter("EncodeChartset", this.Delivery.EncodeChartset);
                szParameters += this.RenderControlAndParamenter("MerchantID", this.MerchantID);
                szParameters += this.RenderControlAndParamenter("MerchantTradeNo", this.Delivery.MerchantTradeNo);
                szParameters += this.RenderControlAndParamenter("Remark", this.Delivery.Remark);
                szParameters += this.RenderControlAndParamenter("ShippingDate", this.Delivery.ShippingDate);
                szParameters += this.RenderControlAndParamenter("ShippingState", (Int32)this.Delivery.ShippingState);
                szParameters += this.RenderControlAndParamenter("TradeNo", this.Delivery.TradeNo);
                // 產生檢查碼。
                szCheckMacValue = this.BuildCheckMacValue(szParameters);
                // 繪製 MD5 檢查碼控制項。
                szParameters += this.RenderControlAndParamenter("CheckMacValue", szCheckMacValue);
                // 紀錄記錄檔
                Logger.WriteLine(String.Format("INFO   {0}  OUTPUT  AllInEscrow.DeliveryNotify: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szParameters));

                if (ServiceMethod == HttpMethod.ServerPOST)
                {
                    szFeedback = this.ServerPost(szParameters);
                }
                else
                {
                    errList.Add("No service for HttpGET, HttpPOST and HttpSOAP.");
                }
                // 紀錄記錄檔
                Logger.WriteLine(String.Format("INFO   {0}  INPUT   AllInEscrow.DeliveryNotify: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szFeedback));
                // 重新整理取回的參數。
                string[] saData = szFeedback.Split(new char[] { '|' });

                if (saData.Length == 2)
                {
                    string szCode = saData[0];
                    string szMessage = saData[1];

                    feedback.Add("RtnCode", szCode);
                    feedback.Add("RtnMsg", szMessage);
                }

                if (szFeedback != "1|OK")
                {
                    errList.Add(szFeedback.Replace("|", ": "));
                }
            }

            return errList;
        }
        /// <summary>
        /// 接收 AllPay 通知商品異常/取消訂單資料。
        /// </summary>
        /// <param name="feedback">回傳的參數資料。</param>
        /// <returns>錯誤訊息的集合字串。</returns>
        /// <remarks>
        /// 該方法使用前，請於 AllPay 廠商後台設定位址(後台帳號管理 > 廠商基本資料查詢)，
        /// AllPay 才能將商品異常/取消訂單的通知送到指定的位址；
        /// 需於 Web 站台或 Web 專案中指定的位址(頁面)撰寫以下程式碼才能正常收到通知結果。
        /// </remarks>
        /// <example>
        /// <code>
        /// /*
        ///  *  接收商品異常/取消訂單的範例程式碼。
        ///  */
        /// List&lt;string&gt; enErrors = new List&lt;string&gt;();
        /// Hashtable htFeedback = null;
        ///
        /// try
        /// {
        ///     using (AllInEscrow oPayment = new AllInEscrow())
        ///     {
        ///         oPayment.HashKey = "&lt;&lt;Hash Key&gt;&gt;";
        ///         oPayment.HashIV = "&lt;&lt;Hash IV&gt;&gt;";
        ///         /* 取回付款結果 */
        ///         enErrors.AddRange(oPayment.CheckOutFeedback(ref htFeedback));
        ///     }
        ///     // 取回所有資料
        ///     if (enErrors.Count() == 0)
        ///     {
        ///         /* 收到通知後取回的基本參數 */
        ///         string szMerchantID = String.Empty;
        ///         string szMerchantTradeNo = String.Empty;
        ///         string szTradeNo = String.Empty;
        ///         string szInfoType = String.Empty;
        ///         string szReason = String.Empty;
        ///         string szRemark = String.Empty;
        ///         // 取得資料於畫面
        ///         foreach (string szKey in htFeedback.Keys)
        ///         {
        ///             switch (szKey)
        ///             {
        ///                 /* 收到通知後取回的基本參數 */
        ///                 case "MerchantID": szMerchantID = this.Request.Form[szKey]; break;
        ///                 case "MerchantTradeNo": szMerchantTradeNo = this.Request.Form[szKey]; break;
        ///                 case "InfoType": szInfoType = this.Request.Form[szKey]; break;
        ///                 case "Reason": szReason = this.Request.Form[szKey]; break;
        ///                 case "Remark": szRemark = this.Request.Form[szKey]; break;
        ///                 case "TradeNo": szTradeNo = this.Request.Form[szKey]; break;
        ///                 default: break;
        ///             }
        ///         }
        ///         // 其他資料處理。
        ///         ......
        ///     }
        /// }
        /// catch (Exception ex)
        /// {
        ///     enErrors.Add(ex.Message);
        /// }
        /// finally
        /// {
        ///     this.Response.Clear();
        ///     // 回覆成功訊息。
        ///     if (enErrors.Count() == 0)
        ///         this.Response.Write("1|OK");
        ///     // 回覆錯誤訊息。
        ///     else
        ///         this.Response.Write(String.Format("0|{0}", String.Join("\\r\\n", enErrors)));
        ///     this.Response.Flush();
        ///     this.Response.End();
        /// }
        /// </code>
        /// </example>
        public IEnumerable<string> OrderChangeFeedback(ref Hashtable feedback)
        {
            string szParameters = String.Empty;
            string szCheckMacValue = String.Empty;

            List<string> errList = new List<string>();

            if (null == feedback) feedback = new Hashtable();

            if (null != this.CurrentPage)
            {
                Array.Sort(this.CurrentPage.Request.Form.AllKeys);

                foreach (string szKey in this.CurrentPage.Request.Form.AllKeys)
                {
                    string szValue = this.CurrentPage.Request.Form[szKey];

                    if (szKey != "CheckMacValue")
                    {
                        feedback.Add(szKey, szValue);

                        szParameters += String.Format("&{0}={1}", szKey, szValue);
                    }
                    else
                    {
                        szCheckMacValue = szValue;
                    }
                }
                // 紀錄記錄檔
                Logger.WriteLine(String.Format("INFO   {0}  INPUT   AllInEscrow.OrderChangeFeedback: {1}&CheckMacValue={2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szParameters, szCheckMacValue));
                // 比對驗證檢查碼。
                errList.AddRange(this.CompareCheckMacValue(szParameters, szCheckMacValue));
            }

            return errList;
        }
        /// <summary>
        /// 商品異常/取消訂單處理結果回覆(ServerPOST)。
        /// </summary>
        /// <param name="feedback">回傳的參數資料。</param>
        /// <returns>錯誤訊息的集合字串。</returns>
        /// <remarks>
        /// 該方法會以 Server POST 的方式送出參數至 AllInEscrow.ServiceURL 位址，
        /// 網路需申請 ACL(Access Control List) 才能正常送出商品異常/取消訂單處理結果資料給 AllPay。
        /// </remarks>
        /// <example>
        /// <code>
        /// /*
        ///  * 商品異常/取消訂單處理結果回覆的範例程式碼。
        ///  */
        /// List&lt;string&gt; enErrors = new List&lt;string&gt;();
        /// Hashtable htFeedback = null;
        ///
        /// try
        /// {
        ///     using (AllInEscrow oPayment = new AllInEscrow())
        ///     {
        ///         /* 服務參數 */
        ///         oPayment.ServiceMethod = HttpMethod.ServerPOST;
        ///         oPayment.ServiceURL = "&lt;&lt;AllPay Service URL&gt;&gt;";
        ///         oPayment.HashKey = "&lt;&lt;Hash Key&gt;&gt;";
        ///         oPayment.HashIV = "&lt;&lt;Hash IV&gt;&gt;";
        ///         oPayment.MerchantID = "&lt;&lt;Merchant ID&gt;&gt;";
        ///         /* 基本參數 */
        ///         oPayment.Change.MerchantTradeNo = "&lt;&lt;Merchant Trade No&gt;&gt;";
        ///         oPayment.Change.TradeNo = "&lt;&lt;Trade No&gt;&gt;";
        ///         oPayment.Change.InfoType = (NotifiedType)Enum.Parse(typeof(NotifiedType), "&lt;&lt;Info Type&gt;&gt;");
        ///         oPayment.Change.RefundAmount = (String.IsNullOrEmpty("&lt;&lt;Refund Amount&gt;&gt;") ? null : (Decimal?)Decimal.Parse("&lt;&lt;Refund Amount&gt;&gt;"));
        ///         oPayment.Change.Remark = "&lt;&lt;Remark&gt;&gt;";
        ///
        ///         enErrors.AddRange(oPayment.OrderChangeNotify(ref htFeedback));
        ///     }
        ///
        ///     if (enErrors.Count() == 0)
        ///     {
        ///         // 其他資料處理。
        ///         ......
        ///     }
        /// }
        /// catch (Exception ex)
        /// {
        ///     enErrors.Add(ex.Message);
        /// }
        /// finally
        /// {
        ///     if (enErrors.Count() > 0)
        ///     {
        ///         ScriptManager.RegisterStartupScript(this, typeof(Page), "_MESSAGE", String.Format("alert(\"{0}\");", String.Join("\\r\\n", enErrors)), true);
        ///     }
        /// }
        /// </code>
        /// </example>
        public IEnumerable<string> OrderChangeNotify(ref Hashtable feedback)
        {
            string szFeedback = String.Empty;
            string szParameters = String.Empty;
            string szCheckMacValue = String.Empty;

            List<string> errList = new List<string>();

            if (null == feedback) feedback = new Hashtable();
            // 驗證服務參數。
            errList.AddRange(ServerValidator.Validate(this));
            // 驗證基本參數。
            errList.AddRange(ServerValidator.Validate(this.Change));

            if (errList.Count == 0)
            {
                // 產生傳遞的參數與檢查碼。
                szParameters += this.RenderControlAndParamenter("InfoType", (Int32)this.Change.InfoType);
                szParameters += this.RenderControlAndParamenter("MerchantID", this.MerchantID);
                szParameters += this.RenderControlAndParamenter("MerchantTradeNo", this.Change.MerchantTradeNo);
                szParameters += this.RenderControlAndParamenter("RefundAmount", this.Change.RefundAmount);
                szParameters += this.RenderControlAndParamenter("Remark", this.Change.Remark);
                szParameters += this.RenderControlAndParamenter("TradeNo", this.Change.TradeNo);
                // 產生檢查碼。
                szCheckMacValue = this.BuildCheckMacValue(szParameters);
                // 繪製 MD5 檢查碼控制項。
                szParameters += this.RenderControlAndParamenter("CheckMacValue", szCheckMacValue);
                // 紀錄記錄檔
                Logger.WriteLine(String.Format("INFO   {0}  OUTPUT  AllInEscrow.OrderChangeNotify: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szParameters));

                if (ServiceMethod == HttpMethod.ServerPOST)
                {
                    szFeedback = this.ServerPost(szParameters);
                }
                else
                {
                    errList.Add("No service for HttpGET, HttpPOST and HttpSOAP.");
                }
                // 紀錄記錄檔
                Logger.WriteLine(String.Format("INFO   {0}  INPUT   AllInEscrow.OrderChangeNotify: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szFeedback));
                // 重新整理取回的參數。
                if (szFeedback == "1|OK")
                {
                    string[] saData = szFeedback.Split(new char[] { '|' });

                    string szCode = saData[0];
                    string szMessage = saData[1];

                    feedback.Add(szCode, szMessage);
                }
                else
                {
                    errList.Add(szFeedback.Replace("|", ": "));
                }
            }

            return errList;
        }
        /// <summary>
        /// 接收 AllPay 通知訂單履約完成資料
        /// </summary>
        /// <param name="feedback">回傳的參數資料。</param>
        /// <returns>錯誤訊息的集合字串。</returns>
        /// <remarks>
        /// 該方法使用前，請於 AllPay 廠商後台設定位址(後台帳號管理 > 廠商基本資料查詢)，
        /// AllPay 才能將訂單履約完成資料的通知送到指定的位址；
        /// 需於 Web 站台或 Web 專案中指定的位址(頁面)撰寫以下程式碼才能正常收到通知結果。
        /// </remarks>
        /// <example>
        /// <code>
        /// /*
        ///  *  接收訂單履約完成通知資料的範例程式碼。
        ///  */
        /// List&lt;string&gt; enErrors = new List&lt;string&gt;();
        /// Hashtable htFeedback = null;
        ///
        /// try
        /// {
        ///     using (AllInEscrow oPayment = new AllInEscrow())
        ///     {
        ///         oPayment.HashKey = "&lt;&lt;Hash Key&gt;&gt;";
        ///         oPayment.HashIV = "&lt;&lt;Hash IV&gt;&gt;";
        ///         /* 取回付款結果 */
        ///         enErrors.AddRange(oPayment.CheckOutFeedback(ref htFeedback));
        ///     }
        ///     // 取回所有資料
        ///     if (enErrors.Count() == 0)
        ///     {
        ///         /* 收到通知後取回的基本參數 */
        ///         string szMerchantID = String.Empty;
        ///         string szMerchantTradeNo = String.Empty;
        ///         string szTradeNo = String.Empty;
        ///         string szTradeStatus = String.Empty;
        ///         string szTotalAmount = String.Empty;
        ///         string szReundAmount = String.Empty;
        ///         string szEscrowCompletedDate = String.Empty;
        ///         // 取得資料於畫面
        ///         foreach (string szKey in htFeedback.Keys)
        ///         {
        ///             switch (szKey)
        ///             {
        ///                 /* 收到通知後取回的基本參數 */
        ///                 case "MerchantID": szMerchantID = this.Request.Form[szKey]; break;
        ///                 case "MerchantTradeNo": szMerchantTradeNo = this.Request.Form[szKey]; break;
        ///                 case "TradeStatus": szTradeStatus = this.Request.Form[szKey]; break;
        ///                 case "TotalAmount": szTotalAmount = this.Request.Form[szKey]; break;
        ///                 case "ReundAmount": szReundAmount = this.Request.Form[szKey]; break;
        ///                 case "TradeNo": szTradeNo = this.Request.Form[szKey]; break;
        ///                 case "EscrowCompletedDate": szEscrowCompletedDate = this.Request.Form[szKey]; break;
        ///                 default: break;
        ///             }
        ///         }
        ///         // 其他資料處理。
        ///         ......
        ///     }
        /// }
        /// catch (Exception ex)
        /// {
        ///     enErrors.Add(ex.Message);
        /// }
        /// finally
        /// {
        ///     this.Response.Clear();
        ///     // 回覆成功訊息。
        ///     if (enErrors.Count() == 0)
        ///         this.Response.Write("1|OK");
        ///     // 回覆錯誤訊息。
        ///     else
        ///         this.Response.Write(String.Format("0|{0}", String.Join("\\r\\n", enErrors)));
        ///     this.Response.Flush();
        ///     this.Response.End();
        /// }
        /// </code>
        /// </example>
        public IEnumerable<string> CompletedFeedback(ref Hashtable feedback)
        {
            string szParameters = String.Empty;
            string szCheckMacValue = String.Empty;

            List<string> errList = new List<string>();

            if (null == feedback) feedback = new Hashtable();

            if (null != this.CurrentPage)
            {
                Array.Sort(this.CurrentPage.Request.Form.AllKeys);

                foreach (string szKey in this.CurrentPage.Request.Form.AllKeys)
                {
                    string szValue = this.CurrentPage.Request.Form[szKey];

                    if (szKey != "CheckMacValue")
                    {
                        feedback.Add(szKey, szValue);

                        szParameters += String.Format("&{0}={1}", szKey, szValue);
                    }
                    else
                    {
                        szCheckMacValue = szValue;
                    }
                }
                // 紀錄記錄檔
                Logger.WriteLine(String.Format("INFO   {0}  INPUT   AllInEscrow.CompletedFeedback: {1}&CheckMacValue={2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szParameters, szCheckMacValue));
                // 比對驗證檢查碼。
                errList.AddRange(this.CompareCheckMacValue(szParameters, szCheckMacValue));
            }

            return errList;
        }
        /// <summary>
        /// 訂單查詢。
        /// </summary>
        /// <param name="feedback">回傳的參數資料。</param>
        /// <returns>錯誤訊息的集合字串。</returns>
        /// <remarks>
        /// 該方法會以 Server POST 或 Http SOAP(Web Service) 的方式送出參數至 AllInOne.ServiceURL 位址，
        /// 網路需申請 ACL(Access Control List) 才能正常請求訂單資料的查詢。
        /// </remarks>
        /// <example>
        /// <code>
        /// /*
        ///  *  查詢訂單的範例程式碼。
        ///  */
        /// List&lt;string&gt; enErrors = new List&lt;string&gt;();
        /// Hashtable htFeedback = null;
        ///
        /// try
        /// {
        ///     using (AllInOne oPayment = new AllInOne())
        ///     {
        ///         /* 服務參數 */
        ///         oPayment.ServiceMethod = HttpMethod.ServerPOST; // 或使用 HttpMethod.HttpSOAP;
        ///         oPayment.ServiceURL = "&lt;&lt;AllPay Service URL&gt;&gt;";
        ///         oPayment.HashKey = "&lt;&lt;Hash Key&gt;&gt;";
        ///         oPayment.HashIV = "&lt;&lt;Hash IV&gt;&gt;";
        ///         oPayment.MerchantID = "&lt;&lt;Merchant ID&gt;&gt;";
        ///         /* 基本參數 */
        ///         oPayment.Query.MerchantTradeNo = "&lt;&lt;Merchant Trade No&gt;&gt;";
        ///         /* 查詢訂單 */
        ///         enErrors.AddRange(oPayment.QueryTradeInfo(ref htFeedback));
        ///     }
        ///     // 取回所有資料
        ///     if (enErrors.Count() == 0)
        ///     {
        ///         /* 查詢後的回傳的基本參數 */
        ///         string szMerchantID = String.Empty;
        ///         string szMerchantTradeNo = String.Empty;
        ///         string szTradeNo = String.Empty;
        ///         string szTradeAmt = String.Empty;
        ///         string szPaymentDate = String.Empty;
        ///         string szPaymentType = String.Empty;
        ///         string szHandlingCharge = String.Empty;
        ///         string szPaymentTypeChargeFee = String.Empty;
        ///         string szTradeDate = String.Empty;
        ///         string szTradeStatus = String.Empty;
        ///         string szItemName = String.Empty;
        ///         string szConsiderHour = String.Empty;
        ///         string szEscrowCompleted = String.Empty;
        ///         // 取得資料於畫面
        ///         foreach (string szKey in htFeedback.Keys)
        ///         {
        ///             switch (szKey)
        ///             {
        ///                 /* 查詢後的回傳的基本參數 */
        ///                 case "MerchantID": szMerchantID = this.Request.Form[szKey]; break;
        ///                 case "MerchantTradeNo": szMerchantTradeNo = this.Request.Form[szKey]; break;
        ///                 case "TradeNo": szTradeNo = this.Request.Form[szKey]; break;
        ///                 case "TradeAmt": szTradeAmt = this.Request.Form[szKey]; break;
        ///                 case "PaymentDate": szPaymentDate = this.Request.Form[szKey]; break;
        ///                 case "PaymentType": szPaymentType = this.Request.Form[szKey]; break;
        ///                 case "HandlingCharge": szHandlingCharge = this.Request.Form[szKey]; break;
        ///                 case "PaymentTypeChargeFee": szPaymentTypeChargeFee = this.Request.Form[szKey]; break;
        ///                 case "TradeDate": szTradeDate = this.Request.Form[szKey]; break;
        ///                 case "TradeStatus": szTradeStatus = this.Request.Form[szKey]; break;
        ///                 case "ItemName": szItemName = this.Request.Form[szKey]; break;
        ///                 case "ConsiderHour": szConsiderHour = this.Request.Form[szKey]; break;
        ///                 case "EscrowCompleted": szEscrowCompleted = this.Request.Form[szKey]; break;
        ///                 default: break;
        ///             }
        ///         }
        ///         // 其他資料處理。
        ///         ......
        ///     }
        /// }
        /// catch (Exception ex)
        /// {
        ///     // 例外錯誤處理。
        ///     enErrors.Add(ex.Message);
        /// }
        /// finally
        /// {
        ///     // 顯示錯誤訊息。
        ///     if (enErrors.Count() &gt; 0)
        ///         ScriptManager.RegisterStartupScript(this, typeof(Page), "_MESSAGE", String.Format("alert(\"{0}\");", String.Join("\\r\\n", enErrors)), true);
        /// }
        /// </code>
        /// </example>
        public IEnumerable<string> QueryTradeInfo(ref Hashtable feedback)
        {
            string szFeedback = String.Empty;
            string szParameters = String.Empty;
            string szCheckMacValue = String.Empty;

            List<string> errList = new List<string>();

            if (null == feedback) feedback = new Hashtable();
            // 驗證服務參數。
            errList.AddRange(ServerValidator.Validate(this));
            // 驗證基本參數。
            errList.AddRange(ServerValidator.Validate(this.Query));

            if (errList.Count == 0)
            {
                // 產生畫面控制項與傳遞參數。
                szParameters += this.BuildParamenter("MerchantID", this.MerchantID);
                szParameters += this.BuildParamenter("MerchantTradeNo", this.Query.MerchantTradeNo);
                szParameters += this.BuildParamenter("TimeStamp", this.Query.TimeStamp);
                // 產生檢查碼。
                szCheckMacValue = this.BuildCheckMacValue(szParameters);
                // 繪製 MD5 檢查碼控制項。
                szParameters += this.BuildParamenter("CheckMacValue", szCheckMacValue);
                szParameters = szParameters.Substring(1);
                // 紀錄記錄檔
                Logger.WriteLine(String.Format("INFO   {0}  OUTPUT  AllInEscrow.QueryTradeInfo: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szParameters));
                // 自遠端伺服器取得資料。
                if (this.ServiceMethod == HttpMethod.ServerPOST)
                {
                    szFeedback = this.ServerPost(szParameters);
                }
                else if (this.ServiceMethod == HttpMethod.HttpSOAP)
                {
                    IAllPayService svcTrade = ChannelProvider.CreateChannel<IAllPayService>(this.ServiceURL);

                    szFeedback = svcTrade.QueryTradeInfo(this.MerchantID, this.Query.MerchantTradeNo, this.Query.TimeStamp, szCheckMacValue);
                }
                else
                {
                    errList.Add("No service for HttpPOST and HttpGET.");
                }
                // 紀錄記錄檔
                Logger.WriteLine(String.Format("INFO   {0}  INPUT   AllInEscrow.QueryTradeInfo: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szFeedback));
                // 重新整理取回的參數。
                if (!String.IsNullOrEmpty(szFeedback))
                {
                    szParameters = String.Empty;
                    szCheckMacValue = String.Empty;

                    foreach (string szData in szFeedback.Split(new char[] { '&' }))
                    {
                        if (!String.IsNullOrEmpty(szData))
                        {
                            string[] saData = szData.Split(new char[] { '=' });

                            string szKey = saData[0];
                            string szValue = saData[1];

                            if (szKey != "CheckMacValue")
                            {
                                szParameters += String.Format("&{0}={1}", szKey, szValue);
                                feedback.Add(szKey, szValue);
                            }
                            else
                            {
                                szCheckMacValue = szValue;
                            }
                        }
                    }

                    if (String.IsNullOrEmpty(szCheckMacValue))
                    {
                        errList.Add(String.Format("ErrorCode: {0}", feedback["TradeStatus"]));
                    }
                    else
                    {
                        // 比對驗證檢查碼。
                        errList.AddRange(this.CompareCheckMacValue(szParameters, szCheckMacValue));
                    }
                }
            }

            return errList;
        }

        #endregion

        #region - 私用的函數方法

        /// <summary>
        /// 產生檢查碼。
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string BuildCheckMacValue(string parameters)
        {
            string szCheckMacValue = String.Empty;
            // 產生檢查碼。
            szCheckMacValue = String.Format("HashKey={0}{1}&HashIV={2}", this.HashKey, parameters, this.HashIV);
            szCheckMacValue = HttpUtility.UrlEncode(szCheckMacValue).ToLower();
            szCheckMacValue = MD5Encoder.Encrypt(szCheckMacValue);

            return szCheckMacValue;
        }
        /// <summary>
        /// 比對驗證檢查碼。
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="checkMacValue"></param>
        /// <returns></returns>
        private IEnumerable<string> CompareCheckMacValue(string parameters, string checkMacValue)
        {
            List<string> errList = new List<string>();

            if (!String.IsNullOrEmpty(checkMacValue))
            {
                // 產生檢查碼。
                string szConfirmMacValue = this.BuildCheckMacValue(parameters);
                // 比對檢查碼。
                if (checkMacValue != szConfirmMacValue) errList.Add("CheckMacValue verify fail.");
            }
            // 查無檢查碼時，拋出例外。
            else
            {
                if (String.IsNullOrEmpty(checkMacValue)) errList.Add("No CheckMacValue parameter.");
            }

            return errList;
        }
        /// <summary>
        /// 清除畫面上所有的控制項。
        /// </summary>
        private void ClearPageControls()
        {
            if (null != this.CurrentPage)
            {
                for (int i = this.CurrentPage.Form.Controls.Count; i > 0; i--)
                {
                    Control oControl = this.CurrentPage.Form.Controls[i - 1];

                    if (oControl.GetType().Name != "ScriptManager" && oControl.GetType().Name != "ToolkitScriptManager")
                    {
                        this.CurrentPage.Form.Controls.Remove(oControl);
                    }
                }
            }
        }
        /// <summary>
        /// 產生參數字串。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string BuildParamenter(string id, object value)
        {
            string szValue = String.Empty;
            string szParameter = String.Empty;

            if (null != value)
            {
                if (value.GetType().Equals(typeof(DateTime)))
                    szValue = ((DateTime)value).ToString("yyyy/MM/dd HH:mm:ss");
                else
                    szValue = value.ToString();
            }

            szParameter = String.Format("&{0}={1}", id, szValue);

            return szParameter;
        }
        /// <summary>
        /// 繪製控制項並產生參數字串。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string RenderControlAndParamenter(string id, object value)
        {
            string szValue = String.Empty;

            if (null != value)
            {
                if (value.GetType().Equals(typeof(DateTime)))
                    szValue = ((DateTime)value).ToString("yyyy/MM/dd HH:mm:ss");
                else
                    szValue = value.ToString();
            }

            if (null != this.CurrentPage)
            {
                HtmlInputHidden hideField = null;

                hideField = new HtmlInputHidden();
                hideField.ID = id;
                hideField.Value = szValue;
                hideField.Attributes["class"] = "__parameter";

                this.CurrentPage.Form.Controls.Add(hideField);
            }

            return this.BuildParamenter(id, value);
        }
        /// <summary>
        /// 伺服器端傳送參數請求資料。
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string ServerPost(string parameters)
        {
            string szResult = String.Empty;

            byte[] byContent = Encoding.UTF8.GetBytes(parameters);

            WebRequest webRequest = WebRequest.Create(this.ServiceURL);
            {
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Method = "POST";
                webRequest.ContentLength = byContent.Length;

                using (System.IO.Stream oStream = webRequest.GetRequestStream())
                {
                    oStream.Write(byContent, 0, byContent.Length); //Push it out there
                    oStream.Close();
                }

                WebResponse webResponse = webRequest.GetResponse();
                {
                    if (null != webResponse)
                    {
                        using (StreamReader oReader = new StreamReader(webResponse.GetResponseStream()))
                        {
                            szResult = oReader.ReadToEnd().Trim();
                        }
                    }

                    webResponse.Close();
                    webResponse = null;
                }

                webRequest = null;
            }

            return szResult;
        }

        #endregion

        #region - 釋放使用資源

        /// <summary>
        /// 執行與釋放 (Free)、釋放 (Release) 或重設 Unmanaged 資源相關聯之應用程式定義的工作。
        /// </summary>
        public void Dispose()
        {
            GC.Collect(); ;
        }

        #endregion
    }
}
