using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreOrderBiz
    {
        #region GetStoreOrderListByMemberNo - 주문리스트 Get
        /// <summary>
        /// GetStoreOrderListByMemberNo - 주문리스트 Get
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public List<OrderInfo> GetStoreOrderListByMemberNo(int memberNo)
        {
            List<OrderInfo> data = new List<OrderInfo>();
            data = new StoreOrderDac().GetOrderList(memberNo);

            return data;
        }
        #endregion
    }
}
