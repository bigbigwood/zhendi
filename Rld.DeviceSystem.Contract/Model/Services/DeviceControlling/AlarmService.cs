using System;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services.UserCredential;

namespace Rld.DeviceSystem.Contract.Model.Services.DeviceControlling
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class AlarmService : ServiceBase
    {
        [DataMember]
        public Boolean? OverTimeOpenAlarmEnabled { get; set; }
        [DataMember]
        public Int32? OverTimeOpenAlarmSeconds { get; set; }
        [DataMember]
        public Boolean? IllegalOpenAlarmEnabled { get; set; }
        [DataMember]
        public Int32? IllegalOpenAlarmSeconds { get; set; }
        [DataMember]
        public Boolean? LinkageAlarmEnabled { get; set; }
    }
}
