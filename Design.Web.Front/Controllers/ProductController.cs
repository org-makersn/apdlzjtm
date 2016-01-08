using Design.Web.Front.Models;
using Library.ObjParser;
using Makersn.Models;
using Net.Common.Define;
using Net.Common.Helper;
using Net.Framework.StoreModel;
using Net.Framwork.BizDac;
using QuantumConcepts.Formats.StereoLithography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public ActionResult Create(string ex)
        {
            return View();
        }

        #region stl upload
        /// <summary>
        /// stl upload
        /// </summary>
        /// <param name="stlupload"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StlUpload(FormCollection collection, IEnumerable<HttpPostedFileBase> product_3d_files)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;

            int uploadCnt = product_3d_files.Count();
            int[] Result = new int[uploadCnt];
            int index = 0;
            long productNo = 0;

            string temp = collection["temp"];

            foreach (HttpPostedFileBase stlupload in product_3d_files)
            {
                if (stlupload != null)
                {
                    if (stlupload.ContentLength > 0)
                    {
                        if (stlupload.ContentLength < 200 * 1024 * 1024)
                        {
                            string[] extType = { ".stl", ".obj" };

                            string extension = Path.GetExtension(stlupload.FileName);

                            if (extType.Contains(extension.ToLower()))
                            {
                                string save3DFolder = @"\product\3d-files";
                                //string saveJSFolder = @"\product\js-files";

                                string fileReName = FileHelper.UploadFile(stlupload, null, save3DFolder, null);
                                
                                StoreProductT _storeProduct = new StoreProductT();

                                _storeProduct.VarNo = 10000;
                                _storeProduct.ProductName = stlupload.FileName.Replace(extension, string.Empty).Replace("_", " ");
                                _storeProduct.FilePath = save3DFolder;
                                _storeProduct.FileName = stlupload.FileName;
                                _storeProduct.FileReName = fileReName;
                                _storeProduct.FileExt = extension.ToLower();
                                _storeProduct.MimeType = stlupload.ContentType;
                                _storeProduct.FileSize = Convert.ToDouble(stlupload.ContentLength.ToString());

                                _storeProduct.SlicedVolume = 0;
                                _storeProduct.ModelVolume = 0;
                                _storeProduct.SizeX = 0;
                                _storeProduct.SizeY = 0;
                                _storeProduct.SizeZ = 0
                                    ;
                                _storeProduct.Scale = 100;
                                _storeProduct.ShortLing = "";
                                _storeProduct.VideoUrl = "";
                                //_storeProduct.VideoType = "";
                                _storeProduct.CategoryNo = 0;
                                _storeProduct.Contents = "";
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
                                
                                IList<StoreProductT> list = new StoreProductBiz().getAllStoreProduct();
                                productNo = new StoreProductBiz().addStoreProduct(_storeProduct);

                                response.Success = true;
                                response.Result = productNo.ToString();

                                //Result[index] = articleFileNo;
                                index++;
                            }
                            else
                            {
                                response.Message = Constant.Product.STL_FileUpload_Validate_Ext_Msg;
                            }
                        }
                        else
                        {
                            response.Message = Constant.Product.STL_FileUpload_Max_Size__Msg;
                        }
                    }
                }
            }

            return Json(new { Success = response.Success, Message = response.Message, Result = response.Result, Count = uploadCnt, ProductNo = productNo }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 제품 관련
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        public ActionResult Models(int no)
        {
            StoreProductT storeProduct = new StoreProductBiz().getStoreProductById(no);

            ViewBag.MaterialList = new StoreMaterialBiz().getAllStoreMaterial();
            return View(storeProduct);
        }

        /// <summary>
        /// modify
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Models(int productNo, string productName, int categoryNo, string content, string description, string tagName, string videoUrl)
        {
            AjaxResponseModel response = new AjaxResponseModel();
            response.Success = false;

            StoreProductT product = new StoreProductBiz().getStoreProductById(productNo);
            if (product != null)
            {
                product.ProductName = productName;
                product.CategoryNo = categoryNo;
                product.Contents = content;
                product.Description = description;
                product.TagName = tagName;
                product.VideoUrl = videoUrl;
                //product.VideoType = "";

                int ret = new StoreProductBiz().setStoreProduct(product);
                if (ret > 0)
                {
                    response.Success = true;
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 좋아요 관련
        
        /// <summary>
        /// 좋아요 CREATE / DELETE (상품번호)
        /// </summary>
        /// <param name="productNo"></param>
        /// <returns></returns>
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
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberNo"></param>
        /// <returns></returns>
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

        #region 모델링 파일 사이즈 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
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
