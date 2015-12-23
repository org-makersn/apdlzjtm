using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StorePrintingCompanyPrinterBiz
    {
        public List<StorePrintingCompanyPrinterT> getAllStorePrinter()
        {
            return new StorePrintingCompanyPrinterDac().SelectAllStorePrintingCompanyPrinter();
        }
        public StorePrintingCompanyPrinterT getStorePrinterById(int no)
        {
            return new StorePrintingCompanyPrinterDac().SelectStorePrintingCompanyPrinterTById(no);
        }
        public int add(StorePrintingCompanyPrinterT storePrinter)
        {
            return new StorePrintingCompanyPrinterDac().InsertStorePrintingCompanyPrinter(storePrinter);
        }
        public int upd(StorePrintingCompanyPrinterT storePrinter)
        {
            return new StorePrintingCompanyPrinterDac().UpdateStorePrintingCompanyPrinter(storePrinter);
        }

    }
}
