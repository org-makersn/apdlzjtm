using Design.Web.Front.Helper;
using Design.Web.Front.Models;
using Library.ObjParser;
using Makersn.BizDac;
using Makersn.Models;
using Makersn.Util;
using Net.Common.Filter;
using Net.Common.Helper;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using Newtonsoft.Json;
using QuantumConcepts.Formats.StereoLithography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Design.Web.Front.Controllers
{
    public class ProductController : BaseController
    {
        StoreLikesBiz likesBiz = new StoreLikesBiz();
        StoreMemberBiz memberBiz = new StoreMemberBiz();
        private Extent Size { get; set; }// Stl Model Size
        //
        // GET: /Product/
        public ActionResult Index()
        {
            ViewBag.UserNo = Profile.UserNo;
            return View();
        }

        //
        // 좋아요 CREATE / DELETE (상품번호)
        public JsonResult SetLikes(int productNo = 2)
        {
            int result = 0;

            StoreLikesT like = new StoreLikesT();
            like.MemberNo = Profile.UserNo;
            like.RegIp = IPAddressHelper.GetIP(this);
            like.ProductNo = productNo;
            like.RegId = "test";
            like.RegDt = DateTime.Now;

            result = likesBiz.set(like);

            return Json(new { Success = true, Result = result }, JsonRequestBehavior.AllowGet);
        }

        //
        // 상품별 좋아요 갯수 출력 (상품번호)
        public JsonResult CountLikesByProductNo(int productNo = 1)
        {
            int result = likesBiz.countLikesByProductNo(productNo);

            return Json(new { Success = true, Result = result }, JsonRequestBehavior.AllowGet);
        }

        //
        // 내가 좋아요 체크한 상품 리스트 (회원번호)
        public ActionResult GetLikedProductsByMemberNo(int memberNo = 202)
        {
            var result = likesBiz.getLikedProductsByMemberNo(memberNo);
            return PartialView(result);
            //return Json(new { Success = true, Result = result }, JsonRequestBehavior.AllowGet);
        }

        //
        public ActionResult GetReceivedNoteListByMemberNo(int memberNo)
        {
            var result = memberBiz.getReceivedNoteListByMemberNo(memberNo);
            return PartialView(result);
        }

        public ActionResult GetSentNoteListByMemberNo(int memberNo)
        {
            var result = memberBiz.getSentNoteListByMemberNo(memberNo);
            return PartialView(result);
        }

        public JsonResult SendNote(int fromMember, int targetMember, string comment)
        {

            MemberMsgT msg = new MemberMsgT();

            msg.MemberNo = fromMember;
            msg.MemberNoRef = targetMember;
            msg.Comment = comment;
            msg.MsgGubun = "NOTE";
            msg.RegDt = DateTime.Now;
            msg.RegId = "test";
            msg.RegIp = IPAddressHelper.GetIP(this);
            msg.IsNew = "Y";
            msg.SiteGubun = "Store";
            msg.DelFlag = "N";

            int result = memberBiz.sendNote(msg);
            return Json(new { Success = true, Result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteNote(int SeqNo)
        {
            int result = memberBiz.deleteNote(SeqNo);
            return Json(new { Success = true, Result = result }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 가격 선정
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult ProductPricing()
        {
            return View();
        }
        public ActionResult ProductPrintable()
        {
            return View();
        }
        //public ActionResult ProductUpload() {
        //    return View();
        //}

        #region get방식 제품 업로드

        [Authorize, HttpGet]
        public ActionResult ProductUpload()
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
        #endregion

        #region post방식 제품 업로드(필요없을듯 )
        //    /// <summary>
        //    /// article upload
        //    /// </summary>
        //    /// <param name="collection"></param>
        //    /// <returns></returns>
        //    [HttpPost]
        //    public JsonResult ProductUpload(FormCollection collection)
        //    {
        //        AjaxResponseModel response = new AjaxResponseModel();
        //        response.Success = false;

        //        int articleNo = 0;

        //        string paramNo = collection["No"];
        //        string temp = collection["temp"];
        //        string mode = collection["mode"];
        //        int mainImg = Convert.ToInt32(collection["main_img"]);
        //        string articleTitle = collection["article_title"];
        //        string articleContents = collection["article_contents"];
        //        int codeNo = Convert.ToInt32(collection["lv1"]);
        //        int copyright = Convert.ToInt32(collection["copyright"]);
        //        string delNo = collection["del_no"];
        //        string VideoSource = collection["insertVideoSource"];
        //        //string[] splitArray = articleContents.Split('#');
        //        //string tags = "";
        //        string tags = collection["article_tags"];

        //        var mulltiSeq = collection["multi[]"];
        //        string[] seqArray = mulltiSeq.Split(',');


        ////        //for (int i = 1; i < splitArray.Length; i++)
        ////        //{
        ////        //    try
        ////        //    {
        ////        //        splitArray[i] = splitArray[i].Replace("\r\n", " ");
        ////        //        splitArray[i] = splitArray[i].Replace("\n", " ");
        ////        //        splitArray[i] = splitArray[i].Replace("\r", " ");
        ////        //        tags += splitArray[i].Substring(0, splitArray[i].IndexOf(' '));
        ////        //    }
        ////        //    catch
        ////        //    {
        ////        //        tags += splitArray[i];
        ////        //    }
        ////        //    if (i < splitArray.Length - 1) { tags += ","; };
        ////        //}
        ////        //articleContents = articleContents.Replace("#", "");
        ////        if (tags.Length > 1000)
        ////        {
        ////            response.Message = "태그는 1000자 이하로 가능합니다.";
        ////            return Json(response, JsonRequestBehavior.AllowGet);
        ////        }
        ////        ArticleT articleT = null;
        ////        //TranslationT tran = null;
        ////        TranslationDetailT tranDetail = null;
        ////        if (!Int32.TryParse(paramNo, out articleNo))
        ////        {
        ////            response.Success = false;
        ////            response.Message = "pk error";
        ////        }

        ////        if (articleNo > 0)
        ////        {
        ////            //update
        ////            articleT = _articleDac.GetArticleByNo(articleNo);
        ////            tranDetail = _translationDetailDac.GetTranslationDetailByArticleNoAndLangFlag(articleT.No, "KR");
        ////            if (tranDetail != null)
        ////            {
        ////                articleT.Title = tranDetail.Title;
        ////                articleT.Contents = tranDetail.Contents;
        ////                articleT.Tag = tranDetail.Tag;
        ////            }

        ////            if (articleT != null)
        ////            {
        ////                if (articleT.MemberNo == Profile.UserNo && articleT.Temp == temp)
        ////                {
        ////                    articleT.UpdDt = DateTime.Now;
        ////                    articleT.UpdId = Profile.UserId;
        ////                    articleT.RegIp = IPAddressHelper.GetIP(this);
        ////                }
        ////            }
        ////        }
        ////        else
        ////        {
        ////            //save
        ////            articleT = new ArticleT();
        ////            articleT.MemberNo = Profile.UserNo;
        ////            //태그 #**
        ////            articleT.Tag = tags;
        ////            articleT.Temp = temp;
        ////            articleT.ViewCnt = 0;
        ////            articleT.RegDt = DateTime.Now;
        ////            articleT.RegId = Profile.UserId;
        ////            articleT.RegIp = IPAddressHelper.GetIP(this);
        ////            articleT.RecommendYn = "N";
        ////            articleT.RecommendDt = null;
        ////        }

        ////        if (tranDetail == null)
        ////        {

        ////            //영문텍스트 TRANSLATION_DETAIL
        ////            tranDetail = new TranslationDetailT();
        ////            tranDetail.LangFlag = "KR";
        ////            tranDetail.RegId = Profile.UserId;
        ////            tranDetail.RegDt = DateTime.Now;
        ////        }

        ////        //return Json(response);

        ////        if (articleT != null)
        ////        {
        ////            articleT.Title = articleTitle;
        ////            articleT.Contents = articleContents;
        ////            articleT.Visibility = mode == "upload" ? "Y" : "N";
        ////            articleT.Copyright = copyright;
        ////            articleT.CodeNo = codeNo;
        ////            articleT.MainImage = mainImg;

        ////            articleT.VideoUrl = VideoSource;

        ////            articleT.Tag = tags;
        ////            articleNo = _articleDac.SaveOrUpdate(articleT, delNo);


        ////            //영문텍스트 TRANSLATION_DETAIL
        ////            tranDetail.ArticleNo = articleNo;
        ////            tranDetail.Title = articleT.Title;
        ////            tranDetail.Contents = articleT.Contents;
        ////            tranDetail.Tag = articleT.Tag;


        ////            _translationDetailDac.SaveOrUpdateTranslationDetail(tranDetail);

        ////            response.Success = true;
        ////            response.Result = articleNo.ToString();
        ////        }

        ////        _articleFileDac.UpdateArticleFileSeq(seqArray);


        //        return Json(response, JsonRequestBehavior.AllowGet);
        //    }
        #endregion

        #region 제품 업로드 정보 확인 페이지
        public ActionResult ProductUploadDetail(int productNo)
        {
            StoreProductT storeProduct = new StoreProductBiz().getStoreProductById(productNo);
            ViewBag.ProductEntity = storeProduct;
            return View();

        }
        #endregion


        #region stl upload
        /// <summary>
        /// stl upload
        /// </summary>
        /// <param name="stlupload"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StlUpload(FormCollection collection, IEnumerable<HttpPostedFileBase> ProductStlUploads)
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

            int uploadCnt = ProductStlUploads.Count();
            bool Success = false;
            string Message = string.Empty;
            int[] Result = new int[uploadCnt];
            int index = 0;
            long productNo = 0;
            #endregion

            //HttpPostedFileBase stlupload = Request.Files["stlupload"];
            string temp = collection["temp"];

            foreach (HttpPostedFileBase stlupload in ProductStlUploads)
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

            foreach (HttpPostedFileBase stlupload in ProductStlUploads)
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

                                StoreProductT _storeProduct = new StoreProductT();

                                _storeProduct.VarNo = 10000;
                                _storeProduct.ProductName = "";
                                _storeProduct.FilePath = "";
                                _storeProduct.FileName = stlupload.FileName;
                                _storeProduct.FileReName = fileName;
                                _storeProduct.FileReName = fileName;
                                _storeProduct.FileExt = extension;
                                _storeProduct.MimeType = "";
                                _storeProduct.FileSize = Convert.ToDouble(stlupload.ContentLength.ToString());
                                _storeProduct.SlicedVolume = new STLHelper().slicing(file3Dpath + fileName);
                                _storeProduct.ModelVolume = sizeResult.Volume;
                                _storeProduct.SizeX = sizeResult.X;
                                _storeProduct.SizeY = sizeResult.Y;
                                _storeProduct.SizeZ = sizeResult.Z;
                                _storeProduct.Scale = 1.0;
                                _storeProduct.ShortLing = "";
                                _storeProduct.VideoUrl = "";
                                _storeProduct.VideoType = "";
                                _storeProduct.CategoryNo = 0;
                                _storeProduct.Content = "";
                                _storeProduct.Description = "";
                                _storeProduct.PartCnt = 1;
                                _storeProduct.CustormizeYn = "Y";
                                _storeProduct.SellYn = "Y";
                                _storeProduct.TagName = "";
                                _storeProduct.CertiFicateStatus = 1;
                                _storeProduct.VisibilityYn = "Y";
                                _storeProduct.UseYn = "Y";
                                _storeProduct.MemberNo = 0;
                                _storeProduct.TxtSizeX = 0;
                                _storeProduct.TxtSizeY = 0;
                                _storeProduct.DetailType = 0;
                                _storeProduct.DetailDepth = 0;
                                _storeProduct.TxtLoc = "";
                                _storeProduct.RegDt = DateTime.Now;
                                _storeProduct.RegId = Profile.UserId;
                                _storeProduct.UpdDt = DateTime.Now;
                                _storeProduct.UpdId = Profile.UserId;

                                IList<StoreProductT> list = new StoreProductBiz().getAllStoreProduct();
                                productNo = new StoreProductBiz().add(_storeProduct);

                                //ArticleFileT articleFileT = new ArticleFileT();

                                //articleFileT.FileGubun = "temp";
                                //articleFileT.FileType = "stl";
                                //articleFileT.MemberNo = Profile.UserNo;
                                //articleFileT.Seq = 5000;
                                //articleFileT.ImgUseYn = "N";
                                //articleFileT.Ext = extension;
                                //articleFileT.ThumbYn = "N";
                                //articleFileT.MimeType = stlupload.ContentType;
                                //articleFileT.Name = stlupload.FileName;
                                //articleFileT.Size = stlupload.ContentLength.ToString();
                                //articleFileT.Rename = fileName;
                                //articleFileT.Path = string.Format("/{0}/", save3DFolder);
                                ////articleFileT.Width = "630";
                                ////articleFileT.Height = "470";
                                //articleFileT.X = sizeResult.X;
                                //articleFileT.Y = sizeResult.Y;
                                //articleFileT.Z = sizeResult.Z;
                                //articleFileT.Volume = sizeResult.Volume;
                                ////articleFileT.PrintVolume = new STLHelper().slicing(file3Dpath + fileName);

                                //articleFileT.UseYn = "Y";
                                //articleFileT.Temp = temp;
                                //articleFileT.RegIp = IPAddressHelper.GetIP(this);
                                //articleFileT.RegId = Profile.UserId;
                                //articleFileT.RegDt = DateTime.Now;

                                ////int articleFileNo = _articleFileDac.InsertArticleFile(articleFileT);

                                ////response.Success = true;
                                ////response.Result = articleFileNo.ToString();
                                Success = true;
                                //Result[index] = articleFileNo;
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
                //_articleFileDac.UpdateArticleFileSeq(seqArray);
            }


            return Json(new { Success = Success, Message = Message, Result = Result, Count = uploadCnt, ProductNo = productNo }, JsonRequestBehavior.AllowGet);
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


    }
}
