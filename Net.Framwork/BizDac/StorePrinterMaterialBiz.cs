using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StorePrinterMaterialBiz
    {
        public List<StorePrinterMaterialT> getAllStorePrinterMaterial() {
            return new StorePrinterMaterialDac().SelectAllStorePrinterMaterial();
        }
        public StorePrinterMaterialT getStorePrinterMaterialById (int no){
            return new StorePrinterMaterialDac().SelectStorePrinterMaterialTById(no);
        }
        public long add(StorePrinterMaterialT StorePrinterMaterial)
        {
            return new StorePrinterMaterialDac().InsertStorePrinterMaterial(StorePrinterMaterial);
        }
        public int upd(StorePrinterMaterialT StorePrinterMaterial)
        {
            return new StorePrinterMaterialDac().UpdateStorePrinterMaterial(StorePrinterMaterial);
        }

    }
}
