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


        protected static void Configure(object obj)
        {
        }

        protected static ISqlMapper InitMapper()
        {
            ConfigureHandler handler = new ConfigureHandler(Configure);
            DomSqlMapBuilder builder = new DomSqlMapBuilder();
            return builder.Configure();
        }
    }
}