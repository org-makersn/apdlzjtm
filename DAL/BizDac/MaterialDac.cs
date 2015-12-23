using Makersn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Makersn.BizDac
{
    public class MaterialDac
    {
        public IList<MaterialT> getMaterialList()
        {
            using(ISession session = NHibernateHelper.OpenSession()){
               return  session.QueryOver<MaterialT>().List<MaterialT>();
            }
        }
        public string getMaterialNameByNo(int no) {

            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<MaterialT>().Where(w => w.No == no).SingleOrDefault<MaterialT>().Name;
            }
        }
    }
}
