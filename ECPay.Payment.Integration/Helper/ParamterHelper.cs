using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ECPay.Payment.Integration.Attributes;

namespace ECPay.Payment.Integration.Helper
{
    internal class ParamterHelper
    {
        /// <summary>
        /// 字典轉換成參數
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public string DictionaryToParamter<TKey, TValue>( IDictionary<TKey, TValue> dict) 
        {
            return string.Join("&", dict.Select(p => p.Key + "=" + p.Value).ToArray());
        }

        //private IEnumerable<object> SearchProps(object target)
        //{
        //    foreach (var prop in target.GetType().GetProperties())
        //    {
        //        yield return RealProp(prop, target);
        //    }
        //}

        //public object RealProp(PropertyInfo prop, object target)
        //{
        //    object Result = null;
        //    var value = prop.GetValue(target, null);
        //    if (null != value)
        //    {
        //        if (prop.GetCustomAttributes(typeof(ParamterAttribute),false).Count() > 0)
        //        {
        //            //如果是參數模型
        //            Result = SearchProps(value);
        //        }
        //        else
        //        {
        //            Result = value;
        //        }
        //    }
        //    return Result;
        //}

    }
}
