﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using log4net;

namespace Rld.Acs.DeviceSystem.Framework
{
    public class WebSocketOperation
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private String _response;
        private Int32 _websocketClientId;
        private CancellationTokenSource blockToken;
        public String Token { get; private set; }
        public WebSocketOperation(int websocketClientId)
        {
            blockToken = new CancellationTokenSource();
            Token = Guid.NewGuid().ToString();
            _websocketClientId = websocketClientId;
        }

        public string Execute(string request)
        {
            var task1 = new Task<string>(() =>
            {
                OperationManager.GetInstance().AddOperation(Token, this);
                WebSocketClientManager.GetInstance().GetClientById(_websocketClientId).Send(request);
                Log.InfoFormat("request {0}", request);

                while (!blockToken.IsCancellationRequested)
                {
                    Thread.Sleep(300);
                }

                OperationManager.GetInstance().RemoveOperation(Token);
                Log.InfoFormat("return {0}", _response);
                return _response;
            }, blockToken.Token);

            task1.Start();
            task1.Wait();

            Log.InfoFormat("task1.Result: {0}", task1.Result);
            return task1.Result;
        }

        public void FillResponse(string response)
        {
            _response = response;
            blockToken.Cancel();
            Log.Info("continue invoked");
        }
    }
}