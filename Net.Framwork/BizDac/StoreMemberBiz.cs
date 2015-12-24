using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreMemberBiz
    {
        public List<StoreMemberT> getAllStorePrinter() {
            return new StoreMemberDac().SelectAllStoreMember();
        }
        public StoreMemberT getStoreMemberById (int no){
            return new StoreMemberDac().SelectStoreMemberTById(no);
        }
        public int add(StoreMemberT StoreMember)
        {
            return new StoreMemberDac().InsertStoreMember(StoreMember);
        }
        public int upd(StoreMemberT StoreMember)
        {
            return new StoreMemberDac().UpdateStoreMember(StoreMember);
        }
        public List< StoreMemberT > getAllMemberList() {
            return new StoreMemberDac().SelectAllStoreMember();
        }



    }
}
