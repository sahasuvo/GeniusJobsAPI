using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GeneralLayer
{
    public class CodeHelper : Master
    {

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public string Base64Decode(string base64EncodedData)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string Base64Encode(string plainText)
        {
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return System.Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool IsValidDatee(string date, out DateTime ultimateDate)
        {
            ultimateDate = DateTime.Now;
            if (!string.IsNullOrEmpty(date))
            {
                try
                {
                    char[] seperator = new char[] { '/' };
                    string[] data = date.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
                    string day = data[0];
                    string month = data[1];
                    string yr = data[2];

                    ultimateDate = DateTime.Parse(month + "/" + day + "/" + yr);
                    return true;
                }
                catch (Exception)
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "as", "alert('Enter Correct Date');", true);
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "as", "alert('Enter date in correct format');", true);
            }
            return false;
        }
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public void PopulateYear(ref DropDownList ddlYear, string Type)
        {
            switch (Type)
            {
                case "All":
                    ddlYear.Items.Add(new ListItem("All", "%"));
                    break;
                case "Select":
                    ddlYear.Items.Add(new ListItem("Select", "-1"));
                    break;
            }

            for (int i = MinYear; i <= MaxYear; i++)
            {
                ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }
    }
}
