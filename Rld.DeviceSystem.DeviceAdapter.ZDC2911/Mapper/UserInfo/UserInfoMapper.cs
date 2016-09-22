using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Riss.Devices;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Services.DeviceConn;
using Rld.DeviceSystem.Contract.Model.Services.UserCredential;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.SystemInfo;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Model;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper.UserInfo
{
    public class UserInfoMapper
    {
        private static readonly ILog Log = LogManager.GetLogger(global::System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Contract.Model.UserInfo ToModel(User deviceUser)
        {
            var userInfo = new  Contract.Model.UserInfo() { UserId = (int)deviceUser.DIN };

            try
            {
                userInfo.UserName = deviceUser.UserName;
                userInfo.Role = (UserRole)deviceUser.Privilege;
                userInfo.Comment = deviceUser.Comment;
                userInfo.UserStatus = deviceUser.Enable;
                userInfo.DepartmentId = deviceUser.Department;
                userInfo.AccessTimeZoneId = deviceUser.AccessTimeZone;

                var enroll = deviceUser.Enrolls.First();
                var services = new List<CredentialService>();

                if (!string.IsNullOrWhiteSpace(enroll.Password))
                    services.Add(new PasswordService() { Enabled = true, Password = ConvertObject.ToPrettyString(enroll.Password) });

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

        public static void UpdateSystemInfo(ref User deviceUser, Contract.Model.UserInfo userInfo)
        {
            if (!string.IsNullOrWhiteSpace(userInfo.UserName))
                deviceUser.UserName = userInfo.UserName;

            if (userInfo.Role != null)
                deviceUser.Privilege = (Int32)userInfo.Role;

            if (!string.IsNullOrWhiteSpace(userInfo.Comment))
                deviceUser.Comment = userInfo.Comment;

            if (userInfo.UserStatus != null)
                deviceUser.Enable = userInfo.UserStatus.Value;

            if (userInfo.DepartmentId != null)
                deviceUser.Department = userInfo.DepartmentId.Value;

            if (userInfo.AccessTimeZoneId != null)
                deviceUser.AccessTimeZone = userInfo.AccessTimeZoneId.Value;

            if (userInfo.CredentialServices != null)
            {
                var enroll = deviceUser.Enrolls.First();
                int enrollType = (int)enroll.EnrollType;

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
        }
    }
}