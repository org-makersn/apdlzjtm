using Makersn.Models;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.BizDac
{
    public class ReviewDac
    {
        public bool addReview(ReviewT data)
        {
            bool result = false;

            using (ISession session = NHibernateHelper.OpenSession())
            {
                ReviewT review = session.QueryOver<ReviewT>().Where(w => w.MemberNo == data.MemberNo && w.OrderNo == data.OrderNo).Take(1).SingleOrDefault<ReviewT>();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (review == null)
                    {
                        session.Save(data);
                    }
                    else
                    {
                        review.Comment = data.Comment;
                        review.Score = data.Score;
                        review.UpdId = data.RegId;
                        review.UpdDt = data.UpdDt;
                        session.Update(review);
                    }
                    transaction.Commit();
                    session.Flush();
                    result = true;
                }
                
            }
            return result;
        }

        public ReviewT GetReviewByOrderNo(int orderNo, int memberNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<ReviewT>().Where(w => w.OrderNo == orderNo && w.MemberNo == memberNo).Take(1).SingleOrDefault<ReviewT>();
            }
        }
        public IList<ReviewT> GetReviewByMemberNo(int memberNo)
        {
            IList<ReviewT> list = new List<ReviewT>();
            MemberDac memberDac = new MemberDac();
            PrinterDac printerDac = new PrinterDac();
            string query = @"select r.no, r.ORDER_NO, r.MEMBER_NO,r.PRINTER_NO,r.SCORE,r.COMMENT,
                            r.REG_ID,r.REG_DT,r.UPD_ID,r.UPD_DT,p.BRAND,p.MODEL
                            from review r, printer p
                            where r.PRINTER_NO = p.no
                            and p.MEMBER_NO = :memberNo ";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("memberNo", memberNo);
                
               IList<object[]> result = queryObj.List<object[]>();
                
                foreach(object[] row in result){
                    ReviewT review = new ReviewT();
                    review.No = Convert.ToInt32(row[0]);
                    review.OrderNo = (int)row[1];
                    review.MemberNo = (int)row[2];
                    review.PrinterNo = (int)row[3];
                    review.Score = Convert.ToInt32(row[4]);
                    review.Comment = (string)row[5];
                    review.RegId = (string)row[6];
                    review.RegDt = (DateTime)row[7];
                    review.UpdId = (string)row[8];
                    if (row[9] != null)
                    {
                        review.UpdDt = (DateTime)row[9];
                    }
                    review.PrinterName = (string)row[10] + (string)row[11];
                    list.Add(review);
                }

                foreach (ReviewT review in list)
                {
                    PrinterT printer = printerDac.GetPrinterByNo(review.PrinterNo);
                    if (printer != null)
                    {
                        review.PrinterName = printer.Brand + " " + printer.Model;
                    }
                    review.MemberName = memberDac.GetMemberProfile(review.MemberNo).Name;
                }
                return list;
            }
            
        }

        public IList<ReviewT> GetReviewListByPrinterNo(int printerNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<ReviewT>().Where(w => w.PrinterNo == printerNo).List<ReviewT>();
            }
            
        }
    }
}
