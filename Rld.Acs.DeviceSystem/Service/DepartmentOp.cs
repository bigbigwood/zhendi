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
using Rld.Acs.Unility.Extension;
using Rld.Acs.Unility.Serialization;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.UserCredential;

namespace Rld.Acs.DeviceSystem.Service
{
    /// <summary>
    /// 每个部门有一个设备权限角色。这个角色会有对于门禁的权限，比如说，可以开A和B两个门。
    /// 后来，增加了一个门C，这样，需要把这个C门的权限同步到所有的部门的人上。
    /// </summary>
    public class DepartmentOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserRepository _userRepo = RepositoryManager.GetRepository<IUserRepository>();
        private IUserAuthenticationRepository _userAuthenticationRepo = RepositoryManager.GetRepository<IUserAuthenticationRepository>();
        private IDeviceRoleRepository _deviceRole = RepositoryManager.GetRepository<IDeviceRoleRepository>();

        private const Int32 SyncUserID = 14;

        public void SyncDepartment(Department department, List<DeviceController> devices)
        {
            if (department == null || department.DepartmentID == 0)
            {
                Log.Warn("invalid department...");
                return;
            }
            if (devices == null || devices.Count == 0)
            {
                Log.Warn("invalid device...");
                return;
            }

            var users = _userRepo.Query(new Hashtable { { "DepartmentID", department.DepartmentID } }).ToList();
            if (!users.Any())
            {
                Log.Info("department does not have any users...");
                return;
            }

            var deviceRoles = _deviceRole.Query(new Hashtable { { "Status", (int)GeneralStatus.Enabled } }).ToList();
            var role = deviceRoles.FirstOrDefault(x => x.DeviceRoleID == department.DeviceRoleID);
            foreach (var user in users)
            {
                SyncRoletoUser(user, devices, role);
            }
        }

        private void SyncRoletoUser(User user, IEnumerable<DeviceController> devices, DeviceRole role)
        {
            var deviceRoles = _deviceRole.Query(new Hashtable { { "Status", (int)GeneralStatus.Enabled } }).ToList();

            var currentDeviceIds = user.GetUserAccessableDeviceIds();
            var roleAuthorizedDeviceIds = user.GetUserRoleAuthorizedDeviceIds(deviceRoles);

            var deviceIdToBeDeleted = currentDeviceIds.Except(roleAuthorizedDeviceIds);
            var deviceIdTobeAdded = roleAuthorizedDeviceIds.Except(currentDeviceIds);
            var deviceIdTobeUpdated = currentDeviceIds.Intersect(roleAuthorizedDeviceIds);

            foreach (var deviceId in deviceIdToBeDeleted)
            {
                var deviceInfo = devices.FirstOrDefault(d => d.DeviceID == deviceId);
                if (deviceInfo != null)
                    DeleteUserMS(user, deviceInfo);
            }

            foreach (var deviceId in deviceIdTobeAdded)
            {
                var deviceInfo = devices.FirstOrDefault(d => d.DeviceID == deviceId);
                if (deviceInfo != null)
                    AddUserMS(user, deviceInfo, role);
            }

            foreach (var deviceId in deviceIdTobeUpdated)
            {
                var deviceInfo = devices.FirstOrDefault(d => d.DeviceID == deviceId);
                if (deviceInfo != null)
                    UpdateUserMS(user, deviceInfo, role);
            }
        }

        private void AddUserMS(User user, DeviceController deviceInfo, DeviceRole role)
        {
            var deviceId = deviceInfo.DeviceID;
            var tryGetAuthentication = user.UserAuthentications.FirstOrDefault(x => x.DeviceID != deviceId);
            if (tryGetAuthentication != null)
            {
                var otherDeviceId = tryGetAuthentication.DeviceID;
                var otherdeviceAuthentications = user.UserAuthentications.FindAll(x => x.DeviceID == otherDeviceId);

                var devicePermission = role.DeviceRolePermissions.FirstOrDefault(x => x.DeviceID == deviceId);

                Log.Info("Building device user...");
                var deviceUser = new UserInfo();
                deviceUser.UserId = tryGetAuthentication.DeviceUserID; // don't know how to determine new the device user id
                deviceUser.ExternalUserCode = user.UserID.ToString();
                deviceUser.UserName = user.Name;
                deviceUser.UserStatus = user.Status == GeneralStatus.Enabled;
                deviceUser.DepartmentId = user.DepartmentID;
                deviceUser.Comment = user.Remark;
                deviceUser.Role = (Rld.DeviceSystem.Contract.Model.UserRole)devicePermission.PermissionAction.GetHashCode();
                deviceUser.AccessTimeZoneId = devicePermission.AllowedAccessTimeZoneID;

                foreach (AuthenticationType type in Enum.GetValues(typeof(AuthenticationType)))
                {
                    switch (type)
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
                                var service = new FingerPrintService() { Index = (int)type, Enabled = false };

                                var userAuthentication = otherdeviceAuthentications.FirstOrDefault(a => a.AuthenticationType == type);
                                if (userAuthentication != null)
                                {
                                    service.Enabled = true;
                                    service.FingerPrintData = userAuthentication.AuthenticationData;
                                    service.UseForDuress = userAuthentication.IsDuress;
                                }

                                deviceUser.CredentialServices.Add(service);
                            }
                            break;
                        case AuthenticationType.Password:
                            {
                                var service = new PasswordService() { Enabled = false };

                                var userAuthentication = otherdeviceAuthentications.FirstOrDefault(a => a.AuthenticationType == type);
                                if (userAuthentication != null)
                                {
                                    service.Enabled = true;
                                    service.Password = userAuthentication.AuthenticationData;
                                    service.UseForDuress = userAuthentication.IsDuress;
                                }

                                deviceUser.CredentialServices.Add(service);
                            }
                            break;
                        case AuthenticationType.IcCard:
                            {
                                var service = new CredentialCardService() { Enabled = false };

                                var userAuthentication = otherdeviceAuthentications.FirstOrDefault(a => a.AuthenticationType == type);
                                if (userAuthentication != null)
                                {
                                    service.Enabled = true;
                                    service.CardNumber = userAuthentication.AuthenticationData;
                                    service.UseForDuress = userAuthentication.IsDuress;
                                }

                                deviceUser.CredentialServices.Add(service);
                            }
                            break;
                        //case AuthenticationType.FacePrint:
                        //    break;
                        default:
                            break;
                    }
                }

                Log.Info("Invoke WebSocketOperation...");
                var operation = new WebSocketOperation(deviceId);
                var createUserInfoRequest = new CreateUserInfoRequest() { Token = operation.Token, UserInfo = deviceUser };
                string rawRequest = DataContractSerializationHelper.Serialize(createUserInfoRequest);

                Log.DebugFormat("Request: {0}", rawRequest);
                var rawResponse = operation.Execute(rawRequest);
                Log.DebugFormat("Response: {0}", rawResponse);

                var response = DataContractSerializationHelper.Deserialize<UpdateUserInfoResponse>(rawResponse);
                Log.InfoFormat("Update user id:[{0}], device user id:[{1}] to device id:[{2}], result:[{3}]", user.UserID, deviceUser.UserId, deviceId, response.ResultType);

                if (response.ResultType != ResultType.OK)
                {
                    throw new Exception(string.Format("Update user id:[{0}], device user id:[{1}] to device id:[{2}] fails]", user.UserID, deviceUser.UserId, deviceId));
                }

                Log.Info("Adding UserAuthentications from database...");
                foreach (var au in otherdeviceAuthentications)
                {
                    var a = au.WiseClone();
                    a.DeviceID = deviceId;
                    a.CreateDate = DateTime.Now;
                    a.CreateUserID = SyncUserID;

                    _userAuthenticationRepo.Insert(a);
                }
            }
        }

        private void UpdateUserMS(User user, DeviceController deviceInfo, DeviceRole role)
        {
            var deviceId = deviceInfo.DeviceID;
            var devicePermission = role.DeviceRolePermissions.FirstOrDefault(x => x.DeviceID == deviceId);

            var deviceUser = new UserInfo();
            deviceUser.UserId = user.UserCode.ToInt32();
            deviceUser.Role = (Rld.DeviceSystem.Contract.Model.UserRole) devicePermission.PermissionAction.GetHashCode();
            deviceUser.AccessTimeZoneId = devicePermission.AllowedAccessTimeZoneID;

            Log.Info("Invoke WebSocketOperation...");
            var operation = new WebSocketOperation(deviceId);
            var updateUserInfoRequest = new UpdateUserInfoRequest() {Token = operation.Token, UserInfo = deviceUser};
            string rawRequest = DataContractSerializationHelper.Serialize(updateUserInfoRequest);

            Log.DebugFormat("Request: {0}", rawRequest);
            var rawResponse = operation.Execute(rawRequest);
            Log.DebugFormat("Response: {0}", rawResponse);

            var response = DataContractSerializationHelper.Deserialize<UpdateUserInfoResponse>(rawResponse);
            Log.InfoFormat("Update user id:[{0}], device user id:[{1}] to device id:[{2}], result:[{3}]", user.UserID, deviceUser.UserId, deviceId, response.ResultType);

            if (response.ResultType != ResultType.OK)
            {
                throw new Exception(string.Format("Update user id:[{0}], device user id:[{1}] to device id:[{2}] fails]", user.UserID, deviceUser.UserId, deviceId));
            }
        }

        private void DeleteUserMS(User user, DeviceController deviceInfo)
        {
            var deviceId = deviceInfo.DeviceID;
            var deviceUserId = user.UserCode.ToInt32();
            Log.Info("Invoke WebSocketOperation...");
            var operation = new WebSocketOperation(deviceId);
            var deleteUserInfoRequest = new DeleteUserInfoRequest() {Token = operation.Token, UserId = deviceUserId};
            string rawRequest = DataContractSerializationHelper.Serialize(deleteUserInfoRequest);

            Log.DebugFormat("Request: {0}", rawRequest);
            var rawResponse = operation.Execute(rawRequest);
            Log.DebugFormat("Response: {0}", rawResponse);

            Log.Info("Deleting UserAuthentications from database...");
            var authentications = user.UserAuthentications.FindAll(x => x.DeviceID == deviceId);
            authentications.ForEach(x => _userAuthenticationRepo.Delete(x.UserAuthenticationID));
        }
    }
}