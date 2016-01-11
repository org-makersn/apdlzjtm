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


        }

        /// <summary>
        /// 
        /// </summary>
        public string Attr1 { get; private set; }


        public class EtcConfiguration
        {
            private static readonly EtcConfiguration instance = new EtcConfiguration();

            public static EtcConfiguration Instance { get { return instance; } }

            private EtcConfiguration()
            {

            }
        }
    }
}
