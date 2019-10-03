using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ECPay.Payment.Integration.SPCheckOut.ExtendArguments
{
    public class SendCommonArguments
    {
        public SendCommonArguments() 
        {
            this.NeedExtraPaidInfo = "N";
            this.InvoiceMark = "N";
            this.HoldTradeAMT = 0;
        }

        /// <summary>
        /// 商品名稱
        /// </summary>        
        public string ItemName { get; set; }

        /// <summary>
        /// 付款完成通知回傳網址
        /// </summary>
        [Required]
        public string ReturnURL { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public uint TotalAmount { get; set; }

        /// <summary>
        /// 交易描述
        /// </summary>
        public string TradeDesc { get; set; }

        /// <summary>
        /// 廠商編號。
        /// </summary>
        [Required(ErrorMessage = "MerchantID is Required")]
        public string MerchantID { get; set; }

        /// <summary>
        /// 允許繳費有效天數
        /// </summary>
        public string ExpireDate { get; set; } 

        /// <summary>
        /// 電子發票開立註記
        /// </summary>
        public string InvoiceMark { get; set; }

        /// <summary>
        /// 是否延遲撥款
        /// </summary>
        public int HoldTradeAMT { get; set; }

        /// <summary>
        /// 是否需要額外的付款資訊 (預設值：N)
        /// </summary>
        public string NeedExtraPaidInfo { get; set; }

        /// <summary>
        /// 特約合作平台商代號(由綠界提供)
        /// </summary>
        public string PlatformID { get; set; }

        /// <summary>
        /// 自訂名稱欄位1
        /// </summary>
        public string CustomField1 { get; set; }

        /// <summary>
        /// 自訂名稱欄位2
        /// </summary>
        public string CustomField2 { get; set; }

        /// <summary>
        /// 自訂名稱欄位3
        /// </summary>
        public string CustomField3 { get; set; }

        /// <summary>
        /// 特店商店代碼
        /// </summary>
        public string StoreID { get; set; }

        /// <summary>
        /// 自訂名稱欄位4
        /// </summary>
        public string CustomField4 { get; set; }

        private string _merchantTradeNo;
        /// <summary>
        /// 特店交易編號
        /// </summary>
        public string MerchantTradeNo
        {
            get
            {
                return string.IsNullOrEmpty(_merchantTradeNo) ? DateTime.Now.ToString("yyyyMMddHHmmss") : _merchantTradeNo;
            }
            set 
            {
                _merchantTradeNo = value;
            }
        }

        /// <summary>
        /// 特店交易時間 
        /// </summary>
        public string MerchantTradeDate
        {
            get
            {
                return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            }

        }

        /// <summary>
        /// 交易類型
        /// </summary>
        public string PaymentType
        {
            get
            {
                return "aio";
            }
        }

        /// <summary>
        /// 選擇預設付款方式
        /// </summary>
        public string ChoosePayment
        {
            get
            {
                return "ALL";
            }
        }

        /// <summary>
        /// CheckMacValue加密類型
        /// </summary>
        public string EncryptType
        {
            get
            {
                return "1";
            }
        }
        /// <summary>
        /// Client端返回特店的按鈕連結
        /// </summary>
        public string ClientBackURL { get; set; }
    }
}
