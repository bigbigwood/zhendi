using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rld.Acs.Repository.Exceptions;
using Rld.Acs.Repository.Framework;

namespace Rld.Acs.Repository
{
 

    public static class RepositoryManager
    {
        [ThreadStatic]
        private static IPersistanceConnection _session;

        private static IConnectionProvider _connectionProvider;
        private static readonly Object Lock = new Object();

        static RepositoryManager()
        {
            NinjectBinder.Initialize();
        }

        /// <summary>
        /// Gets factory that creates the conenctions.
        /// </summary>
        /// <returns>The connection</returns>
        public static  IConnectionProvider GetConnectionProvider()
        {
            if (_connectionProvider == null)
            {
                lock (Lock)
                {
                    _connectionProvider = NinjectBinder.Get<IConnectionProvider>();
                }
            }

            return (_connectionProvider);
        }


        /// <summary>
        /// Creates a new connection to the repository,
        /// will throw an exception if the connection was already created
        /// </summary>
        /// <returns>The created connection</returns>
        public static IPersistanceConnection GetNewConnection()
        {
            if (_session != null)
                throw new ConnectionAlreadyOpened();

            _session = GetConnectionProvider().GetConnection();
            _session.SetNotifier(SesionScopeComplete);

            return (_session);
        }

        /// <summary>
        /// Closes the conenction with the DB
        /// </summary>
        public static void CloseConnection()
        {
            if (_session == null)
                throw new ConnectionNotOpened();

            _session.Close();
            _session = null;
        }

        /// <summary>
        /// This is an scope helper to allow Idisposable and closing the backing connection
        /// </summary>
        public static void SesionScopeComplete()
        {
            _session = null;
        }

        /// <summary>
        /// Gets the connection already opened for the current thread
        /// </summary>
        /// <returns>The existing connection</returns>
        public static IPersistanceConnection GetConnection()
        {
            if (_session == null)
                throw new ConnectionNotOpened();

            return _session;
        }

        /// <summary>
        /// Gets an instance of a class implementing TRepositoryOfEntity
        /// </summary>
        /// <typeparam name="TRepositoryOfEntity">The interface of the requested repository</typeparam>
        /// <returns>the instance implementing the interface</returns>
        public static TRepositoryOfEntity GetRepository<TRepositoryOfEntity>()
        {
            return (NinjectBinder.Get<TRepositoryOfEntity>());
        }
    }
}