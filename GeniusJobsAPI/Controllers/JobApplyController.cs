using GeniusMAppsAPI.Models;
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
    public class JobApplyController : ApiController
    {
        JobServicesController _conn = new JobServicesController();
        RMSNextController _RmsNextconn = new RMSNextController();
        TSSRController _TssrConn = new TSSRController();
        public List<JsJobList> jsJoblist = new List<JsJobList>();
        public DataSet ds = new DataSet();
        public DataTable dtJobList = new DataTable();

        [HttpGet]
        public int GET([FromUri]string jsonData)
        {
            int ret = 0;
            string[] stuff = jsonData.Split('|');

            List<JsJobList> _jsAppliedJobList = new List<JsJobList>();
            foreach (string s in stuff)
            {
                _jsAppliedJobList.Add(new JsJobList
                {
                    UserId = s.Split(',')[0],
                    ResumeId = s.Split(',')[1],
                    JobType = s.Split(',')[2],
                    JobCode = s.Split(',')[3],
                    SL = "",
                    JobAssignedOn = "",
                    JobTitle = "",
                    JobDescription = "",
                    JobKeySkills = "",
                    MinJobExperience = "",
                    MaxJobExperience = "",
                    AnnualJobCTC = "",
                    Locations = "",
                    Qualifications = "",
                    SRCTYPE = "",
                    TotalRows = "",
                    CurrentIndex = ""
                });
            }


            int retValue = 0;
            DataSet ds = new DataSet();
            object[] values = new object[4];

            // Convert the dynamic JObject to a DocumentDto object.
            foreach (JsJobList row in _jsAppliedJobList)
            {
                try
                {
                    if (row.JobCode.Trim().Substring(0, 3) != "HR1")
                    {
                        if (row.JobType == "TEMP")
                        {
                            values = new object[4];
                            values[0] = row.JobCode;
                            values[1] = row.ResumeId;
                            values[2] = row.UserId;
                            values[3] = row.SRCTYPE;

                            retValue = _TssrConn.ExecutionMethod("RMSJobApplicationDetailsAdd"
                                , ref values
                                , DBOperation.ViewAll
                                , ref ds);
                        }
                        if (row.JobType == "PERM")
                        {
                            values[0] = row.ResumeId;
                            retValue = _conn.ExecutionMethod("RMSFetchDetailsForLocal", ref values, DBOperation.ViewAll, ref ds);
                            object[] Param = new object[28];
                            if (ds != null && ds.Tables[0].Rows.Count > 0)
                            {
                                Param[0] = ds.Tables[0].Rows[0]["RMSResumeID"];
                                Param[1] = ds.Tables[0].Rows[0]["RMSCandidateName"];
                                Param[2] = ds.Tables[0].Rows[0]["RMSCandidateSex"];
                                Param[3] = ds.Tables[0].Rows[0]["RMSCandidateDOB"];
                                Param[4] = ds.Tables[0].Rows[0]["RMSCandidateAddress"];
                                Param[5] = ds.Tables[0].Rows[0]["RMSCandidateMobileNo"];
                                Param[6] = ds.Tables[0].Rows[0]["RMSCandidateEmailID"];
                                Param[7] = ds.Tables[0].Rows[0]["RMSAnnualSalary"];
                                Param[8] = ds.Tables[0].Rows[0]["RMSResumeHeadline"];
                                Param[9] = ds.Tables[0].Rows[0]["RMSResumeSource"];
                                Param[10] = ds.Tables[0].Rows[0]["RMSResumeType"];
                                Param[11] = ds.Tables[0].Rows[0]["RMSUserID"];
                                Param[12] = ds.Tables[0].Rows[0]["RMSPassword"];
                                Param[13] = ds.Tables[0].Rows[0]["RMSCandidateCityID"];
                                Param[14] = ds.Tables[0].Rows[0]["RMSQualificationID"];
                                Param[15] = ds.Tables[0].Rows[0]["RMSCategoryID"];
                                Param[16] = ds.Tables[0].Rows[0]["RMSCandidateKeySkill"];
                                Param[17] = Convert.ToInt32(ds.Tables[0].Rows[0]["RMSCandidateExp"]);
                                Param[18] = ds.Tables[0].Rows[0]["RMSCurrentCompany"];
                                Param[19] = ds.Tables[0].Rows[0]["RMSCurrentDesignation"];
                                Param[20] = ds.Tables[0].Rows[0]["RMSPreviousCompany"];
                                Param[21] = ds.Tables[0].Rows[0]["RMSDetailsText"];
                                Param[22] = (byte[])ds.Tables[0].Rows[0]["RMSDetailsFull"];
                                Param[23] = ds.Tables[0].Rows[0]["RMSFileEXT"];
                                Param[24] = ds.Tables[0].Rows[0]["RMSFileName"];
                                Param[25] = row.UserId.Trim();
                                Param[26] = row.JobType.Trim();
                                Param[27] = row.JobCode.Trim();


                                retValue = _RmsNextconn.ExecutionMethod("RMSResumeDetailsAddForLocalDB"
                                           , ref Param
                                           , DBOperation.ViewAll
                                           , ref ds);
                            }
                        }
                    }
                    else if (row.JobCode.Trim().Substring(0, 3) == "HR1") // to be implemented
                    {
                        //SendMailForHRJob();
                    }
                    ret = 1;
                }
                catch (Exception e)
                {
                    ret = -1;
                }
            }
            return ret;
        }
    }
}
