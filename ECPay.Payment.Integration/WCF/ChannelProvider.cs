using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Configuration;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 此處理站可建立不同型別的通道，用戶端使用這些通道將訊息傳送給各種已設定的服務端點。
    /// </summary>
    internal static class ChannelProvider
    {
        /// <summary>
        /// 建立指定的型別通道。
        /// </summary>
        /// <typeparam name="TChannel">由通道處理站產生的通道型別。這個型別必須是 System.ServiceModel.Channels.IOutputChannel 或 System.ServiceModel.Channels.IRequestChannel。</typeparam>
        /// <param name="uri">提供服務位置的位址(設定檔使用的模式為 CLASS 動態連結檔時，請帶入 Assembly Qualified Name：若組件有強勢名稱請帶入完整的 Assembly Qualified Name)。
        /// Assembly Qualified Name 範例：System.Web.UI.WebControls.TextBox, System.Web、System.Web.UI.WebControls.TextBox, System.Web, Version=4.0.0.0、
        /// System.Web.UI.WebControls.TextBox, System.Web, Version=4.0.0.0, Culture=neutral 或 System.Web.UI.WebControls.TextBox, System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
        /// </param>
        /// <returns>由處理站所建立之 System.ServiceModel.Channels.IChannel 型別的 TChannel。</returns>
        public static TChannel CreateChannel<TChannel>(string uri)
        {
            TChannel oChannel = default(TChannel);

            Binding oBinding = new BasicHttpBinding();
            EndpointAddress oEndpoint = new EndpointAddress(new Uri(uri));
            ChannelFactory<TChannel> oFacroty = new ChannelFactory<TChannel>(oBinding, oEndpoint);

            oChannel = oFacroty.CreateChannel();

            return oChannel;
        }
    }
}
