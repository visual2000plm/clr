using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;

namespace PLMCLRTools
{ 
    public partial class PdmMassUpdateViewDal 
    {
        					
        public static  string QueryAll = @" SELECT  MassUpdateViewID ,  Name ,  Description ,  MainTabID ,  GridBlockID ,  GridID ,  UpdateType ,  IsUsedForSearch ,  IsActive ,  FreezeNumber ,  SystemTimeStamp ,  TechPackTypeID   FROM [dbo].[pdmMassUpdateView] ";
			
		public static List< PdmMassUpdateViewClrDto> GetAllList(SqlConnection conn)
        {
            List<PdmMassUpdateViewClrDto> listDto = new List<PdmMassUpdateViewClrDto>();
           
                DataTable entityDataTable = DataAcessHelper.GetDataTableQueryResult(conn, QueryAll);
                foreach (DataRow row in entityDataTable.Rows)
                {
                    listDto.Add(PdmMassUpdateViewConverter.ConvertDataRowDto(row));
                }
           
      
            return listDto;
        }
		        
    }
}

 