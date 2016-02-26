using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Design.Web.Front.Helper
{
    public static class UrlHelperExtensions
    {
        private static readonly string version = typeof(UrlHelperExtensions).Assembly.GetName().Version.ToString();

        public static string VersionedContent(this UrlHelper urlHelper, string resUrl)
        {
            return urlHelper.Content(resUrl) + "?v=" + version;
        }
    }
}