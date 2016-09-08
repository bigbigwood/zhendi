using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Rld.Acs.WpfApplication.Service.Lisence
{
    [LicenseProvider(typeof(SimpleLicenseProvider))]
    class LisenceCheckWorker : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        SimpleLicense _License = null; // _License 用于保存许可证信息

        public LisenceCheckWorker()
        {
            try
            {
                _License = LicenseManager.Validate(typeof(LisenceCheckWorker), this) as SimpleLicense; // 在这里调用LicenseManager进行许可证验证
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public SimpleLicense GetLisence()
        {
            return _License;
        }

        public void Dispose()
        {
            if (_License != null)
            {
                _License.Dispose(); // 调用_License的Dispose方法，显示释放资源
            }
        }
    }
}
