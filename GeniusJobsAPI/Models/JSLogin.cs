using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeniusJobsAPI.Models
{
    public class JSLogin
    {
        public string JsUsername { get; set; }
        public string JsPassword { get; set; }
        public string JsGender { get; set; }
        public string RMSUserID { get; set; }
        public string RMSResumeID { get; set; }
        public string RMSDob { get; set; }
        public string RMSCandidateName { get; set; }
        public string RMSCandidateMobileNo { get; set; }
        public string RMSCandidateEmailID { get; set; }
        public byte[] ResumeData { get; set; }
        public int DbOperation { get; set; }
        public string RMSCandidatQualificationID { get; set; }
        public string RMSCandidateCityID { get; set; }
        public string RMSCandidatQualification { get; set; }
        public string RMSCandidateCity { get; set; }
        public string RMSPermanentAddr { get; set; }
        public string RMSKeySkill { get; set; }
        public int RMSExperience { get; set; }

    }
}
