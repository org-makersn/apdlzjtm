using Net.Framwork.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framwork.BizDac
{
    public class MemberDac_bak
    {
        static IRepository<MemberT> _repository = new Repository<MemberT>();

        /// <summary>
        /// 회원 조회 - 로그인
        /// </summary>
        /// <param name="membId"></param>
        /// <param name="password"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public MemberT GetMemberForLogOn(string membId, string password, string ip)
        {
            MemberT member = _repository.First(m => (m.ID == membId && m.PASSWORD == password) && m.LEVEL >= 50 && m.DEL_FLAG == "N");
            if (member != null)
            {
                member.LOGIN_CNT += 1;
                member.LAST_LOGIN_IP = ip;
                member.LAST_LOGIN_DT = DateTime.Now;

                _repository.Update(member);
            }
            return member;
        }

        /// <summary>
        /// 회원 조회 - sns
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public MemberT GetMemberExistById(string memberId, string openId)
        {
            return _repository.First(m => m.ID == memberId && m.SNS_ID == openId && m.SNS_TYPE == "fb" && m.DEL_FLAG == "N");
        }

        /// <summary>
        /// 회원 저장
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public int RegisterMember(MemberT member)
        {
            return _repository.Insert(member);
        }

        /// <summary>
        /// 이메일 인증
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public bool UpdateMemberEmailCertify(int memberNo)
        {
            bool result = false;
            MemberT member = _repository.First(m => m.NO == memberNo);
            if (member != null)
            {
                member.EMAIL_CERTIFY = "Y";
                member.UPD_DT = DateTime.Now;
                member.UPD_ID = member.EMAIL;

                result = _repository.Update(member);
            }
            return result;
        }

        /// <summary>
        /// 이메일 주소 변경
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public bool UpdateMemberEmail(int memberNo)
        {
            bool result = false;

            MemberT member = _repository.First(m => m.NO == memberNo);

            if (member != null)
            {
                member.EMAIL_CERTIFY = "Y";
                member.ID = member.EMAIL;
                member.UPD_DT = DateTime.Now;
                member.UPD_ID = member.EMAIL;

                result = _repository.Update(member);
            }
            return result;
        }

        /// <summary>
        /// 이메일 변경 취소
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
        public bool UpdateEmailCertifyCancel(int memberNo)
        {
            bool result = false;

            MemberT member = _repository.First(m => m.NO == memberNo);
            if (member != null)
            {
                member.EMAIL_CERTIFY = "Y";
                member.EMAIL = member.ID;
                member.UPD_DT = DateTime.Now;
                member.UPD_ID = member.EMAIL;

                result = _repository.Update(member);
            }
            return result;
        }

        /// <summary>
        /// 임시 비밀번호 발급
        /// </summary>
        /// <param name="email"></param>
        /// <param name="temp"></param>
        /// <returns></returns>
        public bool UpdateTemporaryPassword(string id, string temp)
        {
            bool result = false;
            MemberT member = _repository.First(m => m.ID == id && m.EMAIL_CERTIFY == "Y" && m.DEL_FLAG == "N");
            if (member != null)
            {
                member.PASSWORD = temp;
                member.UPD_DT = DateTime.Now;
                member.UPD_ID = "system";

                result = _repository.Update(member);
            }
            return result;
        }
    }
}
