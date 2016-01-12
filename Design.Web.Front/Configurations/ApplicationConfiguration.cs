using System.Configuration;

namespace Design.Web.Front.Configurations
{
    public class ApplicationConfiguration
    {
        private static readonly ApplicationConfiguration instance = new ApplicationConfiguration();

        public static ApplicationConfiguration Instance { get { return instance; } }

        private ApplicationConfiguration()
        {
            ArticleImgUrl = ConfigurationManager.AppSettings["ArticleImgUrl"] ?? "http://local.dev.makersn.com/fileupload/article/article_img/";
            AdminImgUrl = ConfigurationManager.AppSettings["AdminImgUrl"] ?? "http://local.dev.makersn.com/fileupload/banner/fullsize/";
            Article3DUrl = ConfigurationManager.AppSettings["Article3DUrl"] ?? "http://local.dev.makersn.com/fileupload/article/article_3d/";
            Article3DJsUrl = ConfigurationManager.AppSettings["Article3DJsUrl"] ?? "http://local.dev.makersn.com/fileupload/article/article_js/";
            ProfileImgUrl = ConfigurationManager.AppSettings["ProfileImgUrl"] ?? "http://local.dev.makersn.com/fileupload/profile/thumb/";
            MsgImgThumb = ConfigurationManager.AppSettings["MsgImgThumb"] ?? "http://local.dev.makersn.com/fileupload/msg_file/backup/";
            MsgImgOri = ConfigurationManager.AppSettings["MsgImgOri"] ?? "http://local.dev.makersn.com/fileupload/msg_file/backup/";

            PrinterImgUrl = ConfigurationManager.AppSettings["PrinterImgUrl"] ?? "http://local.dev.makersn.com/fileupload/printer/printer_img/";

            CurrentDomain = ConfigurationManager.AppSettings["CurrentDomain"] ?? "http://beta.makersn.com";
            TargetDomain = ConfigurationManager.AppSettings["TargetDomain"] ?? "http://betaen.makersn.com";
        }

        /// <summary>
        /// 
        /// </summary>
        public string ArticleImgUrl { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string AdminImgUrl { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Article3DUrl { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Article3DJsUrl { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProfileImgUrl { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string MsgImgThumb { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string MsgImgOri { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string PrinterImgUrl { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrentDomain { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string TargetDomain { get; private set; }

    }
}