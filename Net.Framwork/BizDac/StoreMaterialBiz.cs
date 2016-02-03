using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreMaterialBiz
    {
        public List<StoreMaterialT> getAllStoreMaterial() {
            return new StoreMaterialDac().SelectAllStoreMaterial();
        }

        public StoreMaterialT getStoreMaterialById (int no){
            return new StoreMaterialDac().SelectStoreMaterialTById(no);
        }

        public int add(StoreMaterialT StoreMaterial)
        {
            return new StoreMaterialDac().InsertStoreMaterial(StoreMaterial);
        }

        public int upd(StoreMaterialT StoreMaterial)
        {
            return new StoreMaterialDac().UpdateStoreMaterial(StoreMaterial);
        }

    }
}
