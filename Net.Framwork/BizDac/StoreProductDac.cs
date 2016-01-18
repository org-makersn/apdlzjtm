using Net.Framework;
using Net.Framework.StoreModel;
using Net.Framwork.Helper;
using Net.Framwork.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Framwork.BizDac
{
    public class StoreProductDac : DacBase
    {
        private static readonly StoreContext instance = new StoreContext();

        public static StoreContext dbContext { get; set; }
        
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
                printer = dbContext.StoreProductT.Where(m => m.NO == no).FirstOrDefault();
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
                ret = data.NO;
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
                    .Where(p =>( p.CERTIFICATE_YN == certificateStatus) && p.NAME.Contains(query) )
                    .ToList();
            }
        }

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
                return dbContext.StoreProductT.Count(p => p.CATEGORY_NO == codeNo);
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
                return dbContext.StoreProductT.Where(p => p.CATEGORY_NO == codeNo).ToList();
            }
        }


        public IList<StoreProductExT> SelectProductTest()
        {
            string query = DacHelper.GetSqlCommand("StoreProduct.SelectProductList_S");

            var states = dbHelper.ExecuteMultiple<StoreProductExT>(query);

            //IEnumerable<StoreProductT> list = states != null ? states.ToList() : null;
            //foreach (var item in list)
            //{
            //    string mit = item.USE_YN.Value.ToString();
            //}
            //return list;

            using (dbContext = new StoreContext())
            {
                IList<StoreProductExT> list = dbContext.Database.SqlQuery<StoreProductExT>(query).ToList();

                //IList<StoreProductExT> list1 = dbContext.StoreProductT.ToList();
                return list;
            }
        }
    }
}
