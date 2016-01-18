using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Net.Framwork.BizDac
{
    public class StoreOrderDac
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

            string query = @"SELECT
	                        SC.CART_NO,
	                        SC.PRODUCT_DETAIL_NO,
	                        SP.NAME AS PRODUCT_NAME,
	                        SM.NAME,
	                        SP.FILE_SIZE,
	                        SP.MATERIAL_VOLUME,
	                        SP.OBJECT_VOLUME,
	                        SP.SIZE_X,
	                        SP.SIZE_Y,
	                        ISNULL(SPD.TOTAL_PRICE, 0) AS TOTAL_PRICE,
	                        ISNULL(SC.PRODUCT_CNT, 0) AS PRODUCT_CNT,	
	                        ISNULL(SPD.TOTAL_PRICE, 0) * ISNULL(SC.PRODUCT_CNT, 0) AS PAYMENT_PRICE,
	                        3000 AS SHIPPING_PRICE -- 배송비
                        FROM STORE_CART AS SC WITH(NOLOCK) 
                        INNER JOIN STORE_PRODUCT_DETAIL AS SPD WITH(NOLOCK) ON SC.PRODUCT_DETAIL_NO=SPD.NO
                        INNER JOIN STORE_PRODUCT AS SP WITH(NOLOCK) ON SPD.PRODUCT_NO=SP.NO
                        LEFT JOIN STORE_MATERIAL AS SM WITH(NOLOCK) ON SPD.MATERIAL_NO=SM.NO 
                        WHERE SC.ORDER_YN IS NULL
                        AND SC.MEMBER_NO = @memberNo ";

            IEnumerable<OrderInfo> data = dbContext.Database.SqlQuery<OrderInfo>(query);
            orderInfoList = data.ToList();

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
