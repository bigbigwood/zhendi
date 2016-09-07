using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.Backend.Service.Email
{
    class DeviceAlarmJobEmail
    {
        public string FromAddress { get; set; }
        public string FromAccountDisplayName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public static DeviceAlarmJobEmail BuildEmailInfo()
        {
            var sysConfigRepo = RepositoryManager.GetRepository<ISysConfigRepository>();
            var template = new DeviceAlarmJobEmail();
            template.FromAddress = sysConfigRepo.GetConfigValueByName("DeviceAlarmJobEmail_FromAddress");
            template.FromAccountDisplayName = sysConfigRepo.GetConfigValueByName("DeviceAlarmJobEmail_FromAccountDisplayName");
            template.Subject = sysConfigRepo.GetConfigValueByName("DeviceAlarmJobEmail_Subject");
            template.Body = sysConfigRepo.GetConfigValueByName("DeviceAlarmJobEmail_Body");

            return template;
        }
        
    }
}
