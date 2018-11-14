using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using IFBiOSApi.Models;
using IFBiOSApi.Controllers;
using System.Diagnostics;
using System.Data;
using System.Configuration;

namespace IFBiOSApi.Controllers
{
    public class post_DisplayMatrixController : ApiController
    {
        DbConnectionController _conn = new DbConnectionController();
        TsmDbConnectionController _TsmConn = new TsmDbConnectionController();
        string IFB = ConfigurationManager.AppSettings["IFB"].ToString();
        string GCL = ConfigurationManager.AppSettings["GCL"].ToString();

        ResponseModel _objResponseModel = new ResponseModel();

        public async Task<ResponseModel> Postdata()
        {
            string AEMEmployeeID; string Model; string Category; string SalesDate; string SecurityCode;
            AEMEmployeeID = Model = Category = SalesDate = SecurityCode = string.Empty;

            bool dataFlag = true;
            DisplayMatrix _DisplayMatrix = new DisplayMatrix();
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            try
            {
                var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
                //access form data  
                NameValueCollection formData = provider.FormData;

                foreach (var key in provider.FormData.AllKeys)
                {
                    string k = key;
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        // Trace.WriteLine(string.Format("{0}: {1}", key, val));
                        switch (key)
                        {
                            case "SalesDate":
                                SalesDate = val;
                                break;
                            case "Category":
                                Category = val;
                                break;
                            case "Model":
                                Model = val;
                                break;
                            case "AEMEmployeeID":
                                AEMEmployeeID = val;
                                break;
                            case "SecurityCode":
                                SecurityCode = val;
                                break;

                        }
                    }
                }

                if (AEMEmployeeID == "" || AEMEmployeeID == string.Empty)
                    dataFlag = false;
                #region feedback saving
                if (dataFlag)
                {
                    object[] data = new object[5];
                    data[0] = SalesDate;
                    data[1] = Category;
                    data[2] = Model;
                    data[3] = AEMEmployeeID;
                    data[4] = Convert.ToInt32(DBOperation.Insert);

                    DataSet dsResult = new DataSet();
                    int retValue = 0;
                    if (SecurityCode.Contains("IFB"))
                        retValue = _conn.ExecutionMethod("IFBDISPLAYMATRIXCRUDWITHMODELUSINGAPP", ref data, DBOperation.ViewAll, ref dsResult);
                    else if (SecurityCode.Contains("GCL"))
                        retValue = _TsmConn.ExecutionMethod("IFBDISPLAYMATRIXCRUDWITHMODELUSINGAPP", ref data, DBOperation.ViewAll, ref dsResult);

                    //if (SecurityCode == IFB)

                    //    else if (SecurityCode == GCL)
                    //        retValue = _TsmConn.ExecutionMethod("IFBDISPLAYMATRIXCRUDWITHMODELUSINGAPP", ref data, DBOperation.ViewAll, ref dsResult);

                    dataFlag = retValue == 0 ? true : false;
                }
                #endregion
                _objResponseModel.responseData = "";
                _objResponseModel.responseStatus = dataFlag;
                _objResponseModel.responseCode = dataFlag == true ? "1" : "0";
                _objResponseModel.responseText = dataFlag == true ? "Your details updated" : "Sorry! data not saved.";

                return _objResponseModel;
            }
            catch (System.Exception e)
            {

                dataFlag = false;
                _objResponseModel.responseData = "";
                _objResponseModel.responseStatus = dataFlag;
                _objResponseModel.responseCode = dataFlag == true ? "1" : "0";
                _objResponseModel.responseText = e.ToString();

                return _objResponseModel;
            }

        }
    }


}
