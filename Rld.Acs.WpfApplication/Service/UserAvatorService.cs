using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Rld.Acs.Unility;

namespace Rld.Acs.WpfApplication.Service
{
    public class UserAvatorService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public readonly string serverUrl = ConfigurationSettings.AppSettings.Get("BaseUri") + "/userImages";
        public readonly string DefaultAvatorFileName = "avator.jpg";
        public string GetAvator(string fileName)
        {
            string fileFullPath = string.Format(@"{0}\{1}", ApplicationEnvironment.LocalImageCachePath, fileName);
            if (!File.Exists(fileFullPath))
            {
                fileFullPath = GetAvatorFromServer(fileName);
            }

            return fileFullPath;
        }

        private string GetAvatorFromServer(string fileName)
        {
            try
            {
                string requestUri = string.Format(@"{0}?name={1}", serverUrl, fileName);
                string fileFullPath = string.Format(@"{0}\{1}", ApplicationEnvironment.LocalImageCachePath, fileName);

                WebClient webClient = new WebClient();
                webClient.DownloadFile(requestUri, fileFullPath);

                return fileFullPath;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return FilePathHelper.BuildPath(string.Format(@".\{0}", DefaultAvatorFileName));;
            }
        }

        public bool UploadAvatorToServer(string fileName)
        {
            string filePath = string.Format(@"{0}\{1}", ApplicationEnvironment.LocalImageCachePath, fileName);
            System.Net.WebClient myWebClient = new System.Net.WebClient();
            var byteArray = myWebClient.UploadFile(serverUrl, "POST", filePath);
            var response = System.Text.Encoding.UTF8.GetString(byteArray);

            return response.Contains("Success");
        }
    }
}
