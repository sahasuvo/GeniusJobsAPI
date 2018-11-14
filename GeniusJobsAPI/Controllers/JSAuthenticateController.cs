﻿using GeniusMAppsAPI.Models;
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
    public class JSAuthenticateController : ApiController
    {
        JobServicesController _conn = new JobServicesController();

        [HttpGet]
        public List<JSLogin> Get([FromUri]string jsonData)
        {
            JSLogin _jsonData = JsonConvert.DeserializeObject<JSLogin>(jsonData);
            object[] values = new object[2];
            values[0] = _jsonData.JsUsername;
            values[1] = _jsonData.JsPassword;

            List<JSLogin> Jslist = new List<JSLogin>();
            JSLogin Js = new JSLogin();

            DataSet ds = new DataSet();
            int retValue = _conn.ExecutionMethod("RMSJSLogin", ref values, DBOperation.ViewAll, ref ds);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Js.RMSUserID = Convert.ToString(ds.Tables[0].Rows[0]["RMSUserID"]);
                Js.RMSResumeID = Convert.ToString(ds.Tables[0].Rows[0]["RMSResumeID"]);
                Js.JsPassword = Convert.ToString(ds.Tables[0].Rows[0]["Password"]);
                Js.ResumeData = ds.Tables[0].Rows[0]["RMSDetailsFull"] == DBNull.Value ? System.Text.Encoding.UTF8.GetBytes(String.Empty) : (byte[])(ds.Tables[0].Rows[0]["RMSDetailsFull"]);// System.Text.Encoding.Unicode.GetBytes((ds.Tables[0].Rows[r]["AImage"].ToString()));
                Js.RMSCandidateName = Convert.ToString(ds.Tables[0].Rows[0]["RMSCandidateName"]);
                Js.RMSCandidateEmailID = Convert.ToString(ds.Tables[0].Rows[0]["RMSCandidateEmailID"]);
                Js.RMSCandidateMobileNo = Convert.ToString(ds.Tables[0].Rows[0]["RMSCandidateMobileNo"]);
                // 07.02.2017 profile edit
                Js.RMSCandidateCityID = Convert.ToString(ds.Tables[0].Rows[0]["RMSCandidateCityID"]);
                Js.RMSCandidatQualificationID = Convert.ToString(ds.Tables[0].Rows[0]["RMSCandidateCityID"]);
                Js.RMSCandidateCity = Convert.ToString(ds.Tables[0].Rows[0]["CityName"]);
                Js.RMSCandidatQualification = Convert.ToString(ds.Tables[0].Rows[0]["Qualification"]);
                Js.RMSPermanentAddr = Convert.ToString(ds.Tables[0].Rows[0]["RMSCandidateAddress"]);
            }
            else
            {
                Js.RMSUserID = "";
                Js.RMSResumeID = "";
                Js.JsPassword = "";
                Js.ResumeData = System.Text.Encoding.UTF8.GetBytes(String.Empty);
                Js.RMSCandidateName = "";
                Js.RMSCandidateEmailID = "";
                Js.RMSCandidateMobileNo = "";
            }
            Jslist.Add(Js);
            return Jslist;
        }

        //[HttpGet]
        //public int Get([FromUri]string jsonData)
        //{
        //    JSLogin _js = JsonConvert.DeserializeObject<JSLogin>(jsonData);

        //    if (_js.DbOperation == Convert.ToInt32(DBOperation.Delete)) // IT IS USED TO UPDATE FORGOT PASSWORD
        //    {
        //        object[] values = new object[6];
        //        values[0] = "";
        //        values[1] = "";
        //        values[2] = "";
        //        values[3] = null;
        //        values[4] = _js.RMSCandidateEmailID;
        //        values[5] = _js.JsPassword;
        //        values[6] = 7;
        //    }
        //    return 1;
        //}

        [HttpPost]
        public int POST([FromBody]JObject jsonData)
        // public int POST()
        {
            //string a = " { \"JsUsername\":null,\"JsPassword\":\"5osa2i\",\"RMSUserID\":null,\"RMSResumeID\":null,\"RMSCandidateName\":\"demo user\",\"RMSCandidateMobileNo\":\"123654789\",\"RMSCandidateEmailID\":\"userdemo@gmail.com\",\"RMSDob\":\"2017/05/09\",\"ResumeData\":\"TmFtZSA6IGRlbW8gdXNlcgpDb250YWN0IE51bWJlciA6IDEyMzY1NDc4OQpFbWFpbCA6IHVzZXJkZW1vQGdtYWlsLmNvbQpHZW5kZXIgOiBNYWxlCkRhdGUgT2YgQmlydGggOiAyMDE3LzA1LzA5ClBlcm1hbmVudCBBZGRyZXNzIDotIAprb2xrYXRhCkFBTU9OCklORElBCkNhdGVnb3J5IDogQUNPVU5UUy9GSU5BTkNFL1RBWC9DUy9BVURJVApRdWFsaWZpY2F0aW9uIDogRElQTE9NQQpTa2lsbCA6IC5uZXQsIHNxbApFeHBlcmllbmNlIDogNyBZcnMuCg==\",\"DbOperation\":3,\"RMSCandidatQualificationID\":null,\"RMSCandidateCityID\":null,\"RMSCandidatQualification\":null,\"RMSCandidateCity\":null,\"RMSPermanentAddr\":null,\"JsGender\":\"M\",\"RMSKeySkill\":\".net, sql\",\"RMSExperience\":7}";
            //        a = "{ \"JsUsername\":null,\"JsPassword\":\"aalpi@\",\"RMSUserID\":null,\"RMSResumeID\":null,\"RMSCandidateName\":\"suman pal\",\"RMSCandidateMobileNo\":\"123654789\",\"RMSCandidateEmailID\":\"sumanpal@gmail.com\",\"RMSDob\":\"1900/01/01\",\"ResumeData\":\"Q2hvb3NpbmcgdGhlIHJpZ2h0P3RlbXBsYXRlIHdpbGwgYWxsb3cgeW91IHRvIGhpZ2hsaWdodCB0aGUgYmVzdCBhc3BlY3RzIG9mIHlvdXIgcHJvZmVzc2lvbmFsIGJhY2tncm91bmQgYW5kIGNyZWRlbnRpYWxzIHRvIHBvdGVudGlhbCBlbXBsb3llcnMuPyBJZiB5b3VyIGV4cGVyaWVuY2UgZG9lc24ndCBmYWxsIGluIHRoZSBhYm92ZSBhcmVhcywgYSByZXN1bWUgbWlnaHQgd29yayBiZXN0IGZvciB5b3UgaW5zdGVhZCBvZiBhIEN1cnJpY3VsdW0gVml0YWUuPwoKVGhlc2UgQ3VycmljdWx1bSBWaXRhZT9UZW1wbGF0ZXM/YXJlIGluIGJvdGggTWljcm9zb2Z0IFdvcmQgYW5kIEFkb2JlIEFjcm9iYXQgZm9ybWF0cy4gWW91IG11c3QgaGF2ZSB0aGU/QWRvYmUgQWNyb2JhdCBSZWFkZXI/dG8gZG93bmxvYWQ/dGhlIHRlbXBsYXRlcz9pbiBQREYgZm9ybWF0LiBZb3UgY2FuIGRvd25sb2FkIGEgY29weSBvZiBBY3JvYmF0IFJlYWRlciBmb3IgZnJlZT9oZXJlLiBDbGljayBvbiB0aGUgbGluayB0byBvcGVuIHVwIGEgQ1Y/VGVtcGxhdGUgb2YgeW91ciBjaG9pY2Uu\",\"DbOperation\":3,\"RMSCandidatQualificationID\":null,\"RMSCandidateCityID\":null,\"RMSCandidatQualification\":null,\"RMSCandidateCity\":null,\"RMSPermanentAddr\":null,\"JsGender\":null,\"RMSKeySkill\":\"NA\",\"RMSExperience\":0}";
            //string a = "{\"JsUsername\":null,\"JsPassword\":null,\"RMSUserID\":null,\"RMSResumeID\":\"RMRES9495395\",\"RMSCandidateName\":\"\",\"RMSCandidateMobileNo\":\"\",\"RMSCandidateEmailID\":\"suvojit @gmail.com\",\"RMSDob\":\"1900 / 01 / 01\",\"ResumeData\":\"0M8R4KGxGuEAAAAAAAAAAAAAAAAAAAAAPgADAP7 / CQAGAAAAAAAAAAAAAAABAAAAIQAAAAAAAAAAEAAAIwAAAAEAAAD +////AAAAACAAAAD////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////spcEAWQAJCAAAABK/AAAAAAAAEAAAAAAABAAA0wQAAA4AYmpiavNX81cAAAAAAAAAAAAAAAAAAAAAAAAJBBYAIg4AAJE9AQCRPQEA0wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//w8AAAAAAAAAAAD//w8AAAAAAAAAAAD//w8AAAAAAAAAAAAAAAAAAAAAAF0AAAAAAFIBAAAAAAAAUgEAAFIBAAAAAAAAUgEAAAAAAABSAQAAAAAAAFIBAAAAAAAAUgEAABQAAAAAAAAAAAAAAGYBAAAAAAAAZgEAAAAAAABmAQAAAAAAAGYBAAAAAAAAZgEAAAwAAAByAQAAFAAAAGYBAAAAAAAArQIAAP4BAACSAQAAAAAAAJIBAAAAAAAAkgEAAAAAAACSAQAAAAAAAJIBAAAAAAAAkgEAAAAAAACSAQAAAAAAAJIBAAAAAAAAcgIAAAIAAAB0AgAAAAAAAHQCAAAAAAAAdAIAAAAAAAB0AgAAAAAAAHQCAAAAAAAAdAIAACQAAACrBAAA9AEAAJ8GAAAMAQAAmAIAABUAAAAAAAAAAAAAAAAAAAAAAAAAUgEAAAAAAACSAQAAAAA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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAAAAAyAAL1IgAB+wgi4gsMZBIbAIByKwCAcjkKAFJJCgBSWwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA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AZgBmAGkAYwBlAFwAVABlAG0AcABsAGEAdABlAHMAXABOAG8AcgBtAGEAbAAuAGQAbwB0AA0AVABlAHMAdAAgAFcAbwByAGQAIABEAG8AYwAAAAAAAAAXAEQAaQB2AGkAcwBpAG8AbgAgAG8AZgAgAEkAbgBmAG8AcgBtAGEAdABpAGMAcwAXAEQAaQB2AGkAcwBpAG8AbgAgAG8AZgAgAEkAbgBmAG8AcgBtAGEAdABpAGMAcwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7/AAAEWgIAAAAAAAAAAAAAAAAAAAAAAAEAAADghZ/y+U9oEKuRCAArJ7PZMAAAAIABAAAQAAAAAQAAAIgAAAACAAAAkAAAAAMAAACoAAAABAAAALQAAAAFAAAA1AAAAAcAAADgAAAACAAAAPQAAAAJAAAAFAEAABIAAAAgAQAACgAAADwBAAAMAAAASAEAAA0AAABUAQAADgAAAGABAAAPAAAAaAEAABAAAABwAQAAEwAAAHgBAAACAAAA5AQAAB4AAAAOAAAAVGVzdCBXb3JkIERvYwBvAB4AAAABAAAAAGVzdB4AAAAYAAAARGl2aXNpb24gb2YgSW5mb3JtYXRpY3MAHgAAAAEAAAAAaXZpHgAAAAsAAABOb3JtYWwuZG90ACAeAAAAGAAAAERpdmlzaW9uIG9mIEluZm9ybWF0aWNzAB4AAAACAAAAMQB2aR4AAAATAAAATWljcm9zb2Z0IFdvcmQgOC4wAHRAAAAAAF7QsgAAAABAAAAAANQAJVidwwFAAAAAADLR11idwwEDAAAAAQAAAAMAAAAeAAAAAwAAAK4AAAADAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD+/wAABFoCAAAAAAAAAAAAAAAAAAAAAAACAAAAAtXN1ZwuGxCTlwgAKyz5rkQAAAAF1c3VnC4bEJOXCAArLPmuTAEAAAgBAAAMAAAAAQAAAGgAAAAPAAAAcAAAAAUAAACQAAAABgAAAJgAAAARAAAAoAAAABcAAACoAAAACwAAALAAAAAQAAAAuAAAABMAAADAAAAAFgAAAMgAAAANAAAA0AAAAAwAAADqAAAAAgAAAOQEAAAeAAAAGAAAAFVuaXZlcnNpdHkgb2YgRWRpbmJ1cmdoAAMAAAABAAAAAwAAAAEAAAADAAAA1QAAAAMAAABqEAgACwAAAAAAAAALAAAAAAAAAAsAAAAAAAAACwAAAAAAAAAeEAAAAQAAAA4AAABUZXN0IFdvcmQgRG9jAAwQAAACAAAAHgAAAAYAAABUaXRsZQADAAAAAQAAAJgAAAADAAAAAAAAACAAAAABAAAANgAAAAIAAAA+AAAAAQAAAAIAAAAKAAAAX1BJRF9HVUlEAAIAAADkBAAAQQAAAE4AAAB7ADUAMQAwADIAQgA1AEEAMQAtADgARAA0ADgALQA0ADcARgBEAC0AOAAzAEYAQQAtAEQAMgA0ADgANgA4ADIARQA0AEQANwBBAH0AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAIAAAADAAAABAAAAAUAAAAGAAAABwAAAP7///8JAAAACgAAAAsAAAAMAAAADQAAAA4AAAAPAAAA/v///xEAAAASAAAAEwAAABQAAAAVAAAAFgAAABcAAAD+////GQAAABoAAAAbAAAAHAAAAB0AAAAeAAAAHwAAAP7////9////IgAAAP7////+/////v////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////9SAG8AbwB0ACAARQBuAHQAcgB5AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFgAFAf//////////AwAAAAYJAgAAAAAAwAAAAAAAAEYAAAAAID6I5VidwwHAzcblWJ3DASQAAACAAAAAAAAAADEAVABhAGIAbABlAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAOAAIA////////////////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAAAAAQAAAAAAAAVwBvAHIAZABEAG8AYwB1AG0AZQBuAHQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABoAAgEFAAAA//////////8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAAAAAAAAAFAFMAdQBtAG0AYQByAHkASQBuAGYAbwByAG0AYQB0AGkAbwBuAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKAACAQIAAAAEAAAA/////wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAAAAAAEAAAAAAAAAUARABvAGMAdQBtAGUAbgB0AFMAdQBtAG0AYQByAHkASQBuAGYAbwByAG0AYQB0AGkAbwBuAAAAAAAAAAAAAAA4AAIB////////////////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGAAAAAAQAAAAAAAAAQBDAG8AbQBwAE8AYgBqAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABIAAgEBAAAABgAAAP////8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAagAAAAAAAABPAGIAagBlAGMAdABQAG8AbwBsAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFgABAP///////////////wAAAAAAAAAAAAAAAAAAAAAAAAAAwM3G5VidwwHAzcblWJ3DAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA////////////////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQAAAP7///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////8BAP7/AwoAAP////8GCQIAAAAAAMAAAAAAAABGGAAAAE1pY3Jvc29mdCBXb3JkIERvY3VtZW50AAoAAABNU1dvcmREb2MAEAAAAFdvcmQuRG9jdW1lbnQuOAD0ObJxAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==\",\"DbOperation\":4,\"RMSCandidatQualificationID\":null,\"RMSCandidateCityID\":null,\"RMSCandidatQualification\":null,\"RMSCandidateCity\":null,\"RMSPermanentAddr\":null,\"JsGender\":null,\"RMSKeySkill\":null,\"RMSExperience\":0}";
            int retValue = 0;
            //// Convert the dynamic JObject to a DocumentDto object.
            JSLogin _js = jsonData.ToObject<JSLogin>();
            //JSLogin _js = JsonConvert.DeserializeObject<JSLogin>(a);
            if (_js.DbOperation == Convert.ToInt32(DBOperation.Insert))
            {
                object[] values = new object[26];
                values[0] = _js.RMSCandidateName;
                values[1] = _js.JsGender;
                values[2] = _js.RMSDob;
                values[3] = _js.RMSPermanentAddr;
                values[4] = _js.RMSCandidateMobileNo;
                values[5] = _js.RMSCandidateEmailID;
                values[6] = "MOBILE";
                values[7] = 'M';
                values[8] = null;
                values[9] = "";
                values[10] = (_js.RMSCandidateEmailID);// (_js.RMSCandidateEmailID).Split('@')[0].ToString().Trim();
                values[11] = _js.JsPassword;
                values[12] = "CTYGSP0910000599";
                values[13] = "RMQN10043";
                values[14] = null;
                values[15] = null;
                values[16] = _js.RMSKeySkill;
                values[17] = _js.RMSExperience;
                values[18] = "NA";
                values[19] = "NA";
                values[20] = "NA";
                values[21] = "";
                values[22] = _js.ResumeData;//BYTE CONVERTED RESUME
                values[23] = (_js.RMSCandidateEmailID).Split('@')[0].ToString().Trim();
                values[24] = "";//TYPE 
                values[25] = "";
                DataSet ds = new DataSet();
                retValue = _conn.ExecutionMethod("RMSJSResumeDetailsAdd", ref values, DBOperation.ViewAll, ref ds);
            }
            else if (_js.DbOperation == Convert.ToInt32(DBOperation.Update))
            {
                object[] values = new object[10];
                values[0] = _js.RMSResumeID;
                values[1] = _js.RMSCandidateName;
                values[2] = _js.RMSCandidateMobileNo;
                values[3] = _js.ResumeData.Length <= 0 ? null : _js.ResumeData;
                values[4] = _js.RMSCandidateEmailID;
                values[5] = "";
                values[6] = _js.RMSCandidateCityID;
                values[7] = _js.RMSCandidatQualificationID;
                values[8] = _js.RMSPermanentAddr;
                values[9] = _js.ResumeData.Length <= 0 ? 4 : 6;
                DataSet ds = new DataSet();
                retValue = _conn.ExecutionMethod("RMSJSResumeDetailsUpdate_MApp", ref values, DBOperation.ViewAll, ref ds);
            }
            else if (_js.DbOperation == Convert.ToInt32(DBOperation.Delete)) // IT IS USED TO UPDATE FORGOT PASSWORD
            {
                object[] values = new object[10];
                values[0] = "";
                values[1] = "";
                values[2] = "";
                values[3] = null;
                values[4] = _js.RMSCandidateEmailID;
                values[5] = _js.JsPassword;
                values[6] = "";//_js.RMSCandidateCityID;
                values[7] = "";// _js.RMSCandidatQualificationID;
                values[8] = "";
                values[9] = 7;
                DataSet ds = new DataSet();
                retValue = _conn.ExecutionMethod("RMSJSResumeDetailsUpdate_MApp", ref values, DBOperation.ViewAll, ref ds);
            }
            return retValue;
        }
    }
}
