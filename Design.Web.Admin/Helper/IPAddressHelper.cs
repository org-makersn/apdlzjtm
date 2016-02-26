using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Design.Web.Admin.Helper
{
    public static class IPAddressHelper
    {
        #region Get IPAddress
        public static string GetIP(this Controller ctrl)
        {
            string ip;
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
    }
}
