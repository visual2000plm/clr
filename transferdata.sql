
CREATE PROC ForeachInsert
(@tableName varchar(100)
) as

--Declare a cursor to retrieve column specific information for the specified table
DECLARE cursCol CURSOR FAST_FORWARD FOR 
SELECT column_name,data_type FROM information_schema.columns WHERE table_name = @tableName and  column_name not in ('RGB','SwatchColor')
OPEN cursCol
DECLARE @string varchar(8000) --for storing the first half of INSERT statement
DECLARE @stringData varchar(4000) --for storing the data (VALUES) related statement
DECLARE @dataType varchar(1000) --data types returned for respective columns
declare @setIdentityOn nvarchar(1000)
declare @setIdentityOff nvarchar(1000)
--declare @setNocheck nvarchar(1000)
--declare @setCheckAll

SET @string='INSERT into '+@tableName+'('
SET @stringData=''

DECLARE @colName nvarchar(50)

FETCH NEXT FROM cursCol INTO @colName,@dataType

IF @@fetch_status<>0
	begin
	print 'Table '+@tableName+' not found, processing skipped.'
	close curscol
	deallocate curscol
	return
END

WHILE @@FETCH_STATUS=0
BEGIN

if(@dataType<>'timestamp')
begin

SET @stringData=@stringData+@colName+','

SET @string=@string+@colName+','
end

FETCH NEXT FROM cursCol INTO @colName,@dataType
END
DECLARE @Query nvarchar(4000)

select @setIdentityOn='SET IDENTITY_INSERT '+ @tableName  +' ON '
select @setIdentityOff='SET IDENTITY_INSERT '+ @tableName  +' off'

SET @query =  @setIdentityOn + '  '+substring(@string,0,len(@string)) + ') select ' + substring(@stringData,0,len(@stringData))+ ' FROM [srv-scorpion].[sr_plms].[dbo].'+@tableName + '   '+ @setIdentityOff
print @query


--exec  sp_executesql @setIdentityOn

BEGIN TRY
exec sp_executesql @query

END TRY
BEGIN CATCH
--print ERROR_MESSAGE()
END CATCH



CLOSE cursCol
DEALLOCATE cursCol




--INSERT into pdmblock(BlockID,Name,CategoryID,CreatedByID,CreatedDate,ModifiedBy,ModifiedDate,ManagementLevel,InternalCode,UseStandardControl,SyncToPDMProduct,Description,ApproveRoleID,NotifyVersionChange,ProductReferenceID,RootCopyBlockID,IsUseDrillDownControl,IsUseByMerchPlan,IsAllowProductTabCopy,IsMerchPlanCopyAble,FolderID,IsMasterSynToChild,IsReferenceStaticFiledControl,IsAllowCopyValueInRefSaveAsAndTabCopy,UserControlName,IsEnableAJAXCascadingDDL,IsForcedCreateFirstVersion,IsSynFromQuoteSampleToProduct,IsUsedForAutoGeneration,SpecialUserDefineType)
-- select BlockID,Name,CategoryID,CreatedByID,CreatedDate,ModifiedBy,ModifiedDate,ManagementLevel,InternalCode,UseStandardControl,SyncToPDMProduct,Description,ApproveRoleID,NotifyVersionChange,ProductReferenceID,RootCopyBlockID,IsUseDrillDownControl,IsUseByMerchPlan,IsAllowProductTabCopy,IsMerchPlanCopyAble,FolderID,IsMasterSynToChild,IsReferenceStaticFiledControl,IsAllowCopyValueInRefSaveAsAndTabCopy,UserControlName,IsEnableAJAXCascadingDDL,IsForcedCreateFirstVersion,IsSynFromQuoteSampleToProduct,IsUsedForAutoGeneration,SpecialUserDefineTy FROM [srv-scorpion].[sr_plms].dbo.pdmblock

SELECT *  FROM information_schema.columns WHERE table_name = 'pdmRGBColor'  and  is_computed <> 1

SELECT * FROM sys.columns
WHERE is_computed = 1
AND object_id = OBJECT_ID('pdmRGBColor')


SELECT * FROM sys.computed_columns
WHERE object_id = OBJECT_ID('YourTableName')