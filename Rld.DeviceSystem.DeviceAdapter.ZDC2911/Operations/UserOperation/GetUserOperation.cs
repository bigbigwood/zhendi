using Rld.DeviceSystem.Contract.Message;
using Rld.DeviceSystem.Contract.Message.GetUserOperation;
using Rld.DeviceSystem.DeviceAdapter.ZDC2911.Mapper;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.UserOperation
{
    public class GetUserOperation
    {
        public GetUserResponse Process(GetUserRequest request)
        {
            if (request.UserId == 0)
            {

            }

            var service = new UserService(DeviceManager.GetInstance().GetDeviceProxy(1));
            var userInfo = service.GetUserInfo(request.UserId);

            return new GetUserResponse() { ResultType = ResultType.OK, UserInfo = userInfo };
        }

        #region backup, maybe use later.

        //public GetUserResponse Process(GetUserRequest request)
        //{
        //    if (request.UserId == 0)
        //    {

        //    }

        //    User userInfo = new User() { UserId = request.UserId };
        //    var service = new UserService(DeviceManager.GetInstance().GetDeviceProxy(1));

        //    var userEnrollSummary = service.GetUserEnrollSummary(userInfo.UserId);

        //    foreach (var requestType in request.RequestTypes)
        //    {
        //        switch (requestType)
        //        {
        //            case UserRequestType.UserName:
        //                 service.GetUserName(ref userInfo);
        //                break;

        //            case UserRequestType.UserRole:
        //            case UserRequestType.Password:
        //            case UserRequestType.CredentialCard:
        //            case UserRequestType.FingerPrint0:
        //            case UserRequestType.FingerPrint1:
        //            case UserRequestType.FingerPrint2:
        //            case UserRequestType.FingerPrint3:
        //            case UserRequestType.FingerPrint4:
        //            case UserRequestType.FingerPrint5:
        //            case UserRequestType.FingerPrint6:
        //            case UserRequestType.FingerPrint7:
        //            case UserRequestType.FingerPrint8:
        //            case UserRequestType.FingerPrint9:
        //                {
        //                    if (requestType == UserRequestType.UserRole)
        //                        userInfo.Role = userEnrollSummary.Role;
        //                    else if (requestType == UserRequestType.Password && userEnrollSummary.PasswordEnabled)
        //                        service.GetPassword(ref userInfo);
        //                    else if (requestType == UserRequestType.CredentialCard && userEnrollSummary.CardEnabled)
        //                        service.GetCard(ref userInfo);
        //                    else if (requestType == UserRequestType.FingerPrint0 && userEnrollSummary.FingerPrint0Enabled)
        //                        service.GetFingerPrint(ref userInfo, 0);
        //                    else if (requestType == UserRequestType.FingerPrint1 && userEnrollSummary.FingerPrint1Enabled)
        //                        service.GetFingerPrint(ref userInfo, 1);
        //                    else if (requestType == UserRequestType.FingerPrint2 && userEnrollSummary.FingerPrint2Enabled)
        //                        service.GetFingerPrint(ref userInfo, 2);
        //                    else if (requestType == UserRequestType.FingerPrint3 && userEnrollSummary.FingerPrint3Enabled)
        //                        service.GetFingerPrint(ref userInfo, 3);
        //                    else if (requestType == UserRequestType.FingerPrint4 && userEnrollSummary.FingerPrint4Enabled)
        //                        service.GetFingerPrint(ref userInfo, 4);
        //                    else if (requestType == UserRequestType.FingerPrint5 && userEnrollSummary.FingerPrint5Enabled)
        //                        service.GetFingerPrint(ref userInfo, 5);
        //                    else if (requestType == UserRequestType.FingerPrint6 && userEnrollSummary.FingerPrint6Enabled)
        //                        service.GetFingerPrint(ref userInfo, 6);
        //                    else if (requestType == UserRequestType.FingerPrint7 && userEnrollSummary.FingerPrint7Enabled)
        //                        service.GetFingerPrint(ref userInfo, 7);
        //                    else if (requestType == UserRequestType.FingerPrint8 && userEnrollSummary.FingerPrint8Enabled)
        //                        service.GetFingerPrint(ref userInfo, 8);
        //                    else if (requestType == UserRequestType.FingerPrint9 && userEnrollSummary.FingerPrint9Enabled)
        //                        service.GetFingerPrint(ref userInfo, 9);
        //                }

        //                break;

        //            default: break;
        //        }
        //    }


        //    return new GetUserResponse() { UserInfo = userInfo};
        //} 
        #endregion
    }
}