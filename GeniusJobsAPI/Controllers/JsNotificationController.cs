using GeniusMAppsAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace GeniusMAppsAPI.Controllers
{
    public class JsNotificationController : ApiController
    {
        JobServicesController _conn = new JobServicesController();

        [HttpGet]
        //public List<Notification> Get([FromUri]string jsonData)
        public List<Notification> Get()
        {           
            object[] values = new object[1];
            values[0] = "M";

            List<Notification> NotifList = new List<Notification>();
            Notification notif = new Notification();
            DataSet ds = new DataSet();

            int retValue = _conn.ExecutionMethod("RMSJSNotification_M", ref values, DBOperation.ViewAll, ref ds);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                {
                    notif = new Notification();
                    notif.SlNo = (r + 1);
                    notif.TotRow = Convert.ToInt32(ds.Tables[0].Rows.Count.ToString());
                    notif.AEMClientID = ds.Tables[0].Rows[r]["AEMClientID"].ToString();
                    notif.StartDate = ds.Tables[0].Rows[r]["StartDate"].ToString();
                    notif.EndDate = (ds.Tables[0].Rows[r]["EndDate"].ToString());
                    notif.Tagline = ds.Tables[0].Rows[r]["Tagline"].ToString();
                    notif.Description = ds.Tables[0].Rows[r]["Description"].ToString();
                    NotifList.Add(notif);
                }
            }
            else
            {
                notif.SlNo = 0;
                notif.TotRow = 0;
                notif.AEMClientID = "";
                notif.StartDate = "";
                notif.EndDate = "";
                notif.Tagline = "";
                notif.Description = "";
                NotifList.Add(notif);
            }
            return NotifList;
        }
    }
}
