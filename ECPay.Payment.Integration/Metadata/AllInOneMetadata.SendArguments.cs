using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 全功能介接參數的類別。
    /// </summary>
    public partial class AllInOneMetadata
    {
        /// <summary>
        /// 介接的基本資料傳遞成員類別。
        /// </summary>
        public new class SendArguments : CommonMetadata.SendArguments, INotifyPropertyChanged
        {
            private PaymentMethod _ChoosePayment;
            /// <summary>
            /// 付款方式。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public PaymentMethod ChoosePayment
            {
                get { return this._ChoosePayment; }
                set
                {
                    this._ChoosePayment = value;
                    this.RaisePropertyEvents(p => p.ChoosePayment);
                }
            }
            /// <summary>
            /// 付款子項目(預設: None)。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public PaymentMethodItem ChooseSubPayment { get; set; }
            /// <summary>
            /// 是否需要額外的付款資訊(預設: No)。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public ExtraPaymentInfo NeedExtraPaidInfo { get; set; }
            /// <summary>
            /// 裝置來源(預設: PC)。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public DeviceType DeviceSource { get; set; }
            /// <summary>
            /// 不顯示的付款方式(預設: 空)。
            /// </summary>
            [EqualsByPaymentMethod(ErrorMessage = "The {0} string must be same as the PaymentMethod, and not allow as \"ALL\".")]
            [StringLength(100, ErrorMessage = "{0} max langth as {1}.")]
            public string IgnorePayment { get; set; }
            /// <summary>
            /// 特約合作平台商代號(預設: 空)。
            /// </summary>
            [StringLength(10, ErrorMessage = "{0} max langth as {1}.")]
            public string PlatformID { get; set; }
            /// <summary>
            /// 特約合作平台商手續費(預設: 無)。
            /// </summary>
            [ValidatePlatformArguments(ErrorMessage = "If the PlatformID is empty, then the {0} must be empty.")]
            public int? PlatformChargeFee { get; set; }
            /// <summary>
            /// 是否要延遲撥款(※買方付款完成後，需再呼叫「廠商申請撥款／退款」API，綠界科技才會撥款給廠商，或退款給買方。倘若廠商一直不申請撥款，此筆訂單款項會一直放在綠界科技，直到廠商申請撥款，且該功能不支援「信用卡」)。
            /// </summary>
            public HoldTradeType HoldTradeAMT { get; set; }
            /// <summary>
            /// 綠界科技的會員編號。
            /// </summary>
            [ValidatePlatformArguments(ErrorMessage = "If the PlatformID is empty, then the {0} must be empty.")]
            [StringLength(10, ErrorMessage = "{0} max langth as {1}.")]
            public string AllPayID { get; set; }
            /// <summary>
            /// 綠界科技的會員識別碼。
            /// </summary>
            [ValidatePlatformArguments(ErrorMessage = "If the PlatformID is empty, then the {0} must be empty.")]
            [StringLength(50, ErrorMessage = "{0} max langth as {1}.")]
            public string AccountID { get; set; }
            /// <summary>
            /// 電子發票開立註記(預設: 空)。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public InvoiceState InvoiceMark { get; set; }
            /// <summary>
            /// 用戶端回傳付款結果的網址(※設定了此參數值，會使設定的ClientBackURL失效)。
            /// </summary>
            [RegularExpression(@"^(?:http|https|ftp)://[a-zA-Z0-9\.\-]+(?:\:\d{1,5})?(?:[A-Za-z0-9\.\;\:\@\&\=\+\$\,\?/_]|%u[0-9A-Fa-f]{4}|%[0-9A-Fa-f]{2})*$", ErrorMessage = "{0} is not correct URL.")]
            [StringLength(200, ErrorMessage = "{0} max langth as {1}.")]
            public string OrderResultURL { get; set; }

            /// <summary>
            /// 合 作 特 店商店代碼(預設: 空)。
            /// </summary>
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string StoreID { get; set; }

            /// <summary>
            /// 自 訂 名 稱欄位 1(預設: 空)。
            /// </summary>
            [StringLength(50, ErrorMessage = "{0} max langth as {1}.")]
            public string CustomField1 { get; set; }

            /// <summary>
            /// 自 訂 名 稱欄位 2(預設: 空)。
            /// </summary>
            [StringLength(50, ErrorMessage = "{0} max langth as {1}.")]
            public string CustomField2 { get; set; }

            /// <summary>
            /// 自 訂 名 稱欄位 3(預設: 空)。
            /// </summary>
            [StringLength(50, ErrorMessage = "{0} max langth as {1}.")]
            public string CustomField3 { get; set; }

            /// <summary>
            /// 自 訂 名 稱欄位 4(預設: 空)。
            /// </summary>
            [StringLength(50, ErrorMessage = "{0} max langth as {1}.")]
            public string CustomField4 { get; set; }

            /// <summary>
            /// CheckMacValue 加密類型
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public int EncryptType { get; set; }

            /// <summary>
            /// 介接的基本資料傳遞成員類別建構式。
            /// </summary>
            public SendArguments() : base()
            {
                this.PaymentType = "aio";
                this.ChoosePayment = PaymentMethod.ALL;
                this.ChooseSubPayment = PaymentMethodItem.None;
                this.NeedExtraPaidInfo = ExtraPaymentInfo.No;
                this.HoldTradeAMT = HoldTradeType.No;
                this.InvoiceMark = InvoiceState.No;
                this.DeviceSource = DeviceType.PC;
                this.Items.CollectionChanged += new ItemCollectionEventHandler(this.Items_CollectionChanged);
            }

            /// <summary>
            /// 當產品集合被異動時，觸發連動的事件。
            /// </summary>
            /// <param name="sender">來源物件。</param>
            /// <param name="e">提供 System.ComponentModel.INotifyPropertyChanged.PropertyChanged 事件的資料。</param>
            private void Items_CollectionChanged(object sender, ItemCollectionEventArgs e)
            {
                if (!String.IsNullOrEmpty(e.MethodName))
                    this.RaisePropertyEvents(p => p.Items);
            }
            /// <summary>
            /// 屬性值變更時，所要觸發的事件。
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;
            /// <summary>
            /// 實作事件觸發時要處理的方法。
            /// </summary>
            /// <typeparam name="T">觸發事件的屬性型別。</typeparam>
            /// <param name="property">屬性參數。</param>
            protected virtual void RaisePropertyEvents<T>(Expression<Func<SendArguments, T>> property)
            {
                MemberExpression meExpression = property.Body as MemberExpression;

                if (meExpression == null || meExpression.Expression != property.Parameters[0] || meExpression.Member.MemberType != MemberTypes.Property)
                    throw new InvalidOperationException("Now tell me about the property");

                if (this.PropertyChanged != null) this.PropertyChanged(this, new PropertyChangedEventArgs(meExpression.Member.Name));
            }
        }
    }
}
