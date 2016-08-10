using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDeviceAccessLogRequest : RequestBase
    {
        [DataMember]
        public DateTime BeginTime { get; set; }
        [DataMember]
        public DateTime EndTime { get; set; }

        public GetDeviceAccessLogRequest()
        {
            BeginTime = new DateTime(2015,1,1);
            EndTime = new DateTime(2099,12,31);
        }
    }
}
