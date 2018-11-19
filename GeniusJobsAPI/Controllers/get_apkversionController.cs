using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IFBiOSApi.Controllers
{
    public class get_apkversionController : ApiController
    {
        //DataSet ds = new DataSet();
        //ResponseModel _objResponseModel = new ResponseModel();
        [HttpGet]
        [ActionName("GetVersion")]
        public HttpResponseMessage Get()
        {
            String Version = System.Configuration.ConfigurationManager.AppSettings["APKVersion"].ToString();
            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(Version.GetType(), Version, jsonformat);
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}
