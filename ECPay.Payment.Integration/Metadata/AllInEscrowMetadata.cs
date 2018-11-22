using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 履約保證介接參數的類別。
    /// </summary>
    public partial class AllInEscrowMetadata : CommonMetadata
    {
        /// <summary>
        /// 介接產生訂單的基本資料傳遞成員。
        /// </summary>
        public SendArguments Send { get; private set; }
        /// <summary>
        /// 介接訂單查詢的資料傳遞成員。
        /// </summary>
        public QueryArguments Query { get; private set; }
        /// <summary>
        /// 介接出貨通知的資料傳遞成員。
        /// </summary>
        public DeliveryArguments Delivery { get; private set; }
        /// <summary>
        /// 商品異常/取消訂單處理結果的資料傳遞成員。
        /// </summary>
        public OrderChangeArguments Change { get; private set; }

        /// <summary>
        /// 履約保證介接參數的建構式。
        /// </summary>
        public AllInEscrowMetadata()
        {
            this.ServiceMethod = HttpMethod.HttpPOST;

            this.Send = new SendArguments();
            this.Send.PropertyChanged += new PropertyChangedEventHandler(this.Send_PropertyChanged);
            this.Query = new QueryArguments();
            this.Delivery = new DeliveryArguments();
            this.Change = new OrderChangeArguments();
        }
        /// <summary>
        /// 特定屬性參數修改時觸發同步異動的事件。
        /// </summary>
        /// <param name="sender">來源物件。</param>
        /// <param name="e">提供 System.ComponentModel.INotifyPropertyChanged.PropertyChanged 事件的資料。</param>
        private void Send_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Items")
            {
                if (this.Send.Items.Count > 0)
                {
                    string szItemName = String.Empty;
                    string szItemURL = String.Empty;

                    foreach (Item oItem in this.Send.Items)
                    {
                        szItemName += String.Format("{0} {1}{2}x{3}#", oItem.Name, oItem.Price, oItem.Currency, oItem.Quantity);
                        if (String.IsNullOrEmpty(szItemURL)) szItemURL = oItem.URL;
                    }

                    szItemName = szItemName.Substring(0, szItemName.Length - 1);
                    szItemName = szItemName.Substring(0, (szItemName.Length > 200 ? 200 : szItemName.Length));

                    this.Send._ItemName = szItemName;
                    this.Send._ItemURL = szItemURL;
                }
            }
        }
    }
}
