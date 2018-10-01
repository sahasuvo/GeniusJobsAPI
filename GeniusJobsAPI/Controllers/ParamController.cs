using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GeniusJobsAPI.Models;
using System.Web.Script.Serialization;
using System.Data;
using System.Web;
using System.Web.Http.Cors;
using DatabaseAccessLayer;
using System.Configuration;
using Newtonsoft.Json;
using System.Data.SqlClient;
using GeniusJobsAPI.Class;

namespace GeniusJobsAPI.Controllers
{
    public enum ParamType
    {
        Country = 1, // Country
        Location = 2, // Location/City
        Qualification = 3,
        JobCategory = 4,
        JobType = 5
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ParamController : ApiController
    {
        [HttpGet]
        [ActionName("GetLocation")]

        public HttpResponseMessage GetAllLocation()
        {
            String strLoc = GetParamDetails(ParamType.Location, "CONGSP0510000001");

            ResponseClass objresponse = new ResponseClass()
            {
                ResponseCode = strLoc.Length > 0 ? 001 : -101,
                ResponseData = strLoc,
                ResponseStatus = strLoc.Length > 0 ? "Success" : "Failed"
            };

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);
            response.StatusCode = strLoc.Length > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            
            //response.RequestMessage = strLoc.Length > 0 ?  "Success" : "Failed";
            return response;
        }

        [HttpGet]
        [ActionName("GetCountry")]

        public HttpResponseMessage GetAllCountry()
        {
            String strCountry = GetParamDetails(ParamType.Country, String.Empty);

            //ResponseClass objresponse = new ResponseClass()
            //{
            //    ResponseCode = strLoc.Length > 0 ? 001 : -101,
            //    ResponseData = strLoc,
            //    ResponseStatus = strLoc.Length > 0 ? "Success" : "Failed"
            //};

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(strCountry.GetType(), strCountry, jsonformat);
            response.StatusCode = strCountry.Length > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return response;
        }

        [HttpGet]
        [ActionName("GetQualification")]

        public HttpResponseMessage GetAllQualification()
        {
            String strQualify = GetParamDetails(ParamType.Qualification, String.Empty);

            //ResponseClass objresponse = new ResponseClass()
            //{
            //    ResponseCode = strLoc.Length > 0 ? 001 : -101,
            //    ResponseData = strLoc,
            //    ResponseStatus = strLoc.Length > 0 ? "Success" : "Failed"
            //};

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(strQualify.GetType(), strQualify, jsonformat);
            response.StatusCode = strQualify.Length > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return response;
        }

        //[HttpGet]
        //[ActionName("GetLocationByID")]
        //public HttpResponseMessage GetProduct(String ID)
        //{   
        //    //var prod = ProductOps.GetAllProducts().FirstOrDefault(p => p.ID.Equals(ID));
        //    var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
        //    HttpResponseMessage response = new HttpResponseMessage();

        //    //response.Content = new ObjectContent(prod.GetType(), prod, jsonformat);
        //    return response;
        //}


        private static List<Location> lstLocation;

        public static String GetParamDetails(ParamType paramtype,string ID)
        {
            int? ReturnStatus = 0;
            System.Data.DataTable dtd = new System.Data.DataTable();
            DatabaseTransaction objDB = new DatabaseTransaction();
            objDB.AddConnectionName = "RMSRemote";

            List<KeyValuePair<object, object>> lst = new List<KeyValuePair<object, object>>();
            lst.Add(new KeyValuePair<object, object>("@Type", Convert.ToInt32(paramtype) ));
            lst.Add(new KeyValuePair<object, object>("@ID", ID));

            dtd = objDB.SqlGetData("JobSearchMobileApp_New", ref lst, ExecType.Dynamic, ReturnDBOperation.DataTable, ref ReturnStatus);

            JavaScriptSerializer jsontest = new JavaScriptSerializer();
            var jsonsrlz = "";
            if (dtd != null && dtd.Rows.Count > 0)
            {
                jsonsrlz = JsonConvert.SerializeObject(dtd);
            }
            return jsonsrlz;
        }
        public static String GetLocation()
        {
            int? ReturnStatus = 0;
            System.Data.DataTable dtd = new System.Data.DataTable();
            DatabaseTransaction objDB = new DatabaseTransaction();
            objDB.AddConnectionName = "RMSRemote";

            List<KeyValuePair<object,object>> lst = new List<KeyValuePair<object, object>>();
            lst.Add(new KeyValuePair<object, object>("@CountryID", "CONGSP0510000001"));

            dtd = objDB.SqlGetData("RMSSelectSACITYMASTERwithState", ref lst, ExecType.Dynamic, ReturnDBOperation.DataTable, ref ReturnStatus);

            JavaScriptSerializer jsontest = new JavaScriptSerializer();
            var jsonsrlz = "";
            if (dtd!=null && dtd.Rows.Count>0)
            {
                jsonsrlz = JsonConvert.SerializeObject(dtd);
            }
            return jsonsrlz;
        }

    }
}
