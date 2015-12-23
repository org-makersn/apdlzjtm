using System.Web;
using System.Web.Optimization;

namespace Design.Web.Front
{
    public class BundleConfig
    {
        // Bundling에 대한 자세한 정보는 http://go.microsoft.com/fwlink/?LinkId=254725를 방문하십시오.
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js")); 

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                        "~/Scripts/common.js",
                        "~/Scripts/AjaxModel.js",
                        "~/Scripts/Custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-ng-grid.js",
                        "~/Scripts/angular-resource.js",
                        "~/Scripts/angular-route.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                        "~/Scripts/app/app.js",
                        "~/Scripts/app/services.js",
                        "~/Scripts/app/directives.js",
                        "~/Scripts/app/main.js",
                        "~/Scripts/app/info.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/viewer").Include(
                        //"~/Scripts/three.js",
                        "~/Scripts/Thingiview2.js",
                        "~/Scripts/NormalControls.js",
                        "~/Scripts/OBJLoader.js",
                        "~/Scripts/STLLoader.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/prettyprint").Include(
                        "~/Scripts/google-code-prettify/prettify.js"));

            // "~/Scripts/jquery.bxslider.min.js"Modernizr의 개발 버전을 사용하여 개발하고 배우십시오. 그런 다음
            // 프로덕션할 준비가 되면 http://modernizr.com의 빌드 도구를 사용하여 필요한 테스트만 선택하십시오.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new StyleBundle("~/Content/style").Include(
            //            "~/Content/css/common.css",
            //            "~/Content/css/default.css",
            //            "~/Content/css/font.css",
            //            "~/Content/css/layout.css",
            //            "~/Content/css/PagedList.css"));
        }
    }
}