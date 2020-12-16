using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{
    public partial class PdmEntityBlClrDto 
    {

     

        public PdmUserDefineEntityColumnClrDto   SystemDefinePrimaryKeyColumnName
        {
            get
            {
                return this.PdmUserDefineEntityColumnList.Where(o => o.IsPrimaryKey == true).FirstOrDefault();



            }
        }


        public List<PdmUserDefineEntityColumnClrDto> SystemDefineDisplayColumnNames
        {
            get
            {
                return this.PdmUserDefineEntityColumnList.Where(o => !string.IsNullOrEmpty(o.SystemTableColumnName)).ToList(); ;



            }
        }


        public List<PdmUserDefineEntityColumnClrDto> UserDefineDisplayColumnNames
        {
            get
            {
                return this.PdmUserDefineEntityColumnList.Where(o => o.UsedByDropDownList.HasValue && o.UsedByDropDownList.Value).OrderBy(o => o.DataRowSort).ToList(); ;



            }
        }

        //public string QuerySysDefineColumnIdAndDisplay
        //{
        //    get
        //    //		aPdmEntityBlEntity.QuerySysDefineColumnIdAndDisplay	"CieDivisionID as Id, ( CieDivisionID' | 'DivisionCode )  as Display  from tblCompanyDivision"	string
        //    {
        //        //select UserID,  ( LoginName  + ' | '+ UserName + ' | ' + Email ) as Display  from pdmSecurityWebUser 
        //        string splitToken = "+' | '+";

        //        string aselectIdQuery = string.Empty;
        //        if (SystemDefinePrimaryKeyColumnName != null)
        //        {
        //            aselectIdQuery = "select " + SystemDefinePrimaryKeyColumnName.SystemTableColumnName + " as Id, ";
        //        }

        //        string aDisplay = GetDisplayColumn(splitToken);

        //        if (!string.IsNullOrEmpty(aselectIdQuery) && SystemDefineDisplayColumnNames.Count > 1)
        //        {
        //            return aselectIdQuery + "( " + aDisplay + " )  as Display " + " from " + this.SysTableName + " order by " + SystemDefineDisplayColumnNames[0].SystemTableColumnName;



        //        }
        //        else
        //        {
        //            return string.Empty;

        //        }




        //    }
        //}

        //private string GetDisplayColumn(string splitToken)
        //{
        //    string aDisplay = string.Empty;
        //    string orderby = string.Empty;


        //    foreach (PdmUserDefineEntityColumnClrDto aPdmUserDefineEntityColumnClrDto in SystemDefineDisplayColumnNames)
        //    {
        //        if (aPdmUserDefineEntityColumnClrDto.IsPrimaryKey != true)
        //            aDisplay += " IsNull( " + aPdmUserDefineEntityColumnClrDto.SystemTableColumnName + " , '' )" + splitToken;

        //    }

        //    if (aDisplay != string.Empty)
        //    {

        //        aDisplay = aDisplay.Substring(0, aDisplay.Length - splitToken.Length);

        //    }
        //    return aDisplay;
        //}


		        
    }
}

