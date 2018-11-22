using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECPay.Payment.Integration.Helper;
using System.Web;
using System.Security.Cryptography;
using System.Net;
using ECPay.Payment.Integration.SPCheckOut.ExtendArguments;
using System.Reflection;
using ECPay.Payment.Integration.Extensions;
using ECPay.Payment.Integration.Attributes;

namespace ECPay.Payment.Integration.SPCheckOut
{
    /// <summary>
    /// 站內付API
    /// </summary>
    public class SPCheckOutApi : IDisposable
    {
        private ParamterHelper _paraHelper;

        private SortedDictionary<string, string> PostCollection = new SortedDictionary<string, string>();

        /// <summary>
        ///  介接ATM產生訂單的延伸資料傳遞成員。
        /// </summary>
        public ATMExtendArguments ATM { get; private set; }

        /// <summary>
        /// 介接便利商店產生訂單的延伸資料傳遞成員。
        /// </summary>
        public CVSExtendArguments CVS { get; private set; }

        /// <summary>
        /// 介接基本產生訂單的基本資料傳遞成員。
        /// </summary>
        public SendCommonArguments Send { get; private set; }

        /// <summary>
        /// 介接電子發票產生訂單的延伸資料傳遞成員。
        /// </summary>
        public InvoiceExtendArguments Invoice { get; private set; }

        /// <summary>
        /// 信用卡一次付清
        /// </summary>
        public CreditPayOffExtendArguments CreditPayOff { get; private set; }

        /// <summary>
        /// 信用卡分期付款
        /// </summary>
        public CreditInstallmentExtendArguments CreditInstallment { get;private set; }

        /// <summary>
        /// 信用卡定期定額
        /// </summary>
        public CreditRSPExtendArguments CreditRSP { get; private set; }
        
        /// <summary>
        /// 信用卡付款方式
        /// </summary>
        public CreditPayType CreditPayment { get; set; }

        /// <summary>
        /// 電子發票啟用方式
        /// </summary>
        public EInvoiceType EivoiceType { get; set; }

        /// <summary>
        /// 介接服務的網址。
        /// </summary>
        public string ServiceURL { get; set; }

        /// 介接的 HashKey。
        /// </summary>
        public string HashKey { get; set; }

        /// <summary>
        /// 介接的 HashIV。
        /// </summary>
        public string HashIV { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        public SPCheckOutApi()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            _paraHelper = new ParamterHelper();
            ATM = new ATMExtendArguments();
            CVS = new CVSExtendArguments();
            Send = new SendCommonArguments();
            Invoice = new InvoiceExtendArguments();
            CreditPayOff = new CreditPayOffExtendArguments();
            CreditInstallment = new CreditInstallmentExtendArguments();
            CreditRSP = new CreditRSPExtendArguments();
            CreditPayment = CreditPayType.None;
        }

        /// <summary>
        /// 請求站內付SP_Token
        /// </summary>
        /// <returns>站內付API回傳結果</returns>
        public string Excute()
        {
            HttpTool http = new HttpTool();

            //設置參數
            SetParamter();
            string ParameterString = _paraHelper.DictionaryToParamter(PostCollection).Replace("+", "%2B");

            // 紀錄記錄檔
            Logger.WriteLine(String.Format("INFO   {0}  INPUT  SP.CkeckOutAip: {1}  ServiceURL:{2}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ParameterString, ServiceURL));

            string Result = http.DoRequestStrData(ServiceURL, ParameterString);

            // 紀錄記錄檔
            Logger.WriteLine(String.Format("INFO   {0}  OUTPUT  SP.CkeckOutAip: {1}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Result));

            return Result;
        }

        /// <summary>
        /// 計算MacValue
        /// </summary>
        /// <returns></returns>
        private void SetParamter()
        {
            //如果標記要使用電子發票
            if (EInvoiceType.Use == EivoiceType)
            {
                this.Send.InvoiceMark = "Y";
                SetParameter(this.Invoice);
            }
            else
            {
                this.Send.InvoiceMark = "N";
            }

            //填充基本參數
            SetParameter(this.Send);
            //填充ATM參數
            SetParameter(this.ATM);
            //填充便利商店參數
            SetParameter(this.CVS);

            switch (CreditPayment)      
            {
                case CreditPayType.CreditPayOff:
                    SetParameter(this.CreditPayOff);
                    break;
                case CreditPayType.CreditInstallment:
                    SetParameter(this.CreditInstallment);
                    break;
                case CreditPayType.CreditRSP:
                    SetParameter(this.CreditRSP);
                    break;
            }

            this.Send.InvoiceMark = EivoiceType.GetAttributeValue((TextAttribute attr) => attr.Name);

            CalculateMacValue();
        }

        /// <summary>
        /// 計算檢核碼
        /// </summary>
        private void CalculateMacValue()
        {
            // 紀錄記錄檔
            Logger.WriteLine(String.Format("INFO   {0}  INPUT  SP.GetKey_Para_IV: {1}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), GetKey_Para_IV()));

            string EncodeParameter = HttpUtility.UrlEncode(GetKey_Para_IV());
            string paraLower = EncodeParameter.ToLower();
            string CheckMacValue = GetSHA256(paraLower);
            PostCollection.Add("CheckMacValue", CheckMacValue);
        }

        /// <summary>
        /// 設置參數
        /// </summary>
        /// <param name="target"></param>
        private void SetParameter(object target)
        {
            object value;
            foreach (var prop in target.GetType().GetProperties())
            {
                value = prop.GetValue(target, null);
                if (null != value)
                {
                    this.PostCollection[prop.Name] = value.ToString();
                }
            }
        }

        /// <summary>
        /// 取得組完字串的Key和IV
        /// </summary>
        /// <returns></returns>
        private string GetKey_Para_IV() 
        {
            string para = _paraHelper.DictionaryToParamter(PostCollection);

            return string.Format("HashKey={0}&" + para + "&HashIV={1}", HashKey, HashIV);
        }

        #region 加密
        private string GetSHA256(string ToLower)
        {
            SHA256 SHA256Hasher = SHA256.Create();
            byte[] data = SHA256Hasher.ComputeHash(Encoding.Default.GetBytes(ToLower));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));//MD5碼 大小寫
            }
            return sBuilder.ToString();
        }
        #endregion

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
