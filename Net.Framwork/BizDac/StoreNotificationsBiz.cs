using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreNotificationsBiz
    {
        public List<StoreNotificationsT> SelectAllStoreNotice()
        {
            return new StoreNotificationsDac().SelectAllStoreNotice();
        }

        public StoreNotificationsT getStoreNoticeById (int no){
            return new StoreNotificationsDac().SelectStoreNoticeTById(no);
        }

        public int add(StoreNotificationsT StoreNotice)
        {
            return new StoreNotificationsDac().InsertStoreNotice(StoreNotice);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StoreNotice"></param>
        /// <returns></returns>
        public int upd(StoreNotificationsT StoreNotice)
        {
            return new StoreNotificationsDac().UpdateStoreNotice(StoreNotice);
        }

    }
}
