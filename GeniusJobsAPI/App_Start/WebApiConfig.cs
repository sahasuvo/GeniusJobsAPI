using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Http.Cors;

namespace GeniusJobsAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.EnableCors();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               //routeTemplate: "api/{controller}/{id}",
               routeTemplate: "api/{controller}/{action}/{ID}",
               defaults: new { id = RouteParameter.Optional }
           //constraints: new { httpmethod = new HttpMethodConstraint("GET") }
           );
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi_POST",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional, action = "POST" },
            //    constraints: new { httpmethod = new HttpMethodConstraint("POST") }
            //);
        }
    }
}
