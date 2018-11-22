using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace ECPay.Payment.Integration
{
    [ServiceContract(Namespace = "http://Payment.allpay.com.tw/")]
    internal interface IAllPayService
    {
        [OperationContract(Name = "QueryTrade", Action = "http://Payment.allpay.com.tw/QueryTrade", ReplyAction = "*")]
        string QueryTrade(string MerchantID, string XmlData);
        [OperationContract(Name = "QueryTradeInfo", Action = "http://Payment.allpay.com.tw/QueryTradeInfo", ReplyAction = "*")]
        string QueryTradeInfo(string MerchantID, string MerchantTradeNo, int TimeStamp, string CheckMacValue);
    }
}
