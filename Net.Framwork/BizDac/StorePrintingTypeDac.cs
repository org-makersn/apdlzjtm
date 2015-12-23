using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StorePrintingTypeDac
    {

        private static StoreContext dbContext;
        
        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal List<StorePrintingTypeT> SelectAllStorePrintingType()
        { 
            
            List<StorePrintingTypeT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StorePrintingTypeT.ToList();
            }
            return printers;
        }

        /// <summary>
        /// select one Printing Type By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StorePrintingTypeT SelectStorePrintingTypeTById(int no)
        {
            StorePrintingTypeT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StorePrintingTypeT.Where(m => m.No == no).FirstOrDefault();
            }

            return printer;
        }

        /// <summary>
        /// Insert Printing Type
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertStorePrintingType(StorePrintingTypeT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StorePrintingTypeT.Add(data);
                ret = dbContext.SaveChanges();
            }
            return ret;
        }

        /// <summary>
        /// Update Printing Type
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int UpdateStorePrintingType(StorePrintingTypeT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                
                if (dbContext.StorePrintingTypeT.SingleOrDefault(m => m.No == data.No) != null)
                {
                    try
                    {
                        dbContext.StorePrintingTypeT.Attach(data);
                        dbContext.Entry<StorePrintingTypeT>(data).State = System.Data.Entity.EntityState.Modified;
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
