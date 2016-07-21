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
using Rld.Acs.WpfApplication.Models;
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
        public string LocalCachePath { get; private set; }
        public string LocalImageCachePath { get; private set; }

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
            InitEnvironment();

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

        private void InitEnvironment()
        {
            Log.Info("Init local cache...");
            LocalCachePath = string.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppConfiguration.LocalCachePath);
            if (Directory.Exists(LocalCachePath) == false)
            {
                Directory.CreateDirectory(LocalCachePath);
            }

            LocalImageCachePath = string.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppConfiguration.LocalCachePath + @"\images");
            if (Directory.Exists(LocalImageCachePath) == false)
            {
                Directory.CreateDirectory(LocalImageCachePath);
            }


        }
    }
}
