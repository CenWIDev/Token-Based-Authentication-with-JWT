using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TokenAuth.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "Register",
                routeTemplate: "api/auth/register",
                defaults: new { controller = "Auth", action = "Register" }
            );

            config.Routes.MapHttpRoute(
                name: "Login",
                routeTemplate: "api/auth/login",
                defaults: new { controller = "Auth", action = "Login" }
            );

            config.Routes.MapHttpRoute(
                name: "Facebook",
                routeTemplate: "api/auth/facebook",
                defaults: new { controller = "Auth", action = "Facebook" }
            );

            config.Routes.MapHttpRoute(
                name: "GetUser",
                routeTemplate: "api/user",
                defaults: new { controller = "User", action = "GetUser" }
            );
        }
    }
}
