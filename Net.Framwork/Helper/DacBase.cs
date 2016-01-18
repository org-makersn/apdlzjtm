using Net.Framework;
using Net.SqlTools;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace Net.Framwork.Helper
{
    public class DacBase
    {
        public IDbHelper dbHelper;

        public DacBase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["StoreContext"].ConnectionString;
            this.dbHelper = new SqlDbHelper(connectionString);
        }
    }
}
