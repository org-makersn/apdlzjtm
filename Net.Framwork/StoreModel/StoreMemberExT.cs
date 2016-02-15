using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framework.StoreModel
{
    public partial class StoreMemberExT
    {
        //[Column("NO")]
        public int No { get; set; }

        //[Column("MEMBER_NO")]
        public int MemberNo { get; set; }
        
        //[Column("MEMBER_NAME")]
        public int MemberName { get; set; }

        //[Column("MAIN_IMG")]
        public string ProfilePic { get; set; }

        //[Column("STORE_NAME")]
        public string StoreName { get; set; }

        //[Column("OFFICE_PHONE")]
        public string OfficePhone { get; set; }

        //[Column("CELL_PHONE")]
        public string CellPhone { get; set; }

        //[Column("STORE_PROFILE_MSG")]
        public string StoreProfileMsg { get; set; }

        //[Column("STORE_URL")]
        public string StoreUrl { get; set; }

        //[Column("BANK_NAME")]
        public string BankName { get; set; }

        //[Column("BANK_USER_NAME")]
        public string BankUserName { get; set; }

        //[Column("BANK_ACCOUNT")]
        public string BankAccount { get; set; }

        //[Column("DEL_YN")]
        public string DelYn { get; set; }

        //[Column("REG_ID")]
        public string RegId { get; set; }

        //[Column("REG_DT")]
        public string RegDt { get; set; }
    }
}
