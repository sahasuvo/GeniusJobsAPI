using IFBiOSApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IFBiOSApi.Controllers
{
    public class get_apkversionController : ApiController
    {
        DataSet ds = new DataSet();
        ResponseModel _objResponseModel = new ResponseModel();
        [HttpGet]
        public ResponseModel Get()
        {
            bool dataFlag = true;


            string apkversion = "1.1";
            _objResponseModel.responseData = "";
            _objResponseModel.responseStatus = dataFlag;
            _objResponseModel.responseCode = dataFlag == true ? "1" : "0";
            _objResponseModel.responseText = apkversion;


            return _objResponseModel;
        }
    }
}
