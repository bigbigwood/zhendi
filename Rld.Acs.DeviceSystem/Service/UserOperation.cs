using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Rld.Acs.DeviceSystem.Framework;
using Rld.Acs.Model;
using Rld.Acs.Model.Extension;
using Rld.Acs.Unility.Serialization;
using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.UserCredential;

namespace Rld.Acs.DeviceSystem.Service
{
    public class UserOperation
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void UpdateDeviceUser(User user, DeviceController device)
        {
            if (user == null || device == null) return;
            if (user.UserAuthentications == null || user.UserAuthentications.Count == 0) return;
            if (user.GetUserAccessableDeviceIds().Contains(device.DeviceID) == false) return;

            var deviceID = device.DeviceID;
            var userAuthenticationsOfDevice = user.UserAuthentications.Where(a => a.DeviceID == deviceID);

            var deviceUser = new UserInfo();
            var authenticationsOfDevice = userAuthenticationsOfDevice as IList<UserAuthentication> ?? userAuthenticationsOfDevice.ToList();
            deviceUser.UserId = authenticationsOfDevice.First().DeviceUserID;
            deviceUser.ExternalUserCode = user.UserID.ToString();
            //deviceUser.Role = user.UserDeviceRoles.First().DeviceRoleID;
            deviceUser.UserName = user.Name;
            deviceUser.UserStatus = user.Status == GeneralStatus.Enabled;
            deviceUser.DepartmentId = user.DepartmentID;
            deviceUser.Comment = user.Remark;
            deviceUser.AccessTimeZoneId = 0; //Todo

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
            var operation = new WebSocketOperation(deviceID);
            var updateUserInfoRequest = new UpdateUserInfoRequest() { Token = operation.Token, UserInfo = deviceUser };
            string rawRequest = DataContractSerializationHelper.Serialize(updateUserInfoRequest);
            var rawResponse = operation.Execute(rawRequest);

            var response = DataContractSerializationHelper.Deserialize<UpdateUserInfoResponse>(rawResponse);
            Log.InfoFormat("Update device user id:{0} to device id:{1}, result ={2}", user.UserID, deviceID, response.ResultType);

            if (response.ResultType != ResultType.OK)
            {
                throw new Exception(string.Format("Update device user id:{0} to device id:{1} fails", user.UserID, deviceID));
            }
        }
    }
}