using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makersn.Models;
using NHibernate;
using Makersn.Util;
using System.Globalization;



namespace Makersn.BizDac
{
    public class OrderDac
    {
        public OrderT GetOrderByNoForOrderDetail(int orderNo, int memberNo)
        {
            string query = @"SELECT REQ.NO, M.NAME, REQ.PRINTER_MEMBER_NO, REQ.PRINTER_NO
                                            FROM ORDER_REQ REQ WITH(NOLOCK) INNER JOIN MEMBER M WITH(NOLOCK)
	                                            ON PRINTER_MEMBER_NO = M.NO
                                            WHERE REQ.NO = :orderNo AND O.TEST_FLAG <>'Y' AND REQ.MEMBER_NO = :memberNo ";
            OrderT order = new OrderT();

            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("orderNo", orderNo);
                queryObj.SetParameter("memberNo", memberNo);

                IList<object[]> result = queryObj.List<object[]>();
                foreach (object[] row in result)
                {
                    order.No = (long)row[0];
                    order.PrinterMemberName = (string)row[1];
                    order.PrinterMemberNo = (int)row[2];
                    order.PrinterNo = (int)row[3];
                }
            }
            return order;
        }
        public IList<OrderT> GetSearchOrderListPrt(int? memberNo, int statFrom, int statTo, string searchType, string status, string dtStart, string dtEnd, string text)
        {

            string queryAdd = "";
            switch (searchType)
            {
                case "ordMemNm":
                    queryAdd = " AND M.NAME LIKE :text ";
                    break;
                case "ordNo":
                    queryAdd = " AND O.ORDER_NO LIKE :text ";
                    break;
                case "email":
                    queryAdd = " AND M.EMAIL like :text ";
                    break;
                case "postMemNm":
                    queryAdd = " AND O.POST_MEMBER_NAME like :text ";
                    break;
            }
            if (status != "0")
            {
                queryAdd += " AND ORDER_STATUS = :status ";
            }

            string[] dateFormat = { "yyyy-MM-dd", "yyyy/MM/dd", "yyyy.MM.dd", "yyyyMMdd"};
            if (dtStart != "")
            {   
                DateTime outputTemp;
                if (DateTime.TryParseExact(dtStart, dateFormat, null, DateTimeStyles.None, out outputTemp))
                {
                    queryAdd += " AND O.PAY_DT >= :dtStart ";
                }
                else
                {
                    queryAdd += "AND O.PAY_DT >= :dtStart ";
                }
            }
            if(dtEnd != "")
            {
                DateTime outputTemp;
                if (DateTime.TryParseExact(dtEnd, dateFormat, null, DateTimeStyles.None, out outputTemp))
                {
                    queryAdd += "AND O.PAY_DT <= :dtEnd ";
                }
                else
                {
                    queryAdd += "AND O.PAY_DT <= :dtEnd ";
                }
            }

            if (memberNo != null) {
                queryAdd += "AND O.PRINTER_MEMBER_NO = :memberNo ";
            }
            queryAdd += " ORDER BY PAY_DT DESC";


            string query = @"SELECT  O.NO, ORDER_NO, MEMBER_NO, PRINTER_MEMBER_NO, PRINTER_NO, QUALITY, ORDER_DT, POST_DT, ORDER_STATUS, PAY_TYPE,
                            PAY_DT, PAY_BANK, PAY_ACCOUNT_NO,PAY_ACCOUNT_NAME, POST_MEMBER_NAME, POST_ADDRESS, POST_ADDRESS_DETAIL, POST_NUM, POST_MODE, REQUIRE_COMMENT, APPROVE_FLAG, 
                            CANCLE_COMMENT, CANCLE_DT, TEMP, MEMO, TEST_FLAG, O.CELL_PHONE, ORDER_PATH, O.REG_ID, O.REG_DT, O.UPD_ID, O.UPD_DT, POST_PRICE ,O.ADD_PHONE,O.DELIVERY_COMPANY,O.DELIVERY_NUM, O.CURRENCY_FLAG, O.CURRENCY_NUM, O.CURRENCY_NUM_TYPE,
                            O.ORDER_DONE_DT, O.ACCOUNT_DONE_DT, O.ACCOUNT_DONE_ID, O.ACCOUNT_STATE
                            FROM ORDER_REQ O, MEMBER M
                            WHERE O.MEMBER_NO = M.NO AND O.TEST_FLAG <>'Y' AND ORDER_STATUS >= :statFrom AND ORDER_STATUS <= :statTo";
            query += queryAdd;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(OrderT));
                queryObj.SetParameter("statFrom", statFrom);
                queryObj.SetParameter("statTo", statTo);
                if (query.Contains(":text"))
                {
                    queryObj.SetParameter("text", "%" + text + "%");
                }
                if (query.Contains(":status"))
                {
                    queryObj.SetParameter("status", status);
                }
                if (query.Contains(":dtStart"))
                {
                    DateTime outputTemp;
                    if (DateTime.TryParseExact(dtStart, dateFormat, null, DateTimeStyles.None, out outputTemp))
                    {
                        queryObj.SetParameter("dtStart", dtStart + " 00:00:00 ");
                    }
                    else
                    {
                        queryObj.SetParameter("dtStart", dtStart);
                    }
                }
                if (query.Contains(":dtEnd"))
                {
                    DateTime outputTemp;
                    if (DateTime.TryParseExact(dtEnd, dateFormat, null, DateTimeStyles.None, out outputTemp))
                    {
                        queryObj.SetParameter("dtEnd", dtEnd + " 23:59:59 ");
                    }
                    else
                    {
                        queryObj.SetParameter("dtEnd", dtEnd);
                    }
                }
                if (query.Contains(":memberNo"))
                {
                    queryObj.SetParameter("memberNo", memberNo);
                }

                IList<OrderT> orderList = queryObj.List<OrderT>();
                orderList = SetOrderDetail(orderList);
                return orderList;
            }
        }

        public IList<OrderT> GetOrderByPrtNo(int printerNo, int statFrom, int statTo)
        {
            string query = @"SELECT  O.NO, ORDER_NO, O.MEMBER_NO, PRINTER_MEMBER_NO, PRINTER_NO, O.QUALITY, ORDER_DT, POST_DT, ORDER_STATUS, PAY_TYPE,
                            PAY_DT, PAY_BANK, PAY_ACCOUNT_NO,PAY_ACCOUNT_NAME , POST_MEMBER_NAME, POST_ADDRESS, POST_ADDRESS_DETAIL, POST_NUM, O.POST_MODE, REQUIRE_COMMENT, APPROVE_FLAG, 
                            CANCLE_COMMENT, CANCLE_DT, O.TEMP, MEMO, TEST_FLAG, O.CELL_PHONE, ORDER_PATH, O.REG_ID, O.REG_DT, O.UPD_ID, O.UPD_DT, O.POST_PRICE ,O.ADD_PHONE,O.DELIVERY_COMPANY,O.DELIVERY_NUM,
                            O.ORDER_DONE_DT, O.ACCOUNT_DONE_DT, O.ACCOUNT_DONE_ID, O.ACCOUNT_STATE
                            FROM ORDER_REQ O, Printer P
                            WHERE O.PRINTER_NO  = P.NO AND O.TEST_FLAG <>'Y' AND P.NO = :printerNo AND ORDER_STATUS >= :statFrom AND ORDER_STATUS <= :statTo";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query).AddEntity(typeof(OrderT));
                queryObj.SetParameter("printerNo", printerNo);
                queryObj.SetParameter("statFrom", statFrom);
                queryObj.SetParameter("statTo", statTo);

                IList<OrderT> orderList = queryObj.List<OrderT>();
                orderList = SetOrderDetail(orderList);
                return orderList;
            }
        }


        public IList<OrderDetailT> GetOrderDetailByMyOrder(long orderNo)
        {
            //            IList<OrderDetailT> list = new List<OrderDetailT>();
            //            string query = @"SELECT PF.RENAME, PF.NAME, OD.VOLUME
            //                                    FROM ORDER_REQ REQ INNER JOIN PRINTER_FILE PF
            //					                                    ON REQ.PRINTER_NO = PF.PRINTER_NO
            //					                                    INNER JOIN PRINTER P
            //					                                    ON REQ.PRINTER_NO = P.NO 
            //					                                    AND P.MAIN_IMG = PF.NO
            //					                                    INNER JOIN ORDER_DETAIL OD
            //					                                    ON REQ.NO = OD.ORDER_NO
            //					                                    WHERE REQ.NO = " + orderNo;
            //            using (ISession session = NHibernateHelper.OpenSession())
            //            {
            //                IList<object[]> result = session.CreateSQLQuery(query).List<object[]>();

            //                foreach (object[] row in result)
            //                {
            //                    OrderDetailT detail = new OrderDetailT();
            //                    detail.FileImgRename = (string)row[0];
            //                    detail.FileName = (string)row[1];
            //                    if (row[2] != null) detail.Volume = (double)row[2];l
            //                    list.Add(detail);
            //                }
            //            }
            //            return list;

            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<OrderDetailT>().Where(w => w.OrderNo == orderNo ).OrderBy(o => o.RegDt).Desc.List<OrderDetailT>();
            }
        }

        public IList<OrderT> GetMyOrderList(int memberNo)
        {
            string query = @"
                            SELECT (SELECT SUM(UNIT_PRICE * ORDER_COUNT) FROM ORDER_DETAIL WHERE ORDER_NO = REQ.NO)  AS TOTAL_PRICE, 
                                   REQ.ORDER_NO, REQ.ORDER_DT,OD.FILE_IMG_RENAME, OD.[FILE_NAME], REQ.ORDER_STATUS, REQ.PRINTER_NO, REQ.NO,
                            (SELECT COUNT(0) FROM ORDER_DETAIL WITH(NOLOCK) WHERE ORDER_NO = REQ.NO) AS CNT, MEM.NAME, REQ.PRINTER_MEMBER_NO, 
                            REQ.REG_DT, REQ.POST_PRICE, REQ.POST_MODE

                            FROM ORDER_REQ REQ WITH(NOLOCK) INNER JOIN ORDER_DETAIL OD WITH(NOLOCK)
					                            ON REQ.NO = OD.ORDER_NO
												AND OD.NO = (
												SELECT TOP 1 NO
												FROM ORDER_DETAIL OD2 WITH(NOLOCK)
												WHERE OD2.ORDER_NO = REQ.NO
												)
                                                INNER JOIN MEMBER MEM WITH(NOLOCK)
												ON REQ.PRINTER_MEMBER_NO = MEM.NO
					
					                            WHERE REQ.MEMBER_NO = :memberNo ";
            IList<OrderT> list = new List<OrderT>();
            using (ISession session = NHibernateHelper.OpenSession())
            {
                //return session.QueryOver<OrderT>().Where(w => w.MemberNo == memberNo).List<OrderT>();
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("memberNo", memberNo);

                IList<object[]> result = queryObj.List<object[]>();

                foreach (object[] row in result)
                {
                    OrderT order = new OrderT();
                    order.TotalPrice = (int)row[0];
                    order.OrderNo = (string)row[1];
                    order.OrderDt = (DateTime)row[2];
                    order.fileImgName = (string)row[3];
                    order.fileName = (string)row[4];
                    order.OrderStatus = (int)row[5];
                    order.PrinterNo = (int)row[6];
                    order.No = (long)row[7];
                    order.DetailCount = (int)row[8];
                    order.PrinterMemberName = (string)row[9];
                    order.PrinterMemberNo = (int)row[10];
                    order.RegDt = (DateTime)row[11];
                    order.PostPrice = (int)row[12];
                    order.PostMode = (int)row[13];
                    list.Add(order);
                }

            }
            return list;
        }

        public IList<OrderT> GetMyOrderListPrt(int memberNo, int statusFrom, int statusTo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                if (memberNo * statusFrom * statusTo > 0)
                {
                    IList<OrderT> orderList = session.QueryOver<OrderT>().Where(w => w.PrinterMemberNo == memberNo && w.OrderStatus >= statusFrom && w.OrderStatus <= statusTo).List<OrderT>();
                    orderList = SetOrderDetail(orderList);
                    return orderList;
                }
                else
                    return null;
            }
        }
        public int GetMyOrderCntPrt(int memberNo, int statusFrom, int statusTo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                string query = "SELECT COUNT(0) FROM ORDER_REQ WHERE PRINTER_MEMBER_NO= :memberNo AND ORDER_STATUS>= :statusFrom AND ORDER_STATUS<= :statusTo ";

                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("memberNo", memberNo);
                queryObj.SetParameter("statusFrom", statusFrom);
                queryObj.SetParameter("statusTo", statusTo);

                int result = queryObj.UniqueResult<Int32>();

                return result;
            }

        }
        public long InsertOrderReqeust(OrderT data)
        {
            long orderNo = 0;

            using (ISession session = NHibernateHelper.OpenSession())
            {
                OrderT order = session.QueryOver<OrderT>().Where(w => w.Temp == data.Temp).Take(1).SingleOrDefault<OrderT>();
                if (order == null)
                {
                    orderNo = (long)session.Save(data);
                    session.Flush();
                }
                else
                {
                    orderNo = order.No;
                }
            }

            return orderNo;
        }
        public long InsertOrderInTest(OrderT order) {
            long orderNo = 0;

            using (ISession session = NHibernateHelper.OpenSession())
            {
                
                    orderNo = (long)session.Save(order);
                    session.Flush();
                
            }
            return orderNo;
        }
        public void UpdateOrder(OrderT order)
        {

            using (ISession session = NHibernateHelper.OpenSession())
            {
                session.Update(order);
                session.Flush();
            }
        }
        public OrderT GetOrderByNo(long orderNo, int memberNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<OrderT>().Where(w => w.No == orderNo && w.MemberNo == memberNo).Take(1).SingleOrDefault<OrderT>();
            }
        }

        public OrderT GetOrderByNoPrt(long orderNo, int memberNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                OrderT order = session.QueryOver<OrderT>().Where(w => w.No == orderNo && w.PrinterMemberNo == memberNo && w.TestFlag != "Y").Take(1).SingleOrDefault<OrderT>();
                order.orderDetailList = new OrderDetailDac().GetDetailListByOrderNo(order.No);
                order.TotalPrice = 0;

                foreach (OrderDetailT orderDetail in order.orderDetailList)
                {
                    order.TotalPrice += orderDetail.OrderCount * orderDetail.UnitPrice;
                    //orderDetail.MaterialName = new MaterialDac().getMaterialNameByNo();
                }
                return order;
            }
        }


        //gooksong
        public IList<OrderT> SetOrderDetail(IList<OrderT> orderList)
        {
            foreach (OrderT order in orderList)
            {
                order.orderDetailList = GetOrderDetailByMyOrder(order.No);
                order.OrderMemberName = new MemberDac().GetMemberProfile(order.MemberNo).Name;
                int totalPrice = 0;
                foreach (OrderDetailT orderdetail in order.orderDetailList)
                {
                    totalPrice += orderdetail.UnitPrice * orderdetail.OrderCount;
                }
                order.TotalPrice = totalPrice;
            }
            return orderList;
        }

        public void DeleteOrder(OrderT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                OrderT order = session.QueryOver<OrderT>().Where(w => w.No == data.No).Take(1).SingleOrDefault<OrderT>();
                IList<OrderDetailT> detailList = session.QueryOver<OrderDetailT>().Where(w => w.OrderNo == data.No).List<OrderDetailT>();

                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (OrderDetailT detail in detailList)
                    {
                        session.Delete(detail);
                    }
                    session.Delete(order);
                    transaction.Commit();
                    session.Flush();
                }
            }
        }

        public IList<PrinterOutputImageT> GetOutputImage(int orderNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<PrinterOutputImageT>().Where(w => w.OrderNo == orderNo).List<PrinterOutputImageT>();
            }
        }

        public bool UpdateOrderState(OrderT data)
        {
            bool result = false;

            using (ISession session = NHibernateHelper.OpenSession())
            {
                OrderT order = session.QueryOver<OrderT>().Where(w => w.No == data.No && w.MemberNo == data.MemberNo).Take(1).SingleOrDefault<OrderT>();
                if (order != null)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {

                        order.OrderStatus = data.OrderStatus;
                        order.UpdDt = data.UpdDt;
                        order.UpdId = data.UpdId;
                        session.Update(order);
                        transaction.Commit();
                        session.Flush();
                        result = true;
                    }
                }
            }

            return result;
        }

        public DefaultAddressT GetDefaultAddress(int memberNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<DefaultAddressT>().Where(w => w.MemberNo == memberNo).Take(1).SingleOrDefault<DefaultAddressT>();
            }
        }

        public void SaveOrUpdateDefaultAddress(DefaultAddressT data)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (data.No > 0)
                    {
                        session.Update(data);
                    }
                    else
                    {
                        session.Save(data);
                    }
                    transaction.Commit();
                    session.Flush();
                }
            }
        }

        public OrderT GetOrderByTemp(string temp, int memberNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<OrderT>().Where(w => w.Temp == temp && w.MemberNo == memberNo).Take(1).SingleOrDefault<OrderT>();
            }
        }

        public OrderT GetLatelyOrderTop1ByMemberNo(int memberNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<OrderT>().Where(w => w.MemberNo == memberNo && w.PostAddress != null).OrderBy(o => o.OrderDt).Desc.Take(1).SingleOrDefault<OrderT>();
            }
        }

        public OrderT GetOrderByAdmin(long orderNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<OrderT>().Where(w => w.No == orderNo).Take(1).SingleOrDefault<OrderT>();
            }
        }

        public IList<OrderT> GetOrderListForApprPayment(int status)
        {
            IList<OrderT> result = new List<OrderT>();

            using (ISession session = NHibernateHelper.OpenSession())
            {
                result = session.QueryOver<OrderT>().Where(w => w.OrderStatus == status && w.OrderDt > DateTime.Now.AddHours(-12)).OrderBy(o => o.RegDt).Asc.List<OrderT>();

                foreach (OrderT order in result)
                {
                    order.OrderMemberName = session.QueryOver<MemberT>().Where(w => w.No == order.MemberNo).SingleOrDefault<MemberT>().Name;
                    order.PrinterMemberName = session.QueryOver<PrinterMemberT>().Where(w => w.MemberNo == order.PrinterMemberNo).SingleOrDefault<PrinterMemberT>().SpotName;
                    IList<OrderDetailT> detailList = session.QueryOver<OrderDetailT>().Where(w => w.OrderNo == order.No).List<OrderDetailT>();
                    foreach (OrderDetailT detail in detailList)
                    {
                        order.TotalPrice += detail.UnitPrice * detail.OrderCount;
                    }
                }
            }
            return result;
        }

        public IList<OrderT> GetOrderListForApprovedPayment()
        {
            IList<OrderT> result = new List<OrderT>();

            using (ISession session = NHibernateHelper.OpenSession())
            {
                result = session.QueryOver<OrderT>().Where(w => w.PayDt > DateTime.Now.AddHours(-12)).OrderBy(o => o.RegDt).Asc.List<OrderT>();

                foreach (OrderT order in result)
                {
                    order.OrderMemberName = session.QueryOver<MemberT>().Where(w => w.No == order.MemberNo).SingleOrDefault<MemberT>().Name;
                    order.PrinterMemberName = session.QueryOver<PrinterMemberT>().Where(w => w.MemberNo == order.PrinterMemberNo).SingleOrDefault<PrinterMemberT>().SpotName;
                    IList<OrderDetailT> detailList = session.QueryOver<OrderDetailT>().Where(w => w.OrderNo == order.No).List<OrderDetailT>();
                    foreach (OrderDetailT detail in detailList)
                    {
                        order.TotalPrice += detail.UnitPrice * detail.OrderCount;
                    }
                }
            }
            return result;
        }

        public bool UpdateOrderbyDone(OrderT data)
        {
            bool result = false;
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        OrderAccountingDac _orderAccountingDac = new OrderAccountingDac();
                        OrderAccountingT oa = new OrderAccountingT();

                        oa.OrderNo = data.No;
                        oa.PrinterNo = data.PrinterNo;
                        oa.PrinterMemberNo = data.PrinterMemberNo;
                        oa.Price = data.TotalPrice;
                        oa.Status = (int)Makersn.Util.MakersnEnumTypes.OrderAccountingStatus.미결제;
                        oa.RegDt = DateTime.Now;
                        oa.RegId = data.UpdId;

                        //_orderAccountingDac.InsertOrderAccounting(oa);

                        session.Save(oa);

                        session.Update(data);

                        transaction.Commit();
                        session.Flush();
                        result = true;
                    }
                    
                }
            }
            catch
            {
                
            }

            return result;
        }

        public IList<OrderT> getOrderListByMonthForAdmin(int year, int month, int printerMemberNo, int status) {
            using(ISession session = NHibernateHelper.OpenSession()){
                return getOrderListByMonthForAdmin(year, month, printerMemberNo,status, session);
            }
        }
        public IList<OrderT> getOrderListByMonthForAdmin(int year, int month, int printerMemberNo,int status, ISession session)
        {
            int yearFrom = year;
            int yearTo = year;
            int monthFrom = month;
            int monthTo = month;
            if (month == 12)
            {
                yearTo++;
                monthTo = 1;
            }
            else
            {
                monthTo++;
            }

            string startDt = yearFrom + "-" + monthFrom + "-1";
            string endDt = yearTo + "-" + monthTo + "-1";

            string query = @"select O.NO, O.ORDER_NO,O.REG_DT, (SELECT NAME FROM MEMBER M WHERE M.NO = O.MEMBER_NO) 'ORDER_MEMBER_NAME', (SELECT SUM(UNIT_PRICE*ORDER_COUNT) FROM ORDER_DETAIL OD WHERE OD.ORDER_NO = O.NO) 'TOTAL_PRICE',
                             O.POST_PRICE, O.ACCOUNT_STATE, O.ORDER_DONE_DT
                             from ORDER_REQ O ";

            string queryAdd = @"WHERE ORDER_DONE_DT >= :startDt AND ORDER_DONE_DT < :endDt 
                                AND O.ACCOUNT_STATE = :status and  O.ORDER_STATUS = '" + (int)MakersnEnumTypes.OrderState.거래완료 + 
                                @"' and O.ORDER_DONE_DT IS NOT NULL ";

            if(printerMemberNo != 0)
            {
                queryAdd += "and PRINTER_MEMBER_NO = :printerMemberNo ";
            }
            else {
                queryAdd += "and 1<>1 ";
            }

            queryAdd += "order by ORDER_DONE_DT DESC";
            query += queryAdd;

            IQuery queryObj = session.CreateSQLQuery(query);
            queryObj.SetParameter("startDt", startDt);
            queryObj.SetParameter("endDt", endDt);
            queryObj.SetParameter("status", status);
            queryObj.SetParameter("printerMemberNo", printerMemberNo);

            IList<object[]> results = queryObj.List<object[]>();
            IList<OrderT> orderList = new List<OrderT>();
            foreach (object[] row in results)
            {
                OrderT order = new OrderT();
                order.No = System.Convert.ToInt32(row[0]);
                order.OrderNo = (string)row[1];
                order.RegDt = (DateTime)row[2];
                order.OrderMemberName = (string)row[3];
                order.TotalPrice = (int)row[4];
                order.PostPrice = (int)row[5];
                order.AccountState = (int)row[6];
                order.OrderDoneDt = (DateTime)row[7];
                orderList.Add(order);

            }


            return orderList;
        }
    }
}
