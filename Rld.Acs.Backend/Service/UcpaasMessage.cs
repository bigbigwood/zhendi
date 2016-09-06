using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Configuration;
//using Zhuoqin.HXSH.Utility;
using log4net;

namespace Zhuoqin.HXSH.Web.Utility
{
    public class UcpaasMessage
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region parameters

        private string strServiceIp;// = "api.ucpaas.com";
        private string strServicePort;// = "443";
        private string strAccount;// = "5a917be70a8459d9dcfd4f2ddd4eaf76";
        private string strToken;// = "da95d9696bf35d82aa7ad397267ba82c";
        private string strAppId;// = "66a6122656204c86bea843f1c4734072";
        private string strApiVersion;// = "2014-06-30";
        private string strTemplateId;// = "12055";

        #endregion

        public enum RequestBodyType
        {
            XMLType = 0,
            JSONType = 1,
        }

        public enum MessageType
        {
            ModifyPsw = 26456,
            CancelFail = 26169,
            CancelSuccess = 26168,
            RegistFail = 26167,

            RegistSuccess = 26166,
            ImportConflictCancelGoods = 26465,
            ImportConflictCancelFlow = 26464,

            ModifyRegist = 26557,
            ForgetPwd = 27483
        }

        public UcpaasMessage()
        {
            this.strServiceIp = ConfigurationManager.AppSettings["YZXserverIp"];
            this.strServicePort = ConfigurationManager.AppSettings["YZXserverPort"];
            this.strAccount = ConfigurationManager.AppSettings["YZXaccount"];
            this.strToken = ConfigurationManager.AppSettings["YZXtoken"];
            this.strAppId = ConfigurationManager.AppSettings["YZXappId"];
            this.strApiVersion = ConfigurationManager.AppSettings["YZXapiVersion"];
            this.strTemplateId = ConfigurationManager.AppSettings["YZXtemplateId"];
        }

        public UcpaasMessage(string serviceIp, string servicePort, string account, string token, string appId, string apiVersion, string templateId)
        {
            this.strServiceIp = serviceIp;
            this.strServicePort = servicePort;
            this.strAccount = account;
            this.strToken = token;
            this.strAppId = appId;
            this.strApiVersion = apiVersion;
            this.strTemplateId = templateId;

        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="to">接受短信的手机号</param>
        /// <param name="param">内容数据，用于替换模板中{数字}</param>
        /// <returns>是否发送短信成功</returns>
        public bool SendMessage(string to, string param)
        {
            return SendMessage(to, this.strTemplateId, param);
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="to">短信接收端手机号码</param>
        /// <param name="templateId">短信模板ID</param>
        /// <param name="param">内容数据，用于替换模板中{数字}</param>
        /// <exception cref="ArgumentNullException">参数不能为空</exception>
        /// <exception cref="Exception"></exception>
        /// <returns>包体内容</returns>
        public bool SendMessage(string to, string templateId, string param)
        {

            bool isSucess = false;

            RequestBodyType bodyType = RequestBodyType.JSONType;

            if (to == null)
            {
                throw new ArgumentNullException("to");
            }

            if (templateId == null)
            {
                throw new ArgumentNullException("templateId");
            }

            try
            {
                // 构建URL内容
                string currentDate = DateTime.Now.ToString("yyyyMMddHHmmss");
                string strSignature = MD5Encrypt(strAccount + strToken + currentDate);
                string suffixName = bodyType == RequestBodyType.XMLType ? ".xml" : "";
                string strURL = string.Format("https://{0}:{1}/{2}/Accounts/{3}/Messages/templateSMS{4}?sig={5}", strServiceIp, strServicePort, strApiVersion, strAccount, suffixName, strSignature);
                Uri address = new Uri(strURL);

                // 创建网络请求  
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
                setCertificateValidationCallBack();

                // set method
                request.Method = "POST";

                //set authorization
                Encoding myEncoding = Encoding.GetEncoding("utf-8");
                byte[] myByte = myEncoding.GetBytes(strAccount + ":" + currentDate);
                string strAuth = Convert.ToBase64String(myByte);
                request.Headers.Add("Authorization", strAuth);


                // 构建Body
                StringBuilder data = new StringBuilder();
                if (bodyType == RequestBodyType.XMLType)
                {
                    request.Accept = "application/xml";
                    request.ContentType = "application/xml;charset=utf-8";

                    data.Append("<?xml version='1.0' encoding='utf-8'?><templateSMS>");
                    data.Append("<appId>").Append(strAppId).Append("</appId>");
                    data.Append("<templateId>").Append(templateId).Append("</templateId>");
                    data.Append("<to>").Append(to).Append("</to>");
                    data.Append("<param>").Append(param).Append("</param>");
                    data.Append("</templateSMS>");
                }
                else
                {
                    request.Accept = "application/json";
                    request.ContentType = "application/json;charset=utf-8";

                    data.Append("{");
                    data.Append("\"templateSMS\":{");
                    data.Append("\"appId\":\"").Append(strAppId).Append("\"");
                    data.Append(",\"templateId\":\"").Append(templateId).Append("\"");
                    data.Append(",\"to\":\"").Append(to).Append("\"");
                    data.Append(",\"param\":\"").Append(param).Append("\"");
                    data.Append("}}");
                }

                byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
                // 开始请求
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                // 获取请求
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    //get response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string strResponse = reader.ReadToEnd();
                    Log.Info("发送短信回调" + strResponse);
                    if (strResponse != null && strResponse.Length > 0)
                    {

                        if (strResponse.Contains("000000"))
                        {
                            isSucess = true;
                        }
                    }
                }


            }
            catch (Exception e)
            {
                Log.Warn("发送短信异常" + to + "  " + templateId + " ", e);
                throw e;
            }

            return isSucess;

        }

        /// <summary>
        /// 设置服务器证书验证回调
        /// </summary>
        public void setCertificateValidationCallBack()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = CertificateValidationResult;
        }

        /// <summary>
        ///  证书验证回调函数  
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cer"></param>
        /// <param name="chain"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool CertificateValidationResult(object obj, System.Security.Cryptography.X509Certificates.X509Certificate cer, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors error)
        {
            return true;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="source">原内容</param>
        /// <returns>加密后内容</returns>
        private string MD5Encrypt(string source)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(source));

            // Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}