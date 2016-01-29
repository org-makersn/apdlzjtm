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

        #region 
        public List<StoreOrderDetailT> GetStoreOrderDetailListByCartNo(string cartNo)
        {
            List<StoreOrderDetailT> data = new List<StoreOrderDetailT>();

            return data;
        }
        #endregion

        #region GetNewOrderNo - 새로운 주문번호 생성
        /// <summary>
        /// 새로운 주문번호 생성
        /// </summary>
        /// <returns></returns>
        public string GetNewOrderNo()
        {
            return new StoreOrderDac().GetNewOrderNo();
        }
        #endregion

        #region InsertOrderInfo - 주문마스터 저장
        /// <summary>
        /// 주문마스터 저장
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Int64 InsertOrderInfo(StoreOrderT data)
        {
            return new StoreOrderDac().InsertOrderInfo(data);
        }
        #endregion

        #region InsertOrderDetailInfo - 주문상세 저장
        /// <summary>
        /// 주문상세 저장
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public int InsertOrderDetailInfo(List<StoreOrderDetailT> datas)
        {
            return new StoreOrderDac().InsertOrderDetailInfo(datas);
        }
        #endregion
    }
}
