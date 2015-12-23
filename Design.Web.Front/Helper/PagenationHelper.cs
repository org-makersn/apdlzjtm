using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Design.Web.Front.Helper
{
    public static class PagenationHelper
    {
        public static HtmlString PageNavigate(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount)
        {
            //Uri requestUrl = HttpContext.Current.Request.Url;
            string code = HttpContext.Current.Request.QueryString["codeNo"];
            string gbn = HttpContext.Current.Request.QueryString["pageGubun"];
            string globleType = HttpContext.Current.Request.QueryString["gl"];

            var redirectTo = HttpContext.Current.Request.Url.AbsolutePath + "?";

            var rawUrl = HttpContext.Current.Request.RawUrl;

            //if (rawUrl.Contains("?"))
            //{
            //    redirectTo += rawUrl.Substring(rawUrl.IndexOf("?")+1) + "&";
            //}

            string segment = string.Empty;
            if (!string.IsNullOrEmpty(code))
            {
                segment += "&codeNo=" + code;
            }

            if (!string.IsNullOrEmpty(gbn))
            {
                segment += "&pageGubun=" + gbn;
            }

            if (!string.IsNullOrEmpty(globleType))
            {
                segment += "&globleType=" + globleType;
            }

            pageSize = pageSize == 0 ? 3 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1);
            var output = new StringBuilder();
            output.Append("<div class='paging'>");
            if (totalPages > 1)
            {
                if (currentPage != 1)
                {
                    //output.AppendFormat("<a href='{0}page=1{1}' class='first_page'>처음 페이지</a>", redirectTo, segment);
                    int tenPre = currentPage - 10 <= 0 ? 1 : currentPage - 10;
                    output.AppendFormat("<a href='{0}page={1}{2}' class='first_page'>처음 페이지</a>", redirectTo, tenPre, segment);
                }
                if (currentPage > 1)
                {
                    output.AppendFormat("<a href='{0}page={1}{2}' rel='prev' class='prev_page'>이전 페이지</a>", redirectTo, currentPage - 1, segment);
                }

                output.Append(" ");
                int currint = 5;
                int loopTime = 10; // loop time of paging

                if ((currentPage - currint) < 1) // special proceed at begining
                {
                    loopTime = loopTime + currint - currentPage;
                }
                if ((currentPage + currint) > totalPages)
                { // special preceed at the end
                    currint = currint + currint - totalPages + currentPage;
                }

                for (int i = 0; i <= loopTime; i++)
                {
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {
                            output.AppendFormat("<a class='paging_no on'>{0}</a>", currentPage);
                        }
                        else
                        {
                            output.AppendFormat("<a href='{0}page={1}{2}' class='paging_no'>{3}</a>", redirectTo, currentPage + i - currint, segment, currentPage + i - currint);
                        }
                    }

                    output.Append(" ");
                }
                if (currentPage < totalPages)
                {
                    output.AppendFormat("<a href='{0}page={1}{2}' rel='next' class='next_page'>다음 페이지</a>", redirectTo, currentPage + 1, segment);
                }

                output.Append(" ");
                if (currentPage != totalPages)
                {
                    //output.AppendFormat("<a href='{0}page={1}{2}' class='last_page'>마지막 페이지</a>", redirectTo, totalPages, segment);
                    int tenNext = currentPage + 10 > totalPages ? totalPages : currentPage + 10;
                    output.AppendFormat("<a href='{0}page={1}{2}' class='last_page'>마지막 페이지</a>", redirectTo, tenNext, segment);
                }
                output.Append(" ");
            }
            //output.AppendFormat("<label>第{0}页 / 共{1}页</label>", currentPage, totalPages);

            output.Append("</div>");
            return new HtmlString(output.ToString());
        }


    }

    public class PagerInfo
    {
        public int RecordCount { get; set; }

        public int CurrentPageIndex { get; set; }

        public int PageSize { get; set; }
    }


    public class PagerQuery<TPager, TEntityList>
    {
        public PagerQuery(TPager pager, TEntityList entityList)
        {
            this.Pager = pager;
            this.EntityList = entityList;
        }
        public TPager Pager { get; set; }
        public TEntityList EntityList { get; set; }
    }

    public static class PagenationHelperForPartial
    {
        public static HtmlString PageNavigateForPartial(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount, string no)
        {
            //Uri requestUrl = HttpContext.Current.Request.Url;
            string code = HttpContext.Current.Request.QueryString["codeNo"];
            string gbn = HttpContext.Current.Request.QueryString["pageGubun"];

            var redirectTo = HttpContext.Current.Request.Url.AbsolutePath;
            string segment = string.Empty;
            if (!string.IsNullOrEmpty(code))
            {
                segment += "&codeNo=" + code;
            }

            if (!string.IsNullOrEmpty(gbn))
            {
                segment += "&pageGubun=" + gbn;
            }

            pageSize = pageSize == 0 ? 3 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1);
            var output = new StringBuilder();
            output.Append("<div class='paging'>");
            if (totalPages > 1)
            {
                if (currentPage != 1)
                {
                    output.AppendFormat("<a href='{0}page=1' class='first_page'>처음 페이지</a>", redirectTo);
                }
                if (currentPage > 1)
                {
                    output.AppendFormat("<a href='{0}page={1}{2}' rel='prev' class='prev_page'>이전 페이지</a>", redirectTo, currentPage - 1, segment);
                }

                output.Append(" ");
                int currint = 5;
                int loopTime = 10; // loop time of paging

                if ((currentPage - currint) < 1) // special proceed at begining
                {
                    loopTime = loopTime + currint - currentPage;
                }
                if ((currentPage + currint) > totalPages)
                { // special preceed at the end
                    currint = currint + currint - totalPages + currentPage;
                }

                for (int i = 0; i <= loopTime; i++)
                {
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {
                            output.AppendFormat("<a class='paging_no on'>{0}</a>", currentPage);
                        }
                        else
                        {
                            output.AppendFormat("<a href='{0}page={1}{2}' class='paging_no'>{3}</a>", redirectTo, currentPage + i - currint, segment, currentPage + i - currint);
                        }
                    }

                    output.Append(" ");
                }
                if (currentPage < totalPages)
                {
                    output.AppendFormat("<a href='{0}page={1}{2}' rel='next' class='next_page'>다음 페이지</a>", redirectTo, currentPage + 1, segment);
                }

                output.Append(" ");
                if (currentPage != totalPages)
                {
                    output.AppendFormat("<a href='{0}page={1}' class='last_page'>마지막 페이지</a>", redirectTo, totalPages);
                }
                output.Append(" ");
            }
            //output.AppendFormat("<label>第{0}页 / 共{1}页</label>", currentPage, totalPages);

            output.Append("</div>");
            return new HtmlString(output.ToString());
        }
    }
}

