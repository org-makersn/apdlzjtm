using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StorePrintingCompanyDac
    {

        private static StoreContext dbContext;
        
        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal List<StorePrintingCompanyT> SelectAllStorePrintingCompany()
        { 
            
            List<StorePrintingCompanyT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StorePrintingCompanyT.ToList();
            }
            return printers;
        }

        /// <summary>
        /// select one StorePrintingCompany By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StorePrintingCompanyT SelectStorePrintingCompanyTById(int no)
        {
            StorePrintingCompanyT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StorePrintingCompanyT.Where(m => m.No == no).FirstOrDefault();
            }

            return printer;
        }

        /// <summary>
        /// Insert StorePrintingCompany
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertStorePrintingCompany(StorePrintingCompanyT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StorePrintingCompanyT.Add(data);
                dbContext.SaveChangesAsync();
            }
            return ret;
        }

        /// <summary>
        /// Update StorePrintingCompany
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int UpdateStorePrintingCompany(StorePrintingCompanyT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                
                if (dbContext.StorePrintingCompanyT.SingleOrDefault(m => m.No == data.No) != null)
                {
                    try
                    {
                        dbContext.StorePrintingCompanyT.Attach(data);
                        dbContext.Entry<StorePrintingCompanyT>(data).State = System.Data.Entity.EntityState.Modified;
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
