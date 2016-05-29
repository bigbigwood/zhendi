using System.Data;
using Rld.Acs.Repository.Framework;
using IBatisNet.DataMapper;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    /// <summary>
    /// Abstraction class from NHibernate ITransaction to CRM IPersistanceTransaction
    /// </summary>
    internal class TransactionToPersistanceTransaction : IPersistanceTransaction
    {
        private ISqlMapSession _tran;
        private ISqlMapper _session;

        public TransactionToPersistanceTransaction(ISqlMapper session)
        {
            _session = session;
            _tran = _session.BeginTransaction();
        }

        public TransactionToPersistanceTransaction(IsolationLevel level, ISqlMapper session)
        {
            _session = session;
            _tran = _session.BeginTransaction(level);
        }

        public void Commit()
        {
            _tran.CommitTransaction();
        }

        public void Rollback()
        {
            _tran.RollBackTransaction();
        }

        public void Dispose()
        {
            if (_tran.IsTransactionStart)
            {
                Rollback();
            }
        }
    }
}