using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Makersn.Models;
using NHibernate.Criterion.Lambda;
using NHibernate.Criterion;

namespace Makersn.BizDac
{
    public class CodeDac
    {
        /// <summary>
        /// get code list by codegbn
        /// </summary>
        /// <param name="codeGbn"></param>
        /// <returns></returns>
        public IList<CodeT> GetCodeLstByGbn(string codeGbn)
        {
            if(String.IsNullOrEmpty(codeGbn)) throw new ArgumentNullException("There is no variable codeGbn");

            //string query = string.Format(@"select * from CODE with(nolock) where CODE_GBN = '{0}'", codeGbn);

            using (ISession session = NHibernateHelper.OpenSession())
            {
                //var results = session.CreateSQLQuery(query).List<object[]>();

                IList<CodeT> list = session.QueryOver<CodeT>().Where(c => c.CodeGbn == codeGbn).List();

                session.Flush();
                return list;
            }
        }
    }
}
