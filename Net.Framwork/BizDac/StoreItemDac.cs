using Net.Framework.Helper;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Net.Framework.BizDac
{
    public class StoreItemDac
    {
        private ISimpleRepository<StoreItemT> _itemRepo = new SimpleRepository<StoreItemT>();
        
        /// <summary>
        /// 아이템 조회
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public StoreItemT GetItemByNo(long no)
        {
            return _itemRepo.First(m => m.No == no);
        }

        /// <summary>
        /// insert
        /// </summary>
        /// <param name="data"></param>
        /// <param name="paramDelNo"></param>
        /// <returns></returns>
        public long AddItem(StoreItemT data)
        {
            long identity = 0;
            bool ret = _itemRepo.Insert(data);
            
            if (ret)
            {
                identity = _itemRepo.First(m => m.Temp == data.Temp).No;
            }
            return identity;
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateItem(StoreItemT data)
        {
            return _itemRepo.Update(data);
        }
    }
}
