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
                query += " AND REQ.ORDER_STATUS = :state";
            }

            if (text != "")
            {
                query += " AND (M.NAME LIKE :text OR PM.NAME LIKE :text ) ";
            }

            query += " ORDER BY REQ.NO DESC";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(PrinterStateT));
                if (query.Contains(":start"))
                    queryObj.SetParameter("start", start);
                if (query.Contains(":end"))
                    queryObj.SetParameter("end", end + " 23:59:59 ");
                if (query.Contains(":state"))
                    queryObj.SetParameter("state", state);
                if (query.Contains(":text"))
                    queryObj.SetParameter("text", "%" + text + "%");


                return queryObj.List<PrinterStateT>();
            }
        }


        public int InsertPrinter(PrinterT printer)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                int printerNo = (int)session.Save(printer);
                session.Flush();
                return printerNo;
            }
        }
        public void UpdatePrinterTemp(int printerNo, string delno, string temp)
        {
            PrinterT printer = GetPrinterByNo(printerNo);
            printer.Temp = temp;
            UpdatePrinter(printer);
            using (ISession session = NHibernateHelper.OpenSession())
            {
                if (!string.IsNullOrEmpty(delno))
                {
                    string[] delNoL = delno.Split(',');
                    string delfileQuery = string.Empty;
                    foreach (var delNo in delNoL)
                    {
                        delfileQuery += @" UPDATE PRINTER_FILE SET FILE_GUBUN = 'DELETE' WHERE [NO] = :delNo AND TEMP= :temp ";
                    }
                    IQuery delQuery = session.CreateSQLQuery(delfileQuery);
                    delQuery.SetParameter("delNo", delno);
                    delQuery.SetParameter("temp", temp);


                    int cnt = (int)delQuery.ExecuteUpdate();
                }

                string updfileQuery = @"UPDATE PRINTER_FILE set FILE_GUBUN='prt_test_img', PRINTER_NO = :printerNo where FILE_GUBUN='prt_test_temp' and TEMP= :temp ";
                IQuery updQuery = session.CreateSQLQuery(updfileQuery);
                updQuery.SetParameter("printerNo", printerNo);
                updQuery.SetParameter("temp", temp);

                updQuery.ExecuteUpdate();
                session.Flush();
            }


        }

        public void UpdatePrinter(PrinterT printer)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                session.SaveOrUpdate(printer);
                session.Flush();
            }
        }


        #region 검색결과
        public IList<PrinterT> GetSearchList(string text, string recommendYn, string recommendVisibility, string gubun, int fromPage, int toPage)
        {
            //int downloadScore = 40;
            //int commentScore = 20;
            //int likeScore = 10;
            //int viewScore = 5;

            string targetOptQuery = string.Empty;

            string rowNumQuery = string.Empty;

            string rowNumWhere = string.Empty;

            string whereQuery = string.Empty;

            switch ((gubun == null) ? null : gubun.ToUpper())
            {
                case "N":
                    rowNumQuery = ", ROW_NUMBER() OVER(ORDER BY P.REG_DT DESC) AS ROW_NUM ";
                    break;
                case "P":
                    rowNumQuery = @", ROW_NUMBER() OVER(ORDER BY (ISNULL((SELECT AVG(R.SCORE) FROM REVIEW R WITH(NOLOCK) WHERE R.PRINTER_NO = P.NO),0) * 
													(SELECT COUNT(0) FROM ORDER_REQ REQ WITH(NOLOCK) WHERE REQ.PRINTER_NO = P.NO)
													) DESC) AS ROW_NUM ";
                    whereQuery += " AND P.TEST_COMPLETE_FLAG = 'Y' ";
                    break;
                case "R":
                    rowNumQuery = ", ROW_NUMBER() OVER(ORDER BY P.RECOMMEND_DT DESC) AS ROW_NUM ";
                    break;
                default:
                    rowNumQuery = ", ROW_NUMBER() OVER(ORDER BY P.REG_DT DESC) AS ROW_NUM ";
                    break;
            }



            if (fromPage <= toPage) { rowNumWhere += " WHERE ROW_NUM BETWEEN :fromPage AND :toPage"; }

            if (text != "")
            { whereQuery += " AND (PRINTER_MEMBER_NAME LIKE :text OR BRAND LIKE :text OR MODEL LIKE :text )"; }
            if (recommendYn != "")
            { whereQuery += " AND RECOMMEND_YN = :recommendYn "; }
            if (recommendVisibility != "" && recommendVisibility != null)
            { whereQuery += " AND RECOMMEND_VISIBILITY = :recommendVisibility "; }



            targetOptQuery = @";WITH PRINTER_TEMP AS (SELECT P.NO, ISNULL(PF.RENAME, PF.NAME) AS MAIN_IMAGE, P.BRAND, P.MODEL , PF.PATH, PM.NAME AS PRINTER_MEMBER_NAME,M.NAME AS MEMBER_NAME, P.LOC_X, P.LOC_Y, P.QUALITY 'QUALITY', P.STATUS 'STATUS',P.TEST_COMPLETE_FLAG, P.DEL_FLAG 'DEL_FLAG', 
                                     ISNULL(P.DEL_DT, '1999-01-01') 'DEL_DT', P.REG_ID 'REG_ID', P.REG_DT 'REG_DT', P.UPD_ID 'UPD_ID', ISNULL(P.UPD_DT,'1999-01-01') 'UPD_DT', P.RECOMMEND_YN, ISNULL(P.RECOMMEND_DT,'1999-01-01') 'RECOMMEND_DT' , P.RECOMMEND_VISIBILITY, P.RECOMMEND_PRIORITY , (SELECT AVG(SCORE) FROM REVIEW R WHERE R.PRINTER_NO = P.NO) SCORE " + rowNumQuery + @",
                                    (SELECT MIN(UNIT_PRICE) FROM PRINTER_COLOR WITH(NOLOCK) WHERE PRINTER_NO = P.NO) AS MIN_PRICE,
									(SELECT MAX(UNIT_PRICE) FROM PRINTER_COLOR WITH(NOLOCK) WHERE PRINTER_NO = P.NO) AS MAX_PRICE, PM.ADDRESS,
                                    (ISNULL((SELECT AVG(R.SCORE) FROM REVIEW R WITH(NOLOCK) WHERE R.PRINTER_NO = P.NO),0) * 
													(SELECT COUNT(0) FROM ORDER_REQ REQ WITH(NOLOCK) WHERE REQ.PRINTER_NO = P.NO)
													)AS POP
                                    
                                    
                            FROM PRINTER P WITH(NOLOCK) INNER JOIN PRINTER_FILE AS PF WITH(NOLOCK)
					                            ON P.MAIN_IMG = PF.SEQ AND P.NO = PF.PRINTER_NO 
					                            LEFT JOIN PRINTER_MEMBER AS PM WITH(NOLOCK)
					                            ON P.MEMBER_NO = PM.MEMBER_NO 
                                                LEFT JOIN MEMBER AS M WITH(NOLOCK)
                                                ON P.MEMBER_NO = M.NO
                                                WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL) 
                                                AND P.DEL_FLAG='N' " + whereQuery + @") SELECT * FROM PRINTER_TEMP" + rowNumWhere;


            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(targetOptQuery);
                if (fromPage <= toPage)
                {
                    queryObj.SetParameter("fromPage", fromPage);
                    queryObj.SetParameter("toPage", toPage);
                }
                if (targetOptQuery.Contains(":text"))
                {
                    queryObj.SetParameter("text", "%" + text + "%");
                }
                if (targetOptQuery.Contains(":recommendYn"))
                {
                    queryObj.SetParameter("recommendYn", recommendYn);
                }
                if (targetOptQuery.Contains(":recommendVisibility"))
                {
                    queryObj.SetParameter("recommendVisibility", recommendVisibility);
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
                    printer.PrinterMemberName = (string)row[5];
                    printer.MemberName = (string)row[6];
                    printer.LocX = (float)System.Convert.ToDouble(row[7]);
                    printer.LocY = (float)System.Convert.ToDouble(row[8]);
                    printer.Quality = (int)row[9];
                    printer.Status = System.Convert.ToInt32(row[10]);
                    printer.TestCompleteFlag = (string)row[11];
                    printer.DelFlag = (string)row[12];
                    printer.DelDt = (DateTime)row[13];
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
                    if (row[22] != null)
                    {
                        printer.Score = (float)System.Convert.ToDouble(row[22]);
                    }
                    printer.MinPrice = (int)row[24];
                    printer.MaxPrice = (int)row[25];
                    printer.locName = (string)row[26];
                    list.Add(printer);
                }

                return list;
            }
        }

        #endregion


        #region 검색결과 수
        public int GetSearchCount(string text, string recommendYn, string recommendVisibility)
        {

            //int downloadScore = 40;
            //int commentScore = 20;
            //int likeScore = 10;
            //int viewScore = 5;

            string targetCntQuery = string.Empty;

            string rowNumQuery = string.Empty;

            string whereQuery = string.Empty;

            if (text != "") { whereQuery += " AND (PM.NAME LIKE :text OR P.BRAND LIKE :text OR P.MODEL LIKE :text )"; }

            if (recommendYn != "") { whereQuery += "AND P.RECOMMEND_YN = :recommendYn "; }

            if (recommendVisibility != "") { whereQuery += " AND RECOMMEND_VISIBILITY = :recommendVisibility "; }

            targetCntQuery = @"SELECT count(1)   
                            FROM PRINTER P WITH(NOLOCK) INNER JOIN PRINTER_FILE AS PF WITH(NOLOCK)
					                            ON P.MAIN_IMG = PF.NO
					                            LEFT JOIN PRINTER_MEMBER AS PM WITH(NOLOCK)
					                            ON P.MEMBER_NO = PM.NO
                                                LEFT JOIN MEMBER AS M WITH(NOLOCK)
                                                ON P.MEMBER_NO = M.NO
                                                WHERE (M.DEL_FLAG != 'Y' OR M.DEL_FLAG IS NULL)" + whereQuery;


            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(targetCntQuery);
                if (targetCntQuery.Contains(":text"))
                {
                    queryObj.SetParameter("text", "%" + text + "%");
                }
                if (targetCntQuery.Contains(":recommendYn"))
                {
                    queryObj.SetParameter("recommendYn", recommendYn);
                }
                if (targetCntQuery.Contains(":recommendVisibility"))
                {
                    queryObj.SetParameter("recommendVisibility", recommendVisibility);
                }

                int rowCnt = (int)queryObj.UniqueResult();

                session.Flush();

                return rowCnt;
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
                if (printer != null)
                {
                    printer.ImageName = printerFileDac.GetMainImg(printer.MainImg, no);
                }
                return printer;
            }
        }

        public PrinterT GetPrinterDetailByPrinterNo(int no)
        {
            PrinterT printer = new PrinterT();
            using (ISession session = NHibernateHelper.OpenSession())
            {
                printer = session.QueryOver<PrinterT>().Where(w => w.No == no).SingleOrDefault<PrinterT>();

                PrinterMaterialDac _printerMaterialDac = new PrinterMaterialDac();
                printer.PrinterMaterialList = _printerMaterialDac.GetPrinterMaterialByPrinterNo(printer.No);
            }
            return printer;

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
            if (recommend == "Y") { query = "UPDATE PRINTER SET RECOMMEND_DT = GETDATE(), RECOMMEND_YN = :recommend ,RECOMMEND_PRIORITY = (SELECT MAX(RECOMMEND_PRIORITY) FROM PRINTER) WHERE "; }
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
                    queryObj.SetParameter("recommend", recommend);
                    for (int i = 0; i < noList.Length; i++)
                    {
                        queryObj.SetParameter(i, noList[i]);
                    }

                    queryObj.ExecuteUpdate();

                    transaction.Commit();

                    session.Flush();
                    return 1;
                }
            }
        }

        #region 프린팅 메인 & 검색
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="quality"></param>
        /// <param name="material"></param>
        /// <param name="gubun">R,P,C</param>
        /// <returns></returns>
        public IList<PrinterT> SearchPrinterInMain(string location, int quality, int material, string gubun, string text)
        {
            IList<PrinterT> list = new List<PrinterT>();
            string query = string.Empty;

            query = string.Format(@"SELECT P.NO, P.BRAND, P.MODEL, PF.RENAME, PMEM.[ADDRESS], 
                                                    ISNULL((SELECT AVG(SCORE) FROM REVIEW WITH(NOLOCK) WHERE PRINTER_NO = P.NO),0) AS SCORE, PMEM.NAME, P.MEMBER_NO ,
                                                    (SELECT MIN(UNIT_PRICE) FROM PRINTER_COLOR WITH(NOLOCK) WHERE PRINTER_NO = P.NO) AS MIN_PRICE,
													(SELECT MAX(UNIT_PRICE) FROM PRINTER_COLOR WITH(NOLOCK) WHERE PRINTER_NO = P.NO) AS MAX_PRICE
                                                   FROM PRINTER P WITH(NOLOCK) INNER JOIN PRINTER_MATERIAL PM WITH(NOLOCK)
                                                                                ON P.NO = PM.PRINTER_NO
				                                                                INNER JOIN PRINTER_FILE PF WITH(NOLOCK)
                                                                                ON P.NO = PF.PRINTER_NO
                                                                                AND P.MAIN_IMG = PF.SEQ
				                                                                INNER JOIN MEMBER M WITH(NOLOCK)
													                            ON P.MEMBER_NO = M.NO
													                            INNER JOIN MATERIAL MAT WITH(NOLOCK)
                                                                                ON PM.MATERIAL_NO = MAT.NO
                                                                                INNER JOIN PRINTER_MEMBER PMEM WITH(NOLOCK)
                                                   							    ON P.MEMBER_NO = PMEM.MEMBER_NO
                                                                                WHERE P.DEL_FLAG = 'N' AND P.SAVE_FLAG = 'N' ");

            if (location != "")
            {
                query += " AND PMEM.[ADDRESS] LIKE :location ";
            }
            if (quality != 0)
            {
                query += " AND P.QUALITY = :quality ";
            }
            if (material != 0)
            {
                query += " AND PM.MATERIAL_NO = :material ";
            }
            if (text != "")
            {
                query += @" AND (PMEM.NAME LIKE :text OR P.BRAND LIKE :text OR P.MODEL LIKE :text OR PMEM.[ADDRESS] LIKE :text OR MAT.NAME LIKE :text) ";
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                if (query.Contains(":location"))
                {
                    queryObj.SetParameter("location", "%" + location + "%");
                }
                if (query.Contains(":quality"))
                {
                    queryObj.SetParameter("quality", quality);
                }
                if (query.Contains(":material"))
                {
                    queryObj.SetParameter("material", material);
                }
                if (query.Contains(":text"))
                {
                    queryObj.SetParameter("text", "%" + text + "%");
                }

                IList<object[]> result = queryObj.List<object[]>();

                foreach (object[] rows in result)
                {
                    PrinterT printer = new PrinterT();
                    printer.No = (int)rows[0];
                    printer.Brand = (string)rows[1];
                    printer.Model = (string)rows[2];
                    printer.ImageName = (string)rows[3];
                    printer.locName = (string)rows[4];
                    printer.Score = (double)rows[5];
                    printer.PrinterMemberName = (string)rows[6];
                    printer.MemberNo = (int)rows[7];
                    printer.MinPrice = (int)rows[8];
                    printer.MaxPrice = (int)rows[9];
                    list.Add(printer);
                }

            }

            return list;
        }
        #endregion

        #region 메인페이지 4개 select
        public IList<PrinterT> SearchPrinterForMainTop4(string gubun)
        {
            IList<PrinterT> list = new List<PrinterT>();
            string query = string.Format(@"SELECT TOP 4 P.NO, P.BRAND, P.MODEL, PF.RENAME, PM.[ADDRESS], 
                                                    ISNULL((SELECT AVG(SCORE) FROM REVIEW WITH(NOLOCK) WHERE PRINTER_NO = P.NO),0) AS SCORE, PM.NAME, P.MEMBER_NO ,
                                                    (SELECT MIN(UNIT_PRICE) FROM PRINTER_COLOR WITH(NOLOCK) WHERE PRINTER_NO = P.NO) AS MIN_PRICE,
													(SELECT MAX(UNIT_PRICE) FROM PRINTER_COLOR WITH(NOLOCK) WHERE PRINTER_NO = P.NO) AS MAX_PRICE,
													(ISNULL((SELECT AVG(R.SCORE) FROM REVIEW R WITH(NOLOCK) WHERE R.PRINTER_NO = P.NO),0) *
													(SELECT COUNT(0) FROM ORDER_REQ REQ WITH(NOLOCK) WHERE REQ.PRINTER_NO = P.NO)
													)AS POP,
                                                    P.TEST_COMPLETE_FLAG
          
                                                    FROM PRINTER P WITH(NOLOCK) INNER JOIN PRINTER_FILE PF WITH(NOLOCK)
				                                                                    ON P.NO = PF.PRINTER_NO
																					AND P.MAIN_IMG = PF.SEQ
                                                                                    INNER JOIN MEMBER M WITH(NOLOCK)
				                                                                    ON P.MEMBER_NO = M.NO
																					INNER JOIN PRINTER_MEMBER PM WITH(NOLOCK)
																					ON P.MEMBER_NO = PM.MEMBER_NO
                                                                                    WHERE P.DEL_FLAG = 'N' AND P.SAVE_FLAG = 'N' ");

            switch (gubun)
            {
                case "R":
                    query += " AND P.RECOMMEND_VISIBILITY ='Y' ORDER BY P.RECOMMEND_PRIORITY DESC, P.RECOMMEND_DT DESC";
                    break;
                case "P":
                    query += " AND P.TEST_COMPLETE_FLAG = 'Y' ORDER BY POP DESC";
                    break;
                case "N":
                    query += " ORDER BY P.REG_DT DESC";
                    break;
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                IList<object[]> result = queryObj.List<object[]>();

                foreach (object[] rows in result)
                {
                    PrinterT printer = new PrinterT();
                    printer.No = (int)rows[0];
                    printer.Brand = (string)rows[1];
                    printer.Model = (string)rows[2];
                    printer.ImageName = (string)rows[3];
                    printer.locName = (string)rows[4];
                    printer.Score = (double)rows[5];
                    printer.PrinterMemberName = (string)rows[6];
                    printer.MemberNo = (int)rows[7];
                    printer.MinPrice = (int)rows[8];
                    printer.MaxPrice = (int)rows[9];
                    printer.TestCompleteFlag = (string)rows[11];
                    list.Add(printer);
                }

            }

            return list;
        }
        #endregion

        #region 재질 가져오기
        public IList<MaterialT> GetMatrialListByPrinterNo(int printerNo)
        {
            IList<MaterialT> list = new List<MaterialT>();
            string query = @"SELECT PM.NO, M.NAME, 
                            (SELECT MIN(A.UNIT_PRICE) FROM PRINTER_COLOR A WITH(NOLOCK) WHERE A.PRINTER_MATERIAL_NO = PM.NO) AS MIN_PRICE, 
                            (SELECT MAX(A.UNIT_PRICE) FROM PRINTER_COLOR A WITH(NOLOCK) WHERE A.PRINTER_MATERIAL_NO = PM.NO) AS MAX_PRICE

                            FROM PRINTER_MATERIAL PM WITH(NOLOCK)INNER JOIN MATERIAL M WITH(NOLOCK)
					                            ON PM.MATERIAL_NO = M.NO
					                            INNER JOIN PRINTER_COLOR PC WITH(NOLOCK)
					                            ON PM.NO = PC.PRINTER_MATERIAL_NO
					                            WHERE M.DEL_FLAG != 'Y'  AND PM.PRINTER_NO = :printerNo ";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("printerNo", printerNo);

                IList<object[]> result = queryObj.List<object[]>();
                foreach (object[] row in result)
                {
                    MaterialT material = new MaterialT();
                    material.No = (int)row[0];
                    material.Name = (string)row[1];
                    material.MinPrice = (int)row[2];
                    material.MaxPrice = (int)row[3];
                    list.Add(material);
                }
            }
            return list;
        }
        #endregion

        #region
        public IList<PrinterT> GetPrinterByPrtMemberNo(int printerMemberNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IList<PrinterT> printerList = session.QueryOver<PrinterT>().Where(w => w.MemberNo == printerMemberNo && w.DelFlag != "Y").OrderBy(o => o.Brand).Asc.List<PrinterT>();
                foreach (PrinterT printer in printerList)
                {

                    double scoreSum = 0;
                    int scoreCnt = 0;
                    IList<ReviewT> reviewList = new ReviewDac().GetReviewListByPrinterNo(printer.No);
                    foreach (ReviewT review in reviewList)
                    {
                        scoreSum += review.Score;
                        scoreCnt++;

                    }
                    if (scoreCnt != 0)
                    {
                        printer.Score = scoreSum / scoreCnt;
                    }
                    else
                    {
                        printer.Score = 0;
                    }


                    int maxPrice = 0;
                    int minPrice = 999999999;
                    printer.PrinterMaterialList = new PrinterMaterialDac().GetPrinterMaterialByPrinterNo(printer.No);
                    foreach (PrinterMaterialT prtMat in printer.PrinterMaterialList)
                    {
                        foreach (PrinterColorT color in prtMat.MaterialColorList)
                        {
                            if (color.UnitPrice < minPrice)
                            {
                                minPrice = color.UnitPrice;
                            }
                            if (color.UnitPrice > maxPrice)
                            {
                                maxPrice = color.UnitPrice;
                            }
                        }
                    }
                    printer.MaxPrice = maxPrice;
                    printer.MinPrice = minPrice;
                }
                return printerList;
            }
        }
        #endregion

        #region 사용 안함
        public IList<PrinterColorT> GetMaterialColorByPrinterNo(int printerNo, int printerMatNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<PrinterColorT>().Where(w => w.PrinterNo == printerNo && w.PrinterMaterialNo == printerMatNo).List<PrinterColorT>();
            }
        }
        #endregion

        #region
        public IList<PrinterColorT> GetMaterialColorByPrinterMatNo(int printerMatNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<PrinterColorT>().Where(w => w.PrinterMaterialNo == printerMatNo).List<PrinterColorT>();
            }
        }
        #endregion


        //by Gooksong
        #region 재질 가져오기
        public IList<PrinterMaterialT> GetPrinterMatrialListByPrinterNo(int printerNo)
        {
            IList<PrinterMaterialT> list = new List<PrinterMaterialT>();
            string query = @"SELECT PM.NO,M.NO, PM.UNIT_PRICE, M.NAME
                                FROM PRINTER_MATERIAL PM WITH(NOLOCK)INNER JOIN MATERIAL M WITH(NOLOCK)
						                                ON PM.MATERIAL_NO = M.NO
						                                WHERE M.DEL_FLAG != 'Y'  AND PM.PRINTER_NO = :printerNo";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("printerNo", printerNo);

                IList<object[]> result = queryObj.List<object[]>();
                foreach (object[] row in result)
                {
                    PrinterMaterialT material = new PrinterMaterialT();
                    material.No = (int)row[0];
                    material.MaterialNo = (int)row[1];
                    //material.UnitPrice = (int)row[2];
                    material.MaterialName = (string)row[3];
                    list.Add(material);
                }
            }
            return list;
        }
        #endregion

        #region printer_material_no로 색상 가져오기(단일)
        public PrinterColorT GetSinglePrinterColorByColorNo(int colorNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<PrinterColorT>().Where(w => w.No == colorNo).Take(1).SingleOrDefault<PrinterColorT>();
            }
        }
        #endregion

        #region
        public IList<PrinterColorT> GetPrinterMaterialColorByPrinterMaterialNo(int PrinterMaterialNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<PrinterColorT>().Where(w => w.PrinterMaterialNo == PrinterMaterialNo).List<PrinterColorT>();
            }
        }
        #endregion

        #region
        public IList<MaterialT> GetAllMaterialList()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<MaterialT>().Where(w => w.DelFlag == "N").List<MaterialT>();
            }
        }
        #endregion

        #region printerFile
        public IList<PrinterFileT> GetPrinterFile(int printerNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<PrinterFileT>().Where(w => w.PrinterNo == printerNo).List<PrinterFileT>();
            }
        }
        #endregion

        #region
        public IList<PrinterT> GetPrinterTop4ByPrtMemberNo(int printerNo)
        {
            IList<PrinterT> list = new List<PrinterT>();
            using (ISession session = NHibernateHelper.OpenSession())
            {
                PrinterT p = session.QueryOver<PrinterT>().Where(w => w.No == printerNo).Take(1).SingleOrDefault<PrinterT>();
                string query = @"SELECT TOP 4 P.NO, P.BRAND ,P.MODEL, PF.RENAME, M.NAME
                                                FROM PRINTER P WITH(NOLOCK) INNER JOIN PRINTER_FILE PF WITH(NOLOCK)
					                                                ON P.NO = PF.PRINTER_NO
					                                                AND P.MAIN_IMG = PF.SEQ
					                                                INNER JOIN MEMBER M WITH(NOLOCK)
					                                                ON P.MEMBER_NO = M.NO

					                                                WHERE P.DEL_FLAG != 'Y' AND P.MEMBER_NO = :memberNo AND P.NO != :printerNo ";
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("memberNo", p.MemberNo);
                queryObj.SetParameter("printerNo", printerNo);

                IList<object[]> result = queryObj.List<object[]>();

                foreach (object[] row in result)
                {
                    PrinterT printer = new PrinterT();
                    printer.No = (int)row[0];
                    printer.Brand = (string)row[1];
                    printer.Model = (string)row[2];
                    printer.ImageName = (string)row[3];
                    printer.MemberName = (string)row[4];
                    list.Add(printer);
                }
                session.Flush();
            }
            return list;
        }
        #endregion

        #region
        public IList<PrinterMemberT> GetSpotListInOrder(string location, float locationX, float locationY, int quality, int material)
        {
            IList<PrinterMemberT> before = new List<PrinterMemberT>();
            IList<PrinterMemberT> after = new List<PrinterMemberT>();

            using (ISession session = NHibernateHelper.OpenSession())
            {
                string pmQuery = @"SELECT [NO],MEMBER_NO,ACCOUNT_NO,BANK, BANK_NAME ,HOME_PHONE,CELL_PHONE, NAME,[ADDRESS], ADDRESS_DETAIL,LOC_X,LOC_Y,POST_MODE, TAXBILL_YN, PRINTER_PROFILE_MSG, SAVE_FLAG, DEL_FLAG, REG_ID, REG_DT, UPD_ID, UPD_DT, PROFILE_PIC, COVER_PIC, URL,VIEW_CNT, POST_TYPE, POST_PRICE
                                FROM PRINTER_MEMBER WITH(NOLOCK) INNER JOIN 
								(
								SELECT PM.NO AS MATCH 
								FROM PRINTER_MEMBER PM WITH(NOLOCK)
								INNER JOIN PRINTER P WITH(NOLOCK)
								ON PM.MEMBER_NO = P.MEMBER_NO
								AND P.STATUS = 1 AND P.DEL_FLAG='N'
								GROUP BY PM.NO
								) AS D
								ON NO = D.MATCH
                                WHERE ADDRESS LIKE :location";
                //WHERE ADDRESS LIKE '%{0}%' OR (LOC_X = '{1}' AND LOC_Y = '{2}')", location, locationX, locationY);
                //list = session.QueryOver<PrinterMemberT>().Where(w => w.SpotAddress == location).List<PrinterMemberT>();

                IQuery beforeQuery = session.CreateSQLQuery(pmQuery).AddEntity(typeof(PrinterMemberT));
                beforeQuery.SetParameter("location", location);

                before = (IList<PrinterMemberT>)beforeQuery.List<PrinterMemberT>();

                for (int i = 0; i < before.Count; i++)
                {
                    //list[i].printerList = session.QueryOver<PrinterT>().Where(w => w.PrtMemberNo == list[i].MemberNo).List<PrinterT>();

                    string prtQuery = @"SELECT NO, BRAND ,MODEL, MEMBER_NO, COMMENT, LOC_X, LOC_Y, QUALITY, STATUS, TEST_COMPLETE_FLAG, DEL_FLAG, DEL_DT, REG_DT, REG_ID, UPD_ID, UPD_DT, RECOMMEND_DT, RECOMMEND_YN, RECOMMEND_VISIBILITY, RECOMMEND_PRIORITY,  MAIN_IMG, LOC_NAME, SAVE_FLAG, POST_MODE, POST_PRICE, RESOLUTION, TEMP
                                        FROM PRINTER WITH(NOLOCK) 
                                        WHERE STATUS != 2 AND MEMBER_NO = :memberNo ";
                    if (quality != 0) { prtQuery += " AND QUALITY = :quality "; }

                    IQuery printerQuery = session.CreateSQLQuery(prtQuery).AddEntity(typeof(PrinterT));
                    printerQuery.SetParameter("memberNo", before[i].MemberNo);
                    if (prtQuery.Contains(":quality")) { printerQuery.SetParameter("quality", quality); };

                    before[i].printerList = (IList<PrinterT>)printerQuery.List<PrinterT>();

                    List<PrinterMaterialT> mat = new List<PrinterMaterialT>();
                    List<PrinterColorT> col = new List<PrinterColorT>();
                    List<ReviewT> review = new List<ReviewT>();
                    for (int j = 0; j < before[i].printerList.Count; j++)
                    {
                        //string matQuery = string.Format(@"SELECT NO, PRINTER_NO, MATERIAL_NO, UNIT_PRICE, REG_ID, REG_DT
                        string matQuery = @"SELECT NO, PRINTER_NO, MATERIAL_NO, REG_ID, REG_DT
                                                    FROM PRINTER_MATERIAL WITH(NOLOCK)
                                                    WHERE PRINTER_NO = :printerNo ";
                        if (material != 0) { matQuery += "  AND MATERIAL_NO = :material "; }

                        IQuery materialQuery = session.CreateSQLQuery(matQuery).AddEntity(typeof(PrinterMaterialT));
                        materialQuery.SetParameter("printerNo", before[i].printerList[j].No);
                        if (matQuery.Contains(":material")) { materialQuery.SetParameter("material", material); }

                        //mat.AddRange(session.QueryOver<PrinterMaterialT>().Where(w => w.PrinterNo == list[i].printerList[j].No).List<PrinterMaterialT>());
                        mat.AddRange((IList<PrinterMaterialT>)materialQuery.List<PrinterMaterialT>());
                        col.AddRange(session.QueryOver<PrinterColorT>().Where(w => w.PrinterNo == before[i].printerList[j].No).List<PrinterColorT>());
                        review.AddRange(session.QueryOver<ReviewT>().Where(w => w.PrinterNo == before[i].printerList[j].No).List<ReviewT>());
                    }
                    if (before[i].printerList.Count > 0 && mat.Count > 0)
                    {
                        before[i].matList = mat;
                        before[i].colorList = col;
                        before[i].reviewList = review;
                        before[i].MinPrice = col == null ? 0 : col.Min(m => m.UnitPrice);
                        before[i].MaxPrice = col == null ? 0 : col.Max(m => m.UnitPrice);

                        foreach (ReviewT re in review)
                        {
                            before[i].ReviewScore += re.Score;
                        }
                        before[i].ReviewScore = before[i].ReviewScore / before[i].reviewList.Count;

                        after.Add(before[i]);
                    }
                }
            }

            return after;
        }
        #endregion

        #region
        public PrinterMemberT GetPrinterListByMemberNo(int prtMemberNo)
        {
            PrinterMemberT result = new PrinterMemberT();

            using (ISession session = NHibernateHelper.OpenSession())
            {
                result = session.QueryOver<PrinterMemberT>().Where(w => w.MemberNo == prtMemberNo).Take(1).SingleOrDefault<PrinterMemberT>();
                if (result != null)
                {
                    result.printerList = session.QueryOver<PrinterT>().Where(w => w.MemberNo == result.MemberNo && w.Status == (int)Makersn.Util.MakersnEnumTypes.PrinterStatus.출력가능).List<PrinterT>();

                    List<ReviewT> review = new List<ReviewT>();
                    for (int i = 0; i < result.printerList.Count; i++)
                    {
                        review.AddRange(session.QueryOver<ReviewT>().Where(w => w.PrinterNo == result.printerList[i].No).List<ReviewT>());
                    }
                    result.reviewList = review;
                }
            }

            return result;
        }
        #endregion

        #region
        public PrinterMemberT GetPrinterListByPrinterNo(int printerNo)
        {
            PrinterMemberT result = new PrinterMemberT();

            using (ISession session = NHibernateHelper.OpenSession())
            {
                PrinterT printer = session.QueryOver<PrinterT>().Where(w => w.No == printerNo).Take(1).SingleOrDefault<PrinterT>();
                result = session.QueryOver<PrinterMemberT>().Where(w => w.MemberNo == printer.MemberNo).Take(1).SingleOrDefault<PrinterMemberT>();
                if (result != null)
                {
                    result.printerList = session.QueryOver<PrinterT>().Where(w => w.MemberNo == result.MemberNo && w.Status == (int)Makersn.Util.MakersnEnumTypes.PrinterStatus.출력가능).List<PrinterT>();

                    List<ReviewT> review = new List<ReviewT>();
                    for (int i = 0; i < result.printerList.Count; i++)
                    {
                        review.AddRange(session.QueryOver<ReviewT>().Where(w => w.PrinterNo == result.printerList[i].No).List<ReviewT>());
                    }
                    result.reviewList = review;
                }
            }

            return result;
        }
        #endregion

        #region
        public IList<PrinterT> GetTestReqeustPrinter(string flag)
        {
            string query = @"SELECT P.NO, PM.NAME, P.BRAND, P.MODEL, P.REG_DT, P.TEST_COMPLETE_FLAG, REQ.OD_NO,
                            ISNULL((SELECT AVG(SCORE) FROM REVIEW WHERE PRINTER_NO = P.NO),0) AS SCORE
                            FROM PRINTER P WITH(NOLOCK) INNER JOIN 
											(SELECT PRINTER_NO, NO AS OD_NO
											FROM ORDER_REQ 
											WHERE TEST_FLAG = 'Y') AS REQ
				                            ON REQ.PRINTER_NO = P.NO
				                            INNER JOIN MEMBER M WITH(NOLOCK)
				                            ON P.MEMBER_NO = M.NO
				                            INNER JOIN PRINTER_MEMBER PM WITH(NOLOCK)
				                            ON P.MEMBER_NO = PM.MEMBER_NO
                                            AND P.TEST_COMPLETE_FLAG = :flag";

            IList<PrinterT> list = new List<PrinterT>();

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("flag", flag);

                IList<object[]> result = queryObj.List<object[]>();

                foreach (object[] row in result)
                {
                    PrinterT printer = new PrinterT();
                    printer.No = (int)row[0];
                    printer.SpotName = (string)row[1];
                    printer.Brand = (string)row[2];
                    printer.Model = (string)row[3];
                    printer.RegDt = (DateTime)row[4];
                    printer.TestCompleteFlag = (string)row[5];
                    printer.orderNo = (long)row[6];
                    printer.Score = (double)row[7];
                    list.Add(printer);
                }

            }

            return list;
        }
        #endregion

        #region 프린터 테스트 승인
        public bool ApprPrinterTest(int no, string flag)
        {
            bool result = false;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                PrinterT printer = session.QueryOver<PrinterT>().Where(w => w.No == no).Take(1).SingleOrDefault<PrinterT>();
                if (printer != null)
                {
                    printer.TestCompleteFlag = flag;
                    session.Update(printer);
                    session.Flush();
                    result = true;
                }
            }
            return result;
        }
        #endregion


        public bool doPrtEdit(int printerNo, string userId, string userName, string ip, PrinterT printer, string[] imgNameInfo, string[] imgReNameInfo, string[] imgSizeInfo, string[] matInfo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        //int printerNo = 70;
                        //insert PrinterFile
                        new PrinterFileDac().RemovePrinterFileWithPrtNo(printerNo, session);
                        if (imgNameInfo != null && imgReNameInfo != null && imgSizeInfo != null)
                        {
                            if (imgNameInfo.Length == imgReNameInfo.Length && imgNameInfo.Length == imgSizeInfo.Length && imgReNameInfo.Length == imgSizeInfo.Length)
                            {
                                //img Upload to DB
                                List<PrinterFileT> imgList = new List<PrinterFileT>();
                                for (int i = 1; i < imgNameInfo.Length; i++)
                                {
                                    // the first Img must be main Img
                                    if (imgNameInfo[i] != "" && imgReNameInfo[i] != "")
                                    {
                                        PrinterFileT img = new PrinterFileT();
                                        img.PrinterNo = printerNo;
                                        img.Name = imgNameInfo[i];
                                        img.Rename = imgReNameInfo[i];
                                        img.Size = imgSizeInfo[i];
                                        img.FileGubun = "prt_img";
                                        img.Seq = i;
                                        img.RegId = userId;
                                        img.RegDt = DateTime.Now;
                                        img.RegIp = ip;
                                        imgList.Add(img);
                                    }
                                }

                                new PrinterFileDac().InsertPrinterFileByList(imgList, session);
                            }
                        }

                        //insert material
                        new PrinterMaterialDac().RemovePrinterMaterialAndColorWithPrtNo(printerNo, session);
                        for (int i = 0; i < matInfo.Length; i++)
                        {
                            if (matInfo[i] != "")
                            {
                                new PrinterMaterialDac().InsertWithColorByStr(printerNo, userName, matInfo[i], session);
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return false;
                    }


                    transaction.Commit();
                    session.Flush();

                }
            }
            return true;
        }
    }
}
