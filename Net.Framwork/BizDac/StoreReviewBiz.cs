using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreReviewBiz
    {
        public List<StoreReviewT> SelectAllStoreReview()
        {
            return new StoreReviewDac().SelectAllStoreReview();
        }
        public StoreReviewT getStoreReviewById (int no){
            return new StoreReviewDac().SelectStoreReviewTById(no);
        }
        public List<StoreReviewT> getStoreReviewTByParentId(int no)
        {
            return new StoreReviewDac().getStoreReviewTByParentId(no);
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
