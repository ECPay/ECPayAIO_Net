using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 依據付款方式檢核比較欄位字串是否相符於付款方式的類別。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class EqualsByPaymentMethodAttribute : ValidationAttribute
    {
        /// <summary>
        /// 依據付款方式檢核比較欄位字串是否相符於付款方式的類別建構式。
        /// </summary>
        public EqualsByPaymentMethodAttribute() : base()
        {
        }
        /// <summary>
        /// 是否檢核通過。
        /// </summary>
        /// <param name="value">要檢核的物件類別。</param>
        /// <returns>驗證成功為 True 否則為 False。</returns>
        public override bool IsValid(object value)
        {
            object[] oValues = (object[])value;

            object oPropertyName = oValues[0]; // 屬性的名稱。
            object oPropertyValue = oValues[1]; // 屬性的值。
            object oSourceComponent = oValues[2]; // 該屬性所屬物件。
            object oRelatedComponent = oValues[3]; // 驗證時需要的相關連物件。

            string szParameters = (null != oPropertyValue ? oPropertyValue.ToString() : null);

            if (!String.IsNullOrEmpty(szParameters))
            {
                string[] saParameters = szParameters.Split(new char[] { '#' });

                foreach (string szParameter in saParameters)
                {
                    try
                    {
                        PaymentMethod paymentMethod = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), szParameter);

                        if (paymentMethod == PaymentMethod.ALL) return false;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
