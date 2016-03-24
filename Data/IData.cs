using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
  public interface IDataProvider
  {
    DataTable SPDataTable(string StoredProcedure, Hashtable Args);

    DataSet SPDataSet(string StoredProcedure, Hashtable Args);

    int SPExec(string StoredProcedure, Hashtable Args);

    int SPExec(string StoredProcedure, Hashtable Args, out object ReturnValue);
  }
}
