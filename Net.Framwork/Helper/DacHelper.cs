using System;
using System.IO;
using System.Reflection;

namespace Net.Framework.Helper
{
    public static class DacHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string GetSqlCommand(string _query)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();

            string strBuff = "";
            string strBuffTemp = "";
            try
            {
                string _nameSpace = _assembly.GetName().Name;
                string _text = string.Format("{0}.SqlCmd.{1}.sql", _nameSpace, _query);

                using (StreamReader sr = new StreamReader(_assembly.GetManifestResourceStream(_text)))
                {
                    while ((strBuffTemp = sr.ReadLine()) != null)
                    {
                        strBuff += strBuffTemp + " ";
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return strBuff;
        }

    }
}
