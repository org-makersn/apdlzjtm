using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StorePrintingTypeBiz
    {
        public List<StorePrintingTypeT> getAllStorePrinter() {
            return new StorePrintingTypeDac().SelectAllStorePrintingType();
        }
        public StorePrintingTypeT getStorePrintingTypeById (int no){
            return new StorePrintingTypeDac().SelectStorePrintingTypeTById(no);
        }
        public int add(StorePrintingTypeT StorePrintingType)
        {
            return new StorePrintingTypeDac().InsertStorePrintingType(StorePrintingType);
        }
        public int upd(StorePrintingTypeT StorePrintingType)
        {
            return new StorePrintingTypeDac().UpdateStorePrintingType(StorePrintingType);
        }

    }
}
