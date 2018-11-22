using System;
using System.Collections.Generic;
using System.Text;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 付款方式子項目。
    /// </summary>
    public enum PaymentMethodItem
    {
        /// <summary>
        /// 不指定。
        /// </summary>
        None = 0,

        /*
         * WebATM 類(001~100)
         */
        /// <summary>
        /// 台新銀行。
        /// </summary>
        WebATM_TAISHIN = 1,
        /// <summary>
        /// 玉山銀行。
        /// </summary>
        WebATM_ESUN = 2,
        /// <summary>
        /// 華南銀行。
        /// </summary>
        WebATM_HUANAN = 3,
        /// <summary>
        /// 台灣銀行。
        /// </summary>
        WebATM_BOT = 4,
        /// <summary>
        /// 台北富邦。
        /// </summary>
        WebATM_FUBON = 5,
        /// <summary>
        /// 中國信託。
        /// </summary>
        WebATM_CHINATRUST = 6,
        /// <summary>
        /// 第一銀行。
        /// </summary>
        WebATM_FIRST = 7,
        /// <summary>
        /// 國泰世華。
        /// </summary>
        WebATM_CATHAY = 8,
        /// <summary>
        /// 兆豐銀行。
        /// </summary>
        WebATM_MEGA = 9,
        /// <summary>
        /// 元大銀行。
        /// </summary>
        WebATM_YUANTA = 10,
        /// <summary>
        /// 土地銀行。
        /// </summary>
        WebATM_LAND = 11,

        /*
         * ATM 類(101~200)
         */
        /// <summary>
        /// 台新銀行。
        /// </summary>
        ATM_TAISHIN = 101,
        /// <summary>
        /// 玉山銀行。
        /// </summary>
        ATM_ESUN = 102,
        /// <summary>
        /// 華南銀行。
        /// </summary>
        ATM_HUANAN = 103,
        /// <summary>
        /// 台灣銀行。
        /// </summary>
        ATM_BOT = 104,
        /// <summary>
        /// 台北富邦。
        /// </summary>
        ATM_FUBON = 105,
        /// <summary>
        /// 中國信託。
        /// </summary>
        ATM_CHINATRUST = 106,
        /// <summary>
        /// 第一銀行。
        /// </summary>
        ATM_FIRST = 107,
        /// <summary>
        /// 土地銀行。
        /// </summary>
        ATM_LAND = 108,
        /// <summary>
        /// 國泰世華銀行。
        /// </summary>
        ATM_CATHAY = 109,
        /// <summary>
        /// 大眾銀行。
        /// </summary>
        ATM_TACHONG = 200,


        /*
         * 超商類(201~300)
         */
        /// <summary>
        /// 超商代碼繳款。
        /// </summary>
        CVS = 201,
        /// <summary>
        /// OK超商代碼繳款。
        /// </summary>
        CVS_OK = 202,
        /// <summary>
        /// 全家超商代碼繳款。
        /// </summary>
        CVS_FAMILY = 203,
        /// <summary>
        /// 萊爾富超商代碼繳款。
        /// </summary>
        CVS_HILIFE = 204,
        /// <summary>
        /// 7-11 ibon代碼繳款。
        /// </summary>
        CVS_IBON = 205,

        /*
         * 其他第三方支付類(301~400)
         */
        /// <summary>
        /// 支付寶。
        /// </summary>
        Alipay = 311,
        /// <summary>
        /// 財付通。
        /// </summary>
        Tenpay = 321,

        /*
         * 儲值/餘額消費類(401~500)
         */
        /// <summary>
        /// 儲值/餘額消費(綠界科技)
        /// </summary>
        TopUpUsed_AllPay = 401,
        /// <summary>
        /// 儲值/餘額消費(玉山)
        /// </summary>
        TopUpUsed_ESUN = 402,

        /*
         * 其他類(901~999)
         */
        /// <summary>
        /// 超商條碼繳款。
        /// </summary>
        BARCODE = 901,
        /// <summary>
        /// 信用卡(MasterCard/JCB/VISA)。
        /// </summary>
        Credit = 911,
        /// <summary>
        /// 貨到付款。
        /// </summary>
        COD = 921
    }
}
