using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using GeniusMAppsAPI.Models;

namespace GeniusMAppsAPI.Controllers
{
    public class JobServicesController : ApiController
    {
        string strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ORMConn"].ToString();

        SqlConnection sqlCon = null;
        SqlCommand sqlCmd = null;

        bool _flag = true;
        public int ExecutionMethod(String procName, ref object[] values, DBOperation dOp, ref DataSet dsResult)
        {
            SqlDataReader rdr = null;
            int ReturnSuccess = 0;
            string stroutindex = string.Empty;
            using (sqlCon = new SqlConnection(strConnectionString))
            {
                sqlCmd = new SqlCommand("ProcFindSPParams", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter();
                param = sqlCmd.Parameters.Add("@SPName", SqlDbType.VarChar, 100);
                param.Direction = ParameterDirection.Input;
                param.Value = procName;

                sqlCon.Open();
                rdr = sqlCmd.ExecuteReader();
                sqlCmd = new SqlCommand(procName, sqlCon);

                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandTimeout = 100;
                int i = 0;
                // for input and output parameter
                while (rdr.Read())
                {
                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = rdr["parameter_name"].ToString();
                    param1.SqlDbType = clsGetType(rdr["parameter_type"].ToString());
                    param1.Size = Convert.ToInt32(rdr["max_length"]);
                    param1.Direction = Convert.ToBoolean(rdr["is_output"]) == true ? ParameterDirection.InputOutput : ParameterDirection.Input;
                    param1.Value = values[i];
                    sqlCmd.Parameters.Add(param1);
                    stroutindex += Convert.ToBoolean(rdr["is_output"]) == true ? rdr["parameter_id"].ToString() + "," : "";
                    i++;
                }
                sqlCon.Close();
                // for Return parameter
                SqlParameter paramreturn = new SqlParameter();
                paramreturn.ParameterName = "@ReturnValue";
                paramreturn.SqlDbType = SqlDbType.Int;
                paramreturn.Direction = ParameterDirection.ReturnValue;
                paramreturn.Value = ReturnSuccess;
                sqlCmd.Parameters.Add(paramreturn);

                switch (dOp)
                {
                    case DBOperation.ViewAll:
                    case DBOperation.ViewSingle:
                        try
                        {
                            SqlDataAdapter DA = new SqlDataAdapter();
                            DA.SelectCommand = sqlCmd;
                            DA.Fill(dsResult);
                        }
                        catch (SqlException ex) //JS Register Sql Exception raise for duplicacy
                        {
                            switch (ex.Message)
                            {
                                case "E":
                                    _flag = false;
                                    ReturnSuccess = -1;
                                    break;
                                case "U":
                                    _flag = false;
                                    ReturnSuccess = -2;
                                    break;
                            }
                        }
                        break;
                    case DBOperation.Insert:
                    case DBOperation.Update:
                    case DBOperation.Delete:
                        try
                        {
                            sqlCon.Open();
                            sqlCmd.ExecuteNonQuery();
                        }
                        catch (SqlException ex) //JS Register Sql Exception raise for duplicacy
                        {
                            switch (ex.Message)
                            {                               
                                case "E":
                                    _flag = false;
                                    ReturnSuccess = -1;
                                    break;
                                case "U":
                                    _flag = false;
                                    ReturnSuccess = -2;
                                    break;
                            }
                        }
                        break;
                }
                if (stroutindex.Length > 0)
                {
                    stroutindex = stroutindex.Substring(0, stroutindex.Length - 1);
                    string[] arroutindex = stroutindex.Split(',');
                    if (arroutindex.Length > 0)
                        for (int j = 0; j < arroutindex.Length; j++)
                            values[Convert.ToInt32(arroutindex[j].ToString()) - 1]
                                = sqlCmd.Parameters[Convert.ToInt32(arroutindex[j].ToString()) - 1].Value;
                }
                if (_flag == true)
                    ReturnSuccess = Convert.ToInt32(sqlCmd.Parameters["@ReturnValue"].Value);
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
            }
            return ReturnSuccess;
        }

        private SqlDbType clsGetType(string type)
        {
            SqlDbType objDBtype = new SqlDbType();
            switch (type)
            {
                case "bit":
                    objDBtype = SqlDbType.Bit;
                    break;
                case "int":
                    objDBtype = SqlDbType.Int;
                    break;
                case "varchar":
                    objDBtype = SqlDbType.VarChar;
                    break;
                case "bigint":
                    objDBtype = SqlDbType.BigInt;
                    break;
                case "PayrollKeyID":
                    objDBtype = SqlDbType.BigInt;
                    break;
                case "datetime":
                    objDBtype = SqlDbType.DateTime;
                    break;
                case "float":
                    objDBtype = SqlDbType.Float;
                    break;
                case "char":
                    objDBtype = SqlDbType.Char;
                    break;
                case "nvarchar":
                    objDBtype = SqlDbType.NVarChar;
                    break;
                case "decimal":
                    objDBtype = SqlDbType.Decimal;
                    break;
                case "varbinary":
                    objDBtype = SqlDbType.VarBinary;
                    break;
                default:
                    objDBtype = SqlDbType.VarChar;
                    break;
            }
            return objDBtype;
        }

    }
}