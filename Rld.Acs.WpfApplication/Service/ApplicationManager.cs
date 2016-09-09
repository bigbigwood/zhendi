﻿using System;
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
        private ITimeZoneRepository _timeZoneRepository = NinjectBinder.GetRepository<ITimeZoneRepository>();
        private ISysRoleRepository _sysRoleRepo = NinjectBinder.GetRepository<ISysRoleRepository>();

        public List<Department> AuthorizationDepartments { get; set; }
        public List<DeviceController> AuthorizationDevices { get; set; }
        public List<DeviceRole> AuthorizationDeviceRoles { get; set; }
        public List<Model.TimeZone> AuthorizationTimezones { get; set; }
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
            AuthorizationDepartments = _departmentRepository.Query(new Hashtable { { "Status", (int)GeneralStatus.Enabled } }).ToList();
            AuthorizationDevices = _deviceControllerRepository.Query(new Hashtable { { "Status", (int)GeneralStatus.Enabled } }).ToList();
            AuthorizationDeviceRoles = _deviceRoleRepository.Query(new Hashtable { { "Status", (int)GeneralStatus.Enabled } }).ToList();
            AuthorizationTimezones = _timeZoneRepository.Query(new Hashtable { { "Status", (int)GeneralStatus.Enabled } }).ToList();


            var topDepartment = new Department() { DepartmentID = -1, Name = "总经办" };
            AuthorizationDepartments.Insert(0, topDepartment);
            AuthorizationDepartments.FindAll(d => d.Parent == null && d.DepartmentID != -1).ForEach(d => d.Parent = topDepartment);
        }
    }
}
