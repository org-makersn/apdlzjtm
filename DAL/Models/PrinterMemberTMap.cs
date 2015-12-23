using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Makersn.Models
{
    public class PrinterMemberTMap : ClassMap<PrinterMemberT>
    {
        public PrinterMemberTMap()
        {
            Table("PRINTER_MEMBER");
            Id(x => x.No, "NO");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.AccountNo, "ACCOUNT_NO");
            Map(x => x.Bank, "BANK");
            Map(x => x.BankName, "BANK_NAME");
            Map(x => x.HomePhone, "HOME_PHONE");
            Map(x => x.CellPhone, "CELL_PHONE");
            Map(x => x.SpotName, "NAME");
            Map(x => x.SpotAddress, "ADDRESS");
            Map(x => x.SpotAddressDetail, "ADDRESS_DETAIL");
            Map(x => x.LocX, "LOC_X");
            Map(x => x.LocY, "LOC_Y");
            Map(x => x.PostMode, "POST_MODE");
            Map(x => x.PostType, "POST_TYPE");
            Map(x => x.PostPrice, "POST_PRICE");
            Map(x => x.TaxbillFlag, "TAXBILL_YN");
            Map(x => x.PrinterProfileMsg, "PRINTER_PROFILE_MSG");
            Map(x => x.DelFlag, "DEL_FLAG");
            Map(x => x.SaveFlag, "SAVE_FLAG");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.UpdId, "UPD_ID");
            Map(x => x.UpdDt, "UPD_DT");
            Map(x => x.PrinterProfilePic, "PROFILE_PIC");
            Map(x => x.PrinterCoverPic, "COVER_PIC");
            Map(x => x.SpotUrl, "URL");
            Map(x => x.ViewCnt, "VIEW_CNT");

            //Map(x => x.ReviewCnt, "REVIEW_CNT").Nullable();
            //Map(x => x.ReviewScore, "REVIEW_SCORE").Nullable();

        }
    }
}
