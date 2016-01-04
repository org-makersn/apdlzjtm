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

namespace Design.Web.Front.Controllers
{
    //[Authorize]
    public class MainController : BaseController
    {
        ArticleDac articleDac = new ArticleDac();
        MemberT member = new MemberT();
        MemberDac memberDac = new MemberDac();

        private Extent Size { get; set; }

        public ActionResult Index(string url = "")
        {
            if (url != "")
            {
                return Content(@"<form action='/profile' id='goBlog' method='post'>
                                            <input type='hidden' name='url' value='" + url + @"' />
                                             </form><script>document.getElementById('goBlog').submit();</script>");
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

            ViewBag.IsMain = "Y";
            IList<ArticleDetailT> recommendList = new List<ArticleDetailT>();
            var stateList = EnumHelper.GetTitleAndSynonyms<MakersnEnumTypes.CateName>();

            recommendList = articleDac.GetListByOption(Profile.UserNo, 0, "R", 1, 4);
            if (recommendList.Count == 0) { recommendList = articleDac.GetListByOption(Profile.UserNo, 0, "P", 1, 4); };
            ViewBag.RecommendList = recommendList;
            ViewBag.PopularList = articleDac.GetListByOption(Profile.UserNo, 0, "P", 1, 4);
            ViewBag.NewList = articleDac.GetListByOption(Profile.UserNo, 0, "N", 1, 4);

            //ViewBag.ComList = articleDac.GetListByOption(Profile.UserNo, 10203, "", 1, 4);

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
            string save3DFolder = "Article/article_3d";

            string file3Dpath = string.Format("{0}/FileUpload/{1}/", AppDomain.CurrentDomain.BaseDirectory, save3DFolder);

            //StlModel stlModel = new STLHelper().GetStlModel(file3Dpath + "1bfaedca-ce7c-4f2c-be6a-6f47789321c5.stl", "stl");

            STLDocument facets = STLDocument.Open(file3Dpath + "a8b84e2f-4740-4171-847b-d08235bf6fd7.stl");

            OBJDocument objDoc = new OBJDocument().LoadObj(file3Dpath + "uploads-ab-35-38-38-92-BoulderHiveFinal_ExportOBJ.obj");
            //Size = new Extent
            //{
            //    //XMax = facets.Facets[0].Vertices.Max(v => v.X),
            //    //XMin = facets.Facets[0].Vertices.Min(v => v.X),
            //    //YMax = facets.Facets[0].Vertices.Max(v => v.Y),
            //    //YMin = facets.Facets[0].Vertices.Min(v => v.Y),
            //    //ZMax = facets.Facets[0].Vertices.Max(v => v.Z),
            //    //ZMin = facets.Facets[0].Vertices.Min(v => v.Z)
            //    XMax = facets.Facets.Max(f => f.Vertices.Max(v => v.X)),
            //    XMin = facets.Facets.Min(f => f.Vertices.Min(v => v.X)),
            //    YMax = facets.Facets.Max(f => f.Vertices.Max(v => v.Y)),
            //    YMin = facets.Facets.Min(f => f.Vertices.Min(v => v.Y)),
            //    ZMax = facets.Facets.Max(f => f.Vertices.Max(v => v.Z)),
            //    ZMin = facets.Facets.Min(f => f.Vertices.Min(v => v.Z)),
            //};

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
            double stlVolume = 0;
            double x1 = 0;
            double y1 = 0;
            double z1 = 0;
            double x2 = 0;
            double y2 = 0;
            double z2 = 0;
            double x3 = 0;
            double y3 = 0;
            double z3 = 0;


            //stl
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

                stlVolume +=
                    (-x3 * y2 * z1 +
                    x2 * y3 * z1 +
                    x3 * y1 * z2 -
                    x1 * y3 * z2 -
                    x2 * y1 * z3 +
                    x1 * y2 * z3) / 6;
            }

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

            double x5 = 0;
            double y5 = 0;
            double volume2 = 0;

            for (int i = 0; i < objDoc.FaceList.Count; i++)
            {
                x5 = objDoc.TextureList[objDoc.FaceList[i].TextureVertexIndexList[0]].X;
                y5 = objDoc.TextureList[objDoc.FaceList[i].TextureVertexIndexList[0]].Y;

                volume2 += x5 * y5;
            }

            ViewBag.Size = Size.XSize + "x" + Size.YSize + "x" + Size.ZSize;
            ViewBag.Volume = volume + "???????????????" + volume2;
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

    }
}
