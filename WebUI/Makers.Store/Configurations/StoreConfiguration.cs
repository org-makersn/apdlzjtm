using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Makers.Store.Configurations
{
    public class StoreConfiguration
    {
        private static readonly StoreConfiguration instance = new StoreConfiguration();

        public static StoreConfiguration Instance { get { return instance; } }

        private StoreConfiguration()
        {
            PhysicalDir = ConfigurationManager.AppSettings["PhysicalDir"] ?? @"D:\uploadfiles";
            Slic3rDir = ConfigurationManager.AppSettings["Slic3rDir"] ?? @"D:\slic3r";
            ServiceUrl = ConfigurationManager.AppSettings["ServiceUrl"] ?? @"http://openapi.epost.go.kr/postal/retrieveNewAdressAreaCdSearchAllService/retrieveNewAdressAreaCdSearchAllService/getNewAddressListAreaCdSearchAll";
            ServiceKey = ConfigurationManager.AppSettings["ServiceKey"] ?? @"H7xR5DleYEuhPTFBxnqXc%2B5rkjgzsHQiUUzQCvhp8JVZ2JhMTtRrGq%2FfgqcJ%2B%2BKQ1Rt2q0PyT5%2BnAqMKQ9RKYA%3D%3D";
        }

        /// <summary>
        /// 파일저장 
        /// slic3r 시 물리적 경로 사용해야 함
        /// </summary>
        public string PhysicalDir { get; private set; }

        /// <summary>
        /// slic3r 폴더 경로
        /// </summary>
        public string Slic3rDir { get; set; }

        /// <summary>
        /// 우편번호 api 호출 경로
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// 우편번호 api 호출 key
        /// </summary>
        public string ServiceKey { get; set; }
    }
}