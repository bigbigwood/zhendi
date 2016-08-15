using System;
using System.Collections.Generic;
using log4net;

namespace Rld.Acs.DeviceSystem.Framework
{
    public class OperationManager
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Object _lockObject;
        private static readonly OperationManager _instance;

        private Dictionary<string, WebSocketOperation> operationDictionary;

        private OperationManager()
        {
            operationDictionary = new Dictionary<string, WebSocketOperation>();
        }

        static OperationManager()
        {
            _lockObject = new object();
            _instance = new OperationManager();
        }

        public static OperationManager GetInstance()
        {
            return _instance;
        }

        public void AddOperation(string operationGuid, WebSocketOperation op)
        {
            lock (_lockObject)
            {
                operationDictionary.Add(operationGuid, op);
            }
            Log.InfoFormat("Token:{0} has been added", operationGuid);
        }

        public void RemoveOperation(string operationGuid)
        {
            lock (_lockObject)
            {
                operationDictionary.Remove(operationGuid);
            }
            Log.InfoFormat("Token:{0} has been removed", operationGuid);
        }

        public WebSocketOperation GetOperationByToken(string token)
        {
            WebSocketOperation op;
            operationDictionary.TryGetValue(token, out op);

            return op;
        }
    }
}