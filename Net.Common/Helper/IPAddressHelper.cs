using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Net.Common.Helper
{
    public static class IPAddressHelper
    {
        #region Get IPAddress
        public static string GetIP(this Controller ctrl)
        {
            string ip = string.Empty; ;
            if (ctrl.HttpContext.Request.ServerVariables["HTTP_VIA"] != null)
            {
                ip = ctrl.HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                ip = ctrl.HttpContext.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return ip;
        }
        #endregion

        #region
        public static string GetClientIP()
        {
            string GetIP = string.Empty;
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    GetIP = ip.ToString();
                }
            }
            return GetIP;
        }
        #endregion
    }
}