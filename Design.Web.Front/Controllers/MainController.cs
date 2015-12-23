using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Makersn.Util;
using Makersn.BizDac;
using Makersn.Models;
using Library.ObjParser;
using Design.Web.Front.Helper;
using Design.Web.Front.Models;
using QuantumConcepts.Formats.StereoLithography;
using Design.Web.Front.Filter;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Design.Web.Front.Controllers
{
    //[Authorize]
    public class MainController : BaseController
    {
        ArticleDac _articleDac = new ArticleDac();
        MemberT _member = new MemberT();
        MemberDac _memberDac = new MemberDac();
        FollowerDac _followerDac = new FollowerDac();


        private Extent Size { get; set; }

        public ActionResult Index(string url = "", string gl = "", string no = "", string gubun = "U", int page = 1, int listNo = 0)
        {
            if (url != "")
            {
                int memberNo = 0;
                //ActionResult action = new ProfileController().Index("", "U", url, 1, 0);
                MemberT member = _memberDac.GetMemberNoByBlogUrl2(url);
                if (member == null) { return Content("<script type='text/javascript'>alert('잘못된 주소입니다.'); location.href='/'</script>"); }
                no = Base64Helper.Base64Encode(member.No.ToString());
                memberNo = member.No;

                if (member.DelFlag == "Y") { return Redirect("returnMainPage"); }

                int visitorNo = Profile.UserNo;

                ViewBag.No = no;
                ViewBag.VisitorNo = visitorNo;
                ViewBag.CheckFollow = _followerDac.CheckFollow(memberNo, visitorNo);
                ViewBag.CntList = _memberDac.GetCntList(memberNo);
                ViewBag.CheckSelf = memberNo == visitorNo ? 1 : 0;

                ViewBag.Gubun = gubun;
                ViewBag.Page = page;
                ViewBag.ListNo = listNo;

                ViewBag.ContClass = "w100";

                if (member.ProfileMsg != null) { member.ProfileMsg = new ContentFilter().HtmlEncode(member.ProfileMsg); };



                return View("~/Views/profile/index.cshtml", member);

//                return Content(@"<form action='/profile' id='goBlog' method='post'>
//                                            <input type='hidden' name='url' value='" + url + @"' />
//                                             </form><script>document.getElementById('goBlog').submit();</script>");
                #region
                //                var chkContoller = EnumHelper.GetEnumDictionaryT<MakersnEnumTypes.ControllerList>();
                //                foreach (var ctrName in chkContoller)
                //                {
                //                    if (url.ToLower() == ctrName.Key.ToLower())
                //                    {
                //                        return Redirect(url + "/index");
                //                    }
                //                }

                //                member = memberDac.GetMemberNoByBlogUrl2(url);
                //                if (member == null) { return Content("<script type='text/javascript'>alert('잘못된 주소입니다.'); location.href='/'</script>"); }
                //                //else { return Redirect("/profile/index?url=" + url); }
                //                else
                //                {
                //                    //return RedirectToAction("index", "profile", new { url = url});
                //                    return Content(@"<form action='/profile' id='goBlog' method='post'>
                //                                            <input type='hidden' name='url' value='" + url + @"' />
                //                                             </form><script>document.getElementById('goBlog').submit();</script>");
                //                }
                #endregion
            }

            if (!Request.Cookies.AllKeys.Contains("FirstLoadKr"))
            {
                HttpCookie cookie = new HttpCookie("FirstLoadKr");
                cookie.Domain = ".makersn.com";
                cookie.Value = "Y";
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("GlobalFlag"))
                {
                    HttpCookie cookie1 = this.ControllerContext.HttpContext.Request.Cookies["GlobalFlag"];
                    cookie1.Expires = DateTime.Now.AddDays(-1);
                    this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                }

                if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("FirstLoadEn"))
                {
                    HttpCookie cookie2 = this.ControllerContext.HttpContext.Request.Cookies["FirstLoadEn"];
                    cookie2.Expires = DateTime.Now.AddDays(-1);
                    this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                }
            }

            if (gl == "")
            {
                if (Request.Cookies.AllKeys.Contains("GlobalFlag"))
                {
                    gl = Request.Cookies["GlobalFlag"].Value;
                }
                else {
                    HttpCookie cookie = new HttpCookie("GlobalFlag");
                    cookie.Value = ViewBag.LangFlag;
                    cookie.Domain = ".makersn.com";
                    gl = ViewBag.LangFlag;
                    this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                }
            }
            else
            {
                ViewBag.LangFlag = gl;
            }

            switch (gl)
            {
                case "EN": ViewBag.LangFlagName = "English"; break;
                case "KR": ViewBag.LangFlagName = "한국어"; break;
                default: ViewBag.LangFlagName = "모든언어"; break;
            }

            ViewBag.IsMain = "Y";
            IList<ArticleDetailT> recommendList = new List<ArticleDetailT>();
            var stateList = EnumHelper.GetTitleAndSynonyms<MakersnEnumTypes.CateName>();

            gl = gl == "ALL" ? "" : gl;

            recommendList = _articleDac.GetListByOption(Profile.UserNo, 0, "R", gl, 1, 4);
            if (recommendList.Count == 0) { recommendList = _articleDac.GetListByOption(Profile.UserNo, 0, "P", gl, 1, 4); };
            ViewBag.RecommendList = recommendList;
            ViewBag.PopularList = _articleDac.GetListByOption(Profile.UserNo, 0, "P", gl, 1, 4);
            ViewBag.NewList = _articleDac.GetListByOption(Profile.UserNo, 0, "N", gl, 1, 4);

            ViewBag.ComList = _articleDac.GetListByOption(Profile.UserNo, 10203, "", gl, 1, 4);

            return View();
        }

        /// <summary>
        /// 메인 배너 가져오기
        /// </summary>
        /// <returns></returns>
        //[OutputCache(Duration = 60 * 60, VaryByParam = "none")]
        public PartialViewResult GetMainBanner()
        {
            IList<BannerT> banner = new BannerDac().GetBannerInFront();
            return PartialView(banner);
        }

        /// <summary>
        /// 프리뷰어
        /// </summary>
        /// <returns></returns>
        public ActionResult Freeviewer()
        {
            //string save3DFolder = "Article/article_3d";

            //string file3Dpath = string.Format("{0}/FileUpload/{1}/", AppDomain.CurrentDomain.BaseDirectory, save3DFolder);

            ////StlModel stlModel = new STLHelper().GetStlModel(file3Dpath + "1bfaedca-ce7c-4f2c-be6a-6f47789321c5.stl", "stl");

            //STLDocument facets = STLDocument.Open(file3Dpath + "a8b84e2f-4740-4171-847b-d08235bf6fd7.stl");

            //OBJDocument objDoc = new OBJDocument().LoadObj(file3Dpath + "uploads-ab-35-38-38-92-BoulderHiveFinal_ExportOBJ.obj");
            ////Size = new Extent
            ////{
            ////    //XMax = facets.Facets[0].Vertices.Max(v => v.X),
            ////    //XMin = facets.Facets[0].Vertices.Min(v => v.X),
            ////    //YMax = facets.Facets[0].Vertices.Max(v => v.Y),
            ////    //YMin = facets.Facets[0].Vertices.Min(v => v.Y),
            ////    //ZMax = facets.Facets[0].Vertices.Max(v => v.Z),
            ////    //ZMin = facets.Facets[0].Vertices.Min(v => v.Z)
            ////    XMax = facets.Facets.Max(f => f.Vertices.Max(v => v.X)),
            ////    XMin = facets.Facets.Min(f => f.Vertices.Min(v => v.X)),
            ////    YMax = facets.Facets.Max(f => f.Vertices.Max(v => v.Y)),
            ////    YMin = facets.Facets.Min(f => f.Vertices.Min(v => v.Y)),
            ////    ZMax = facets.Facets.Max(f => f.Vertices.Max(v => v.Z)),
            ////    ZMin = facets.Facets.Min(f => f.Vertices.Min(v => v.Z)),
            ////};

            //Size = new Extent
            //{
            //    XMax = objDoc.VertexList.Max(v => v.X),
            //    XMin = objDoc.VertexList.Min(v => v.X),
            //    YMax = objDoc.VertexList.Max(v => v.Y),
            //    YMin = objDoc.VertexList.Min(v => v.Y),
            //    ZMax = objDoc.VertexList.Max(v => v.Z),
            //    ZMin = objDoc.VertexList.Min(v => v.Z)
            //};

            //double volume = 0;
            //double stlVolume = 0;
            //double x1 = 0;
            //double y1 = 0;
            //double z1 = 0;
            //double x2 = 0;
            //double y2 = 0;
            //double z2 = 0;
            //double x3 = 0;
            //double y3 = 0;
            //double z3 = 0;


            ////stl
            //for (int i = 0; i < facets.Facets.Count; i++)
            //{
            //    x1 = facets.Facets[i].Vertices[0].X;
            //    y1 = facets.Facets[i].Vertices[0].Y;
            //    z1 = facets.Facets[i].Vertices[0].Z;

            //    x2 = facets.Facets[i].Vertices[1].X;
            //    y2 = facets.Facets[i].Vertices[1].Y;
            //    z2 = facets.Facets[i].Vertices[1].Z;

            //    x3 = facets.Facets[i].Vertices[2].X;
            //    y3 = facets.Facets[i].Vertices[2].Y;
            //    z3 = facets.Facets[i].Vertices[2].Z;

            //    stlVolume +=
            //        (-x3 * y2 * z1 +
            //        x2 * y3 * z1 +
            //        x3 * y1 * z2 -
            //        x1 * y3 * z2 -
            //        x2 * y1 * z3 +
            //        x1 * y2 * z3) / 6;
            //}

            ////object
            //for (int i = 0; i < objDoc.VertexList.Count; i++)
            //{
            //    x1 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[0]].X;
            //    y1 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[0]].Y;
            //    z1 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[0]].Z;

            //    x2 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[1]].X;
            //    y2 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[1]].Y;
            //    z2 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[1]].Z;

            //    x3 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[2]].X;
            //    y3 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[2]].Y;
            //    z3 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[2]].Z;


            //    volume +=
            //        (-x3 * y2 * z1 +
            //        x2 * y3 * z1 +
            //        x3 * y1 * z2 -
            //        x1 * y3 * z2 -
            //        x2 * y1 * z3 +
            //        x1 * y2 * z3) / 6;
            //}

            //double x5 = 0;
            //double y5 = 0;
            //double volume2 = 0;

            //for (int i = 0; i < objDoc.FaceList.Count; i++)
            //{
            //    x5 = objDoc.TextureList[objDoc.FaceList[i].TextureVertexIndexList[0]].X;
            //    y5 = objDoc.TextureList[objDoc.FaceList[i].TextureVertexIndexList[0]].Y;

            //    volume2 += x5 * y5;
            //}

            //ViewBag.Size = Size.XSize + "x" + Size.YSize + "x" + Size.ZSize;
            //ViewBag.Volume = volume + "???????????????" + volume2;
            return View();
        }

        public JsonResult testVolume(string fileName)
        {
            string save3DFolder = "Article/article_3d";

            string file3Dpath = string.Format("{0}/FileUpload/{1}/", AppDomain.CurrentDomain.BaseDirectory, save3DFolder);

            OBJDocument objDoc = new OBJDocument().LoadObj(file3Dpath + fileName);

            Size = new Extent
            {
                XMax = objDoc.VertexList.Max(v => v.X),
                XMin = objDoc.VertexList.Min(v => v.X),
                YMax = objDoc.VertexList.Max(v => v.Y),
                YMin = objDoc.VertexList.Min(v => v.Y),
                ZMax = objDoc.VertexList.Max(v => v.Z),
                ZMin = objDoc.VertexList.Min(v => v.Z)
            };

            double volume = 0;
            double x1 = 0;
            double y1 = 0;
            double z1 = 0;
            double x2 = 0;
            double y2 = 0;
            double z2 = 0;
            double x3 = 0;
            double y3 = 0;
            double z3 = 0;


            //object
            for (int i = 0; i < objDoc.VertexList.Count; i++)
            {

                x1 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[0]].X;
                y1 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[0]].Y;
                z1 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[0]].Z;

                x2 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[1]].X;
                y2 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[1]].Y;
                z2 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[1]].Z;

                x3 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[2]].X;
                y3 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[2]].Y;
                z3 = objDoc.VertexList[objDoc.FaceList[i].VertexIndexList[2]].Z;

                volume +=
                    (-x3 * y2 * z1 +
                    x2 * y3 * z1 +
                    x3 * y1 * z2 -
                    x1 * y3 * z2 -
                    x2 * y1 * z3 +
                    x1 * y2 * z3) / 6;
            }

            return Json(new { volume = volume, x = Size.XSize, y = Size.YSize, z = Size.ZSize });
        }

        public ActionResult Error()
        {
            return View();
        }

        #region 임시 삭제해야됨
        //public ActionResult ChgPw()
        //{
        //    IList<ChgPwT> list = memberDac.GetMemberPw();
        //    //CryptFilter crypt = new CryptFilter();
        //    //foreach (ChgPwT item in list)
        //    //{
        //    //    if (item.Pw1 != null)
        //    //    {
        //    //        item.Pw2 = crypt.Encrypt(item.Pw1);
        //    //    }
        //    //}
        //    //memberDac.InsertMemberPw(list);

        //    memberDac.ChgPwOnce(list);

        //    return View();
        //}
        #endregion

        public ActionResult betaPop()
        {
            return View();
        }


        //public List<Vertex> VertexList;

        private class Extent
        {
            public double XMax { get; set; }
            public double XMin { get; set; }
            public double YMax { get; set; }
            public double YMin { get; set; }
            public double ZMax { get; set; }
            public double ZMin { get; set; }

            public double XSize { get { return XMax - XMin; } }
            public double YSize { get { return YMax - YMin; } }
            public double ZSize { get { return ZMax - ZMin; } }
        }

        public bool updPrintVolume()
        {
            ArticleFileDac _articleFileDac = new ArticleFileDac();
            IList<ArticleFileT> before = _articleFileDac.TestGetArticleFileList();
            IList<ArticleFileT> after = new List<ArticleFileT>();
            STLHelper helper = new STLHelper();

            string file3Dpath = string.Format("{0}/FileUpload/{1}/", AppDomain.CurrentDomain.BaseDirectory, "Article/article_3d");

            foreach (ArticleFileT file in before)
            {
                if (file.X == 0)
                {
                    StlSize size = helper.GetSizeFor3DFile(file3Dpath + file.Rename, file.Ext);

                    file.X = size.X;
                    file.Y = size.Y;
                    file.Z = size.Z;
                    file.Volume = size.Volume;
                }
                file.PrintVolume = new STLHelper().slicing(file3Dpath + file.Rename);
                after.Add(file);
            }

            bool result = _articleFileDac.TestUpdatePrintVolume(after);

            return result;
        }

        public ActionResult test()
        {

//            //file
//            //fileName
//            //hasRightsToModel
//            //acceptTermsAndConditions

//            //string filePath = @"C:\Users\Administrator\Downloads\";
//            string filePath = @"C:\";


//            //FileStream stream = new FileStream(filePath + "1.stl", FileMode.Open, FileAccess.Read);

//            //var file1 = file_get_contents(filePath + "1.stl");
//            StlModel stlModel = new STLHelper().GetStlModel(filePath + "rabbit.stl", "stl");
//            var json = JsonConvert.SerializeObject(stlModel);

//            //string file1 = new StreamReader(filePath+"rabbit.stl").ReadToEnd();


//            var postData = "file=" + json;

//            postData+= "&fileName=1.stl&hasRightsToModel=1&acceptTermsAndConditions=1";

//            var data = Encoding.UTF8.GetBytes(postData);

//            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://dev.makersn.com/main/test2");
//            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"
//                https://api.shapeways.com/models/v1?oauth_consumer_key=4712cfb5a80a8b5d42e73810de77bd4b279fddf0&oauth_nonce=2mb0828hxmx6v786&oauth_signature=BAOZT%2FtMGtVY4D5UfXMGh0gJqnI%3D&oauth_signature_method=HMAC-SHA1&oauth_timestamp=1449105293&oauth_token=af730d5686fa01dbf888c5c1a6aa86a29f7156ff&oauth_version=1.0a
//            ");
        
//            //4105613


//            request.Method = "POST";
//            request.ContentLength = postData.Length;
//            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

//            using (var stream = request.GetRequestStream())
//            {
//                stream.Write(data, 0, data.Length);
//            }

//            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
//            string content = new StreamReader(response.GetResponseStream()).ReadToEnd();

//            ViewBag.Test = content;


            return View();
        }

        [HttpPost]
        public string test2(string file)
        {
            return file;
        }

    }
}
