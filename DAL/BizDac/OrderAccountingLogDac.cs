using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using Makersn.Models;
using NHibernate;
using Makersn.Util;
using NHibernate.Criterion;
using System.Net;
public class OrderAccountingLogDac
{
    public void insertOrderAccountingLog(OrderAccountingLogT oal,ISession session){
            session.Save(oal);
    }
}

