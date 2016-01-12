﻿using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreProductDac
    {

        private static StoreContext dbContext;
        
        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal IList<StoreProductT> SelectAllStoreProduct()
        {

            IList<StoreProductT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StoreProductT.ToList();
            }
            return printers;
        }

        /// <summary>
        /// select one StoreProduct By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StoreProductT SelectStoreProductTById(int no)
        {
            StoreProductT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StoreProductT.Where(m => m.No == no).FirstOrDefault();
            }

            return printer;
        }

        /// <summary>
        /// Insert StoreProduct
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public long InsertStoreProduct(StoreProductT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            long ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StoreProductT.Add(data);
                dbContext.SaveChanges();
                ret = data.No;
            }
            return ret;
        }

        /// <summary>
        /// Update StoreProduct
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int UpdateStoreProduct(StoreProductT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                try
                {
                    dbContext.StoreProductT.Attach(data);
                    dbContext.Entry<StoreProductT>(data).State = System.Data.Entity.EntityState.Modified;
                    dbContext.SaveChanges();
                    ret = 1;
                }
                catch (Exception)
                {
                    ret = -1;
                }
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="certificateStatus"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        internal List<StoreProductT> SelectProductWithCertification(int certificateStatus,string query)
        {
            using (dbContext = new StoreContext())
            {
                return dbContext.StoreProductT
                    .Where(p =>( p.CertiFicateStatus == certificateStatus) && p.ProductName.Contains(query) )
                    .ToList();
            }
        }

        //public int SaveOrUpdate(ArticleT data, string delno)
        //{
        //    using (ISession session = NHibernateHelper.OpenSession())
        //    {
        //        using (ITransaction transaction = session.BeginTransaction())
        //        {
        //            int articleNo = 0;
        //            try
        //            {
        //                if (data.No > 0)
        //                {
        //                    session.Update(data);
        //                    articleNo = data.No;
        //                }
        //                else
        //                {
        //                    articleNo = (Int32)session.Save(data);
        //                }

        //                if (!string.IsNullOrEmpty(delno))
        //                {
        //                    string[] delNoL = delno.Split(',');
        //                    //int[] delnoLT = new int[] { };
        //                    //if (delNoL.Length > 1)
        //                    //{
        //                    //    delnoLT = delNoL.Cast<int>().ToArray();
        //                    //}
        //                    //else
        //                    //{
        //                    //    delnoLT = new int[] { Convert.ToInt32(delno) };
        //                    //}
        //                    string delfileQuery = string.Empty;
        //                    foreach (var delNo in delNoL)
        //                    {
        //                        //delfileQuery += @" UPDATE ARTICLE_FILE SET FILE_GUBUN = 'DELETE' WHERE [NO] =" + delNo + " AND TEMP='" + data.Temp + "'";
        //                        delfileQuery += @" UPDATE ARTICLE_FILE SET FILE_GUBUN = 'DELETE' WHERE [NO] = ? AND TEMP= ? ";
        //                    }

        //                    IQuery queryObj = session.CreateSQLQuery(delfileQuery);
        //                    for (int i = 0; i < delNoL.Length; i++)
        //                    {
        //                        queryObj.SetParameter(i * 2, delNoL[i]);
        //                        queryObj.SetParameter((i * 2) + 1, data.Temp);
        //                    }

        //                    int cnt = queryObj.ExecuteUpdate();
        //                }

        //                //string updfileQuery = @"UPDATE ARTICLE_FILE set FILE_GUBUN='article', ARTICLE_NO = '" + articleNo + "' , UPD_DT = GETDATE(), UPD_ID = '" + data.RegId + "' where FILE_GUBUN='temp' and TEMP='" + data.Temp + "' ";      


        //                string updfileQuery = @"UPDATE ARTICLE_FILE set FILE_GUBUN='article', ARTICLE_NO = :articleNo , UPD_DT = GETDATE(), UPD_ID = :RegId  where FILE_GUBUN='temp' and TEMP= :Temp";

        //                IQuery queryObj2 = session.CreateSQLQuery(updfileQuery);
        //                queryObj2.SetParameter("articleNo", articleNo);
        //                queryObj2.SetParameter("RegId", data.RegId);
        //                queryObj2.SetParameter("Temp", data.Temp);

        //                queryObj2.ExecuteUpdate();
        //            }
        //            catch (Exception ex)
        //            {
        //                throw;
        //            }

        //            transaction.Commit();
        //            session.Flush();

        //            return articleNo;
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="codeNo"></param>
        /// <returns></returns>
        internal int SelectTotalCountByOption(int memberNo, int codeNo)
        {
            using (dbContext = new StoreContext())
            {
                return dbContext.StoreProductT.Count(p => p.CategoryNo == codeNo);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="codeNo"></param>
        /// <returns></returns>
        internal IList<StoreProductT> SelectProductsByOption(int memberNo, int codeNo)
        {
            using (dbContext = new StoreContext())
            {
                return dbContext.StoreProductT.Where(p => p.CategoryNo == codeNo).ToList();
            }
        }
    }
}
