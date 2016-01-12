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
            PhysicalDir = ConfigurationManager.AppSettings["PhysicalDir"] ?? @"D:\storefile";
            UncDir = ConfigurationManager.AppSettings["UncDir"] ?? @"192.168.219.103";
            Slic3rDir = ConfigurationManager.AppSettings["Slic3rDir"] ?? @"D:\slic3r";
        }

        /// <summary>
        /// 파일저장 
        /// slic3r 시 물리적 경로 사용해야 함
        /// </summary>
        public string PhysicalDir { get; private set; }

        /// <summary>
        /// unc 경로 사용될지 안될지 몰라 추가함
        /// </summary>
        public string UncDir { get; private set; }

        /// <summary>
        /// slic3r 폴더 경로
        /// </summary>
        public string Slic3rDir { get; set; }
    }
}