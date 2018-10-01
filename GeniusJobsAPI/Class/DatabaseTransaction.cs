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
                paramreturn.Direction = ParameterDirection.ReturnValue;
                paramreturn.Value = ReturnSuccess;
                cmd.Parameters.Add(paramreturn);


                switch (executiontype)
                {
                    case ExecType.Scalar: // Execute scalar
                        object objexecscalar = cmd.ExecuteScalar();
                        if (objexecscalar != null)
                            datareturn = objexecscalar.ToString();

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
                                break;
                            case ReturnDBOperation.DataTable:
                                ReturnSuccess = 1;
                                DataTable dt = new DataTable();
                                adpNew.Fill(dt);
                                datareturn = dt;
                                break;
                        }
                        
                        break;
                    default:
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adp.Fill(ds);
                        datareturn = ds;
                        break;
                }

                ReturnSuccess = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
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

        /* Kept only for testing purpose
        public int CallStoreProcedure(string SpName, List<SqlParameter> ParamList,ReturnDBOperation RetDBoperation,
            ref DataSet dsResult, ref DataTable dtResult)
        {
            // ref object[] values   
            int ReturnSuccess = 0;
            SqlConnection Con = null;
            string stroutindex = string.Empty;
            string ParamIndex = string.Empty;

            if (!string.IsNullOrEmpty(AddConnectionName))
            {
                Con = new SqlConnection(WebsiteDatabase.ConnectionString(AddConnectionName));
            }

            try
            {
                if (Con.State == ConnectionState.Closed)
                    Con.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SpName;
                cmd.Connection = Con;

                //SqlCommandBuilder.DeriveParameters(cmd);

                //for (int i = 0; i < cmd.Parameters.Count - 1; i++)
                //{
                //    cmd.Parameters[i + 1].Value = values[i];
                //}

                if (ParamList != null)
                {
                    int i = 0;
                    foreach (SqlParameter p in ParamList)
                    {
                        cmd.Parameters.Add(p);

                        //For Output Param
                        stroutindex += (cmd.Parameters[p.ParameterName].Direction == ParameterDirection.Output) || (cmd.Parameters[p.ParameterName].Direction == ParameterDirection.InputOutput) ? p.ParameterName + "," : "";
                        ParamIndex += (cmd.Parameters[p.ParameterName].Direction == ParameterDirection.Output) || (cmd.Parameters[p.ParameterName].Direction == ParameterDirection.InputOutput) ? i + "," : "";
                        i++;
                    }
                }
                cmd.CommandTimeout = 0;


                // for Return parameter
                SqlParameter paramreturn = new SqlParameter();
                paramreturn.ParameterName = "@ReturnValue";
                paramreturn.SqlDbType = SqlDbType.Int;
                paramreturn.Direction = ParameterDirection.ReturnValue;
                paramreturn.Value = ReturnSuccess;
                cmd.Parameters.Add(paramreturn);

                switch (RetDBoperation)
                {
                    case ReturnDBOperation.DataTable:
                        SqlDataAdapter daTable = new SqlDataAdapter(cmd);
                        daTable.Fill(dtResult);
                        ReturnSuccess = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);

                        //For Output Param
                        if (stroutindex != string.Empty)
                        {
                            stroutindex = stroutindex.Substring(0, stroutindex.Length - 1);
                            string[] arroutindex = stroutindex.Split(',');

                            ParamIndex = ParamIndex.Substring(0, ParamIndex.Length - 1);
                            string[] arroutindexParam = ParamIndex.Split(',');

                            if (arroutindex.Length > 0)
                            {
                                for (int j = 0; j < arroutindex.Length; j++)
                                {
                                    ParamList[j].Value = cmd.Parameters[arroutindex[j]].Value.ToString(); //ParamList.Find(p => p.ParameterName.Equals(cmd.Parameters[arroutindex[j]].ParameterName)).Value;
                                    //ParamList.Find(p=>p.ParameterName.Equals(cmd.Parameters[arroutindex[j]].ParameterName))
                                }
                            }

                            //if (arroutindex.Length > 0)
                            //{
                            //    for (int j = 0; j < arroutindex.Length; j++)
                            //    {
                            //        for (int K = 0; K < arroutindexParam.Length; K++)
                            //        {
                            //            if (j == K)
                            //            {
                            //                values[int.Parse(arroutindexParam[K])] = cmd.Parameters[arroutindex[j]].Value.ToString();
                            //                break;
                            //            }
                            //        }
                            //    }
                            //}
                        }

                        Con.Close();
                        break;


                    case ReturnDBOperation.DataSet:
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        ReturnSuccess = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);

                        //For Output Param
                        if (stroutindex != string.Empty)
                        {
                            stroutindex = stroutindex.Substring(0, stroutindex.Length - 1);
                            string[] arroutindex = stroutindex.Split(',');

                            ParamIndex = ParamIndex.Substring(0, ParamIndex.Length - 1);
                            string[] arroutindexParam = ParamIndex.Split(',');

                            if (arroutindex.Length > 0)
                            {
                                for (int j = 0; j < arroutindex.Length; j++)
                                {
                                    for (int K = 0; K < arroutindexParam.Length; K++)
                                    {
                                        if (j == K)
                                        {
                                            values[int.Parse(arroutindexParam[K])] = cmd.Parameters[arroutindex[j]].Value.ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        Con.Close();
                        break;


                    case ReturnDBOperation.InUpDel:
                        cmd.ExecuteNonQuery();
                        ReturnSuccess = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);

                        //For Output Param
                        if (stroutindex != string.Empty)
                        {
                            stroutindex = stroutindex.Substring(0, stroutindex.Length - 1);
                            string[] arroutindex = stroutindex.Split(',');

                            ParamIndex = ParamIndex.Substring(0, ParamIndex.Length - 1);
                            string[] arroutindexParam = ParamIndex.Split(',');

                            if (arroutindex.Length > 0)
                            {
                                for (int j = 0; j < arroutindex.Length; j++)
                                {
                                    for (int K = 0; K < arroutindexParam.Length; K++)
                                    {
                                        if (j == K)
                                        {
                                            values[int.Parse(arroutindexParam[K])] = cmd.Parameters[arroutindex[j]].Value.ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        Con.Close();
                        break;

                }

            }
            catch (Exception ex)
            {
                Logger.Log("Exception Source : " + ex.TargetSite + " Message : " + ex.Message);

            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
            return ReturnSuccess;
        }

        */

    }
}
