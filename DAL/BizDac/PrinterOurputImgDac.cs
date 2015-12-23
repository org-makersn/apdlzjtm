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
    public class PrinterOurputImgDac : BizDacHelper
    {
        public void InsertImgByString(List<PrinterOutputImageT> imgList) {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                foreach(PrinterOutputImageT img in imgList ){
                    session.Save(img);
                    session.Flush();
                }
            }
        }
        public IList<PrinterOutputImageT> GetImglistByOrderNo(long orderNo) {
            using(ISession session = NHibernateHelper.OpenSession()){
                return session.QueryOver<PrinterOutputImageT>().Where(w => w.OrderNo == orderNo).List<PrinterOutputImageT>();
            }
        }

        public bool DeleteOutputImageByList(IList<PrinterOutputImageT> imgList)
        {
            bool result = false;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (PrinterOutputImageT img in imgList)
                    {
                        session.Delete(img);
                    }
                    transaction.Commit();
                    session.Flush();
                    result = true;
                }
            }
            return result;
        }
    }
}
