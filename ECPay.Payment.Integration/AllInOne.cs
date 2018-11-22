using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using ECPay.Payment.Integration.Helper;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 綠界科技 All In One 金流介接模組。
    /// </summary>
    public class AllInOne : AllInOneMetadata, IDisposable
    {
        #region - 屬性欄位成員

        private Page _currentPage;
        private HtmlHead _currentHead;
        private HtmlForm _currentForm;
        private HttpRequest _currentRequest;

        /// <summary>
        /// 取得或設定目前頁面。
        /// </summary>
        private Page CurrentPage
        {
            get
            {
                if (null == this._currentPage)
                    if (null != HttpContext.Current)
                        this._currentPage = HttpContext.Current.Handler as Page;

                return this._currentPage;
            }
            set
            {
                this._currentPage = value;
            }
        }
        /// <summary>
        /// 取得或設定目前表頭。
        /// </summary>
        private HtmlHead CurrentHead
        {
            get
            {
                if (null == this._currentHead)
                    this._currentHead = this.CurrentPage.Header;

                return this._currentHead;
            }
            set
            {
                this._currentHead = value;
            }
        }
        /// <summary>
        /// 取得或設定目前表單。
        /// </summary>
        private HtmlForm CurrentForm
        {
            get
            {
                if (null == this._currentForm)
                    this._currentForm = this.CurrentPage.Form;

                return this._currentForm;
            }
            set
            {
                this._currentForm = value;
            }
        }
        /// <summary>
        /// 取得或設定目前 HttpRequest 物件。
        /// </summary>
        private HttpRequest CurrentRequest
        {
            get
            {
                if (null != this.CurrentPage)
                    this._currentRequest = this.CurrentPage.Request;
                else if (null != HttpContext.Current)
                    this._currentRequest = HttpContext.Current.Request;

                return this._currentRequest;
            }
            set
            {
                this._currentRequest = value;
            }
        }

        #endregion

        #region - AIO 介接函式庫的建構式

        /// <summary>
        /// 全功能介接的建構式。
        /// </summary>
        /// <example>
        /// <code>
        /// /*
        ///  *  全功能介接的建構範例程式碼。
        ///  */
        /// try
        /// {
        ///     using (AllInOne oPayment = new AllInOne())
        ///     {
        ///         ......
        ///     }
        /// }
        /// catch (Exception ex)
        /// {
        ///     ......
        /// }
        /// finally
        /// {
        ///     ......
        /// }
        /// </code>
        /// </example>
        public AllInOne()
            : base()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        }

        #endregion

        #region - 提供介接的函數方法

       
        public IEnumerable<string> CheckOut()
        {
            return this.CheckOut("_self");
        }
     
        public IEnumerable<string> CheckOut(string target)
        {
            List<string> errList = new List<string>();
            // 驗證服務參數。
            errList.AddRange(ServerValidator.Validate(this));
            // 驗證基本參數。
            errList.AddRange(ServerValidator.Validate(this.Send));
            // 驗證延伸參數。
            errList.AddRange(ServerValidator.Validate(this.Send, this.SendExtend));
            // TODO: 額外檢查金額

            if (errList.Count == 0)
            {
                // 清除畫面控制項。
                this.ClearPageControls();
                // 信用卡特殊邏輯判斷(行動裝置畫面的信用卡分期處理，不支援定期定額)
                if (this.Send.ChoosePayment == PaymentMethod.Credit && this.Send.DeviceSource == DeviceType.Mobile && !this.SendExtend.PeriodAmount.HasValue)
                {
                    bool tIgnorePayment = (!string.IsNullOrEmpty(this.Send.IgnorePayment) && this.Send.IgnorePayment.ToUpper() == "GOOGLEPAY") ? true : false;

                    this.Send.ChoosePayment = PaymentMethod.ALL;
                    this.Send.IgnorePayment = String.Empty;
                    // 僅留下信用卡。
                    foreach (PaymentMethod pmEnum in Enum.GetValues(typeof(PaymentMethod)))
                    {
                        if (pmEnum != PaymentMethod.ALL && pmEnum != PaymentMethod.Credit)
                        {
                            if (pmEnum != PaymentMethod.GooglePay)
                            {
                                this.Send.IgnorePayment += String.Format("{0}#", pmEnum.ToString());
                            }
                        }
                    }

                    this.Send.IgnorePayment += "APPBARCODE#";

                    if (tIgnorePayment)
                    {
                        this.Send.IgnorePayment += "GooglePay";
                    }

                    if (this.Send.IgnorePayment.EndsWith("#"))
                    {
                        this.Send.IgnorePayment = this.Send.IgnorePayment.TrimEnd('#');
                    }
                }
                // 產生傳遞的參數與檢查碼。
                Hashtable htParameters = new Hashtable();
                string szChoosePayment = this.Send.ChoosePayment.ToString();
                string szParameters = String.Empty;
                string szCheckMacValue = String.Empty;
                // 取得並整理參數。
                htParameters.Add("MerchantID", this.MerchantID);
                htParameters.Add("MerchantTradeNo", this.Send.MerchantTradeNo);
                htParameters.Add("MerchantTradeDate", this.Send.MerchantTradeDate);
                htParameters.Add("PaymentType", this.Send.PaymentType);
                htParameters.Add("TotalAmount", this.Send.TotalAmount);
                htParameters.Add("TradeDesc", this.Send.TradeDesc);
                htParameters.Add("ItemName", this.Send.ItemName);
                htParameters.Add("ReturnURL", this.Send.ReturnURL);
                htParameters.Add("ChoosePayment", szChoosePayment);
                htParameters.Add("ClientBackURL", this.Send.ClientBackURL);
                htParameters.Add("ItemURL", this.Send.ItemURL);
                htParameters.Add("Remark", this.Send.Remark);
                if (this.Send.EncryptType == 1) htParameters.Add("EncryptType", this.Send.EncryptType);

                if (!String.IsNullOrEmpty(this.Send.StoreID)) htParameters.Add("StoreID", this.Send.StoreID);
                if (!String.IsNullOrEmpty(this.Send.CustomField1)) htParameters.Add("CustomField1", this.Send.CustomField1);
                if (!String.IsNullOrEmpty(this.Send.CustomField2)) htParameters.Add("CustomField2", this.Send.CustomField2);
                if (!String.IsNullOrEmpty(this.Send.CustomField3)) htParameters.Add("CustomField3", this.Send.CustomField3);
                if (!String.IsNullOrEmpty(this.Send.CustomField4)) htParameters.Add("CustomField4", this.Send.CustomField4);

                if (this.Send.ChooseSubPayment != PaymentMethodItem.None)
                {
                    string[] saChooseSubPayment = this.Send.ChooseSubPayment.ToString().Split(new char[] { '_' });

                    switch (saChooseSubPayment.Length)
                    {
                        case 1: htParameters.Add("ChooseSubPayment", saChooseSubPayment[0]); break;
                        case 2: htParameters.Add("ChooseSubPayment", saChooseSubPayment[1]); break;
                        default: break;
                    }
                }
                htParameters.Add("OrderResultURL", this.Send.OrderResultURL);
                htParameters.Add("NeedExtraPaidInfo", this.Send.NeedExtraPaidInfo.ToString().Substring(0, 1));

                //#18453 - 當None時傳送空值
                if (this.Send.DeviceSource == DeviceType.None)
                {
                    htParameters.Add("DeviceSource", "");
                }
                else if (this.Send.DeviceSource == DeviceType.PC || this.Send.DeviceSource == DeviceType.Mobile)
                {
                    htParameters.Add("DeviceSource", this.Send.DeviceSource.ToString().Substring(0, 1));
                }


                //htParameters.Add("DeviceSource", this.Send.DeviceSource.ToString().Substring(0, 1));
                if (!String.IsNullOrEmpty(this.Send.IgnorePayment)) htParameters.Add("IgnorePayment", this.Send.IgnorePayment);
                if (!String.IsNullOrEmpty(this.Send.PlatformID)) htParameters.Add("PlatformID", this.Send.PlatformID);
                if (this.Send.InvoiceMark == InvoiceState.Yes) htParameters.Add("InvoiceMark", "Y");
                if (this.Send.PlatformChargeFee.HasValue) htParameters.Add("PlatformChargeFee", this.Send.PlatformChargeFee);
                if (this.Send.HoldTradeAMT == HoldTradeType.Yes) htParameters.Add("HoldTradeAMT", (int)this.Send.HoldTradeAMT);
                if (!String.IsNullOrEmpty(this.Send.AllPayID)) htParameters.Add("AllPayID", this.Send.AllPayID);
                if (!String.IsNullOrEmpty(this.Send.AccountID)) htParameters.Add("AccountID", this.Send.AccountID);

                if (szChoosePayment == "ATM")
                {
                    htParameters.Add("ExpireDate", this.SendExtend.ExpireDate);
                    if (!String.IsNullOrEmpty(this.SendExtend.PaymentInfoURL)) htParameters.Add("PaymentInfoURL", this.SendExtend.PaymentInfoURL);
                    if (!String.IsNullOrEmpty(this.SendExtend.ClientRedirectURL)) htParameters.Add("ClientRedirectURL", this.SendExtend.ClientRedirectURL);
                }
                if (szChoosePayment == "CVS" || szChoosePayment == "BARCODE")
                {
                    if (this.SendExtend.StoreExpireDate.HasValue) htParameters.Add("StoreExpireDate", this.SendExtend.StoreExpireDate.Value);
                    if (!String.IsNullOrEmpty(this.SendExtend.Desc_1)) htParameters.Add("Desc_1", this.SendExtend.Desc_1);
                    if (!String.IsNullOrEmpty(this.SendExtend.Desc_2)) htParameters.Add("Desc_2", this.SendExtend.Desc_2);
                    if (!String.IsNullOrEmpty(this.SendExtend.Desc_3)) htParameters.Add("Desc_3", this.SendExtend.Desc_3);
                    if (!String.IsNullOrEmpty(this.SendExtend.Desc_4)) htParameters.Add("Desc_4", this.SendExtend.Desc_4);

                    if (!String.IsNullOrEmpty(this.SendExtend.PaymentInfoURL)) htParameters.Add("PaymentInfoURL", this.SendExtend.PaymentInfoURL);
                    if (!String.IsNullOrEmpty(this.SendExtend.ClientRedirectURL)) htParameters.Add("ClientRedirectURL", this.SendExtend.ClientRedirectURL);
                }
              
                if (szChoosePayment == "Credit" || szChoosePayment == "GooglePay" || szChoosePayment == "AndroidPay")
                {
                    // 一般分期。
                    if (!String.IsNullOrEmpty(this.SendExtend.CreditInstallment)) htParameters.Add("CreditInstallment", this.SendExtend.CreditInstallment);
                    if (this.SendExtend.InstallmentAmount.HasValue) htParameters.Add("InstallmentAmount", this.SendExtend.InstallmentAmount);
                    if (this.SendExtend.Redeem.HasValue && this.SendExtend.Redeem.Value) htParameters.Add("Redeem", "Y");
                    if (this.SendExtend.UnionPay.HasValue) htParameters.Add("UnionPay", (this.SendExtend.UnionPay.Value ? 1 : 0));
                    // 定期定額。
                    if (this.SendExtend.PeriodAmount.HasValue) htParameters.Add("PeriodAmount", this.SendExtend.PeriodAmount);
                    if (this.SendExtend.PeriodType != PeriodType.None) htParameters.Add("PeriodType", this.SendExtend.PeriodType.ToString().Substring(0, 1));
                    if (this.SendExtend.Frequency.HasValue) htParameters.Add("Frequency", this.SendExtend.Frequency);
                    if (this.SendExtend.ExecTimes.HasValue) htParameters.Add("ExecTimes", this.SendExtend.ExecTimes);
                    if (!String.IsNullOrEmpty(this.SendExtend.PeriodReturnURL)) htParameters.Add("PeriodReturnURL", this.SendExtend.PeriodReturnURL);
                    if (this.SendExtend.BindingCard == BindingCardType.Yes) htParameters.Add("BindingCard", 1);
                    if (this.SendExtend.BindingCard == BindingCardType.Yes && !String.IsNullOrEmpty(this.SendExtend.MerchantMemberID)) htParameters.Add("MerchantMemberID", this.SendExtend.MerchantMemberID);
                }
                if (!String.IsNullOrEmpty(this.SendExtend.Language)) htParameters.Add("Language", this.SendExtend.Language);
                // 電子發票額外參數。
                if (this.Send.InvoiceMark == InvoiceState.Yes)
                {
                    htParameters.Add("RelateNumber", this.SendExtend.RelateNumber);
                    htParameters.Add("CustomerID", this.SendExtend.CustomerID);
                    htParameters.Add("CustomerIdentifier", this.SendExtend.CustomerIdentifier);
                    htParameters.Add("CustomerName", HttpUtility.UrlEncode(this.SendExtend.CustomerName));
                    htParameters.Add("CustomerAddr", HttpUtility.UrlEncode(this.SendExtend.CustomerAddr));
                    htParameters.Add("CustomerPhone", this.SendExtend.CustomerPhone);
                    htParameters.Add("CustomerEmail", HttpUtility.UrlEncode(this.SendExtend.CustomerEmail));
                    htParameters.Add("ClearanceMark", (this.SendExtend.ClearanceMark == CustomsClearance.None ? String.Empty : ((int)this.SendExtend.ClearanceMark).ToString()));
                    htParameters.Add("TaxType", (int)this.SendExtend.TaxType);
                    htParameters.Add("CarruerType", (this.SendExtend.CarruerType == InvoiceVehicleType.None ? String.Empty : ((int)this.SendExtend.CarruerType).ToString()));
                    htParameters.Add("CarruerNum", this.SendExtend.CarruerNum);
                    htParameters.Add("Donation", (int)this.SendExtend.Donation);
                    htParameters.Add("LoveCode", this.SendExtend.LoveCode);
                    htParameters.Add("Print", (int)this.SendExtend.Print);
                    htParameters.Add("InvoiceItemName", HttpUtility.UrlEncode(this.SendExtend.InvoiceItemName));
                    htParameters.Add("InvoiceItemCount", this.SendExtend.InvoiceItemCount);
                    htParameters.Add("InvoiceItemWord", HttpUtility.UrlEncode(this.SendExtend.InvoiceItemWord));
                    htParameters.Add("InvoiceItemPrice", this.SendExtend.InvoiceItemPrice);
                    htParameters.Add("InvoiceItemTaxType", this.SendExtend.InvoiceItemTaxType);
                    htParameters.Add("InvoiceRemark", (String.IsNullOrEmpty(this.SendExtend.InvoiceRemark) ? String.Empty : HttpUtility.UrlEncode(this.SendExtend.InvoiceRemark)));
                    htParameters.Add("DelayDay", this.SendExtend.DelayDay);
                    htParameters.Add("InvType", String.Format("{0:00}", (int)this.SendExtend.InvType));
                }
                // 當為 ALL 時，若屬性有值，還是必須傳遞的參數。
                if (szChoosePayment == "ALL")
                {
                    // 信用卡選擇性參數。
                    if (!String.IsNullOrEmpty(this.SendExtend.CreditInstallment)) htParameters.Add("CreditInstallment", this.SendExtend.CreditInstallment);
                    if (this.SendExtend.InstallmentAmount.HasValue) htParameters.Add("InstallmentAmount", this.SendExtend.InstallmentAmount);
                    if (this.SendExtend.Redeem.HasValue && this.SendExtend.Redeem.Value) htParameters.Add("Redeem", "Y");
                    if (this.SendExtend.UnionPay.HasValue) htParameters.Add("UnionPay", (this.SendExtend.UnionPay.Value ? 1 : 0));
                    if (!String.IsNullOrEmpty(this.SendExtend.PaymentInfoURL)) htParameters.Add("PaymentInfoURL", this.SendExtend.PaymentInfoURL);

                    // 信用卡定期定額。
                    if (this.SendExtend.PeriodAmount.HasValue) htParameters.Add("PeriodAmount", this.SendExtend.PeriodAmount);
                    if (this.SendExtend.PeriodType != PeriodType.None) htParameters.Add("PeriodType", this.SendExtend.PeriodType.ToString().Substring(0, 1));
                    if (this.SendExtend.Frequency.HasValue) htParameters.Add("Frequency", this.SendExtend.Frequency);
                    if (this.SendExtend.ExecTimes.HasValue) htParameters.Add("ExecTimes", this.SendExtend.ExecTimes);
                    if (!String.IsNullOrEmpty(this.SendExtend.PeriodReturnURL)) htParameters.Add("PeriodReturnURL", this.SendExtend.PeriodReturnURL);

                 
                    //CVS 或 BARCODE 參數
                    if (this.SendExtend.StoreExpireDate.HasValue) htParameters.Add("StoreExpireDate", this.SendExtend.StoreExpireDate.Value);
                    if (!String.IsNullOrEmpty(this.SendExtend.Desc_1)) htParameters.Add("Desc_1", this.SendExtend.Desc_1);
                    if (!String.IsNullOrEmpty(this.SendExtend.Desc_2)) htParameters.Add("Desc_2", this.SendExtend.Desc_2);
                    if (!String.IsNullOrEmpty(this.SendExtend.Desc_3)) htParameters.Add("Desc_3", this.SendExtend.Desc_3);
                    if (!String.IsNullOrEmpty(this.SendExtend.Desc_4)) htParameters.Add("Desc_4", this.SendExtend.Desc_4);

                    //ATM 參數
                    htParameters.Add("ExpireDate", this.SendExtend.ExpireDate);

                    // if (!String.IsNullOrEmpty(this.SendExtend.PaymentInfoURL)) htParameters.Add("PaymentInfoURL", this.SendExtend.PaymentInfoURL); --信用卡選擇性參數已經有
                    if (!String.IsNullOrEmpty(this.SendExtend.ClientRedirectURL)) htParameters.Add("ClientRedirectURL", this.SendExtend.ClientRedirectURL);
                }
                // 產生畫面控制項與傳遞參數。
                szParameters = this.RenderControlAndParamenter(htParameters);
                // 產生檢查碼。
                szCheckMacValue = this.BuildCheckMacValue(szParameters, this.Send.EncryptType);
                // 繪製 MD5 檢查碼控制項。
                szParameters += this.RenderControlAndParamenter("CheckMacValue", szCheckMacValue);
                // 紀錄記錄檔
                Logger.WriteLine(String.Format("INFO   {0}  OUTPUT  AllInOne.CheckOut: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szParameters));

                if (ServiceMethod == HttpMethod.HttpPOST)
                {
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

                        this.CurrentHead.Controls.Clear();
                        this.CurrentForm.Controls.AddAt(0, htmJQuery);
                        this.CurrentForm.Controls.AddAt(1, htmScript);
                        this.CurrentForm.Action = this.ServiceURL;
                    }
                }
                else
                {
                    errList.Add("No service for HttpGET, ServerPOST and HttpSOAP.");
                }
            }

            return errList;
        }
      
        public IEnumerable<string> CheckOutString(ref string html)
        {
            return this.CheckOutString("_self", ref html);
        }
     
        public IEnumerable<string> CheckOutString(string target, ref string html)
        {
            List<string> errList = null;

            this.CurrentPage = new Page() { EnableEventValidation = false };
            this.CurrentHead = new HtmlHead() { Title = "Form Redirect" };
            this.CurrentForm = new HtmlForm() { Name = "aspnetForm" };

            errList = new List<string>();
            errList.AddRange(this.CheckOut(target));

            if (errList.Count == 0)
            {
                this.CurrentPage.Controls.Add(this.CurrentHead);
                this.CurrentPage.Controls.Add(this.CurrentForm);

                StringBuilder stringBuilder = new StringBuilder();
                StringWriter stringWriter = new StringWriter(stringBuilder);
                HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);

                this.CurrentForm.RenderControl(htmlWriter);

                html = stringBuilder.ToString();
            }

            return errList;
        }
    
        public IEnumerable<string> CheckOutFeedback(ref Hashtable feedback)
        {
            string szParameters = String.Empty;
            string szCheckMacValue = String.Empty;

            List<string> errList = new List<string>();

            if (null == feedback) feedback = new Hashtable();

            Array.Sort(this.CurrentRequest.Form.AllKeys);

            foreach (string szKey in this.CurrentRequest.Form.AllKeys)
            {
                string szValue = this.CurrentRequest.Form[szKey];

                if (szKey != "CheckMacValue")
                {
                    szParameters += String.Format("&{0}={1}", szKey, szValue);

                    if (szKey == "PaymentType")
                    {
                        szValue = szValue.Replace("_CVS", String.Empty);
                        szValue = szValue.Replace("_BARCODE", String.Empty);
                        szValue = szValue.Replace("_CreditCard", String.Empty);
                    }

                    if (szKey == "PeriodType")
                    {
                        szValue = szValue.Replace("Y", "Year");
                        szValue = szValue.Replace("M", "Month");
                        szValue = szValue.Replace("D", "Day");
                    }

                    feedback.Add(szKey, szValue);
                }
                else
                {
                    szCheckMacValue = szValue;
                }
            }
            // 紀錄記錄檔
            Logger.WriteLine(String.Format("INFO   {0}  INPUT   AllInOne.CheckOutFeedback: {1}&CheckMacValue={2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szParameters, szCheckMacValue));
            // 比對驗證檢查碼。
            errList.AddRange(this.CompareCheckMacValue(szParameters, szCheckMacValue));

            return errList;
        }
      
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
                Logger.WriteLine(String.Format("INFO   {0}  OUTPUT  AllInOne.QueryTradeInfo: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szParameters));
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
                Logger.WriteLine(String.Format("INFO   {0}  INPUT   AllInOne.QueryTradeInfo: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szFeedback));
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

                                if (szKey == "PaymentType")
                                {
                                    szValue = szValue.Replace("_CVS", String.Empty);
                                    szValue = szValue.Replace("_BARCODE", String.Empty);
                                    szValue = szValue.Replace("_Alipay", String.Empty);
                                    szValue = szValue.Replace("_Tenpay", String.Empty);
                                    szValue = szValue.Replace("_CreditCard", String.Empty);
                                }

                                if (szKey == "PeriodType")
                                {
                                    szValue = szValue.Replace("Y", "Year");
                                    szValue = szValue.Replace("M", "Month");
                                    szValue = szValue.Replace("D", "Day");
                                }

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
        /// <summary>
        /// 信用卡定期定額訂單查詢
        /// </summary>
        /// <param name="feedback">回傳的參數資料</param>
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
        /// PeriodCreditCardTradeInfo oFeedback = null;
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
        ///         enErrors.AddRange(oPayment.QueryPeriodCreditCardTradeInfo(ref oFeedback));
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
        ///         /* 使用 WebATM 交易時，回傳的額外參數 */
        ///         string szWebATMAccBank = String.Empty;
        ///         string szWebATMAccNo = String.Empty;
        ///         /* 使用 ATM 交易時，回傳的額外參數 */
        ///         string szATMAccBank = String.Empty;
        ///         string szATMAccNo = String.Empty;
        ///         /* 使用 CVS 或 BARCODE 交易時，回傳的額外參數 */
        ///         string szPaymentNo = String.Empty;
        ///         string szPayFrom = String.Empty;
        ///         /* 使用 Alipay 交易時，回傳的額外參數 */
        ///         string szAlipayID = String.Empty;
        ///         string szAlipayTradeNo = String.Empty;
        ///         /* 使用 Tenpay 交易時，回傳的額外參數 */
        ///         string szTenpayTradeNo = String.Empty;
        ///         /* 使用 Credit 交易時，回傳的額外參數 */
        ///         string szGwsr = String.Empty;
        ///         string szProcessDate = String.Empty;
        ///         string szAuthCode = String.Empty;
        ///         string szAmount = String.Empty;
        ///         string szStage = String.Empty;
        ///         string szStast = String.Empty;
        ///         string szStaed = String.Empty;
        ///         string szECI = String.Empty;
        ///         string szCard4No = String.Empty;
        ///         string szCard6No = String.Empty;
        ///         string szRedDan = String.Empty;
        ///         string szRedDeAmt = String.Empty;
        ///         string szRedOkAmt = String.Empty;
        ///         string szRedYet = String.Empty;
        ///         string szPeriodType = String.Empty;
        ///         string szFrequency = String.Empty;
        ///         string szExecTimes = String.Empty;
        ///         string szPeriodAmount = String.Empty;
        ///         string szTotalSuccessTimes = String.Empty;
        ///         string szTotalSuccessAmount = String.Empty;
        ///         // 取得資料於畫面
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
        public IEnumerable<string> QueryPeriodCreditCardTradeInfo(ref PeriodCreditCardTradeInfo feedback)
        {
            string szFeedback = String.Empty;
            string szParameters = String.Empty;
            string szCheckMacValue = String.Empty;

            List<string> errList = new List<string>();

            if (null == feedback) feedback = new PeriodCreditCardTradeInfo();
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
                Logger.WriteLine(String.Format("INFO   {0}  OUTPUT  AllInOne.QueryTradeInfo: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szParameters));
                // 自遠端伺服器取得資料。
                if (this.ServiceMethod == HttpMethod.ServerPOST)
                {
                    szFeedback = this.ServerPost(szParameters);
                    // 重新整理取回的參數。
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    // 反序列化為物件。
                    feedback = jsonSerializer.Deserialize<PeriodCreditCardTradeInfo>(szFeedback);
                }
                else
                {
                    errList.Add("No service for HttpPOST and HttpGET.");
                }
                // 紀錄記錄檔
                Logger.WriteLine(String.Format("INFO   {0}  INPUT   AllInOne.QueryTradeInfo: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szFeedback));
            }

            return errList;
        }
      
        public IEnumerable<string> DoAction(ref Hashtable feedback)
        {
            string szFeedback = String.Empty;
            string szParameters = String.Empty;
            string szCheckMacValue = String.Empty;

            List<string> errList = new List<string>();

            if (null == feedback) feedback = new Hashtable();
            // 驗證服務參數。
            errList.AddRange(ServerValidator.Validate(this));
            // 驗證基本參數。
            errList.AddRange(ServerValidator.Validate(this.Action));

            if (errList.Count == 0)
            {
                // 產生傳遞的參數與檢查碼。
                string szChoosePayment = this.Send.ChoosePayment.ToString();
                // 產生畫面控制項與傳遞參數。
                szParameters += this.BuildParamenter("Action", this.Action.Action.ToString());
                szParameters += this.BuildParamenter("MerchantID", this.MerchantID);
                szParameters += this.BuildParamenter("MerchantTradeNo", this.Action.MerchantTradeNo);
                szParameters += this.BuildParamenter("TotalAmount", this.Action.TotalAmount);
                szParameters += this.BuildParamenter("TradeNo", this.Action.TradeNo);
                // 產生檢查碼。
                szCheckMacValue = this.BuildCheckMacValue(szParameters);
                // 繪製 MD5 檢查碼控制項。
                szParameters += this.BuildParamenter("CheckMacValue", szCheckMacValue);
                szParameters = szParameters.Substring(1);
                // 紀錄記錄檔
                Logger.WriteLine(String.Format("INFO   {0}  OUTPUT  AllInOne.DoAction: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szParameters));
                // 自遠端伺服器取得資料。
                if (this.ServiceMethod == HttpMethod.ServerPOST)
                {
                    szFeedback = this.ServerPost(szParameters);
                }
                else
                {
                    errList.Add("No service for HttpPOST, HttpGET and HttpSOAP.");
                }
                // 紀錄記錄檔
                Logger.WriteLine(String.Format("INFO   {0}  INPUT   AllInOne.DoAction: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szFeedback));
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

                    if (feedback.ContainsKey("RtnCode") && !"1".Equals(feedback["RtnCode"]))
                    {
                        errList.Add(String.Format("{0}: {1}", feedback["RtnCode"], feedback["RtnMsg"]));
                    }
                }
            }

            return errList;
        }
     

        /// <summary>
        /// 廠商下載對帳檔。
        /// </summary>
        /// <param name="filepath">對帳檔要儲存的路徑(含檔名)。</param>
        /// <returns>錯誤訊息的集合字串。</returns>
        /// <remarks>
        /// 該方法會以 Server POST 的方式送出參數至 AllInOne.ServiceURL 位址，
        /// 網路需申請 ACL(Access Control List) 才能正常下載訂單資料的對帳檔。
        /// </remarks>
        /// <example>
        /// <code>
        /// </code>
        /// </example>
        public IEnumerable<string> TradeNoAio(string filepath)
        {
            Hashtable htParameters = new Hashtable();
            string szFeedback = String.Empty;
            string szParameters = String.Empty;
            string szCheckMacValue = String.Empty;

            List<string> errList = new List<string>();

            if (String.IsNullOrEmpty(filepath)) errList.Add("The filepath is required.");
            // 驗證服務參數。
            errList.AddRange(ServerValidator.Validate(this));
            // 驗證基本參數。
            errList.AddRange(ServerValidator.Validate(this.TradeFile));

            if (errList.Count == 0)
            {
                // 取得並整理參數。
                htParameters.Add("MerchantID", this.MerchantID);
                htParameters.Add("DateType", (int)this.TradeFile.DateType);
                htParameters.Add("BeginDate", this.TradeFile.BeginDate);
                htParameters.Add("EndDate", this.TradeFile.EndDate);
                if (this.TradeFile.PaymentType != PaymentMethod.ALL) htParameters.Add("PaymentType", String.Format("{0:00}", (int)this.TradeFile.PaymentType));
                if (this.TradeFile.PlatformStatus != PlatformState.ALL) htParameters.Add("PlatformStatus", (int)this.TradeFile.PlatformStatus);
                if (this.TradeFile.PaymentStatus != PaymentState.ALL) htParameters.Add("PaymentStatus", (int)this.TradeFile.PaymentStatus);
                if (this.TradeFile.AllocateStatus != AllocateState.ALL) htParameters.Add("AllocateStauts", (int)this.TradeFile.AllocateStatus);
                htParameters.Add("MediaFormated", (this.TradeFile.NewFormatedMedia ? 1 : 0));
                if (this.TradeFile.CharSet != CharSetState.Default) htParameters.Add("CharSet", (byte)this.TradeFile.CharSet);
                // 產生畫面控制項與傳遞參數。
                szParameters = this.RenderControlAndParamenter(htParameters);
                // 產生檢查碼。
                szCheckMacValue = this.BuildCheckMacValue(szParameters);
                // 繪製 MD5 檢查碼控制項。
                szParameters += this.BuildParamenter("CheckMacValue", szCheckMacValue);
                szParameters = szParameters.Substring(1);
                // 紀錄記錄檔
                Logger.WriteLine(String.Format("INFO   {0}  OUTPUT  AllInOne.TradeNoAio: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szParameters));
                // 自遠端伺服器取得資料。
                if (this.ServiceMethod == HttpMethod.ServerPOST)
                {
                    szFeedback = this.ServerPost(szParameters, Encoding.GetEncoding("Big5"));
                }
                else
                {
                    errList.Add("No service for HttpPOST, HttpGET and HttpSOAP.");
                }
                // 紀錄記錄檔
                Logger.WriteLine(String.Format("INFO   {0}  INPUT   AllInOne.TradeNoAio: {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), szFeedback));
                // 寫入檔案
                CharSetState charset =  this.TradeFile.CharSet == CharSetState.Default ?  CharSetState.UTF8 : this.TradeFile.CharSet;
                using (StreamWriter writer = new StreamWriter(filepath, false, CharSetHelper.GetCharSet(charset)))
                {
                    writer.Write(szFeedback); 
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
        private string BuildCheckMacValue(string parameters, int encryptType = 0)
        {
            string szCheckMacValue = String.Empty;
            // 產生檢查碼。
            szCheckMacValue = String.Format("HashKey={0}{1}&HashIV={2}", this.HashKey, parameters, this.HashIV);
            szCheckMacValue = HttpUtility.UrlEncode(szCheckMacValue).ToLower();
            if (encryptType == 1)
            {
                szCheckMacValue = SHA256Encoder.Encrypt(szCheckMacValue);
            }
            else
            {
                szCheckMacValue = MD5Encoder.Encrypt(szCheckMacValue);
            }

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
                string szConfirmMacValueMD5 = this.BuildCheckMacValue(parameters, 0);

                // 產生檢查碼。
                string szConfirmMacValueSHA256 = this.BuildCheckMacValue(parameters, 1);

                // 比對檢查碼。
                if (checkMacValue != szConfirmMacValueMD5 && checkMacValue != szConfirmMacValueSHA256)
                {
                    errList.Add("CheckMacValue verify fail.");
                }

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
                for (int i = this.CurrentForm.Controls.Count; i > 0; i--)
                {
                    Control oControl = this.CurrentForm.Controls[i - 1];

                    if (oControl.GetType().Name != "ScriptManager" && oControl.GetType().Name != "ToolkitScriptManager")
                    {
                        this.CurrentForm.Controls.Remove(oControl);
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

                this.CurrentForm.Controls.Add(hideField);
            }

            return this.BuildParamenter(id, value);
        }
        /// <summary>
        /// 繪製控制項並產生參數字串。
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string RenderControlAndParamenter(Hashtable parameters)
        {
            string szParameters = String.Empty;
            ArrayList aryKeys = null;

            aryKeys = new ArrayList(parameters.Keys);
            aryKeys.Sort();

            foreach (string szKey in aryKeys)
            {
                string szValue = String.Empty;
                object oValue = parameters[szKey];

                if (null != oValue)
                {
                    if (oValue.GetType().Equals(typeof(DateTime)))
                        szValue = ((DateTime)oValue).ToString("yyyy/MM/dd HH:mm:ss");
                    else
                        szValue = oValue.ToString();
                }

                if (null != this.CurrentPage)
                {
                    HtmlInputHidden hideField = null;

                    hideField = new HtmlInputHidden();
                    hideField.ID = szKey;
                    hideField.Value = szValue;
                    hideField.Attributes["class"] = "__parameter";

                    this.CurrentForm.Controls.Add(hideField);
                }

                szParameters += this.BuildParamenter(szKey, oValue);
            }

            return szParameters;
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


        /// <summary>
        /// 伺服器端傳送參數請求資料。
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="receviceEncoding"></param>
        /// <returns></returns>
        private string ServerPost(string parameters, Encoding receviceEncoding)
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
                        using (StreamReader oReader = new StreamReader(webResponse.GetResponseStream(), receviceEncoding))
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
            GC.Collect();
        }

        #endregion
    }
}
