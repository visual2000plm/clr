using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Data;
//using WSLib.ServiceReference1;



//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Data.SqlTypes;
//using Microsoft.SqlServer.Server;
//using System.Collections.Generic;
//using System.Linq;


//http://www.webservicex.net/WS/wscatlist.aspx
//http://www.webservicex.net/sunsetriseservice.asmx?WSDL
//The TRUSTWORTHY database property is used to indicate whether the instance of SQL Server trusts the database and the contents within it
//ALTER DATABASE PLMS_Devlp SET TRUSTWORTHY ON
//http://www.codeproject.com/Articles/21149/Invoking-a-WCF-Service-from-a-CLR-Trigger
//-- Turn advanced options on
//EXEC sp_configure 'show advanced options' , '1';
//go
//reconfigure;
//go
//EXEC sp_configure 'clr enabled' , '1'
//go
//reconfigure;
//-- Turn advanced options back off
//EXEC sp_configure 'show advanced options' , '0';
//go

//use custdb
//ALTER DATABASE custdb SET TRUSTWORTHY ON
//reconfigure

// select * from sys.dm_clr_properties

//select * from sys.dm_clr_appdomains
//select * from sys.dm_clr_loaded_assemblies
// select * from sys.dm_clr_tasks
//http://social.msdn.microsoft.com/Forums/en-US/ssdt/thread/c086b200-e0cf-4407-a1da-42b7ab38be1f/
//We don't expose a way to directly add a service reference to a database project through the IDE. 

//Your best approach is to add a C# class library project to your solution as your service client. 
// Create your service reference from that class library project, then add a reference from your database project containing your SQL CLR stored proc to the class library containing the service reference.


namespace PLMCLRTools
{
  
}
