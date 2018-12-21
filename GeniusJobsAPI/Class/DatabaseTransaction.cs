using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DatabaseAccessLayer
{
    public enum ExecType
    {
        Scalar = 1, // Scalar Data
        Dynamic =2 // Dynamic Data
    }
    public enum ReturnDBOperation
    {
        DataTable = 1, //Return Datatableand Return value
        DataSet = 2,//Return Datatable and Return value
        InUpDel = 3 //Return Insert ,Update,Delete
    }
    public class DatabaseTransaction
    {
        internal SqlConnection conn;
        public string AddConnectionName { get; set; }
       
        dynamic datareturn { get; set; }
        public dynamic SqlGetData(String StrStoredProcedure, ref List<KeyValuePair<object,object>> ParamList, ExecType executiontype, ReturnDBOperation RetDBoperation, ref int? ReturnSuccess)
        {
            ReturnSuccess = 0;
            
            datareturn = null;
            try
            {
                if (!string.IsNullOrEmpty(AddConnectionName))
                {
                    conn = new SqlConnection(ConfigurationManager.ConnectionStrings[AddConnectionName].ConnectionString);
                    //new SqlConnection(WebsiteDatabase.ConnectionString(AddConnectionName));
                    //conn = new SqlConnection(AddConnectionName);
                }
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                SqlCommand cmd = new SqlCommand(StrStoredProcedure, conn);

                cmd.CommandType = CommandType.StoredProcedure;

                SqlCommandBuilder.DeriveParameters(cmd);

                if (ParamList != null && ParamList.Count > 0)
                {
                    for (int i = 0; i < cmd.Parameters.Count; i++)
                    {
                        if(ParamList.Exists(p=>p.Key.Equals(cmd.Parameters[i].ParameterName)))
                        {
                            cmd.Parameters[i].Value = ParamList.Find(p => p.Key.Equals(cmd.Parameters[i].ParameterName)).Value;
                        }
                    }
                }

                // for Return parameter
                SqlParameter paramreturn = new SqlParameter();
                paramreturn.ParameterName = "@ReturnValue";
                paramreturn.SqlDbType = SqlDbType.Int;
                //paramreturn.SqlDbType = SqlDbType.Char;
                paramreturn.Direction = ParameterDirection.ReturnValue;
                paramreturn.Value = ReturnSuccess;
                cmd.Parameters.Add(paramreturn);


                switch (executiontype)
                {
                    case ExecType.Scalar: // Execute scalar
                        object objexecscalar = cmd.ExecuteScalar();
                        if (objexecscalar != null)
                            datareturn = objexecscalar.ToString();

                            ReturnSuccess = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
                        break;
                    case ExecType.Dynamic:
                        SqlDataAdapter adpNew = new SqlDataAdapter(cmd);
                        switch (RetDBoperation)
                        {
                            case ReturnDBOperation.DataSet:
                                ReturnSuccess = 1;
                                DataSet objds = new DataSet();
                                adpNew.Fill(objds);
                                datareturn = objds;
                                ReturnSuccess = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
                                break;
                            case ReturnDBOperation.DataTable:
                                ReturnSuccess = 1;
                                DataTable dt = new DataTable();
                                adpNew.Fill(dt);
                                datareturn = dt;
                                ReturnSuccess = Convert.ToInt32(cmd.Parameters["@RETURN_VALUE"].Value);
                                //ReturnSuccess = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
                                break;
                            case ReturnDBOperation.InUpDel:
                                ReturnSuccess = 1;
                                try
                                {
                                    cmd.ExecuteNonQuery();
                                    ReturnSuccess = Convert.ToInt32(cmd.Parameters["@RETURN_VALUE"].Value); //Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
                                    datareturn = ReturnSuccess;
                                }
                                catch (Exception ex)
                                {
                                    ReturnSuccess = -1;
                                    throw ex;
                                }
                                finally
                                {

                                }

                                //if (ParamList != null && ParamList.Count > 0)
                                //{
                                //    for (int i = 0; i < cmd.Parameters.Count; i++)
                                //    {
                                //        if (ParamList.Exists(p => p.Key.Equals(cmd.Parameters[i].ParameterName)))
                                //        {
                                //            if (cmd.Parameters[i].Direction == ParameterDirection.InputOutput)
                                //            {
                                //                ParamList[i].Key. = "";
                                //                ParamList.Find(p => p.Key.Equals(cmd.Parameters[i].ParameterName)).Value = cmd.Parameters[i].Value;
                                //            }
                                //            //cmd.Parameters[i].Value = ParamList.Find(p => p.Key.Equals(cmd.Parameters[i].ParameterName)).Value;
                                //        }
                                //    }
                                //}
                                //DataTable dt = new DataTable();
                                //adpNew.Fill(dt);
                                //datareturn = dt;
                                break;
                        }
                        break;
                    default:
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adp.Fill(ds);
                        datareturn = ds;
                        ReturnSuccess = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
                        break;
                }

                
            }
            catch(Exception ex)
            {
                Logger.Log("Exception Source : " + ex.TargetSite + " Message : " + ex.Message);
                //ReturnSuccess = ex.GetHashCode();
                //datareturn = "Exception Source : " + ex.TargetSite + " Message : " + ex.Message;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

            if (conn.State == ConnectionState.Open)
                conn.Close();
            return datareturn;
        }

    }
}
