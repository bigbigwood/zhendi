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
    public class UserOp
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserRepository _userRepo = RepositoryManager.GetRepository<IUserRepository>();
        private IDeviceRoleRepository _deviceRole = RepositoryManager.GetRepository<IDeviceRoleRepository>();
        private const Int32 SyncUserID = 14;

        public void UpdateDeviceUser(User user, DeviceController device)
        {
            if (user == null || device == null) return;
            if (user.UserAuthentications == null || user.UserAuthentications.Count == 0) return;
            if (user.GetUserAccessableDeviceIds().Contains(device.DeviceID) == false) return;

            var deviceID = device.DeviceID;
            Log.Info("Getting user authentication infos...");
            var userAuthenticationsOfDevice = user.UserAuthentications.Where(a => a.DeviceID == deviceID);
            var authenticationsOfDevice = userAuthenticationsOfDevice as IList<UserAuthentication> ?? userAuthenticationsOfDevice.ToList();

            Log.Info("Getting user permission infos...");
            var deviceRoles = _deviceRole.Query(new Hashtable { { "Status", (int)GeneralStatus.Enabled } }).ToList();
            var userDevicePermission = user.GetUserDeviceRoleAuthorizedPermissionByDeviceId(deviceID, deviceRoles);

            Log.Info("Building device user...");
            var deviceUser = new UserInfo();
            deviceUser.UserId = user.GetDeviceUserId(device);
            deviceUser.ExternalUserCode = user.UserID.ToString();
            deviceUser.UserName = user.Name;
            deviceUser.UserStatus = user.Status == GeneralStatus.Enabled;
            deviceUser.DepartmentId = user.DepartmentID;
            deviceUser.Comment = user.Remark;
            deviceUser.Role = (Rld.DeviceSystem.Contract.Model.UserRole)userDevicePermission.PermissionAction.GetHashCode();
            deviceUser.AccessTimeZoneId = userDevicePermission.AllowedAccessTimeZoneID;

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

                            var userAuthentication = authenticationsOfDevice.FirstOrDefault(a => a.AuthenticationType == type);
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

                            var userAuthentication = authenticationsOfDevice.FirstOrDefault(a => a.AuthenticationType == type);
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

                            var userAuthentication = authenticationsOfDevice.FirstOrDefault(a => a.AuthenticationType == type);
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
            var operation = new WebSocketOperation(deviceID);
            var updateUserInfoRequest = new UpdateUserInfoRequest() { Token = operation.Token, UserInfo = deviceUser };
            string rawRequest = DataContractSerializationHelper.Serialize(updateUserInfoRequest);

            Log.DebugFormat("Request: {0}", rawRequest);
            var rawResponse = operation.Execute(rawRequest);
            Log.DebugFormat("Response: {0}", rawResponse);
            if (string.IsNullOrWhiteSpace(rawResponse))
            {
                throw new Exception(string.Format("Update user id:[{0}], device user id:[{1}] to device id:[{2}] fails]. Response is empty, maybe the device is not register to device system." , 
                    user.UserID, deviceUser.UserId, deviceID));
            }

            var response = DataContractSerializationHelper.Deserialize<UpdateUserInfoResponse>(rawResponse);
            Log.InfoFormat("Update user id:[{0}], device user id:[{1}] to device id:[{2}], result:[{3}]", user.UserID, deviceUser.UserId, deviceID, response.ResultType);

            if (response.ResultType != ResultType.OK)
            {
                throw new Exception(string.Format("Update user id:[{0}], device user id:[{1}] to device id:[{2}] fails]", user.UserID, deviceUser.UserId, deviceID));
            }
        }

        public void UpdateDBUser(User user, DeviceController device)
        {
            if (user == null || device == null) return;
            if (user.UserAuthentications == null || user.UserAuthentications.Count == 0) return;
            if (user.GetUserAccessableDeviceIds().Contains(device.DeviceID) == false) return;

            var deviceID = device.DeviceID;

            Log.Info("Getting user authentication infos...");
            var userAuthenticationsOfDevice = user.UserAuthentications.Where(a => a.DeviceID == deviceID);
            var authenticationsOfDevice = userAuthenticationsOfDevice as IList<UserAuthentication> ?? userAuthenticationsOfDevice.ToList();
            var deviceUserId = user.GetDeviceUserId(device);

            //var deviceRoles = _deviceRole.Query(new Hashtable { { "Status", (int)GeneralStatus.Enabled } }).ToList();
            //var userDevicePermission = user.GetUserDevicePermission(deviceID, deviceRoles);

            Log.Info("Invoke WebSocketOperation...");
            var operation = new WebSocketOperation(deviceID);
            var getUserInfoRequest = new GetUserInfoRequest() { Token = operation.Token, UserId = deviceUserId };
            string rawRequest = DataContractSerializationHelper.Serialize(getUserInfoRequest);

            Log.DebugFormat("Request: {0}", rawRequest);
            var rawResponse = operation.Execute(rawRequest);
            Log.DebugFormat("Response: {0}", rawResponse);

            var response = DataContractSerializationHelper.Deserialize<GetUserInfoResponse>(rawResponse);
            Log.InfoFormat("Getting user id:[{0}], device user id:[{1}] from device id:[{2}], result:[{3}]", user.UserID, deviceUserId, deviceID, response.ResultType);

            if (response.ResultType != ResultType.OK)
            {
                throw new Exception(string.Format("Getting user id:[{0}], device user id:[{1}] from device id:[{2}]", user.UserID, deviceUserId, deviceID));
            }

            var deviceUser = response.UserInfo;
            //user.Name = deviceUser.UserName;
            //user.Status = deviceUser.UserStatus == true ? GeneralStatus.Enabled : GeneralStatus.Disabled;
            //user.DepartmentID = deviceUser.DepartmentId ?? 0;
            //user.Remark = deviceUser.Comment;

            //Role
            //deviceUser.Role = (Rld.DeviceSystem.Contract.Model.UserRole)userDevicePermission.PermissionAction.GetHashCode();
            //deviceUser.AccessTimeZoneId = userDevicePermission.AllowedAccessTimeZoneID;

            foreach (var service in  deviceUser.CredentialServices)
            {
                if (service is PasswordService)
                {
                    var passwordService = service as PasswordService;
                    var userAuthentication = authenticationsOfDevice.FirstOrDefault(a => a.AuthenticationType == AuthenticationType.Password);
                    if (userAuthentication == null)
                    {
                        userAuthentication = new UserAuthentication()
                        {
                            UserID = user.UserID,
                            DeviceID = deviceID,
                            DeviceUserID = deviceUserId,
                            AuthenticationType = AuthenticationType.Password,
                            Status = GeneralStatus.Enabled,
                            CreateDate = DateTime.Now,
                            CreateUserID = SyncUserID,
                        };
                        user.UserAuthentications.Add(userAuthentication);
                    }

                    userAuthentication.AuthenticationData = passwordService.Password;
                    userAuthentication.IsDuress = passwordService.UseForDuress;
                }
                else if (service is CredentialCardService)
                {
                    var credentialCardService = service as CredentialCardService;
                    var userAuthentication = authenticationsOfDevice.FirstOrDefault(a => a.AuthenticationType == AuthenticationType.IcCard);
                    if (userAuthentication == null)
                    {
                        userAuthentication = new UserAuthentication()
                        {
                            UserID = user.UserID,
                            DeviceID = deviceID,
                            DeviceUserID = deviceUserId,
                            AuthenticationType = AuthenticationType.IcCard,
                            Status = GeneralStatus.Enabled,
                            CreateDate = DateTime.Now,
                            CreateUserID = SyncUserID,
                        };
                        user.UserAuthentications.Add(userAuthentication);
                    }

                    userAuthentication.AuthenticationData = credentialCardService.CardNumber;
                    userAuthentication.IsDuress = credentialCardService.UseForDuress;
                }
                else if (service is FingerPrintService)
                {
                    var fpService = service as FingerPrintService;

                    var userAuthentication = authenticationsOfDevice.FirstOrDefault(a => a.AuthenticationType.GetHashCode() == fpService.Index);
                    if (userAuthentication == null)
                    {
                        userAuthentication = new UserAuthentication()
                        {
                            UserID = user.UserID,
                            DeviceID = deviceID,
                            DeviceUserID = deviceUserId,
                            AuthenticationType = (AuthenticationType)fpService.Index,
                            Status = GeneralStatus.Enabled,
                            CreateDate = DateTime.Now,
                            CreateUserID = SyncUserID,
                        };

                        user.UserAuthentications.Add(userAuthentication);
                    }

                    userAuthentication.AuthenticationData = fpService.FingerPrintData;
                    userAuthentication.IsDuress = fpService.UseForDuress;
                }
            }

            Log.Info("Sync user to database...");
            _userRepo.Update(user);
        }
    }
}