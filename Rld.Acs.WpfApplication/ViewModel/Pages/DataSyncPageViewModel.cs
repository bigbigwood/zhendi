using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using log4net;
using Microsoft.SqlServer.Server;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models.Command;
using Rld.Acs.WpfApplication.Models.Messages;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.ViewModel.Views;

namespace Rld.Acs.WpfApplication.ViewModel.Pages
{
    public class DataSyncPageViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISysConfigRepository _sysConfigRepository = NinjectBinder.GetRepository<ISysConfigRepository>();

        public RelayCommand SaveCmd { get; private set; }
        public ObservableCollection<SysConfigViewModel> DataSyncJobTimeConfigViewModels { get; set; }

        public List<DeviceRole> DeviceRoles
        {
            get
            {
                var deviceRoles = ApplicationManager.GetInstance().AuthorizationDeviceRoles;
                deviceRoles.Insert(0, new DeviceRole(){DeviceRoleID = 0, RoleName = ""});
                return deviceRoles;
            }
        }

        public DeviceRole DefaultDeviceRole { get; set; }

        public List<Department> Departments
        {
            get
            {
                var depts = ApplicationManager.GetInstance().AuthorizationDepartments.FindAll(x => x.DepartmentID != -1);
                depts.Insert(0, new Department() {DepartmentID = 0, Name = ""});
                return depts;
            }
        }

        public Department DefaultDepartment { get; set; }

        public DataSyncPageViewModel()
        {
            SaveCmd = new AuthCommand(SaveDataSyncSettings);
            DataSyncJobTimeConfigViewModels = new ObservableCollection<SysConfigViewModel>(InitDataSyncJobTimeConfigs());
            DefaultDepartment = InitDefaultDepartment();
            DefaultDeviceRole = InitDeviceRole();
        }

        private List<SysConfigViewModel> InitDataSyncJobTimeConfigs()
        {
            var configViewModels = new List<SysConfigViewModel>();
            for (int index = 0; index < 10; index++)
            {
                configViewModels.Add(new SysConfigViewModel() { Value = "00:00:00" });
            }

            var config = _sysConfigRepository.Query(new Hashtable()).FirstOrDefault(x => x.Name == ConstStrings.DataSyncConfig);
            if (config != null)
            {
                var configs = config.Value.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
                for (int index = 0; index < configs.Count(); index++)
                {
                    configViewModels[index].Value = configs[index];
                    configViewModels[index].IsSelected = true;
                }
            }

            return configViewModels;
        }

        private Department InitDefaultDepartment()
        {
            var config = _sysConfigRepository.Query(new Hashtable()).FirstOrDefault(x => x.Name == ConstStrings.DataSyncDefaultDepartment);
            if (config != null && !string.IsNullOrWhiteSpace(config.Value))
            {
                return Departments.FirstOrDefault(x => x.DepartmentID == config.Value.ToInt32());
            }
            else
            {
                return Departments.First();
            }
        }

        private DeviceRole InitDeviceRole()
        {
            var config = _sysConfigRepository.Query(new Hashtable()).FirstOrDefault(x => x.Name == ConstStrings.DataSyncDefaultRole);
            if (config != null && !string.IsNullOrWhiteSpace(config.Value))
            {
                return DeviceRoles.FirstOrDefault(x => x.DeviceRoleID == config.Value.ToInt32());
            }
            else
            {
                return DeviceRoles.First();
            }
        }

        private void SaveDataSyncSettings()
        {
            try
            {
                var defaultDepartmentConfig = _sysConfigRepository.Query(new Hashtable()).FirstOrDefault(x => x.Name == ConstStrings.DataSyncDefaultDepartment);
                var defaultRoleConfig = _sysConfigRepository.Query(new Hashtable()).FirstOrDefault(x => x.Name == ConstStrings.DataSyncDefaultRole);
                var dataSyncConfig = _sysConfigRepository.Query(new Hashtable()).FirstOrDefault(x => x.Name == ConstStrings.DataSyncConfig);
                
                Log.Info("保存缺省部门...");
                defaultDepartmentConfig.Value = DefaultDepartment.DepartmentID == 0? "" : DefaultDepartment.DepartmentID.ToString();
                _sysConfigRepository.Update(defaultDepartmentConfig);
                
                Log.Info("保存缺省角色...");
                defaultRoleConfig.Value = DefaultDeviceRole.DeviceRoleID == 0 ? "" : DefaultDeviceRole.DeviceRoleID.ToString();
                _sysConfigRepository.Update(defaultRoleConfig);

                Log.Info("保存同步时间...");
                string configValues = "";

                var enabledItems = DataSyncJobTimeConfigViewModels.FindAll(x => x.IsSelected);
                if (enabledItems.Any())
                {
                    configValues = string.Join(";", enabledItems.Select(x =>
                    {
                        var mydt = DateTime.Parse(x.Value);
                        return mydt.ToString("HH:mm:ss");
                    }));
                }

                dataSyncConfig.Value = configValues;
                _sysConfigRepository.Update(dataSyncConfig);

                Messenger.Default.Send(new NotificationMessage("保存成功!"), Tokens.DataSyncPage_ShowNotification);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }
    }
}
