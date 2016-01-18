using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreMaterialDac
    {

        private static StoreContext dbContext;
        
        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal List<StoreMaterialT> SelectAllStoreMaterial()
        { 
            
            List<StoreMaterialT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StoreMaterialT.ToList();
            }
            return printers;
        }

        /// <summary>
        /// select one StoreMaterial By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StoreMaterialT SelectStoreMaterialTById(int no)
        {
            StoreMaterialT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StoreMaterialT.Where(m => m.NO == no).FirstOrDefault();
            }

            return printer;
        }

        /// <summary>
        /// Insert StoreMaterial
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertStoreMaterial(StoreMaterialT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StoreMaterialT.Add(data);
                ret = dbContext.SaveChanges();
            }
            return ret;
        }

        /// <summary>
        /// Update StoreMaterial
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int UpdateStoreMaterial(StoreMaterialT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                
                if (dbContext.StoreMaterialT.SingleOrDefault(m => m.NO == data.NO) != null)
                {
                    try
                    {
                        dbContext.StoreMaterialT.Attach(data);
                        dbContext.Entry<StoreMaterialT>(data).State = System.Data.Entity.EntityState.Modified;
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
