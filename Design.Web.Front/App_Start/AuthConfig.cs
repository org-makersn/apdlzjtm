using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;

namespace Design.Web.Front
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // 이 사이트의 사용자가 다른 사이트(예: Microsoft, Facebook 및 Twitter)의 계정을 사용하여 로그인할 수 있도록 하려면
            // 이 사이트를 업데이트해야 합니다. 자세한 내용은 http://go.microsoft.com/fwlink/?LinkID=252166을 참조하십시오.

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "904738086253291",
            //    appSecret: "4373ba93bfe8e2c867b2f79610a54026");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "426712864183755",
                appSecret: "b7061e193583148eb518b313d78b9983");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
