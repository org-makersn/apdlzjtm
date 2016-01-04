using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makersn.Util
{
    public class MakersnEnumTypes
    {
        /// <summary>
        /// join type
        /// </summary>
        public enum JoinType
        {
            [EnumTitle("사이트")]
            Email = 1,
            [EnumTitle("페이스북")]
            Facebook = 2,
            //...필요하면 추가
        }

        /// <summary>
        /// 코드구분
        /// </summary>
        public enum CodeGbn
        {
            [EnumTitle("게시물")]
            Article = 1,
            [EnumTitle("문의사항")]
            Contact = 2
        }

        /// <summary>
        /// 개월수
        /// </summary>
        public enum MonthLst
        {
            [EnumTitle("1월")]
            January = 1,
            [EnumTitle("2월")]
            Febuary = 2,
            [EnumTitle("3월")]
            March = 3,
            [EnumTitle("4월")]
            April = 4,
            [EnumTitle("5월")]
            May = 5,
            [EnumTitle("6월")]
            June = 6,
            [EnumTitle("7월")]
            July = 7,
            [EnumTitle("8월")]
            August = 8,
            [EnumTitle("9월")]
            September = 9,
            [EnumTitle("10월")]
            October = 10,
            [EnumTitle("11월")]
            November = 11,
            [EnumTitle("12월")]
            December = 12
        }

        /// <summary>
        /// 
        /// </summary>
        public enum ContactState
        {
            [EnumTitle("접수")]
            접수 = 1,
            [EnumTitle("답변")]
            답변 = 2,
            [EnumTitle("보류")]
            보류 = 3
        }

        /// <summary>
        /// 
        /// </summary>
        public enum ReportState
        {
            [EnumTitle("모두")]
            모두 = 1,
            [EnumTitle("게시자")]
            게시자 = 2,
            [EnumTitle("리포터")]
            리포터 = 3
        }

        /// <summary>
        /// 컨테스트 관련 DB 카테고리 번호 명명규칙
        /// ex) 첫번째 컨테스트 && 생활용품 > 10104
        ///     두번째 컨테스트 && 캐릭터토이 > 10203
        /// </summary>
        public enum CateName
        {
            [EnumTitle("전체")]
            전체 = 0,
            [EnumTitle("아트&데코")]
            아트데코 = 1001,
            [EnumTitle("패션&악세사리")]
            패션악세사리 = 1002,
            [EnumTitle("캐릭터&토이")]
            캐릭터토이 = 1003,
            [EnumTitle("생활용품")]
            생활용품 = 1004,
            [EnumTitle("테크&툴")]
            테크툴 = 1005,
            [EnumTitle("교육&학습도구")]
            교육학습도구 = 1006,
            //[EnumTitle("컨테스트")]
            //컨테스트 = 10203
        }

        /// <summary>
        /// 
        /// </summary>
        public enum ArticleFileGbn
        {
            [EnumTitle("Temp")]
            Temp = 1,
            [EnumTitle("Article")]
            Article = 2
        }

        /// <summary>
        /// 
        /// </summary>
        public enum ContactName
        {
            [EnumTitle("불편/개선사항")]
            불편개선사항 = 2001,
            [EnumTitle("채용관련")]
            채용관련 = 2002,
            [EnumTitle("제휴 및 협력관련")]
            제휴및협력관련 = 2003,
            [EnumTitle("미디어관련")]
            미디어관련 = 2004,
            [EnumTitle("기타")]
            기타 = 2005
        }

        /// <summary>
        /// 
        /// </summary>
        public enum InfoName
        {
            [EnumTitle("소개")]
            about = 0,
            //[EnumTitle("블로그")]
            //blog = 1,
            [EnumTitle("고객센터")]
            customer = 2,
            [EnumTitle("공지사항")]
            notice = 3,
            [EnumTitle("라이센스")]
            license = 4,
            [EnumTitle("이용약관")]
            terms = 5,
            [EnumTitle("개인정보취급방침")]
            privacy = 6
        }

        /// <summary>
        /// 
        /// </summary>
        public enum MenuGugun
        {
            [EnumTitle("디자인")]
            design = 1,
            [EnumTitle("프린팅")]
            printing = 2
        }

        public enum ControllerList
        {
            [EnumTitle("article")]
            article = 1,
            [EnumTitle("account")]
            account = 2,
            [EnumTitle("info")]
            info = 3,
            [EnumTitle("main")]
            main = 4,
            [EnumTitle("profile")]
            profile = 5,
            [EnumTitle("gmap")]
            gmap = 6,
            [EnumTitle("cleanup")]
            cleanup = 7,
            [EnumTitle("admin")]
            admin = 8,
            [EnumTitle("printing")]
            printing = 9,
            [EnumTitle("order")]
            order = 10,
            [EnumTitle("cate")]
            cate = 11,
            [EnumTitle("item")]
            item = 12,
            [EnumTitle("design")]
            design = 13,
            [EnumTitle("printingprofile")]
            printingprofile = 14,
            [EnumTitle("spot")]
            spot = 15
        }

        public enum blogVeto
        {
            [EnumTitle("article")]
            article = 1,
            [EnumTitle("account")]
            account = 2,
            [EnumTitle("info")]
            info = 3,
            [EnumTitle("main")]
            main = 4,
            [EnumTitle("profile")]
            profile = 5,
            [EnumTitle("gmap")]
            gmap = 6,
            [EnumTitle("cleanup")]
            cleanup = 7,
            [EnumTitle("admin")]
            admin =8
        }

        public enum OrderState {
            [EnumTitle("주문신청")]
            주문신청 = 10,
            [EnumTitle("결제대기")]
            결제대기 = 11,
            [EnumTitle("결제완료")]
            결제완료 = 12,
            [EnumTitle("주문확인")]
            주문확인 = 13,
            [EnumTitle("배송중")]
            배송중 = 14,
            [EnumTitle("배송완료")]
            배송완료 = 15,
            [EnumTitle("취소")]
            취소 = 16,
            [EnumTitle("구매완료")]
            구매완료 = 17,

            [EnumTitle("반품신청")]
            반품신청 = 20,
            [EnumTitle("반품판매자확인")]
            반품판매자확인 = 21,
            [EnumTitle("반품거부")]
            반품거부 = 22,
            [EnumTitle("반품처리중")]
            반품처리중 = 23,
            [EnumTitle("반품완료")]
            반품완료 = 24,
            [EnumTitle("환불접수")]
            환불접수 = 25,
            [EnumTitle("요청거부")]
            요청거부 = 30
        }

        public enum PayType
        {
            [EnumTitle("계좌 이체")]
            BTP = 10,
            [EnumTitle("무통장 입금")]
            MTP = 11,
            [EnumTitle("신용 카드")]
            CCP =20,
            [EnumTitle("핸드폰 결제")]
            MPP = 30
        }

    }
}
