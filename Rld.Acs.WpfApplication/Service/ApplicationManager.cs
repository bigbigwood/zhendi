using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility.Extension;
using Rld.Acs.WpfApplication.Models;
using Rld.Acs.WpfApplication.Repository;
using Rld.Acs.WpfApplication.Service.Authorization;
using Rld.Acs.WpfApplication.ViewModel;

namespace Rld.Acs.WpfApplication
{
    public class ApplicationManager
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ApplicationManager _instance = null;

        private IDepartmentRepository _departmentRepository = NinjectBinder.GetRepository<IDepartmentRepository>();
        private IDeviceRoleRepository _deviceRoleRepository = NinjectBinder.GetRepository<IDeviceRoleRepository>();
        private IDeviceControllerRepository _deviceControllerRepository = NinjectBinder.GetRepository<IDeviceControllerRepository>();
        private ISysRoleRepository _sysRoleRepo = NinjectBinder.GetRepository<ISysRoleRepository>();

        public List<Department> AuthorizationDepartments
        {
            get
            {
                var allDept = _departmentRepository.Query(new Hashtable()).FindAll(x => x.Status == GeneralStatus.Enabled);
                var topDepartment = new Department() { DepartmentID = -1, Name = "公司" };
                allDept.Insert(0, topDepartment);
                allDept.FindAll(d => d.Parent == null && d.DepartmentID != -1).ForEach(d => d.Parent = topDepartment);
                return allDept;
            }
        }
        public List<DeviceController> AuthorizationDevices
        {
            get { return _deviceControllerRepository.Query(new Hashtable()).FindAll(x => x.Status == GeneralStatus.Enabled); }
        }
        public List<DeviceRole> AuthorizationDeviceRoles
        {
            get { return _deviceRoleRepository.Query(new Hashtable()).FindAll(x => x.Status == GeneralStatus.Enabled); }
        }
        public SysOperator CurrentOperatorInfo { get; set; }
        public List<SysRolePermission> AuthorizationPermissions { get; set; }

        public static ApplicationManager GetInstance()
        {
            return _instance;
        }

        public static void Initialize()
        {
            Log.Info("Initializing ApplicationManager...");
            _instance = new ApplicationManager();

            Log.Info("Initializing ApplicationManager Finish...");
        }

        private ApplicationManager()
        {
            InitResource();
        }

        public void UpdateCurrentOperatorAndPermission(SysOperator currentOperator)
        {
            CurrentOperatorInfo = currentOperator;

            var roles = new List<SysRole>();
            currentOperator.SysOperatorRoles.ForEach(x => roles.Add(_sysRoleRepo.GetByKey(x.RoleID)));
            AuthorizationPermissions = roles.SelectMany(x => x.SysRolePermissions).ToList();

            var accessControlList = new List<string>();
            accessControlList.AddRange(AuthorizationPermissions.FindAll(x => x.ModuleInfo != null).Select(x => x.ModuleInfo.ModuleCode));
            accessControlList.AddRange(AuthorizationPermissions.FindAll(x => x.ElementInfo != null).Select(x => x.ElementInfo.ElementCode));
            AuthProvider.Initialize<DefaultAuthProvider>(accessControlList.ToArray());
        }

        private void InitResource()
        {
            _departmentRepository.Query(new Hashtable()).FindAll(x => x.Status == GeneralStatus.Enabled);
            _deviceControllerRepository.Query(new Hashtable()).FindAll(x => x.Status == GeneralStatus.Enabled);
            _deviceRoleRepository.Query(new Hashtable()).FindAll(x => x.Status == GeneralStatus.Enabled);
        }
    }
}
