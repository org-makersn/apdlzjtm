using Net.Framework;
using Net.Framework.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class StoreMemberDac
    {

        private static StoreContext dbContext;
        
        /// <summary>
        /// select multi data
        /// </summary>
        /// <returns></returns>
        internal List<StoreMemberT> SelectAllStoreMember()
        {        
            List<StoreMemberT> printers = null;
            using (dbContext = new StoreContext())
            {
                printers = dbContext.StoreMemberT.ToList();
            }
            return printers;
        }

        /// <summary>
        /// select one StoreMember By Id
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        internal StoreMemberT SelectStoreMemberTById(int no)
        {
            StoreMemberT printer = null;

            using (dbContext = new StoreContext())
            {
                printer = dbContext.StoreMemberT.Where(m => m.NO == no).FirstOrDefault();
            }

            return printer;
        }

        /// <summary>
        /// Insert StoreMember
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int InsertStoreMember(StoreMemberT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");
            int ret = 0;
            using (dbContext = new StoreContext())
            {
                dbContext.StoreMemberT.Add(data);
                ret = dbContext.SaveChanges();
            }
            return ret;
        }

        /// <summary>
        /// Update StoreMember
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal int UpdateStoreMember(StoreMemberT data)
        {
            if (data == null) throw new ArgumentNullException("The expected Segment data is not here.");

            int ret = 0;
            using (dbContext = new StoreContext())
            {
                
                //if (dbContext.StoreMemberT.SingleOrDefault(m => m.NO == data.NO) != null)
                //{
                    try
                    {
                        dbContext.StoreMemberT.Attach(data);
                        dbContext.Entry<StoreMemberT>(data).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                    catch (Exception)
                    {
                        ret = -1;
                    }
                //}
                //else
                //{
                //    ret = -2;
                //    throw new NullReferenceException("The expected original Segment data is not here.");
                //}
            }
            return ret;
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
