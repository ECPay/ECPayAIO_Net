using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ECPay.Payment.Integration.Extensions
{
    public static class AttributeExtension
    {
        public static TValue GetAttributeValue<TAttribute, TValue, TEnum>
            (
                this TEnum e,
                Func<TAttribute,TValue> seletor
            ) 
            where TAttribute : Attribute
            where TEnum : struct , IConvertible
        {
            var type = typeof(TEnum);
            var field = type.GetField(e.ToString());
            TAttribute attr = Attribute.GetCustomAttribute(field, typeof(TAttribute)) as TAttribute;

            if (attr !=null)
            {
                return seletor(attr);
            }

            return default(TValue);
        }
    }
}
