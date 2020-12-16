
----- Dataware house Retriuve  Subitem Or GridColumn EntityInfo
go
/****** Object:  StoredProcedure [dbo].[DWRetrieveSubitemOrGridColumnEntityInfo]    Script Date: 06/10/2009 10:21:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DWRetrieveSubitemOrGridColumnEntityInfo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DWRetrieveSubitemOrGridColumnEntityInfo]
go



GO
/****** Object:  StoredProcedure [dbo].[DWRetrieveSubitemOrGridColumnEntityInfo]    Script Date: 06/10/2009 10:22:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[DWRetrieveSubitemOrGridColumnEntityInfo]
	@iSubitemID int, @iGridColumnID  int
AS
begin
	
 
   if( @iSubitemID is not null)
    begin
    SELECT     dbo.pdmEntity.EntityID, dbo.pdmEntity.EntityCode, dbo.pdmEntity.IsSimpleColumn, dbo.pdmUserDefineEntityColumn.ColumnName, 
                      dbo.pdmUserDefineEntityColumn.UserDefineEntityColumnID, dbo.pdmUserDefineEntityColumn.IsIdentity, 
                      dbo.pdmUserDefineEntityColumn.IsPrimaryKey, dbo.pdmUserDefineEntityColumn.MappingValueKey, dbo.pdmEntity.SysTableName, 
                      dbo.pdmBlockSubItem.SubItemName, dbo.pdmBlockSubItem.SubItemID
                     FROM         dbo.pdmEntity INNER JOIN
                     dbo.pdmBlockSubItem ON dbo.pdmEntity.EntityID = dbo.pdmBlockSubItem.EntityID LEFT OUTER JOIN
                     dbo.pdmUserDefineEntityColumn ON dbo.pdmEntity.EntityID = dbo.pdmUserDefineEntityColumn.EntityID
      where  dbo.pdmBlockSubItem.SubItemID = @iSubitemID
    return
    end  


         if( @iGridColumnID is not null)
    begin

   SELECT     dbo.pdmEntity.EntityID, dbo.pdmEntity.EntityCode, dbo.pdmEntity.IsSimpleColumn, dbo.pdmUserDefineEntityColumn.ColumnName, 
                      dbo.pdmUserDefineEntityColumn.UserDefineEntityColumnID, dbo.pdmUserDefineEntityColumn.IsIdentity, 
                      dbo.pdmUserDefineEntityColumn.IsPrimaryKey, dbo.pdmUserDefineEntityColumn.MappingValueKey, dbo.pdmEntity.SysTableName, 
                      dbo.PdmGridMetaColumn.GridColumnID, dbo.PdmGridMetaColumn.ColumnName AS GridColumnName, dbo.PdmGridMetaColumn.GridID
                  FROM         dbo.pdmEntity INNER JOIN
                      dbo.PdmGridMetaColumn ON dbo.pdmEntity.EntityID = dbo.PdmGridMetaColumn.EntityID LEFT OUTER JOIN
                      dbo.pdmUserDefineEntityColumn ON dbo.pdmEntity.EntityID = dbo.pdmUserDefineEntityColumn.EntityID

    
      where  dbo.PdmGridMetaColumn.GridColumnID =@iGridColumnID
    return
    end

  
end

go






------------- Entity Value retrive function

go
/****** Object:  UserDefinedFunction [dbo].[GetIdentitySysDefineTableColumnValue]    Script Date: 06/10/2009 09:53:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetIdentitySysDefineTableColumnValue]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetIdentitySysDefineTableColumnValue]
GO
/****** Object:  UserDefinedFunction [dbo].[GetIdentityUserDefineTableColumnValue]    Script Date: 06/10/2009 09:53:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetIdentityUserDefineTableColumnValue]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetIdentityUserDefineTableColumnValue]
GO
/****** Object:  UserDefinedFunction [dbo].[GetUserDefineTableColumnValue]    Script Date: 06/10/2009 09:53:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserDefineTableColumnValue]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetUserDefineTableColumnValue]
go



/****** Object:  UserDefinedFunction [dbo].[GetIdentitySysDefineTableColumnValue]    Script Date: 06/10/2009 09:53:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetIdentitySysDefineTableColumnValue] (@entityID int)
 RETURNS @ValueList Table
   (   ValueID     Int
    ,  ValueText   nVarChar(4000)
    
   )

AS
Begin
  -- Sys define
   if(@entityID > 3000)
    return
 -- sys define table

	  declare @EntityType int  -- entityType=1 erp,  entityType=2 iis, entitytype 3= System Define Enum type
	  declare @SysTableName nvarchar(200)
	  select  @EntityType=EntityType, @SysTableName=SysTableName from pdmentity where entityid = @entityID


--	 if(  @EntityType=3)--  enum type  
       if( @EntityType=1)
		begin

			 if (@SysTableName='tblproductclass')
                insert into  @ValueList (  ValueID ,ValueText)
				select ProductClassID,name    from  DevlpV2k.dbo.tblproductclass  where name is not null

         
			 if (@SysTableName='tblProductType')
                 insert into  @ValueList (  ValueID ,ValueText)
                 select  ProductType_ID, name    from  DevlpV2k.dbo.tblProductType    where name is not null
    
  
             if (@SysTableName='tblCompanyDivision')
             insert into  @ValueList (  ValueID ,ValueText) 
			 select   CieDivisionID, DivisionCode from DevlpV2k.dbo.tblCompanyDivision  where DivisionCode is not null


            if (@SysTableName='tblCustomer')
               insert into  @ValueList (  ValueID ,ValueText)  
			  select CustomerID , CustomerCode     from  DevlpV2k.dbo.tblCustomer  

			 if (@SysTableName='tblAgent')
               insert into  @ValueList (  ValueID ,ValueText)  
			  select  agent_id, agent_code from  DevlpV2k.dbo.tblAgent 
		

			 if (@SysTableName='tblSellingPeriod')
                 insert into  @ValueList (  ValueID ,ValueText) 
				select SellingPeriod_id ,Description   from DevlpV2k.dbo.tblSellingPeriod  where  Description is not null 


             if (@SysTableName='tblCollection')
               insert into  @ValueList (  ValueID ,ValueText) 
			   select Collection_Id, Description from DevlpV2k.dbo.tblCollection  where  Description is not null 

            if (@SysTableName='tblGroup')
                 insert into  @ValueList (  ValueID ,ValueText) 
				select  Group_ID, Description from DevlpV2k.dbo.tblGroup   where Description is not null



			 if (@SysTableName='tblDimension')
                insert into  @ValueList (  ValueID ,ValueText) 
				select DimensionID,  DimensionCode  from  DevlpV2k.dbo.tblDimension  where   DimensionCode is not null  

 	

			   if (@SysTableName='tblSizeRun')
                insert into  @ValueList (  ValueID ,ValueText) 
				select  SizeRunID, SizeRunCode  from  DevlpV2k.dbo.tblSizeRun  where  SizeRunCode is not null 	

		

			 if (@SysTableName='tblvendor')
              insert into  @ValueList (  ValueID ,ValueText) 
			  select  VendorID, VendorCode   	 from  DevlpV2k.dbo.tblvendor  where   VendorCode is not null  

			if (@SysTableName='tblSystemProductStatusType')
              insert into  @ValueList (  ValueID ,ValueText) 
			  select SystemProductStatusTypeID, SystemTypeCode    from  DevlpV2k.dbo.tblSystemProductStatusType  where   SystemTypeCode is not null

           
			
			if (@SysTableName='tblCountry')
                insert into  @ValueList (  ValueID ,ValueText) 
			  select  Country_Id,  Country_Code   from  DevlpV2k.dbo.tblCountry  
		
	
		        end --- end v2k system define talbe
           if( @EntityType=2)-- pdm table
			begin
		
				
				if (@SysTableName='PdmRGBColor')
					 insert into  @ValueList (  ValueID ,ValueText) 
					  select  RGBColorID, Code      	 from  PdmRGBColor 
				
				if (@SysTableName='pdmsecuritywebuser')
                      insert into  @ValueList (  ValueID ,ValueText)
					  select  UserID,LoginName  from  pdmsecuritywebuser  


 			end

         return 
 
End




GO
/****** Object:  UserDefinedFunction [dbo].[GetIdentityUserDefineTableColumnValue]    Script Date: 06/10/2009 09:53:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetIdentityUserDefineTableColumnValue] (@entityID int)

 RETURNS @ValueList Table

   (   ValueID     Int

    ,  ValueText   nVarChar(4000)

    

   )

 

AS

Begin

  -- Sys define

   if(@entityID < 3000)

    return

 -- user Define

 

 

   declare @IsSmpleColumn bit

    select  @IsSmpleColumn = IsSimpleColumn from pdmentity where entityid=@entityID

     if( @IsSmpleColumn = 1)  -- user Define simple Column

            begin

                   

              insert into  @ValueList (  ValueID ,ValueText)

 

          SELECT dbo.pdmUserDefineEntityValue.RowID AS ValueID, TextValue as  ValueText

          from  dbo.pdmUserDefineEntityValue 

          where  EntityID = @entityID

         return

 

          end  -- end user Define simple Column

     

 

 

 

  -- mutiple Column --

  declare  @valueMapingKey int

 

 SELECT  top 1  @valueMapingKey = MappingValueKey    from pdmUserDefineEntityColumn where EntityID=@entityID and  IsIdentity=1

 

  if( @valueMapingKey is null)

   return

 

 

insert into  @ValueList (  ValueID ,ValueText)

 

 SELECT      dbo.pdmUserDefineEntityValue.RowID AS ValueID,

      case 

       when @valueMapingKey=1 then  Value1

       when @valueMapingKey=2 then  Value2

         when @valueMapingKey=3 then  Value3

         when @valueMapingKey=4 then  Value4

         when @valueMapingKey=5 then  Value5

       when @valueMapingKey=6 then  Value6

       when @valueMapingKey=7 then  Value7

         when @valueMapingKey=8 then  Value8

         when @valueMapingKey=9 then  Value9

         when @valueMapingKey=10 then  Value10 

     

        End as ValueText

     from  dbo.pdmUserDefineEntityValue 

 

     where  EntityID = @entityID

    return

End

 


GO
/****** Object:  UserDefinedFunction [dbo].[GetUserDefineTableColumnValue]    Script Date: 06/10/2009 09:53:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetUserDefineTableColumnValue] (  @columnID int)
 RETURNS @ValueList Table
   (   ValueID     Int
    ,  ValueText   nVarChar(4000)
    
   )

AS
Begin


declare  @entityID int
SELECT  @entityID= EntityID   from pdmUserDefineEntityColumn where UserDefineEntityColumnID=@columnID

  -- Sys define
   if(@entityID < 3000)
    return
 -- user Define


   declare @IsSmpleColumn bit
    select  @IsSmpleColumn = IsSimpleColumn from pdmentity where entityid=@entityID
     if( @IsSmpleColumn = 1)  -- user Define simple Column
		begin
			 
              insert into  @ValueList (  ValueID ,ValueText)

          SELECT dbo.pdmUserDefineEntityValue.RowID AS ValueID, TextValue as  ValueText
          from  dbo.pdmUserDefineEntityValue 
          where  EntityID = @entityID
         return

          end  -- end user Define simple Column
     



  -- mutiple Column --
  declare  @valueMapingKey int
  set   @valueMapingKey =1 
 SELECT @valueMapingKey = MappingValueKey   from pdmUserDefineEntityColumn where UserDefineEntityColumnID=@columnID


insert into  @ValueList (  ValueID ,ValueText)

 SELECT      dbo.pdmUserDefineEntityValue.RowID AS ValueID,
      case 
       when @valueMapingKey=1 then  Value1
       when @valueMapingKey=2 then  Value2
	   when @valueMapingKey=3 then  Value3
	   when @valueMapingKey=4 then  Value4
	   when @valueMapingKey=5 then  Value5
       when @valueMapingKey=6 then  Value6
       when @valueMapingKey=7 then  Value7
	   when @valueMapingKey=8 then  Value8
	   when @valueMapingKey=9 then  Value9
	   when @valueMapingKey=10 then  Value10 
     
	  End as ValueText
     from  dbo.pdmUserDefineEntityValue 

     where  EntityID = @entityID
    return
End
go




