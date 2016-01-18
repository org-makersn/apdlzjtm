using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StorePrinterBiz
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<StorePrinterT> getAllStorePrinter() {
            return new StorePrinterDac().SelectAllStorePrinter();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public StorePrinterT getStorePrinterById (int no){
            return new StorePrinterDac().SelectStorePrinterById(no);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storePrinter"></param>
        /// <returns></returns>
        public int add(StorePrinterT storePrinter)
        {
            return new StorePrinterDac().InsertStorePrinter(storePrinter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storePrinter"></param>
        /// <returns></returns>
        public int upd(StorePrinterT storePrinter)
        {
            return new StorePrinterDac().UpdateStorePrinter(storePrinter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IList<StorePrinterT> getSearchedStorePrinter(string text)
        {
            return new StorePrinterDac().SelectAllStorePrinter().Where(w => w.NAME.Contains(text)).ToList();
        }

    }
}
