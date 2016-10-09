using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.DeviceSystem.Message;
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
        private IDeviceRoleRepository _deviceRole = RepositoryManager.GetRepository<IDeviceRoleRepository>();
        private IDeviceControllerRepository _deviceRepo = RepositoryManager.GetRepository<IDeviceControllerRepository>();

        public void SyncUser(SyncOption option, User user, DeviceController device)
        {
            var deviceInfo = _deviceRepo.GetByKey(device.DeviceID);

            var deviceCode = deviceInfo.Code.ToInt32();
            if (WebSocketClientManager.GetInstance().GetClientById(deviceCode) == null)
                throw new DeviceNotConnectedException();

            var userInfo = _userRepo.GetByKey(user.UserID);

            switch (option)
            {
                case SyncOption.Create:
                    AddUser(userInfo, deviceInfo);
                    break;
                case SyncOption.Delete:
                    DeleteUser(userInfo, deviceInfo);
                    break;
                case SyncOption.Update:
                case SyncOption.Unknown:
                    UpdateUser(userInfo, deviceInfo);
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
            deviceUser.DepartmentId = user.DepartmentID;
            deviceUser.Comment = user.Remark;
            // user role
            deviceUser.Role = (Rld.DeviceSystem.Contract.Model.UserRole)userDevicePermission.PermissionAction.GetHashCode();
            deviceUser.AccessTimeZoneId = userDevicePermission.AllowedAccessTimeZoneID;

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

            Log.DebugFormat("Request: {0}", rawRequest);
            var rawResponse = operation.Execute(rawRequest);
            Log.DebugFormat("Response: {0}", rawResponse);
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
            deviceUser.DepartmentId = user.DepartmentID;
            deviceUser.Comment = user.Remark;
            // user role
            deviceUser.Role = (Rld.DeviceSystem.Contract.Model.UserRole)userDevicePermission.PermissionAction.GetHashCode();
            deviceUser.AccessTimeZoneId = userDevicePermission.AllowedAccessTimeZoneID;

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

            Log.DebugFormat("Request: {0}", rawRequest);
            var rawResponse = operation.Execute(rawRequest);
            Log.DebugFormat("Response: {0}", rawResponse);
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

            Log.DebugFormat("Request: {0}", rawRequest);
            var rawResponse = operation.Execute(rawRequest);
            Log.DebugFormat("Response: {0}", rawResponse);
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
    }
}