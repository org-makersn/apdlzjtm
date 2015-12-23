using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    class StorePrintingCompanyBiz
    {
        public List<StorePrintingCompanyT> getAllStorePrinter() {
            return new StorePrintingCompanyDac().SelectAllStorePrintingCompany();
        }
        public StorePrintingCompanyT getStorePrintingCompanyById (int no){
            return new StorePrintingCompanyDac().SelectStorePrintingCompanyTById(no);
        }
        public int add(StorePrintingCompanyT StorePrintingCompany)
        {
            return new StorePrintingCompanyDac().InsertStorePrintingCompany(StorePrintingCompany);
        }
        public int upd(StorePrintingCompanyT StorePrintingCompany)
        {
            return new StorePrintingCompanyDac().UpdateStorePrintingCompany(StorePrintingCompany);
        }

    }
}
