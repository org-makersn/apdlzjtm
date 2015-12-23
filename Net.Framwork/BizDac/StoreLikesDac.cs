using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreLikesDac
    {

        private static StoreContext dbContext;
        
        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal List<StoreLikesT> SelectAllStoreLikes()
        { 
            
            List<StoreLikesT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StoreLikesT.ToList();
            }
            return printers;
        }

        /// <summary>
        /// select one likes By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StoreLikesT SelectStoreLikesTById(int no)
        {
            StoreLikesT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StoreLikesT.Where(m => m.No == no).FirstOrDefault();
            }

            return printer;
        }

        /// <summary>
        /// Insert likes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertStoreLikes(StoreLikesT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StoreLikesT.Add(data);
                ret = dbContext.SaveChanges();
            }
            return ret;
        }

        /// <summary>
        /// Update likes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int UpdateStoreLikes(StoreLikesT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                
                if (dbContext.StoreLikesT.SingleOrDefault(m => m.No == data.No) != null)
                {
                    try
                    {
                        dbContext.StoreLikesT.Attach(data);
                        dbContext.Entry<StoreLikesT>(data).State = System.Data.Entity.EntityState.Modified;
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
