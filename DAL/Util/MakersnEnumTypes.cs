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
            [EnumTitle("컨테스트")]
            컨테스트 = 10203
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
            admin = 8,
            [EnumTitle("printing")]
            printing = 9,
            [EnumTitle("order")]
            order = 10,
            [EnumTitle("printingprofile")]
            printingprofile = 11
        }

        public enum OrderState
        {


            [EnumTitle("주문요청")]
            주문요청 = 100,
            [EnumTitle("결제대기")]
            결제대기 = 110,
            [EnumTitle("결제완료")]
            결제완료 = 120,
            [EnumTitle("출력중")]
            출력중 = 130,
            [EnumTitle("출력완료")]
            출력완료 = 140,
            //[EnumTitle("사진 업로드 완료")]
            //사진_업로드_완료 = 150,
            [EnumTitle("배송요청")]
            배송요청 = 160,
            //[EnumTitle("배송전")]
            //배송전 = 170,
            [EnumTitle("배송중")]
            배송중 = 180,
            [EnumTitle("배송완료")]
            배송완료 = 190,



            [EnumTitle("거래완료")]
            거래완료 = 200,
            [EnumTitle("요청거부")] // 스팟이 직접 주문 취소
            요청거부 = 210,
            [EnumTitle("시간초과 요청거부")] //시간 초과되어 주문 취소
            시간초과 = 215,
            [EnumTitle("결제취소")]
            결제취소 = 220,

            [EnumTitle("테스트요청")]
            테스트요청 = 300,
            [EnumTitle("테스트성공")]
            테스트성공 = 310,
            [EnumTitle("테스트실패")]
            테스트실패 = 320,

            [EnumTitle("환불완료")]
            환불완료 = 400

            //[EnumTitle("반품신청")]
            //반품신청 = 210,
            //[EnumTitle("반품판매자확인")]
            //반품판매자확인 = 220,
            //[EnumTitle("반품거부")]
            //반품거부 = 230,
            //[EnumTitle("반품처리중")]
            //반품처리중 = 240,
            //[EnumTitle("반품완료")]
            //반품완료 = 250,
            //[EnumTitle("환불접수")]
            //환불접수 = 260,



        }

        public enum PayType
        {
            [EnumTitle("계좌 이체")]
            BTP = 10,
            [EnumTitle("무통장 입금")]
            MTP = 11,
            [EnumTitle("신용 카드")]
            CCP = 20,
            [EnumTitle("핸드폰 결제")]
            MPP = 30
        }

        public enum Material
        {
            [EnumTitle("ABS")]
            ABS = 1,
            [EnumTitle("PLA")]
            PLA = 2,
            [EnumTitle("레진")]
            레진 = 3,
            [EnumTitle("샌드론")]
            샌드론 = 4
        }

        public enum MaterialColor
        {
            [EnumTitle("빨강")]
            빨강 = 1,
            [EnumTitle("핑크")]
            핑크 = 2,
            [EnumTitle("주황")]
            주황 = 3,
            [EnumTitle("형광노랑")]
            형광노랑 = 4,
            [EnumTitle("연두")]
            연두= 5,
            [EnumTitle("초록")]
            초록 = 6,
            [EnumTitle("하늘")]
            하늘 = 7,
            [EnumTitle("파랑")]
            파랑 = 8,
            [EnumTitle("보라")]
            보라 = 9,
            [EnumTitle("갈색")]
            갈색= 10,
            [EnumTitle("베이지(Nature)")]
            베이지=11,
            [EnumTitle("흰색")]
            흰색= 12,
            [EnumTitle("검정")]
            검정= 13
        }

        public enum PrinterStatus
        {
            [EnumTitle("지금 바로 출력 가능")]
            출력가능 = 1,
            [EnumTitle("쉬는 중")]
            쉬는중 = 2
        }

        public enum BankType
        {
            [EnumTitle("국민은행")]
            국민은행 = 1,
            [EnumTitle("신한은행")]
            신한은행 = 2,
            [EnumTitle("우리은행")]
            우리은행 = 3,
            [EnumTitle("농협은행")]
            농협은행 = 4,
            [EnumTitle("하나은행")]
            하나은행 = 5,
            [EnumTitle("기업은행")]
            기업은행 = 6,
            [EnumTitle("외환은행")]
            외환은행 = 7,
            [EnumTitle("씨티은행")]
            씨티은행 = 8,
            [EnumTitle("우체국")]
            우체국 = 9,
            [EnumTitle("부산은행")]
            부산은행 = 10,
            [EnumTitle("SC은행")]
            SC은행 = 11
        }

        public enum QualityType
        {
            [EnumTitle("최상")]
            최상 = 9,
            [EnumTitle("상")]
            상 = 10,
            [EnumTitle("중")]
            중 = 20,
            [EnumTitle("하")]
            하 = 30
        }

        public enum PostType
        {
            [EnumTitle("픽업")]
            픽업 = 1,
            [EnumTitle("택배")]
            택배 = 2,
            [EnumTitle("픽업, 택배")]
            픽업택배 = 3
        }
        public enum PrinterPostType
        {
            [EnumTitle("무료배송")]
            무료배송 = 0,
            [EnumTitle("고정 배송비")]
            고정배송비 = 1,
            //[EnumTitle("상품별 배송비")]
            //상품별_배송비 = 2,
            [EnumTitle("수신자 부담")]
            수신자_부담 = 2
        }
        public enum PrinterNoticeType
        {

        }

        public enum TranslationStatus
        {
            [EnumTitle("요청")]
            요청 = 1,
            [EnumTitle("완료")]
            완료 = 2,
            [EnumTitle("보류")]
            보류 = 3
        }
        public enum TranslationFlag
        {
            [EnumTitle("번역요청")]
            번역요청 = 1,
            [EnumTitle("직접번역")]
            직접번역 = 2
        }

        public enum LanguageType
        {
            [EnumTitle("한국어")]
            KR = 1,
            [EnumTitle("영어")]
            EN = 2,
            [EnumTitle("중국어")]
            CN = 3,
            [EnumTitle("일번어")]
            JP = 4
        }

        public enum EnCateName
        {
            [EnumTitle("All")]
            All = 0,
            [EnumTitle("Art")]
            Art = 1001,
            [EnumTitle("Fashion & Accessories")]
            패션악세사리 = 1002,
            [EnumTitle("Figurine & Toy")]
            캐릭터토이 = 1003,
            [EnumTitle("Home & Living")]
            생활용품 = 1004,
            [EnumTitle("Tech & Tool")]
            테크툴 = 1005,
            [EnumTitle("Learning")]
            교육학습도구 = 1006,
            [EnumTitle("Contest")]
            컨테스트 = 1007
        }


        public enum OrderAccountingStatus { 
            [EnumTitle("미결제")]
            미결제 = 1,
            [EnumTitle("결제완료")]
            결제완료 = 2
        }

        public enum CateNameToUrl
        {
            [EnumTitle("All")]
            All = 0,
            [EnumTitle("Art")]
            Art = 1001,
            [EnumTitle("Fashion_Accessories")]
            Fashion_Accessories = 1002,
            [EnumTitle("Toy_Figurine")]
            Toy_Figurine = 1003,
            [EnumTitle("Home_Living")]
            Home_Living = 1004,
            [EnumTitle("Tech_Tool")]
            Tech_Tool = 1005,
            [EnumTitle("Learning")]
            Learning = 1006,
            [EnumTitle("Contest")]
            Contest = 1007
        }
    }
}
