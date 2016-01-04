using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StorePrinterMaterialDac
    {

        private static StoreContext dbContext;

        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal List<StorePrinterMaterialT> SelectAllStorePrinterMaterial()
        {

            List<StorePrinterMaterialT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StorePrinterMaterialT.ToList();
            }
            return printers;
        }

        /// <summary>
        /// select one StorePrinterMaterial By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StorePrinterMaterialT SelectStorePrinterMaterialTById(int no)
        {
            StorePrinterMaterialT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StorePrinterMaterialT.Where(m => m.No == no).FirstOrDefault();
            }

            return printer;
        }

        /// <summary>
        /// Insert StorePrinterMaterial
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public long InsertStorePrinterMaterial(StorePrinterMaterialT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            long ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StorePrinterMaterialT.Add(data);
                dbContext.SaveChanges();
                ret = data.No;
            }
            return ret;
        }

        /// <summary>
        /// Update StorePrinterMaterial
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int UpdateStorePrinterMaterial(StorePrinterMaterialT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                try
                {
                    dbContext.StorePrinterMaterialT.Attach(data);
                    dbContext.Entry<StorePrinterMaterialT>(data).State = System.Data.Entity.EntityState.Modified;
                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    ret = -1;
                }

            }
            return ret;
        }

    }
}
