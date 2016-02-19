using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Makers.Bus.Configurations
{
    public class BusConfigurations
    {
        private static readonly BusConfigurations instance = new BusConfigurations();
        public static BusConfigurations Instance { get { return instance; } }

        private BusConfigurations()
        {
            PhysicalDir = ConfigurationManager.AppSettings["PhysicalDir"] ?? @"D:\uploadfiles";

        }

        /// <summary>
        /// 파일저장 
        /// slic3r 시 물리적 경로 사용해야 함
        /// </summary>
        public string PhysicalDir { get; private set; }
    }
}