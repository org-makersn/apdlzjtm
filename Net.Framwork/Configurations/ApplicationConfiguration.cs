using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.Configurations
{
    public class ApplicationConfiguration
    {
        private static readonly ApplicationConfiguration instance = new ApplicationConfiguration();

        public static ApplicationConfiguration Instance { get { return instance; } }

        private ApplicationConfiguration()
        {
            _Server_IP = ConfigurationManager.AppSettings["DBServerIP"] ?? "storedb_dev";
            _UserNm = ConfigurationManager.AppSettings["DBUser"] ?? "0000";
            _Password = ConfigurationManager.AppSettings["DBPwd"] ?? "0000";
            _Database = ConfigurationManager.AppSettings["Database"] ?? "0000";
        }

        /// <summary>
        /// 
        /// </summary>
        public string _Server_IP { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string _UserNm { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string _Password { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string _Database { get; private set; }


        public class SiteDomainConfiguration
        {
            private static readonly SiteDomainConfiguration instance = new SiteDomainConfiguration();

            public static SiteDomainConfiguration Instance { get { return instance; } }

            private SiteDomainConfiguration()
            {
                ArticleImgUrl = ConfigurationManager.AppSettings["articleimgurl"] ?? "http://local.dev.makersn.com/fileupload/article/article_img/";
                AdminImgUrl = ConfigurationManager.AppSettings["adminimgurl"] ?? "http://local.dev.makersn.com/fileupload/banner/fullsize/";
                Article3DUrl = ConfigurationManager.AppSettings["Article3DUrl"] ?? "http://local.dev.makersn.com/fileupload/article/article_3d/";
                Article3DJsUrl = ConfigurationManager.AppSettings["Article3DJsUrl"] ?? "http://local.dev.makersn.com/fileupload/article/article_js/";
                ProfileImgUrl = ConfigurationManager.AppSettings["profileimgurl"] ?? "http://local.dev.makersn.com/fileupload/profile/thumb/";
                MsgImgThumb = ConfigurationManager.AppSettings["msgImgThumb"] ?? "http://local.dev.makersn.com/fileupload/msg_file/backup/";
                MsgImgOri = ConfigurationManager.AppSettings["msgImgOri"] ?? "http://local.dev.makersn.com/fileupload/msg_file/backup/";

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
}
