using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rld.DeviceSystem.Contract.Model.Configuration;

namespace Rld.DeviceSystem.Contract.Interface
{
    public interface IDevice
    {
        Boolean IsRunning { get; set; }
        Boolean Start(DeviceConfiguration deviceConfig);
        Boolean ReStart(DeviceConfiguration deviceConfig);
        Boolean Stop();
        void ReceiveMessage(String message);
        void SendMessage(String message);
    }
}
