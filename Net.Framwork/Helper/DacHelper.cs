using System;
using System.IO;
using System.Reflection;

namespace Net.Framwork.Helper
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
            string result = "";
            Assembly _assembly = Assembly.GetExecutingAssembly();

            try
            {
                string _nameSpace = _assembly.GetName().Name;
                string _text = string.Format("{0}.SqlCmd.{1}.sql", _nameSpace, _query);

                using (StreamReader reader = new StreamReader(_assembly.GetManifestResourceStream(_text)))
                {
                    string line = reader.ReadLine();

                    while (line != null)
                    {
                        result += line + " ";
                        line = reader.ReadLine();
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

    }
}
