using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model
{
    public enum GetLogOptions
    {
        GetAllLogs,
        GetNewLogs,
    }

    public class QueryLogCondictions
    {
        public GetLogOptions Options { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Boolean CleanNewLogPosition { get; set; }
    }
}