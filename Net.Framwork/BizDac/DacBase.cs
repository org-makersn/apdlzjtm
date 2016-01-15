using Net.Framework;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace Net.Framwork.BizDac
{
    public class DacBase
    {
        private static readonly StoreContext instance = new StoreContext();

        public static StoreContext dbContext { get; set; }


        public DacBase()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityType"></param>
        /// <param name="query"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public virtual System.Collections.Generic.List<T> ExecuteNonQuery<T>(Type entityType, string query, params SqlParameter[] sqlParams)
        {
            System.Collections.Generic.List<T> rows = new System.Collections.Generic.List<T>();


            return rows;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string GetSqlCommand(string _query)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();

            StreamReader _textStreamReader;
            try
            {
                string _nameSpace = _assembly.GetName().Name;
                string _text = string.Format("{0}.SqlCmd.{1}.sql", _nameSpace, _query);
                _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream(_text));
            }
            catch (Exception)
            {
                throw;
            }

            return _textStreamReader.ReadLine();
        }
    }
}
