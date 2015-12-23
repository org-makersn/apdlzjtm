using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreProductUpdateHistoryBiz
    {
        public List<StoreProductUpdateHistoryT> getAllStorePrinter() {
            return new StoreProductUpdateHistoryDac().SelectAllStoreProductUpdateHistory();
        }
        public StoreProductUpdateHistoryT getStoreProductUpdateHistoryById (int no){
            return new StoreProductUpdateHistoryDac().SelectStoreProductUpdateHistoryTById(no);
        }
        public int add(StoreProductUpdateHistoryT StoreProductUpdateHistory)
        {
            return new StoreProductUpdateHistoryDac().InsertStoreProductUpdateHistory(StoreProductUpdateHistory);
        }
        public int upd(StoreProductUpdateHistoryT StoreProductUpdateHistory)
        {
            return new StoreProductUpdateHistoryDac().UpdateStoreProductUpdateHistory(StoreProductUpdateHistory);
        }

    }
}
