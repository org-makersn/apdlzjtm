using System;
using Net.Framework;
using Net.Framwork.Helper;
using Net.Framework.StoreModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Net.SqlTools;

namespace Net.Framwork.BizDac
{
    public class StoreOrderDac : DacBase
    {
        #region 전역변수
        private static StoreContext dbContext;
        #endregion

        #region InsertOrder - 주문서(결제요청) 저장
        public void InsertOrderInfo()
        {
 
        }
        #endregion

        #region InsertOrderDetail - 주문상세내역 저장
        public void InsertOrderDetailInfo()
        {

        }
        #endregion

        #region InsertShippingInfo - 배송지 저장
        public void InsertShippingInfo()
        {

        }
        #endregion

        #region InsertPaymentResultInfo - 결제요청결과 저장
        public void InsertPaymentResultInfo()
        {
 
        }
        #endregion

        #region GetOrderList - 주문상세 리스트
        internal List<OrderInfo> GetOrderList(int memberNo)
        {
            List<OrderInfo> orderInfoList = new List<OrderInfo>();
            string query = DacHelper.GetSqlCommand("StoreOrder.SelectOrderList_S");

            using (dbContext = new StoreContext())
            {
                orderInfoList = dbContext.Database.SqlQuery<OrderInfo>(query,
                    new SqlParameter("MEMBER_NO", memberNo)).ToList();
            }

            return orderInfoList;
        }
        #endregion

        #region GetOrderInfo - 주문서 확인
        public StoreOrderT GetOrderInfo()
        {
            StoreOrderT storeOrderT = new StoreOrderT();

            return storeOrderT;
        }
        #endregion
    }
}
