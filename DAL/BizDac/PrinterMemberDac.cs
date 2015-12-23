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

namespace Makersn.BizDac
{
    public class PrinterMemberDac : BizDacHelper
    {

        public PrinterMemberT GetPrinterMemberByNoWithReview(int no)
        {
            string query = @"SELECT NO,PM.MEMBER_NO, ACCOUNT_NO, BANK, HOME_PHONE, CELL_PHONE, NAME, ADDRESS, ADDRESS_DETAIL, POST_MODE, 
                                TAXBILL_YN, PRINTER_PROFILE_MSG, PROFILE_PIC, COVER_PIC, REG_ID, REG_DT, UPD_ID, UPD_DT, 
                                ISNULL((SELECT AVG(SCORE) FROM (select R.SCORE from printer p, review r where P.MEMBER_NO = " + no + @" AND R.PRINTER_NO = P.NO) AS TEMP),0) 'REVIEW_SCORE',
                                (SELECT COUNT(SCORE) FROM (select R.SCORE from printer p, review r where P.MEMBER_NO = " + no + @" AND R.PRINTER_NO = P.NO) AS TEMP) 'REVIEW_CNT'
	                            FROM PRINTER_MEMBER PM
                                WHERE PM.MEMBER_NO = :no ";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("no", no);

                object[] row = queryObj.UniqueResult<object[]>();

                PrinterMemberT printerMember = new PrinterMemberT();
                printerMember.No = (int)row[0];
                printerMember.MemberNo = (int)row[1];
                printerMember.AccountNo = (string)row[2];
                printerMember.Bank = (string)row[3];
                printerMember.HomePhone = (string)row[4];
                printerMember.CellPhone = (string)row[5];
                printerMember.SpotName = (string)row[6];
                printerMember.SpotAddress = (string)row[7];
                printerMember.SpotAddressDetail = (string)row[8];
                printerMember.PostMode = System.Convert.ToInt32(row[9]);


                printerMember.TaxbillFlag = (string)row[10];
                printerMember.PrinterProfileMsg = (string)row[11];
                printerMember.PrinterProfilePic = (string)row[12];
                printerMember.PrinterCoverPic = (string)row[13];
                printerMember.RegId = (string)row[14];
                printerMember.RegDt = (DateTime)row[15];
                printerMember.UpdId = (string)row[16];
                if (row[17] != null)
                    printerMember.UpdDt = (DateTime)row[17];
                printerMember.ReviewScore = (double)row[18];
                printerMember.ReviewCnt = (int)row[19];

                return printerMember;
            }
        }


        public PrinterMemberT GetPrinterMemberByNo(int memberNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<PrinterMemberT>().Where(w => w.MemberNo == memberNo).SingleOrDefault<PrinterMemberT>();
            }
        }

        public PrinterMemberT GetPrinterMemberByUrl(string url)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<PrinterMemberT>().Where(w => w.SpotUrl == url).SingleOrDefault<PrinterMemberT>();
            }
        }

        /// <summary>
        /// if the member has spot return "true" else return "false"
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public bool CheckSpotOpen(int no) {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                PrinterMemberT printerMember =  session.QueryOver<PrinterMemberT>().Where(w => w.MemberNo == no && w.SaveFlag =="Y").SingleOrDefault<PrinterMemberT>();
                if (printerMember != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public int InsertPrinterMember(PrinterMemberT printerMember) {
            if (!CheckMember(printerMember.MemberNo))
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    session.Save(printerMember);
                    session.Flush();
                    return (int)printerMember.No;

                }
            }
            else
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    printerMember.No = GetPrinterMemberByNo(printerMember.MemberNo).No;
                    session.Update(printerMember);
                    session.Flush();
                    return (int)printerMember.No;
                }
            }
        }



        private bool CheckMember(int no) {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                if (session.QueryOver<PrinterMemberT>().Where(w => w.MemberNo == no).List<PrinterMemberT>().Count>=1)
                {
                    return true;
                }
            }
            return false;
        }


        public bool CheckSpotUrl(string url) {
            using(ISession session = NHibernateHelper.OpenSession()){
                if (session.QueryOver<PrinterMemberT>().Where(w => w.SpotUrl == url).List<PrinterMemberT>().Count >= 1)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
