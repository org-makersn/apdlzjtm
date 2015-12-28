using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreNoticeDac
    {

        private static StoreContext dbContext;
        
        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal List<StoreNoticeT> SelectAllStoreNotice()
        { 
            
            List<StoreNoticeT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StoreNoticeT.ToList();
            }
            return printers;
        }

        /// <summary>
        /// select one Notice By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StoreNoticeT SelectStoreNoticeTById(int no)
        {
            StoreNoticeT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StoreNoticeT.Where(m => m.No == no).FirstOrDefault();
            }

            return printer;
        }

        /// <summary>
        /// Insert Notice
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertStoreNotice(StoreNoticeT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StoreNoticeT.Add(data);
                ret = dbContext.SaveChanges();
            }
            return ret;
        }

        /// <summary>
        /// Update Notice
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int UpdateStoreNotice(StoreNoticeT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                
                //if (dbContext.StoreNoticeT.SingleOrDefault(m => m.No == data.No) != null)
                //{
                    try
                    {
                        dbContext.StoreNoticeT.Attach(data);
                        dbContext.Entry<StoreNoticeT>(data).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        ret = -1;
                    }
                //}
                //else
                //{
                //    ret = -2;
                //    throw new NullReferenceException("The expected original Segment data is not here.");
                //}
            }
            return ret;
        }
        
        
    }
}
