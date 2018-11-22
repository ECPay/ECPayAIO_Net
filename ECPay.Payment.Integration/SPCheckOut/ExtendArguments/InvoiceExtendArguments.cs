using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration.SPCheckOut.ExtendArguments
{
    /// <summary>
    ///電子發票擴展參數
    /// </summary>
    public class InvoiceExtendArguments
    {
        private string _relateNumber;
        /// <summary>
        /// 特店自訂編號
        /// SDK自動產生
        /// </summary>
        public string RelateNumber { 
            get 
            {
                return string.IsNullOrEmpty(_relateNumber) ? DateTime.Now.ToString("yyyyMMddHHmmss") : _relateNumber; 
            }
            set
            {
                _relateNumber = value;
            }
        }

        /// <summary>
        /// 客戶代號
        /// </summary>
        public string CustomerID { get; set; }

        /// <summary>
        /// 統一編號
        /// </summary>
        public string CustomerIdentifier { get; set; }
        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 客戶地址
        /// </summary>
        public string CustomerAddr { get; set; }

        /// <summary>
        /// 客戶手機號碼
        /// </summary>
        public string CustomerPhone { get; set; }

        /// <summary>
        /// 客戶電子信箱
        /// </summary>
        public string CustomerEmail { get; set; }

        /// <summary>
        /// 通關方式
        /// </summary>
        public string ClearanceMark { get; set; }

        /// <summary>
        /// 課稅類別
        /// </summary>
        public string TaxType { get; set; }

        /// <summary>
        /// 載具類別
        /// </summary>
        public string CarruerType { get; set; }

        /// <summary>
        /// 載具編號
        /// </summary>
        public string CarruerNum { get; set; }

        /// <summary>
        /// 捐贈註記
        /// </summary>
        public string Donation { get; set; }

        /// <summary>
        /// 愛心碼
        /// </summary>
        public string LoveCode { get; set; }

        /// <summary>
        /// 列印註記
        /// </summary>
        public string Print { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string InvoiceItemName { get; set; }

        /// <summary>
        /// 商品數量
        /// </summary>
        public string InvoiceItemCount { get; set; }

        /// <summary>
        /// 商品單位
        /// </summary>
        public string InvoiceItemWord { get; set; }

        /// <summary>
        /// 商品價格
        /// </summary>
        public string InvoiceItemPrice { get; set; }

        /// <summary>
        /// 商品課稅別
        /// </summary>
        public string InvoiceItemTaxType { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string InvoiceRemark { get; set; }

        /// <summary>
        /// 延遲天數
        /// </summary>
        public int DelayDay { get; set; }

        /// <summary>
        /// 字軌類別
        /// </summary>
        public string InvType { get; set; }
    }
}
