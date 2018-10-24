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
using System.Reflection;
using System.Dynamic;

namespace GeniusJobsAPI.Controllers
{
    /// <Types of HTTP Headers>
    ////[HttpDelete]
    ////[HttpGet]
    ////[HttpHead]
    ////[HttpOptions]
    ////[HttpPatch]
    ////[HttpPost]
    ////[HttpPut]
    /// </Types of HTTP Headers>

    public enum ParamType
    {
        Country = 1, // Country
        Location = 2, // Location/City
        Qualification = 3,
        JobCategory = 4,
        JobType = 5,
        QualificationType = 6
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ParamController : ApiController
    {

        [HttpGet]
        [ActionName("GetCountry")]
        public HttpResponseMessage GetAllCountry()
        {
            //String strCountry = GetParamDetails(ParamType.Country, String.Empty);
            List<dynamic> lst = GetParamDetails(ParamType.Country, String.Empty);

            ResponseClass objresponse = new ResponseClass()
            {
                ResponseCode = lst.Count > 0 ? 001 : -101,
                ResponseData = lst,
                ResponseStatus = lst.Count > 0 ? "Success" : "Failed"
            };

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);
            response.StatusCode = lst.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return response;
        }

        [HttpGet]
        [ActionName("GetLocation")]

        public HttpResponseMessage GetAllLocation()
        {
            //[FromUri] String strParam
            //strParam = strParam.Length > 0 ? strParam : "CONGSP0510000001";
            List<dynamic> lstLocation = GetParamDetails(ParamType.Location, "CONGSP0510000001");

            ResponseClass objresponse = new ResponseClass()
            {
                ResponseCode = lstLocation.Count > 0 ? 001 : -101,
                ResponseData = lstLocation,
                ResponseStatus = lstLocation.Count > 0 ? "Success" : "Failed"
            };

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);
            response.StatusCode = lstLocation.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;

            //response.RequestMessage = strLoc.Length > 0 ?  "Success" : "Failed";
            return response;
        }


        [HttpGet]
        [ActionName("GetQualifyType")]

        public HttpResponseMessage GetAllQualiType()
        {
            List<dynamic> lstQualifyType = GetParamDetails(ParamType.QualificationType, String.Empty);

            ResponseClass objresponse = new ResponseClass()
            {
                ResponseCode = lstQualifyType.Count > 0 ? 001 : -101,
                ResponseData = lstQualifyType,
                ResponseStatus = lstQualifyType.Count > 0 ? "Success" : "Failed"
            };

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);
            response.StatusCode = lstQualifyType.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return response;
        }

        [HttpGet]
        [ActionName("GetQualification")]

        public HttpResponseMessage GetAllQualification()
        {
            List<dynamic> lstQualify = GetParamDetails(ParamType.Qualification, String.Empty);

            ResponseClass objresponse = new ResponseClass()
            {
                ResponseCode = lstQualify.Count > 0 ? 001 : -101,
                ResponseData = lstQualify,
                ResponseStatus = lstQualify.Count > 0 ? "Success" : "Failed"
            };

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);
            response.StatusCode = lstQualify.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return response;
        }

        [HttpGet]
        [ActionName("GetJobCategory")]
        public HttpResponseMessage GetAllJobCategory()
        {
            List<dynamic> lstJobCat = GetParamDetails(ParamType.JobCategory, String.Empty);

            ResponseClass objresponse = new ResponseClass()
            {
                ResponseCode = lstJobCat.Count > 0 ? 001 : -101,
                ResponseData = lstJobCat,
                ResponseStatus = lstJobCat.Count > 0 ? "Success" : "Failed"
            };

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);
            response.StatusCode = lstJobCat.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return response;
        }

        [HttpGet]
        [ActionName("GetJobType")]
        public HttpResponseMessage GetAllJobType()
        {
            List<dynamic> lstJobtype = GetParamDetails(ParamType.JobType, String.Empty);

            ResponseClass objresponse = new ResponseClass()
            {
                ResponseCode = lstJobtype.Count > 0 ? 001 : -101,
                ResponseData = lstJobtype,
                ResponseStatus = lstJobtype.Count > 0 ? "Success" : "Failed"
            };

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);
            response.StatusCode = lstJobtype.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
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

        public List<dynamic> GetParamDetails(ParamType paramtype, string ID)
        {
            int? ReturnStatus = 0;
            System.Data.DataTable dtd = new System.Data.DataTable();
            DatabaseTransaction objDB = new DatabaseTransaction();
            objDB.AddConnectionName = "RMSRemote";

            List<KeyValuePair<object, object>> lst = new List<KeyValuePair<object, object>>();
            lst.Add(new KeyValuePair<object, object>("@Type", Convert.ToInt32(paramtype) ));
            lst.Add(new KeyValuePair<object, object>("@ID", ID));
            dtd = objDB.SqlGetData("JobSearchMobileApp_New", ref lst, ExecType.Dynamic, ReturnDBOperation.DataTable, ref ReturnStatus);


            var dtlist = new List<dynamic>();
            if (dtd != null && dtd.Rows.Count > 0)
            {
                dtlist = DatatableToList(dtd);
            }
            return dtlist;
        }

        private List<dynamic> DatatableToList(DataTable dt)
        {
            //var dt = new DataTable();

            var dns = new List<dynamic>();

            foreach (var item in dt.AsEnumerable())
            {
                // Expando objects are IDictionary<string, object>
                IDictionary<string, object> dn = new ExpandoObject();

                foreach (var column in dt.Columns.Cast<DataColumn>())
                {
                    dn[column.ColumnName] = item[column];
                }

                dns.Add(dn);
            }

            return dns;
        }

    }
}
