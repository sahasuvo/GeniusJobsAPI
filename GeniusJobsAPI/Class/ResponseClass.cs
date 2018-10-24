using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeniusJobsAPI.Class
{
    public class ResponseClass
    {
        public int ResponseCode { get; set; }
        public String ResponseStatus { get; set; }
        //public String ResponseData { get; set; }
        public List<dynamic> ResponseData { get; set; }
    }
}