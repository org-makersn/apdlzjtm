using Makersn.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.BizDac
{
    public class PrinterCommentDac
    {
        public IList<PrinterCommentT> GetPrinterCommentList(int printerNo)
        {
            string query = @"SELECT M.PROFILE_PIC, M.NAME , PC.CONTENT, PC.REG_DT, PC.NO, M.NO AS MEMBER_NO, PC.MEMBER_NO_REF
                                FROM PRINTER_COMMENT PC WITH(NOLOCK) INNER JOIN MEMBER M WITH(NOLOCK)
						                                ON PC.MEMBER_NO = M.NO
                                WHERE PRINTER_NO = :printerNo ORDER BY REG_DT DESC";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("printerNo", printerNo);

                IList<PrinterCommentT> list = new List<PrinterCommentT>();
                IList<object[]> result = queryObj.List<object[]>();
                foreach (object[] row in result)
                {
                    PrinterCommentT commnent = new PrinterCommentT();
                    commnent.ProfilePic = (string)row[0];
                    commnent.MemberName = (string)row[1];
                    commnent.Content = (string)row[2];
                    commnent.Regdt = (DateTime)row[3];
                    commnent.No = (long)row[4];
                    commnent.MemberNo = (int)row[5];
                    commnent.MemberNoRef = (int)row[6];
                    list.Add(commnent);
                }

                return list;
            }
        }

        public void DeletePrinterCommentByNo(int no)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    PrinterCommentT comment = session.QueryOver<PrinterCommentT>().Where(a => a.No == no).SingleOrDefault<PrinterCommentT>();
                    if (comment != null)
                    {
                        session.Delete(comment);
                    }
                    transaction.Commit();
                    session.Flush();
                }
            }
        }

        public void UpdatePrinterCommentByNo(PrinterCommentT act)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    PrinterCommentT updAct = session.QueryOver<PrinterCommentT>().Where(a => a.No == act.No).SingleOrDefault<PrinterCommentT>();
                    if (updAct != null)
                    {
                        updAct.Content = act.Content;
                        updAct.UpdId = act.UpdId;
                        updAct.UpdDt = act.UpdDt;
                        session.Update(updAct);
                    }
                    transaction.Commit();
                    session.Flush();
                }
            }
        }

        public void InsertPrinterComment(PrinterCommentT act)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(act);
                    transaction.Commit();
                    session.Flush();
                }
            }
        }
    }
}
