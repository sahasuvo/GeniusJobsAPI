using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using DatabaseAccessLayer;
using GeniusJobsAPI.Class;


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
        QualificationType = 6,
        Experience = 7
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ParamController : ApiController
    {

        [HttpGet]
        [ActionName("GetCountry")]
        //[Route("api/Param/GetCountry")]
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
        [ActionName("GetCountryBySearch")]
        //[Route("api/Param/GetCountryBySearch")]
        public HttpResponseMessage GetCountryBySearch([FromUri]string SearchName, [FromUri]String SearchVal)
        {

            List<dynamic> lstCountrySearch = GetParamDetailsBySearch(ParamType.Country, string.Empty, SearchName, SearchVal);

            ResponseClass objresponse = new ResponseClass()
            {
                ResponseCode = lstCountrySearch.Count > 0 ? 001 : -101,
                ResponseData = lstCountrySearch,
                ResponseStatus = lstCountrySearch.Count > 0 ? "Success" : "Failed"
            };

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);
            response.StatusCode = lstCountrySearch.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return response;
        }

        [HttpGet]
        [ActionName("GetLocation")]
        //[Route("api/Param/GetLocation")]
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
        [ActionName("GetLocationBySearch")]
        //[Route("api/Param/GetLocationBySearch")]
        public HttpResponseMessage GetLocationBySearch([FromUri]string SearchName, [FromUri]String SearchVal)
        {
            List<dynamic> lstLocSearch = GetParamDetailsBySearch(ParamType.Location, "CONGSP0510000001", SearchName, SearchVal);

            #region CodeReasearch on Dynamic Keyword based List

            ////+       [0] Count = 4   object { System.Collections.Generic.Dictionary<string, object>}

            //List<Dictionary<string, dynamic>> result = lstLocSearch.ConvertAll(new Converter<dynamic, Dictionary<string, dynamic>>(DynamicToDictionaryConverter));


            //List<Dictionary<string, dynamic>> result1 = new List<Dictionary<string, dynamic>>();

            //Dictionary<string, dynamic> dd1 = new Dictionary<string, dynamic>();


            //var LocSearch = lstLocSearch.FindAll(p =>
            //{
            //    var lst11 = p as Dictionary<string, dynamic>;
            //    return lst11.Keys.Any(x => x[x.IndexOf(searchName)].ToString().Contains(SearchVal));
            //}
            //);

            //PropertyInfo[] props = result.GetType().GetProperties();
            //foreach (Dictionary<string, dynamic> dd in lstLocSearch)
            //{
            //    if(dd.Keys.ToString().ToUpper().Contains(searchName.ToUpper()) && dd.Values.ToString().ToUpper().Contains(SearchVal.ToUpper()))
            //    {
            //        result1.Add(dd);
            //    }
            //}
            //foreach (var item in lstLocSearch.AsEnumerable())
            //{
            //    IDictionary<string, object> dn = new ExpandoObject();
            //    item
            //}

            //((System.Collections.Generic.IDictionary<string, object>)new System.Collections.Generic.ICollection<object>(lstLocSearch);
            //dynamic[] arr;
            //IDictionary<object, object> objaa = new ExpandoObject();
            //objaa = (IDictionary<string, object>)lstLocSearch;

            //Enumerable.ToList<dynamic>((lstLocSearch)).IndexOf(new ExpandoObject().;

            //Dictionary<string, object> dd = (new System.Collections.Generic.Dictionary<string, object>())(lstLocSearch);

            //for (int i = 0; i < lstLocSearch.Count; i++)
            //{
            //    objaa.Add(lstLocSearch[i].Keys, lstLocSearch[i].Values);
            //}

            //foreach(ExpandoObject eo in lstLocSearch)
            //{
            //    objaa.Add(eo. lstLocSearch[i].Values);
            //}

            //Dictionary<dynamic, dynamic> result = new Dictionary<dynamic, dynamic>();
            //if (objaa.Keys.Contains(searchName) && objaa.Values.Contains(SearchVal))
            //{
            //    result.Add(objaa.Keys,objaa.Values);
            //}

            //foreach (IDictionary<string, object> eo in lstLocSearch.AsEnumerable())
            //{
            //    //dn[column.ColumnName] = item[column];
            //    //objaa.Add(eo);
            //    //eo.
            //}

            //lstLocSearch.CopyTo(arr);


            //dynamic[] lstarr = lstLocSearch.ToArray();


            //foreach (dynamic value in lstLocSearch)
            //{
            //    if (lstLocSearch.Contains())
            //    {
            //        exampleDictionary.Add(value, 1);
            //    }
            //}


            //result = result.FindAll(p => p.Keys.Any(x => x[x.IndexOf(searchName)].ToString().Contains(SearchVal))).ToList();
            //result = result.Find(p=>p.Keys.Equals(searchName)).Values.Equals(SearchVal)

            //var aaa = from p in result where p.Keys.Contains(searchName) && p.Values.Contains(SearchVal) select p;
            //var aaa = from p in lstLocSearch where p.
            //List < dynamic > lst1 = (List<dynamic>)aaa;


            //var result = (from c in lstLocSearch where c.LastName.StartsWith("Mc") select c).ToList();

            //List<dynamic> lstLocSearc1 = dn.Keys.Select(x => x[x.IndexOf(searchName)].ToString().Contains(SearchVal)).ToList();
            //var LocSearch = lstLocSearch.FindAll(p =>
            //{
            //    var lst11 = p as IDictionary<string, object>;
            //    return lst11.Keys.Any(x => x[x.IndexOf(searchName)].ToString().Contains(SearchVal));
            //}
            //);
            #endregion CodeReasearch on Dynamic Keyword based List
            
            ResponseClass objresponse = new ResponseClass()
            {
                ResponseCode = lstLocSearch.Count > 0 ? 001 : -101,
                ResponseData = lstLocSearch,//Enumerable.ToList<dynamic>(result.AsEnumerable),
                ResponseStatus = lstLocSearch.Count > 0 ? "Success" : "Failed"
            };

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);
            response.StatusCode = lstLocSearch.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;

            return response;
        }


        [HttpGet]
        [ActionName("GetQualifyType")]
        //[Route("api/Param/GetQualifyType")]
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
        [ActionName("GetQualifyTypeBySearch")]
        //[Route("api/Param/GetQualifyTypeBySearch")]
        public HttpResponseMessage GetAllQualiTypeBySearch([FromUri]string SearchName, [FromUri]String SearchVal)
        {
            List<dynamic> lstQualifyTypeSearch = GetParamDetailsBySearch(ParamType.QualificationType, string.Empty, SearchName, SearchVal);

            ResponseClass objresponse = new ResponseClass()
            {
                ResponseCode = lstQualifyTypeSearch.Count > 0 ? 001 : -101,
                ResponseData = lstQualifyTypeSearch,
                ResponseStatus = lstQualifyTypeSearch.Count > 0 ? "Success" : "Failed"
            };

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);
            response.StatusCode = lstQualifyTypeSearch.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return response;
        }

        [HttpGet]
        [ActionName("GetQualification")]
        //[Route("api/Param/GetQualification")]
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
        [ActionName("GetQualificationBySearch")]
        //[Route("api/Param/GetQualificationBySearch")]
        public HttpResponseMessage GetQualificationBySearch([FromUri]string SearchName, [FromUri]String SearchVal)
        {
            List<dynamic> lstQualify = GetParamDetailsBySearch(ParamType.Qualification, string.Empty, SearchName, SearchVal);

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
        //[Route("api/Param/GetJobCategory")]
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
        [ActionName("GetJobCategoryBySearch")]
        //[Route("api/Param/GetJobCategoryBySearch")]
        public HttpResponseMessage GetJobCategoryBySearch([FromUri]string SearchName, [FromUri]String SearchVal)
        {
            List<dynamic> lstJobCat = GetParamDetailsBySearch(ParamType.JobCategory, String.Empty, SearchName, SearchVal);

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
        //[Route("api/Param/GetJobType")]
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

        [HttpGet]
        [ActionName("GetExperience")]
        //[Route("api/Param/GetExperience")]
        public HttpResponseMessage GetAllExperience()
        {
            List<dynamic> lstJobExp = GetParamDetails(ParamType.Experience, String.Empty);

            ResponseClass objresponse = new ResponseClass()
            {
                ResponseCode = lstJobExp.Count > 0 ? 001 : -101,
                ResponseData = lstJobExp,
                ResponseStatus = lstJobExp.Count > 0 ? "Success" : "Failed"
            };

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);
            response.StatusCode = lstJobExp.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return response;
        }

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
                dtlist = new CommonMethods().DatatableToList(dtd);
            }
            return dtlist;
        }

        public List<dynamic> GetParamDetailsBySearch(ParamType paramtype, string ID,String SearchParam, String SearchValue)
        {
            int? ReturnStatus = 0;
            System.Data.DataTable dtd = new System.Data.DataTable();
            DatabaseTransaction objDB = new DatabaseTransaction();
            objDB.AddConnectionName = "RMSRemote";

            List<KeyValuePair<object, object>> lst = new List<KeyValuePair<object, object>>();
            lst.Add(new KeyValuePair<object, object>("@Type", Convert.ToInt32(paramtype)));
            lst.Add(new KeyValuePair<object, object>("@ID", ID));
            lst.Add(new KeyValuePair<object, object>("@SearchParam", SearchParam));
            lst.Add(new KeyValuePair<object, object>("@SearchVal", SearchValue));
            dtd = objDB.SqlGetData("JobSearchMobileAppbySearch", ref lst, ExecType.Dynamic, ReturnDBOperation.DataTable, ref ReturnStatus);


            var dtlist = new List<dynamic>();
            if (dtd != null && dtd.Rows.Count > 0)
            {
                dtlist = new CommonMethods().DatatableToList(dtd);
            }
            return dtlist;
        }

        //private List<dynamic> DatatableToList(DataTable dt)
        //{
        //    //var dt = new DataTable();

        //    var dns = new List<dynamic>();

        //    foreach (var item in dt.AsEnumerable())
        //    {
        //        // Expando objects are IDictionary<string, object>
        //        IDictionary<string, object> dn = new Dictionary<string, object>();//new ExpandoObject();

        //        foreach (var column in dt.Columns.Cast<DataColumn>())
        //        {
        //            dn[column.ColumnName] = item[column];
        //        }

        //        dns.Add(dn);
        //    }

        //    return dns;
        //}


        //private List<dynamic> DictionaryToList(Dictionary<dynamic,dynamic> dt)
        //{
        //    //var dt = new DataTable();

        //    var dns = new List<dynamic>();

        //    foreach (var item in dt.AsEnumerable())
        //    {
        //        // Expando objects are IDictionary<string, object>
        //        IDictionary<string, object> dn = new ExpandoObject();

        //        foreach (var column in dt.Keys.Cast<Da>())
        //        {
        //            dn[column.ColumnName] = item[column];
        //        }

        //        dns.Add(dn);
        //    }

        //    return dns;
        //}

        //public static Dictionary<string, dynamic> DynamicToDictionaryConverter(dynamic item)
        //{
        //    if (IsDictionary(item))
        //    {
        //        return (Dictionary<string, dynamic>)item;
        //    }

        //    Dictionary<string, dynamic> newItem = new Dictionary<string, dynamic>();
        //    PropertyInfo[] props = item.GetType().GetProperties();
        //    foreach (PropertyInfo prop in props)
        //    {
        //        newItem[prop.Name] = item.GetType().GetProperty(prop.Name).GetValue(item, null);
        //    }
        //    return newItem;
        //}

        //public static bool IsDictionary(object o)
        //{
        //    if (o == null) return false;
        //    return o is IDictionary &&
        //           o.GetType().IsGenericType &&
        //           o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        //}

    }
}
