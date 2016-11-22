using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.Unility
{
    public class LocalMachineInfomationProvider
    {
        public static string GetHostname()
        {
            return Environment.MachineName;
        }

        public static string GetLocalIp()
        {
            string hostname = Dns.GetHostName();//得到本机名
            //IPHostEntry localhost = Dns.GetHostByName(hostname);//方法已过期，只得到IPv4的地址
            IPHostEntry localhost = Dns.GetHostEntry(hostname); //新方法
            var localaddr = string.Join(",", localhost.AddressList.Select(x => x.ToString()));
            return localaddr;  
        }

        ///<summary>       
        /// 获取本机MAC地址        
        /// </summary>       
        /// <returns>返回当前机器上的所有MAC地址</returns>        
        public static string GetMac()
        {
            string macAddress = "";
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in nics)
                {
                    if (!adapter.GetPhysicalAddress().ToString().Equals(""))
                    {
                        macAddress = adapter.GetPhysicalAddress().ToString();
                        for (int i = 1; i < 6; i++)
                        {
                            macAddress = macAddress.Insert(3 * i - 1, ":");
                        }
                        break;
                    }
                }

            }
            catch
            {
            }

            return macAddress.Replace(":", "-");
        }
    }
}
