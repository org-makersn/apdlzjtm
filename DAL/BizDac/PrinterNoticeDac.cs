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
using NHibernate.Transform;

namespace Makersn.BizDac
{
    public class PrinterNoticeDac : BizDacHelper
    {
        public void InsertNotice(PrinterNoticeT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                int chk = 0;
                chk = session.QueryOver<PrinterNoticeT>().Where(w => w.OrderNo == data.OrderNo && w.Type == data.Type).RowCount();
                if (chk == 0)
                {
                    session.Save(data);
                }
                session.Flush();
            }
        }

        public IList<PrinterNoticeT> GetNoticeByOrderNoAndType(int orderNo, string type)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<PrinterNoticeT>().Where(w => w.OrderNo == orderNo && w.Type == type).List<PrinterNoticeT>();
            }
        }
        public void RemoveWithOrderNo(long orderNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                PrinterNoticeT notice = session.QueryOver<PrinterNoticeT>().Where(w => w.OrderNo == orderNo).SingleOrDefault<PrinterNoticeT>();
                session.Delete(notice);
            }

        }
    }
}
