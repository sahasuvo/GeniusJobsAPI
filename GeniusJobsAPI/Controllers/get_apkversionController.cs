using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GeniusJobsAPI.Class;

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

            ResponseClass objresponse = new ResponseClass()
            {
                ResponseCode = 001,
                ResponseData = Version,
                ResponseStatus = "Success"
            };

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}
