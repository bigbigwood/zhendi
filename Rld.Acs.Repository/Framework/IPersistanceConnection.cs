using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Rld.Acs.Repository.Framework
{

    /// <summary>
    /// Signature to the method that will be invoked when the session is closed. 
    /// </summary>
    public delegate void ClearSessionForThread();

    /// <summary>
    /// An interface to abstract the connectivity to different storage systems ORMs
    /// </summary>
    public interface IPersistanceConnection : IDisposable
    {
        /// <summary>
        /// Sets The notifier to invoke when the Session is disposed
        /// </summary>
        /// <param name="notifier">The notifier to invoke when the Session is disposed</param>
        void SetNotifier(ClearSessionForThread notifier);

        /// <summary>
        /// Close the connection to the persitance layer
        /// </summary>
        void Close();

        /// <summary>
        /// Starts a transaction in the persistance layer
        /// </summary>
        /// <returns>The transaction open</returns>
        IPersistanceTransaction BeginTransaction();

        /// <summary>
        /// Starts a transaction in the persistance layer
        /// </summary>
        /// <param name="level">The data isolation level to start the transaction <see cref="System.Data.IsolationLevel"/></param>
        /// <returns>The transaction open</returns>
        IPersistanceTransaction BeginTransaction(IsolationLevel level);
    }
}