using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.Model;
using Rld.Acs.Model.Extension;
using Rld.Acs.Repository;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.Unility;
using Rld.Acs.Unility.Encryption;
using Rld.Acs.Unility.Extension;
using Rld.Acs.Unility.Serialization;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.UserCredential;

namespace Rld.Acs.DeviceSystem.Service
{
    public class SystemUserOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserRepository _userRepo = RepositoryManager.GetRepository<IUserRepository>();
        private IUserDeviceRoleRepository _userDeviceRoleRepo = RepositoryManager.GetRepository<IUserDeviceRoleRepository>();
        private IUserAuthenticationRepository _userAuthenticationRepo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
        private IUserEventRepository _userEventRepo = RepositoryManager.GetRepository<IUserEventRepository>();
        private IDeviceRoleRepository _deviceRole = RepositoryManager.GetRepository<IDeviceRoleRepository>();
        private ISysConfigRepository _sysConfigRepo = RepositoryManager.GetRepository<ISysConfigRepository>();
        private ITimeZoneRepository _timeZoneRepo = RepositoryManager.GetRepository<ITimeZoneRepository>();
        private IEnumerable<DeviceController> deviceCache;
        private Dictionary<Int32, UserInfo> userDeviceInfoDict;

        public SystemUserOp(IEnumerable<DeviceController> deviceCache, Dictionary<Int32, UserInfo> userDeviceInfoDict)
        {
            this.deviceCache = deviceCache;
            this.userDeviceInfoDict = userDeviceInfoDict;
        }

        public void SyncUser(User systemUser, List<DeviceController> targetDevices)
        {
            foreach (var device in targetDevices)
            {
                var deviceUser = new UserInfo();
                userDeviceInfoDict.TryGetValue(device.DeviceID, out deviceUser);

                bool deviceUserExists = IsDeviceUserExist(deviceUser);
                bool systemUserExists = IsSystemUserExist(systemUser);

                if (!deviceUserExists && systemUserExists)
                {
                    DeleteUserAuthentications(systemUser, device);
                }
                else if (deviceUserExists && systemUserExists)
                {
                    UpdateUser(systemUser, deviceUser, device);
                }
                else if (deviceUserExists && !systemUserExists)
                {
                    AddUser(systemUser, deviceUser, device);
                }
            }

            Log.Info("Try to update user device role...");
            UpdateUserDeviceRole(systemUser);
        }

        private bool IsDeviceUserExist(UserInfo deviceUserInfo)
        {
            return deviceUserInfo != null && deviceUserInfo.UserId != 0;
        }

        private bool IsSystemUserExist(User userInfo)
        {
            return userInfo.UserID != 0;
        }

        private String GetDeviceUserName(UserInfo deviceUserInfo)
        {
            if (string.IsNullOrWhiteSpace(deviceUserInfo.UserName))
                return deviceUserInfo.UserId.ToString();

            return deviceUserInfo.UserName;
        }

        public static void EncodePassword(User user)
        {
            if (user != null && user.UserAuthentications != null)
            {
                var passwords = user.UserAuthentications.FindAll(x => x.AuthenticationType == AuthenticationType.Password);
                if (passwords.Any())
                {
                    passwords.ForEach(x => x.AuthenticationData = SimpleEncryption.Encode(x.AuthenticationData));
                }
            }
        }

        public void DeleteUserAuthentications(User user, DeviceController device)
        {
            if (user == null || device == null) return;
            if (user.UserAuthentications == null || user.UserAuthentications.Count == 0) return;

            Log.Info("Getting user authentication infos...");
            var userAuthenticationsOfDevice = user.UserAuthentications.Where(a => a.DeviceID == device.DeviceID);
            var authenticationsOfDevice = userAuthenticationsOfDevice as IList<UserAuthentication> ?? userAuthenticationsOfDevice.ToList();

            Log.InfoFormat("Deleting user authentication, UserId={0}, DeviceId={1}.", user.UserID, device.DeviceID);
            authenticationsOfDevice.ForEach(x =>
            {
                _userAuthenticationRepo.Delete(x.UserAuthenticationID);
                Log.InfoFormat("User authentication id={0} deleted", x.UserAuthenticationID);
            });

            _userEventRepo.Insert(new UserEvent()
            {
                EventType = UserEventType.Modify,
                UserID = user.UserID,
                CreateDate = DateTime.Now,
                CreateUserID = GlobalSetting.DeviceSystemId,
                IsFinished = true,
                EventData = "Delete user authentications by sync system user operation",
            });
        }

        public void AddUser(User systemUserInfo, UserInfo deviceUserInfo, DeviceController device)
        {
            var deviceID = device.DeviceID;
            var deviceUserId = systemUserInfo.UserCode.ToInt32();

            systemUserInfo.Photo = "avator.jpg";
            systemUserInfo.Type = UserType.Employee;
            systemUserInfo.UserCode = systemUserInfo.UserCode;
            systemUserInfo.Gender = GenderType.Male;
            systemUserInfo.Phone = "";
            systemUserInfo.Status = GeneralStatus.Enabled;
            systemUserInfo.StartDate = DateTime.Now;
            systemUserInfo.EndDate = null;

            systemUserInfo.UserPropertyInfo.LastName = null;
            systemUserInfo.UserPropertyInfo.FirstName = null;
            systemUserInfo.UserPropertyInfo.Nationality = 0;
            systemUserInfo.UserPropertyInfo.NativePlace = null;
            systemUserInfo.UserPropertyInfo.Birthday = new DateTime(2000, 1, 1);
            systemUserInfo.UserPropertyInfo.Marriage = Marriage.Single;
            systemUserInfo.UserPropertyInfo.PoliticalStatus = null;
            systemUserInfo.UserPropertyInfo.Degree = null;
            systemUserInfo.UserPropertyInfo.HomeNumber = "";
            systemUserInfo.UserPropertyInfo.EnglishName = "";
            systemUserInfo.UserPropertyInfo.Company = "";
            systemUserInfo.UserPropertyInfo.TechnicalTitle = "";
            systemUserInfo.UserPropertyInfo.TechnicalLevel = "";
            systemUserInfo.UserPropertyInfo.IDType = (int)IDType.ID;
            systemUserInfo.UserPropertyInfo.IDNumber = "";
            systemUserInfo.UserPropertyInfo.SocialNumber = "";
            systemUserInfo.UserPropertyInfo.Email = "";
            systemUserInfo.UserPropertyInfo.Address = "";
            systemUserInfo.UserPropertyInfo.Postcode = "";
            systemUserInfo.UserPropertyInfo.Remark = "";

            systemUserInfo.Name = GetDeviceUserName(deviceUserInfo);
            systemUserInfo.Remark = deviceUserInfo.Comment;
            UpdateUserDepartment(systemUserInfo, deviceUserInfo.DepartmentId);

            foreach (var service in deviceUserInfo.CredentialServices)
            {
                var userAuthentication = new UserAuthentication()
                {
                    UserID = systemUserInfo.UserID,
                    DeviceID = deviceID,
                    DeviceUserID = deviceUserId,
                    Status = GeneralStatus.Enabled,
                    CreateDate = DateTime.Now,
                    CreateUserID = GlobalSetting.DeviceSystemId,
                };

                if (service is PasswordService)
                {
                    var passwordService = service as PasswordService;
                    userAuthentication.AuthenticationType = AuthenticationType.Password;
                    userAuthentication.AuthenticationData = SimpleEncryption.Encode(passwordService.Password);
                    userAuthentication.IsDuress = passwordService.UseForDuress;
                }
                else if (service is CredentialCardService)
                {
                    var credentialCardService = service as CredentialCardService;
                    userAuthentication.AuthenticationType = AuthenticationType.IcCard;
                    userAuthentication.AuthenticationData = credentialCardService.CardNumber;
                    userAuthentication.IsDuress = credentialCardService.UseForDuress;
                }
                else if (service is FingerPrintService)
                {
                    var fpService = service as FingerPrintService;
                    userAuthentication.AuthenticationType = (AuthenticationType)fpService.Index;
                    userAuthentication.AuthenticationData = fpService.FingerPrintData;
                    userAuthentication.IsDuress = fpService.UseForDuress;
                }

                systemUserInfo.UserAuthentications.Add(userAuthentication);
            }

            var userAuthenticationRepo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
            var userPropertyRepo = RepositoryManager.GetRepository<IUserPropertyRepository>();
            var userRepo = RepositoryManager.GetRepository<IUserRepository>();
            var userEventRepo = RepositoryManager.GetRepository<IUserEventRepository>();

            Log.Info("Sync user to database...");
            userPropertyRepo.Insert(systemUserInfo.UserPropertyInfo);
            userRepo.Insert(systemUserInfo);

            EncodePassword(systemUserInfo);
            systemUserInfo.UserAuthentications.ForEach(a => a.UserID = systemUserInfo.UserID);
            systemUserInfo.UserAuthentications.ForEach(a => userAuthenticationRepo.Insert(a));

            userEventRepo.Insert(new UserEvent()
            {
                EventType = UserEventType.Add,
                UserID = systemUserInfo.UserID,
                CreateDate = DateTime.Now,
                CreateUserID = GlobalSetting.DeviceSystemId,
                IsFinished = true,
                EventData = "Add user by sync system user operation",
            });
        }

        public void UpdateUser(User systemUserInfo, UserInfo deviceUserInfo, DeviceController device)
        {
            if (systemUserInfo == null || device == null) return;

            systemUserInfo.Name = GetDeviceUserName(deviceUserInfo);
            systemUserInfo.Remark = deviceUserInfo.Comment;
            UpdateUserDepartment(systemUserInfo, deviceUserInfo.DepartmentId);

            Log.Info("Getting user authentication infos...");
            var deviceID = device.DeviceID;
            var userAuthenticationsOfDevice = systemUserInfo.UserAuthentications.Where(a => a.DeviceID == deviceID);
            var authenticationsOfDevice = userAuthenticationsOfDevice as IList<UserAuthentication> ?? userAuthenticationsOfDevice.ToList();
            var deviceUserId = systemUserInfo.UserCode.ToInt32();

            var userAuthenticationFromDevice = new List<UserAuthentication>();
            foreach (var service in deviceUserInfo.CredentialServices)
            {
                var userAuthentication = new UserAuthentication()
                {
                    UserID = systemUserInfo.UserID,
                    DeviceID = deviceID,
                    DeviceUserID = deviceUserId,
                    Status = GeneralStatus.Enabled,
                    CreateDate = DateTime.Now,
                    CreateUserID = GlobalSetting.DeviceSystemId,
                };

                if (service is PasswordService)
                {
                    var passwordService = service as PasswordService;
                    var originalUserAuthentication = authenticationsOfDevice.FirstOrDefault(a => a.AuthenticationType == AuthenticationType.Password);
                    userAuthentication.UserAuthenticationID = originalUserAuthentication != null ? originalUserAuthentication.UserAuthenticationID : 0;
                    userAuthentication.AuthenticationType = AuthenticationType.Password;
                    userAuthentication.AuthenticationData = SimpleEncryption.Encode(passwordService.Password);
                    userAuthentication.IsDuress = passwordService.UseForDuress;
                }
                else if (service is CredentialCardService)
                {
                    var credentialCardService = service as CredentialCardService;
                    var originalUserAuthentication = authenticationsOfDevice.FirstOrDefault(a => a.AuthenticationType == AuthenticationType.IcCard);
                    userAuthentication.UserAuthenticationID = originalUserAuthentication != null ? originalUserAuthentication.UserAuthenticationID : 0;
                    userAuthentication.AuthenticationType = AuthenticationType.IcCard;
                    userAuthentication.AuthenticationData = credentialCardService.CardNumber;
                    userAuthentication.IsDuress = credentialCardService.UseForDuress;
                }
                else if (service is FingerPrintService)
                {
                    var fpService = service as FingerPrintService;
                    var originalUserAuthentication = authenticationsOfDevice.FirstOrDefault(a => a.AuthenticationType.GetHashCode() == fpService.Index);
                    userAuthentication.UserAuthenticationID = originalUserAuthentication != null ? originalUserAuthentication.UserAuthenticationID : 0;
                    userAuthentication.AuthenticationType = (AuthenticationType) fpService.Index;
                    userAuthentication.AuthenticationData = fpService.FingerPrintData;
                    userAuthentication.IsDuress = fpService.UseForDuress;
                }

                userAuthenticationFromDevice.Add(userAuthentication);
            }

            Log.Info("Sync user to database...");
            var addedAuthentications = new List<UserAuthentication>();
            var deletedAuthenticationIds = new List<int>();
            if (userAuthenticationFromDevice.Any())
            {
                var originalUserAuthenticationIDs = systemUserInfo.UserAuthentications.Select(d => d.UserAuthenticationID);
                var UserAuthenticationIDs = userAuthenticationFromDevice.Select(d => d.UserAuthenticationID);
                deletedAuthenticationIds = originalUserAuthenticationIDs.Except(UserAuthenticationIDs).ToList();

                addedAuthentications = userAuthenticationFromDevice.FindAll(d => d.UserAuthenticationID == 0);
            }
            else
            {
                deletedAuthenticationIds = systemUserInfo.UserAuthentications.Select(d => d.UserAuthenticationID).ToList();
            }

            userAuthenticationFromDevice.FindAll(d => d.UserAuthenticationID != 0).ForEach(d =>
            {
                var auth = systemUserInfo.UserAuthentications.First(x => x.UserAuthenticationID == d.UserAuthenticationID);
                auth.AuthenticationData = d.AuthenticationData;

                _userAuthenticationRepo.Update(auth);
                Log.InfoFormat("User authentication id={0} updated", d.UserAuthenticationID);
            });
            deletedAuthenticationIds.ForEach(d =>
            {
                _userAuthenticationRepo.Delete(d);
                Log.InfoFormat("User authentication id={0} deleted", d);
            });
            addedAuthentications.ForEach(d =>
            {
                var auth = _userAuthenticationRepo.Insert(d);
                Log.InfoFormat("User authentication id={0} inserted", auth.UserAuthenticationID);
            });

            _userRepo.Update(systemUserInfo);

            _userEventRepo.Insert(new UserEvent()
            {
                EventType = UserEventType.Modify,
                UserID = systemUserInfo.UserID,
                CreateDate = DateTime.Now,
                CreateUserID = GlobalSetting.DeviceSystemId,
                IsFinished = true,
                EventData = "Sync system user operation",
            });
        }

        public void UpdateUserDepartment(User systemUserInfo, int? departmentCode)
        {
            Department departmentInfo = null;
            if (departmentCode.HasValue)
            {
                var departmentRepo = RepositoryManager.GetRepository<IDepartmentRepository>();
                departmentInfo = departmentRepo.Query(new Hashtable() { { "DepartmentCode", departmentCode } }).FirstOrDefault();
            }

            if (departmentInfo != null)
            {
                systemUserInfo.DepartmentID = departmentInfo.DepartmentCode.ToInt32();
            }
            else
            {
                var defaultDepartment = GetDefaultDepartment();
                if (defaultDepartment != 0)
                    systemUserInfo.DepartmentID = defaultDepartment;
            }
        }

        public void UpdateUserDeviceRole(User systemUserInfo)
        {
            var userDeviceRoles = new List<UserDeviceRole>();

            if (systemUserInfo.DepartmentID != 0)
            {
                var departmentRepo = RepositoryManager.GetRepository<IDepartmentRepository>();
                var departmentInfo = departmentRepo.GetByKey(systemUserInfo.DepartmentID);
                if (departmentInfo != null)
                {
                    userDeviceRoles.Add(new UserDeviceRole() { UserID = systemUserInfo.UserID, DeviceRoleID = departmentInfo.DeviceRoleID });
                }
            }

            var deviceRoles = _deviceRole.Query(new Hashtable { { "Status", (int)GeneralStatus.Enabled } }).ToList();
            var timezoneInfos = _timeZoneRepo.Query(new Hashtable { { "Status", (int)GeneralStatus.Enabled } }).ToList();
            var role = deviceRoles.FirstOrDefault(x => CheckRoleForUser(x, userDeviceInfoDict, timezoneInfos));
            if (role != null)
            {
                if (userDeviceRoles.All(x => x.DeviceRoleID != role.DeviceRoleID))
                    userDeviceRoles.Add(new UserDeviceRole() { UserID = systemUserInfo.UserID, DeviceRoleID = role.DeviceRoleID });
            }
            else
            {
                Log.Info("Cannot find existing device roles matches user, trying to apply default role for user.");
                var defaultRole = GetDefaultRole(deviceRoles);
                if (defaultRole != null && userDeviceRoles.All(x => x.DeviceRoleID != defaultRole.DeviceRoleID))
                {
                    Log.InfoFormat("Apply default role id={0} for user", defaultRole.DeviceRoleID);
                    userDeviceRoles.Add(new UserDeviceRole() { UserID = systemUserInfo.UserID, DeviceRoleID = defaultRole.DeviceRoleID });
                }
            }

            var addedRoleIds = new List<int>();
            var deletedRoleIds = new List<int>();
            if (userDeviceRoles.Any())
            {
                var originalRoleIds = systemUserInfo.UserDeviceRoles.Select(d => d.DeviceRoleID);
                var currentRoleIds = userDeviceRoles.Select(d => d.DeviceRoleID);
                deletedRoleIds = originalRoleIds.Except(currentRoleIds).ToList();

                addedRoleIds = currentRoleIds.Except(originalRoleIds).ToList();
            }
            else
            {
                deletedRoleIds = systemUserInfo.UserDeviceRoles.Select(d => d.UserDeviceRoleID).ToList();
            }

            deletedRoleIds.ForEach(d => _userDeviceRoleRepo.Delete(d));
            addedRoleIds.ForEach(d =>
            {
                var auth = userDeviceRoles.First(x => x.DeviceRoleID == d);
                _userDeviceRoleRepo.Insert(auth);
            });

            systemUserInfo.UserDeviceRoles = userDeviceRoles;
        }

        private bool CheckRoleForUser(DeviceRole role, Dictionary<Int32, UserInfo> userDeviceInfoDict, List<Rld.Acs.Model.TimeZone> timezoneInfos)
        {
            string userDevicePermissionString = string.Empty;
            string rolePermissionString = string.Empty;

            foreach (KeyValuePair<Int32, UserInfo> item in userDeviceInfoDict)
            {
                var roleid = item.Value.Role.HasValue ? (int) item.Value.Role.Value : 0;
                var accessTimeZoneId = 1;
                if (item.Value.AccessTimeZoneId.HasValue)
                {
                    accessTimeZoneId = timezoneInfos.First(x => x.TimeZoneCode == item.Value.AccessTimeZoneId.Value.ToString()).TimeZoneID;
                }
                userDevicePermissionString += string.Format("{0}_{1}_{2},", item.Key, roleid, accessTimeZoneId);
            }

            foreach (var permission in role.DeviceRolePermissions.OrderBy(x => x.DeviceID))
            {
                rolePermissionString += string.Format("{0}_{1}_{2},", permission.DeviceID, (int)permission.PermissionAction, permission.AllowedAccessTimeZoneID);
            }

            Log.InfoFormat("Compare permission string {0} == {1}", userDevicePermissionString, rolePermissionString);
            return userDevicePermissionString == rolePermissionString;
        }

        private int GetDefaultDepartment()
        {
            var dataSyncDefaultDepartmentConfig = _sysConfigRepo.Query(new Hashtable { { ConstStrings.Name, ConstStrings.DataSyncDefaultDepartment } }).FirstOrDefault();
            if (dataSyncDefaultDepartmentConfig != null && !string.IsNullOrWhiteSpace(dataSyncDefaultDepartmentConfig.Value))
            {
                return dataSyncDefaultDepartmentConfig.Value.ToInt32();
            }

            return 0;
        }

        private DeviceRole GetDefaultRole(List<DeviceRole> roles)
        {
            var dataSyncDefaultRoleConfig = _sysConfigRepo.Query(new Hashtable { { ConstStrings.Name, ConstStrings.DataSyncDefaultRole } }).FirstOrDefault();
            if (dataSyncDefaultRoleConfig != null && !string.IsNullOrWhiteSpace(dataSyncDefaultRoleConfig.Value))
            {
                var roleId = dataSyncDefaultRoleConfig.Value.ToInt32();
                return roles.FirstOrDefault(x => x.DeviceRoleID == roleId);
            }

            return null;
        }
    }
}