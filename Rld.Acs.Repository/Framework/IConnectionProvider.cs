using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rld.Acs.Repository.Framework
{
    public interface IConnectionProvider
    {
        /// <summary>
        /// Gets a connection from the persistance layer
        /// </summary>
        /// <returns>the new connection</returns>
        IPersistanceConnection GetConnection();
    }
}