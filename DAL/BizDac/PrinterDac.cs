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
    public class PrinterDac : BizDacHelper
    {
        public IList<PrinterStateT> GetPrintingListForAdmin(string dateGubun = "", string start = "", string end = "", string txtGubun = "", string text = "", int state = 0)
        {
            string query = @"SELECT REQ.NO, REQ.ORDER_NO, POI.IMG_RENAME AS IMG_NAME, REQ.REG_DT, M.NAME AS ORDER_MEMBER_NAME, PM.NAME AS SPOT_NAME, REQ.ORDER_STATUS, REQ.PAY_TYPE, OD.UNIT_PRICE
                                    FROM ORDER_REQ REQ  WITH(NOLOCK) LEFT JOIN PRINTER_OUTPUT_IMG POI WITH(NOLOCK)
				                                    ON REQ.NO = POI.ORDER_NO
				                                    INNER JOIN MEMBER M WITH(NOLOCK)
				                                    ON REQ.MEMBER_NO = M.NO
				                                    INNER JOIN PRINTER_MEMBER PM WITH(NOLOCK)
				                                    ON REQ.PRINTER_MEMBER_NO = PM.MEMBER_NO
				                                    INNER JOIN ORDER_DETAIL OD WITH(NOLOCK)
				                                    ON REQ.NO = OD.ORDER_NO
                                                    WHERE 1=1 ";

            if (start != "" && end != "")
            {
                query += " AND REQ.REG_DT >= :start AND  REQ.REG_DT < :end ";
            }

            if (state != 0)
            {
                query += " AND REQ.ORDER_STATUS = :state ";
            }

            if (text != "")
            {
                query += " AND (M.NAME LIKE :text OR PM.NAME LIKE :text) " ;
            }

            query += " ORDER BY REQ.NO DESC";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(PrinterStateT));
                if(query.Contains(":text")){
                    queryObj.SetParameter("text","%"+text+"%");
                }
                if(query.Contains(":state")){
                    queryObj.SetParameter("state",state);
                }
                if(query.Contains(":start") &&query.Contains(":end")  ){
                    queryObj.SetParameter("start",start);
                    queryObj.SetParameter("end", end + " 23:59:59 ");
                }
                return queryObj.List<PrinterStateT>();
            }
        }


        #region 검색결과
        public IList<PrinterT> GetSearchList(string text, string recommendYn)
        {
            string query = @"SELECT P.NO, ISNULL(PF.NAME, PF.RENAME) AS MAIN_IMAGE, P.BRAND, P.MODEL , PF.PATH, PM.NAME AS PRINTER_MEMBER_NAME,M.NAME AS MEMBER_NAME, P.LOC_X, P.LOC_Y, P.QUALITY, P.STATUS,P.TEST_COMPLETE_FLAG, P.DEL_FLAG, 
                                    P.DEL_DATE, P.REG_ID, P.REG_DT, P.UPD_ID, P.UPD_DT, P.RECOMMEND_YN, P.RECOMMEND_DT, P.RECOMMEND_VISIBILITY, P.RECOMMEND_PRIORITY 
                                    
                                    
                            FROM PRINTER P WITH(NOLOCK) INNER JOIN PRINTER_FILE AS PF WITH(NOLOCK)
					                            ON P.MAIN_IMG = PF.NO
					                            LEFT JOIN PRINTER_MEMBER AS PM WITH(NOLOCK)
					                            ON P.PRT_MEMBER_NO = PM.NO
                                                LEFT JOIN MEMBER AS M WITH(NOLOCK)
                                                ON P.PRT_MEMBER_NO = M.NO
                                                WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL) ";

            if (text != "") { query += "AND (PM.NAME LIKE :text  OR P.BRAND LIKE :text OR P.MODEL LIKE :text)"; }

            if (recommendYn != "") { query += " AND P.RECOMMEND_YN = :recommendYn "; }

            using (ISession session = NHibernateHelper.OpenSession())
            {

                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(PrinterStateT));
                if (query.Contains(":text"))
                {
                    queryObj.SetParameter("text", "%"+text+"%");
                }
                if (query.Contains(":recommendYn"))
                {
                    queryObj.SetParameter("recommendYn", recommendYn);
                }
                IList<object[]> results = queryObj.List<object[]>();
                session.Flush();

                IList<PrinterT> list = new List<PrinterT>();
                
                foreach (object[] row in results)
                {
                    PrinterT printer = new PrinterT();
                    printer.No = (int)row[0];
                    printer.ImageName = (string)row[1];
                    printer.Brand = (string)row[2];
                    printer.Model = (string)row[3];
                    printer.Path = (string)row[4] + printer.ImageName;
                    printer.PrinterMemberName = (string)row[6];
                    printer.MemberName = (string)row[6];
                    printer.LocX = (float)System.Convert.ToDouble(row[7]);
                    printer.LocY = (float)System.Convert.ToDouble(row[8]);
                    printer.Quality = (string)row[9];
                    printer.Status = (string)row[10];
                    printer.TestCompleteFlag = (string)row[11];
                    printer.DelFlag = (string)row[12];
                    printer.DelDate = (DateTime)row[13];
                    printer.RegId = (string)row[14];
                    printer.RegDt = (DateTime)row[15];
                    printer.UpdId = (string)row[16];
                    printer.UpdDt = (DateTime)row[17];
                    printer.RecommendYn = (string)row[18];
                    if (row[19] != null)
                    {
                        printer.RecommendDt = (DateTime)row[19];
                    }
                    printer.RecommendVisibility = (string)row[20];
                    printer.RecommendPriority = (int)row[21];
                    list.Add(printer);
                }

                return list;
            }
        }
        #endregion


        #region 추천 사용여부 업데이트
        public void UpdateRecommendVisibility(int no, string visibility)
        {
            // if visibility is null just change printer.RecommendVisibility's value to the other, 
            // else change printer.RecommendVisibility to visibility 
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    PrinterT printer = session.QueryOver<PrinterT>().Where(w => w.No == no).SingleOrDefault<PrinterT>();

                    if (visibility == null)
                    {
                        if (printer.RecommendVisibility == "Y")
                            printer.RecommendVisibility = "N";
                        else
                            printer.RecommendVisibility = "Y";
                    }
                    else
                    {
                        printer.RecommendVisibility = visibility;
                    }
                    session.Update(printer);
                    transaction.Commit();
                    session.Flush();
                }
            }
        }
        #endregion

        #region 순위 업데이트
        public void UpdatePriority(int no, int priority)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    PrinterT printer = session.QueryOver<PrinterT>().Where(w => w.No == no).SingleOrDefault<PrinterT>();
                    printer.RecommendPriority = priority;
                    session.Update(printer);
                    transaction.Commit();
                    session.Flush();
                }
            }
        }
        #endregion

        public PrinterT GetPrinterByNo(int no)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                PrinterT printer = session.QueryOver<PrinterT>().Where(a => a.No == no).SingleOrDefault<PrinterT>();
                PrinterFileDac printerFileDac = new PrinterFileDac();
                printer.ImageName = printerFileDac.GetMainImg(printer.MainImg);
                return printer;
            }
        }

        //public int UpdateVisibility(string no, string visibility)
        //{
        //    string query = "UPDATE PRINTER SET RECOMMED_VISIBILITY = '" + visibility + "' WHERE ";

        //    if (no.LastIndexOf(',') > 0)
        //    {
        //        no = no.Substring(0, no.LastIndexOf(','));
        //    }
        //    string[] noList = no.Split(',');
        //    for (int i = 0; i < noList.Length; i++)
        //    {
        //        query += " NO = " + noList[i] + "";
        //        if (i < noList.Length - 1)
        //        {
        //            query += " OR ";
        //        }
        //    }

        //    using (ISession session = NHibernateHelper.OpenSession())
        //    {
        //        using (ITransaction transaction = session.BeginTransaction())
        //        {
        //            session.CreateSQLQuery(query).ExecuteUpdate();

        //            transaction.Commit();

        //            session.Flush();
        //            return 1;
        //        }
        //    }
        //}

        public int UpdateRecommend(string no, string recommend)
        {
            string query = "";
            if (recommend == "Y") { query = "UPDATE PRINTER SET RECOMMEND_DT = GETDATE(), RECOMMEND_YN = :recommend  WHERE "; }
            else { query = "UPDATE PRINTER SET RECOMMEND_DT = null, RECOMMEND_YN = :recommend WHERE "; }

            if (no.LastIndexOf(',') > 0)
            {
                no = no.Substring(0, no.LastIndexOf(','));
            }
            string[] noList = no.Split(',');
            for (int i = 0; i < noList.Length; i++)
            {
                query += " NO = ? ";
                if (i < noList.Length - 1)
                {
                    query += " OR ";
                }
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    IQuery queryObj = session.CreateSQLQuery(query); 

                    for (int i = 0; i < noList.Length; i++)
                    {
                        queryObj.SetParameter(i, noList[i]);
                    } 
                    queryObj.SetParameter("recommend", recommend);

                    queryObj.ExecuteUpdate();

                    transaction.Commit();

                    session.Flush();
                    return 1;
                }
            }
        }


    }
}
