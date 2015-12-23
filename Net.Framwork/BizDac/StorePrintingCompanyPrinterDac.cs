using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StorePrintingCompanyPrinterDac
    {

        private static StoreContext dbContext;
        
        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal List<StorePrintingCompanyPrinterT> SelectAllStorePrintingCompanyPrinter()
        {

            List<StorePrintingCompanyPrinterT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StorePrintingCompanyPrinterT.ToList();
            }
            return printers;
        }
        internal StorePrintingCompanyPrinterT SelectStorePrintingCompanyPrinterTById(int StorePirnterId)
        {
            StorePrintingCompanyPrinterT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StorePrintingCompanyPrinterT.Where(m => m.No == StorePirnterId).FirstOrDefault();
            }

            return printer;
        }

        internal int InsertStorePrintingCompanyPrinter(StorePrintingCompanyPrinterT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StorePrintingCompanyPrinterT.Add(data);
                dbContext.SaveChangesAsync();
            }
            return ret;
        }

        internal int UpdateStorePrintingCompanyPrinter(StorePrintingCompanyPrinterT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                var originalData = dbContext.StorePrintingCompanyPrinterT.SingleOrDefault(m => m.No == data.No);
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
