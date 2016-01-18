using System;
using System.IO;
using System.Reflection;

namespace Net.Framwork.Helper
{
    public class DacHelper
    {
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
