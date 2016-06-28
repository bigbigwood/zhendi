using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceOperationLog
    {
        public Int32 LogID { get; set; }
        public Int32? RegisterID { get; set; }
        public Int32 DeviceID { get; set; }
        public Int32 DeviceType { get; set; }
        public Int32 OperationType { get; set; }
        public Int32 OperatorID { get; set; }
        public String OperationContent { get; set; }
        public String OperationTime { get; set; }
        public String OperationUploadTime { get; set; }
    }
}
