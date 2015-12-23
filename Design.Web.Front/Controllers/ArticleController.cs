using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using Makersn.BizDac;
using Makersn.Models;
using Makersn.Util;
using Design.Web.Front.Helper;
using Design.Web.Front.Filter;
using Design.Web.Front.Models;
using System.Text.RegularExpressions;
using System.IO.Compression;
using Ionic.Zip;
using QuantumConcepts.Formats.StereoLithography;
using Library.ObjParser;


namespace Design.Web.Front.Controllers
{
    [RoutePrefix("article")]
    public class ArticleController : BaseController
    {
        ArticleDac _articleDac = new ArticleDac();
        ArticleCommentDac _articleCommentDac = new ArticleCommentDac();
        LikesDac _likesDac = new LikesDac();
        ReportDac _reportDac = new ReportDac();
        ArticleFileDac _articleFileDac = new ArticleFileDac();
        DownloadDac _downloadDac = new DownloadDac();
        ListDac _listDac = new ListDac();
        NoticesDac _noticesDac = new NoticesDac();
        TranslationDac _translationDac = new TranslationDac();
        TranslationDetailDac _translationDetailDac = new TranslationDetailDac();
        MemberDac _memberDac = new MemberDac();


        static HttpCookie searchCookie = new HttpCookie("searchCntBlock");

        private Extent Size { get; set; }


        #region 리스팅 페이지
        /// <summary>
        /// 기본 리스팅 페이지(신규,인기,추천,카테고리별)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="orderby"></param>
        /// <param name="codeNo"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public ActionResult Index(int page = 1, int codeNo = 0, string pageGubun = "", string gl = "", string cateNm = "")
        {
            if (cateNm != "")
            {
                codeNo = (int)(Makersn.Util.MakersnEnumTypes.CateName)Enum.Parse(typeof(Makersn.Util.MakersnEnumTypes.CateNameToUrl), cateNm);
            }

            int pageSize = 40;
            int codeNum = codeNo;
            IList<ArticleDetailT> list = null;
            ViewBag.PageTitle = EnumHelper.GetEnumTitle((MakersnEnumTypes.CateName)codeNo); //EnumTitle 가져오기 string반환
            ViewBag.CodeNo = codeNo;
            ViewBag.Gubun = pageGubun;

            switch (pageGubun)
            {
                case "N":
                    ViewBag.PageTitle = "신규";
                    break;

                case "P":
                    ViewBag.PageTitle = "인기";
                    break;

                case "R":
                    ViewBag.PageTitle = "추천";
                    break;
                default:
                    break;
            }


            int fromIndex = ((page - 1) * pageSize) + 1;
            int toIndex = page * pageSize;

            //PagerInfo pager = new PagerInfo();
            //pager.CurrentPageIndex = page;
            //pager.PageSize = pageSize;
            //pager.RecordCount = totalCnt;

            if (Request.Cookies.AllKeys.Contains("GlobalFlag"))
            {
                gl = Request.Cookies["GlobalFlag"].Value;
            }

            ViewBag.LangFlag = gl;

            switch (gl)
            {
                case "EN": ViewBag.LangFlagName = "English"; break;
                case "KR": ViewBag.LangFlagName = "한국어"; break;
                default: ViewBag.LangFlagName = "모든언어"; break;
            }
            gl = gl == "ALL" ? "" : gl;

            int totalCnt = _articleDac.GetTotalCountByOption(Profile.UserNo, codeNum, "", pageGubun, gl);

            list = _articleDac.GetListByOption(Profile.UserNo, codeNum, pageGubun, gl, fromIndex, toIndex);
            if (pageGubun == "R")
            {
                if (list.Count() == 0)
                {
                    list = _articleDac.GetListByOption(Profile.UserNo, codeNum, "N", gl, fromIndex, toIndex);
                }
            }

            PagerInfo pager = new PagerInfo();
            pager.CurrentPageIndex = page;
            pager.PageSize = pageSize;
            pager.RecordCount = totalCnt;
            PagerQuery<PagerInfo, IList<ArticleDetailT>> model = new PagerQuery<PagerInfo, IList<ArticleDetailT>>(pager, list);

            return View(model);
        }
        #endregion

        #region 게시물 상세보기 페이지
        /// <summary>
        /// 게시물 상세보기 페이지
        /// </summary>
        /// <param name="codeNo"></param>
        /// <param name="no"></param>
        /// <returns></returns>
        //[Route("detail/{no:string}")]
        public ActionResult Detail(string no = "", string goReply = "N")
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

            int articleNo = 0;
            var visitorNo = Profile.UserNo;
            ViewBag.GoReply = goReply;
            ArticleDetailT detail = new ArticleDetailT();
            if (Int32.TryParse(no, out articleNo))
            {
                //조회수 증가 방지
                if (Request.Cookies[no] == null)
                {
                    Response.Cookies[no].Value = no;
                    Response.Cookies[no].Expires = DateTime.Now.AddDays(1);

                    _articleDac.UpdateViewCnt(articleNo);
                }

                detail = _articleDac.GetArticleDetailByArticleNo(articleNo, visitorNo);

                TranslationDetailT trans = _translationDetailDac.GetTranslationDetailByArticleNoAndLangFlag(articleNo, langFlag);
                if (trans != null)
                {
                    detail.Title = trans.Title;
                    detail.Contents = trans.Contents;
                    detail.Tag = trans.Tag;
                }

                ViewBag.MetaDescription = detail.Contents;

                detail.Contents = new ContentFilter().HtmlEncode(detail.Contents);
                //detail.Contents = detail.Contents.Replace("#", "");
                detail.Contents = chkContent(detail.Contents);
                if ((detail.MemberNo != visitorNo && Profile.UserLevel < 50) && detail.Visibility.ToUpper() == "N")
                {
                    return Content("<script>alert('비공개 처리된 게시물 입니다.'); location.href='/';</script>");
                }

                ViewBag.chkStlCnt = _articleFileDac.GetSTLFileList(articleNo).Count;
                ViewBag.Files = _articleFileDac.GetFileList(articleNo);
                ViewBag.MainImg = detail.MainImgName;
                ViewBag.ListCnt = _listDac.GetListARticleCntByArticleNo(articleNo);
                ViewBag.ListList = _listDac.GetListNames(visitorNo); //리스트 이름 목록
            }
            else
            {

            }

            ViewBag.VisitorNo = visitorNo;

            ViewBag.No = no;
            ViewBag.CodeNo = detail.CodeNo;

            ViewBag.Class = "bdB mgB15";
            ViewBag.WrapClass = "bgW";

            return View(detail);
        }
        #endregion

        #region 검색 페이지
        /// <summary>
        /// 검색 페이지
        /// </summary>
        /// <param name="page"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public ActionResult Search(int page = 1, string text = "", string tag = "", string gl = "")
        {
            text = Server.UrlDecode(text);
            ViewBag.Text = text;
            if (tag != "") { ViewBag.IsTag = "태그"; };

            if (Request.Cookies.AllKeys.Contains("GlobalFlag"))
            {
                gl = Request.Cookies["GlobalFlag"].Value;
            }
            gl = gl == "ALL" ? "" : gl;

            ViewBag.LangFlag = gl;


            IList<ArticleT> list = _articleDac.GetSearchList(text, Profile.UserNo, tag, gl);

            //조회수 증가 방지
            //if (searchCookie[Profile.UserNo + "_" + text] == null)
            if (Request.Cookies[Profile.UserNo + "_" + text] == null)
            {
                PopularT popular = new PopularT();

                string chgDt = DateTime.Now.ToString("yyyy-MM-dd");
                popular.RegDt = Convert.ToDateTime(chgDt);
                popular.Word = text;
                popular.Cnt = 1;
                popular.RegId = Profile.UserId;
                popular.MemberNo = Profile.UserNo;

                IPHostEntry host;
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily.ToString() == "InterNetwork")
                    {
                        popular.RegIp = ip.ToString();
                    }
                }
                PopularDac popularDac = new PopularDac();
                popularDac.AddSearchText(popular);

                //searchCookie[Profile.UserNo + "_" + text] = "1";
                //Response.Cookies[Profile.UserNo + "_" + text].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies[Profile.UserNo + "_" + text].Value = Profile.UserNo + "_" + text;
                Response.Cookies[Profile.UserNo + "_" + text].Expires = DateTime.Now.AddDays(-1);

                //Response.Cookies["searchCntBlock"].Expires = DateTime.Now.AddDays(-1);
                //Response.Cookies["searchCntBlock"].Expires = DateTime.Now.AddMinutes(10);
            }

            return View(list.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
        }
        #endregion

        #region 게시글 업로드
        /// <summary>
        /// article upload
        /// </summary>
        /// <returns></returns>
        [Authorize, HttpGet]
        public ActionResult Upload()
        {
            ViewBag.Temp = Profile.UserNo + "_" + new DateTimeHelper().ConvertToUnixTime(DateTime.Now);

            //int articleNo = 0;
            //ArticleT articleT = new ArticleT();
            //if (no != "")
            //{
            //    articleT = articleDac.GetArticleByNo(articleNo);
            //}

            //if (!Int32.TryParse(no, out articleNo))
            //{
            //    //history
            //    return RedirectToAction("Upload");
            //}

            return View();
        }

        /// <summary>
        /// article upload
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Upload(FormCollection collection)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;

            int articleNo = 0;

            string paramNo = collection["No"];
            string temp = collection["temp"];
            string mode = collection["mode"];
            int mainImg = Convert.ToInt32(collection["main_img"]);
            string articleTitle = collection["article_title"];
            string articleContents = collection["article_contents"];
            int codeNo = Convert.ToInt32(collection["lv1"]);
            int copyright = Convert.ToInt32(collection["copyright"]);
            string delNo = collection["del_no"];
            string VideoSource = collection["insertVideoSource"];
            //string[] splitArray = articleContents.Split('#');
            //string tags = "";
            string tags = collection["article_tags"];

            var mulltiSeq = collection["multi[]"];
            string[] seqArray = mulltiSeq.Split(',');


            //for (int i = 1; i < splitArray.Length; i++)
            //{
            //    try
            //    {
            //        splitArray[i] = splitArray[i].Replace("\r\n", " ");
            //        splitArray[i] = splitArray[i].Replace("\n", " ");
            //        splitArray[i] = splitArray[i].Replace("\r", " ");
            //        tags += splitArray[i].Substring(0, splitArray[i].IndexOf(' '));
            //    }
            //    catch
            //    {
            //        tags += splitArray[i];
            //    }
            //    if (i < splitArray.Length - 1) { tags += ","; };
            //}
            //articleContents = articleContents.Replace("#", "");
            if (tags.Length > 1000)
            {
                response.Message = "태그는 1000자 이하로 가능합니다.";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            ArticleT articleT = null;
            //TranslationT tran = null;
            TranslationDetailT tranDetail = null;
            if (!Int32.TryParse(paramNo, out articleNo))
            {
                response.Success = false;
                response.Message = "pk error";
            }

            if (articleNo > 0)
            {
                //update
                articleT = _articleDac.GetArticleByNo(articleNo);
                tranDetail = _translationDetailDac.GetTranslationDetailByArticleNoAndLangFlag(articleT.No, "KR");
                if (tranDetail != null)
                {
                    articleT.Title = tranDetail.Title;
                    articleT.Contents = tranDetail.Contents;
                    articleT.Tag = tranDetail.Tag;
                }

                if (articleT != null)
                {
                    if (articleT.MemberNo == Profile.UserNo && articleT.Temp == temp)
                    {
                        articleT.UpdDt = DateTime.Now;
                        articleT.UpdId = Profile.UserId;
                        articleT.RegIp = IPAddressHelper.GetIP(this);
                    }
                }
            }
            else
            {
                //save
                articleT = new ArticleT();
                articleT.MemberNo = Profile.UserNo;
                //태그 #**
                articleT.Tag = tags;
                articleT.Temp = temp;
                articleT.ViewCnt = 0;
                articleT.RegDt = DateTime.Now;
                articleT.RegId = Profile.UserId;
                articleT.RegIp = IPAddressHelper.GetIP(this);
                articleT.RecommendYn = "N";
                articleT.RecommendDt = null;
            }

            if (tranDetail == null)
            {

                //영문텍스트 TRANSLATION_DETAIL
                tranDetail = new TranslationDetailT();
                tranDetail.LangFlag = "KR";
                tranDetail.RegId = Profile.UserId;
                tranDetail.RegDt = DateTime.Now;
            }

            //return Json(response);

            if (articleT != null)
            {
                articleT.Title = articleTitle;
                articleT.Contents = articleContents;
                articleT.Visibility = mode == "upload" ? "Y" : "N";
                articleT.Copyright = copyright;
                articleT.CodeNo = codeNo;
                articleT.MainImage = mainImg;

                articleT.VideoUrl = VideoSource;

                articleT.Tag = tags;
                articleNo = _articleDac.SaveOrUpdate(articleT, delNo);


                //영문텍스트 TRANSLATION_DETAIL
                tranDetail.ArticleNo = articleNo;
                tranDetail.Title = articleT.Title;
                tranDetail.Contents = articleT.Contents;
                tranDetail.Tag = articleT.Tag;


                _translationDetailDac.SaveOrUpdateTranslationDetail(tranDetail);

                response.Success = true;
                response.Result = articleNo.ToString();
            }

            _articleFileDac.UpdateArticleFileSeq(seqArray);


            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region article edit view
        /// <summary>
        /// article edit view
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        [Authorize, HttpGet]
        public ActionResult Edit(string no = "")
        {

            int articleNo = 0;
            if (no == "")
            {
                return RedirectToAction("Upload");
            }

            if (!Int32.TryParse(no, out articleNo))
            {
                //history
                return RedirectToAction("Upload");
            }

            ArticleT articleT = _articleDac.GetArticleForEdit(articleNo);
            TranslationDetailT transDetail = _translationDetailDac.GetTranslationDetailByArticleNoAndLangFlag(articleT.No, "KR");
            if (transDetail != null)
            {
                articleT.Title = transDetail.Title;
                articleT.Tag = transDetail.Tag;
                articleT.Contents = transDetail.Contents;
            }
            if (articleT.MemberNo != Profile.UserNo && Profile.UserLevel < 50) { return RedirectToAction("detail", new { no = articleT.No }); }

            int UploadCnt = _articleFileDac.GetArticleFileCntByNo(articleT.No);

            int totalPages = Math.Max((UploadCnt + 5 - 1) / 5, 1);
            ViewBag.UploadCnt = totalPages == 1 ? totalPages * 5 * 2 : totalPages * 5;
            ViewBag.UploadFileCnt = UploadCnt;

            //페이지 들어올때 temp와 같은 임시파일이 존재하는지 체크
            _articleDac.CheckTempFile(articleT.Temp);

            return View(articleT);
        }
        #endregion

        #region stl upload
        /// <summary>
        /// stl upload
        /// </summary>
        /// <param name="stlupload"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StlUpload(FormCollection collection, IEnumerable<HttpPostedFileBase> stluploads)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;
            string fileName = string.Empty;

            //foreach (var stl in stlupload)
            //{
            //    string testNm = stl.FileName;
            //}
            //return null;

            #region 신규 반환값

            int uploadCnt = stluploads.Count();
            bool Success = false;
            string Message = string.Empty;
            int[] Result = new int[uploadCnt];
            int index = 0;

            #endregion

            //HttpPostedFileBase stlupload = Request.Files["stlupload"];
            string temp = collection["temp"];

            foreach (HttpPostedFileBase stlupload in stluploads)
            {
                if (stlupload != null)
                {
                    if (stlupload.ContentLength > 0)
                    {
                        if (stlupload.ContentLength < 200 * 1024 * 1024)
                        {
                             string[] extType = { "stl", "obj" };

                            string extension = Path.GetExtension(stlupload.FileName).ToLower().Replace(".", "").ToLower();

                            if (!extType.Contains(extension))
                            {
                                Message = "stl, obj 형식 파일만 가능합니다.";
                                return Json(new { Success = Success, Message = Message }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        //response.Message = "최대 사이즈 100MB 파일만 가능합니다.";
                        Message = "최대 사이즈 100MB 파일만 가능합니다.";
                    }
                }
            }

            foreach (HttpPostedFileBase stlupload in stluploads)
            {
                if (stlupload != null)
                {
                    if (stlupload.ContentLength > 0)
                    {
                        if (stlupload.ContentLength < 200 * 1024 * 1024)
                        {
                            string[] extType = { "stl", "obj" };

                            string extension = Path.GetExtension(stlupload.FileName).ToLower().Replace(".", "").ToLower();

                            if (extType.Contains(extension))
                            {
                                string save3DFolder = "Article/article_3d";
                                string saveJSFolder = "Article/article_js";
                                fileName = FileUpload.UploadFile(stlupload, null, save3DFolder, null);

                                string file3Dpath = string.Format("{0}/FileUpload/{1}/", AppDomain.CurrentDomain.BaseDirectory, save3DFolder);
                                string fileJSpath = string.Format("{0}/FileUpload/{1}/", AppDomain.CurrentDomain.BaseDirectory, saveJSFolder);

                                StlModel stlModel = new STLHelper().GetStlModel(file3Dpath + fileName, extension);

                                ArticleFileT sizeResult = GetSizeFor3DFile(file3Dpath + fileName, extension);

                                var json = JsonConvert.SerializeObject(stlModel);

                                if (!Directory.Exists(fileJSpath))
                                {
                                    Directory.CreateDirectory(fileJSpath);
                                }

                                string jsFileNm = fileJSpath + fileName + ".js";

                                System.IO.File.WriteAllText(jsFileNm, json, Encoding.UTF8);

                                ArticleFileT articleFileT = new ArticleFileT();

                                articleFileT.FileGubun = "temp";
                                articleFileT.FileType = "stl";
                                articleFileT.MemberNo = Profile.UserNo;
                                articleFileT.Seq = 5000;
                                articleFileT.ImgUseYn = "N";
                                articleFileT.Ext = extension;
                                articleFileT.ThumbYn = "N";
                                articleFileT.MimeType = stlupload.ContentType;
                                articleFileT.Name = stlupload.FileName;
                                articleFileT.Size = stlupload.ContentLength.ToString();
                                articleFileT.Rename = fileName;
                                articleFileT.Path = string.Format("/{0}/", save3DFolder);
                                //articleFileT.Width = "630";
                                //articleFileT.Height = "470";
                                articleFileT.X = sizeResult.X;
                                articleFileT.Y = sizeResult.Y;
                                articleFileT.Z = sizeResult.Z;
                                articleFileT.Volume = sizeResult.Volume;
                                //articleFileT.PrintVolume = new STLHelper().slicing(file3Dpath + fileName);

                                articleFileT.UseYn = "Y";
                                articleFileT.Temp = temp;
                                articleFileT.RegIp = IPAddressHelper.GetIP(this);
                                articleFileT.RegId = Profile.UserId;
                                articleFileT.RegDt = DateTime.Now;

                                int articleFileNo = _articleFileDac.InsertArticleFile(articleFileT);

                                //response.Success = true;
                                //response.Result = articleFileNo.ToString();
                                Success = true;
                                Result[index] = articleFileNo;
                                index++;
                            }
                            else
                            {
                                //response.Message = "stl, obj 형식 파일만 가능합니다.";
                                Message = "stl, obj 형식 파일만 가능합니다.";
                            }
                        }
                        else
                        {
                            //response.Message = "최대 사이즈 100MB 파일만 가능합니다.";
                            Message = "최대 사이즈 100MB 파일만 가능합니다.";
                        }
                    }
                }
            }

            var mulltiSeq = collection["multi[]"];

            string[] seqArray = null;
            if (mulltiSeq != null)
            {
                seqArray = mulltiSeq.Split(',');
                _articleFileDac.UpdateArticleFileSeq(seqArray);
            }


            return Json(new { Success = Success, Message = Message, Result = Result, Count = uploadCnt }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 3D파일 사이즈 구하기
        public ArticleFileT GetSizeFor3DFile(string path, string ext)
        {
            ArticleFileT getSize = new ArticleFileT();

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


            switch (ext)
            {
                case "stl":
                    STLDocument facets = STLDocument.Open(path);
                    //stl
                    Size = new Extent
                    {
                        XMax = facets.Facets.Max(f => f.Vertices.Max(v => v.X)),
                        XMin = facets.Facets.Min(f => f.Vertices.Min(v => v.X)),
                        YMax = facets.Facets.Max(f => f.Vertices.Max(v => v.Y)),
                        YMin = facets.Facets.Min(f => f.Vertices.Min(v => v.Y)),
                        ZMax = facets.Facets.Max(f => f.Vertices.Max(v => v.Z)),
                        ZMin = facets.Facets.Min(f => f.Vertices.Min(v => v.Z)),
                    };

                    for (int i = 0; i < facets.Facets.Count; i++)
                    {
                        x1 = facets.Facets[i].Vertices[0].X;
                        y1 = facets.Facets[i].Vertices[0].Y;
                        z1 = facets.Facets[i].Vertices[0].Z;

                        x2 = facets.Facets[i].Vertices[1].X;
                        y2 = facets.Facets[i].Vertices[1].Y;
                        z2 = facets.Facets[i].Vertices[1].Z;

                        x3 = facets.Facets[i].Vertices[2].X;
                        y3 = facets.Facets[i].Vertices[2].Y;
                        z3 = facets.Facets[i].Vertices[2].Z;

                        volume +=
                            (-x3 * y2 * z1 +
                            x2 * y3 * z1 +
                            x3 * y1 * z2 -
                            x1 * y3 * z2 -
                            x2 * y1 * z3 +
                            x1 * y2 * z3) / 6;
                    }
                    break;

                case "obj":
                    OBJDocument objDoc = new OBJDocument().LoadObj(path);
                    int[] idx = new int[3];
                    List<Library.ObjParser.Vertex> test = objDoc.VertexList.Where(s => (int)s.Y != 0).OrderByDescending(s => s.Y).ToList();

                    Size = new Extent
                    {
                        XMax = objDoc.VertexList.Where(w => (int)w.X != 0).Max(v => v.X),
                        XMin = objDoc.VertexList.Where(w => (int)w.X != 0).Min(v => v.X),
                        //YMax = objDoc.VertexList.Max(v => v.Y) <= 0 ? objDoc.VertexList.Where(s => (int)s.Y != 0).OrderByDescending(s => s.Y).Max(v => v.Y) : objDoc.VertexList.Max(v => v.Y),
                        YMax = objDoc.VertexList.Where(w => (int)w.Y != 0).Max(v => v.Y),
                        YMin = objDoc.VertexList.Where(w => (int)w.Y != 0).Min(v => v.Y),
                        ZMax = objDoc.VertexList.Where(w => (int)w.Z != 0).Max(v => v.Z),
                        ZMin = objDoc.VertexList.Where(w => (int)w.Z != 0).Min(v => v.Z)
                    };

                    int vertexCnt = objDoc.VertexList.Count();
                    for (int i = 0; i < objDoc.FaceList.Count; i++)
                    {
                        idx[0] = objDoc.FaceList[i].VertexIndexList[0] - 1;
                        idx[1] = objDoc.FaceList[i].VertexIndexList[1] - 1;
                        idx[2] = objDoc.FaceList[i].VertexIndexList[2] - 1;
                        if (idx[0] > 0 && idx[1] > 0 && idx[2] > 0)
                        {
                            if (idx[0] > vertexCnt && idx[1] > vertexCnt && idx[2] > vertexCnt)
                            {
                                //log 로 남겨보기
                            }
                            else
                            {
                                x1 = objDoc.VertexList[idx[0]].X;
                                y1 = objDoc.VertexList[idx[0]].Y;
                                z1 = objDoc.VertexList[idx[0]].Z;

                                x2 = objDoc.VertexList[idx[1]].X;
                                y2 = objDoc.VertexList[idx[1]].Y;
                                z2 = objDoc.VertexList[idx[1]].Z;

                                x3 = objDoc.VertexList[idx[2]].X;
                                y3 = objDoc.VertexList[idx[2]].Y;
                                z3 = objDoc.VertexList[idx[2]].Z;

                                volume +=
                                    (-x3 * y2 * z1 +
                                    x2 * y3 * z1 +
                                    x3 * y1 * z2 -
                                    x1 * y3 * z2 -
                                    x2 * y1 * z3 +
                                    x1 * y2 * z3) / 6;
                            }
                        }
                        else
                        {
                            //log 로 남겨보기

                        }
                    }
                    break;

            }
            volume = volume < 0 ? volume * -1 : volume;

            getSize.X = Math.Round(Size.XSize, 1);
            getSize.Y = Math.Round(Size.YSize, 1);
            getSize.Z = Math.Round(Size.ZSize, 1);
            getSize.Volume = Math.Round(volume, 1);

            return getSize;
        }
        #endregion

        #region img upload
        /// <summary>
        /// img upload
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImgUpload(FormCollection collection, IEnumerable<HttpPostedFileBase> imguploads)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;
            string fileName = string.Empty;

            #region 신규 반환값

            int uploadCnt = imguploads.Count();
            bool Success = false;
            string Message = string.Empty;
            int[] Result = new int[uploadCnt];
            int index = 0;

            #endregion

            //HttpPostedFileBase imgupload = Request.Files["imgupload"];
            string temp = collection["temp"];

            foreach (HttpPostedFileBase chkExt in imguploads)
            {
                if (chkExt != null)
                {
                    if (chkExt.ContentLength > 0)
                    {
                        string[] extType = { "jpg", "png", "gif" };

                        string extension = Path.GetExtension(chkExt.FileName).ToLower().Replace(".", "").ToLower();
                        if (!extType.Contains(extension))
                        {
                            //response.Message = "gif, jpg, png 형식 파일만 가능합니다.";
                            Message = "gif, jpg, png 형식 파일만 가능합니다.";
                            return Json(new { Success = Success, Message = Message}, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }

            foreach (HttpPostedFileBase imgupload in imguploads)
            {
                if (imgupload != null)
                {
                    if (imgupload.ContentLength > 0)
                    {
                        string[] extType = { "jpg", "png", "gif" };

                        string extension = Path.GetExtension(imgupload.FileName).ToLower().Replace(".", "").ToLower();

                        if (extType.Contains(extension))
                        {
                            fileName = FileUpload.UploadFile(imgupload, new ImageSize().GetArticleResize(), "Article", null);

                            ArticleFileT articleFileT = new ArticleFileT();

                            articleFileT.FileGubun = "temp";
                            articleFileT.FileType = "img";
                            articleFileT.MemberNo = Profile.UserNo;
                            articleFileT.Seq = 5000;
                            articleFileT.ImgUseYn = "U";
                            articleFileT.Ext = extension;
                            articleFileT.ThumbYn = "Y";
                            articleFileT.MimeType = imgupload.ContentType;
                            articleFileT.Name = imgupload.FileName;
                            articleFileT.Size = imgupload.ContentLength.ToString();
                            articleFileT.Rename = fileName;
                            articleFileT.Path = "/Article/article_img/";

                            articleFileT.Width = "630";
                            articleFileT.Height = "470";

                            articleFileT.UseYn = "Y";
                            articleFileT.Temp = temp;
                            articleFileT.RegIp = IPAddressHelper.GetIP(this);
                            articleFileT.RegId = Profile.UserId;
                            articleFileT.RegDt = DateTime.Now;

                            int articleFileNo = _articleFileDac.InsertArticleFile(articleFileT);

                            //response.Success = true;
                            //response.Result = articleFileNo.ToString();
                            Success = true;
                            Result[index] = articleFileNo;
                            index++;
                        }
                        else
                        {
                            //response.Message = "gif, jpg, png 형식 파일만 가능합니다.";
                            Message = "gif, jpg, png 형식 파일만 가능합니다.";
                        }
                    }
                }
            }


            var mulltiSeq = collection["multi[]"];
            string[] seqArray = null;
            if (mulltiSeq != null)
            {
                seqArray = mulltiSeq.Split(',');
                _articleFileDac.UpdateArticleFileSeq(seqArray);

            }

            return Json(new { Success = Success, Message = Message, Result = Result, Count = uploadCnt }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 이미지 캡쳐
        /// <summary>
        /// capture
        /// </summary>
        /// <param name="stl_val"></param>
        /// <param name="stl_img_no"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImgCapture(string stl_val, int stl_img_no)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;

            string saveImgFolder = "Article/article_img";
            string fileImgpath = string.Format("{0}/FileUpload/{1}/", AppDomain.CurrentDomain.BaseDirectory, saveImgFolder);

            if (!Directory.Exists(fileImgpath))
            {
                Directory.CreateDirectory(fileImgpath);
            }

            if (!string.IsNullOrEmpty(stl_val) && stl_img_no != 0)
            {
                ArticleFileT file = _articleFileDac.GetArticleFileByNo(stl_img_no);

                string fileNm = file.Rename + ".jpg";

                using (FileStream fs = new FileStream(fileImgpath + fileNm, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        byte[] data = Convert.FromBase64String(stl_val.Replace("data:image/png;base64,", ""));
                        bw.Write(data);
                        bw.Close();
                    }
                    fs.Close();
                }

                file.ImgName = fileNm;
                response.Success = _articleFileDac.UpdateArticleFile(file);
                //response.Result = file.Rename;
                response.Result = file.No.ToString();
            }

            response.Success = true;

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 3d 모델 뷰어 - GitHub
        /// <summary>
        /// 3d 모델 뷰어
        /// </summary>
        /// <param name="fileNm"></param>
        /// <returns></returns>
        public ActionResult Solid(string fileNm)
        {
            ViewBag.FileNm = fileNm;
            return View();
        }
        #endregion

        #region 업로드된 파일 리스트
        /// <summary>
        /// 업로드된 파일 리스트
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [HttpPost]
        public PartialViewResult UploadFilesView(FormCollection collection)
        {
            string paramNo = collection["No"];
            string uploadCnt = collection["uploadCnt"];
            string temp = collection["temp"];
            string mode = collection["mode"];
            string delNo = collection["del_no"];

            ViewBag.UploadCnt = uploadCnt;

            IList<ArticleFileT> fileLst = new List<ArticleFileT>();

            if (mode == "edit")
            {
                fileLst = _articleFileDac.GetFileList(int.Parse(paramNo));
            }
            else
            {
                fileLst = _articleFileDac.GetArticleFilesByTemp(temp);
            }

            if (!string.IsNullOrEmpty(delNo))
            {
                string[] delNos = delNo.Split(',');

                IList<ArticleFileT> files = fileLst.Where(n => delNos.Contains(n.No.ToString())).ToList();

                fileLst = fileLst.Except(files).ToList();
            }

            return PartialView(fileLst);
        }
        #endregion

        #region
        public PartialViewResult AppendFile(int no, int idx)
        {
            ArticleFileT file = _articleFileDac.GetArticleFileByNo(no);
            ViewBag.Index = idx;
            return PartialView(file);
        }
        #endregion

        #region 게시글 삭제
        /// <summary>
        /// 게시글 삭제
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(FormCollection collection)
        {
            AjaxResponseModel responseModel = new AjaxResponseModel();

            string paramNo = collection["No"];

            if (!string.IsNullOrEmpty(paramNo))
            {
                _articleDac.DeleteArticle(Convert.ToInt32(paramNo));
            }

            return Json(responseModel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 댓글 파샬뷰
        /// <summary>
        /// 댓글 파샬뷰
        /// </summary>
        /// <param name="no"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PartialViewResult Reply(string no = "", int page = 1, string goReply = "N")
        {
            ViewBag.No = no;
            ViewBag.MemberNo = Profile.UserNo;
            ViewBag.GoReply = goReply;
            IList<ArticleCommentT> list = _articleCommentDac.GetArticleCommentListByNo(int.Parse(no));

            MemberT visitor = _memberDac.GetMemberProfile(Profile.UserNo);
            ViewBag.VisitorPic = visitor == null ? "" : visitor.ProfilePic;
            return PartialView(list.OrderByDescending(o => o.Regdt).ToPagedList(page, 10));
            //return PartialView(list);
        }
        #endregion

        #region 댓글 삭제
        /// <summary>
        /// 댓글 삭제
        /// </summary>
        /// <param name="commentNo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CommentDel(string commentNo = "")
        {
            commentNo = Base64Helper.Base64Decode(commentNo);
            _articleCommentDac.DeleteArticleCommentByNo(int.Parse(commentNo));
            return Json(new { Message = "삭제되었습니다.", Success = true });
        }
        #endregion

        #region 댓글 수정
        /// <summary>
        /// 댓글 수정
        /// </summary>
        /// <param name="commentNo"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CommentUpd(string commentNo = "", string content = "")
        {
            ArticleCommentT act = new ArticleCommentT();
            act.No = int.Parse(commentNo);
            act.Content = content;
            act.UpdDt = DateTime.Now;
            act.UpdId = Profile.UserId;

            //no = Base64Helper.Base64Decode(no);
            _articleCommentDac.UpdateArticleCommentByNo(act);
            return Json(new { Message = "수정되었습니다.", Success = true });
        }
        #endregion

        #region 댓글 추가
        /// <summary>
        /// 댓글 추가
        /// </summary>
        /// <param name="articleNo"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public JsonResult CommentAdd(int articleNo = 0, string content = "", string articleMemberNo = "0")
        {
            if (Profile.UserNo == 0) { return Json(new { Success = false }); };
            articleMemberNo = Base64Helper.Base64Decode(articleMemberNo);
            string getIp = IPAddressHelper.GetIP(this);
            ArticleCommentT act = new ArticleCommentT();
            act.ArticleNo = articleNo;
            act.Content = content;
            act.Regdt = DateTime.Now;
            act.RegId = Profile.UserId;
            act.MemberNoRef = int.Parse(articleMemberNo);
            act.MemberNo = Profile.UserNo;
            act.Writer = Profile.UserNm;
            act.RegIp = getIp;
            act.Depth = 0;

            long articleCommentNo = _articleCommentDac.InsertArticleComment(act);


            if (act.MemberNoRef != act.MemberNo)
            {
                NoticeT notice = new NoticeT();
                notice.IdxName = "c:" + articleNo;
                notice.ArticleNo = articleNo;
                notice.MemberNo = Profile.UserNo;
                notice.MemberNoRef = act.MemberNoRef;
                notice.RefNo = articleCommentNo;
                notice.Type = "comment";
                notice.IsNew = "Y";
                notice.RegDt = DateTime.Now;
                notice.RegId = Profile.UserId;
                notice.RegIp = IPAddressHelper.GetClientIP();

                _noticesDac.InsertNoticeByComment(notice);
            }

            return Json(new { Message = "등록되었습니다.", Success = true });
        }
        #endregion

        #region 댓글의 댓글
        public JsonResult AddInReply(int no, string content, int articleNo, int memberNoRef, int chkReply)
        {
            AjaxResponseModel model = new AjaxResponseModel();
            model.Success = false;
            if (Profile.UserNo == 0) { return Json(new { Success = false }); };
            try
            {
                ArticleT article = _articleDac.GetArticleByNo(articleNo);
                string getIp = IPAddressHelper.GetIP(this);
                ArticleCommentT act = new ArticleCommentT();
                act.ArticleNo = articleNo;

                int refCommentMemberNo = 0;

                if (chkReply == 0)
                {
                    act.Content = content;
                }
                else
                {
                    MemberT mem = _memberDac.GetMemberProfile(memberNoRef);
                    act.Content = mem.Name + "#user#" + content;

                    ArticleCommentT refCom = _articleCommentDac.GetRefCommentByRefNo(no);
                    refCommentMemberNo = refCom.MemberNo;
                }

                //act.Content = content;
                act.Regdt = DateTime.Now;
                act.RegId = Profile.UserId;
                act.MemberNoRef = memberNoRef; act.MemberNo = Profile.UserNo;
                act.Writer = Profile.UserNm;
                act.RegIp = getIp;
                act.RefNo = no;
                act.Depth = 1;

                long articleCommentNo = _articleCommentDac.InsertArticleCommentInReply(act);

                if (act.MemberNoRef != act.MemberNo)
                {
                    NoticeT notice = new NoticeT();
                    notice.IdxName = "c:" + articleNo;
                    notice.ArticleNo = articleNo;
                    notice.MemberNo = Profile.UserNo;
                    notice.MemberNoRef = act.MemberNoRef;
                    notice.RefNo = articleCommentNo;
                    notice.Type = "inComment";
                    notice.IsNew = "Y";
                    notice.RegDt = DateTime.Now;
                    notice.RegId = Profile.UserId;
                    notice.RegIp = IPAddressHelper.GetClientIP();

                    _noticesDac.InsertNoticeByComment(notice);
                }

                if (refCommentMemberNo != 0 && refCommentMemberNo != Profile.UserNo)
                {
                    NoticeT notice = new NoticeT();
                    notice.IdxName = "c:" + articleNo;
                    notice.ArticleNo = articleNo;
                    notice.MemberNo = Profile.UserNo;
                    notice.MemberNoRef = refCommentMemberNo;
                    notice.RefNo = articleCommentNo;
                    notice.Type = "inComment";
                    notice.IsNew = "Y";
                    notice.RegDt = DateTime.Now;
                    notice.RegId = Profile.UserId;
                    notice.RegIp = IPAddressHelper.GetClientIP();

                    _noticesDac.InsertNoticeByComment(notice);
                }

                model.Success = true;
            }
            catch
            {

            }

            return Json(model);
        }
        #endregion

        #region detail page 미리보기 파샬뷰 (현재 사용안함)
        public PartialViewResult ArticleInfo(string no = "")
        {
            IList<ArticleFileT> list = _articleFileDac.GetFileList(int.Parse(no));
            return PartialView(list);
        }
        #endregion

        #region detail page 게시자의 다른게시물, 추천게시물 파샬뷰
        /// <summary>
        /// 
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public PartialViewResult MemberOtherArticle(string no = "")
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

            IList<ArticleT> before = _articleDac.GetMemberArticleTop4(int.Parse(no));
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

            IList<ArticleDetailT> beforeRecommendList = new List<ArticleDetailT>();
            IList<ArticleDetailT> recommendList = new List<ArticleDetailT>();
            beforeRecommendList = _articleDac.GetListByOption(Profile.UserNo, 0, "R", "", 1, 4);
            if (beforeRecommendList.Count == 0) { beforeRecommendList = _articleDac.GetListByOption(Profile.UserNo, 0, "P", "", 1, 4); };

            foreach (ArticleDetailT recommend in beforeRecommendList)
            {
                TranslationDetailT trans = _translationDetailDac.GetTranslationDetailByArticleNoAndLangFlag(recommend.No, langFlag);
                if (trans != null)
                {
                    recommend.Title = trans.Title;
                }
                recommendList.Add(recommend);
            }

            ViewBag.RecommendList = beforeRecommendList;

            return PartialView(list);
        }
        #endregion

        #region detail right contents
        /// <summary>
        /// detail right contents
        /// </summary>
        /// <param name="articleNo"></param>
        /// <returns></returns>
        public PartialViewResult DetailRightContents(int authorNo)
        {
            ViewBag.CheckFollow = new FollowerDac().CheckFollow(authorNo, Profile.UserNo);
            //ViewBag.VisitorNo = Base64Helper.Base64Encode(Profile.UserNo.ToString());
            ViewBag.chkSelf = (Profile.UserNo == authorNo);
            ViewBag.AuthorNo = Base64Helper.Base64Encode(authorNo.ToString());
            return PartialView();
        }
        #endregion

        #region 신고
        public JsonResult SendReport(int articleNo, string reportComment)
        {
            if (Profile.UserNo == 0) { return Json(new { Success = false, Message = "로그인해주세요." }); }
            ReportT report = new ReportT();
            report.ArticleNo = articleNo;
            report.Report = reportComment;
            report.MemberNo = Profile.UserNo;
            report.RegDt = DateTime.Now;
            report.RegId = Profile.UserId;
            report.RegIp = IPAddressHelper.GetIP(this);
            report.State = 1;
            int result = _reportDac.InsertReport(report);

            string title = _articleDac.GetArticleByNo(articleNo).Title;
            sendEmailReport(Profile.UserId, title, reportComment);

            return Json(new { Success = true, Result = result, Message = "접수 되었습니다." });
        }
        #endregion

        #region 신고 등록시 이메일 발송
        public void sendEmailReport(string id, string title, string comment)
        {
            string Subject = id + "님으로 부터 신고가 접수되었습니다.";

            SendMailModels oMail = new SendMailModels();
            oMail.SendMail("sendReport", "info@makersi.com", new String[] { Subject, title, comment });
        }
        #endregion

        #region 좋아요 클릭
        public JsonResult SetLikes(int articleNo)
        {
            if (Profile.UserNo == 0) { return Json(new { Success = false, Message = "로그인해주세요." }); }
            LikesT likes = new LikesT();
            likes.ArticleNo = articleNo;
            likes.MemberNo = Profile.UserNo;
            likes.RegId = Profile.UserId;
            likes.RegDt = DateTime.Now;
            likes.RegIp = IPAddressHelper.GetIP(this);
            int result = _likesDac.SetLikes(likes);

            int articleMemberNo = _articleDac.GetArticleByNo(articleNo).MemberNo;

            NoticeT notice = new NoticeT();
            notice.IdxName = "l:" + articleNo.ToString();
            notice.ArticleNo = articleNo;
            notice.MemberNo = Profile.UserNo;
            notice.MemberNoRef = articleMemberNo;
            notice.Type = "like";
            notice.IsNew = "y";
            notice.RefNo = result;
            notice.RegDt = DateTime.Now;
            notice.RegId = Profile.UserId;
            notice.RegIp = IPAddressHelper.GetClientIP();

            _noticesDac.InsertNoticesLikes(notice);
            return Json(new { Success = true, Result = result });
        }
        #endregion

        #region detail page 조회수, 좋아요수, 댓글수 리스팅 파샬뷰
        /// <summary>
        /// 
        /// </summary>
        /// <param name="articleNo"></param>
        /// <returns></returns>
        public PartialViewResult DetailRightCntList(int articleNo)
        {
            ArticleT article = new ArticleT();
            article = _articleDac.GetDetailPageCntList(articleNo, Profile.UserNo);
            return PartialView(article);
        }
        #endregion

        #region 파일 다운로드
        public ActionResult FileDownload(int articleNo, string filePath, string fileName)
        {
            string contentType = "application/octet-stream";
            //string dirPath = System.Configuration.ConfigurationManager.AppSettings["Article3DUrl"];
            //string existPath = AppDomain.CurrentDomain.BaseDirectory + filePath;
            string host = Request.Url.Host;
            string Scheme = Request.Url.Scheme;

            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(Scheme + "://" + host + filePath);
            request.Method = "HEAD";

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                DownloadT download = new DownloadT();
                download.ArticleNo = articleNo;
                download.MemberNo = Profile.UserNo;
                download.RegId = Profile.UserId;
                download.RegIp = IPAddressHelper.GetIP(this);
                download.RegDt = DateTime.Now;
                download.Cnt = 1;
                _downloadDac.InsertDownloadCnt(download);

                return File(filePath, contentType, fileName);
            }
            catch
            {
                return Content("<script type='text/javascript'> alert('존재하지 않는 파일입니다'); history.go(-1);</script>");
            }
        }
        #endregion

        //파일경로 생각좀
        public ActionResult AllDownloadForZip(int articleNo = 0, string zipName = "")
        {
            IList<ArticleFileT> article = _articleFileDac.GetSTLFileList(articleNo);
            HttpWebResponse response = null;
            ZipFile zipFile = new ZipFile();
            WebClient wc = new WebClient();
            MemoryStream stream = new MemoryStream();

            string dirPath = System.Configuration.ConfigurationManager.AppSettings["Article3DUrl"];
            Stream s = null;
            foreach (ArticleFileT f in article)
            {
                var request = (HttpWebRequest)WebRequest.Create(dirPath + f.Rename);
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    s = wc.OpenRead(dirPath + f.Rename);
                    zipFile.AddEntry(f.Name, s);
                }
                catch
                {
                    return Content("<script type='text/javascript'> alert('존재하지 않는 파일이 있습니다.'); history.go(-1);</script>");
                    //return Content(dirPath + f.Rename);
                }
            }

            zipFile.Save(stream);
            zipFile.Dispose();
            s.Dispose();
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/zip", zipName + ".zip");
        }

        public JsonResult AddNewList(string listName)
        {
            ListT list = new ListT();
            list.MemberNo = Profile.UserNo;
            list.ListName = listName;
            list.RegId = Profile.UserId;
            list.RegDt = DateTime.Now;
            int result = _articleDac.InsertNewList(list);
            return Json(new { Success = true, Result = result });
        }

        public JsonResult AddToList(int listNo, int articleNo)
        {
            ListArticleT listArticle = new ListArticleT();
            listArticle.ArticleNo = articleNo;
            listArticle.ListNo = listNo;
            listArticle.MemberNo = Profile.UserNo;
            listArticle.RegId = Profile.UserId;
            listArticle.RegDt = DateTime.Now;

            bool result = _listDac.InsertListArticle(listArticle);
            return Json(new { Success = result });
        }


        #region upload cancle
        /// <summary>
        /// upload cancle
        /// </summary>
        /// <param name="temp"></param>
        /// <returns></returns>
        public JsonResult UploadCancle(string temp)
        {
            bool result = _articleFileDac.UploadCancle(temp);
            return Json(new { Success = result });
        }
        #endregion

        #region A태그 변환
        private string chkContent(string contents)
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
                //contents = contents.Replace(matchEmail.Value, "<a href='mailto:" + matchEmail.Value + "'>" + matchEmail.Value + "</a>");
                matchEmail = matchEmail.NextMatch();
            }

            //string domainList = string.Empty;
            //while (matchDomain.Success)
            //{
            //    if (!domainList.Contains(matchDomain.Value))
            //    {
            //        contents = contents.Replace(matchDomain.Value, "<a href='" + matchDomain.Value + "' target='_blank'>" + matchDomain.Value + "</a>");
            //        domainList += matchDomain.Value;
            //        matchDomain = matchDomain.NextMatch();
            //    }
            //    else
            //    {
            //        matchDomain = matchDomain.NextMatch();
            //    }
            //}

            #region
            //string domainList = string.Empty;
            IList<string> domainList = new List<string>();
            contents += "&nbsp";
            while (matchDomain.Success)
            {
                //if (!domainList.Contains(matchDomain.Value))
                //{
                //    //contents = contents.Replace(matchDomain.Value, "<a href='" + matchDomain.Value + "' target='_blank'>" + matchDomain.Value + "</a>");
                //    //domainList += matchDomain.Value;
                //    contents = contents.Replace(matchDomain.Value.Replace("&nbsp", ""), "<a href='" + matchDomain.Value.Replace("&nbsp", "") + "' target='_blank'>" + matchDomain.Value.Replace("&nbsp","") + "</a>");
                //    domainList.Add(matchDomain.Value);
                //    matchDomain = matchDomain.NextMatch();
                //}
                //else
                //{
                //    matchDomain = matchDomain.NextMatch();
                //}

                string replaceDomain = matchDomain.Value.Replace("&nbsp", "");
                if (!domainList.Contains(replaceDomain))
                {
                    //contents = contents.Replace(matchDomain.Value, "<a href='" + matchDomain.Value + "' target='_blank'>" + matchDomain.Value + "</a>");
                    //domainList += matchDomain.Value;
                    contents = contents.Replace(replaceDomain + "&nbsp", "<a href='" + replaceDomain + "' target='_blank'>" + replaceDomain + "</a>");
                    contents = contents.Replace(replaceDomain + "\r", "<a href='" + replaceDomain + "' target='_blank'>" + replaceDomain + "</a>");

                    domainList.Add(replaceDomain);
                    matchDomain = matchDomain.NextMatch();
                }
                else
                {
                    matchDomain = matchDomain.NextMatch();
                }


            }
            #endregion

            for (int i = 0; i < emailList.Length; i++)
            {
                contents = contents.Replace("#" + i + "#", emailList[i]);
            }

            return contents;
        }
        #endregion

        public ActionResult test(string no = "")
        {
            int articleNo = 0;
            ArticleDetailT detail = new ArticleDetailT();
            if (Int32.TryParse(no, out articleNo))
            {
                detail = _articleDac.GetArticleDetailByArticleNo(articleNo, Profile.UserNo);
                detail.Contents = new ContentFilter().HtmlEncode(detail.Contents);

                ViewBag.Files = _articleFileDac.GetFileList(articleNo);
                ViewBag.MainImg = detail.MainImgName;
            }
            ViewBag.VisitorNo = Profile.UserNo;

            ViewBag.No = no;
            ViewBag.CodeNo = detail.CodeNo;

            detail.Contents = detail.Contents.Replace("#", "");
            detail.Contents = chkContent(detail.Contents);

            return View(detail);
        }

        #region
        public ActionResult Competition(int page = 1)
        {
            int pageSize = 20;
            IList<ArticleDetailT> list = null;
            //int codeNum = int.Parse(codeNo);
            //ViewBag.CodeNo = codeNo;
            //ViewBag.Gubun = pageGubun;

            int totalCnt = _articleDac.GetCompetitionListCount("컨테스트#01");

            int fromIndex = ((page - 1) * pageSize) + 1;
            int toIndex = page * pageSize;

            PagerInfo pager = new PagerInfo();
            pager.CurrentPageIndex = page;
            pager.PageSize = pageSize;
            pager.RecordCount = totalCnt;


            list = _articleDac.GetCompetitionList(Profile.UserNo, fromIndex, toIndex, "컨테스트#01");

            PagerQuery<PagerInfo, IList<ArticleDetailT>> model = new PagerQuery<PagerInfo, IList<ArticleDetailT>>(pager, list);

            return View(model);
        }
        #endregion


        #region 번역 페이지
        [Authorize, HttpGet]
        public ActionResult Translation(string no = "", int transFlag = 0)
        {
            int articleNo = 0;
            if (no == "")
            {
                return RedirectToAction("Upload");
            }

            if (!Int32.TryParse(no, out articleNo))
            {
                //history
                return RedirectToAction("Upload");
            }

            ArticleT articleT = _articleDac.GetArticleForEdit(articleNo);
            TranslationDetailT transDetail = _translationDetailDac.GetTranslationDetailByArticleNoAndLangFlag(articleT.No, "KR");
            if (transDetail != null)
            {
                articleT.Title = transDetail.Title;
                articleT.Tag = transDetail.Tag;
                articleT.Contents = transDetail.Contents;
            }
            if (articleT.MemberNo != Profile.UserNo && Profile.UserLevel < 50) { return RedirectToAction("detail", new { no = articleT.No }); }

            int UploadCnt = _articleFileDac.GetArticleFileCntByNo(articleT.No);

            int totalPages = Math.Max((UploadCnt + 5 - 1) / 5, 1);
            ViewBag.UploadCnt = totalPages == 1 ? totalPages * 5 * 2 : totalPages * 5;
            ViewBag.UploadFileCnt = UploadCnt;
            ViewBag.ArticleNo = articleNo;

            //페이지 들어올때 temp와 같은 임시파일이 존재하는지 체크
            _articleDac.CheckTempFile(articleT.Temp);

            ViewBag.FileList = _articleFileDac.GetFileList(articleNo);
            ViewBag.TransFlag = transFlag;
            ViewBag.WrapClass = "bgW";

            return View(articleT);
        }
        #endregion

        #region 번역
        public JsonResult SetTranslation(FormCollection collection)
        {
            AjaxResponseModel model = new AjaxResponseModel();
            model.Success = false;

            int articleNo = int.Parse(collection["No"]);
            string transTitle = collection["trans_title"];
            string transContents = collection["trans_contents"];
            string transTags = collection["trans_tags"];
            string language = collection["chkLang"];
            int transFlag = int.Parse(collection["transFlag"]);

            TranslationDetailT transDetail = _translationDetailDac.GetTranslationDetailByArticleNoAndLangFlag(articleNo, language);

            if (transDetail != null)
            {
                model.Message = "이미 번역 되어 있습니다.";

            }
            else
            {
                TranslationT translation = _translationDac.GetTranslation(articleNo, language, transFlag);

                if (translation.ReqMemberNo != Profile.UserNo && Profile.UserLevel < 50)
                {
                    model.Message = "번역 권한이 없습니다.";
                    return Json(model);
                }

                transDetail = new TranslationDetailT();
                transDetail.ArticleNo = articleNo;
                transDetail.TranslationNo = translation.No;
                transDetail.Title = transTitle;
                transDetail.Contents = transContents;
                transDetail.Tag = transTags;
                transDetail.LangFlag = language;
                transDetail.RegId = Profile.UserId;
                transDetail.RegDt = DateTime.Now;
                model.Result = _translationDetailDac.SaveOrUpdateTranslationDetail(transDetail).ToString();
                model.Message = "번역 되었습니다.";
                model.Success = true;
            }

            return Json(model);
        }
        #endregion

        #region 번역 요청
        public JsonResult RequestTranslation(FormCollection collection)
        {
            AjaxResponseModel model = new AjaxResponseModel();
            model.Success = false;

            int articleNo = int.Parse(collection["articleNo"]);
            string langFrom = collection["langFrom"];
            string langTo = collection["langTo"];

            TranslationT translation = new TranslationT();
            translation.ArticleNo = articleNo;
            translation.TransFlag = (int)Makersn.Util.MakersnEnumTypes.TranslationFlag.번역요청;
            translation.Status = (int)Makersn.Util.MakersnEnumTypes.TranslationStatus.요청;
            translation.LangFrom = langFrom;
            translation.LangTo = langTo;
            translation.ReqMemberNo = Profile.UserNo;
            translation.ReqDt = DateTime.Now;
            translation.RegId = Profile.UserId;
            translation.RegDt = DateTime.Now;

            TranslationT chkTrans = _translationDac.CheckTranslation(translation);
            if (chkTrans != null && chkTrans.TransFlag == (int)Makersn.Util.MakersnEnumTypes.TranslationFlag.번역요청)
            {
                model.Message = "이미 요청 되어 있습니다.";
                return Json(model);
            }

            //TranslationDetailT chkDetail = _translationDetailDac.GetTranslationDetailByArticleNoAndLangFlag(articleNo, langTo);
            //if (chkDetail != null)
            //{
            //    model.Message = "해당 언어로 번역이 되어 있습니다.";
            //    return Json(model);
            //}

            model.Result = _translationDac.InsertTranslation(translation).ToString();
            model.Success = true;
            model.Message = "요청 되었습니다.";

            return Json(model);
        }
        #endregion

        #region 직접 번역
        public JsonResult DirectTranslation(FormCollection collection)
        {
            AjaxResponseModel model = new AjaxResponseModel();
            model.Success = false;

            int articleNo = int.Parse(collection["articleNo"]);
            string langFrom = collection["langFrom"];
            string langTo = collection["langTo"];

            TranslationT translation = new TranslationT();
            translation.ArticleNo = articleNo;
            translation.TransFlag = (int)Makersn.Util.MakersnEnumTypes.TranslationFlag.직접번역;
            translation.Status = (int)Makersn.Util.MakersnEnumTypes.TranslationStatus.완료;
            translation.LangFrom = langFrom;
            translation.LangTo = langTo;
            translation.ReqMemberNo = Profile.UserNo;
            translation.ReqDt = DateTime.Now;
            translation.RegId = Profile.UserId;
            translation.RegDt = DateTime.Now;
            translation.WorkMemberNo = Profile.UserNo;
            translation.WorkDt = DateTime.Now;

            TranslationDetailT chkDetail = _translationDetailDac.GetTranslationDetailByArticleNoAndLangFlag(articleNo, langTo);
            if (chkDetail != null)
            {
                model.Message = "이미 번역 되어 있습니다. \n수정은 편집 버튼을 이용해주세요.";
                return Json(model);
            }

            TranslationT chkTrans = _translationDac.CheckTranslation(translation);
            if (chkTrans != null && chkTrans.TransFlag == 2)
            {
                model.Message = "이미 번역 되어 있습니다. \n수정은 편집 버튼을 이용해주세요.";
                return Json(model);
            }

            //기존 번역요청 삭제
            if (chkTrans != null && chkTrans.TransFlag == (int)Makersn.Util.MakersnEnumTypes.TranslationFlag.번역요청)
            {
                _translationDac.DeleteTranslation(chkTrans);
            }

            model.Result = _translationDac.InsertTranslation(translation).ToString();
            model.Success = true;

            return Json(model);
        }
        #endregion

    }
}
