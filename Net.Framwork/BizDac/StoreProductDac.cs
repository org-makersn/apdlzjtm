using Net.Framework;
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
        internal List<StoreProductT> SelectAllStoreProduct()
        { 
            
            List<StoreProductT> printers = null;
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
        internal int InsertStoreProduct(StoreProductT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StoreProductT.Add(data);
                ret = dbContext.SaveChanges();
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
                
                if (dbContext.StoreProductT.SingleOrDefault(m => m.No == data.No) != null)
                {
                    try
                    {
                        dbContext.StoreProductT.Attach(data);
                        dbContext.Entry<StoreProductT>(data).State = System.Data.Entity.EntityState.Modified;
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
