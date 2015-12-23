using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    class StoreReviewBiz
    {
        public List<StoreReviewT> getAllStorePrinter() {
            return new StoreReviewDac().SelectAllStoreReview();
        }
        public StoreReviewT getStoreReviewById (int no){
            return new StoreReviewDac().SelectStoreReviewTById(no);
        }
        public int add(StoreReviewT StoreReview)
        {
            return new StoreReviewDac().InsertStoreReview(StoreReview);
        }
        public int upd(StoreReviewT StoreReview)
        {
            return new StoreReviewDac().UpdateStoreReview(StoreReview);
        }

    }
}
