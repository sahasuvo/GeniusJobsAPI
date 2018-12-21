using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DatabaseAccessLayer;
using GeniusJobsAPI.Class;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http.Headers;
using GeniusJobsAPI.Models;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Http.Cors;

namespace GeniusJobsAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class JobApplicationController : ApiController
    {

        [HttpPost]
        [ActionName("JobApply")]
        //public async Task<JSLogin> Candidate()
        public async Task<HttpResponseMessage> JobApplication()
        //public async Task<ResponseClass> PostJobApplication()
        {
            ///Request: RMASD49685-Perm, RMASD49520-Perm, RMASD49276-Perm, RMASD49188-Perm

            //Logger.Log("Checking : " + "ResumeId: " + "111");

            String ResumeID = string.Empty;
            String JobType = string.Empty;
            String JobID = string.Empty;

            List<dynamic> lstJs = new List<dynamic>();
            List<JobApply> lstJob = new List<JobApply>();

            try
            {
                //if (!Request.Content.IsMimeMultipartContent())
                //{
                //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                //}

                //Logger.Log("Checking : " + "ResumeId: " + "222");

                //RMSResumeCheckforMobile

                //if(Request.Content.Headers.ContentType.MediaType.ToString() != "multipart/form-data")
                //{
                //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                //}


                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                NameValueCollection formData = provider.FormData;

                //Logger.Log("Checking : " + "ResumeId: " + "333");
                foreach (var key in provider.FormData.AllKeys)
                {
                    //string k = key;
                    object k = key;
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        // Trace.WriteLine(string.Format("{0}: {1}", key, val));
                        switch (key)
                        {
                            case "ResumeId":
                                //Logger.Log("Checking : " + "ResumeId: " + val);
                                ResumeID = val.Replace('"', ' ').Trim();
                                break;

                            case "JobDetails":
                                //Logger.Log("Checking : " + "JobDetails: " + val);
                                String str = val.Replace('"', ' ').Trim();//"RMASD49685-Perm, RMASD49520-Perm, RMASD49276-Perm, RMASD49188-Perm";
                                String[] strsplic = str.Trim().Split(',');
                                foreach(var staa in strsplic.AsEnumerable())
                                {
                                    JobApply jbapl = new JobApply()
                                    {
                                        JobType = staa.Split('-').GetValue(1).ToString(),
                                        JobID = staa.Split('-').GetValue(0).ToString(),
                                    };
                                    lstJob.Add(jbapl);
                                   
                                }
                                break;
                        }
                    }
                }

                //Logger.Log("Checking : " + "ResumeId: " + ResumeID + ",JobCode:" + lstJob[0].JobID);

                foreach (JobApply objjob in lstJob)
                {
                    if (objjob.JobType.ToUpper() == "TEMP")
                    {
                        int? tempReturnStatus = 0;
                        System.Data.DataTable dtd = new System.Data.DataTable();
                        DatabaseTransaction objDB = new DatabaseTransaction();
                        objDB.AddConnectionName = "RMSRemote";

                        List<KeyValuePair<object, object>> lst = new List<KeyValuePair<object, object>>();
                        lst.Add(new KeyValuePair<object, object>("@RMSJobCode", objjob.JobID));
                        lst.Add(new KeyValuePair<object, object>("@RMSResumeID", ResumeID));
                        lst.Add(new KeyValuePair<object, object>("@CreatedBy", ResumeID));
                        lst.Add(new KeyValuePair<object, object>("@Type", "TEMP"));

                        dynamic retValue1 = objDB.SqlGetData("RMSJobApplicationDetailsAdd", ref lst, ExecType.Dynamic, ReturnDBOperation.InUpDel, ref tempReturnStatus);

                    }
                    else if (objjob.JobType.ToUpper() == "PERM")
                    {
                        int? canReturnStatus = 0;
                        System.Data.DataTable dtd = new System.Data.DataTable();
                        DatabaseTransaction objDB = new DatabaseTransaction();
                        objDB.AddConnectionName = "RMSRemote";

                        List<KeyValuePair<object, object>> lstcan = new List<KeyValuePair<object, object>>();
                        lstcan.Add(new KeyValuePair<object, object>("@RMSResumeID", ResumeID));


                        dtd = objDB.SqlGetData("RMSFetchDetailsForLocal", ref lstcan, ExecType.Dynamic, ReturnDBOperation.DataTable, ref canReturnStatus);

                        if (dtd != null && dtd.Rows.Count > 0)
                        {

                            int? chkReturnStatus = 0;
                            System.Data.DataTable dtd1 = new System.Data.DataTable();
                            DatabaseTransaction objDB1 = new DatabaseTransaction();
                            objDB.AddConnectionName = "RMSLocal";

                            List<KeyValuePair<object, object>> lstcan1 = new List<KeyValuePair<object, object>>();
                            lstcan1.Add(new KeyValuePair<object, object>("@RemoteRMSResumeID", ResumeID));
                            lstcan1.Add(new KeyValuePair<object, object>("@RMSCandidateName", Convert.ToString(dtd.Rows[0]["RMSCandidateName"])));

                            lstcan1.Add(new KeyValuePair<object, object>("@RMSCandidateSex", Convert.ToString(dtd.Rows[0]["RMSCandidateSex"])));
                            lstcan1.Add(new KeyValuePair<object, object>("@RMSCandidateDOB", Convert.ToString(dtd.Rows[0]["RMSCandidateDOB"])));

                            lstcan1.Add(new KeyValuePair<object, object>("@RMSCandidateAddress", Convert.ToString(dtd.Rows[0]["RMSCandidateAddress"])));
                            lstcan1.Add(new KeyValuePair<object, object>("@RMSCandidateMobileNo", Convert.ToString(dtd.Rows[0]["RMSCandidateMobileNo"])));

                            lstcan1.Add(new KeyValuePair<object, object>("@RMSCandidateEmailID", Convert.ToString(dtd.Rows[0]["RMSCandidateEmailID"])));
                            lstcan1.Add(new KeyValuePair<object, object>("@RMSAnnualSalary", Convert.ToString(dtd.Rows[0]["RMSAnnualSalary"])));

                            lstcan1.Add(new KeyValuePair<object, object>("@RMSResumeHeadline", Convert.ToString(dtd.Rows[0]["RMSResumeHeadline"])));
                            lstcan1.Add(new KeyValuePair<object, object>("@RMSResumeSource", Convert.ToString(dtd.Rows[0]["RMSResumeSource"])));

                            lstcan1.Add(new KeyValuePair<object, object>("@RMSResumeType", Convert.ToString(dtd.Rows[0]["RMSResumeType"])));
                            lstcan1.Add(new KeyValuePair<object, object>("@RMSUserID", Convert.ToString(dtd.Rows[0]["RMSUserID"])));

                            lstcan1.Add(new KeyValuePair<object, object>("@RMSPassword", Convert.ToString(dtd.Rows[0]["RMSPassword"])));
                            lstcan1.Add(new KeyValuePair<object, object>("@RMSCity", Convert.ToString(dtd.Rows[0]["RMSCandidateCityID"])));

                            lstcan1.Add(new KeyValuePair<object, object>("@RMSQualification", Convert.ToString(dtd.Rows[0]["RMSQualificationID"])));
                            lstcan1.Add(new KeyValuePair<object, object>("@RMSCategory", Convert.ToString(dtd.Rows[0]["RMSCategoryID"])));

                            lstcan1.Add(new KeyValuePair<object, object>("@RMSCandidateKeySkill", Convert.ToString(dtd.Rows[0]["RMSCandidateKeySkill"])));
                            lstcan1.Add(new KeyValuePair<object, object>("@RMSCandidateExp", Convert.ToInt32(dtd.Rows[0]["RMSCandidateExp"])));

                            lstcan1.Add(new KeyValuePair<object, object>("@RMSCurrentCompany", Convert.ToString(dtd.Rows[0]["RMSCurrentCompany"])));
                            lstcan1.Add(new KeyValuePair<object, object>("@RMSCurrentDesignation", Convert.ToString(dtd.Rows[0]["RMSCurrentDesignation"])));

                            lstcan1.Add(new KeyValuePair<object, object>("@RMSPreviousCompany", Convert.ToString(dtd.Rows[0]["RMSPreviousCompany"])));
                            lstcan1.Add(new KeyValuePair<object, object>("@RMSDetailsText", Convert.ToString(dtd.Rows[0]["RMSDetailsText"])));
                            lstcan1.Add(new KeyValuePair<object, object>("@RMSDetailsFull", (byte[])dtd.Rows[0]["RMSDetailsFull"]));

                            lstcan1.Add(new KeyValuePair<object, object>("@RMSFileExt", Convert.ToString(dtd.Rows[0]["RMSFileEXT"])));

                            lstcan1.Add(new KeyValuePair<object, object>("@RMSFileName", Convert.ToString(dtd.Rows[0]["RMSFileName"])));
                            lstcan1.Add(new KeyValuePair<object, object>("@CreatedBy", ResumeID));
                            lstcan1.Add(new KeyValuePair<object, object>("@TYPE", objjob.JobType.ToUpper()));
                            lstcan1.Add(new KeyValuePair<object, object>("@JOBCODE", objjob.JobID));


                            dynamic retValue2 = objDB.SqlGetData("RMSResumeDetailsAddForLocalDB", ref lstcan1, ExecType.Dynamic, ReturnDBOperation.InUpDel, ref chkReturnStatus);
                        }

                    }

                }
                lstJs.Add(lstJob);
                ResponseClass objresponse = new ResponseClass()
                {
                    ResponseCode = 1,
                    ResponseData = lstJs,
                    ResponseStatus = "Success"
                };

                var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
                HttpResponseMessage response = new HttpResponseMessage();
                response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);

                return response;
                //return objresponse;

            }
            catch (Exception ex)
            {
                lstJs.Add(ex.Message);
                ResponseClass objresponse = new ResponseClass()
                {
                    ResponseCode = -102,
                    ResponseData = lstJs,
                    ResponseStatus = "Error"
                };

                Logger.Log("Exception Source : " + ex.TargetSite + " Message : " + ex.Message);


                var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
                HttpResponseMessage response = new HttpResponseMessage();
                response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);

                return response;
                //return objresponse;
            }

            finally
            {
                lstJs = null;
            }

        }
    }
}
