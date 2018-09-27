using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer
{
    public class WebsiteDatabase
    {
        public static string ConnectionString(string AddConnectionName)
        {
            return ConfigurationManager.ConnectionStrings[AddConnectionName].ConnectionString;
        }

    }
}
