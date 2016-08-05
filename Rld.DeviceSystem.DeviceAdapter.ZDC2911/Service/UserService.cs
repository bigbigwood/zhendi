using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.Contract.Model.Services;
using Rld.DeviceSystem.Contract.Model.Services.UserCredential;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.UserOperation;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Service
{
    public class UserService
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceProxy _deviceProxy = null;
        public UserService(DeviceProxy proxy)
        {
            _deviceProxy = proxy;
        }

        public Contract.Model.Users.User GetUserInfo(int userId)
        {
            try
            {
                var device = _deviceProxy.Device;
                var deviceUser = new User() { DIN = (UInt64)userId };
                object extraData = new object();

                bool result = false;
                result = _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, null, device, DeviceStatus.DeviceBusy);

                result = _deviceProxy.DeviceConnection.GetProperty(UserProperty.Enroll, null, ref deviceUser, ref extraData);

                result = _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, null, device, DeviceStatus.DeviceIdle);


                var userInfo = new Contract.Model.Users.User() { UserId = userId };
                userInfo.UserName = deviceUser.UserName;
                userInfo.Role = (Contract.Model.Users.UserRole)deviceUser.Privilege;
                userInfo.Comment = deviceUser.Comment;
                userInfo.UserStatus = deviceUser.Enable;
                userInfo.DepartmentId = !string.IsNullOrWhiteSpace(deviceUser.DeptId) ? int.Parse(deviceUser.DeptId) : 0;
                userInfo.AccessTimeZoneId = deviceUser.AccessTimeZone;

                var enroll = deviceUser.Enrolls.First();
                List<CredentialService> services = new List<CredentialService>();

                if (!string.IsNullOrWhiteSpace(enroll.Password))
                    services.Add(new PasswordService() { Enabled = true, Password = enroll.Password });

                if (!string.IsNullOrWhiteSpace(enroll.CardID))
                    services.Add(new CredentialCardService() { Enabled = true, CardNumber = enroll.CardID });

                int enrollType = (Int32)enroll.EnrollType;
                for (int index = 0; index < Zd2911Utils.MaxFingerprintCount; index++)
                {
                    if (Zd2911Utils.BitCheck(enrollType, index) != 0)
                    {
                        byte[] fpBytes = enroll.Fingerprint.Skip(index * Zd2911Utils.MaxFingerprintLength).Take(Zd2911Utils.MaxFingerprintLength).ToArray();
                        services.Add(new FingerPrintService() { Enabled = true, FingerPrintData = ConvertObject.ConvertByteToHex(fpBytes) });
                    }
                }

                userInfo.CredentialServices = services;
                return userInfo;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        public bool ModifyUserInfo(Contract.Model.Users.User userInfo)
        {
            var device = _deviceProxy.Device;

            var enroll = new Enroll() { DIN = (UInt64)userInfo.UserId, Fingerprint = new byte[Zd2911Utils.MaxFingerprintLength * 10] };
            var deviceUser = new User() { DIN = (UInt64)userInfo.UserId, Enrolls = new List<Enroll> { enroll } };

            if (!string.IsNullOrWhiteSpace(userInfo.UserName))
                deviceUser.UserName = userInfo.UserName;

            if (userInfo.Role != null)
                deviceUser.Privilege = (Int32)userInfo.Role;

            if (!string.IsNullOrWhiteSpace(userInfo.Comment))
                deviceUser.Comment = userInfo.Comment;

            if (userInfo.UserStatus != null)
                deviceUser.Enable = userInfo.UserStatus.Value;

            if (userInfo.DepartmentId != null)
                deviceUser.DeptId = userInfo.DepartmentId.ToString();

            if (userInfo.AccessTimeZoneId != null)
                deviceUser.AccessTimeZone = userInfo.AccessTimeZoneId.Value;

            if (userInfo.CredentialServices != null)
            {
                object extraProperty = (UInt64)userInfo.UserId;
                object extraData = new object();
                var getSummaryResult = _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.Enrolls, extraProperty, ref device, ref extraData);

                var rawUser = (User)extraData;
                var rawEnroll = rawUser.Enrolls.First();
                int enrollType = (int)rawEnroll.EnrollType;


                foreach (var service in userInfo.CredentialServices)
                {
                    if (service is PasswordService)
                    {
                        var passwordService = service as PasswordService;
                        if (passwordService.Enabled)
                        {
                            if (Zd2911Utils.BitCheck(enrollType, (int)EnrollType.Password) == 0)
                                enrollType += Zd2911Utils.SetBit(0, (int)EnrollType.Password);

                            enroll.Password = passwordService.Password;
                        }
                    }
                    else if (service is CredentialCardService)
                    {
                        var credentialCardService = service as CredentialCardService;
                        if (credentialCardService.Enabled)
                        {
                            if (Zd2911Utils.BitCheck(enrollType, (int)EnrollType.Card) == 0)
                                enrollType += Zd2911Utils.SetBit(0, (int)EnrollType.Card); //password is 10, fp0-fp9, card is 11

                            enroll.CardID = credentialCardService.CardNumber;
                        }

                    }
                    else if (service is FingerPrintService)
                    {
                        var fpService = service as FingerPrintService;
                        if (fpService.Enabled)
                        {
                            if (Zd2911Utils.BitCheck(enrollType, fpService.Index) == 0)
                                enrollType += Zd2911Utils.SetBit(0, fpService.Index);

                            byte[] bytesFromText = ConvertObject.ConvertStringToBytes(fpService.FingerPrintData);
                            Array.Copy(bytesFromText, 0, enroll.Fingerprint, fpService.Index * Zd2911Utils.MaxFingerprintLength, bytesFromText.Length);
                        }
                    }
                }

                enroll.EnrollType = (EnrollType)enrollType;
            }


            bool result = false;
            result = _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, null, device, DeviceStatus.DeviceBusy);

            result = _deviceProxy.DeviceConnection.SetProperty(UserProperty.Enroll, null, deviceUser, false);

            result = _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, null, device, DeviceStatus.DeviceIdle);

            return result;
        }


        public bool CreateUserInfo(Contract.Model.Users.User userInfo)
        {
            var device = _deviceProxy.Device;

            var enroll = new Enroll() { DIN = (UInt64)userInfo.UserId, Fingerprint = new byte[Zd2911Utils.MaxFingerprintLength * 10] };
            var deviceUser = new User() { DIN = (UInt64)userInfo.UserId, Enrolls = new List<Enroll> { enroll } };

            if (!string.IsNullOrWhiteSpace(userInfo.UserName))
                deviceUser.UserName = userInfo.UserName;

            if (userInfo.Role != null)
                deviceUser.Privilege = (Int32)userInfo.Role;

            if (!string.IsNullOrWhiteSpace(userInfo.Comment))
                deviceUser.Comment = userInfo.Comment;

            if (userInfo.UserStatus != null)
                deviceUser.Enable = userInfo.UserStatus.Value;

            if (userInfo.DepartmentId != null)
                deviceUser.DeptId = userInfo.DepartmentId.ToString();

            if (userInfo.AccessTimeZoneId != null)
                deviceUser.AccessTimeZone = userInfo.AccessTimeZoneId.Value;

            if (userInfo.CredentialServices != null)
            {
                int enrollType = 0;
                foreach (var service in userInfo.CredentialServices)
                {
                    if (service is PasswordService)
                    {
                        var passwordService = service as PasswordService;
                        if (passwordService.Enabled)
                        {
                            enrollType += Zd2911Utils.SetBit(0, (int)EnrollType.Password);
                            enroll.Password = passwordService.Password;
                        }
                    }
                    else if (service is CredentialCardService)
                    {
                        var credentialCardService = service as CredentialCardService;
                        if (credentialCardService.Enabled)
                        {
                            enrollType += Zd2911Utils.SetBit(0, (int)EnrollType.Card);
                            enroll.CardID = credentialCardService.CardNumber;
                        }

                    }
                    else if (service is FingerPrintService)
                    {
                        var fpService = service as FingerPrintService;
                        if (fpService.Enabled)
                        {
                            enrollType += Zd2911Utils.SetBit(0, fpService.Index);
                            byte[] bytesFromText = ConvertObject.ConvertStringToBytes(fpService.FingerPrintData);
                            Array.Copy(bytesFromText, 0, enroll.Fingerprint, fpService.Index * Zd2911Utils.MaxFingerprintLength, bytesFromText.Length);
                        }
                    }
                }

                enroll.EnrollType = (EnrollType)enrollType;
            }


            bool result = false;
            result = _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, null, device, DeviceStatus.DeviceBusy);

            result = _deviceProxy.DeviceConnection.SetProperty(UserProperty.Enroll, null, deviceUser, false);

            result = _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, null, device, DeviceStatus.DeviceIdle);

            return result;
        }

        public bool DeleteUserInfo(int userId)
        {
            try
            {
                var device = _deviceProxy.Device;
                object extraProperty = new object();
                object extraData = new object();
                extraData = (UInt64)userId;
                bool result = _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enrolls, extraProperty, device, extraData);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        public bool SetUserName(int userId, string userName)
        {
            try
            {
                object extraProperty = new object();
                object extraData = new object();
                var user = new User() { DIN = (UInt64)userId, UserName = userName };
                bool result = _deviceProxy.DeviceConnection.SetProperty(UserProperty.UserName, extraProperty, user, extraData);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public void GetUserName(ref Contract.Model.Users.User DeviceUser)
        {
            var extraData = new object();
            var user = new User() { DIN = (UInt64)DeviceUser.UserId };
            bool result = _deviceProxy.DeviceConnection.GetProperty(UserProperty.UserName, new object(), ref user, ref extraData);
            DeviceUser.UserName = user.UserName;
        }

        public void GetPassword(ref Contract.Model.Users.User DeviceUser)
        {
            object extraData = new object();
            Enroll enroll = new Enroll() { DIN = (UInt64)DeviceUser.UserId };
            User user = new User() { DIN = (UInt64)DeviceUser.UserId, Enrolls = new List<Enroll> { enroll } };
            bool result = _deviceProxy.DeviceConnection.GetProperty(UserProperty.UserEnroll, UserEnrollCommand.ReadPassword, ref user, ref extraData);
            var service = new PasswordService() { Password = user.Enrolls[0].Password.Replace("\0", ""), Enabled = true };
            DeviceUser.CredentialServices.Add(service);
        }

        public void GetCard(ref Contract.Model.Users.User DeviceUser)
        {
            object extraData = new object();
            Enroll enroll = new Enroll() { DIN = (UInt64)DeviceUser.UserId };
            User user = new User() { DIN = (UInt64)DeviceUser.UserId, Enrolls = new List<Enroll> { enroll } };
            bool result = _deviceProxy.DeviceConnection.GetProperty(UserProperty.UserEnroll, UserEnrollCommand.ReadPassword, ref user, ref extraData);
            var service = new CredentialCardService() { CardNumber = user.Enrolls[0].CardID, Enabled = true };
            DeviceUser.CredentialServices.Add(service);
        }

        public void GetFingerPrint(ref Contract.Model.Users.User DeviceUser, Int32 index)
        {
            try
            {
                var device = _deviceProxy.Device;
                object extraProperty = new object();
                object extraData = new object();

                bool result = _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceBusy);

                Enroll enroll = new Enroll() { DIN = (UInt64)DeviceUser.UserId, EnrollType = (EnrollType)index, Fingerprint = new byte[Zd2911Utils.MaxFingerprintLength] };
                User user = new User() { DIN = (UInt64)DeviceUser.UserId, Enrolls = new List<Enroll> { enroll } };
                result = _deviceProxy.DeviceConnection.GetProperty(UserProperty.UserEnroll, UserEnrollCommand.ReadFingerprint, ref user, ref extraData);

                var fpBytes = user.Enrolls[0].Fingerprint;
                var service = new FingerPrintService() { Enabled = true, Index = index, FingerPrintData = ConvertObject.ConvertByteToHex(fpBytes) };
                DeviceUser.CredentialServices.Add(service);

                _deviceProxy.DeviceConnection.SetProperty(DeviceProperty.Enable, extraProperty, device, DeviceStatus.DeviceIdle);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public UserEnrollSummary GetUserEnrollSummary(Int32 userID)
        {
            var device = _deviceProxy.Device;
            object extraProperty = new object();
            extraProperty = (UInt64)userID;
            object extraData = new object();
            bool result = _deviceProxy.DeviceConnection.GetProperty(DeviceProperty.Enrolls, extraProperty, ref device, ref extraData);

            var rawUser = (User)extraData;
            var rawEnroll = rawUser.Enrolls.First();

            var dto = new UserEnrollSummary();
            dto.Role = (Contract.Model.Users.UserRole)rawUser.Privilege;
            dto.PasswordEnabled = (Zd2911Utils.BitCheck((int)rawEnroll.EnrollType, (int)EnrollType.Password) != 0);
            dto.CardEnabled = (Zd2911Utils.BitCheck((int)rawEnroll.EnrollType, (int)EnrollType.Card) != 0);
            dto.FingerPrint0Enabled = (Zd2911Utils.BitCheck((int)rawEnroll.EnrollType, (int)EnrollType.Finger0) != 0);
            dto.FingerPrint1Enabled = (Zd2911Utils.BitCheck((int)rawEnroll.EnrollType, (int)EnrollType.Finger1) != 0);
            dto.FingerPrint2Enabled = (Zd2911Utils.BitCheck((int)rawEnroll.EnrollType, (int)EnrollType.Finger2) != 0);
            dto.FingerPrint3Enabled = (Zd2911Utils.BitCheck((int)rawEnroll.EnrollType, (int)EnrollType.Finger3) != 0);
            dto.FingerPrint4Enabled = (Zd2911Utils.BitCheck((int)rawEnroll.EnrollType, (int)EnrollType.Finger4) != 0);
            dto.FingerPrint5Enabled = (Zd2911Utils.BitCheck((int)rawEnroll.EnrollType, (int)EnrollType.Finger5) != 0);
            dto.FingerPrint6Enabled = (Zd2911Utils.BitCheck((int)rawEnroll.EnrollType, (int)EnrollType.Finger6) != 0);
            dto.FingerPrint7Enabled = (Zd2911Utils.BitCheck((int)rawEnroll.EnrollType, (int)EnrollType.Finger7) != 0);
            dto.FingerPrint8Enabled = (Zd2911Utils.BitCheck((int)rawEnroll.EnrollType, (int)EnrollType.Finger8) != 0);
            dto.FingerPrint9Enabled = (Zd2911Utils.BitCheck((int)rawEnroll.EnrollType, (int)EnrollType.Finger9) != 0);

            return dto;
        }
    }
}