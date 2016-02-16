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
        StoreMemberDac sMemberDac = new StoreMemberDac();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<StoreMemberT> GetAllStoreMember()
        {
            return sMemberDac.SelectAllStoreMember();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public StoreMemberT GetStoreMemberByNo(int no)
        {
            return sMemberDac.SelectStoreMemberByNo(no);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public StoreMemberT GetStoreMemberByMemberNO(int memberNo)
        {
            return sMemberDac.SelectStoreMemberByMemberNo(memberNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public bool GetStoreMemberExists(int memberNo)
        {
            return sMemberDac.SelectStoreMemberExists(memberNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StoreMember"></param>
        /// <returns></returns>
        public int AddStoreMember(StoreMemberT StoreMember)
        {
            return sMemberDac.InsertStoreMember(StoreMember);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StoreMember"></param>
        /// <returns></returns>
        public bool UpdateStoreMember(StoreMemberT StoreMember)
        {
            return sMemberDac.UpdateStoreMember(StoreMember);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public List<MemberMsgT> getReceivedNoteListByMemberNo(int memberNo)
        {
            return sMemberDac.SelectReceivedNoteListByMemberNo(memberNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public List<MemberMsgT> getSentNoteListByMemberNo(int memberNo)
        {
            return sMemberDac.SelectSentNoteListByMemberNo(memberNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int sendNote(MemberMsgT msg)
        {
            return sMemberDac.CreateNote(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SeqNo"></param>
        /// <returns></returns>
        public int deleteNote(int SeqNo)
        {
            return sMemberDac.DeleteNote(SeqNo);
        }
    }
}
