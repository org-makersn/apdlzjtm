using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    class StorePrinterBiz
    {
        public List<StorePrinterT> getAllStorePrinter() {
            return new StorePrinterDac().SelectAllStorePrinter();
        }
        public StorePrinterT getStorePrinterById (int no){
            return new StorePrinterDac().SelectStorePrinterById(no);
        }
        public int add(StorePrinterT storePrinter)
        {
            return new StorePrinterDac().InsertStorePrinter(storePrinter);
        }
        public int upd(StorePrinterT storePrinter)
        {
            return new StorePrinterDac().UpdateStorePrinter(storePrinter);
        }

    }
}
