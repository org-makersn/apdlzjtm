using System;
using System.Web;
using System.Web.Mvc;

namespace Design.Web.Front
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            //인증 받지 못해 LogOn 으로 튕겨져 갈때 Hash 형태의 메뉴코드(#HMain등)를 처리하기 위해 오버라이드
            if (filterContext.Result is HttpUnauthorizedResult)
            {
                HttpRequestBase request = filterContext.HttpContext.Request;
                string returnUrl = request.Path;
                bool queryStringPresent = request.QueryString.Count > 0;
                if (queryStringPresent || request.Form.Count > 0)
                    returnUrl += '?' + request.QueryString.ToString();
                if (queryStringPresent)
                    returnUrl += '&';
                returnUrl += request.Form;

                String url = System.Web.Security.FormsAuthentication.LoginUrl + "?returnUrl=" + returnUrl;

                string sHtml = string.Format("<script type='text/javascript'>alert('로그인이 필요한 서비스 입니다.'); history.back();</script>", url);

                filterContext.Result = new ContentResult { Content = sHtml, ContentType = "text/html", ContentEncoding = System.Text.Encoding.UTF8 };
            }

            //if(filterContext.HttpContext.Response.StatusCode == 403)
            //{
            //    if(filterContext.HttpContext.User.Identity.IsAuthenticated)
            //        filterContext.Result = new RedirectResult("/AccessError");
            //    else
            //        filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl + "?returnUrl=" + filterContext.HttpContext.Request.UrlReferrer);
            //} 
        }
    }
}
