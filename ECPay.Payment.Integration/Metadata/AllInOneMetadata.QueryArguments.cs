using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
//using System.Linq.Expressions;
using System.Reflection;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 全功能介接參數的類別。
    /// </summary>
    public partial class AllInOneMetadata
    {
        /// <summary>
        /// 介接訂單查詢的資料傳遞成員類別。
        /// </summary>
        public new class QueryArguments : CommonMetadata.QueryArguments
        {
            /// <summary>
            /// 介接訂單查詢的資料建構式。
            /// </summary>
            public QueryArguments() : base()
            {
            }
        }
    }
}
