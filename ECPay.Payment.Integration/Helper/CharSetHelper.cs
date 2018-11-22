using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECPay.Payment.Integration.Helper
{
    public class CharSetHelper
    {
        public static Encoding GetCharSet(CharSetState CharSet)
        {
            Encoding type = null;

            switch (CharSet)
            {
                case CharSetState.Big5:
                    type = Encoding.GetEncoding("Big5");
                    break;
                case CharSetState.UTF8:
                    type = Encoding.UTF8;
                    break;
                case CharSetState.Default:
                default:
                    type = Encoding.Default;
                    break;
            }

            return type;
        }
    }
}
