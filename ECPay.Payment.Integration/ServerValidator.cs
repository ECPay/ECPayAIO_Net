using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Reflection;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 伺服器端的 Model 驗證。
    /// </summary>
    internal static class ServerValidator
    {
        /// <summary>
        /// 要驗證的結構描述。
        /// </summary>
        /// <param name="source">主要驗證的元件。</param>
        /// <returns>驗證的訊息內容。</returns>
        public static IEnumerable<string> Validate(object source)
        {
            return ServerValidator.Validate(null, source);
        }

        /// <summary>
        /// 要驗證的結構描述。
        /// </summary>
        /// <param name="relation">相關要驗證的元件。</param>
        /// <param name="source">主要驗證的元件。</param>
        /// <returns>驗證的訊息內容。</returns>
        public static IEnumerable<string> Validate(object relation, object source)
        {
            foreach (PropertyInfo propInfo in source.GetType().GetProperties())
            {
                object[] customAttributes = propInfo.GetCustomAttributes(typeof(ValidationAttribute), inherit: true);

                foreach (object customAttribute in customAttributes)
                {
                    ValidationAttribute validationAttribute = (ValidationAttribute)customAttribute;

                    bool isValid = false;
                    // 預設驗證的 Attributes。
                    if (validationAttribute.GetType() == typeof(RequiredAttribute) || validationAttribute.GetType() == typeof(RangeAttribute)
                        || validationAttribute.GetType() == typeof(RegularExpressionAttribute) || validationAttribute.GetType() == typeof(StringLengthAttribute))
                    {
                        isValid = validationAttribute.IsValid(propInfo.GetValue(source, BindingFlags.GetProperty, null, null, null));
                    }
                    // 自訂驗證的 Attributes。
                    else
                    {
                        isValid = validationAttribute.IsValid(new object[] { propInfo.Name, propInfo.GetValue(source, BindingFlags.GetProperty, null, null, null), source, relation });
                    }

                    if (!isValid)
                    {
                        yield return validationAttribute.FormatErrorMessage(propInfo.Name);
                    }
                }
            }
        }
    }
}
