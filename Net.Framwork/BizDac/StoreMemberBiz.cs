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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<StoreMemberT> GetAllStoreMember()
        {
            return new StoreMemberDac().SelectAllStoreMember();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public StoreMemberT GetStoreMemberByNo(int no)
        {
            return new StoreMemberDac().SelectStoreMemberByNo(no);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public StoreMemberT GetStoreMemberByMemberNO(int memberNo)
        {
            return new StoreMemberDac().SelectStoreMemberByMemberNo(memberNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StoreMember"></param>
        /// <returns></returns>
        public int AddStoreMember(StoreMemberT StoreMember)
        {
            return new StoreMemberDac().InsertStoreMember(StoreMember);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StoreMember"></param>
        /// <returns></returns>
        public bool UpdateStoreMember(StoreMemberT StoreMember)
        {
            return new StoreMemberDac().UpdateStoreMember(StoreMember);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public List<MemberMsgT> getReceivedNoteListByMemberNo(int memberNo)
        {
            return new StoreMemberDac().SelectReceivedNoteListByMemberNo(memberNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public List<MemberMsgT> getSentNoteListByMemberNo(int memberNo)
        {
            return new StoreMemberDac().SelectSentNoteListByMemberNo(memberNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int sendNote(MemberMsgT msg)
        {
            return new StoreMemberDac().CreateNote(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SeqNo"></param>
        /// <returns></returns>
        public int deleteNote(int SeqNo)
        {
            return new StoreMemberDac().DeleteNote(SeqNo);
        }
    }
}
