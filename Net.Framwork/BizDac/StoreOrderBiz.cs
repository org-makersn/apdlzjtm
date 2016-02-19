using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.Framework.Util;

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

        #region GetStoreOrderGroupItemlListByMemberNo - 판매자별 카트 주문 리스트
        /// <summary>
        /// 판매자별 카트 주문 리스트
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public List<OrderGroupItem> GetStoreOrderGroupItemlListByMemberNo(int memberNo)
        {
            List<OrderGroupItem> data = new List<OrderGroupItem>();
            data = new StoreOrderDac().GetOrderGroupItemListByMemberNo(memberNo);

            return data;
        }
        #endregion

        #region GetStoreOrderDetailListByCartNo
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cartNo"></param>
        /// <returns></returns>
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

        #region GetContractListByCondition - 구매내역
        /// <summary>
        /// 구매내역
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="startDt"></param>
        /// <param name="endDt"></param>
        /// <returns></returns>
        public List<StoreOrderT> GetContractListByCondition(int memberNo, DateTime startDt, DateTime endDt)
        {
            List<StoreOrderT> list = new StoreOrderDac().GetContractListByCondition(memberNo, startDt, endDt);

            foreach (StoreOrderT item in list)
            {
                // 주문상태
                if(item.ORDER_STATUS.Equals(StringEnum.GetValue(OrderStatus.Complete)))
                {
                    item.ORDER_STATUS = StringEnum.GetDescription(OrderStatus.Complete);
                }
                else if (item.ORDER_STATUS.Equals(StringEnum.GetValue(OrderStatus.Cancel)))
                {
                    item.ORDER_STATUS = StringEnum.GetDescription(OrderStatus.Cancel);
                }

                // 결제상태
                if (item.PAYMENT_STATUS.Equals(StringEnum.GetValue(PaymentStatus.Complete)))
                {
                    item.PAYMENT_STATUS = StringEnum.GetDescription(PaymentStatus.Complete);
                }
                else if (item.PAYMENT_STATUS.Equals(StringEnum.GetValue(PaymentStatus.Cancel)))
                {
                    item.PAYMENT_STATUS = StringEnum.GetDescription(PaymentStatus.Cancel);
                }
                else if (item.PAYMENT_STATUS.Equals(StringEnum.GetValue(PaymentStatus.Waiting)))
                {
                    item.PAYMENT_STATUS = StringEnum.GetDescription(PaymentStatus.Waiting);
                }
                else if (item.PAYMENT_STATUS.Equals(StringEnum.GetValue(PaymentStatus.InProgress)))
                {
                    item.PAYMENT_STATUS = StringEnum.GetDescription(PaymentStatus.InProgress);
                }

                // 배송상태
                if (item.SHIPPING_STATUS != null)
                {                    
                    if (item.SHIPPING_STATUS.Equals(StringEnum.GetValue(ShippingStatus.Complete)))
                    {
                        item.SHIPPING_STATUS = StringEnum.GetDescription(ShippingStatus.Complete);
                    }
                    else if (item.SHIPPING_STATUS.Equals(StringEnum.GetValue(ShippingStatus.Cancel)))
                    {
                        item.SHIPPING_STATUS = StringEnum.GetDescription(ShippingStatus.Cancel);
                    }
                    else if (item.SHIPPING_STATUS.Equals(StringEnum.GetValue(ShippingStatus.Waiting)))
                    {
                        item.SHIPPING_STATUS = StringEnum.GetDescription(ShippingStatus.Waiting);
                    }
                    else if (item.SHIPPING_STATUS.Equals(StringEnum.GetValue(ShippingStatus.InProgress)))
                    {
                        item.SHIPPING_STATUS = StringEnum.GetDescription(ShippingStatus.InProgress);
                    }
                }
            }

            return list;

        }
        #endregion

        #region GetContractInfoListByCondition - 상세구매내역
        /// <summary>
        /// 상세구매내역
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="startDt"></param>
        /// <param name="endDt"></param>
        /// <returns></returns>
        public List<ContractDetail> GetContractDetailListByCondition(int memberNo, DateTime startDt, DateTime endDt)
        {
            return new StoreOrderDac().GetContractDetailListByCondition(memberNo, startDt, endDt);
        }
        #endregion

        #region GetContractDetailListByOid - OID에 따른 구매상세내역
        /// <summary>
        /// OID에 따른 구매상세내역
        /// </summary>
        /// <param name="OId"></param>
        /// <returns></returns>
        public List<ContractDetail> GetContractDetailListByOid(string oId)
        {
            List<ContractDetail> list = new StoreOrderDac().GetContractDetailListByOid(oId);

            foreach (ContractDetail item in list)
            {
                if (item.PRINTING_STATUS.Equals(StringEnum.GetValue(PrintingStatus.Complete)))
                {
                    item.PRINTING_STATUS = StringEnum.GetDescription(PrintingStatus.Complete);
                }
                else if (item.PRINTING_STATUS.Equals(StringEnum.GetValue(PrintingStatus.Cancel)))
                {
                    item.PRINTING_STATUS = StringEnum.GetDescription(PrintingStatus.Cancel);
                }
                else if (item.PRINTING_STATUS.Equals(StringEnum.GetValue(PrintingStatus.Waiting)))
                {
                    item.PRINTING_STATUS = StringEnum.GetDescription(PrintingStatus.Waiting);
                }
                else if (item.PRINTING_STATUS.Equals(StringEnum.GetValue(PrintingStatus.InProgress)))
                {
                    item.PRINTING_STATUS = StringEnum.GetDescription(PrintingStatus.InProgress);
                }
            }

            return list;
        }
        #endregion

        #region GetTradeId - 거래아이디 Get
        /// <summary>
        /// 거래아이디 Get
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public string GetTradeId(string oid)
        {          
            return new StoreOrderDac().GetTradeIdByOid(oid);
        }
        #endregion

        #region UpdateOrderStatus - 주문상태 업데이트
        /// <summary>
        /// 주문상태 업데이트
        /// </summary>
        /// <param name="oId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateOrderStatus(string oId, string status)
        {
            return new StoreOrderDac().updateOrderStatus(oId, status);
        }
        #endregion

        #region UpdatePaymentStatus - 결제상태 업데이트
        /// <summary>
        /// 결제상태 업데이트
        /// </summary>
        /// <param name="oId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdatePaymentStatus(string oId, string status)
        {
            return new StoreOrderDac().updatePaymentStatus(oId, status);
        }
        #endregion

        #region UpdatePrintingStatus - 출력상태 업데이트
        /// <summary>
        /// 출력상태 업데이트
        /// </summary>
        /// <param name="orderDetailNo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdatePrintingStatus(Int64 orderDetailNo, string status)
        {
            return new StoreOrderDac().updatePrintingStatus(orderDetailNo, status);
        }
        #endregion

        #region UpdateShippingStatus - 배송상태 업데이트
        /// <summary>
        /// 배송상태 업데이트
        /// </summary>
        /// <param name="oId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateShippingStatus(string oId, string status)
        {
            return new StoreOrderDac().updateShippingStatus(oId, status);
        }
        #endregion

        #region InsertOrderCancelInfo - 주문취소 저장
        /// <summary>
        /// 주문취소 저장
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Int64 InsertOrderCancelInfo(StorePaymentCancelT data)
        {
            return new StoreOrderDac().InsertOrderCancelInfo(data);
        }
        #endregion

        #region GetOrderListForPaymentWaiting - 결제대기건 조회
        /// <summary>
        /// 결제대기건 조회
        /// </summary>
        /// <returns></returns>
        public List<StoreOrderT> GetOrderListForPaymentWaiting()
        {
            return new StoreOrderDac().GetOrderListForPaymentWaiting();
        }
        #endregion
    }
}
