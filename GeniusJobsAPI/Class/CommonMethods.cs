using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GeniusJobsAPI.Class
{
    public class CommonMethods
    {
        public List<dynamic> DatatableToList(DataTable dt)
        {
            //var dt = new DataTable();

            var dns = new List<dynamic>();

            foreach (var item in dt.AsEnumerable())
            {
                // Expando objects are IDictionary<string, object>
                IDictionary<string, object> dn = new Dictionary<string, object>();//new ExpandoObject();

                foreach (var column in dt.Columns.Cast<DataColumn>())
                {
                    dn[column.ColumnName] = item[column];
                }

                dns.Add(dn);
            }

            return dns;
        }
    }
}
