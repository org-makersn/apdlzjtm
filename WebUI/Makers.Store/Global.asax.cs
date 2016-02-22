using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Makers.Store
{
    // 참고: IIS6 또는 IIS7 클래식 모드를 사용하도록 설정하는 지침을 보려면 
    // http://go.microsoft.com/?LinkId=9394801을 방문하십시오.

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //BundleTable.EnableOptimizations = true;

            AuthConfig.RegisterAuth();
            RemoveWebFormEngines();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //foreach (var item in this.Context.Items.Values)
            //{
            //    var disposableObj = item as IDisposable;
            //    if (disposableObj != null) disposableObj.Dispose();
            //}
        }

        protected void RemoveWebFormEngines()
        {
            var viewEngines = ViewEngines.Engines;
            var webFormEngines = viewEngines.OfType<WebFormViewEngine>().FirstOrDefault();
            if (webFormEngines != null)
            {
                viewEngines.Remove(webFormEngines);
            }
        }
    }
}