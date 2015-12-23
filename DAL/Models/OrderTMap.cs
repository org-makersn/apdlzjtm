using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class OrderTMap : ClassMap<OrderT>
    {
        public OrderTMap()
        {
            Table("ORDER_REQ");
            Id(x => x.No, "NO");
            Map(x => x.OrderNo, "ORDER_NO");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.PrinterMemberNo, "PRINTER_MEMBER_NO");
            Map(x => x.PrinterNo, "PRINTER_NO");
            Map(x => x.Quality, "QUALITY");
            Map(x => x.OrderDt, "ORDER_DT").Nullable();
            Map(x => x.PostDt, "POST_DT").Nullable();
            Map(x => x.OrderStatus, "ORDER_STATUS");
            Map(x => x.PayType, "PAY_TYPE").Nullable();
            Map(x => x.PayDt, "PAY_DT").Nullable();
            Map(x => x.PayBank, "PAY_BANK").Nullable();
            Map(x => x.PayAccountNo, "PAY_ACCOUNT_NO").Nullable();
            Map(x => x.PayAccountName, "PAY_ACCOUNT_NAME").Nullable();
            Map(x => x.CurrencyFlag, "CURRENCY_FLAG").Nullable();
            Map(x => x.CurrencyNum, "CURRENCY_NUM").Nullable();
            Map(x => x.CurrencyNumType, "CURRENCY_NUM_TYPE").Nullable();
            Map(x => x.PostMemberName, "POST_MEMBER_NAME").Nullable();
            Map(x => x.PostAddress, "POST_ADDRESS").Nullable();
            Map(x => x.PostAddressDetail, "POST_ADDRESS_DETAIL").Nullable();
            Map(x => x.PostNum, "POST_NUM").Nullable();
            Map(x => x.PostMode, "POST_MODE");
            Map(x => x.PostPrice, "POST_PRICE");
            Map(x => x.DeliveryCompany, "DELIVERY_COMPANY");
            Map(x => x.DeliveryNum, "DELIVERY_NUM");
            Map(x => x.RequireComment, "REQUIRE_COMMENT").Nullable();
            Map(x => x.ApproveFlag, "APPROVE_FLAG").Nullable();
            Map(x => x.CancleComment, "CANCLE_COMMENT").Nullable();
            Map(x => x.CancleDt, "CANCLE_DT").Nullable();
            Map(x => x.Temp, "TEMP");
            Map(x => x.Memo, "MEMO").Nullable();
            Map(x => x.TestFlag, "TEST_FLAG");
            Map(x => x.CellPhone, "CELL_PHONE").Nullable();
            Map(x => x.AddPhone, "ADD_PHONE").Nullable();
            Map(x => x.OrderPath, "ORDER_PATH").Nullable();
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.UpdId, "UPD_ID").Nullable();
            Map(x => x.UpdDt, "UPD_DT").Nullable();

            // ==>  these colums is for order accounting 
            Map(x => x.OrderDoneDt, "ORDER_DONE_DT").Nullable();
            Map(x => x.AccountDoneDt, "ACCOUNT_DONE_DT").Nullable();
            Map(x => x.AccountDoneID, "ACCOUNT_DONE_ID").Nullable();
            Map(x => x.AccountState, "ACCOUNT_STATE").Nullable();
            // <== finish accounting 

        }
    }
}
