using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.Service.Lisence
{
    class SimpleLicense : License
    {
        public string Password { get; private set; }
        public DateTime ExpireDateTime { get; private set; }
        public int LicenseType { get; private set; }

        public SimpleLicense(string password, DateTime expireDateTime, int licenseType)
        {
            Password = password;
            ExpireDateTime = expireDateTime;
            LicenseType = licenseType;
        }

        public override void Dispose()
        {
            // TODO: 根据需要插入垃圾回收的代码
        }

        public override string LicenseKey
        {
            get { return (Password); }
        }

        public bool IsExpired
        {
            get { return ExpireDateTime <= DateTime.Now; }
        }
    }
}
