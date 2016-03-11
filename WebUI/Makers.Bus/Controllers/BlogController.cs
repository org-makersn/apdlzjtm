using Makers.Bus.Helper;
using Net.Common.Filter;
using Net.Common.Helper;
using Net.Framework.BizDac;
using Net.Framework.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Makers.Bus.Controllers
{
    public class BlogController : BaseController
    {
        BusManageDac busManageDac = new BusManageDac();

        public BlogController()
        {
            ViewData["Menu"] = "blog";
        }

        /// <summary>
        /// 블로그 리스트
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(int page = 1)
        {
            int pageSize = page_size;

            IList<BusBlog> list = null;

            int fromIndex = ((page - 1) * pageSize) + 1;
            int toIndex = page * pageSize;

            int totalCnt = busManageDac.GetBusBlogTotalCountByUseYn("Y");

            list = busManageDac.GetBusBlogListByOption("Y", fromIndex, toIndex);

            PagerInfo pager = new PagerInfo();
            pager.CurrentPageIndex = page;
            pager.PageSize = pageSize;
            pager.RecordCount = totalCnt;
            PagerQuery<PagerInfo, IList<BusBlog>> model = new PagerQuery<PagerInfo, IList<BusBlog>>(pager, list);
            return View(model);
        }

        /// <summary>
        /// 블로그 상세
        /// </summary>
        /// <returns></returns>
        public ActionResult Views(string no)
        {
            long blogNo = 0;

            BusBlog blog = new BusBlog();

            if (long.TryParse(no, out blogNo))
            {
                blog = busManageDac.GetBlogByNo(blogNo);
                if (blog != null)
                {
                    string strNo = no.ToString();
                    int idx = 0;

                    //조회수 증가 방지
                    if (Request.Cookies["views"] == null)
                    {
                        //생성 > 증가
                        Response.Cookies["views"].Value = Base64Helper.Base64Encode(strNo);
                        Response.Cookies["views"].Expires = DateTime.Now.AddDays(1);
                        idx += 1;
                    }
                    else
                    {
                        //비교
                        string value = Base64Helper.Base64Decode(Request.Cookies["views"].Value);
                        string[] arrNo = value.Split('`');
                        if (!arrNo.Any(t => t == strNo))
                        {
                            //append > 증가
                            Response.Cookies["views"].Value = Base64Helper.Base64Encode(value + "`" + strNo);
                            idx += 1;
                        }
                    }

                    if (idx > 0)
                    {
                        busManageDac.UpdateBlogViewCnt(blogNo);
                        blog.VIEW_CNT += 1;
                    }

                    blog.BLOG_CONTENTS = HtmlFilter.PunctuationDecode(blog.BLOG_CONTENTS);
                }
                else
                {
                    return Content("<script>alert('잘못된 게시글 입니다.'); location.href='/blog';</script>");
                }
            }
            else
            {
                return Content("<script>alert('잘못된 게시글 입니다.'); location.href='/blog';</script>");
            }

            return View(blog);
        }
    }
}
