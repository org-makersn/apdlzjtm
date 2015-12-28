using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreProductBiz
    {
        public List<StoreProductT> getAllStorePrinter() {
            return new StoreProductDac().SelectAllStoreProduct();
        }
        public StoreProductT getStoreProductById (int no){
            return new StoreProductDac().SelectStoreProductTById(no);
        }
        public int add(StoreProductT StoreProduct)
        {
            return new StoreProductDac().InsertStoreProduct(StoreProduct);
        }
        public int upd(StoreProductT StoreProduct)
        {
            return new StoreProductDac().UpdateStoreProduct(StoreProduct);
        }
        public List<StoreProductT> searchProductWithCertification(int certificateStatus , string query){
            return new StoreProductDac().SelectProductWithCertification(certificateStatus, query);

        }

    }
}
