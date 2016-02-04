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
        public List<StoreMaterialT> GetAllStoreMaterial() {
            return new StoreMaterialDac().GetAllStoreMaterial();
        }

        public StoreMaterialT GetStoreMaterialById (int no){
            return new StoreMaterialDac().GetStoreMaterialById(no);
        }

        public bool Update(StoreMaterialT StoreMaterial)
        {
            return new StoreMaterialDac().UpdateStoreMaterial(StoreMaterial);
        }

    }
}
