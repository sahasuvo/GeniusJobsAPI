using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using DatabaseAccessLayer;
using GeniusJobsAPI.Class;

namespace GeniusJobsAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CurriculumController : ApiController
    {
        //[HttpGet]
        //[Route("Ebook/GetBookFor/{format}")]
        //public IHttpActionResult GetbookFor()
        //{
        //    string reqBook = format.ToLower() == "pdf" ? bookPath_Pdf : (format.ToLower() == "xls" ? bookPath_xls : (format.ToLower() == "doc" ? bookPath_doc : bookPath_zip));
        //    string bookName = "sample." + format.ToLower();

        //    //converting Pdf file into bytes array  
        //    var dataBytes = File.ReadAllBytes(reqBook);
        //    //adding bytes to memory stream   
        //    var dataStream = new MemoryStream(dataBytes);
        //    return new eBookResult(dataStream, Request, bookName);
        //}

        [HttpGet]
        [ActionName("GetCV")]
        public HttpResponseMessage CVDownload([FromUri]string ResumeID)
        {
            //RMSFetchBinaryDataByIDMobile

            //string reqBook = format.ToLower() == "pdf" ? bookPath_Pdf : (format.ToLower() == "xls" ? bookPath_xls : (format.ToLower() == "doc" ? bookPath_doc : bookPath_zip));
            //string bookName = "Resume." + format.ToLower();

            int? ReturnStatus = 0;
            System.Data.DataTable dtd = new System.Data.DataTable();
            DatabaseTransaction objDB = new DatabaseTransaction();
            objDB.AddConnectionName = "RMSRemote";

            List<KeyValuePair<object, object>> lst = new List<KeyValuePair<object, object>>();
            lst.Add(new KeyValuePair<object, object>("@resumeID", ResumeID));
            dtd = objDB.SqlGetData("RMSFetchBinaryDataByIDMobile", ref lst, ExecType.Dynamic, ReturnDBOperation.DataTable, ref ReturnStatus);

            MemoryStream objMemory = new MemoryStream();
            String Contenttype = String.Empty;
            String Extension = ".docx";

            if(dtd !=null && dtd.Rows.Count>0)
            {
                objMemory = new MemoryStream((byte[])(dtd.Rows[0]["RMSDetailsFull"]));
                Contenttype = Convert.ToString(dtd.Rows[0]["MIMEType"]);
                Extension = Convert.ToString(dtd.Rows[0]["EXT"]);
            }

            //converting Pdf file into bytes array  
            //var dataBytes = File.ReadAllBytes(reqBook);
            //adding bytes to memory stream   
            

            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(objMemory);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = "Resume"+ Extension;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream"); //new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
        }

        [HttpGet]
        [ActionName("GetProfile")]
        public HttpResponseMessage CandidateProfile([FromUri]string ResumeID)
        {
            //RMSFetchBinaryDataByIDMobile

            //string reqBook = format.ToLower() == "pdf" ? bookPath_Pdf : (format.ToLower() == "xls" ? bookPath_xls : (format.ToLower() == "doc" ? bookPath_doc : bookPath_zip));
            //string bookName = "Resume." + format.ToLower();

            int? ReturnStatus = 0;
            System.Data.DataTable dtd = new System.Data.DataTable();
            DatabaseTransaction objDB = new DatabaseTransaction();
            objDB.AddConnectionName = "RMSRemote";

            List<KeyValuePair<object, object>> lst = new List<KeyValuePair<object, object>>();
            lst.Add(new KeyValuePair<object, object>("@RMSResumeID", ResumeID));
            dtd = objDB.SqlGetData("RMSProfileDetaileMobile", ref lst, ExecType.Dynamic, ReturnDBOperation.DataTable, ref ReturnStatus);

            
            var dtlist = new List<dynamic>();
            if (dtd != null && dtd.Rows.Count > 0)
            {
                dtlist = new CommonMethods().DatatableToList(dtd);
            }

            ResponseClass objresponse = new ResponseClass()
            {
                ResponseCode = dtlist.Count > 0 ? 001 : -101,
                ResponseData = dtlist,
                ResponseStatus = dtlist.Count > 0 ? "Success" : "Failed"
            };

            var jsonformat = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ObjectContent(objresponse.GetType(), objresponse, jsonformat);
            response.StatusCode = dtlist.Count > 0 ? HttpStatusCode.OK : HttpStatusCode.NotFound;
            return response;
        }
    }

    
}
