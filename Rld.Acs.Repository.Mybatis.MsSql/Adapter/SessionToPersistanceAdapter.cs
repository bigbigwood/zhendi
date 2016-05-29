using System;
using System.Data;
using IBatisNet.DataMapper;
using Rld.Acs.Repository.Framework;

namespace Rld.Acs.Repository.Mybatis.MsSql
{
    internal class SessionToPersistanceAdapter : IPersistanceConnection
    {
         ISqlMapper _session;
        ClearSessionForThread _notifier;

        public SessionToPersistanceAdapter(ISqlMapper iSession)
        {
            this._session = iSession;
        }

        public void Close()
        {
            if (_session.IsSessionStarted)
                _session.CloseConnection();
        }

        public void Dispose()
        {
            _notifier.Invoke();

            Close();
        }

        public ISqlMapper GetUndelayingSession()
        {
            return _session;
        }

        public IPersistanceTransaction BeginTransaction()
        {
            return (new TransactionToPersistanceTransaction(_session));
        }

        public IPersistanceTransaction BeginTransaction(IsolationLevel level)
        {
            return (new TransactionToPersistanceTransaction(level, _session));
        }

        public void SetNotifier(ClearSessionForThread notifier)
        {
            if (notifier == null)
                throw new ArgumentNullException("notifier");

            _notifier = notifier;
        }


    }
}