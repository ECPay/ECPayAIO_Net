using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        /// 介接廠商退款的資料傳遞成員類別。
        /// </summary>
        public class ChargeBackArguments : INotifyPropertyChanged
        {
            /// <summary>
            /// 廠商交易編號。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string MerchantTradeNo { get; set; }
            /// <summary>
            /// AllPay 交易編號。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            [StringLength(20, ErrorMessage = "{0} max langth as {1}.")]
            public string TradeNo { get; set; }
            /// <summary>
            /// 退款金額。
            /// </summary>
            [Required(ErrorMessage = "{0} is required.")]
            public decimal ChargeBackTotalAmount { get; set; }
            /// <summary>
            /// 執行的動作。
            /// </summary>
            [StringLength(100, ErrorMessage = "{0} max langth as {1}.")]
            public string Remark { get; set; }
            /// <summary>
            /// 廠商退款介接參數的建構式。
            /// </summary>
            public ChargeBackArguments()
            {

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
