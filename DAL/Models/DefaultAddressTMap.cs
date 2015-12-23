using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Models
{
    public class DefaultAddressTMap :ClassMap<DefaultAddressT>
    {
        public DefaultAddressTMap()
        {
            Table("DEFAULT_ADDRESS");

            Id(x => x.No, "NO");
            Map(x => x.MemberNo, "MEMBER_NO");
            Map(x => x.PostMemberName, "POST_MEMBER_NAME");
            Map(x => x.Address, "ADDRESS");
            Map(x => x.AddressDetail, "ADDRESS_DETAIL");
            Map(x => x.PostNum, "POST_NUM");
            Map(x => x.CellPhone, "CELL_PHONE");
            Map(x => x.AddPhone, "ADD_PHONE");
            Map(x => x.RegDt, "REG_DT");
            Map(x => x.RegId, "REG_ID");
            Map(x => x.UpdDt, "UPD_DT").Nullable();
            Map(x => x.UpdId, "UPD_ID").Nullable();
        }
    }
}
