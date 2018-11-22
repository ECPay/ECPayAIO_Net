using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration.Attributes
{
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
    public class TextAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
