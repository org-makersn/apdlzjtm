using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Design.Web.Front.Helper;
using Makersn.BizDac;
using Makersn.Models;
using PagedList;
using Design.Web.Front.Filter;
using System.Text.RegularExpressions;
using Design.Web.Front.Models;
using System.Web;
using System.IO;
using Makersn.Util;
using System.Web.Security;
using Newtonsoft.Json;


namespace Design.Web.Front.Controllers
{
    public class ProfileController : BaseController
    {
        MemberDac _memberDac = new MemberDac();
        ArticleDac _articleDac = new ArticleDac();
        LikesDac _likesDac = new LikesDac();
        NoticesDac _noticesDac = new NoticesDac();
        FollowerDac _followerDac = new FollowerDac();
        MessageDac _messageDac = new MessageDac();
        ListDac _listDac = new ListDac();
        PrinterMemberDac _printerMemberDac = new PrinterMemberDac();
        TranslationDetailDac _translationDetailDac = new TranslationDetailDac();
        private static int _receiveMemberNo;
        private static string _val1;
        private static string _val2;

        /// <summary>
        /// getmembernoblogurl2 list2
        /// </summary>
        /// <param name="no"></param>
        /// <param name="gubun"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult Index(string no = "", string gubun = "U", string url = "", int page = 1, int listNo = 0)
        {
            if (no == "" && Profile.UserNo == 0 && url == "") return Redirect("/");

            int memberNo = 0;
            if (no == "") { no = Base64Helper.Base64Encode(Profile.UserNo.ToString()); }
            memberNo = int.Parse(Base64Helper.Base64Decode(no));

            MemberT member = new MemberT();

            if (url != "")
            {
                member = _memberDac.GetMemberNoByBlogUrl2(url);
                if (member == null) { return Content("<script type='text/javascript'>alert('잘못된 주소입니다.'); location.href='/'</script>"); }
                no = Base64Helper.Base64Encode(member.No.ToString());
                memberNo = member.No;
            }
            else
            {
                member = _memberDac.GetMemberProfile(memberNo);
            }


            if (member.DelFlag == "Y") { return Redirect("returnMainPage"); }

            int visitorNo = Profile.UserNo;
            if (gubun == "D" && visitorNo != memberNo) { return Redirect("/Profile?no=" + Base64Helper.Base64Encode(visitorNo.ToString()) + "&gubun=" + gubun); }; // 임시게시물 다른경로 차단

            ViewBag.No = no;
            ViewBag.VisitorNo = visitorNo;
            ViewBag.CheckFollow = _followerDac.CheckFollow(memberNo, visitorNo);
            ViewBag.CntList = _memberDac.GetCntList(memberNo);
            ViewBag.CheckSelf = memberNo == visitorNo ? 1 : 0;

            ViewBag.Gubun = gubun;
            ViewBag.Page = page;
            ViewBag.ListNo = listNo;

            ViewBag.ContClass = "w100";
            ViewBag.Url = url;

            if (member.ProfileMsg != null)
            {
                member.ProfileMsg = new ContentFilter().HtmlEncode(member.ProfileMsg);
                member.ProfileMsg = CreateATag(member.ProfileMsg);
            };
            ViewBag.PrinterMember = _printerMemberDac.CheckSpotOpen(memberNo);
            return View(member);
        }

        #region 본인게시물,좋아요,숨김처리 게시물 리스팅
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="no"></param>
        /// <param name="gubun"></param>
        /// <returns></returns>
        public PartialViewResult Lists(int page, string no, string gubun)
        {
            string langFlag = string.Empty;

            if (Request.Cookies.AllKeys.Contains("GlobalFlag"))
            {
                langFlag = Request.Cookies["GlobalFlag"].Value;
            }
            else
            {
                langFlag = ViewBag.LangFlag;
            }

            if (langFlag == "ALL")
                langFlag = "";

            if (no == "") { no = Base64Helper.Base64Encode(Profile.UserNo.ToString()); }
            ViewBag.No = no;

            no = Base64Helper.Base64Decode(no);

            int visitorNo = Profile.UserNo;

            IList<ArticleT> before = _articleDac.GetMemberArticleByNo(no, gubun, visitorNo);

            IList<ArticleT> list = new List<ArticleT>();
            foreach (ArticleT article in before)
            {
                TranslationDetailT trans = _translationDetailDac.GetTranslationDetailByArticleNoAndLangFlag(article.No, langFlag);
                if (trans != null)
                {
                    article.Title = trans.Title;
                }
                list.Add(article);
            }

            ViewBag.Gubun = gubun;
            ViewBag.VisitorNo = visitorNo;
            ViewBag.CheckSelf = int.Parse(no) == visitorNo ? 1 : 0;

            return PartialView(list.ToPagedList(page, 40));
        }
        //public ActionResult Lists(int page = 1, string no = "", string gubun = "", string url = "")
        //{
        //    if (url != "")
        //    {
        //        int memberNo = memberDac.GetMemberNoByBlogUrl(url);
        //        if (memberNo == 0) { return Content("<script type='text/javascript'>alert('잘못된 주소입니다.'); location.href='/'</script>"); }
        //        no = Base64Helper.Base64Encode(memberNo.ToString());
        //    }
        //    if (no == "" && Profile.UserNo == 0) return Redirect("/");
        //    if (no == "") { no = Base64Helper.Base64Encode(Profile.UserNo.ToString()); }
        //    ViewBag.No = no;

        //    no = Base64Helper.Base64Decode(no);

        //    MemberT member = memberDac.GetMemberProfile(int.Parse(no));//탈퇴회원 체크
        //    if (member.DelFlag == "Y") { return Redirect("returnMainPage"); }

        //    int visitorNo = Profile.UserNo;
        //    if (gubun == "D" && visitorNo != int.Parse(no)) { return Redirect("/profile/lists?no=" + Base64Helper.Base64Encode(visitorNo.ToString()) + "&gubun=" + gubun); }; // 임시게시물 다른경로 차단

        //    //if (gubun == "D" && visitorNo != int.Parse(no)) { return WrongAccessPath(); };
        //    IList<ArticleT> list = articleDac.GetMemberArticleByNo(no, gubun, visitorNo);
        //    ViewBag.Gubun = gubun;
        //    ViewBag.VisitorNo = visitorNo;

        //    return View(list.ToPagedList(page, 20));
        //}
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="no"></param>
        /// <param name="gubun"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ArticleView(int page = 1, string memberNo = "", string gubun = "")
        {
            if (memberNo == "") { memberNo = Base64Helper.Base64Encode(Profile.UserNo.ToString()); }
            ViewBag.No = memberNo;

            memberNo = Base64Helper.Base64Decode(memberNo);

            int visitorNo = Profile.UserNo;

            IList<ArticleT> list = _articleDac.GetMemberArticleByNo(memberNo, gubun, visitorNo);
            ViewBag.Gubun = gubun;
            ViewBag.VisitorNo = visitorNo;

            return PartialView(list.ToPagedList(page, 20));
        }

        #region 프로필 상단 파샬뷰 (사용안함)
        public PartialViewResult TopArea(string no)
        {
            int visitorNo = Profile.UserNo;
            int memberNo = int.Parse(Base64Helper.Base64Decode(no));
            ViewBag.No = no;
            ViewBag.VisitorNo = visitorNo;
            ViewBag.CheckFollow = _followerDac.CheckFollow(memberNo, visitorNo);


            ViewBag.CntList = _memberDac.GetCntList(memberNo);

            MemberT member = _memberDac.GetMemberProfile(memberNo);

            if (member.ProfileMsg != null) { member.ProfileMsg = new ContentFilter().HtmlEncode(member.ProfileMsg); };
            return PartialView(member);
        }
        #endregion

        #region 알림 리스팅
        [Authorize]
        public PartialViewResult Notice(int page = 1)
        {
            ViewBag.NoticeCnt = 0;
            int no = Profile.UserNo;
            ViewBag.No = Base64Helper.Base64Encode(no.ToString());
            
            IList<NoticeT> list = _noticesDac.GetNoticeList(no);

            //IList<NoticeT> outputImage = new List<NoticeT>();

            //outputImage = list.Where(w => w.Type == "outputImage").ToList<NoticeT>();
            //list = list.Where(w => w.Type != "outputImage").ToList<NoticeT>();

            //list = _noticesDac.GetPrinterOutputImage(outputImage, list);


            _noticesDac.UpdateNoticeIsNew(no);

            return PartialView(list.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
        }
        //public ActionResult Notice(int page = 1)
        //{
        //    ViewBag.NoticeCnt = 0;
        //    int no = Profile.UserNo;
        //    ViewBag.No = Base64Helper.Base64Encode(no.ToString());
        //    //no = Base64Helper.Base64Decode(no);
        //    //NoticesDac noticesDac = new NoticesDac();

        //    IList<NoticeT> list = noticesDac.GetNoticeList(no);
        //    noticesDac.UpdateNoticeIsNew(no);

        //    return View(list.OrderByDescending(o => o.RegDt).ToPagedList(page, 11));
        //}
        #endregion

        #region 팔로워, 팔로잉 페이지
        public PartialViewResult Follow(int page = 1, string no = "0", string gubun = "ing")
        {
            ViewBag.No = no;
            no = Base64Helper.Base64Decode(no);
            //MemberT member = memberDac.GetMemberProfile(int.Parse(no));//탈퇴회원 체크
            //if (member.DelFlag == "Y") { return Redirect("returnMainPage"); }
            ViewBag.Gubun = gubun;
            IList<FollowerT> list = new List<FollowerT>();
            switch (gubun)
            {
                case "ing":
                    list = _followerDac.GetFollowingList(int.Parse(no), Profile.UserNo);
                    break;
                case "wer":
                    list = _followerDac.GetFollowerLIst(int.Parse(no), Profile.UserNo);
                    break;
            }
            ViewBag.CheckSelf = int.Parse(no) == Profile.UserNo ? 1 : 0;

            return PartialView(list.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
        }
        //public ActionResult Follow(int page = 1, string no = "0", string gubun = "ing")
        //{
        //    ViewBag.No = no;
        //    no = Base64Helper.Base64Decode(no);
        //    MemberT member = memberDac.GetMemberProfile(int.Parse(no));//탈퇴회원 체크
        //    if (member.DelFlag == "Y") { return Redirect("returnMainPage"); }
        //    ViewBag.Gubun = gubun;
        //    IList<FollowerT> list = new List<FollowerT>();
        //    switch (gubun)
        //    {
        //        case "ing":
        //            list = followerDac.GetFollowingList(int.Parse(no), Profile.UserNo);
        //            break;
        //        case "wer":
        //            list = followerDac.GetFollowerLIst(int.Parse(no), Profile.UserNo);
        //            break;
        //    }

        //    return View(list.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
        //}
        #endregion

        #region 탈퇴 회원일시 리턴
        public ActionResult returnMainPage()
        {
            return Content("<script type='text/javascript'>alert('탈퇴한 회원입니다.'); location.href='/'</script>");
        }
        #endregion

        #region 팔로우 설정
        [Authorize]
        public JsonResult SetFollow(string memberNo)
        {
            memberNo = Base64Helper.Base64Decode(memberNo);
            //int visitorNo = Profile.UserNo;

            FollowerT follow = new FollowerT();
            follow.MemberNo = Profile.UserNo;
            follow.MemberNoRef = int.Parse(memberNo);
            follow.RegId = Profile.UserId;
            follow.RegDt = DateTime.Now;
            follow.IsNew = "Y";

            NoticeT notice = new NoticeT();
            //notice.

            int result = _followerDac.SetFollow(follow);
            return Json(new { Result = result });
        }
        #endregion

        #region 프로필 세팅
        [Authorize]
        public PartialViewResult Setting()
        {
            string memberNo = Base64Helper.Base64Encode(Profile.UserNo.ToString());
            ViewBag.No = memberNo;
            MemberT member = _memberDac.GetMemberProfile(Profile.UserNo);
            if (member.ProfileMsg != null) { member.ProfileMsg = new ContentFilter().HtmlDecode(member.ProfileMsg); };
            return PartialView(member);
        }
        //public ActionResult Setting()
        //{
        //    string memberNo = Base64Helper.Base64Encode(Profile.UserNo.ToString());
        //    ViewBag.No = memberNo;
        //    MemberT member = memberDac.GetMemberProfile(Profile.UserNo);
        //    if (member.ProfileMsg != null) { member.ProfileMsg = new ContentFilter().HtmlDecode(member.ProfileMsg); };
        //    return View(member);
        //}
        #endregion

        #region
        public JsonResult GetNoticeCnt()
        {
            int noticeCnt = _noticesDac.GetNoticesCntByMemberNo(Profile.UserNo);
            int MessageCnt = _messageDac.GetNewMessageCount(Profile.UserNo);
            //ViewBag.NoticeCnt = result;
            return Json(new { notice = noticeCnt, message = MessageCnt });
        }
        #endregion

        #region 프로필 수정
        [HttpPost]
        [Authorize]
        public JsonResult UpdateProfile(string no, string name, string blog, string email, string webUrl, string pw, string rePw, string profileMsg, string armAll, string armReply,
                                        string armLikes, string armFollower)
        {
            MemberT member = new MemberT();

            int memberNo = int.Parse(Base64Helper.Base64Decode(no));
            if (memberNo != Profile.UserNo) { return Json(new { Success = false, Result = 1 }); }

            if ((pw != rePw && pw != "******") || pw == "") { return Json(new { Success = false, Result = 2 }); }
            else
            {
                member.Password = pw;
                member.UpdPasswordDt = DateTime.Now;
            }

            int reChkBlog = 0;
            if (blog.Trim() != "") { reChkBlog = _memberDac.CheckBlogUrl(blog, memberNo); }

            if (reChkBlog > 0) { return Json(new { Success = false, Result = 3 }); }
            else { { member.BlogUrl = blog; } }

            var chkContoller = EnumHelper.GetEnumDictionaryT<MakersnEnumTypes.ControllerList>();
            foreach (var ctrName in chkContoller)
            {
                if (blog.ToLower() == ctrName.Key.ToLower())
                {
                    return Json(new { Success = false, Result = 3 });
                }
            }

            if (!chkEmail(email) && email != "") { return Json(new { Success = false, Result = 4 }); }
            if (!chkEng(blog) && blog != "") { return Json(new { Success = false, Result = 5 }); }
            if (name.Length > 20 || profileMsg.Length > 150) { return Json(new { Success = false, Result = 6 }); }


            member.No = memberNo;
            member.Name = name;
            member.Email = email;
            member.Url = webUrl;
            member.ProfileMsg = profileMsg;
            member.AllIs = armAll;
            member.RepleIs = armReply;
            member.LikeIs = armLikes;
            member.FollowIs = armFollower;
            member.UpdId = Profile.UserId;
            member.UpdDt = DateTime.Now;

            int result = _memberDac.UpdateMember(member);
            //이메일 변경 보류
            //if (result == 7)
            //{
            //    sendEmailCertify(member.No, member.Name, member.Email);
            //}

            return Json(new { Success = true, Result = result });
        }
        #endregion

        #region email인증
        public void sendEmailCertify(int memberNo, string name, string email)
        {
            string no = Base64Helper.Base64Encode(memberNo.ToString());
            string Subject = "makersN 이메일 변경 인증";
            string change = "http://www.makersn.com/account/ChangeEmailCertify?chk=" + no;
            string cancel = "http://www.makersn.com/account/CancleEmailCertify?chk=" + no;

            SendMailModels oMail = new SendMailModels();
            oMail.SendMail("chgMailCertify", email, new String[] { Subject, name, change, cancel });
        }
        #endregion

        #region 회원탈퇴
        [Authorize]
        public JsonResult DropMember(string dropComment)
        {
            _memberDac.DeleteMember(Profile.UserNo, dropComment);
            return Json(new { Success = true });
        }
        #endregion

        #region check blog url
        /// <summary>
        /// check blog url
        /// </summary>
        /// <param name="no"></param>
        /// <param name="blog"></param>
        /// <returns></returns>
        public JsonResult ChkBlogUrl(string no, string blog)
        {
            if (!chkEng(blog) && blog != "") { return Json(new { Result = 2 }); }

            var chkContoller = EnumHelper.GetEnumDictionaryT<MakersnEnumTypes.ControllerList>();
            foreach (var ctrName in chkContoller)
            {
                if (blog.ToLower() == ctrName.Key.ToLower())
                {
                    return Json(new { Success = false, Result = 3 });
                }
            }
            if (blog.Length < 5 || blog.Length > 20)
            {
                return Json(new { Success = false, Result = 4 });
            }

            int memberNo = int.Parse(Base64Helper.Base64Decode(no));
            int result = _memberDac.CheckBlogUrl(blog, memberNo);
            return Json(new { Result = result });
        }
        #endregion

        #region Message
        /// <summary>
        /// Message
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [Authorize]
        public PartialViewResult Message(string no = "")
        {
            ViewBag.GetMessage = null;
            string base64MemberNo = Base64Helper.Base64Encode(Profile.UserNo.ToString());
            if (no != "" && no != base64MemberNo)
            {
                _val1 = no;
                _val2 = base64MemberNo;
                ViewBag.GetMessage = "on";
            }
            ViewBag.No = base64MemberNo;
            int memberNo = Profile.UserNo;

            IList<MessageT> message = _messageDac.GetMessageList(memberNo);
            _messageDac.UpdateMessageIsNew(memberNo);

            return PartialView(message);
        }
        //public ActionResult Message(string no = "")
        //{
        //    if (no != "")
        //    {
        //        _val1 = no;
        //        _val2 = Base64Helper.Base64Encode(Profile.UserNo.ToString());
        //        ViewBag.GetMessage = "on";
        //    }
        //    ViewBag.No = Base64Helper.Base64Encode(Profile.UserNo.ToString());
        //    int memberNo = Profile.UserNo;

        //    IList<MessageT> message = messageDac.GetMessageList(memberNo);
        //    messageDac.UpdateMessageIsNew(memberNo);
        //    return View(message);
        //}
        #endregion

        #region message room
        /// <summary>
        /// message room
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        [Authorize]
        public PartialViewResult MessageRoom(string val1 = "", string val2 = "")
        {
            if (val1 != "" && val2 != "") { _val1 = val1; _val2 = val2; }
            else { val1 = _val1; val2 = _val2; }

            int sendMemberNo = int.Parse(Base64Helper.Base64Decode(val1));
            int receiveMemberNo = int.Parse(Base64Helper.Base64Decode(val2));

            int memberNo = Profile.UserNo;
            //string roomName = "";
            //if (val1 == memberNo.ToString()) { roomName = val1 + "_" + val2; }
            //else { roomName = val2 + "_" + val1; }

            if (sendMemberNo == memberNo) { _receiveMemberNo = receiveMemberNo; }
            else { _receiveMemberNo = sendMemberNo; }

            IList<MessageT> message = _messageDac.GetMessageByRoomName(memberNo, sendMemberNo, receiveMemberNo);
            ViewBag.MemberNo = memberNo;
            ViewBag.Name = _memberDac.GetMemberProfile(_receiveMemberNo).Name;
            ViewBag.MsgImgThumb = System.Configuration.ConfigurationManager.AppSettings["msgImgThumb"];
            ViewBag.MsgImgOri = System.Configuration.ConfigurationManager.AppSettings["msgImgOri"];
            return PartialView(message);
        }
        #endregion

        #region message box
        /// <summary>
        /// message box
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public PartialViewResult MessageBox()
        {
            return PartialView();
        }
        #endregion

        #region send message
        /// <summary>
        /// send message
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [Authorize]
        public JsonResult SendMessage(string comment)
        {
            if (comment != null && comment != "")
            {
                MessageT message = new MessageT();
                message.MemberNo = Profile.UserNo;
                message.MemberNoRef = _receiveMemberNo;
                message.Comment = comment;
                message.RoomName = message.MemberNo + "_" + message.MemberNoRef;
                message.IsNew = "Y";
                message.DelFlag = "N";
                message.RegId = Profile.UserId;
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                message.RegDt = dt.AddSeconds(1); ;
                message.RegIp = IPAddressHelper.GetClientIP();

                message.MsgGubun = "MSG";

                _messageDac.AddMessage(message);
            }


            return Json(new { Result = 1 });
        }
        #endregion

        #region test chat
        [Authorize]
        public ActionResult testChat()
        {
            return View();
        }
        #endregion

        #region 이메일 정규식
        private bool chkEmail(string email)
        {
            bool emailCheck = Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            if (emailCheck)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 영어 정규식
        private bool chkEng(string text)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9]*$");
            Boolean ismatch = regex.IsMatch(text);
            if (ismatch)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 프로필 이미지 업로드
        /// <summary>
        /// 프로필 이미지 업로드
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProfileImgUpload(FormCollection collection)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;
            string fileName = string.Empty;

            HttpPostedFileBase membPic = Request.Files["memb_pic"];
            if (membPic != null)
            {
                if (membPic.ContentLength > 0)
                {
                    string[] extType = { "jpg", "png", "gif" };

                    string extension = Path.GetExtension(membPic.FileName).ToLower().Replace(".", "").ToLower();

                    if (extType.Contains(extension))
                    {
                        //update
                        MemberT memberT = _memberDac.GetMemberProfile(Profile.UserNo);
                        //save img,
                        fileName = FileUpload.UploadFile(membPic, new ImageSize().GetProfileResize(), "Profile", memberT.ProfilePic);

                        memberT.ProfilePic = fileName;
                        memberT.UpdDt = DateTime.Now;
                        memberT.UpdId = Profile.UserId;

                        int memberNo = _memberDac.UpdateProfilePic(memberT);
                        if (memberNo > 0)
                        {
                            response.Success = true;
                            response.Result = fileName;
                        }
                    }
                    else
                    {
                        response.Message = "gif, jpg, png 형식 파일만 가능합니다.";
                    }
                }
            }

            if (Profile.UserLevel < 50)
            {
                ProfileModel reProfile = new ProfileModel();
                reProfile.UserNo = Profile.UserNo;
                reProfile.UserNm = Profile.UserNm;
                reProfile.UserId = Profile.UserId;
                reProfile.UserProfilePic = fileName;
                reProfile.UserLevel = Profile.UserLevel;
                var hashJson = JsonConvert.SerializeObject(reProfile);
                FormsAuthentication.SignOut();
                FormsAuthentication.SetAuthCookie(hashJson, false);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult DeleteProfileImg(string memberNo)
        {
            int no = 0;
            if (Profile.UserLevel < 50) { no = Profile.UserNo; }
            else { no = int.Parse(Base64Helper.Base64Decode(memberNo)); }
            bool result = _memberDac.DeleteProfilePic(no);

            if (Profile.UserLevel < 50)
            {
                ProfileModel reProfile = new ProfileModel();
                reProfile.UserNo = Profile.UserNo;
                reProfile.UserNm = Profile.UserNm;
                reProfile.UserId = Profile.UserId;
                reProfile.UserProfilePic = "";
                reProfile.UserLevel = Profile.UserLevel;
                var hashJson = JsonConvert.SerializeObject(reProfile);
                FormsAuthentication.SignOut();
                FormsAuthentication.SetAuthCookie(hashJson, false);
            }
            //if (Request.Cookies[".ASPXAUTH"] != null)
            //{
            //    var value = Request.Cookies[".ASPXAUTH"].Value;
            //}

            return Json(new { Success = result });
        }

        #region 메시지 이미지 보내기
        [HttpPost]
        public JsonResult sendImage(FormCollection collection)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;
            string fileName = string.Empty;

            HttpPostedFileBase imgupload = Request.Files["imgupload"];
            string text = collection["txtMessage"];

            if (imgupload != null)
            {
                if (imgupload.ContentLength > 0)
                {
                    string[] extType = { "jpg", "png", "gif" };

                    string extension = Path.GetExtension(imgupload.FileName).ToLower().Replace(".", "").ToLower();

                    if (extType.Contains(extension))
                    {
                        fileName = FileUpload.UploadFile(imgupload, new ImageSize().GetMsgFIleResize(), "Msg_File", null);

                        MessageT message = new MessageT();
                        message.MemberNo = Profile.UserNo;
                        message.MemberNoRef = _receiveMemberNo;
                        message.Comment = fileName;
                        message.RoomName = message.MemberNo + "_" + message.MemberNoRef;
                        message.IsNew = "Y";
                        message.DelFlag = "N";
                        message.RegId = Profile.UserId;
                        message.RegDt = DateTime.Now;
                        message.RegIp = IPAddressHelper.GetClientIP();

                        message.MsgGubun = "IMG";

                        _messageDac.AddMessage(message);

                        if (text != null)
                        {
                            SendMessage(text);
                        }

                        response.Success = true;
                        //response.Result = articleFileNo.ToString();
                    }
                    else
                    {
                        response.Message = "gif, jpg, png 형식 파일만 가능합니다.";
                    }
                }
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult Collection(string no, string userNm)
        {
            int memberNo = int.Parse(Base64Helper.Base64Decode(no));
            ViewBag.ItemList = _listDac.GetListItem(memberNo);
            ViewBag.ListNameList = _listDac.GetListNames(memberNo);
            ViewBag.No = no;
            ViewBag.UserNm = userNm;
            ViewBag.CheckSelf = memberNo == Profile.UserNo ? 1 : 0;
            return View();
        }

        public ActionResult CollectionDetail(string no = "", int page = 1, int listNo = 0)
        {
            if (no == "") { no = Base64Helper.Base64Encode(Profile.UserNo.ToString()); }
            ViewBag.No = no;

            no = Base64Helper.Base64Decode(no);

            int visitorNo = Profile.UserNo;
            IList<ArticleT> list = _listDac.GetMemberListItems(no, visitorNo, listNo);
            ViewBag.List = _listDac.GetSingleListbyListNo(listNo);
            ViewBag.VisitorNo = visitorNo;
            ViewBag.ListNo = listNo;
            ViewBag.ChangeAble = (int.Parse(no) == visitorNo) || (Profile.UserLevel > 50);

            return View(list.ToPagedList(page, 20));
        }

        public JsonResult DeleteArticleInList(int articleNo, int listNo)
        {
            ListArticleT list = new ListArticleT();
            list.MemberNo = Profile.UserNo;
            list.ArticleNo = articleNo;
            list.ListNo = listNo;
            bool result = _listDac.DeleteArticleInList(list);
            return Json(new { Success = result });
        }

        #region user message room
        /// <summary>
        /// user message room
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        [Authorize]
        public PartialViewResult UserMessageRoom(string val1 = "")
        {
            int memberNo = Profile.UserNo;

            int sendMemberNo = int.Parse(Base64Helper.Base64Decode(val1));
            int receiveMemberNo = memberNo;

            //string roomName = "";
            //if (val1 == memberNo.ToString()) { roomName = val1 + "_" + val2; }
            //else { roomName = val2 + "_" + val1; }

            _receiveMemberNo = sendMemberNo;

            IList<MessageT> message = _messageDac.GetMessageByRoomName(memberNo, sendMemberNo, receiveMemberNo);
            ViewBag.MemberNo = memberNo;
            ViewBag.Name = _memberDac.GetMemberProfile(_receiveMemberNo).Name;
            ViewBag.MsgImgThumb = System.Configuration.ConfigurationManager.AppSettings["msgImgThumb"];
            ViewBag.MsgImgOri = System.Configuration.ConfigurationManager.AppSettings["msgImgOri"];
            return PartialView(message);
        }
        #endregion

        #region list 이름 변경
        public JsonResult ChgListName(string listName, int no)
        {
            ListT list = new ListT();
            list.ListName = listName;
            list.No = no;
            list.MemberNo = Profile.UserNo;
            bool result = _listDac.UpdateListName(list);
            return Json(new { Success = result });
        }
        #endregion

        #region
        public JsonResult DeleteList(int no)
        {
            ListT list = new ListT();
            list.No = no;
            list.MemberNo = Profile.UserNo;
            bool result = _listDac.DeleteList(list);
            return Json(new { Success = result });
        }
        #endregion


        #region 스팟 오픈
        public PartialViewResult SpotOpen()
        {
            int memberNo = Profile.UserNo;
            PrinterMemberT printerMember = _printerMemberDac.GetPrinterMemberByNo(memberNo);
            if (printerMember == null) {
                printerMember = new PrinterMemberT();
                printerMember.MemberNo = memberNo;
                printerMember.SpotAddress = "";
                printerMember.Bank = "";
                printerMember.PostMode = 2;
                printerMember.SpotName = "";
                printerMember.PrinterProfileMsg = "";
                printerMember.TaxbillFlag = "Y";
                printerMember.HomePhone = "";
                printerMember.CellPhone = "";
                printerMember.ViewCnt = 0;
                printerMember.BankName = "";
            }
            return PartialView(printerMember);
        }
        #endregion
        #region 스팟 오픈 정보 업데이트
        public JsonResult SpotInfoUpd(FormCollection collection)
        {
            bool success = false;
            
            PrinterMemberT printerMember = null;

            printerMember = _printerMemberDac.GetPrinterMemberByNo(Profile.UserNo);
            if(printerMember == null){
                printerMember= new PrinterMemberT();
            }

            printerMember.MemberNo = Profile.UserNo;
            printerMember.SpotName = collection["name"];
            printerMember.SpotUrl = collection["url"];
            printerMember.PrinterProfileMsg = collection["comment"];
            printerMember.HomePhone = collection["home_tel1"] + "-" + collection["home_tel2"] + "-" + collection["home_tel3"];
            printerMember.CellPhone = collection["cell_tel1"] + "-" + collection["cell_tel2"] + "-" + collection["cell_tel3"];
            printerMember.SpotAddress = collection["address"];
            //printerMember.LocX = System.Convert.ToDouble(collection["locationX"]);
            //printerMember.LocY = System.Convert.ToDouble(collection["locationY"]);
            printerMember.LocX = 0;
            printerMember.LocY = 0;
            printerMember.AccountNo = collection["accountNum"];
            printerMember.Bank = collection["bank"];
            printerMember.BankName = collection["bankName"];
            printerMember.PostMode = System.Convert.ToInt32(collection["post"]) * 2 + System.Convert.ToInt32(collection["pickUp"]);

            if (printerMember.PostMode == (int)MakersnEnumTypes.PostType.택배 || printerMember.PostMode == (int)MakersnEnumTypes.PostType.픽업택배)
            {
                printerMember.PostType = System.Convert.ToInt32(collection["postType"]);

                if (printerMember.PostType == (int)MakersnEnumTypes.PrinterPostType.고정배송비)
                {
                    printerMember.PostPrice = System.Convert.ToInt32(collection["postPrice"]);
                }
            }

            printerMember.TaxbillFlag = collection["taxBillFlag"];

            printerMember.PrinterProfilePic = Profile.UserProfilePic;
            //printerMember.SaveFlag = collection["saveFlag"];

            printerMember.RegDt = System.DateTime.Now;
            printerMember.RegId = Profile.UserId;

            _printerMemberDac.InsertPrinterMember(printerMember);

            success = true;
            //if (printerMember.SaveFlag == "Y"){
            //    success = true;
            //}

            return Json(new { Success = success });

        }
        #endregion


        public PartialViewResult SpotDone() {
            return PartialView();
        }

        #region A태그 변환
        private string CreateATag(string contents)
        {
            //프로토콜부분 - 있을수도 없을수도
            //string ptProtocol = "(?:(ftp|https?|mailto|telnet):\\/\\/)?";
            string ptProtocol = "(?:(ftp|https?|mailto|telnet):\\/\\/)";
            //domain의 기본 골격은 daum.net
            string domain = @"[a-zA-Z]\w+\.[a-zA-Z]\w+(\.\w+)?(\.\w+)?";
            //도메인 뒤에 추가로 붙는 서브url 및 파라미터들
            //이부분이 아직은 미흡하여 오류가 가끔 일어난다.
            string adds = "([:?=&/%.-]+\\w+)*";
            Regex rgxDomain = new Regex(ptProtocol + domain + adds, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match matchDomain = rgxDomain.Match(contents);

            Regex rgxDomainNonProt = new Regex("www." + domain + adds, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match matchDomainNonProt = rgxDomainNonProt.Match(contents);

            Regex rgxEmail = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match matchEmail = rgxEmail.Match(contents);

            string[] emailList = new string[matchEmail.Length];
            int index = 0;
            while (matchEmail.Success)
            {
                if (Regex.IsMatch(matchEmail.Value, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                {
                    emailList[index] = "<a href='mailto:" + matchEmail.Value + "'>" + matchEmail.Value + "</a>";
                    contents = contents.Replace(matchEmail.Value, "#" + index + "#");
                    index++;
                }
                matchEmail = matchEmail.NextMatch();
            }

            IList<string> domainList = new List<string>();
            contents += "&nbsp";
            while (matchDomain.Success)
            {
                string replaceDomain = matchDomain.Value.Replace("&nbsp", "");
                if (!domainList.Contains(replaceDomain))
                {
                    contents = contents.Replace(replaceDomain + "&nbsp", "<a href='" + replaceDomain + "' style='color:#ff7900' target='_blank'>" + replaceDomain + "</a>");
                    contents = contents.Replace(replaceDomain + "\r", "<a href='" + replaceDomain + "' style='color:#ff7900' target='_blank'>" + replaceDomain + "</a>");
                    contents = contents.Replace(replaceDomain + "<br/>", "<a href='" + replaceDomain + "' style='color:#ff7900' target='_blank'>" + replaceDomain + "</a><br/>");

                    domainList.Add(replaceDomain);
                    matchDomain = matchDomain.NextMatch();
                }
                else
                {
                    matchDomain = matchDomain.NextMatch();
                }


            }

            IList<string> domainList2 = new List<string>();
            while (matchDomainNonProt.Success)
            {
                string replaceDomain = matchDomainNonProt.Value.Replace("&nbsp", "");
                if (!domainList2.Contains(replaceDomain))
                {
                    contents = contents.Replace(replaceDomain + "&nbsp", "<a href='http://" + replaceDomain + "' style='color:#ff7900' target='_blank'>" + replaceDomain + "</a>");
                    contents = contents.Replace(replaceDomain + "\r", "<a href='http://" + replaceDomain + "' style='color:#ff7900' target='_blank'>" + replaceDomain + "</a>");
                    contents = contents.Replace(replaceDomain + "<br/>", "<a href='http://" + replaceDomain + "' style='color:#ff7900' target='_blank'>" + replaceDomain + "</a><br/>");

                    domainList2.Add(replaceDomain);
                    matchDomainNonProt = matchDomainNonProt.NextMatch();
                }
                else
                {
                    matchDomainNonProt = matchDomainNonProt.NextMatch();
                }


            }

            for (int i = 0; i < emailList.Length; i++)
            {
                contents = contents.Replace("#" + i + "#", emailList[i]);
            }

            return contents;
        }
        #endregion
    }
}
