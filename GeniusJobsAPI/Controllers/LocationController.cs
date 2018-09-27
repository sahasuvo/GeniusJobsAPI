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

namespace GeniusJobsAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LocationController : ApiController
    {
        [HttpGet]
        [ActionName("GetLocation")]

        public HttpResponseMessage GetAllLocation()
        {
            GetLocation();

            //var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            //HttpResponseMessage response = new HttpResponseMessage();
            //response.Content = new ObjectContent(jsonsrlz.GetType(), jsonsrlz, jsonformat);

            HttpRequestMessage req = new HttpRequestMessage();
            //string str = req.RequestUri.AbsoluteUri;
            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            //response.Content = new ObjectContent<List<Products>>(ProductOps.GetAllProducts(), jsonformat);
            return response;
        }
        [HttpGet]
        [ActionName("GetLocationByID")]
        public HttpResponseMessage GetProduct(String ID)
        {   
            //var prod = ProductOps.GetAllProducts().FirstOrDefault(p => p.ID.Equals(ID));
            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            
            //response.Content = new ObjectContent(prod.GetType(), prod, jsonformat);
            return response;
        }

        private static List<Location> lstLocation;

        public static String GetLocation()
        {
            System.Data.DataTable dtd = new System.Data.DataTable();
            DatabaseTransaction objDB = new DatabaseTransaction();
            objDB.AddConnectionName = "RMSRemote"; //ConfigurationManager.ConnectionStrings[""].ConnectionString;
            object[] param = new object[1];
            param[0] = "CONGSP0510000001";

            List<KeyValuePair<object,object>> lst = new List<KeyValuePair<object, object>>();
            lst.Add(new KeyValuePair<object, object>("", ""));

            dtd = objDB.SqlGetData("RMSSelectSACITYMASTERwithState", lst, param, ExecType.Dynamic, ReturnDBOperation.DataTable);

            JavaScriptSerializer jsontest = new JavaScriptSerializer();
            var jsonsrlz = "";
            if (dtd!=null && dtd.Rows.Count>0)
            {
                jsonsrlz = JsonConvert.SerializeObject(dtd);
            }

            //var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            //HttpResponseMessage response = new HttpResponseMessage();
            //response.Content = new ObjectContent(jsonsrlz.GetType(), jsonsrlz, jsonformat);


            return jsonsrlz;

            //return objNewData;
        }

    }
}
