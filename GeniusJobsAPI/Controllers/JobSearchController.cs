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
    public class JobSearchController : ApiController
    {
        JobServicesController _conn = new JobServicesController();

        [ActionName("GetJobsBySearch")]
        [HttpGet]
        public JsJobSearch Get()
        {
            JsJobSearch _JsJobSearch = new JsJobSearch();

            object[] values = new object[1];
            DataSet ds = new DataSet();
            int retValue = _conn.ExecutionMethod("JobSearchMobile", ref values, DBOperation.ViewAll, ref ds);

            /* COUNTRY LIST */
            List<CountryMaster> countryList = new List<CountryMaster>();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    countryList.Add(new CountryMaster { CountryID = Convert.ToString(ds.Tables[0].Rows[i]["CountryID"]), CountryName = Convert.ToString(ds.Tables[0].Rows[i]["CountryName"]) });
                }
            }
            else
                countryList.Add(new CountryMaster { CountryID = "0", CountryName = "Select" });


            /* LOCATION LIST */
            List<LocationMaster> locationList = new List<LocationMaster>();
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    locationList.Add(new LocationMaster
                    {
                        CityID = Convert.ToString(ds.Tables[1].Rows[i]["CityID"]),
                        CityName = Convert.ToString(ds.Tables[1].Rows[i]["CityName"]),
                        StateID = Convert.ToString(ds.Tables[1].Rows[i]["StateID"]),
                        CountryID = Convert.ToString(ds.Tables[1].Rows[i]["CountryID"])
                    });
                }
            }
            else
            {
                locationList.Add(new LocationMaster
                {
                    CityID = "0",
                    CityName = "Select",
                    StateID = "0",
                    CountryID = "0"
                });
            }

            /* QUALIFICATION LIST */
            List<QualificationMaster> qualificationList = new List<QualificationMaster>();
            if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    qualificationList.Add(new QualificationMaster
                    {
                        RMSQualificationID = Convert.ToString(ds.Tables[2].Rows[i]["RMSQualificationID"]),
                        RMSQualificationName = Convert.ToString(ds.Tables[2].Rows[i]["RMSQualificationName"]),
                    });
                }
            }
            else
            {
                qualificationList.Add(new QualificationMaster
                {
                    RMSQualificationID = "0",
                    RMSQualificationName = "Select"
                });
            }

            /* JOB CATEGORY LIST */
            List<FunctionMaster> jobCategoryList = new List<FunctionMaster>();
            if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    jobCategoryList.Add(new FunctionMaster
                    {
                        RMSSAFunctionID = Convert.ToString(ds.Tables[3].Rows[i]["RMSSAFunctionID"]),
                        RMSSAFunctionName = Convert.ToString(ds.Tables[3].Rows[i]["RMSSAFunctionName"]),
                    });
                }
            }
            else
            {
                jobCategoryList.Add(new FunctionMaster
                {
                    RMSSAFunctionID = "0",
                    RMSSAFunctionName = "Select"
                });
            }

            /* JOB TYPE LIST */
            List<JobType> jobTypeList = new List<JobType>();
            if (ds.Tables.Count > 4 && ds.Tables[4].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    jobTypeList.Add(new JobType
                    {
                        JOBTYPE = Convert.ToString(ds.Tables[4].Rows[i]["JOBTYPE"])
                    });
                }
            }
            else
            {
                jobTypeList.Add(new JobType
                {
                    JOBTYPE = "Select"
                });
            }

            _JsJobSearch._CountryMaster = countryList;
            _JsJobSearch._LocationMaster = locationList;
            _JsJobSearch._QualificationMaster = qualificationList;
            _JsJobSearch._FunctionMaster = jobCategoryList;
            _JsJobSearch._JobType = jobTypeList;

            return _JsJobSearch;
        }
    }
}
