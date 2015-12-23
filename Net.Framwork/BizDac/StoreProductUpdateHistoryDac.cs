using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreProductUpdateHistoryDac
    {

        private static StoreContext dbContext;
        
        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal List<StoreProductUpdateHistoryT> SelectAllStoreProductUpdateHistory()
        { 
            
            List<StoreProductUpdateHistoryT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StoreProductUpdateHistoryT.ToList();
            }
            return printers;
        }

        /// <summary>
        /// select one Product Update History By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StoreProductUpdateHistoryT SelectStoreProductUpdateHistoryTById(int no)
        {
            StoreProductUpdateHistoryT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StoreProductUpdateHistoryT.Where(m => m.No == no).FirstOrDefault();
            }

            return printer;
        }

        /// <summary>
        /// Insert Product Update History
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertStoreProductUpdateHistory(StoreProductUpdateHistoryT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StoreProductUpdateHistoryT.Add(data);
                dbContext.SaveChangesAsync();
            }
            return ret;
        }

        /// <summary>
        /// Update Product Update History
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int UpdateStoreProductUpdateHistory(StoreProductUpdateHistoryT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                
                if (dbContext.StoreProductUpdateHistoryT.SingleOrDefault(m => m.No == data.No) != null)
                {
                    try
                    {
                        dbContext.StoreProductUpdateHistoryT.Attach(data);
                        dbContext.Entry<StoreProductUpdateHistoryT>(data).State = System.Data.Entity.EntityState.Modified;
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
