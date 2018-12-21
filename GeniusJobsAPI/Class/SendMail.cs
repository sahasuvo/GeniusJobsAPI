using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MailSetting
{
    public class MailSend
    {
        public String mailSubject { get; set; }
        public String mailBody { get; set; }
        public String mailTo { get; set; }
        public String mailCC { get; set; }
        public String mailBcc { get; set; }
        public String mailFrom { get; set; }
        public String mailFromName { get; set; }
        public String[] mailAttachment { get; set; }

        // Mail Server Settings
        private string MailServer { get; set; }
        private Int32 MailPort { get; set; }
        private string NCEmail { get; set; }
        private string NCPassword { get; set; }
        private string NCBCCEmail { get; set; }
        private string NCFromEmail { get; set; }
        private Int32 SSLEnable { get; set; }

        Int32 Return = 0;
        public MailSend()
        {
            MailServer = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            MailPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
            NCEmail = ConfigurationManager.AppSettings["NCEmail"].ToString();
            NCPassword = ConfigurationManager.AppSettings["NCPassword"].ToString();
            NCBCCEmail = ConfigurationManager.AppSettings["NCBCCEmail"].ToString();
            SSLEnable = Convert.ToInt32(ConfigurationManager.AppSettings["SSLEnable"].ToString());
            NCFromEmail = ConfigurationManager.AppSettings["NCFromEmail"].ToString();
        }

        public Int32 SendMail()
        {
            MailMessage myMail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient();
            try
            {
                //Mail Config
                mailFrom = string.IsNullOrEmpty(mailFrom) ? NCFromEmail : mailFrom;
                mailBcc = string.IsNullOrEmpty(mailBcc) ? NCBCCEmail : mailBcc;
                myMail.To.Add(mailTo.Replace(";", ","));
                if (!string.IsNullOrEmpty(mailCC))
                {
                    myMail.CC.Add(mailCC.Replace(";", ","));
                }

                if (!string.IsNullOrEmpty(mailBcc))
                {
                    myMail.Bcc.Add(mailBcc.Replace(";", ","));
                }
                myMail.From = new MailAddress(mailFrom, string.IsNullOrEmpty(mailFromName) ? "Email" : mailFromName);
                
                myMail.Subject = mailSubject;
                //myMail.ReplyTo = new MailAddress(mailFrom);
                myMail.ReplyToList.Add(mailFrom);

                if (mailAttachment != null && mailAttachment.Length > 0)
                {
                    for (int i = 0; i < mailAttachment.Length; i++)
                    {
                        myMail.Attachments.Add(new Attachment(mailAttachment[i].ToString()));
                    }
                }

                myMail.IsBodyHtml = true;
                myMail.Body = mailBody; //set the body message

                //Server Settings
                SmtpServer.Host = MailServer; 
                SmtpServer.Port = MailPort; 
                //SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(NCEmail, NCPassword);
                if (!string.IsNullOrEmpty(mailTo)) {
                    SmtpServer.Send(myMail); 
                }
                Return = 7;
            }
            catch 
            {
                Return = -7;
            }
            finally
            {
                myMail.Attachments.Dispose();
                myMail.Dispose();
                SmtpServer.Dispose();
            }
            return Return;
        }
    }
}
