using GeniusMAppsAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeniusMAppsAPI.Controllers
{
    public class JsQueriesController : ApiController
    {
        JobServicesController _conn = new JobServicesController();

        [HttpGet]
        public List<JSQueries> Get([FromUri]string jsonData)
        {
            JSQueries _jsonData = JsonConvert.DeserializeObject<JSQueries>(jsonData);
           
            object[] values = new object[3];
            values[0] = _jsonData.RMSUserID;
            values[1] = _jsonData.JsQuery;
            values[2] = Convert.ToInt32(DBOperation.ViewAll);

            List<JSQueries> FeedbackList = new List<JSQueries>();
            JSQueries _feedback = new JSQueries();
            DataSet ds = new DataSet();

            int retValue = _conn.ExecutionMethod("ProcQuery_M", ref values, DBOperation.ViewAll, ref ds);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                {
                    _feedback = new JSQueries();
                    _feedback.JsQuery = Convert.ToString(ds.Tables[0].Rows[r]["Query"]);
                    _feedback.SubmittedOn = Convert.ToString(ds.Tables[0].Rows[r]["SubmitedOn"]);                   
                    FeedbackList.Add(_feedback);
                }
            }
            else
            {
                _feedback.JsQuery = "-1";
                _feedback.SubmittedOn = "";
                FeedbackList.Add(_feedback);
            }
            return FeedbackList;
        }

        [HttpPost]
        public int POST([FromBody]JObject jsonData)
        {
            // Convert the dynamic JObject to a DocumentDto object.
            JSQueries _JSQueries = jsonData.ToObject<JSQueries>();

            object[] values = new object[3];
            values[0] = _JSQueries.RMSUserID;
            values[1] = _JSQueries.JsQuery;
            values[2] = Convert.ToInt32(DBOperation.Insert);
        
            DataSet ds = new DataSet();
            int retValue = _conn.ExecutionMethod("ProcQuery_M", ref values, DBOperation.ViewAll, ref ds);
            return retValue;
        }
    }
}
