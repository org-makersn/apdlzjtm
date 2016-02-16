using System.Configuration;

namespace Net.Common.Configurations
{
    public class ApplicationConfiguration
    {
        private static readonly ApplicationConfiguration instance = new ApplicationConfiguration();

        public static ApplicationConfiguration Instance { get { return instance; } }

        /// <summary>
        /// 공용 - 회원관련
        /// </summary>
        private ApplicationConfiguration()
        {
            FileSeviceDomain = ConfigurationManager.AppSettings["SeviceDomain"] ?? "local.file.makersn.com";
            FileServerHost = string.Format("http://{0}", FileSeviceDomain);
            FileServerUncPath = ConfigurationManager.AppSettings["FileServerUncPath"] ?? @"\\192.168.219.120\fileupload";

            ProfileImg = ConfigurationManager.AppSettings["ProfileImg"] ?? "profile/thumb";
            ProfileCover = ConfigurationManager.AppSettings["ProfileCover"] ?? "profile/cover";
        }

        /// <summary>
        /// 
        /// </summary>
        //public string ServiceMode { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileSeviceDomain { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileServerHost { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileServerUncPath { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProfileImg { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProfileCover { get; private set; }


        #region 디자인 Configurations
        /// <summary>
        /// 디자인
        /// </summary>
        public class DesignConfiguration
        {
            private static readonly DesignConfiguration instance = new DesignConfiguration();

            public static DesignConfiguration Instance { get { return instance; } }

            private DesignConfiguration()
            {
                //design
                DesignImgFile = ConfigurationManager.AppSettings["DesignImgFile"] ?? "design/img-files";
                Design3DFile = ConfigurationManager.AppSettings["Design3DFile"] ?? "design/3d-files";
                DesignJsonFile = ConfigurationManager.AppSettings["DesignJsonFile"] ?? "design/json-files";
                BannerImg = ConfigurationManager.AppSettings["BannerImg"] ?? "design/banner";
                MsgImgThumb = ConfigurationManager.AppSettings["MsgImgThumb"] ?? "design/msg-file/thumb";
                MsgImgOrigin = ConfigurationManager.AppSettings["MsgImgOrigin"] ?? "design/msg-file/backup";
            }

            /// <summary>
            /// 
            /// </summary>
            public string DesignImgFile { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public string Design3DFile { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public string DesignJsonFile { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public string BannerImg { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public string MsgImgThumb { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public string MsgImgOrigin { get; private set; }
        }
        #endregion

        #region 프린팅 Configurations
        /// <summary>
        /// 프린팅
        /// </summary>
        public class PrintingConfiguration
        {
            private static readonly PrintingConfiguration instance = new PrintingConfiguration();

            public static PrintingConfiguration Instance { get { return instance; } }

            private PrintingConfiguration()
            {
                //Printing
            }

            /// <summary>
            /// 
            /// </summary>
            public string Attrbute { get; private set; }
        }
        #endregion

        #region 스토어 Configurations
        /// <summary>
        /// 스토어
        /// </summary>
        public class StoreConfiguration
        {
            private static readonly StoreConfiguration instance = new StoreConfiguration();

            public static StoreConfiguration Instance { get { return instance; } }

            private StoreConfiguration()
            {
                //Store
                StoreImgFile = ConfigurationManager.AppSettings["StoreImgFile"] ?? "store/img-files";
                Store3DFile = ConfigurationManager.AppSettings["Store3DFile"] ?? "store/3d-files";
                StoreJsonFile = ConfigurationManager.AppSettings["StoreJsonFile"] ?? "store/json-files";
                StoreBanner = ConfigurationManager.AppSettings["StoreBanner"] ?? "store/banner";
                MsgImgThumb = ConfigurationManager.AppSettings["MsgImgThumb"] ?? "store/msg-file/thumb";
                MsgImgOrigin = ConfigurationManager.AppSettings["MsgImgOrigin"] ?? "store/msg-file/backup";

                ServiceUrl = ConfigurationManager.AppSettings["ServiceUrl"] ?? @"http://openapi.epost.go.kr/postal/retrieveNewAdressAreaCdSearchAllService/retrieveNewAdressAreaCdSearchAllService/getNewAddressListAreaCdSearchAll";
                ServiceKey = ConfigurationManager.AppSettings["ServiceKey"] ?? @"H7xR5DleYEuhPTFBxnqXc%2B5rkjgzsHQiUUzQCvhp8JVZ2JhMTtRrGq%2FfgqcJ%2B%2BKQ1Rt2q0PyT5%2BnAqMKQ9RKYA%3D%3D";
                Mid = ConfigurationManager.AppSettings["Mid"] ?? @"INIpayTest";
                MidPassword = ConfigurationManager.AppSettings["MidPassword"] ?? @"1111";
                Currency = ConfigurationManager.AppSettings["Currency"] ?? @"WON";
                PgId = ConfigurationManager.AppSettings["PgId"] ?? @"IniTechPG_";
                PgIp = ConfigurationManager.AppSettings["PgIp"] ?? @"203.238.37";

                PhysicalDir = ConfigurationManager.AppSettings["PhysicalDir"] ?? @"D:\uploadfile";
                Slic3rDir = ConfigurationManager.AppSettings["Slic3rDir"] ?? @"D:\slic3r";
            }

            /// <summary>
            /// 
            /// </summary>
            public string StoreImgFile { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public string Store3DFile { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public string StoreJsonFile { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public string StoreBanner { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public string MsgImgThumb { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            public string MsgImgOrigin { get; private set; }

            /// <summary>
            /// 우편번호 api 호출 경로
            /// </summary>
            public string ServiceUrl { get; set; }

            /// <summary>
            /// 우편번호 api 호출 key
            /// </summary>
            public string ServiceKey { get; set; }

            /// <summary>
            /// Kg이니시스 상점 아이디
            /// </summary>
            public string Mid { get; set; }

            /// <summary>
            /// Kg이니시스 상점 아이디 비밀번호
            /// </summary>
            public string MidPassword { get; set; }

            /// <summary>
            /// Kg이니시스 통화단위
            /// </summary>
            public string Currency { get; set; }

            /// <summary>
            /// PG 아이디
            /// </summary>
            public string PgId { get; set; }

            /// <summary>
            /// PG 아이피
            /// </summary>
            public string PgIp { get; set; }

            /// <summary>
            /// 파일저장 
            /// slic3r 시 물리적 경로 사용해야 함
            /// </summary>
            public string PhysicalDir { get; private set; }

            /// <summary>
            /// slic3r 폴더 경로
            /// </summary>
            public string Slic3rDir { get; set; }
        }
        #endregion
    }
}
