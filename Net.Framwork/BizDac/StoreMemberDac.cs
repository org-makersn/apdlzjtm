using Net.Framework;
using Net.Framework.Helper;
using Net.Framework.StoreModel;
using Net.SqlTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreMemberDac : DacBase
    {
        private StoreContext dbContext { get; set; }

        private ISimpleRepository<StoreMemberT> _sMembRepo = new SimpleRepository<StoreMemberT>();

        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal List<StoreMemberT> SelectAllStoreMember()
        {
            var state = _sMembRepo.GetAll().ToList();

            return state == null ? null : state.ToList();
        }

        /// <summary>
        /// select one StoreMember By No
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StoreMemberT SelectStoreMemberByNo(int no)
        {
            return _sMembRepo.First(m => m.No == no);
        }

        /// <summary>
        /// select one StoreMember By MemberNo
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        internal StoreMemberT SelectStoreMemberByMemberNo(int memberNo)
        {
            return _sMembRepo.First(m => m.MemberNo == memberNo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        internal bool SelectStoreMemberExists(int memberNo)
        {
            dbHelper = new SqlDbHelper(connectionString);
            string query = "SELECT count(1) FROM STORE_MEMBER with(nolock) where MEMBER_NO = @MEMBER_NO";

            using (var cmd = new SqlCommand(query))
            {
                cmd.Parameters.Add("@MEMBER_NO", SqlDbType.Int).Value = memberNo;
                int cnt = dbHelper.ExecuteScalar<int>(cmd);
                return cnt > 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        internal StoreMemberExT SelectFullStoreMemberByMemberNo(int memberNo)
        {
            dbHelper = new SqlDbHelper(connectionString);

            string query = DacHelper.GetSqlCommand("StoreMemberDac.SelectFullStoreMemberByMemberNo");
            using (var cmd = new SqlCommand(query))
            {
                cmd.Parameters.Add("@STORE_MEMBER_NO", SqlDbType.Int).Value = memberNo;

                return dbHelper.ExecuteSingle<StoreMemberExT>(cmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        internal int SelectStoreMemberNoByMemberNo(int memberNo)
        {
            dbHelper = new SqlDbHelper(connectionString);
            string query = "SELECT NO FROM STORE_MEMBER with(nolock) where MEMBER_NO = @MEMBER_NO";

            using (var cmd = new SqlCommand(query))
            {
                cmd.Parameters.Add("@MEMBER_NO", SqlDbType.Int).Value = memberNo;
                return dbHelper.ExecuteScalar<int>(cmd);
            }
        }

        /// <summary>
        /// Insert StoreMember
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertStoreMember(StoreMemberT data)
        {
            int identity = 0;
            bool ret = _sMembRepo.Insert(data);

            if (ret)
            {
                identity = _sMembRepo.First(m => m.StoreName == data.StoreName).No;
            }
            return identity;
        }

        /// <summary>
        /// Update StoreMember
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal bool UpdateStoreMember(StoreMemberT data)
        {
            return _sMembRepo.Update(data);
        }


        /// <summary>
        /// 받은 쪽지 리스트 가져오기
        /// </summary>
        /// <param name="memberNo">회원번호</param>
        /// <returns></returns>
        internal List<MemberMsgT> SelectReceivedNoteListByMemberNo(int memberNo)
        {
            List<MemberMsgT> msgs = null;
            using (dbContext = new StoreContext())
            {
                msgs = dbContext.MemberMsgT.Where(m => m.MemberNoRef == memberNo && m.DelFlag == "N").ToList();
            }
            return msgs;
        }

        /// <summary>
        /// 보낸 쪽지 리스트 가져오기
        /// </summary>
        /// <param name="memberNo">회원번호</param>
        /// <returns></returns>
        internal List<MemberMsgT> SelectSentNoteListByMemberNo(int memberNo)
        {
            List<MemberMsgT> msgs = null;
            using (dbContext = new StoreContext())
            {
                msgs = dbContext.MemberMsgT.Where(m => m.MemberNo == memberNo && m.DelFlag == "N").ToList();
            }
            return msgs;
        }

        /// <summary>
        /// 쪽지 보내기
        /// </summary>
        /// <param name="fromMember">보내는 사람</param>
        /// <param name="targetMember">받는 사람</param>
        /// <param name="comment">쪽지 내용</param>
        /// <returns></returns>
        internal int CreateNote(MemberMsgT msg)
        {
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.MemberMsgT.Add(msg);
                ret = dbContext.SaveChanges();
            }
            return ret;
        }

        /// <summary>
        /// 쪽지 삭제하기
        /// 플래그 값 변경 (DelFlag) => Y
        /// </summary>
        /// <param name="SeqNo">쪽지 고유번호</param>
        /// <returns></returns>
        internal int DeleteNote(int SeqNo)
        {
            int ret = 0;
            using (dbContext = new StoreContext())
            {

                MemberMsgT msg = dbContext.MemberMsgT.SingleOrDefault(m => m.No == SeqNo);
                if (msg != null)
                {
                    try
                    {
                        msg.DelFlag = "Y";
                        dbContext.MemberMsgT.Attach(msg);
                        dbContext.Entry<MemberMsgT>(msg).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                        ret = -1;
                    }
                }
                else
                {
                    ret = -2;
                    throw new NullReferenceException("The expected original Segment data is not here.");
                }
            }
            return ret;
        }
    }
}
