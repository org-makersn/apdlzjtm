using System;
using Net.Framework;
using Net.Framework.Helper;
using Net.Framework.StoreModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Net.SqlTools;
using Net.Framework.Util;

namespace Net.Framwork.BizDac
{
    public class StoreOrderDac : DacBase
    {
        #region 전역변수
        private static StoreContext dbContext;
        #endregion

        #region InsertOrder - 주문서(결제요청) 저장
        /// <summary>
        /// 주문서(결제요청) 저장
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal Int64 InsertOrderInfo(StoreOrderT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            using (dbContext = new StoreContext())
            {
                dbContext.StoreOrderT.Add(data);
                dbContext.SaveChanges();
                dbContext.Entry(data).GetDatabaseValues();
            }

            return data.No;
        }
        #endregion

        #region InsertOrderDetail - 주문상세내역 저장
        /// <summary>
        /// 주문상세내역 저장
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertOrderDetailInfo(List<StoreOrderDetailT> datas)
        {
            if (datas == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                foreach (StoreOrderDetailT data in datas)
                {
                    dbContext.StoreOrderDetailT.Add(data);
                    ret += dbContext.SaveChanges();
                }
            }
            return ret;
        }
        #endregion

        #region GetOrderList - 주문상세 리스트
        /// <summary>
        /// 주문상세 리스트
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
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

        #region GetNewOrderNo - 새로운 주문번호 생성
        /// <summary>
        /// 새로운 주문번호 생성
        /// </summary>
        /// <returns></returns>
        internal string GetNewOrderNo()
        {
            List<string> newOrderNo;

            string query = DacHelper.GetSqlCommand("StoreOrder.GetNewOrderNo");

            using (dbContext = new StoreContext())
            {
                newOrderNo = dbContext.Database.SqlQuery<string>(query).ToList();
            }

            return newOrderNo[0];
        }
        #endregion

        #region GetOrderMasterListByCondition - 구매내역
        /// <summary>
        /// 구매내역
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="startDt"></param>
        /// <param name="endDt"></param>
        /// <returns></returns>
        internal List<StoreOrderT> GetContractListByCondition(int memberNo, DateTime startDt, DateTime endDt)
        {
            List<StoreOrderT> storeOrderMasterList = new List<StoreOrderT>();

            using (dbContext = new StoreContext())
            {
                storeOrderMasterList = dbContext.StoreOrderT.Where(p => p.MEMBER_NO == memberNo && p.ORDER_DATE >= startDt && p.ORDER_DATE <= endDt).ToList();
                storeOrderMasterList = storeOrderMasterList.OrderByDescending(p => p.ORDER_DATE).ToList();
            }
            return storeOrderMasterList;
 
        }
        #endregion

        #region GetContractDetailListByCondition - 상세구매내역
        /// <summary>
        /// 구매내역
        /// </summary>
        /// <param name="memberNo"></param>
        /// <param name="startDt"></param>
        /// <param name="endDt"></param>
        /// <returns></returns>
        internal List<ContractDetail> GetContractDetailListByCondition(int memberNo, DateTime startDt, DateTime endDt)
        {
            List<ContractDetail> list = new List<ContractDetail>();
            string query = DacHelper.GetSqlCommand("StoreOrder.SelectContractDetailListByCondition_S");

            using (dbContext = new StoreContext())
            {
                list = dbContext.Database.SqlQuery<ContractDetail>(query,
                    new SqlParameter("MEMBER_NO", memberNo),
                    new SqlParameter("ST_DT", startDt),
                    new SqlParameter("ED_DT", endDt)
                    ).ToList();
            }

            return list;
        }
        #endregion

        #region GetContractDetailListByOid - OID에 따른 구매상세내역
        /// <summary>
        /// OID에 따른 구매상세내역
        /// </summary>
        /// <param name="oId"></param>
        /// <returns></returns>
        internal List<ContractDetail> GetContractDetailListByOid(string oId)
        {
            List<ContractDetail> list = new List<ContractDetail>();
            string query = DacHelper.GetSqlCommand("StoreOrder.SelectContractDetailListByOid_S");

            using (dbContext = new StoreContext())
            {
                list = dbContext.Database.SqlQuery<ContractDetail>(query,
                    new SqlParameter("OID", oId)
                    ).ToList();
            }

            return list;
        }
        #endregion

        #region GetTradeId - 거래아이디 Get
        /// <summary>
        /// 주문취소
        /// </summary>
        /// <param name="oId"></param>
        /// <returns></returns>
        internal string GetTradeIdByOid(string oId)
        {
            StorePaymentHistoryT data = new StorePaymentHistoryT();
            using (dbContext = new StoreContext())
            {
                data = dbContext.StorePaymentHistoryT.Where(p => p.M_OID == oId).FirstOrDefault();
            }

            return data.TID;
        }
        #endregion

        #region updateOrderStatus - 주문상태 업데이트
        /// <summary>
        /// 주문상태 업데이트
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        internal int updateOrderStatus(string oId, string status)
        {
            if (oId == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                StoreOrderT data = dbContext.StoreOrderT.Where(p => p.OID == oId).FirstOrDefault();

                if (data != null)
                {
                    try
                    {
                        data.ORDER_STATUS = status;
                        dbContext.StoreOrderT.Attach(data);
                        dbContext.Entry<StoreOrderT>(data).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    ret = -2;
                    throw new NullReferenceException("The expected original Segment data is not here.");
                }

            }
            return ret;
        }
        #endregion

        #region updatePaymentStatus - 결제상태 업데이트
        /// <summary>
        /// 결제상태 업데이트
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        internal int updatePaymentStatus(string oId, string status)
        {
            if (oId == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                StoreOrderT data = dbContext.StoreOrderT.Where(p => p.OID == oId).FirstOrDefault();

                if (data != null)
                {
                    try
                    {
                        data.PAYMENT_STATUS = status;
                        dbContext.StoreOrderT.Attach(data);
                        dbContext.Entry<StoreOrderT>(data).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    ret = -2;
                    throw new NullReferenceException("The expected original Segment data is not here.");
                }

            }
            return ret;
        }
        #endregion

        #region updatePrintingStatus - 출력상태 업데이트
        /// <summary>
        /// 출력상태 업데이트
        /// </summary>
        /// <param name="orderDetailNo"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        internal int updatePrintingStatus(Int64 orderDetailNo, string status)
        {
            if (orderDetailNo == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                StoreOrderDetailT data = dbContext.StoreOrderDetailT.Where(p => p.No == orderDetailNo).FirstOrDefault();

                if (data != null)
                {
                    try
                    {
                        data.PRINTING_STATUS = status;
                        dbContext.StoreOrderDetailT.Attach(data);
                        dbContext.Entry<StoreOrderDetailT>(data).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    ret = -2;
                    throw new NullReferenceException("The expected original Segment data is not here.");
                }

            }
            return ret;
        }
        #endregion

        #region updateShippingStatus - 배송상태 업데이트
        /// <summary>
        /// 배송상태 업데이트
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        internal int updateShippingStatus(string oId, string status)
        {
            if (oId == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                StoreOrderT data = dbContext.StoreOrderT.Where(p => p.OID == oId).FirstOrDefault();

                if (data != null)
                {
                    try
                    {
                        data.SHIPPING_STATUS = status;
                        dbContext.StoreOrderT.Attach(data);
                        dbContext.Entry<StoreOrderT>(data).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    ret = -2;
                    throw new NullReferenceException("The expected original Segment data is not here.");
                }

            }
            return ret;
        }
        #endregion

        #region InsertOrderCancelInfo - 주문취소 저장
        /// <summary>
        /// 주문취소 저장
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal Int64 InsertOrderCancelInfo(StorePaymentCancelT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            using (dbContext = new StoreContext())
            {
                dbContext.StorePaymentCancelT.Add(data);
                dbContext.SaveChanges();
                dbContext.Entry(data).GetDatabaseValues();
            }

            return data.No;
        }
        #endregion

        #region GetOrderListForPaymentWaiting - 결제대기건 조회
        /// <summary>
        /// 결제대기건 조회
        /// </summary>
        /// <returns></returns>
        internal List<StoreOrderT> GetOrderListForPaymentWaiting()
        {
            List<StoreOrderT> data = new List<StoreOrderT>();

            using (dbContext = new StoreContext())
            {
                data = dbContext.StoreOrderT.Where(p => p.ORDER_STATUS.Equals("00") && 
                                                        p.PAYMENT_STATUS.Equals("02")
                                                   ).ToList();
            }

            return data;
        }
        #endregion
    }
}
