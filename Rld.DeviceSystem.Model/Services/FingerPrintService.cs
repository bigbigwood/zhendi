using System;

namespace Rld.DeviceSystem.Model.Services
{
    public class FingerPrintService : CredentialService
    {
        public Int32 Index { get; set; }
        public String FingerPrintData { get; set; }
    }
}
