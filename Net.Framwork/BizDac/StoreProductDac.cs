using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreProductDac : DacBase
    {
        
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
                    .Where(p =>( p.CertiFicateStatus == certificateStatus) && p.Name.Contains(query) )
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


        public void SelectProductTest()
        {
            string query = GetSqlCommand("StoreProduct.SelectProductList_S");
            using (dbContext = new StoreContext())
            {
                var test = dbContext.Database.ExecuteSqlCommand(query);
                //var test dbContext.Database.SqlQuery<StoreProductT>(query).ToList();
            }
        }
    }
}
