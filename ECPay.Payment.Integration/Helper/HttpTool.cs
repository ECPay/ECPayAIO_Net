using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Security;
using System.Net;
using System.IO;

namespace ECPay.Payment.Integration.Helper
{
    internal class HttpTool
    {
        /// <summary>
        /// 不使用PostGate，且PostData為字串格式的DoRequest
        /// </summary>
        public string DoRequestStrData(string requestUrl, string sPostData, string contentType = "application/x-www-form-urlencoded")
        {
            HttpWebRequest httpWebRequest = null;

            //如果是https請求
            if (requestUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertifications);
                httpWebRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
                httpWebRequest.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                httpWebRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
            }

            //指定送出去的方式為POST
            httpWebRequest.Method = "POST";

            //設定content type, it is required, otherwise it will not work.
            httpWebRequest.ContentType = contentType;

            string receiveData;
            try
            {
                //取得request stream 並且寫入post data
                using (StreamWriter sw = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    sw.Write(sPostData);
                    sw.Close();
                }

                //取得server的reponse結果
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    receiveData = sr.ReadToEnd();
                }
            }
            catch (Exception exception)
            {
                receiveData = "{\"RtnCode\":\"99999\",\"RtnMsg\":\"連接錯誤\"}";
            }

            return receiveData;
        }


        private bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
