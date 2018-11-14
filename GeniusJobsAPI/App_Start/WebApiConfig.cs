using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Http.Cors;
using GeniusJobsAPI.App_Start;

namespace GeniusJobsAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ///Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.EnableCors();
            config.MessageHandlers.Add(new CrossDomainHandler());

            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               //routeTemplate: "api/{controller}/{id}",
               routeTemplate: "api/{controller}/{action}/{ID}",
               defaults: new { id = RouteParameter.Optional }
           );

            // //var json = config.Formatters.JsonFormatter;
            // //json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            // //config.Formatters.Remove(config.Formatters.XmlFormatter);


            // //config.Routes.MapHttpRoute(
            // //    name: "DefaultApi_POST",
            // //    routeTemplate: "api/{controller}/{id}",
            // //    defaults: new { id = RouteParameter.Optional, action = "POST" },
            // //    constraints: new { httpmethod = new HttpMethodConstraint("POST") }
            // //);




            /////*From Shibuda **/
            ////// Web API configuration and services

            ////// Web API routes
            ////config.MapHttpAttributeRoutes();
            ////var cors = new EnableCorsAttribute("*", "*", "*");
            ////config.EnableCors(cors);
            ////config.MessageHandlers.Add(new CrossDomainHandler());

            ////config.Routes.MapHttpRoute(
            ////    name: "DefaultApi",
            ////    routeTemplate: "api/{controller}/{id}",
            ////    defaults: new { id = RouteParameter.Optional }
            ////);

            ////var json = config.Formatters.JsonFormatter;
            ////json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            ////config.Formatters.Remove(config.Formatters.XmlFormatter);



        }
    }
}
