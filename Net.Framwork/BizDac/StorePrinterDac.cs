using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StorePrinterDac
    {

        private static StoreContext dbContext;
        
        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal IList<StorePrinterT> SelectAllStorePrinter()
        { 
            
            List<StorePrinterT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StorePrinterT.ToList();
            }
            return printers;
        }

        /// <summary>
        /// select one StorePrinter By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StorePrinterT SelectStorePrinterById(int no)
        {
            StorePrinterT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StorePrinterT.Where(m => m.NO == no).FirstOrDefault();
            }

            return printer;
        }

        /// <summary>
        /// Insert StorePrinter
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertStorePrinter(StorePrinterT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StorePrinterT.Add(data);
                ret = dbContext.SaveChanges();
            }
            return ret;
        }

        /// <summary>
        /// Update StorePrinter
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int UpdateStorePrinter(StorePrinterT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                
                if (dbContext.StorePrinterT.SingleOrDefault(m => m.NO == data.NO) != null)
                {
                    try
                    {
                        dbContext.StorePrinterT.Attach(data);
                        dbContext.Entry<StorePrinterT>(data).State = System.Data.Entity.EntityState.Modified;
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
