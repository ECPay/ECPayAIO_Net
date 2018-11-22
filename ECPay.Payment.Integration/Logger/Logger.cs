using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Web;

namespace ECPay.Payment.Integration
{
    /// <summary>
    /// 記錄檔處理的靜態類別。
    /// </summary>
    internal static class Logger
    {
        /// <summary>
        /// 處理 IO 資料的物件。
        /// </summary>
        private static StreamWriter swLogger = null;
        /// <summary>
        /// 記錄檔儲存路徑格式。
        /// </summary>
        private const string LOGGER_FULL_FILENAME = "~/App_Data/_allpay/{0}.log";
        /// <summary>
        /// 取得目前要使用的記錄檔檔名。
        /// </summary>
        private static string FileName
        {
            get {
                string szFileName = String.Format(LOGGER_FULL_FILENAME, DateTime.Now.ToString("yyyyMMddHH"));

                if (null != HttpContext.Current)
                {
                    szFileName = HttpContext.Current.Server.MapPath(szFileName);
                }
                else
                {
                    szFileName = LOGGER_FULL_FILENAME.Replace("~", Assembly.GetAssembly(typeof(Logger)).CodeBase);
                    szFileName = szFileName.Replace("file:///", String.Empty);
                    szFileName = szFileName.Replace("/", "\\");
                }

                return szFileName;
            }
        }
        /// <summary>
        /// 記錄檔處理的靜態類別建構式。
        /// </summary>
        static Logger()
        {
            CreateInstance();
        }
        /// <summary>
        /// 寫入記錄檔。
        /// </summary>
        /// <param name="message">要寫入的訊息。</param>
        public static void Write(string message)
        {
            try
            {
                CreateInstance();
                swLogger.Write(message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// 寫入一行記錄檔。
        /// </summary>
        /// <param name="message">要寫入的訊息。</param>
        public static void WriteLine(string message)
        {
            try
            {
                CreateInstance();
                swLogger.WriteLine(message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// 建立處理 IO 資料的物件實例。
        /// </summary>
        private static void CreateInstance()
        {
            DirectoryInfo dirInfo = null;
            FileInfo fileInfo = null;

            try
            {
                fileInfo = new FileInfo(FileName);

                if (fileInfo.Exists)
                {
                    if (null == swLogger)
                    {
                        swLogger = File.AppendText(FileName);
                        swLogger.AutoFlush = true;
                    }
                }
                else
                {
                    dirInfo = new DirectoryInfo(fileInfo.DirectoryName);

                    if (!dirInfo.Exists) dirInfo.Create();

                    if (null != swLogger)
                    {
                        swLogger.Close();
                        swLogger.Dispose();
                        swLogger = null;
                    }

                    swLogger = File.CreateText(FileName);
                    swLogger.AutoFlush = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                fileInfo = null;
                dirInfo = null;
            }
        }
    }
}
