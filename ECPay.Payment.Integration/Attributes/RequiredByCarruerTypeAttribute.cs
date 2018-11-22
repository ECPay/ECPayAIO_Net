using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 依據載具類型檢查該欄位是否必填的類別。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class RequiredByCarruerTypeAttribute : RequiredAttribute
    {
        /// <summary>
        /// 依據載具類型檢查該欄位是否必填的類別建構式。
        /// </summary>
        public RequiredByCarruerTypeAttribute() : base()
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
                    bool isValid = (oPropertyValue != null);
                    // 特殊驗證：當會員載具是綠界科技會員時時，客戶代號不可以為空值。
                    if (oPropertyName.Equals("CustomerID"))
                    {
                        object oNeedCheckedValue = null;

                        pdcProperties = TypeDescriptor.GetProperties(oSourceComponent);

                        oNeedCheckedValue = pdcProperties.Find("CarruerType", true).GetValue(oSourceComponent);

                        if (oNeedCheckedValue.Equals(InvoiceVehicleType.Member))
                        {
                            return base.IsValid(oPropertyValue);
                        }
                    }
                    // 特殊驗證：當會員載具是電子發票或手機條碼時，載具編號不可以為空值。
                    else if (oPropertyName.Equals("CarruerNum"))
                    {
                        object oNeedCheckedValue = null;

                        pdcProperties = TypeDescriptor.GetProperties(oSourceComponent);

                        oNeedCheckedValue = pdcProperties.Find("CarruerType", true).GetValue(oSourceComponent);

                        if (oNeedCheckedValue.Equals(InvoiceVehicleType.NaturalPersonEvidence) || oNeedCheckedValue.Equals(InvoiceVehicleType.PhoneBarcode))
                        {
                            return base.IsValid(oPropertyValue);
                        }
                    }

                    return isValid;
                }
            }

            return true;
        }
    }
}
