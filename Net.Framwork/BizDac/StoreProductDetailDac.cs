using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreProductDetailDac
    {

        private static StoreContext dbContext;
        
        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal List<StoreProductDetailT> SelectAllStoreProductDetail()
        { 
            
            List<StoreProductDetailT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StoreProductDetailT.ToList();
            }
            return printers;
        }

        /// <summary>
        /// select one Store Product Detail By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StoreProductDetailT SelectStoreProductDetailTById(int no)
        {
            StoreProductDetailT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StoreProductDetailT.Where(m => m.No == no).FirstOrDefault();
            }

            return printer;
        }

        /// <summary>
        /// Insert Store Product Detail
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertStoreProductDetail(StoreProductDetailT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StoreProductDetailT.Add(data);
                ret = dbContext.SaveChanges();
            }
            return ret;
        }

        /// <summary>
        /// Update Store Product Detail
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int UpdateStoreProductDetail(StoreProductDetailT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                
                if (dbContext.StoreProductDetailT.SingleOrDefault(m => m.No == data.No) != null)
                {
                    try
                    {
                        dbContext.StoreProductDetailT.Attach(data);
                        dbContext.Entry<StoreProductDetailT>(data).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        ret = -1;
                    }
                }
                else
                {
                    ret = -2;
                    throw new NullReferenceException("The expected original Segment data is not here.");
                }
            }
            return ret;
        }
        
        
    }
}
