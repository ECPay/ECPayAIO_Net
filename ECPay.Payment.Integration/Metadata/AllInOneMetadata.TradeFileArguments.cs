using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AllInOneMetadata
    {
        /// <summary>
        /// 廠商下載對帳媒體檔的資料傳遞成員類別。
        /// </summary>
        public class TradeFileArguments : INotifyPropertyChanged
        {
            /// <summary>
            /// 查詢日期類別。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public TradeDateType DateType { get; set; }
            /// <summary>
            /// 查詢開始日期。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            [RegularExpression("^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$", ErrorMessage = "{0} format is yyyy-MM-dd.")]
            public string BeginDate { get; set; }
            /// <summary>
            /// 查詢結束日期。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            [RegularExpression("^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])$", ErrorMessage = "{0} format is yyyy-MM-dd.")]
            public string EndDate { get; set; }
            /// <summary>
            /// 付款方式。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public PaymentMethod PaymentType { get; set; }
            /// <summary>
            /// 訂單類型。
            /// </summary>
            public PlatformState PlatformStatus { get; set; }
            /// <summary>
            /// 付款狀態。
            /// </summary>
            public PaymentState PaymentStatus { get; set; }
            /// <summary>
            /// 撥款狀態。
            /// </summary>
            public AllocateState AllocateStatus { get; set; }
            /// <summary>
            /// CSV 格式。
            /// </summary>
            public bool NewFormatedMedia { get; set; }
            /// <summary>
            /// 檔案編碼格式
            /// </summary>
            public CharSetState CharSet { get; set; }
            /// <summary>
            /// 廠商下載對帳媒體檔介接參數的建構式。
            /// </summary>
            public TradeFileArguments() : base()
            {
                this.DateType = TradeDateType.Payment;
                this.PaymentType = PaymentMethod.ALL;
                this.PlatformStatus = PlatformState.ALL;
                this.PaymentStatus = PaymentState.ALL;
                this.AllocateStatus = AllocateState.ALL;
                this.NewFormatedMedia = true;
                this.CharSet = CharSetState.Default;
            }
            /// <summary>
            /// 屬性變更的事件。
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;
            /// <summary>
            /// 屬性變更時，觸發變更事件的方法。
            /// </summary>
            /// <typeparam name="T">來源型別。</typeparam>
            /// <param name="property">屬性。</param>
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
