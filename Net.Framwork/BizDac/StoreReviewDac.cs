using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreReviewDac
    {

        private static StoreContext dbContext;
        
        /// <summary>
        /// select Store Review data
        /// </summary>
        /// <returns></returns>
        internal List<StoreReviewT> SelectAllStoreReview()
        { 
            List<StoreReviewT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StoreReviewT.ToList();
            }
            return printers;
        }

        /// <summary>
        /// select one Store Review By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StoreReviewT SelectStoreReviewTById(int no)
        {
            StoreReviewT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StoreReviewT.Where(m => m.No == no).FirstOrDefault();
            }

            return printer;
        }

        /// <summary>
        /// Insert Store Review
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertStoreReview(StoreReviewT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StoreReviewT.Add(data);
                ret = dbContext.SaveChanges();
            }
            return ret;
        }

        /// <summary>
        /// Update Store Review
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int UpdateStoreReview(StoreReviewT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                
                if (dbContext.StoreReviewT.SingleOrDefault(m => m.No == data.No) != null)
                {
                    try
                    {
                        dbContext.StoreReviewT.Attach(data);
                        dbContext.Entry<StoreReviewT>(data).State = System.Data.Entity.EntityState.Modified;
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
