using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(SignalRChat.StartSignalR))]
namespace SignalRChat
{
    public class StartSignalR
    {
        public void Configuration(IAppBuilder app)
        {
            var idProvider = new CustomUserIdProvider();
            //GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => "test");
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);          

            app.MapSignalR();
        }
    }

    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            // your logic to fetch a user identifier goes here.

            // for example:

            var userId = request.User.Identity.Name;
            return userId.ToString();
        }
    }
}
