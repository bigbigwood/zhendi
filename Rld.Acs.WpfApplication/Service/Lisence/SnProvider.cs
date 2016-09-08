using System.Management;
using Rld.Acs.WpfApplication.Models;

namespace Rld.Acs.WpfApplication.Service.Lisence
{
    static class SnProvider
    {
        #region -> Private Variables

        public static bool UseProcessorID;
        public static bool UseBaseBoardProduct;
        public static bool UseBaseBoardManufacturer;
        public static bool UseDiskDriveSignature;
        public static bool UseVideoControllerCaption;
        public static bool UsePhysicalMediaSerialNumber;
        public static bool UseBiosVersion;
        public static bool UseBiosManufacturer;
        public static bool UseWindowsSerialNumber;

        #endregion

        public static string GetSystemInfo(string SoftwareName)
        {
            if (UseProcessorID == true)
                SoftwareName += RunQuery("Processor", "ProcessorId");

            if (UseBaseBoardProduct == true)
                SoftwareName += RunQuery("BaseBoard", "Product");

            if (UseBaseBoardManufacturer == true)
                SoftwareName += RunQuery("BaseBoard", "Manufacturer");

            if (UseDiskDriveSignature == true)
                SoftwareName += RunQuery("DiskDrive", "Signature");

            if (UseVideoControllerCaption == true)
                SoftwareName += RunQuery("VideoController", "Caption");

            if (UsePhysicalMediaSerialNumber == true)
                SoftwareName += RunQuery("PhysicalMedia", "SerialNumber");

            if (UseBiosVersion == true)
                SoftwareName += RunQuery("BIOS", "Version");

            if (UseWindowsSerialNumber == true)
                SoftwareName += RunQuery("OperatingSystem", "SerialNumber");

            SoftwareName = RemoveUseLess(SoftwareName);

            if (SoftwareName.Length < 25)
                return GetSystemInfo(SoftwareName);

            return SoftwareName.Substring(0, 25).ToUpper();
        }

        private static string RemoveUseLess(string st)
        {
            char ch;
            for (int i = st.Length - 1; i >= 0; i--)
            {
                ch = char.ToUpper(st[i]);

                if ((ch < 'A' || ch > 'Z') &&
                    (ch < '0' || ch > '9'))
                {
                    st = st.Remove(i, 1);
                }
            }
            return st;
        }

        private static string RunQuery(string TableName, string MethodName)
        {
            ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * from Win32_" + TableName);
            foreach (ManagementObject MO in MOS.Get())
            {
                return MO[MethodName].ToString();
            }
            return "";
        }

        public static string CalculateSN()
        {
            return CalculateSN(AppConfiguration.ApplicationName);
        }
        public static string CalculateSN(string softwareName)
        {
            UseBaseBoardProduct = true;
            UseBiosVersion = true;
            UseProcessorID = true;

            var sn = SnProvider.GetSystemInfo(softwareName);
            var _BaseString = Encryption.Boring(Encryption.InverseByBase(sn, 10));

            return _BaseString;
        }
    }
}
