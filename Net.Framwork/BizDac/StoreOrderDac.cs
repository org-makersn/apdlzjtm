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
    }
}
