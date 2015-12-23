using Makersn.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.BizDac
{
    public class MessageDac
    {

        public IList<MessageT> GetMessageList(int MemberNo)
        {
            string query = @"SELECT MS.NO, MB.NAME, MB.PROFILE_PIC, MS.ROOM_NAME, 
                                MS.MEMBER_NO, MS.MEMBER_NO_REF, MS.COMMENT, MS.REG_DT, MS.MSG_GUBUN
                            FROM 
                            (SELECT ROOM_NAME, MAX(REG_DT) AS REG_DT
                            FROM MEMBER_MSG WITH(NOLOCK)
                            GROUP BY ROOM_NAME) AS A LEFT OUTER JOIN MEMBER_MSG MS WITH(NOLOCK)
						                            ON A.ROOM_NAME = MS.ROOM_NAME
						                            AND A.REG_DT = MS.REG_DT

						                            INNER JOIN MEMBER MB WITH(NOLOCK)
						                            ON (MS.MEMBER_NO = MB.NO
						                            OR MS.MEMBER_NO_REF = MB.NO)
													AND MB.DEL_FLAG='N'

				                                    WHERE MS.DEL_FLAG = 'N'
				                                    AND (MS.MEMBER_NO = :memberNo
				                                    OR MS.MEMBER_NO_REF = :memberNo )
                                                    AND MB.NO != :memberNo ORDER BY REG_DT DESC";


            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("memberNo", MemberNo);

                IList<object[]> result = queryObj.List<object[]>();



                IList<MessageT> message = new List<MessageT>();
                foreach (object[] row in result)
                {
                    MessageT m = new MessageT();
                    m.No = (long)row[0];
                    m.MemberName = (string)row[1];
                    m.ProfilePic = (string)row[2];
                    m.RoomName = (string)row[3];
                    m.MemberNo = (int)row[4];
                    m.MemberNoRef = (int)row[5];
                    m.Comment = (string)row[6];
                    m.RegDt = (DateTime)row[7];
                    m.MsgGubun = (string)row[8];
                    message.Add(m);
                }
                return message;
            }
        }

        public IList<MessageT> GetMessageByRoomName(int memberNo, int sendMemberNo, int receiveMemberNo)
        {
            string query = @"SELECT MS.NO, MB.NAME, MB.PROFILE_PIC, MS.ROOM_NAME, 
                                MS.MEMBER_NO, MS.MEMBER_NO_REF, MS.COMMENT, MS.REG_DT, MS.MSG_GUBUN
                            FROM 
                            (SELECT ROOM_NAME
                            FROM MEMBER_MSG WITH(NOLOCK)
                            GROUP BY ROOM_NAME) AS A LEFT OUTER JOIN MEMBER_MSG MS WITH(NOLOCK)
						                            ON A.ROOM_NAME = MS.ROOM_NAME

						                            INNER JOIN MEMBER MB WITH(NOLOCK)
						                            ON MS.MEMBER_NO = MB.NO

				                                    WHERE MS.DEL_FLAG = 'N'
				                                    AND (MS.MEMBER_NO = :memberNo
				                                    OR MS.MEMBER_NO_REF = :memberNo )
                                                    AND (MS.ROOM_NAME = :roomName1 OR MS.ROOM_NAME = :roomName2) ORDER BY REG_DT ASC";
            using (ISession session = NHibernateHelper.OpenSession())
            {
                IQuery queryObj = session.CreateSQLQuery(query);
                queryObj.SetParameter("memberNo", memberNo);
                queryObj.SetParameter("roomName1", sendMemberNo + "_" + receiveMemberNo);
                queryObj.SetParameter("roomName2", receiveMemberNo + "_" + sendMemberNo);

                IList<object[]> result = queryObj.List<object[]>();

                IList<MessageT> message = new List<MessageT>();
                foreach (object[] row in result)
                {
                    MessageT m = new MessageT();
                    m.No = (long)row[0];
                    m.MemberName = (string)row[1];
                    m.ProfilePic = (string)row[2];
                    m.RoomName = (string)row[3];
                    m.MemberNo = (int)row[4];
                    m.MemberNoRef = (int)row[5];
                    m.Comment = (string)row[6];
                    m.RegDt = (DateTime)row[7];
                    m.MsgGubun = (string)row[8];
                    message.Add(m);
                }
                return message;
            }
        }

        public void AddMessage(MessageT message)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                MessageT chkRoom = session.QueryOver<MessageT>().Where(w => w.RoomName == message.MemberNo + "_" + message.MemberNoRef || w.RoomName == message.MemberNoRef + "_" + message.MemberNo).
                                                                    Take(1).SingleOrDefault<MessageT>();
                if (chkRoom != null) { message.RoomName = chkRoom.RoomName; }
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(message);
                    transaction.Commit();
                    session.Flush();
                }
            }
        }

        #region 확인여부
        public void UpdateMessageIsNew(int memberNo)
        {
            string query = @"UPDATE MEMBER_MSG SET IS_NEW = 'N' WHERE IS_NEW='Y' AND MEMBER_NO_REF = :memberNo";

            using (ISession session = NHibernateHelper.OpenSession())
            {

                using (ITransaction transaction = session.BeginTransaction())
                {
                    IQuery queryObj = session.CreateSQLQuery(query);
                    queryObj.SetParameter("memberNo", memberNo);
                    queryObj.ExecuteUpdate();
                    transaction.Commit();

                    //session.CreateSQLQuery(query).ExecuteUpdate();
                    //transaction.Commit();
                    session.Flush();
                }
            }
        }
        #endregion

        #region 신규 메시지 갯수
        public int GetNewMessageCount(int memberNo)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                return session.QueryOver<MessageT>().Where(w => w.MemberNoRef == memberNo && w.IsNew == "Y").RowCount();
            }
        }
        #endregion
    }
}
