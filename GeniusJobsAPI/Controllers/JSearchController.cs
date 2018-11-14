using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using DatabaseAccessLayer;
using GeniusJobsAPI.Class;
using System.Data;
using System.Collections;

namespace GeniusJobsAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class JSParams
    {
        public string Location { get; set; }
        public string KeySkill { get; set; }
        public string Qualification { get; set; }
        public string JobCategory { get; set; }
        public string MinExp { get; set; }
        public string MaxExp { get; set; }
        public string JobCode { get; set; }
        public string SourceType { get; set; }
    }
    
    public class JSearchController : ApiController
    {

        [HttpGet]
        [ActionName("GetJobs")]
        //[Route("api/JSearch/GetJobs")]
        public HttpResponseMessage GetJobbySearch([FromUri] String PLocation, [FromUri] String PKeySkill, [FromUri] String PQualification, [FromUri] String PJobCategory, [FromUri] String PMinExp,[FromUri]String PMaxExp,[FromUri] String PJobCode,[FromUri] String PSourceType)
        {
            #region R&D Work
            //{Location:"CTYGSP0510000001",KeySkill:"accounts",Qualification:"%",JobCategory:"%",MinExp:"0",MaxExp:"31",JobCode:"%",SourceType:"PERM"}

            //dynamic item1 = Newtonsoft.Json.JsonConvert.DeserializeObject(Params);

            //var lst = Newtonsoft.Json.JsonConvert.DeserializeObject(Params);

            //List<dynamic> lstitem = new List<dynamic>();
            //lstitem.Add(item1);

            ////var bb = from item in lst select item;

            ////foreach(var aa in lst)
            ////{

            ////}
            #endregion R&D Work

            JSParams JParam = new JSParams()
            {
                Location = PLocation,
                KeySkill = PKeySkill,
                Qualification= PQualification,
                JobCategory = PJobCategory,
                MinExp= PMinExp,
                MaxExp = PMaxExp,
                JobCode= PJobCode,
                SourceType = PSourceType
            };
            List<dynamic> lstJob = GetParamDetails(JParam);

            ResponseClass objresponse = new ResponseClass()
            {
                ResponseCode = lstJob.Count > 0 ? 001 : -101,
                ResponseData = lstJob,
                ResponseStatus = lstJob.Count > 0 ? "Success" : "Failed"
            };

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);
            response.StatusCode = lstJob.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return response;
        }

        [HttpPost]
        [Route("Jobs")]
        public IHttpActionResult PostJobbySearch([FromUri] List<JSParams> Params)
        {

            foreach (var item in Params)
            {
                //Do Some Thing Here 
            }
            return ok();


            // //{ Location: "CTYGSP0510000001",KeySkill: "accounts",Qualification: "%",JobCategory: "%",MinExp: "0",MaxExp: "31",JobCode: "%",SourceType: "PERM"}

            //dynamic item1 = Newtonsoft.Json.JsonConvert.DeserializeObject(Params);

            //  //var lst = Newtonsoft.Json.JsonConvert.DeserializeObject(Params);

            //List<dynamic> lstitem = new List<dynamic>();
            //lstitem.Add(item1);

            //var bb = from item in lst select item;

            ////foreach (var aa in lst)
            ////{

            ////}

            //JSParams JParam = new JSParams();
            //List<dynamic> lstJob = GetParamDetails(JParam);

            //ResponseClass objresponse = new ResponseClass()
            //{
            //    ResponseCode = lstJob.Count > 0 ? 001 : -101,
            //    ResponseData = lstJob,
            //    ResponseStatus = lstJob.Count > 0 ? "Success" : "Failed"
            //};

            //var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            //HttpResponseMessage response = new HttpResponseMessage();
            //response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);
            //response.StatusCode = lstJob.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            //return response;
        }

        private IHttpActionResult ok()
        {
            throw new NotImplementedException();
        }

        public List<dynamic> GetParamDetails(JSParams objparams)
        {

            //RMSJSSelectJobDetailsSearchforApp
            //TRJOBSEARCHforApp

            int? ReturnStatus = 0;
            System.Data.DataTable dtd = new System.Data.DataTable();
            DatabaseTransaction objDB = new DatabaseTransaction();
            string strProc = string.Empty;


            List<KeyValuePair<object, object>> lstJobs = new List<KeyValuePair<object, object>>();
            lstJobs.Add(new KeyValuePair<object, object>("@SelectedCity", objparams.Location));
            lstJobs.Add(new KeyValuePair<object, object>("@SelectedSKILL", objparams.KeySkill));
            lstJobs.Add(new KeyValuePair<object, object>("@SelectedQualification", objparams.Qualification));
            lstJobs.Add(new KeyValuePair<object, object>("@Selectedfunction", objparams.JobCategory));
            lstJobs.Add(new KeyValuePair<object, object>("@SelectedMINEXP", objparams.MinExp));
            lstJobs.Add(new KeyValuePair<object, object>("@SelectedMAXEXP", objparams.MaxExp));
            lstJobs.Add(new KeyValuePair<object, object>("@JobCode", objparams.JobCode));


            switch (objparams.SourceType.ToUpper())
            {
                case "PERM":
                    dtd = null;
                    objDB.AddConnectionName = "RMSLocal";
                    dtd = objDB.SqlGetData("RMSJSSelectJobDetailsSearchforApp", ref lstJobs, ExecType.Dynamic, ReturnDBOperation.DataTable, ref ReturnStatus);
                    break;

                case "TEMP":
                    dtd = null;
                    objDB.AddConnectionName = "TSSRMS";
                    dtd = objDB.SqlGetData("TRJOBSEARCHforApp", ref lstJobs, ExecType.Dynamic, ReturnDBOperation.DataTable, ref ReturnStatus);
                    break;
                case "ALL":

                    dtd = null;
                    DataSet dsPerm = new DataSet();
                    DataSet dsTemp = new DataSet();
                    objDB.AddConnectionName = "RMSLocal";
                    dsPerm = objDB.SqlGetData("RMSJSSelectJobDetailsSearchforApp", ref lstJobs, ExecType.Dynamic, ReturnDBOperation.DataSet, ref ReturnStatus);

                    objDB.AddConnectionName = "TSSRMS";
                    dsTemp = objDB.SqlGetData("TRJOBSEARCHforApp", ref lstJobs, ExecType.Dynamic, ReturnDBOperation.DataSet, ref ReturnStatus);

                    if (dsPerm != null && dsPerm.Tables.Count > 0 && dsPerm.Tables[0].Rows.Count > 0)
                    {
                        if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                        {
                            dsPerm.Merge(dsTemp.Tables[0]);
                        }

                        dtd = dsPerm.Tables[0];
                    }

                    if (dsPerm == null && dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
                    {
                        dtd = dsTemp.Tables[0];
                    }

                    

                    break;
            }
            var dtlist = new List<dynamic>();
            if (dtd != null && dtd.Rows.Count > 0)
            {
                dtlist = new CommonMethods().DatatableToList(dtd);
            }
            return dtlist;
        }

        /*
        void TempDB()
        {
            string strLocationID = "";
            bool comma = false;
            ArrayListLocationName = new ArrayList();
            for (int i = 0; i < ChkJLoc.Items.Count; i++)
            {
                if (ChkJLoc.Items[i].Selected)
                {
                    strLocationID += comma ? "," + ChkJLoc.Items[i].Value : ChkJLoc.Items[i].Value;
                    ArrayListLocationName.Add(ChkJLoc.Items[i].Text);
                    comma = true;
                }
            }
            string strQualificationID = "";
            comma = false;
            ArrayListQualificationName = new ArrayList();
            for (int i = 0; i < ChkJQual.Items.Count; i++)
            {
                if (ChkJQual.Items[i].Selected)
                {
                    strQualificationID += comma ? "," + ChkJQual.Items[i].Value : ChkJQual.Items[i].Value;
                    ArrayListQualificationName.Add(ChkJQual.Items[i].Text);
                    comma = true;
                }
            }
            stringSkill = new String[0];
            if (TxtKeySkill.Text.Length > 0)
            {
                stringSkill = TxtKeySkill.Text.Split(',');
            }

            //RMSLibrary obj = new RMSLibrary();
            DBConnection obj = new DBConnection();
            //RMSLibrary obj1 = new RMSLibrary();
            DBConnection obj1 = new DBConnection();

            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataView dtView = new DataView();

            if (TxtJobCode.Text.Trim() == String.Empty)
            {

                try
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TSSRMS"].ConnectionString);
                    SqlCommand cmdobj = new SqlCommand();
                    cmdobj.Connection = con;
                    cmdobj.CommandType = CommandType.StoredProcedure;
                    cmdobj.CommandText = "TRJOBSEARCH";
                    cmdobj.Parameters.AddWithValue("@SelectedQualification", strQualificationID.Trim().Length == 0 ? "%" : strQualificationID.Trim());
                    cmdobj.Parameters.AddWithValue("@SelectedCity", strLocationID.Trim().Length == 0 ? "%" : strLocationID.Trim());
                    cmdobj.Parameters.AddWithValue("@Selectedfunction", DrpFunctionalArea.SelectedItem.Value);
                    cmdobj.Parameters.AddWithValue("@SelectedSKILL", TxtKeySkill.Text.Trim() == string.Empty ? "%" : TxtKeySkill.Text.Trim());
                    cmdobj.Parameters.AddWithValue("@SelectedMAXEXP", DrpExpMaxYear.SelectedItem.Value);
                    cmdobj.Parameters.AddWithValue("@SelectedMINEXP", DrpExpMinYear.SelectedItem.Value);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmdobj;
                    da.Fill(ds1);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }

                ds.Merge(ds1.Tables[0]);
                dtView.Table = ds.Tables[0];
                dtView.Sort = "RMSJobAssignedOn DESC";

            }
            else
            {

                String strTempCode = TxtJobCode.Text.Trim();
                DataSet dsTemp = new DataSet();
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TSSRMS"].ConnectionString);
                SqlCommand cmdobj = new SqlCommand();
                cmdobj.Connection = con;
                cmdobj.CommandType = CommandType.StoredProcedure;
                cmdobj.CommandText = "TRJOBSEARCHBYID";
                cmdobj.Parameters.AddWithValue("@AssignmentID", strTempCode);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmdobj;
                da.Fill(ds);

                dtView.Table = ds.Tables[0];
            }


            if (ds.Tables.Count > 0)
            {
                Totalrecord = ds.Tables[0].Rows.Count;
                GridView1.DataSource = dtView;
                GridView1.DataBind();
                ds = null;
                dtView = null;
            }
        }

        void PermDB()
        {
            string strLocationID = "";
            bool comma = false;
            ArrayListLocationName = new ArrayList();
            for (int i = 0; i < ChkJLoc.Items.Count; i++)
            {
                if (ChkJLoc.Items[i].Selected)
                {
                    strLocationID += comma ? "," + ChkJLoc.Items[i].Value : ChkJLoc.Items[i].Value;
                    ArrayListLocationName.Add(ChkJLoc.Items[i].Text);
                    comma = true;
                }
            }
            string strQualificationID = "";
            comma = false;
            ArrayListQualificationName = new ArrayList();
            for (int i = 0; i < ChkJQual.Items.Count; i++)
            {
                if (ChkJQual.Items[i].Selected)
                {
                    strQualificationID += comma ? "," + ChkJQual.Items[i].Value : ChkJQual.Items[i].Value;
                    ArrayListQualificationName.Add(ChkJQual.Items[i].Text);
                    comma = true;
                }
            }
            stringSkill = new String[0];
            if (TxtKeySkill.Text.Length > 0)
            {
                stringSkill = TxtKeySkill.Text.Split(',');
            }

            DBConnection obj = new DBConnection();
            obj._ConnectionString = ConfigurationManager.ConnectionStrings["RMSNext"].ConnectionString;
            DataSet ds = new DataSet();
            DataView dtView = new DataView();
            if (TxtJobCode.Text == string.Empty)
            {
                string[] param ={   strLocationID.Trim().Length ==0 ? "%":strLocationID.Trim() ,

                                TxtKeySkill.Text.Trim() ==string.Empty ? "%":TxtKeySkill.Text.Trim() ,

                                strQualificationID.Trim().Length ==0 ? "%":strQualificationID.Trim(),
                                DrpFunctionalArea.SelectedItem.Value,
                                DrpExpMinYear.SelectedItem.Value,
                                DrpExpMaxYear.SelectedItem.Value
                            };
                ds = obj.SelectCommand("RMSJSSelectJobDetailsAfterSearch", param);

                dtView.Table = ds.Tables[0];
                dtView.Sort = "RMSJobAssignedOn DESC";
            }

            else
            {
                String strPermCode = TxtJobCode.Text.Trim();
                string[] param = { strPermCode, "perm" };
                ds = obj.SelectCommand("RMSJSSelectJobDetailsAfterSearchByJobCode", param);
                dtView.Table = ds.Tables[0];
            }

            if (ds.Tables.Count > 0)
            {
                Totalrecord = ds.Tables[0].Rows.Count;
                GridView1.DataSource = dtView;
                GridView1.DataBind();
                ds = null;
                dtView = null;
            }
        }

        void TempPermDB()
        {
            string strLocationID = "";
            bool comma = false;
            ArrayListLocationName = new ArrayList();
            for (int i = 0; i < ChkJLoc.Items.Count; i++)
            {
                if (ChkJLoc.Items[i].Selected)
                {
                    strLocationID += comma ? "," + ChkJLoc.Items[i].Value : ChkJLoc.Items[i].Value;
                    ArrayListLocationName.Add(ChkJLoc.Items[i].Text);
                    comma = true;
                }
            }
            string strQualificationID = "";
            comma = false;
            ArrayListQualificationName = new ArrayList();
            for (int i = 0; i < ChkJQual.Items.Count; i++)
            {
                if (ChkJQual.Items[i].Selected)
                {
                    strQualificationID += comma ? "," + ChkJQual.Items[i].Value : ChkJQual.Items[i].Value;
                    ArrayListQualificationName.Add(ChkJQual.Items[i].Text);
                    comma = true;
                }
            }
            stringSkill = new String[0];
            if (TxtKeySkill.Text.Length > 0)
            {
                stringSkill = TxtKeySkill.Text.Split(',');
            }

            DBConnection obj = new DBConnection();
            DBConnection obj1 = new DBConnection();

            obj._ConnectionString = ConfigurationManager.ConnectionStrings["RMSNext"].ConnectionString;
            obj1._ConnectionString = ConfigurationManager.ConnectionStrings["TSSRMS"].ConnectionString;
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataView dtView = new DataView();
            if (TxtJobCode.Text == string.Empty && ddlJType.SelectedValue == "%")
            {
                string[] param ={   strLocationID.Trim().Length ==0 ? "%":strLocationID.Trim() ,

                                TxtKeySkill.Text.Trim() ==string.Empty ? "%":TxtKeySkill.Text.Trim() ,

                                strQualificationID.Trim().Length ==0 ? "%":strQualificationID.Trim(),
                                DrpFunctionalArea.SelectedItem.Value,
                                DrpExpMinYear.SelectedItem.Value,
                                DrpExpMaxYear.SelectedItem.Value
                            };

                ds = obj.SelectCommand("RMSJSSelectJobDetailsAfterSearch", param);

                try
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TSSRMS"].ConnectionString);
                    SqlCommand cmdobj = new SqlCommand();
                    cmdobj.Connection = con;
                    cmdobj.CommandType = CommandType.StoredProcedure;
                    cmdobj.CommandText = "TRJOBSEARCH";
                    cmdobj.Parameters.AddWithValue("@SelectedQualification", strQualificationID.Trim().Length == 0 ? "%" : strQualificationID.Trim());
                    cmdobj.Parameters.AddWithValue("@SelectedCity", strLocationID.Trim().Length == 0 ? "%" : strLocationID.Trim());
                    cmdobj.Parameters.AddWithValue("@Selectedfunction", DrpFunctionalArea.SelectedItem.Value);
                    cmdobj.Parameters.AddWithValue("@SelectedSKILL", TxtKeySkill.Text.Trim() == string.Empty ? "%" : TxtKeySkill.Text.Trim());
                    cmdobj.Parameters.AddWithValue("@SelectedMAXEXP", DrpExpMaxYear.SelectedItem.Value);
                    cmdobj.Parameters.AddWithValue("@SelectedMINEXP", DrpExpMinYear.SelectedItem.Value);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmdobj;
                    da.Fill(ds1);
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }

                ds.Merge(ds1.Tables[0]);
                dtView.Table = ds.Tables[0];
                dtView.Sort = "RMSJobAssignedOn DESC";
            }

            if (ds.Tables.Count > 0)
            {
                Totalrecord = ds.Tables[0].Rows.Count;
                GridView1.DataSource = dtView;
                GridView1.DataBind();
                ds = null;
                dtView = null;
            }
        }
        void DB()
        {
            if (ddlJType.SelectedValue.ToUpper() == "TEMP")
            {
                TempDB();
            }

            else if (ddlJType.SelectedValue.ToUpper() == "PERM")
            {
                PermDB();
            }
            else
            {
                TempPermDB();
            }

        }
            */
    }
}
