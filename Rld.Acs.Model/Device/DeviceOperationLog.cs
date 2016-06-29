using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceOperationLog
    {
        public virtual Int32 LogID { get; set; }
        public virtual Int32? RegisterID { get; set; }
        public virtual Int32 DeviceID { get; set; }
        public virtual Int32 DeviceType { get; set; }
        public virtual Int32 OperationType { get; set; }
        public virtual Int32 OperatorID { get; set; }
        public virtual String OperationContent { get; set; }
        public virtual String OperationTime { get; set; }
        public virtual String OperationUploadTime { get; set; }
    }
}
