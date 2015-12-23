using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using Makersn.Models;
using NHibernate;
using Makersn.Util;
using NHibernate.Criterion;
using System.Net;
using Makersn.BizDac;
public class OrderAccountingDac
{
    public IList<OrderAccountingStateT> GetOrderAccountingList(int status,string sequenceFlag)
    {

//        string query = @"SELECT YEAR, MONTH,M.NAME, PM.BANK, PM.BANK_NAME,PM.ACCOUNT_NO,PA.TOTAL_PRICE ,PM.MEMBER_NO, PA.POST_PRICE
//                        FROM 
//                        PRINTER_MEMBER  PM
//                        INNER JOIN MEMBER M
//                        ON PM.MEMBER_NO = M.NO
//                        INNER JOIN 
//                        (SELECT YEAR(OA.REG_DT) 'YEAR',MONTH( OA.REG_DT) 'MONTH',OA.PRINTER_MEMBER_NO 'PRINTER_MEMBER_NO', SUM(OA.PRICE) 'TOTAL_PRICE',SUM(POST_PRICE) 'POST_PRICE'
//                        FROM 
//                        ORDER_ACCOUNTING AS OA
//                        JOIN ORDER_REQ O
//                        ON OA.ORDER_NO = O.NO
//                        WHERE OA.STATUS = " + status + @"
//                        GROUP BY YEAR(OA.REG_DT),MONTH(OA.REG_DT) , OA.PRINTER_MEMBER_NO) AS PA
//                        ON PA.PRINTER_MEMBER_NO = PM.MEMBER_NO";


        string queryAdd = string.Empty;
        if (sequenceFlag == "desc")
        {
            queryAdd += "order by YEAR,MONTH DESC";
        }
        else {
            queryAdd += "order by YEAR,MONTH";
        }
        string query = @"SELECT YEAR, MONTH,PM.NAME, PM.BANK, PM.BANK_NAME,PM.ACCOUNT_NO,PA.TOTAL_PRICE ,PM.MEMBER_NO, PA.POST_PRICE
                        FROM 
                        PRINTER_MEMBER  PM
                        INNER JOIN MEMBER M
                        ON PM.MEMBER_NO = M.NO
                        INNER JOIN 
                        (SELECT YEAR(O.ORDER_DONE_DT) 'YEAR',MONTH( O.ORDER_DONE_DT) 'MONTH',O.PRINTER_MEMBER_NO 'PRINTER_MEMBER_NO', SUM(O.TOTAL_PRICE) 'TOTAL_PRICE',SUM(O.POST_PRICE) 'POST_PRICE'
                        FROM (SELECT ORDER_DONE_DT, PRINTER_MEMBER_NO, (SELECT SUM(UNIT_PRICE * ORDER_COUNT) FROM ORDER_DETAIL WHERE ORDER_DETAIL.ORDER_NO = ORDER_REQ.NO)'TOTAL_PRICE' , ACCOUNT_STATE, POST_PRICE, ORDER_STATUS FROM ORDER_REQ) O
                        WHERE O.ACCOUNT_STATE = :status and  O.ORDER_STATUS = '" + (int)MakersnEnumTypes.OrderState.거래완료 + @"' and O.ORDER_DONE_DT IS NOT NULL
                        GROUP BY YEAR(O.ORDER_DONE_DT),MONTH(O.ORDER_DONE_DT),O.PRINTER_MEMBER_NO) AS PA
                        ON PA.PRINTER_MEMBER_NO = PM.MEMBER_NO ";

        query += queryAdd;

        using (ISession session = NHibernateHelper.OpenSession())
        {
            IQuery queryObj = session.CreateSQLQuery(query);
            queryObj.SetParameter("status",status);
            IList<object[]> results = queryObj.List<object[]>();
            IList<OrderAccountingStateT> orderAccountingStateList = new List<OrderAccountingStateT>();
            foreach (object[] row in results)
            {
                OrderAccountingStateT orderAccountingState = new OrderAccountingStateT();
                orderAccountingState.Year = (int)row[0];
                orderAccountingState.Month = (int)row[1];
                orderAccountingState.PrinterMemberName = (string)row[2];
                orderAccountingState.Bank = System.Convert.ToInt32(row[3]);
                orderAccountingState.BankName = (string)row[4];
                orderAccountingState.Account = (string)row[5];
                orderAccountingState.Price = (int)row[6];
                orderAccountingState.PrinterMemberNo = (int)row[7];
                orderAccountingState.PostPrice = (int)row[8];
                orderAccountingState.PaidPrice = (int)(orderAccountingState.Price * (1-DataSetting.PRT_ORDER_RECEIVE_RATE)) + orderAccountingState.PostPrice;
                orderAccountingState.Status = status;

                orderAccountingState.OrderList = new OrderDac().getOrderListByMonthForAdmin(orderAccountingState.Year, orderAccountingState.Month, orderAccountingState.PrinterMemberNo,status,session);

                orderAccountingStateList.Add(orderAccountingState);

            }

            return orderAccountingStateList;
        }
    }

    public void Adjustment(int year, int month, int printerMemberNo, int paidPrice,int postPrice, string userId)
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
        else {
            monthTo++;
        }

        //string query = "UPDATE ORDER_REQ SET ACCOUNT_STATE = '2' , ACCOUNT_DONE_DT = getdate(), ACCOUNT_DONE_ID = '" + userId + "' , UPD_DT = getdate() WHERE ORDER_DONE_DT >='" + yearFrom + "-" + monthFrom + "-1' AND ORDER_DONE_DT <'" + yearTo + "-" + monthTo + "-1' AND PRINTER_MEMBER_NO = " + printerMemberNo;
        string query = "UPDATE ORDER_REQ SET ACCOUNT_STATE = '2' , ACCOUNT_DONE_DT = getdate(), ACCOUNT_DONE_ID = :userId , UPD_DT = getdate() WHERE ORDER_DONE_DT >= :DtStart AND ORDER_DONE_DT < :DtEnd AND PRINTER_MEMBER_NO = :printerMemberNo";
        //save Log
        OrderAccountingLogT oal = new OrderAccountingLogT();
        
        oal.Year = year;
        oal.Month = month;
        oal.PrinterMemberNo = printerMemberNo;
        oal.PaidPrice = paidPrice;
        oal.PostPrice = postPrice;
        oal.PayDt = DateTime.Now;

        using (ISession session = NHibernateHelper.OpenSession())
        {

            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    IQuery queryObj = session.CreateSQLQuery(query);
                    queryObj.SetParameter("userId", userId);
                    queryObj.SetParameter("DtStart",  yearFrom + "-" + monthFrom + "-1");
                    queryObj.SetParameter("DtEnd", yearTo + "-" + monthTo + "-1");
                    queryObj.SetParameter("printerMemberNo", printerMemberNo);
                    queryObj.ExecuteUpdate();
                    new OrderAccountingLogDac().insertOrderAccountingLog(oal, session);
                    session.Flush();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
                transaction.Commit();
            }
        }
    }
    public void InsertOrderAccounting(OrderAccountingT data)
    {
        using (ISession session = NHibernateHelper.OpenSession())
        {
            session.Save(data);
        }
    }
}

