using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    class StoreLikesBiz
    {
        public List<StoreLikesT> getAllStorePrinter() {
            return new StoreLikesDac().SelectAllStoreLikes();
        }
        public StoreLikesT getStoreLikesById (int no){
            return new StoreLikesDac().SelectStoreLikesTById(no);
        }
        public int add(StoreLikesT StoreLikes)
        {
            return new StoreLikesDac().InsertStoreLikes(StoreLikes);
        }
        public int upd(StoreLikesT StoreLikes)
        {
            return new StoreLikesDac().UpdateStoreLikes(StoreLikes);
        }

    }
}
