using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 依據付款方式檢查該欄位是否必填的類別。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class RequiredByPaymentMethodAttribute : RequiredAttribute
    {
        /// <summary>
        /// 付款方式。
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// 依據付款方式檢查該欄位是否必填的類別建構式。
        /// </summary>
        /// <param name="paymentMethod">付款方式。</param>
        public RequiredByPaymentMethodAttribute(PaymentMethod paymentMethod) : base()
        {
            this.PaymentMethod = paymentMethod;
        }
        /// <summary>
        /// 是否檢核通過。
        /// </summary>
        /// <param name="value">要檢核的物件類別。</param>
        /// <returns>驗證成功為 True 否則為 False。</returns>
        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection pdcProperties = null;

            object[] oValues = (object[])value;

            object oPropertyName = oValues[0]; // 屬性的名稱。
            object oPropertyValue = oValues[1]; // 屬性的值。
            object oSourceComponent = oValues[2]; // 該屬性所屬物件。
            object oRelatedComponent = oValues[3]; // 驗證時需要的相關連物件。

            pdcProperties = TypeDescriptor.GetProperties(oSourceComponent);

            object oMethodValue = pdcProperties.Find("_PaymentMethod", true).GetValue(oSourceComponent);

            if (this.PaymentMethod.Equals(oMethodValue))
                return base.IsValid(oPropertyValue);

            return true;
        }
    }
}
