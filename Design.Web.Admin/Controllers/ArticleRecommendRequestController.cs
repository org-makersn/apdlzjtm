using Design.Web.Admin.Models;
using Makersn.BizDac;
using Makersn.Models;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Design.Web.Admin.Controllers
{
    [Authorize]
    public class ArticleRecommendRequestController : BaseController
    {
        private MenuModel menuModel = new MenuModel();
        ArticleDac articleDac = new ArticleDac();

        public MenuModel MenuModel(int subIndex)
        {
            menuModel.Group = "_Management";
            menuModel.MainIndex = 4;
            menuModel.SubIndex = subIndex;
            return menuModel;
        }
       
        public ActionResult Design()
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(2);
            return View();
        }
        public ActionResult Printer()
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(2);
            return View();
        }


        //Edit recommandation
        public JsonResult SetVisibiliy(string no = "", string setVisi = "")
        {
            articleDac.UpdateVisibility(no, setVisi);
            return Json(new { result = 1 });
        }

        public JsonResult SetRecommend(string no = "", string setNo = "")
        {
            articleDac.UpdateRecommend(no, setNo);
            return Json(new { result = 1 });
        }


        public PartialViewResult RecommendEdit(int page = 1, string orderby = "regdt", int cate = 0, string RecommendYn = "", string option = "", string text = "")
        {
            ViewData["Group"] = MenuModel(1);

            IList<ArticleT> list = articleDac.GetArticleListByAdminPage(cate, RecommendYn,"", text);
            ViewData["cnt"] = list.Count;
            ViewData["cateList"] = articleDac.GetArticleCodeNo();
            ViewData["setCate"] = cate;
            ViewData["RecommendYn"] = RecommendYn;
            ViewData["text"] = text;

            if (orderby == "pop")
            {
                return PartialView(list.OrderByDescending(o => o.Pop).ToPagedList(page, 20));
            }
            if (orderby == "recommend")
            {
                list = list.Where(w => w.RecommendYn == "Y").ToList<ArticleT>();
            }
            return PartialView(list.OrderByDescending(o => o.RegDt).ToPagedList(page, 20));
        }
        

        //LIST recommendation
        public PartialViewResult RecommendList(int page = 1, string orderby = "regdt", int cate = 0,  string option = "", string text = "")
        {
            string RecommendYn = "Y";
            ViewData["Group"] = MenuModel(1);

            IList<ArticleT> list = articleDac.GetArticleListByAdminPage(cate, RecommendYn,"", text);
            ViewData["cnt"] = list.Count;
            ViewData["cateList"] = articleDac.GetArticleCodeNo();
            ViewData["setCate"] = cate;
            ViewData["text"] = text;

            //if (orderby == "pop")
            //{
            //    return PartialView(list.OrderByDescending(o => o.Pop).ToPagedList(page, 20));
            //}
            //if (orderby == "recommend")
            //{
            //    list = list.Where(w => w.RecommendYn == "Y").ToList<ArticleT>();
            //}
            //if (orderby == "recommendPriority")
            //{
            //    return PartialView(list.OrderByDescending(o => o.Priority).ToPagedList(page, 20));
            //}

            return PartialView(list.OrderByDescending(o => o.RecommendPriority).ThenByDescending(t=>t.RecommendDt).ToPagedList(page, 20));
        }


        
        public ActionResult RecommendListEdit(int no) 
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            ViewData["Group"] = MenuModel(1);
            ArticleT articleT = articleDac.GetArticleByNo(no);
            return PartialView(articleT);
        }

        public ActionResult RecommendListUpdatePriority(int no, int priority)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            articleDac.UpdatePriority(no, priority);
            return Redirect("../ArticleRecommendRequest/Design");
        }

        public ActionResult RecommendListUpdateVisibility(int no, string visibility)
        {
            if (Profile.UserLevel < 50) { return Redirect("/account/logon"); }

            articleDac.UpdateRecommendVisibility(no, visibility);
            return Redirect("../ArticleRecommendRequest/Design");
        }

        

        
    }
}