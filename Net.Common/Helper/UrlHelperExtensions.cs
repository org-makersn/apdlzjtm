using Net.Common.Configurations;
using System.Web.Mvc;

namespace Net.Common.Helper
{
    public static class UrlHelperExtensions
    {
        private static ApplicationConfiguration instance = ApplicationConfiguration.Instance;
        private static ApplicationConfiguration.DesignConfiguration designInstance = ApplicationConfiguration.DesignConfiguration.Instance;
        private static ApplicationConfiguration.StoreConfiguration storeInstance = ApplicationConfiguration.StoreConfiguration.Instance;

        private static readonly string version = typeof(UrlHelperExtensions).Assembly.GetName().Version.ToString();

        #region Common Url Helper
        /// <summary>
        /// version
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string VersionedContent(this UrlHelper urlHelper, string data)
        {
            return urlHelper.Content(data) + "?v=" + version;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ProfileImg(this UrlHelper urlHelper, string data)
        {
            string fullPath = !string.IsNullOrEmpty(data) ? string.Format("{0}/{1}/{2}", instance.FileServerHost, instance.ProfileImg, data) : "/Content/image/default.png";
            return urlHelper.Content(fullPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ProfileCover(this UrlHelper urlHelper, string data)
        {
            string fullPath = string.Format("{0}/{1}/{2}", instance.FileServerHost, instance.ProfileCover, data);
            return urlHelper.Content(fullPath);
        }
        #endregion

        #region Design Url Helper
        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DesignImgFile(this UrlHelper urlHelper, string data)
        {
            string fullPath = string.Format("{0}/{1}/{2}", instance.FileServerHost, designInstance.DesignImgFile, data);
            return urlHelper.Content(fullPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Design3DFile(this UrlHelper urlHelper, string data)
        {
            string fullPath = string.Format("{0}/{1}/{2}", instance.FileServerHost, designInstance.Design3DFile, data);
            return urlHelper.Content(fullPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DesignJsonFile(this UrlHelper urlHelper, string data)
        {
            string fullPath = string.Format("{0}/{1}/{2}", instance.FileServerHost, designInstance.DesignJsonFile, data);
            return urlHelper.Content(fullPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DesignBannerImg(this UrlHelper urlHelper, string data)
        {
            string fullPath = string.Format("{0}/{1}/{2}", instance.FileServerHost, designInstance.BannerImg, data);
            return urlHelper.Content(fullPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DesignMsgImgThumb(this UrlHelper urlHelper, string data)
        {
            string fullPath = string.Format("{0}/{1}/{2}", instance.FileServerHost, designInstance.MsgImgThumb, data);
            return urlHelper.Content(fullPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DesignMsgImgOrigin(this UrlHelper urlHelper, string data)
        {
            string fullPath = string.Format("{0}/{1}/{2}", instance.FileServerHost, designInstance.MsgImgOrigin, data);
            return urlHelper.Content(fullPath);
        }
        #endregion

        #region Store Url Helper
        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string StoreImgFile(this UrlHelper urlHelper, string data)
        {
            string fullPath = string.Format("{0}/{1}/{2}", instance.FileServerHost, storeInstance.StoreImgFile, data);
            return urlHelper.Content(fullPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Store3DFile(this UrlHelper urlHelper, string data)
        {
            string fullPath = string.Format("{0}/{1}/{2}", instance.FileServerHost, storeInstance.Store3DFile, data);
            return urlHelper.Content(fullPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string StoreJsonFile(this UrlHelper urlHelper, string data)
        {
            string fullPath = string.Format("{0}/{1}/{2}", instance.FileServerHost, storeInstance.StoreJsonFile, data);
            return urlHelper.Content(fullPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string StoreBannerImg(this UrlHelper urlHelper, string data)
        {
            string fullPath = string.Format("{0}/{1}/{2}", instance.FileServerHost, storeInstance.StoreBanner, data);
            return urlHelper.Content(fullPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string StoreMsgImgThumb(this UrlHelper urlHelper, string data)
        {
            string fullPath = string.Format("{0}/{1}/{2}", instance.FileServerHost, storeInstance.MsgImgThumb, data);
            return urlHelper.Content(fullPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string StoreMsgImgOrigin(this UrlHelper urlHelper, string data)
        {
            string fullPath = string.Format("{0}/{1}/{2}", instance.FileServerHost, storeInstance.MsgImgOrigin, data);
            return urlHelper.Content(fullPath);
        }
        #endregion
    }
}