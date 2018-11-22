using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 履約保證介接參數的類別。
    /// </summary>
    public partial class AllInEscrowMetadata
    {
        /// <summary>
        /// 介接的基本資料傳遞成員類別。
        /// </summary>
        public new class SendArguments : CommonMetadata.SendArguments, INotifyPropertyChanged
        {
            /// <summary>
            /// 幣別。
            /// </summary>
            [StringLength(5, ErrorMessage = "{0} max langth as {1}.")]
            public string Currency { get; set; }
            /// <summary>
            /// 中文編碼格式。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(10, ErrorMessage = "{0} max langth as {1}.")]
            public string EncodeChartset { get; set; }
            /// <summary>
            /// 是否採用 Allpay 平台提供的住址(0: 不使用, 1: 使用)。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public bool UseAllpayAddress { get; set; }
            /// <summary>
            /// 刷卡分期期數(不提供分期，預設 0)。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public int CreditInstallment { get; set; }
            /// <summary>
            /// 使用刷卡分期的付款金額(不使用信用卡分期時，預設 0)。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public decimal InstallmentAmount { get; set; }
            /// <summary>
            /// 信用卡是否使用紅利折抵(使用紅利折抵時，請設為 Y 時)。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public bool Redeem { get; set; }
            /// <summary>
            /// 預計出貨日。
            /// </summary>
            [StringLength(8, ErrorMessage = "{0} max langth as {1}.")]
            public string ShippingDate { get; set; }
            /// <summary>
            /// 商品猶豫期時間(預設 168 小時)。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public int ConsiderHour { get; set; }
            /// <summary>
            /// 介接的基本資料傳遞成員類別建構式。
            /// </summary>
            public SendArguments() : base()
            {
                this.PaymentType = "allpay";
                this.EncodeChartset = "utf-8";
                this.UseAllpayAddress = false;
                this.CreditInstallment = 0;
                this.InstallmentAmount = Decimal.Zero;
                this.ConsiderHour = 168;
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
