using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceOperationLog
    {
        public virtual Int32 LogID { get; set; }
        public virtual Int32? DeviceUserId { get; set; }
        public virtual Int32 DeviceId { get; set; }
        public virtual String DeviceCode { get; set; }
        public virtual String DeviceType { get; set; }
        public virtual Int32 OperationType { get; set; }
        public virtual String OperationDescription { get; set; }
        public virtual Int32 OperatorId { get; set; }
        public virtual String OperationContent { get; set; }
        public virtual DateTime? OperationTime { get; set; }
        public virtual DateTime? OperationUploadTime { get; set; }
    }
}
