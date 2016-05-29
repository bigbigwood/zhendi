using System.Xml;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;
using Rld.Acs.Repository.Framework;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    public class MyBatisConnectionProvider : IConnectionProvider
    {
        private ISqlMapper _sqlMapper;

        public MyBatisConnectionProvider()
        {
            this._sqlMapper = InitMapper();
        }

        public IPersistanceConnection GetConnection()
        {
            return new SessionToPersistanceAdapter(_sqlMapper);
        }


        public static string path = "daan.sqlserver.config";
        private static readonly string key = "daanhealth2011";
        private static readonly string datakeyfile = "prop.config";

        protected static void Configure(object obj)
        {
        }

        protected static ISqlMapper InitMapper()
        {
            ConfigureHandler handler = new ConfigureHandler(Configure);
            DomSqlMapBuilder builder = new DomSqlMapBuilder();

            //XmlDocument doc = GetConfig();

            return builder.Configure();
        }

        private static XmlDocument GetConfig()
        {
            string uid = string.Empty;
            string pwd = string.Empty;
            string db = string.Empty;
            string source = string.Empty;
            XmlDocument doc = Resources.GetResourceAsXmlDocument(datakeyfile);
            XmlNodeList nodeList = doc.SelectNodes("settings/add");
            int n = nodeList.Count;
            if (n >= 3)
            {
                for (int i = 0; i < n; i++)
                {
                    switch (nodeList[i].Attributes["key"].Value.ToLower())
                    {
                        case "userid":
                            uid = nodeList[i].Attributes["value"].Value;
                            break;
                        case "password":
                            pwd = nodeList[i].Attributes["value"].Value;
                            break;
                        case "database":
                            db = nodeList[i].Attributes["value"].Value;
                            break;
                        case "datasource":
                            source = nodeList[i].Attributes["value"].Value;
                            break;
                    }
                }
            }

            XmlDocument configDoc = Resources.GetResourceAsXmlDocument(path);
            configDoc.InnerXml = configDoc.InnerXml.Replace("${datasource}", source).Replace("${userid}", uid).Replace("${password}", pwd);

            return configDoc;
        }
    }
}