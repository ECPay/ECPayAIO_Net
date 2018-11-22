using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 全功能介接參數的類別。
    /// </summary>
    public partial class AllInOneMetadata : CommonMetadata
    {
        /// <summary>
        /// 介接產生訂單的基本資料傳遞成員。
        /// </summary>
        public SendArguments Send { get; private set; }
        /// <summary>
        /// 介接產生訂單的延伸資料傳遞成員。
        /// </summary>
        public SendExtendArguments SendExtend { get; private set; }
        /// <summary>
        /// 介接訂單查詢的資料傳遞成員。
        /// </summary>
        public QueryArguments Query { get; private set; }
        /// <summary>
        /// 介接信用卡關帳/退刷/取消/放棄的資料傳遞成員。
        /// </summary>
        public ActionArguments Action { get; private set; }
        /// <summary>
        /// 介接廠商退款的資料傳遞成員。
        /// </summary>
        public ChargeBackArguments ChargeBack { get; private set; }

        /// <summary>
        /// 介接廠商下載對帳媒體檔的資料傳遞成員。
        /// </summary>
        public TradeFileArguments TradeFile { get; private set; }

        /// <summary>
        /// 全功能介接參數的建構式。
        /// </summary>
        public AllInOneMetadata()
        {
            this.ServiceMethod = HttpMethod.HttpPOST;

            this.Send = new SendArguments();
            this.Send.PropertyChanged += new PropertyChangedEventHandler(this.Send_PropertyChanged);
            this.SendExtend = new SendExtendArguments();

            this.Query = new QueryArguments();
            this.Action = new ActionArguments();
            this.ChargeBack = new ChargeBackArguments();
            this.TradeFile = new TradeFileArguments();
        }

        /// <summary>
        /// 特定屬性參數修改時觸發同步異動的事件。
        /// </summary>
        /// <param name="sender">來源物件。</param>
        /// <param name="e">提供 System.ComponentModel.INotifyPropertyChanged.PropertyChanged 事件的資料。</param>
        private void Send_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ChoosePayment")
            {
                this.SendExtend._PaymentMethod = this.Send.ChoosePayment;
			}
            if (e.PropertyName == "Items")
            {
				if (this.Send.Items.Count > 0)
				{
                    // 一般描述
                    string szItemName = String.Empty;
                    string szItemURL = String.Empty;
                    // 阿里巴巴
                    string szAlipayItemName = String.Empty;
                    string szAlipayItemCounts = String.Empty;
                    string szAlipayItemPrice = String.Empty;
                    // 電子發票
                    string szInvoiceItemName = String.Empty;
                    string szInvoiceItemCount = String.Empty;
                    string szInvoiceItemWord = String.Empty;
                    string szInvoiceItemPrice = String.Empty;
                    string szInvoiceItemTaxType = String.Empty;

					foreach (Item oItem in this.Send.Items)
					{
                        // 一般描述
						szItemName += String.Format("{0} {1} {2} x {3}#", oItem.Name, oItem.Price, oItem.Currency, oItem.Quantity);
                        if (String.IsNullOrEmpty(szItemURL)) szItemURL = oItem.URL;
                        // 阿里巴巴
						szAlipayItemName += String.Format("{0}#", oItem.Name);
						szAlipayItemCounts += String.Format("{0}#", oItem.Quantity);
						szAlipayItemPrice += String.Format("{0}#", oItem.Price);
                        // 電子發票
                        szInvoiceItemName += String.Format("{0}|", oItem.Name);
                        szInvoiceItemCount += String.Format("{0}|", oItem.Quantity);
                        szInvoiceItemWord += String.Format("{0}|", oItem.Unit);
                        szInvoiceItemPrice += String.Format("{0}|", oItem.Price);
                        szInvoiceItemTaxType += String.Format("{0}|", (oItem.TaxType == TaxationType.None ? String.Empty : ((int)oItem.TaxType).ToString()));
					}
                    // 一般描述
                    szItemName = szItemName.Substring(0, szItemName.Length - 1);
                    szItemName = szItemName.Substring(0, (szItemName.Length > 200 ? 200 : szItemName.Length));
                    // 阿里巴巴
                    szAlipayItemName = szAlipayItemName.Substring(0, szAlipayItemName.Length - 1);
                    szAlipayItemName = szAlipayItemName.Substring(0, (szAlipayItemName.Length > 200 ? 200 : szAlipayItemName.Length));
					szAlipayItemCounts = szAlipayItemCounts.Substring(0, szAlipayItemCounts.Length - 1);
                    szAlipayItemCounts = szAlipayItemCounts.Substring(0, (szAlipayItemCounts.Length > 100 ? 100 : szAlipayItemCounts.Length));
					szAlipayItemPrice = szAlipayItemPrice.Substring(0, szAlipayItemPrice.Length - 1);
                    szAlipayItemPrice = szAlipayItemPrice.Substring(0, (szAlipayItemPrice.Length > 20 ? 20 : szAlipayItemPrice.Length));
                    // 電子發票
                    szInvoiceItemName = szInvoiceItemName.Substring(0, szInvoiceItemName.Length - 1);
                    szInvoiceItemCount = szInvoiceItemCount.Substring(0, szInvoiceItemCount.Length - 1);
                    szInvoiceItemWord = (szInvoiceItemWord.Length == this.Send.Items.Count ? String.Empty : szInvoiceItemWord.Substring(0, szInvoiceItemWord.Length - 1));
                    szInvoiceItemPrice = szInvoiceItemPrice.Substring(0, szInvoiceItemPrice.Length - 1);
                    szInvoiceItemTaxType = (szInvoiceItemTaxType.Length == this.Send.Items.Count ? String.Empty : szInvoiceItemTaxType.Substring(0, szInvoiceItemTaxType.Length - 1));

                    this.Send._ItemName = szItemName;
                    this.Send._ItemURL = szItemURL;
                    this.SendExtend.AlipayItemName = szAlipayItemName;
                    this.SendExtend.AlipayItemCounts = szAlipayItemCounts;
                    this.SendExtend.AlipayItemPrice = szAlipayItemPrice;
                    this.SendExtend.InvoiceItemName = szInvoiceItemName;
                    this.SendExtend.InvoiceItemCount = szInvoiceItemCount;
                    this.SendExtend.InvoiceItemWord = szInvoiceItemWord;
                    this.SendExtend.InvoiceItemPrice = szInvoiceItemPrice;
                    this.SendExtend.InvoiceItemTaxType = szInvoiceItemTaxType;
                }
            }
        }
    }
}
