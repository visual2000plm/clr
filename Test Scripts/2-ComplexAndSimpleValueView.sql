

GO
/****** Object:  View [dbo].[PLM_DW_ComplexColumnValue]    Script Date: 07/30/2009 14:55:01 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[PLM_DW_ComplexColumnValue]'))
DROP VIEW [dbo].[PLM_DW_ComplexColumnValue]

GO
/****** Object:  View [dbo].[PLM_DW_SimpleDCUCopyTabValue]    Script Date: 07/30/2009 14:55:09 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[PLM_DW_SimpleDCUCopyTabValue]'))
DROP VIEW [dbo].[PLM_DW_SimpleDCUCopyTabValue]

GO
/****** Object:  View [dbo].[PLM_DW_SimpleDCUValue]    Script Date: 07/30/2009 14:55:30 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[PLM_DW_SimpleDCUValue]'))
DROP VIEW [dbo].[PLM_DW_SimpleDCUValue]
GO
/****** Object:  View [dbo].[PLM_DW_ComplexColumnCopyTabValue]    Script Date: 07/30/2009 14:56:07 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[PLM_DW_ComplexColumnCopyTabValue]'))
DROP VIEW [dbo].[PLM_DW_ComplexColumnCopyTabValue]
go


GO
CREATE VIEW [dbo].[PLM_DW_SimpleDCUValue]
AS
SELECT DISTINCT 
                      dbo.pdmTabField.TabID, dbo.pdmProductBlockMaxVersion.ProductReferenceID, dbo.pdmBlockSubItemValue.SubItemID, 
                      CASE WHEN ValueID IS NOT NULL THEN CAST(ValueID AS varchar) WHEN ValueDate IS NOT NULL THEN CAST(ValueDate AS varchar) 
                      ELSE ValueText END AS ValueText, dbo.pdmItem.BlockID
FROM         dbo.pdmProductBlockMaxVersion INNER JOIN
                      dbo.pdmBlockSubItemValue ON dbo.pdmProductBlockMaxVersion.VersionID = dbo.pdmBlockSubItemValue.VersionID INNER JOIN
                      dbo.pdmBlockSubItem ON dbo.pdmBlockSubItemValue.SubItemID = dbo.pdmBlockSubItem.SubItemID INNER JOIN
                      dbo.pdmItem ON dbo.pdmProductBlockMaxVersion.BlockID = dbo.pdmItem.BlockID INNER JOIN
                      dbo.pdmTabField ON dbo.pdmItem.FieldID = dbo.pdmTabField.FieldID INNER JOIN
                      dbo.pdmTab ON dbo.pdmTabField.TabID = dbo.pdmTab.TabID
WHERE     (dbo.pdmTab.ProductCopyTabRootTabID IS NULL) AND (dbo.pdmTab.ProductReferenceID IS NULL)



GO
CREATE VIEW [dbo].[PLM_DW_SimpleDCUCopyTabValue]
AS
SELECT DISTINCT 
                      dbo.pdmTabField.TabID, dbo.pdmProductBlockMaxVersion.ProductReferenceID, dbo.pdmBlockSubItemValue.SubItemID, 
                      CASE WHEN ValueID IS NOT NULL THEN CAST(ValueID AS varchar) WHEN ValueDate IS NOT NULL THEN CAST(ValueDate AS varchar) 
                      ELSE ValueText END AS ValueText, dbo.pdmItem.BlockID
FROM         dbo.pdmProductBlockMaxVersion INNER JOIN
                      dbo.pdmBlockSubItemValue ON dbo.pdmProductBlockMaxVersion.VersionID = dbo.pdmBlockSubItemValue.VersionID INNER JOIN
                      dbo.pdmBlockSubItem ON dbo.pdmBlockSubItemValue.SubItemID = dbo.pdmBlockSubItem.SubItemID INNER JOIN
                      dbo.pdmItem ON dbo.pdmProductBlockMaxVersion.BlockID = dbo.pdmItem.BlockID INNER JOIN
                      dbo.pdmTabField ON dbo.pdmItem.FieldID = dbo.pdmTabField.FieldID INNER JOIN
                      dbo.pdmTab ON dbo.pdmTabField.TabID = dbo.pdmTab.TabID AND 
                      dbo.pdmProductBlockMaxVersion.ProductReferenceID = dbo.pdmTab.ProductReferenceID

GO

CREATE VIEW [dbo].[PLM_DW_ComplexColumnValue]
AS
SELECT DISTINCT 
                      dbo.pdmTabField.TabID, dbo.pdmProductBlockMaxVersion.ProductReferenceID, dbo.pdmProductBlockMaxVersion.BlockID, 
                      dbo.pdmGridProductRow.GridID, dbo.pdmGridProductValue.RowID, dbo.pdmGridProductValue.GridColumnID, 
                      CASE WHEN dbo.pdmGridProductValue.ValueID IS NOT NULL THEN CAST(dbo.pdmGridProductValue.ValueID AS varchar) 
                      WHEN dbo.pdmGridProductValue.ValueDate IS NOT NULL THEN CAST(dbo.pdmGridProductValue.ValueDate AS varchar) 
                      ELSE dbo.pdmGridProductValue.ValueText END AS ValueText
FROM         dbo.pdmProductBlockMaxVersion INNER JOIN
                      dbo.pdmGridProductRow ON dbo.pdmProductBlockMaxVersion.VersionID = dbo.pdmGridProductRow.VersionID INNER JOIN
                      dbo.pdmGridProductValue ON dbo.pdmGridProductRow.RowID = dbo.pdmGridProductValue.RowID INNER JOIN
                      dbo.pdmItem ON dbo.pdmProductBlockMaxVersion.BlockID = dbo.pdmItem.BlockID INNER JOIN
                      dbo.pdmTabField ON dbo.pdmItem.FieldID = dbo.pdmTabField.FieldID INNER JOIN
                      dbo.pdmTab ON dbo.pdmTabField.TabID = dbo.pdmTab.TabID
WHERE     (dbo.pdmTab.ProductCopyTabRootTabID IS NULL) AND (dbo.pdmTab.ProductReferenceID IS NULL)



GO
CREATE VIEW [dbo].[PLM_DW_ComplexColumnCopyTabValue]
AS
SELECT DISTINCT 
                      dbo.pdmTabField.TabID, dbo.pdmProductBlockMaxVersion.ProductReferenceID, dbo.pdmProductBlockMaxVersion.BlockID, 
                      dbo.pdmGridProductRow.GridID, dbo.pdmGridProductValue.RowID, dbo.pdmGridProductValue.GridColumnID, 
                      CASE WHEN dbo.pdmGridProductValue.ValueID IS NOT NULL THEN CAST(dbo.pdmGridProductValue.ValueID AS varchar) 
                      WHEN dbo.pdmGridProductValue.ValueDate IS NOT NULL THEN CAST(dbo.pdmGridProductValue.ValueDate AS varchar) 
                      ELSE dbo.pdmGridProductValue.ValueText END AS ValueText
FROM         dbo.pdmProductBlockMaxVersion INNER JOIN
                      dbo.pdmGridProductRow ON dbo.pdmProductBlockMaxVersion.VersionID = dbo.pdmGridProductRow.VersionID INNER JOIN
                      dbo.pdmGridProductValue ON dbo.pdmGridProductRow.RowID = dbo.pdmGridProductValue.RowID INNER JOIN
                      dbo.pdmItem ON dbo.pdmProductBlockMaxVersion.BlockID = dbo.pdmItem.BlockID INNER JOIN
                      dbo.pdmTabField ON dbo.pdmItem.FieldID = dbo.pdmTabField.FieldID INNER JOIN
                      dbo.pdmTab ON dbo.pdmTabField.TabID = dbo.pdmTab.TabID AND 
                      dbo.pdmProductBlockMaxVersion.ProductReferenceID = dbo.pdmTab.ProductReferenceID
WHERE     (dbo.pdmTab.ProductCopyTabRootTabID IS NOT NULL)

GO