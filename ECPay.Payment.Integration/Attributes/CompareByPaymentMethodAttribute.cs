using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 依據付款方式檢核比較該填寫資料欄位的類別。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class CompareByPaymentMethodAttribute : RequiredAttribute
    {
        /// <summary>
        /// 付款方式。
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }
        /// <summary>
        /// 要檢查的屬性欄位名稱(多個以/符號分隔)。
        /// </summary>
        public string ConfirmPropertyNames { get; set; }

        /// <summary>
        /// 依據付款方式檢核比較該填寫資料欄位的類別建構式。
        /// </summary>
        /// <param name="paymentMethod">付款方式。</param>
        /// <param name="confirmPropertyNames">要檢查的屬性欄位名稱(多個以/符號分隔)。</param>
        public CompareByPaymentMethodAttribute(PaymentMethod paymentMethod, string confirmPropertyNames) : base()
        {
            if (confirmPropertyNames == null)
                throw new ArgumentNullException("confirmPropertyNames is null.");

            this.PaymentMethod = paymentMethod;
            this.ConfirmPropertyNames = confirmPropertyNames;
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

            string[] szPropertyNames = this.ConfirmPropertyNames.Split(new char[] { '/' });

            pdcProperties = TypeDescriptor.GetProperties(oSourceComponent);

            object oMethodValue = pdcProperties.Find("_PaymentMethod", true).GetValue(oSourceComponent);

            if (this.PaymentMethod.Equals(oMethodValue))
            {
                if (base.IsValid(oPropertyValue) && !PeriodType.None.Equals(oPropertyValue))
                {
                    foreach (string szPropertyName in szPropertyNames)
                    {
                        object oConfirmValue = pdcProperties.Find(szPropertyName, true).GetValue(oSourceComponent);

                        if (!PeriodType.None.Equals(oConfirmValue))
                            if (null != oConfirmValue) return false;
                    }
                }
            }

            return true;
        }
    }
}
