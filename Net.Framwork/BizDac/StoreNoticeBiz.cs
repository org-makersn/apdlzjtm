using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreNoticeBiz
    {
        public List<StoreNoticeT> SelectAllStoreNotice()
        {
            return new StoreNoticeDac().SelectAllStoreNotice();
        }
        public StoreNoticeT getStoreNoticeById (int no){
            return new StoreNoticeDac().SelectStoreNoticeTById(no);
        }
        public int add(StoreNoticeT StoreNotice)
        {
            return new StoreNoticeDac().InsertStoreNotice(StoreNotice);
        }
        public int upd(StoreNoticeT StoreNotice)
        {
            return new StoreNoticeDac().UpdateStoreNotice(StoreNotice);
        }

    }
}
