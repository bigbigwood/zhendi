using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Quartz;
using Rld.Acs.Backend.Service.Email;
using Rld.Acs.Model;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Framework;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;
using StmpMailTest;
using Zhuoqin.HXSH.Web.Utility;

namespace Rld.Acs.Backend.Jobs
{
    class DeviceAlarmJob : JobBase
    {
        private const string LastDeviceAlarmJobScanTime = "DeviceAlarmJobScanTime";
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceAlarmJobEmail emailTemplate;

        protected override void ProcessBusiness(IJobExecutionContext context)
        {
            var sysConfigRepo = RepositoryManager.GetRepository<ISysConfigRepository>();
            var deviceTrafficLogRepo = RepositoryManager.GetRepository<IDeviceTrafficLogRepository>();
            emailTemplate = DeviceAlarmJobEmail.BuildEmailInfo();
            var lastDeviceAlarmJobScanTimeConfig = sysConfigRepo.Query(new Hashtable { { ConstStrings.Name, LastDeviceAlarmJobScanTime } }).FirstOrDefault();
            DateTime lastDeviceAlarmJobScanTime = DateTime.Parse(lastDeviceAlarmJobScanTimeConfig.Value);
            DateTime currentDeviceAlarmJobScanTime  = DateTime.Now;

           var conditions = new Hashtable()
           {
               {"RecordType", 1}, // 告警记录类型为1
               {"StartDate",lastDeviceAlarmJobScanTime},
               {"EndDate", currentDeviceAlarmJobScanTime},
           };
            var alarmLogs = deviceTrafficLogRepo.Query(conditions);
            Log.InfoFormat("Detect {0} alarm logs...", alarmLogs.Count());
            if (alarmLogs.Any())
            {
                var alarmEmailAccountsConfig = sysConfigRepo.GetConfigValueByName(ConstStrings.AlarmEmailAccounts);
                var alarmEmailAccounts = ParseAccounts(alarmEmailAccountsConfig);
                if (alarmEmailAccounts.Any())
                {
                    alarmLogs.ForEach(x => SendEmail(x, alarmEmailAccounts));
                }

                var alarmSmsAccountsConfig = sysConfigRepo.GetConfigValueByName(ConstStrings.AlarmSMSAccounts);
                var alarmSmsAccounts = ParseAccounts(alarmSmsAccountsConfig);
                if (alarmSmsAccounts.Any())
                {
                    alarmLogs.ForEach(x => SendSms(x, alarmSmsAccounts));
                }
            }

            lastDeviceAlarmJobScanTimeConfig.Value = currentDeviceAlarmJobScanTime.ToString();
            sysConfigRepo.Update(lastDeviceAlarmJobScanTimeConfig);
        }

        private List<String> ParseAccounts(string accountConfig)
        {
            List<string> result = new List<string>();
            if (string.IsNullOrWhiteSpace(accountConfig))
                return result;

            var accounts = accountConfig.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return accounts;
        }

        private void SendEmail(DeviceTrafficLog log, List<String> accounts)
        {
            try
            {
                foreach (var account in accounts)
                {
                    var m = new SmtpConfig();
                    var mFrom = new MailAddress(emailTemplate.FromAddress, emailTemplate.FromAccountDisplayName, Encoding.UTF8);
                    var mTo = new MailAddress(account, account, Encoding.UTF8);
                    string subject = emailTemplate.Subject;
                    string body = emailTemplate.Body;
                    string res = SmtpMail.MailTo(m, mFrom, mTo, subject, body);
                    Log.InfoFormat("send email to {0}, result:{1}", account, res);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void SendSms(DeviceTrafficLog log, List<String> accounts)
        {
            try
            {
                var sysConfigRepo = RepositoryManager.GetRepository<ISysConfigRepository>();
                foreach (var account in accounts)
                {
                    string msg = sysConfigRepo.GetConfigValueByName("DeviceAlarmJobSms_Body");
                    UcpaasMessage messageService = new UcpaasMessage();
                    bool sendSuccess = messageService.SendMessage(account,((int) UcpaasMessage.MessageType.CancelFail).ToString(), msg);

                    Log.InfoFormat("send sms to {0}, result: {1}", account, sendSuccess);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
