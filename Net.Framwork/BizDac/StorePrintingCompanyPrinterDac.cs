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
        internal List<StorePrinterT> SelectAllStorePirnter()
        { 
            
            List<StorePrinterT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StorePrinterT.ToList();
            }
            return printers;
        }
        internal StorePrinterT SelectStorePrinterTById(int StorePirnterId)
        {
            StorePrinterT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StorePrinterT.Where(m => m.No == StorePirnterId).FirstOrDefault();
            }

            return printer;
        }

        internal int InsertStorePrinter(StorePrinterT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StorePrinterT.Add(data);
                dbContext.SaveChangesAsync();
            }
            return ret;
        }

        internal int UpdateStorePrinter(StorePrinterT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                var originalData = dbContext.StorePrinterT.SingleOrDefault(m => m.No == data.No);
                if (originalData != null)
                {
                    try
                    {
                        //dbContext.StorePrinterT.Attach(data);
                        //dbContext.Entry<StorePrinterT>(data).State = System.Data.Entity.EntityState.Modified;
                        //dbContext.SaveChangesAsync();
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
