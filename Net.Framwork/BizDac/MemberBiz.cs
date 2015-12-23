using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class MemberBiz
    {
        public List<MemberT> getAllMembers()
        {
            return new MemberDac().SelectAllMembers();
        }
    }
}
