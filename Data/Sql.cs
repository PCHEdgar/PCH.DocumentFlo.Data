using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
  public class Sql : IDataProvider
  {
    private SqlConnection _oConnection = null;

    public Sql()
    {
      string sConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
      try
      {
        _oConnection = new SqlConnection(sConnectionString);
        _oConnection.Open();
      }
      catch(Exception ex)
      {
        throw new Exception("Could not connect to database. Check connection string and connection state.", ex);
      }
      finally
      {
        _oConnection.Close();
      }
    }
    
    public DataTable SPDataTable(string StoredProcedure, Hashtable Args)
    {
      DataTable oData = new DataTable();

      SqlCommand oCommand = new SqlCommand(StoredProcedure, _oConnection);
      oCommand.CommandType = CommandType.StoredProcedure;

      if (Args != null)
      {
        foreach (string key in Args.Keys)
        {
          oCommand.Parameters.AddWithValue(key, Args[key] == null ? DBNull.Value : Args[key]);
        }
      }

      try
      {
        _oConnection.Open();
        using (var da = new SqlDataAdapter(oCommand))
        {
          da.Fill(oData);
        }
      }
      finally
      {
        _oConnection.Close();
      }
      return oData;
    }

    public int SPExec(string StoredProcedure, Hashtable Args)
    {
      object oRetVal;
      return SPExec(StoredProcedure, Args, out oRetVal);
    }

    public int SPExec(string StoredProcedure, Hashtable Args, out object ReturnValue)
    {
      int iRowsAffected = 0;

      SqlCommand oCommand = new SqlCommand(StoredProcedure, _oConnection);
      oCommand.CommandType = CommandType.StoredProcedure;

      SqlParameter oReturnValue = new SqlParameter();
      oReturnValue.Direction = ParameterDirection.ReturnValue;
      oCommand.Parameters.Add(oReturnValue);

      if (Args != null)
      {
        foreach (string key in Args.Keys)
        {
          oCommand.Parameters.AddWithValue(key, Args[key] == null ? DBNull.Value : Args[key]);
        }
      }

      try
      {
        _oConnection.Open();

        iRowsAffected = oCommand.ExecuteNonQuery();

        ReturnValue = oReturnValue.Value;
      }
      finally
      {
        _oConnection.Close();
      }
      return iRowsAffected;
    }
  }
}
