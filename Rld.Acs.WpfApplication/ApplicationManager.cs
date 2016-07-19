using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication
{
    public class ApplicationManager
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ApplicationManager _instance = null;

        private IDepartmentRepository _departmentRepository = NinjectBinder.GetRepository<IDepartmentRepository>();
        private IDeviceRoleRepository _deviceRoleRepository = NinjectBinder.GetRepository<IDeviceRoleRepository>();
        private IDeviceControllerRepository _deviceControllerRepository = NinjectBinder.GetRepository<IDeviceControllerRepository>();

        public List<Department> AuthorizationDepartments { get; set; }
        public List<DeviceController> AuthorizationDevices { get; set; }
        public List<DeviceRole> AuthorizationDeviceRoles { get; set; }

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

        private void InitResource()
        {
            AuthorizationDepartments = _departmentRepository.Query(new Hashtable()).ToList();
            AuthorizationDevices = _deviceControllerRepository.Query(new Hashtable { { "Status", 1 } }).ToList();
            AuthorizationDeviceRoles = _deviceRoleRepository.Query(new Hashtable { { "Status", 1 } }).ToList();

            var topDepartment = new Department() { DepartmentID = -1, Name = "总经办" };
            AuthorizationDepartments.Insert(0, topDepartment);
            AuthorizationDepartments.FindAll(d => d.Parent == null && d.DepartmentID != -1).ForEach(d => d.Parent = topDepartment);
        }
    }
}
