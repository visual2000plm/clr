using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Linq;
using System.Collections.Generic;

namespace PLMCLRTools
{


    public partial class SysDefinBlockHelper
    {

        // Key: InternalCode, Value: subitemID
        public static Dictionary<string,int>  GetDictInteralCodeSubItemId(SqlConnection conn, string blockInterCode)
        {
            string queryString = @" SELECT     pdmBlockSubItem.SubItemID, pdmBlockSubItem.InternalCode 
                      FROM         pdmBlockSubItem INNER JOIN
                      pdmBlock ON pdmBlockSubItem.BlockID = pdmBlock.BlockID 
                      where  pdmBlock.InternalCode =@blockInterCode  and pdmBlockSubItem.BlockID <3000";

            SqlParameter parablockInterCode = new SqlParameter("@blockInterCode", blockInterCode);
            
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.Parameters.Add(parablockInterCode);

            return DataAcessHelper.GetDataTableQueryResult(cmd).AsDataRowEnumerable()
                .ToDictionary(row => row["InternalCode"].ToString(), row => (int)row["SubItemID"]);
       

          
        }

      
    }



}

