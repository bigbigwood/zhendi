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
        private IUserAuthenticationRepository _userAuthenticationRepo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
        private IDeviceRoleRepository _deviceRole = RepositoryManager.GetRepository<IDeviceRoleRepository>();
        private IUserEventRepository _userEventRepo = RepositoryManager.GetRepository<IUserEventRepository>();
        private IDeviceControllerRepository _deviceRepo = RepositoryManager.GetRepository<IDeviceControllerRepository>();


        public void SyncUser(User user, DeviceController device)
        {
            var deviceID = device.DeviceID;
            var deviceInfo = _deviceRepo.GetByKey(deviceID);
            var deviceCode = deviceInfo.Code.ToInt32();
            if (WebSocketClientManager.GetInstance().GetClientById(deviceCode) == null)
                throw new DeviceNotConnectedException();

            if (user.UserID != 0)
                user = _userRepo.GetByKey(user.UserID);

            Log.Info("Invoke WebSocketOperation...");
            var deviceUserId = user.UserCode.ToInt32();
            var operation = new WebSocketOperation(deviceCode);
            var getUserInfoRequest = new GetUserInfoRequest() { Token = operation.Token, UserId = deviceUserId };
            string rawRequest = DataContractSerializationHelper.Serialize(getUserInfoRequest);

            Log.DebugFormat("Request: {0}", rawRequest);
            var rawResponse = operation.Execute(rawRequest);
            Log.DebugFormat("Response: {0}", rawResponse);
            if (string.IsNullOrWhiteSpace(rawResponse))
            {
                throw new Exception(string.Format("Getting user id:[{0}], device user id:[{1}] from device id:[{2}]. Response is empty, maybe the device is not register to device system.",
                    user.UserID, deviceUserId, deviceID));
            }

            var response = DataContractSerializationHelper.Deserialize<GetUserInfoResponse>(rawResponse);
            Log.InfoFormat("Getting user id:[{0}], device user id:[{1}] from device id:[{2}], result:[{3}]", user.UserID, deviceUserId, deviceID, response.ResultType);

            if (response.ResultType != ResultType.OK)
            {
                throw new Exception(string.Format("Getting user id:[{0}], device user id:[{1}] from device id:[{2}]", user.UserID, deviceUserId, deviceID));
            }

            bool deviceUserExists = IsDeviceUserExist(response.UserInfo);
            bool systemUserExists = IsSystemUserExist(user);

            if (deviceUserExists && systemUserExists)
            {
                var userInfo = _userRepo.GetByKey(user.UserID);
                UpdateUser(userInfo, response.UserInfo, deviceInfo);
            }
            else if (!deviceUserExists && systemUserExists)
            {
                var userInfo = _userRepo.GetByKey(user.UserID);
                DeleteUser(userInfo, deviceInfo);
            }
            else if (deviceUserExists && !systemUserExists)
            {
                AddUser(user, response.UserInfo, deviceInfo);
            }
        }

        private bool IsDeviceUserExist(UserInfo deviceUserInfo)
        {
            return deviceUserInfo != null && deviceUserInfo.UserId != 0;
        }

        private bool IsSystemUserExist(User userInfo)
        {
            return userInfo.UserID != 0;
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

        public void DeleteUser(User user, DeviceController device)
        {
            if (user == null || device == null) return;
            if (user.UserAuthentications == null || user.UserAuthentications.Count == 0) return;
            if (user.GetUserAccessableDeviceIds().Contains(device.DeviceID) == false) return;

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
                EventData = "Delete user authentication by sync system user operation",
            });
        }

        public void AddUser(User systemUserInfo, UserInfo deviceUserInfo, DeviceController device)
        {
            var deviceID = device.DeviceID;
            var deviceUserId = systemUserInfo.UserCode.ToInt32();

            var deviceUser = deviceUserInfo;
            systemUserInfo.Photo = "avator.jpg";
            systemUserInfo.Type = UserType.Employee;
            systemUserInfo.UserCode = systemUserInfo.UserCode;
            systemUserInfo.Name = deviceUser.UserName;
            systemUserInfo.Gender = GenderType.Male;
            systemUserInfo.Phone = "";
            systemUserInfo.Status = GeneralStatus.Enabled;
            systemUserInfo.StartDate = DateTime.Now;
            systemUserInfo.EndDate = null;
            systemUserInfo.Remark = deviceUser.Comment;

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


            foreach (var service in deviceUser.CredentialServices)
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


            if (deviceUser.DepartmentId.HasValue && deviceUser.DepartmentId != 0)
            {
                systemUserInfo.DepartmentID = deviceUser.DepartmentId.Value;

                var departmentRepo = RepositoryManager.GetRepository<IDepartmentRepository>();
                var departmentInfo = departmentRepo.GetByKey(systemUserInfo.DepartmentID);

                Log.Info("Apply default device role from department.");
                systemUserInfo.UserDeviceRoles.Add(new UserDeviceRole() { UserDeviceRoleID = departmentInfo.DeviceRoleID });
            }

            if (deviceUser.AccessTimeZoneId.HasValue && deviceUser.Role.HasValue)
            {
                var deviceRoles = _deviceRole.Query(new Hashtable { { "Status", (int)GeneralStatus.Enabled } }).ToList();

                bool permissionExist = deviceRoles.FindAll(x => systemUserInfo.UserDeviceRoles.Select(e => e.DeviceRoleID).Contains(x.DeviceRoleID))
                    .SelectMany(x => x.DeviceRolePermissions)
                    .Any(x => x.DeviceID == deviceID && (int) x.PermissionAction == (int) deviceUser.Role);

                if (!permissionExist)
                {
                    var deviceRole = deviceRoles.FirstOrDefault(x =>
                    {
                        if (x.DeviceRolePermissions.Count() != 1) return false;

                        var permission = x.DeviceRolePermissions.FirstOrDefault();
                        return (permission.DeviceID == deviceID &&
                                (int)permission.PermissionAction == (int)deviceUser.Role);
                    });

                    if (deviceRole != null)
                        systemUserInfo.UserDeviceRoles.Add(new UserDeviceRole() { UserDeviceRoleID = deviceRole.DeviceRoleID });
                }
            }

            var userAuthenticationRepo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
            var userPropertyRepo = RepositoryManager.GetRepository<IUserPropertyRepository>();
            var userRepo = RepositoryManager.GetRepository<IUserRepository>();
            var userDeviceRoleRepo = RepositoryManager.GetRepository<IUserDeviceRoleRepository>();
            var userEventRepo = RepositoryManager.GetRepository<IUserEventRepository>();

            Log.Info("Sync user to database...");
            userPropertyRepo.Insert(systemUserInfo.UserPropertyInfo);
            userRepo.Insert(systemUserInfo);

            EncodePassword(systemUserInfo);
            systemUserInfo.UserAuthentications.ForEach(a => a.UserID = systemUserInfo.UserID);
            systemUserInfo.UserAuthentications.ForEach(a => userAuthenticationRepo.Insert(a));

            systemUserInfo.UserDeviceRoles.ForEach(a => a.UserID = systemUserInfo.UserID);
            systemUserInfo.UserDeviceRoles.ForEach(a => userDeviceRoleRepo.Insert(a));

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
            if (systemUserInfo.UserAuthentications == null || systemUserInfo.UserAuthentications.Count == 0) return;
            if (systemUserInfo.GetUserAccessableDeviceIds().Contains(device.DeviceID) == false) return;

            Log.Info("Getting user authentication infos...");
            var deviceID = device.DeviceID;
            var userAuthenticationsOfDevice = systemUserInfo.UserAuthentications.Where(a => a.DeviceID == deviceID);
            var authenticationsOfDevice = userAuthenticationsOfDevice as IList<UserAuthentication> ?? userAuthenticationsOfDevice.ToList();
            var deviceUserId = systemUserInfo.GetDeviceUserId(device);

            var deviceUser = deviceUserInfo;
            systemUserInfo.Name = deviceUser.UserName;
            //user.Status = deviceUser.UserStatus == true ? GeneralStatus.Enabled : GeneralStatus.Disabled;
            //user.DepartmentID = deviceUser.DepartmentId ?? 0;
            systemUserInfo.Remark = deviceUser.Comment;

            var userAuthenticationFromDevice = new List<UserAuthentication>();
            foreach (var service in deviceUser.CredentialServices)
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
    }
}