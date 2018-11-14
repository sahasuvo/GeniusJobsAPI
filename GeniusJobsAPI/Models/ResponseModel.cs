using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeniusJobsAPI.Models
{
    public class ResponseModel
    {
        public string responseID { set; get; }
        public string responseText { set; get; }
        public bool responseStatus { set; get; }
        public string responseCode { set; get; }
        public object responseData { set; get; }

        public string image_path { set; get; }

        public string image_file { set; get; }

        public string image_url { set; get; }
    }
}
