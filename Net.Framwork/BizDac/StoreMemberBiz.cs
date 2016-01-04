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
        public StoreMemberT getStoreMemberById(int no)
        {
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
        public List<StoreMemberT> getAllMemberList()
        {
            return new StoreMemberDac().SelectAllStoreMember();
        }




        public List<MemberMsgT> getReceivedNoteListByMemberNo(int memberNo)
        {
            return new StoreMemberDac().SelectReceivedNoteListByMemberNo(memberNo);
        }

        public List<MemberMsgT> getSentNoteListByMemberNo(int memberNo)
        {
            return new StoreMemberDac().SelectSentNoteListByMemberNo(memberNo);
        }

        public int sendNote(MemberMsgT msg)
        {
            return new StoreMemberDac().CreateNote(msg);
        }

        public int deleteNote(int SeqNo)
        {
            return new StoreMemberDac().DeleteNote(SeqNo);
        }
    }
}
