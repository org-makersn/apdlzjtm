using Net.Framework.Helper;
using Net.Framework.StoreModel;
using System;

namespace Net.Framwork.BizDac
{
    public class MemberExDac
    {
        private ISimpleRepository<MemberExT> _repository = new SimpleRepository<MemberExT>();

        /// <summary>
        /// 회원 조회 - 로그인
        /// </summary>
        /// <param name="membId"></param>
        /// <param name="password"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public MemberExT GetMemberForLogOn(string membId, string password, string ip)
        {
            MemberExT member = _repository.First(m => (m.Id == membId && m.Password == password) && m.Level >= 50 && m.DelFlag == "N");
            if (member != null)
            {
                member.LoginCnt += 1;
                member.LastLoginIp = ip;
                member.LastLoginDt = DateTime.Now;

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
        public MemberExT GetMemberExistById(string memberId, string openId)
        {
            return _repository.First(m => m.Id == memberId && m.SnsId == openId && m.SnsType == "fb" && m.DelFlag == "N");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public MemberExT GetMemberByNo(int no)
        {
            return _repository.First(m => m.No == no);
        }

        /// <summary>
        /// 회원 저장
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public bool RegisterMember(MemberExT member)
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
            MemberExT member = _repository.First(m => m.No == memberNo);
            if (member != null)
            {
                member.EmailCertify = "Y";
                member.UpdDt = DateTime.Now;
                member.UpdId = member.Email;

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

            MemberExT member = _repository.First(m => m.No == memberNo);

            if (member != null)
            {
                member.EmailCertify = "Y";
                member.Id = member.Email;
                member.UpdDt = DateTime.Now;
                member.UpdId = member.Email;

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

            MemberExT member = _repository.First(m => m.No == memberNo);
            if (member != null)
            {
                member.EmailCertify = "Y";
                member.Email = member.Id;
                member.UpdDt = DateTime.Now;
                member.UpdId = member.Email;

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
            MemberExT member = _repository.First(m => m.Id == id && m.EmailCertify == "Y" && m.DelFlag == "N");
            if (member != null)
            {
                member.Password = temp;
                member.UpdDt = DateTime.Now;
                member.UpdId = "system";

                result = _repository.Update(member);
            }
            return result;
        }
    }
}
