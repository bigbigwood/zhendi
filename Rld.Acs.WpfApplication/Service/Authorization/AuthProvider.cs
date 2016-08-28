using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.Service.Authorization
{
    public abstract class AuthProvider
    {
        private static AuthProvider _instance;

        /// <summary>
        /// This method determines whether the user is authorize to perform the requested operation
        /// </summary>
        public abstract bool CheckAccess(string operation);

        /// <summary>
        /// This method determines whether the user is authorize to perform the requested operation
        /// </summary>
        public abstract bool CheckAccess(object commandParameter);


        public static void Initialize<TProvider>() where TProvider : AuthProvider, new()
        {
            _instance = new TProvider();
        }

        public static void Initialize<TProvider>(object[] parameters)
        {
            _instance = (AuthProvider)typeof(TProvider).GetConstructor(new Type[] { typeof(object[]) }).Invoke(new object[] { parameters });
        }

        public static AuthProvider Instance
        {
            get { return _instance; }
        }
    }
}
