using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Makersn.Models;

namespace Makersn.BizDac
{
    public class BizDacHelper
    {
        /// <summary>
        /// 
        /// </summary>
        protected ISession Session
        {
            get { return NHibernateHelper.OpenSession(); }
        }

        protected void CloseSession()
        {
            NHibernateHelper.CloseSession();
        }
    }
}
