using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.DeviceSystem.Model.User
{
    public enum UserRole
    {
        User = 1,
        Registrar = 2,
        LogQuery = 4,
        Manager = 8,
        Custom = 16,    }
}
