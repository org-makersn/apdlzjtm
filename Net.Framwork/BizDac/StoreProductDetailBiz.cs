using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    class StoreProductDetailBiz
    {
        public List<StoreProductDetailT> getAllStorePrinter() {
            return new StoreProductDetailDac().SelectAllStoreProductDetail();
        }
        public StoreProductDetailT getStoreProductDetailById (int no){
            return new StoreProductDetailDac().SelectStoreProductDetailTById(no);
        }
        public int add(StoreProductDetailT StoreProductDetail)
        {
            return new StoreProductDetailDac().InsertStoreProductDetail(StoreProductDetail);
        }
        public int upd(StoreProductDetailT StoreProductDetail)
        {
            return new StoreProductDetailDac().UpdateStoreProductDetail(StoreProductDetail);
        }

    }
}
