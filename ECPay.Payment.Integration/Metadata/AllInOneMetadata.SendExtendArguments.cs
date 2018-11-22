using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Linq.Expressions;
using System.Reflection;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 全功能介接參數的類別。
    /// </summary>
    public partial class AllInOneMetadata
	{
        /// <summary>
        /// 介接的延伸資料傳遞成員類別。
        /// </summary>
        public class SendExtendArguments
        {
            /// <summary>
            /// 付款方式變更的快取。
            /// </summary>
            public PaymentMethod _PaymentMethod { get; internal set; }

            /*
             * ATM 延伸參數。
             */
            /// <summary>
            /// 允許繳費有效天數。
            /// </summary>
            [RequiredByPaymentMethod(PaymentMethod.ATM, ErrorMessage = "{0} is required.")]
            public int ExpireDate { get; set; }
            /*
             * CVS, BARCODE 延伸參數。
             */
            /// <summary>
            /// 超商繳費截止時間(※CVS：以分為計算單位 BARCODE：以天為計算單位)。
            /// </summary>
            public int? StoreExpireDate { get; set; }
            /// <summary>
            /// 交易描述1(會顯示在超商繳費平台螢幕上)。
            /// </summary>
            public string Desc_1 { get; set; }
            /// <summary>
            /// 交易描述1(會顯示在超商繳費平台螢幕上)。
            /// </summary>
            public string Desc_2 { get; set; }
            /// <summary>
            /// 交易描述1(會顯示在超商繳費平台螢幕上)。
            /// </summary>
            public string Desc_3 { get; set; }
            /// <summary>
            /// 交易描述1(會顯示在超商繳費平台螢幕上)。
            /// </summary>
            public string Desc_4 { get; set; }
            /*
             * ATM, CVS, BARCODE 延伸參數。
             */
            /// <summary>
            /// 此網址為訂單建立完成後(非付款完成後)，綠界科技會將付款相關資訊以Client端方式回傳給特約商店(※有設定此參數ClientBackURL參數將會失去作用)。
            /// </summary>
            [RegularExpression(@"^(?:http|https|ftp)://[a-zA-Z0-9\.\-]+(?:\:\d{1,5})?(?:[A-Za-z0-9\.\;\:\@\&\=\+\$\,\?/_]|%u[0-9A-Fa-f]{4}|%[0-9A-Fa-f]{2})*$", ErrorMessage = "{0} is not correct URL.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string ClientRedirectURL { get; set; }
            /*
             * Alipay 延伸參數。
             */
            /// <summary>
            /// 商品名稱(於 Arguments.Items 帶入購買商品的資訊後，系統會自動轉為 AlipayItemName 所需格式)。
            /// </summary>
            [RequiredByPaymentMethod(PaymentMethod.Alipay, ErrorMessage = "{0} is required.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string AlipayItemName { get; set; }
            /// <summary>
            /// 商品購買數量(於 Arguments.Items 帶入購買商品的資訊後，系統會自動轉計算 AlipayItemCounts 數量)。
            /// </summary>
            [RequiredByPaymentMethod(PaymentMethod.Alipay, ErrorMessage = "{0} is required.")]
            [StringLength(100, ErrorMessage = "{0} max langth as {1}.")]
            public string AlipayItemCounts { get; set; }
            /// <summary>
            /// 商品單價(於 Arguments.Items 帶入購買商品的資訊後，系統會自動轉為 AlipayItemPrice 所需格式)。
            /// </summary>
            [RequiredByPaymentMethod(PaymentMethod.Alipay, ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string AlipayItemPrice { get; set; }
            /// <summary>
            /// 購買人信箱(支付寶紀錄用，本公司不會紀錄)。
            /// </summary>
            [RequiredByPaymentMethod(PaymentMethod.Alipay, ErrorMessage = "{0} is required.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string Email { get; set; }
            /// <summary>
            /// 購買人電話(支付寶紀錄用，本公司不會紀錄)。
            /// </summary>
            [RequiredByPaymentMethod(PaymentMethod.Alipay, ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string PhoneNo { get; set; }
            /// <summary>
            /// 購買人姓名(支付寶紀錄用，本公司不會紀錄)。
            /// </summary>
            [RequiredByPaymentMethod(PaymentMethod.Alipay, ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string UserName { get; set; }
            /*
             * Tenpay 延伸參數。
             */
            /// <summary>
            /// 付款截止時間(只能帶入送出交易後的72小時(三天)之內時間；不填則預設為送出交易後的72小時。)。
            /// </summary>
            [RequiredByPaymentMethod(PaymentMethod.Tenpay, ErrorMessage = "{0} is required.")]
            public DateTime ExpireTime { get; set; }
            /*
             * Credit 分期延伸參數。
             */
            /// <summary>
            /// 刷卡分期期數。
            /// </summary>
            [CompareByPaymentMethod(PaymentMethod.Credit, "PeriodAmount/PeriodType/Frequency/ExecTimes", ErrorMessage = "When payment method is Credit and you want to installment, then {0} is required and {4} is not allow required.")]
            public string CreditInstallment { get; set; }
            /// <summary>
            /// 使用刷卡分期的付款金額。
            /// </summary>
            [CompareByPaymentMethod(PaymentMethod.Credit, "PeriodAmount/PeriodType/Frequency/ExecTimes", ErrorMessage = "When payment method is Credit and you want to installment, then {0} is required and {4} is not allow required.")]
            public decimal? InstallmentAmount { get; set; }
            /// <summary>
            /// 信用卡是否使用紅利折抵(False: 否, True: 是)。
            /// </summary>
            [CompareByPaymentMethod(PaymentMethod.Credit, "PeriodAmount/PeriodType/Frequency/ExecTimes", ErrorMessage = "When payment method is Credit and you want to installment, then {0} is required and {4} is not allow required.")]
            public bool? Redeem { get; set; }
            /// <summary>
            /// 是否為銀聯卡交易(False: 否, True: 是)。
            /// </summary>
            [CompareByPaymentMethod(PaymentMethod.Credit, "PeriodAmount/PeriodType/Frequency/ExecTimes", ErrorMessage = "When payment method is Credit and you want to installment, then {0} is required and {4} is not allow required.")]
            public bool? UnionPay { get; set; }
            /// <summary>
            /// 語系設定(預設: 空[中文], ENG[英文])
            /// </summary>
            public string Language { get; set; }
            /*
             * Credit 定期定額延伸參數。
             */
            /// <summary>
            /// 定期定額每次授權金額。
            /// </summary>
            [CompareByPaymentMethod(PaymentMethod.Credit, "CreditInstallment/InstallmentAmount/Redeem/UnionPay", ErrorMessage = "When payment method is Credit and you want to Systematic Investment Plan(SIP), then {0} is required and {4} is not allow required.")]
            public int? PeriodAmount { get; set; }
            /// <summary>
            /// 定期定額的週期種類。
            /// </summary>
            [CompareByPaymentMethod(PaymentMethod.Credit, "CreditInstallment/InstallmentAmount/Redeem/UnionPay", ErrorMessage = "When payment method is Credit and you want to Systematic Investment Plan(SIP), then {0} is required and {4} is not allow required.")]
            public PeriodType PeriodType { get; set; }
            /// <summary>
            /// 定期定額的執行頻率。
            /// </summary>
            [CompareByPaymentMethod(PaymentMethod.Credit, "CreditInstallment/InstallmentAmount/Redeem/UnionPay", ErrorMessage = "When payment method is Credit and you want to Systematic Investment Plan(SIP), then {0} is required and {4} is not allow required.")]
            public int? Frequency { get; set; }
            /// <summary>
            /// 定期定額的執行次數。
            /// </summary>
            [CompareByPaymentMethod(PaymentMethod.Credit, "CreditInstallment/InstallmentAmount/Redeem/UnionPay", ErrorMessage = "When payment method is Credit and you want to Systematic Investment Plan(SIP), then {0} is required and {4} is not allow required.")]
            public int? ExecTimes { get; set; }
            /*
             * 電子發票開立額外參數。
             */
            /// <summary>
            /// 廠商自訂編號(當 InvoiceMark=Yes 時，則必填)。
            /// </summary>
            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(30, ErrorMessage = "{0} max langth as {1}.")]
            public string RelateNumber { get; set; }
            /// <summary>
            /// 客戶代號(當 InvoiceMark=Yes 和 CarruerType=Member 時，則必填)。
            /// </summary>
            [RequiredByCarruerType(ErrorMessage = "When CarruerType equal \"Member\", then {0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string CustomerID { get; set; }
            /// <summary>
            /// 統一編號(當 InvoiceMark=Yes 時，則必填)。
            /// </summary>
            [StringLength(8, ErrorMessage = "{0} max langth as {1}.")]
            public string CustomerIdentifier { get; set; }
            /// <summary>
            /// 客戶名稱(當 InvoiceMark=Yes 時，則必填)。
            /// </summary>
            [RequiredByPrint(ErrorMessage = "When Print equal \"Yes\", then {0} is required.")]
            [StringLength(60, ErrorMessage = "{0} max langth as {1}.")]
            public string CustomerName { get; set; }
            /// <summary>
            /// 客戶地址(當 InvoiceMark=Yes 時，則必填)。
            /// </summary>
            [RequiredByPrint(ErrorMessage = "When Print equal \"Yes\", then {0} is required.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string CustomerAddr { get; set; }
            /// <summary>
            /// 客戶手機號碼(當 InvoiceMark=Yes 時，CustomerPhone 與 CustomerEmail 則責一選填，不可兩個都為空字串)。
            /// </summary>
            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string CustomerPhone { get; set; }
            /// <summary>
            /// 客戶電子信箱(當 InvoiceMark=Yes 時，CustomerPhone 與 CustomerEmail 則責一選填，不可兩個都為空字串)。
            /// </summary>
            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string CustomerEmail { get; set; }
            /// <summary>
            /// 通關方式。
            /// </summary>
            [RequiredByTaxType(ErrorMessage = "{0} is not allow none.")]
            public CustomsClearance ClearanceMark { get; set; }
            /// <summary>
            /// 課稅類別(當 InvoiceMark=Yes 時，則 CarruerType 不可以為 None)。
            /// </summary>
            [RequiredByInvoiceMark(ErrorMessage = "{0} is not allow none.")]
            public TaxationType TaxType { get; set; }
            /// <summary>
            /// 載具類別。
            /// </summary>
            public InvoiceVehicleType CarruerType { get; set; }
            /// <summary>
            /// 載具編號(當 InvoiceMark=Yes 和 CarruerType=NaturalPersonEvidence|PhoneBarcode 時，則必填)。
            /// </summary>
            [RequiredByCarruerType(ErrorMessage = "When CarruerType equal \"NaturalPersonEvidence\" or \"PhoneBarcode\", then {0} is required.")]
            [StringLength(64, ErrorMessage = "{0} max langth as {1}.")]
            public string CarruerNum { get; set; }
            /// <summary>
            /// 捐贈註記。
            /// </summary>
            [RequiredByInvoiceMark(ErrorMessage = "{0} is not allow none.")]
            public DonatedInvoice Donation { get; set; }
            /// <summary>
            /// 愛心碼。
            /// </summary>
            [RequiredByDonation(ErrorMessage = "{0} is required.")]
            [RegularExpression(@"^([Xx0-9])[0-9]{2,6}$", ErrorMessage = "{0} format error.")]
            [StringLength(7, ErrorMessage = "{0} max langth as {1}.")]
            public string LoveCode { get; set; }
            /// <summary>
            /// 列印註記。
            /// </summary>
            [RequiredByInvoiceMark(ErrorMessage = "{0} is not allow none.")]
            public PrintFlag Print { get; set; }
            /// <summary>
            /// 商品名稱(於 Arguments.Items 帶入購買商品的資訊後，系統會自動轉為 AlipayItemPrice 所需格式)。
            /// </summary>
            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(4000, ErrorMessage = "{0} max langth as {1}.")]
            public string InvoiceItemName { get; set; }
            /// <summary>
            /// 商品數量(於 Arguments.Items 帶入購買商品的資訊後，系統會自動轉為 AlipayItemPrice 所需格式)。
            /// </summary>
            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(4000, ErrorMessage = "{0} max langth as {1}.")]
            public string InvoiceItemCount { get; set; }
            /// <summary>
            /// 商品單位(於 Arguments.Items 帶入購買商品的資訊後，系統會自動轉為 AlipayItemPrice 所需格式)。
            /// </summary>
            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(4000, ErrorMessage = "{0} max langth as {1}.")]
            public string InvoiceItemWord { get; set; }
            /// <summary>
            /// 商品價格(於 Arguments.Items 帶入購買商品的資訊後，系統會自動轉為 AlipayItemPrice 所需格式)。
            /// </summary>
            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(4000, ErrorMessage = "{0} max langth as {1}.")]
            public string InvoiceItemPrice { get; set; }
            /// <summary>
            /// 商品課稅別(於 Arguments.Items 帶入購買商品的資訊後，系統會自動轉為 AlipayItemPrice 所需格式)。
            /// </summary>
            [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
            [StringLength(4000, ErrorMessage = "{0} max langth as {1}.")]
            public string InvoiceItemTaxType { get; set; }
            /// <summary>
            /// 備註。
            /// </summary>
            [StringLength(4000, ErrorMessage = "{0} max langth as {1}.")]
            public string InvoiceRemark { get; set; }
            /// <summary>
            /// 延遲天數。
            /// </summary>
            [Range(0, 15, ErrorMessage = "{0} range langth as between {1} to {2}.")]
            public int DelayDay { get; set; }
            /// <summary>
            /// 字軌類別。
            /// </summary>
            [RequiredByInvoiceMark(ErrorMessage = "{0} is not allow none.")]
            public TheWordType InvType { get; set; }
            
            /*
             * 回傳網址的延伸參數。
             */
            /// <summary>
            /// 伺服器端回傳自動櫃員機/超商/條碼付款相關資訊的網址。
            /// </summary>
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string PaymentInfoURL { get; set; }
            /// <summary>
            /// 定期定額/貨到付款的執行結果相關資訊的網址。
            /// </summary>
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string PeriodReturnURL { get; set; }

            /// <summary>
            /// 記憶卡號
            /// </summary>
            public BindingCardType BindingCard { get; set; }

            /// <summary>
            /// 記憶卡號識別碼
            /// </summary>
            [ValidateMerchantMemberID(ErrorMessage = "If the BindingCard is Yes, then the {0} must be required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string MerchantMemberID{ get; set; }

            /// <summary>
            /// 介接的延伸資料傳遞成員類別建構式。
            /// </summary>
            public SendExtendArguments()
            {
                this.ExpireDate = 3;
                this.ExpireTime = DateTime.Now.AddHours(72);
                this.PeriodType = PeriodType.None;
                this._PaymentMethod = PaymentMethod.ALL;
                // 電子發票初始值。
                this.ClearanceMark = CustomsClearance.None;
                this.TaxType = TaxationType.None;
                this.CarruerType = InvoiceVehicleType.None;
                this.Donation = DonatedInvoice.None;
                this.Print = PrintFlag.None;
                this.DelayDay = 0;
                this.InvType = TheWordType.None;
            }
        }
    }
}
