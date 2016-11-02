using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.DeviceSystem.Message;
using Rld.Acs.DeviceSystem.Model;
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
    public class DeviceUserOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserRepository _userRepo = RepositoryManager.GetRepository<IUserRepository>();
        private IDepartmentRepository _departmentRepo = RepositoryManager.GetRepository<IDepartmentRepository>();
        private IDeviceRoleRepository _deviceRole = RepositoryManager.GetRepository<IDeviceRoleRepository>();
        private ITimeZoneRepository _timeZoneRepo = RepositoryManager.GetRepository<ITimeZoneRepository>();

        public void SyncUser(SyncOption option, User user, DeviceController device)
        {
            switch (option)
            {
                case SyncOption.Create:
                    AddUser(user, device);
                    break;
                case SyncOption.Delete:
                    DeleteUser(user, device);
                    break;
                case SyncOption.Update:
                case SyncOption.Unknown:
                    UpdateUser(user, device);
                    break;
            }
        }

        public void AddUser(User user, DeviceController device)
        {
            if (user == null || device == null) return;
            if (user.UserAuthentications == null || user.UserAuthentications.Count == 0) return;
            if (user.GetUserAccessableDeviceIds().Contains(device.DeviceID) == false) return;

            var deviceID = device.DeviceID;
            var deviceCode = device.Code.ToInt32();

            Log.Info("Getting user authentication infos...");
            var userAuthenticationsOfDevice = user.UserAuthentications.Where(a => a.DeviceID == deviceID);
            var authenticationsOfDevice = userAuthenticationsOfDevice as IList<UserAuthentication> ?? userAuthenticationsOfDevice.ToList();

            Log.Info("Getting user permission infos...");
            var deviceRoles = _deviceRole.Query(new Hashtable { { "Status", (int)GeneralStatus.Enabled } }).ToList();
            var userDevicePermission = user.GetUserDeviceRoleAuthorizedPermissionByDeviceId(deviceID, deviceRoles);

            Log.Info("Building device user...");
            var deviceUser = new UserInfo();
            deviceUser.UserId = user.UserCode.ToInt32();
            deviceUser.ExternalUserCode = user.UserID.ToString();
            // user info
            deviceUser.UserName = user.Name;
            deviceUser.UserStatus = user.Status == GeneralStatus.Enabled;
            deviceUser.DepartmentId = GetDepartmentCode(user.DepartmentID);
            deviceUser.Comment = user.Remark;
            // user role
            deviceUser.Role = (Rld.DeviceSystem.Contract.Model.UserRole)userDevicePermission.PermissionAction.GetHashCode();
            deviceUser.AccessTimeZoneId = GetTimeZoneCode(userDevicePermission.AllowedAccessTimeZoneID);

            //user authentication
            foreach (var userAuthentication in authenticationsOfDevice)
            {
                switch (userAuthentication.AuthenticationType)
                {
                    case AuthenticationType.FingerPrint1:
                    case AuthenticationType.FingerPrint2:
                    case AuthenticationType.FingerPrint3:
                    case AuthenticationType.FingerPrint4:
                    case AuthenticationType.FingerPrint5:
                    case AuthenticationType.FingerPrint6:
                    case AuthenticationType.FingerPrint7:
                    case AuthenticationType.FingerPrint8:
                    case AuthenticationType.FingerPrint9:
                    case AuthenticationType.FingerPrint10:
                        {
                            var service = new FingerPrintService() { Index = (int)userAuthentication.AuthenticationType, Enabled = true };
                            service.FingerPrintData = userAuthentication.AuthenticationData;
                            service.UseForDuress = userAuthentication.IsDuress;
                            deviceUser.CredentialServices.Add(service);
                        }
                        break;
                    case AuthenticationType.Password:
                        {
                            var service = new PasswordService() { Enabled = true };
                            service.Password = SimpleEncryption.Decode(userAuthentication.AuthenticationData);
                            service.UseForDuress = userAuthentication.IsDuress;
                            deviceUser.CredentialServices.Add(service);
                        }
                        break;
                    case AuthenticationType.IcCard:
                        {
                            var service = new CredentialCardService() { Enabled = true };
                            service.CardNumber = userAuthentication.AuthenticationData;
                            service.UseForDuress = userAuthentication.IsDuress;
                            deviceUser.CredentialServices.Add(service);
                        }
                        break;
                    default:
                        break;
                }
            }

            Log.Info("Invoke WebSocketOperation...");
            var operation = new WebSocketOperation(deviceCode);
            var createUserInfoRequest = new CreateUserInfoRequest() { Token = operation.Token, UserInfo = deviceUser };
            string rawRequest = DataContractSerializationHelper.Serialize(createUserInfoRequest);
            var rawResponse = operation.Execute(rawRequest);
            if (string.IsNullOrWhiteSpace(rawResponse))
            {
                throw new Exception(string.Format("Create user id:[{0}], device user id:[{1}] to device id:[{2}] fails. Response is empty, maybe the device is not register to device system.",
                    user.UserID, deviceUser.UserId, deviceID));
            }

            var response = DataContractSerializationHelper.Deserialize<CreateUserInfoResponse>(rawResponse);
            Log.InfoFormat("Create user id:[{0}], device user id:[{1}] to device id:[{2}], result:[{3}]", user.UserID, deviceUser.UserId, deviceID, response.ResultType);

            if (response.ResultType != ResultType.OK)
            {
                throw new Exception(string.Format("Create user id:[{0}], device user id:[{1}] to device id:[{2}] fails.", user.UserID, deviceUser.UserId, deviceID));
            }
        }

        public void UpdateUser(User user, DeviceController device)
        {
            if (user == null || device == null) return;
            if (user.UserAuthentications == null || user.UserAuthentications.Count == 0) return;
            if (user.GetUserAccessableDeviceIds().Contains(device.DeviceID) == false) return;

            var deviceID = device.DeviceID;
            var deviceCode = device.Code.ToInt32();

            Log.Info("Getting user authentication infos...");
            var userAuthenticationsOfDevice = user.UserAuthentications.Where(a => a.DeviceID == deviceID);
            var authenticationsOfDevice = userAuthenticationsOfDevice as IList<UserAuthentication> ?? userAuthenticationsOfDevice.ToList();

            Log.Info("Getting user permission infos...");
            var deviceRoles = _deviceRole.Query(new Hashtable { { "Status", (int)GeneralStatus.Enabled } }).ToList();
            var userDevicePermission = user.GetUserDeviceRoleAuthorizedPermissionByDeviceId(deviceID, deviceRoles);

            Log.Info("Building device user...");
            var deviceUser = new UserInfo();
            deviceUser.UserId = user.UserCode.ToInt32();
            deviceUser.ExternalUserCode = user.UserID.ToString();
            // user info
            deviceUser.UserName = user.Name;
            deviceUser.UserStatus = user.Status == GeneralStatus.Enabled;
            deviceUser.DepartmentId = GetDepartmentCode(user.DepartmentID);
            deviceUser.Comment = user.Remark;
            // user role
            deviceUser.Role = (Rld.DeviceSystem.Contract.Model.UserRole)userDevicePermission.PermissionAction.GetHashCode();
            deviceUser.AccessTimeZoneId = GetTimeZoneCode(userDevicePermission.AllowedAccessTimeZoneID);

            //user authentication
            foreach (var userAuthentication in authenticationsOfDevice)
            {
                switch (userAuthentication.AuthenticationType)
                {
                    case AuthenticationType.FingerPrint1:
                    case AuthenticationType.FingerPrint2:
                    case AuthenticationType.FingerPrint3:
                    case AuthenticationType.FingerPrint4:
                    case AuthenticationType.FingerPrint5:
                    case AuthenticationType.FingerPrint6:
                    case AuthenticationType.FingerPrint7:
                    case AuthenticationType.FingerPrint8:
                    case AuthenticationType.FingerPrint9:
                    case AuthenticationType.FingerPrint10:
                        {
                            var service = new FingerPrintService() { Index = (int)userAuthentication.AuthenticationType, Enabled = true };
                            service.FingerPrintData = userAuthentication.AuthenticationData;
                            service.UseForDuress = userAuthentication.IsDuress;
                            deviceUser.CredentialServices.Add(service);
                        }
                        break;
                    case AuthenticationType.Password:
                        {
                            var service = new PasswordService() { Enabled = true };
                            service.Password = SimpleEncryption.Decode(userAuthentication.AuthenticationData);
                            service.UseForDuress = userAuthentication.IsDuress;
                            deviceUser.CredentialServices.Add(service);
                        }
                        break;
                    case AuthenticationType.IcCard:
                        {
                            var service = new CredentialCardService() { Enabled = true };
                            service.CardNumber = userAuthentication.AuthenticationData;
                            service.UseForDuress = userAuthentication.IsDuress;
                            deviceUser.CredentialServices.Add(service);
                        }
                        break;
                    default:
                        break;
                }
            }

            Log.Info("Invoke WebSocketOperation...");
            var operation = new WebSocketOperation(deviceCode);
            var updateUserInfoRequest = new UpdateUserInfoRequest() { Token = operation.Token, UserInfo = deviceUser };
            string rawRequest = DataContractSerializationHelper.Serialize(updateUserInfoRequest);
            var rawResponse = operation.Execute(rawRequest);
            if (string.IsNullOrWhiteSpace(rawResponse))
            {
                throw new Exception(string.Format("Update user id:[{0}], device user id:[{1}] to device id:[{2}] fails. Response is empty, maybe the device is not register to device system.",
                    user.UserID, deviceUser.UserId, deviceID));
            }

            var response = DataContractSerializationHelper.Deserialize<UpdateUserInfoResponse>(rawResponse);
            Log.InfoFormat("Update user id:[{0}], device user id:[{1}] to device id:[{2}], result:[{3}]", user.UserID, deviceUser.UserId, deviceID, response.ResultType);

            if (response.ResultType != ResultType.OK)
            {
                throw new Exception(string.Format("Update user id:[{0}], device user id:[{1}] to device id:[{2}] fails.", user.UserID, deviceUser.UserId, deviceID));
            }
        }

        public void DeleteUser(User user, DeviceController device)
        {
            if (user == null || device == null) return;

            var deviceID = device.DeviceID;
            var deviceCode = device.Code.ToInt32();

            var userCode = user.UserCode.ToInt32();

            Log.Info("Invoke WebSocketOperation...");
            var operation = new WebSocketOperation(deviceCode);
            var deleteUserInfoRequest = new DeleteUserInfoRequest() { Token = operation.Token, UserId = userCode};
            string rawRequest = DataContractSerializationHelper.Serialize(deleteUserInfoRequest);
            var rawResponse = operation.Execute(rawRequest);
            if (string.IsNullOrWhiteSpace(rawResponse))
            {
                throw new Exception(string.Format("Delete user id:[{0}], device user id:[{1}] to device id:[{2}] fails. Response is empty, maybe the device is not register to device system.",
                    user.UserID, userCode, deviceID));
            }

            var response = DataContractSerializationHelper.Deserialize<DeleteUserInfoResponse>(rawResponse);
            Log.InfoFormat("Delete user id:[{0}], device user id:[{1}] to device id:[{2}], result:[{3}]", user.UserID, userCode, deviceID, response.ResultType);

            if (response.ResultType != ResultType.OK)
            {
                throw new Exception(string.Format("Delete user id:[{0}], device user id:[{1}] to device id:[{2}] fails.", user.UserID, userCode, deviceID));
            }
        }

        public UserInfo TryGetUesrInfo(User user, DeviceController device)
        {
            if (user == null || device == null) return null;

            Log.Info("Trying to get user device info...");
            var deviceUserId = user.UserCode.ToInt32();
            var deviceID = device.Code.ToInt32();
            var deviceCode = device.Code.ToInt32();
            var operation = new WebSocketOperation(deviceCode);
            var getUserInfoRequest = new GetUserInfoRequest() { Token = operation.Token, UserId = deviceUserId };
            string rawRequest = DataContractSerializationHelper.Serialize(getUserInfoRequest);
            var rawResponse = operation.Execute(rawRequest);
            if (string.IsNullOrWhiteSpace(rawResponse))
            {
                throw new Exception(string.Format("Getting user id:[{0}], device user id:[{1}] from device id:[{2}]. Response is empty, maybe the device is not register to device system.",
                    user.UserID, deviceUserId, deviceID));
            }

            var response = DataContractSerializationHelper.Deserialize<GetUserInfoResponse>(rawResponse);
            Log.InfoFormat("Getting user id:[{0}], device user id:[{1}] from device id:[{2}], result:[{3}]", user.UserID, deviceUserId, deviceID, response.ResultType);

            return response.ResultType == ResultType.OK ?response.UserInfo : null;
        }

        public List<UserInfo> QueryUsersByDevice(DeviceController device)
        {
            if (device == null) return null;

            var deviceID = device.Code.ToInt32();
            var deviceCode = device.Code.ToInt32();
            var operation = new WebSocketOperation(deviceCode);
            var getAllUsersRequest = new GetAllUsersRequest() { Token = operation.Token };
            string rawRequest = DataContractSerializationHelper.Serialize(getAllUsersRequest);
            var rawResponse = operation.Execute(rawRequest);
            if (string.IsNullOrWhiteSpace(rawResponse))
            {
                throw new Exception(string.Format("Query users from device id:[{0}]. Response is empty, maybe the device is not register to device system.", deviceID));
            }

            var response = DataContractSerializationHelper.Deserialize<GetAllUsersResponse>(rawResponse);
            Log.InfoFormat("Query users from device id:[{0}], result:[{1}]", deviceID, response.ResultType);

            return response.ResultType == ResultType.OK ? response.Users.ToList() : null;
        }

        private int GetDepartmentCode(int departmentId)
        {
            var departmentInfo = _departmentRepo.GetByKey(departmentId);
            return departmentInfo.DepartmentCode.ToInt32();
        }

        private int GetTimeZoneCode(int timezoneId)
        {
            var timezoneInfo = _timeZoneRepo.GetByKey(timezoneId);
            return timezoneInfo.TimeZoneCode.ToInt32();
        }
    }
}