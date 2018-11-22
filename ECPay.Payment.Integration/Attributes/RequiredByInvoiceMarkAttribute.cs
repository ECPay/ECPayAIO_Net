using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 依據是否開立電子發票檢查該欄位是否必填的類別。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class RequiredByInvoiceMarkAttribute : RequiredAttribute
    {
        /// <summary>
        /// 手機或郵件擇一條件檢查用的參數。
        /// </summary>
        private string[] szaPhoneOrEmail = new string[] { "CustomerPhone", "CustomerEmail" };
        /// <summary>
        /// 手機或郵件擇一條件檢查用的參數。
        /// </summary>
        private string[] szaAllowEmpty = new string[] { "CustomerID", "CustomerIdentifier", "CustomerName", "CustomerAddr" };
        /// <summary>
        /// 依據是否開立電子發票檢查該欄位是否必填的類別建構式。
        /// </summary>
        public RequiredByInvoiceMarkAttribute() : base()
        {
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

            if (null != oRelatedComponent)
            {
                pdcProperties = TypeDescriptor.GetProperties(oRelatedComponent);

                object oInvoiceMark = pdcProperties.Find("InvoiceMark", true).GetValue(oRelatedComponent);

                if (oInvoiceMark.Equals(InvoiceState.Yes))
                {
                    // 不可為 Null，但允許空字串。
                    bool isValid = base.IsValid(oPropertyValue);
                    // 特殊驗證：手機或郵件必須則一填寫(不可以為空字串)。
                    if (!isValid && szaPhoneOrEmail.Contains(oPropertyName))
                    {
                        object oNeedCheckedValue = null;

                        pdcProperties = TypeDescriptor.GetProperties(oSourceComponent);

                        if (oPropertyName.Equals("CustomerPhone")) oNeedCheckedValue = pdcProperties.Find("CustomerEmail", true).GetValue(oSourceComponent);
                        if (oPropertyName.Equals("CustomerEmail")) oNeedCheckedValue = pdcProperties.Find("CustomerPhone", true).GetValue(oSourceComponent);

                        return base.IsValid(oNeedCheckedValue);
                    }
                    // 特殊驗證：課稅類別不可以為 None。
                    else if (oPropertyName.Equals("TaxType"))
                    {
                        return !oPropertyValue.Equals(TaxationType.None);
                    }
                    // 特殊驗證：捐贈註記不可以為 None。
                    else if (oPropertyName.Equals("Donation"))
                    {
                        return !oPropertyValue.Equals(DonatedInvoice.None);
                    }
                    // 特殊驗證：列印註記不可以為 None。
                    else if (oPropertyName.Equals("Print"))
                    {
                        return !oPropertyValue.Equals(PrintFlag.None);
                    }
                    // 特殊驗證：字軌類別不可以為 None。
                    else if (oPropertyName.Equals("InvType"))
                    {
                        return !oPropertyValue.Equals(TheWordType.None);
                    }
                    // 特殊驗證：允許空字串欄位。
                    else if (szaAllowEmpty.Contains(oPropertyName))
                    {
                        return oPropertyValue.Equals(String.Empty);
                    }

                    return isValid;
                }
            }

            return true;
        }
    }
}
