using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Net.Framework.StoreModel;

namespace Makersn.BizDac
{
    public class StoreOrder
    {
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

        #region GetOrderList - 주문리스트
        public List<StoreOrderT> GetOrderList(string cartNo)
        {
            List<StoreOrderT> storeOrderT = new List<StoreOrderT>();

            return storeOrderT;
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
