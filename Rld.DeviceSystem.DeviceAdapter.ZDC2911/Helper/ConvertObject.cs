using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Riss.Devices;

namespace Rld.DeviceSystem.DeviceAdapter.ZDC2911.Helper
{
    public class ConvertObject {
        private ConvertObject() { }

        /// <summary>
        /// 将目标IP地址字符串转换为数字
        /// </summary>
        /// <param name="strIPAddress">IP地址字符串</param>
        /// <returns>数字</returns>
        public static int ConvertIPAddressToNumber(string strIPAddress) {
            string[] arrayIP = strIPAddress.Split('.');
            int sip1 = Int32.Parse(arrayIP[0]);
            int sip2 = Int32.Parse(arrayIP[1]);
            int sip3 = Int32.Parse(arrayIP[2]);
            int sip4 = Int32.Parse(arrayIP[3]);
            int tmpIpNumber;
            tmpIpNumber = (sip1 << 24) + (sip2 << 16) + (sip3 << 8) + sip4;
            return tmpIpNumber;
        }

        /// <summary>
        /// 将目标整形数字转换为IP地址字符串 
        /// </summary>
        /// <param name="intIPAddress">整形数字</param>
        /// <returns>字转换为IP地址</returns>
        public static string ConvertNumberToIPAddress(int intIPAddress) {
            byte[] bs = BitConverter.GetBytes(intIPAddress);
            return string.Format("{0}.{1}.{2}.{3}", bs[3], bs[2], bs[1], bs[0]);
        }

        /// <summary>
        /// 判断是否为合法的IP地址格式
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>true：合法的IP地址，false：非法的IP地址</returns>
        public static bool IsCorrenctIP(string ip) {
            if (Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}")) {
                string[] ips = ip.Split('.');
                if (4 == ips.Length) {
                    if (Int32.Parse(ips[0]) < 256 && Int32.Parse(ips[1]) < 256
                        && Int32.Parse(ips[2]) < 256 && Int32.Parse(ips[3]) < 256) {
                        return true;
                    } else {
                        return false;
                    }
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }

        /// <summary>
        /// 判断是否为正整数
        /// </summary>
        /// <param name="content">文本字符串</param>
        /// <returns>true：正整数，false：非正整数</returns>
        public static bool IsInt(string content) {
            if (Regex.IsMatch(content, "^\\d+$")) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// 逐字节变为16进制字符，以-隔开
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>16进制字符</returns>
        public static string ConvertByteToHex(byte[] bytes) {
            //string result = string.Empty;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++) {
                sb.Append( "-" + Convert.ToString(bytes[i], 16).PadLeft(2, '0'));
            }
            return sb.ToString().Remove(0, 1).ToUpper();
        }

        /// <summary>
        /// added by Axu at 20130828 for new requirement.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ConvertStringToBytes(string str)
        {
            string[] byteStr = str.Split(new char[] { '-' });
            byte[] bytes = new byte[Zd2911Utils.MaxFingerprintLength];
            for (int i = 0; i < byteStr.Length; i++)
			{
                bytes[i] =  Convert.ToByte(byteStr[i], 16);
			}

            return bytes;
        }

        public static string SLogType(int opertationType) {
            string message = string.Empty;

            switch (opertationType) {
                case 1:
                    message = "Registered user";
                    break;
                case 2:
                    message = "Register super administrator";
                    break;
                case 3:
                    message = "Register registrar";
                    break;
                case 4:
                    message = "Register query member";
                    break;
                case 5:
                    message = "Delete fingerprint";
                    break;
                case 6:
                    message = "Remove the password";
                    break;
                case 7:
                    message = "Delete the card number";
                    break;
                case 8:
                    message = "Remove access to records";
                    break;
                case 9:
                    message = "Remove records";
                    break;
                case 10:
                    message = "Set system information";
                    break;
                case 11:
                    message = "Setup time";
                    break;
                case 12:
                    message = "Set the record information";
                    break;
                case 13:
                    message = "Set the communications and information";
                    break;
                case 14:
                    message = "Set the access control information";
                    break;
                case 15:
                    message = "Set the user type";
                    break;
                case 16:
                    message = "Set the attendance time";
                    break;
            }
            return message;
        }

        public static string IOMode(int mode) {
            string message = string.Empty;

            switch (mode) {
                case 0:
                    message = "Check in";
                    break;
                case 1:
                    message = "Clock in";
                    break;
                case 2:
                    message = "Clock out";
                    break;
                case 3:
                    message = "Customer in";
                    break;
                case 4:
                    message = "Customer out";
                    break;
                case 5:
                    message = "Out";
                    break;
                case 6:
                    message = "In";
                    break;
                case 7:
                    message = "User defined 1";
                    break;
                case 8:
                    message = "User defined 2";
                    break;
                case 12:
                    message = "Button open";
                    break;
                case 13:
                    message = "Software open";
                    break;
                case 14:
                    message = "Keep open";
                    break;
                case 15:
                    message = "Keep close";
                    break;
                case 16:
                    message = "Auto mode";
                    break;
                case 17:
                    message = "Open in";
                    break;
                case 18:
                    message = "Open out";
                    break;
                case 19:
                    message = "Overtime open alarm";
                    break;
                case 20:
                    message = "Forced open alarm";
                    break;
                case 21:
                    message = "Antihijack alarm";
                    break;
                case 22:
                    message = "Input action 1";
                    break;
                case 23:
                    message = "Input action 2";
                    break;
                case 24:
                    message = "Output action 1";
                    break;
                case 25:
                    message = "Output action 2";
                    break;
                case 26:
                    message = "Invalid time";
                    break;
                case 27:
                    message = "Invalid date";
                    break;
                case 31:
                    message = "Illegal operation";
                    break;
            }
            return message;
        }

        public static string GLogType(int opertationType) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 4; i++) {
                if (0 != Zd2911Utils.BitCheck(opertationType, i)) {
                    switch (i) {
                        case 0:
                            sb.Append("F ");
                            break;
                        case 1:
                            sb.Append("P ");
                            break;
                        case 2:
                            sb.Append("C ");
                            break;
                        case 3:
                            sb.Append("I ");
                            break;
                    }
                }
            }
            return sb.ToString();
        }

        //public static string GetUserPrivilege(UserPrivilege privilege) {
        //    string userRole = string.Empty;
        //    switch (privilege) {
        //        case UserPrivilege.ROLE_GENERAL_USER:
        //            userRole = "General user";
        //            break;

        //        case UserPrivilege.ROLE_SUPER_USER:
        //            userRole = "Super user";
        //            break;

        //        case UserPrivilege.ROLE_ENROLL_USER:
        //            userRole = "Enroll user";
        //            break;

        //        case UserPrivilege.ROLE_VIEW_USER:
        //            userRole = "View user";
        //            break;

        //        case UserPrivilege.ROLE_CUSTOMER:
        //            userRole = "Customer";
        //            break;
        //    }
        //    return userRole;
        //}
    }
}
