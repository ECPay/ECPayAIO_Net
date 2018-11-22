using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
//using System.Linq.Expressions;
using System.Reflection;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 商品項目。
    /// </summary>
    public class Item
    {
        /// <summary>
        /// 商品名稱。
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商品金額。
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 商品貨幣。
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 商品訂購數量。
        /// </summary>
        public int Quantity { get; set;}
        /// <summary>
        /// 商品單位(當 InvoiceMark=Yes 時，則必填)。
        /// </summary>
        [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
        public string Unit { get; set; }
        /// <summary>
        /// 商品課稅別(當 InvoiceMark=Yes 時，則必填)。
        /// </summary>
        [RequiredByInvoiceMark(ErrorMessage = "{0} is required.")]
        public TaxationType TaxType { get; set; }
        /// <summary>
        /// 商品銷售(介紹)的網址。
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 商品項目的建構式。
        /// </summary>
        public Item()
        {
            this.TaxType = TaxationType.None;
        }
    }
    /// <summary>
    /// 商品項目的集合類別。
    /// </summary>
    public class ItemCollection : List<Item>
    {
        /// <summary>
        /// 將物件加入至商品集合的結尾。
        /// </summary>
        /// <param name="item">要加入至商品集合結尾的物件。</param>
        public new void Add(Item item)
        {
            base.Add(item);
            this.RaiseCollectionEvents("Add");
        }
        /// <summary>
        /// 將特定商品集合的元素加入至商品集合的結尾。
        /// </summary>
        /// <param name="collection">商品集合，其元素應加入至商品集合的結尾。集合本身不能是 null，但它可以包含 null 的元素。</param>
        public new void AddRange(IEnumerable<Item> collection)
        {
            base.AddRange(collection);
            this.RaiseCollectionEvents("AddRange");
        }
        /// <summary>
        /// 將所有元素從商品集合移除。
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            this.RaiseCollectionEvents("Clear");
        }
        /// <summary>
        /// 將項目插入商品集合中指定的索引處。
        /// </summary>
        /// <param name="index">應在該處插入 item 之以零起始的索引。</param>
        /// <param name="item">要插入的物件。</param>
        public new void Insert(int index, Item item)
        {
            base.Insert(index, item);
            this.RaiseCollectionEvents("Insert");
        }
        /// <summary>
        /// 將商品集合的元素插入至位於指定索引的商品集合中。
        /// </summary>
        /// <param name="index">應插入新元素處的以零起始的索引。</param>
        /// <param name="collection">商品集合，其項目應插入至商品集合。集合本身不能是 null，但它可以包含 null 的項目。</param>
        public new void InsertRange(int index, IEnumerable<Item> collection)
        {
            base.InsertRange(index, collection);
            this.RaiseCollectionEvents("InsertRange");
        }
        /// <summary>
        /// 從商品集合移除特定物件的第一個相符項目。
        /// </summary>
        /// <param name="item">要從商品集合中移除的物件。參考型別的值可以是 null。</param>
        /// <returns>如果成功移除 item 則為 true，否則為 false。如果在商品集合中找不到 item，則這個方法也會傳回 false。</returns>
        public new bool Remove(Item item)
        {
            bool blResult = base.Remove(item);
            this.RaiseCollectionEvents("Remove");
            return blResult;
        }
        /// <summary>
        /// 移除符合指定之述詞所定義的條件之所有項目。
        /// </summary>
        /// <param name="match">定義要移除項目之條件的 System.Predicate&lt;T&gt; 委派。</param>
        /// <returns>商品集合中已移除的項目數。</returns>
        public new int RemoveAll(Predicate<Item> match)
        {
            int nResult = base.RemoveAll(match);
            this.RaiseCollectionEvents("RemoveAll");
            return nResult;
        }
        /// <summary>
        /// 移除商品集合中指定之索引處的項目。
        /// </summary>
        /// <param name="index">要移除元素之以零起始的索引。</param>
        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            this.RaiseCollectionEvents("RemoveAt");
        }
        /// <summary>
        /// 從商品集合移除的元素範圍。
        /// </summary>
        /// <param name="index">要移除之元素範圍內之以零起始的起始索引。</param>
        /// <param name="count">要移除的元素數目。</param>
        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            this.RaiseCollectionEvents("RemoveRange");
        }
        /// <summary>
        /// 資料集合被異動時的事件。
        /// </summary>
        public event ItemCollectionEventHandler CollectionChanged;
        /// <summary>
        /// 實作事件觸發時要處理的方法。
        /// </summary>
        /// <param name="methodName">屬性參數。</param>
        protected virtual void RaiseCollectionEvents(string methodName)
        {
            if (this.CollectionChanged != null) this.CollectionChanged(this, new ItemCollectionEventArgs(this, methodName));
        }
    }
}
