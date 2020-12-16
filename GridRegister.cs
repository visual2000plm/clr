
using System;
using System.Collections.Generic;
using System.Text;

namespace PLMCLRTools
{
    public static class GridRegister
    {
        public class GridSpecFit
        {
            //------------------------- SpecFitGrid-----------
            public static readonly string SpecFitGrid = "SpecFitGrid";

            // body part will be reused by spec grading
            public readonly static string BodyPartDetailIDWDimDetailID = "BodyPartDetailIDWDimDetailID";
            public readonly static string BodyPartCustomerCode = "BodyPartCustomerCode";
            public readonly static string BodyPartName = "BodyPartName";
            public readonly static string BodyPartDesc = "BodyPartDesc";
            public readonly static string HowToMeasure = "HowToMeasure";
            //public readonly static string  Comments = "Comments";
            public readonly static string AddDesc = "Add.Desc";
            //  public readonly static string IsDimension = "IsDimension";
            public readonly static string SpecDimensionDetail = "SpecDimensionDetail";
            public readonly static string SpecDimension = "SpecDimension";
            public readonly static string SpecIsNeedToApplyGradingRule = "SpecIsNeedToApplyGradingRule";

            public readonly static string Tolerance = "Tolerance";

            #region---------Base  Size Fit

            public readonly static string Difference = "Difference";

            public readonly static string InitiaSpec = "InitiaSpec";   // no need it at all ?

            public readonly static string SampleInitiaSpec = "SampleInitiaSpec";  //GradingSizeValue Get from Grding Value


            public readonly static string Sample = "Sample";
            public readonly static string DiffSample = "DiffSample";
            public readonly static string Revise = "Revise";

            public readonly static string FinalSpec = "FinalSpec";



            public readonly static string Sample11 = "Sample11";        // Before wash
            public readonly static string Sample1 = "Sample1";          // After wash ( default)
            public readonly static string DiffSample1 = "DiffSample1";  //// shringkage = (sample11 - sample1) / sample11
            public readonly static string Revise1 = "Revise1";          // Revised!
            public readonly static string Difference1 = "Difference1";   // Sample1 - InitaSampleGrading
            public readonly static string Difference11 = "Difference11";  // (Before wash Diffrence: Sample11 - InitaSampleGrading)

            //   sample11 = 0.0;
            //   sample1 = 0.0;
            //  "DiffSample1"
            //  if (sample11 == 0 || sample1 == 0)
            //    return;

            //if (sample11 > 0)
            //{
            //    double diffValue = Math.Round((sample11 - sample1) / sample11, 4);
            //    diffCell.Value = (diffValue * 100).ToString() + "%";
            //}

            public readonly static string Sample22 = "Sample22";
            public readonly static string Sample2 = "Sample2";
            public readonly static string DiffSample2 = "DiffSample2";
            public readonly static string Revise2 = "Revise2";
            public readonly static string Difference2 = "Difference2";
            public readonly static string Difference22 = "Difference22";

            public readonly static string Sample33 = "Sample33";
            public readonly static string Sample3 = "Sample3";
            public readonly static string DiffSample3 = "DiffSample3";
            public readonly static string Revise3 = "Revise3";
            public readonly static string Difference3 = "Difference3";
            public readonly static string Difference33 = "Difference33";

            public readonly static string Sample44 = "Sample44";
            public readonly static string Sample4 = "Sample4";
            public readonly static string DiffSample4 = "DiffSample4";
            public readonly static string Revise4 = "Revise4";
            public readonly static string Difference4 = "Difference4";
            public readonly static string Difference44 = "Difference44";

            public readonly static string Sample55 = "Sample55";
            public readonly static string Sample5 = "Sample5";
            public readonly static string DiffSample5 = "DiffSample5";
            public readonly static string Revise5 = "Revise5";
            public readonly static string Difference5 = "Difference5";
            public readonly static string Difference55 = "Difference55";

            public readonly static string Sample66 = "Sample66";
            public readonly static string Sample6 = "Sample6";
            public readonly static string DiffSample6 = "DiffSample6";
            public readonly static string Revise6 = "Revise6";
            public readonly static string Difference6 = "Difference6";
            public readonly static string Difference66 = "Difference66";

         

            #endregion


            //public readonly static string Third = "Third";
            //public readonly static string Second = "Second";

            public readonly static string Comment = "Comment";

            //  public readonly static string Sort = "Sort";
            public readonly static string CriticalPoint = "CriticalPoint";
        }

        public class GridSpecGrading
        {
            public static readonly string SpecGradingGrid = "SpecGradingGrid";
            // virtual

            public readonly static string BodyPartDetailIDWDimDetailID = "BodyPartDetailIDWDimDetailID";
            public readonly static string BodyPartCustomerCode = "BodyPartCustomerCode";
            public readonly static string BodyPartName = "BodyPartName";
            public readonly static string BodyPartDesc = "BodyPartDesc";
            public readonly static string HowToMeasure = "HowToMeasure";
            public readonly static string Tolerance = "Tolerance";
            public readonly static string AddDesc = "Add.Desc";
            // public readonly static string IsDimension = "IsDimension";
            public readonly static string SpecDimensionDetail = "SpecDimensionDetail";
            public readonly static string SpecDimension = "SpecDimension";
            public readonly static string SpecIsNeedToApplyGradingRule = "SpecIsNeedToApplyGradingRule";

            // virtual constantance
            //
            // public readonly static string Difference = "Difference";
            public readonly static string GradingSize = "GradingSize";
            //  public readonly static string Comment = "Comment";

            public readonly static string CriticalPoint = "CriticalPoint";

            public static readonly string GradingBaseSize = "GradingBaseSize";
            public static readonly string GradingSize1 = "GradingSize1";
            public static readonly string GradingSize2 = "GradingSize2";
            public static readonly string GradingSize3 = "GradingSize3";
            public static readonly string GradingSize4 = "GradingSize4";
            public static readonly string GradingSize5 = "GradingSize5";
            public static readonly string GradingSize6 = "GradingSize6";
            public static readonly string GradingSize7 = "GradingSize7";
            public static readonly string GradingSize8 = "GradingSize8";
            public static readonly string GradingSize9 = "GradingSize9";
            public static readonly string GradingSize10 = "GradingSize10";
            public static readonly string GradingSize11 = "GradingSize11";
            public static readonly string GradingSize12 = "GradingSize12";
            public static readonly string GradingSize13 = "GradingSize13";
            public static readonly string GradingSize14 = "GradingSize14";
            public static readonly string GradingSize15 = "GradingSize15";
            public static readonly string GradingSize16 = "GradingSize16";
            public static readonly string GradingSize17 = "GradingSize17";
            public static readonly string GradingSize18 = "GradingSize18";
            public static readonly string GradingSize19 = "GradingSize19";
            public static readonly string GradingSize20 = "GradingSize20";
            public static readonly string Commtents = "Commtents";
        }

        public class GridSpecQC
        {
            public static readonly string SpecQCGrid = "SpecQCGrid";
            // virtual

            public readonly static string BodyPartDetailIDWDimDetailID = "BodyPartDetailIDWDimDetailID";
            public readonly static string BodyPartCustomerCode = "BodyPartCustomerCode";
            public readonly static string BodyPartName = "BodyPartName";
            public readonly static string BodyPartDesc = "BodyPartDesc";
            public readonly static string HowToMeasure = "HowToMeasure";
            public readonly static string Tolerance = "Tolerance";
            public readonly static string AddDesc = "Add.Desc";
            //  public readonly static string IsDimension = "IsDimension";
            public readonly static string SpecDimensionDetail = "SpecDimensionDetail";
            public readonly static string SpecDimension = "SpecDimension";
            public readonly static string SpecIsNeedToApplyGradingRule = "SpecIsNeedToApplyGradingRule";


            // virtual constantance
            public readonly static string Difference = "Difference";
            public readonly static string QCSize = "QCSize";
            public readonly static string GradingSize = "GradingSize";

            public readonly static string CriticalPoint = "CriticalPoint";


            public static readonly string GradingBaseSize = "GradingBaseSize";
            public static readonly string GradingSize1 = "GradingSize1";
            public static readonly string GradingSize2 = "GradingSize2";
            public static readonly string GradingSize3 = "GradingSize3";
            public static readonly string GradingSize4 = "GradingSize4";
            public static readonly string GradingSize5 = "GradingSize5";
            public static readonly string GradingSize6 = "GradingSize6";
            public static readonly string GradingSize7 = "GradingSize7";
            public static readonly string GradingSize8 = "GradingSize8";
            public static readonly string GradingSize9 = "GradingSize9";
            public static readonly string GradingSize10 = "GradingSize10";
            public static readonly string GradingSize11 = "GradingSize11";
            public static readonly string GradingSize12 = "GradingSize12";
            public static readonly string GradingSize13 = "GradingSize13";
            public static readonly string GradingSize14 = "GradingSize14";
            public static readonly string GradingSize15 = "GradingSize15";
            public static readonly string GradingSize16 = "GradingSize16";
            public static readonly string GradingSize17 = "GradingSize17";
            public static readonly string GradingSize18 = "GradingSize18";
            public static readonly string GradingSize19 = "GradingSize19";
            public static readonly string GradingSize20 = "GradingSize20";
            // public static readonly string GradingComments	=				   "GradingComments" ;

            // QC						   // QC

            public static readonly string QCSize1 = "QCSize1";
            public static readonly string QCSize2 = "QCSize2";
            public static readonly string QCSize3 = "QCSize3";
            public static readonly string QCSize4 = "QCSize4";
            public static readonly string QCSize5 = "QCSize5";
            public static readonly string QCSize6 = "QCSize6";
            public static readonly string QCSize7 = "QCSize7";
            public static readonly string QCSize8 = "QCSize8";
            public static readonly string QCSize9 = "QCSize9";
            public static readonly string QCSize10 = "QCSize10";
            public static readonly string QCSize11 = "QCSize11";
            public static readonly string QCSize12 = "QCSize12";
            public static readonly string QCSize13 = "QCSize13";
            public static readonly string QCSize14 = "QCSize14";
            public static readonly string QCSize15 = "QCSize15";
            public static readonly string QCSize16 = "QCSize16";
            public static readonly string QCSize17 = "QCSize17";
            public static readonly string QCSize18 = "QCSize18";
            public static readonly string QCSize19 = "QCSize19";
            public static readonly string QCSize20 = "QCSize20";

            public static readonly string Difference1 = "Difference1";
            public static readonly string Difference2 = "Difference2";
            public static readonly string Difference3 = "Difference3";
            public static readonly string Difference4 = "ifference4";
            public static readonly string Difference5 = "Difference5";
            public static readonly string Difference6 = "Difference6";
            public static readonly string Difference7 = "Difference7";
            public static readonly string Difference8 = "Difference8";
            public static readonly string Difference9 = "ifference9";
            public static readonly string Difference10 = "Difference10";
            public static readonly string Difference11 = "Difference11";
            public static readonly string Difference12 = "Difference12";
            public static readonly string Difference13 = "Difference13";
            public static readonly string Difference14 = "Difference14";
            public static readonly string Difference15 = "Difference15";
            public static readonly string Difference16 = "Difference16";
            public static readonly string Difference17 = "Difference17";
            public static readonly string Difference18 = "Difference18";
            public static readonly string Difference19 = "Difference19";
            public static readonly string Difference20 = "Difference20";
            public static readonly string Commtents = "Commtents";
            //    public readonly static string Sort = "Sort";
        }

        public class GridProductWareHouse
        {
            public static readonly string ProductWarehouseGrid = "ProductWarehouseGrid";
            public static readonly string ProductWarehouseID = "ProductWarehouseID";  // dropdown list
            public static readonly string WarehouseDesc = "WarehouseDesc";

        }

        public class GirdProductCatagory
        {
            public static readonly string ProductCatagoryGrid = "ProductCatagoryGrid";
            public static readonly string ProductCategoryID = "ProductCategoryID";  // dropdown list
            public static readonly string SystemTypeName1 = "SystemTypeName1";
            public static readonly string SystemTypeName2 = "SystemTypeName2";

        }


        public class GridProductVendorInvite
        {

            public static readonly string ProductVendorInviteGrid = " ProductVendorInviteGrid";
            public static readonly string VendorID = "VendorID";// drop down list to Vednor

        }


        public class GridProductCustomerInvite
        {

            public static readonly string ProductCustomerInviteGrid = " ProducCustomerInviteGrid";
            public static readonly string CustomerID = "CustomerID";// drop down list to Customers

        }

        public class GridProductAgentInvite
        {

            public static readonly string ProductAgentInviteGrid = " ProducAgentInviteGrid";
            public static readonly string AgentID = "AgentID";// drop down list to Customers

        }




        public class GridProductPackage
        {
            public static readonly string ProductPackageGrid = "ProductPackageGrid";
            public static readonly string ProductPackageGridPackageID = "PackageID";  // dropdown list
            public static readonly string ProductPackageGridVolumn = "Volumn";
            public static readonly string ProductPackageGridWeight = "Weight";
            public static readonly string ProductPackageGridUnitofmeasure = "UnitOfMeasureID";
            public static readonly string ProductPackageGridWidth = "Width";
            public static readonly string ProductPackageGridHeight = "Height";
            public static readonly string ProductPackageGridLength = "Length";
            public static readonly string ProductPackageGridIsDefualt = "IsDefualt"; //ckeckBox
            public static readonly string ProductPackageGridDigit = "Digit";
            public static readonly string ProductPackageGridSort = "Sort";
            //virtual
            public static readonly string ProductPackageGridDesc = "Description1";
            public static readonly string ProductPackageGridQuantity = "Quantity";
        }

        public class GridProductDesignColor
        {
            public static readonly string ProductDesignColorGrid = "ProductDesignColorGrid";
            public static readonly string ProductDesignColorGridColorID = "ColorID";  // dropdown list apping Color Colde

            // new added
            public static readonly string Name = "Name";
            // public static readonly string NRFCode = "NRFCode";
            // public static readonly string NRFGroup = "NRFGroup";

            public static readonly string ColorNRfid = "ColorNRfid";

            public static readonly string Description = "Description";
            public static readonly string RGB = "RGB";
            public static readonly string SwatchColor = "SwatchColor";

            public static readonly string Image = "Image";

            public static readonly string Approve = "Approve";
            public static readonly string ApprovDate = "ApprovDate";

            public static readonly string Code = "Code";
            public static readonly string SellingCurrency = "SellingCurrency";
            public static readonly string SupplierColor = "SupplierColor";
            public static readonly string EffectiveDate = "EffectiveDate";

            // Color user define  Fileds
            public static readonly string Userdefine = "Userdefine";
            public static readonly string Userdefine1 = "Userdefine1";
            public static readonly string Userdefine2 = "Userdefine2";
            public static readonly string Userdefine3 = "Userdefine3";
            public static readonly string Userdefine4 = "Userdefine4";
            public static readonly string Userdefine5 = "Userdefine5";
            public static readonly string Userdefine6 = "Userdefine6";
            public static readonly string Userdefine7 = "Userdefine7";
            public static readonly string Userdefine8 = "Userdefine8";
            public static readonly string Userdefine9 = "Userdefine9";
            public static readonly string Userdefine10 = "Userdefine10";
            public static readonly string Userdefine11 = "Userdefine11";
            public static readonly string Userdefine12 = "Userdefine12";
            public static readonly string Userdefine13 = "Userdefine13";
            public static readonly string Userdefine14 = "Userdefine14";
            public static readonly string Userdefine15 = "Userdefine15";

            public static readonly string SketchId = "SketchId";
            public static readonly string ColorReferenceTypeID = "ColorReferenceTypeID";
            public static readonly string ReferenceCode = "ReferenceCode";
            public static readonly string ReferenceName = "ReferenceName";
            public static readonly string ColorFamilyID = "ColorFamilyID";

            public static readonly string FirstCost = "FirstCost";
            public static readonly string FirstCostCurrency = "FirstCostCurrency";
            public static readonly string StandardCost = "StandardCost";
            public static readonly string SellingPrice = "SellingPrice";
            public static readonly string Retail = "Retail";
            public static readonly string ProductSpecificColorNRF = "ProductSpecificColorNRF";


        }

        public class GridProductSizeBased
        {
            public static readonly string ProductDesignSizeGrid = "ProductSizeBasedGrid";
            public static readonly string SizePrefix = "Size";

            // Size Code  Fileds
            public static readonly string Size1 = "Size1";
            public static readonly string Size2 = "Size2";
            public static readonly string Size3 = "Size3";
            public static readonly string Size4 = "Size4";
            public static readonly string Size5 = "Size5";
            public static readonly string Size6 = "Size6";
            public static readonly string Size7 = "Size7";
            public static readonly string Size8 = "Size8";
            public static readonly string Size9 = "Size9";
            public static readonly string Size10 = "Size10";
            public static readonly string Size11 = "Size11";
            public static readonly string Size12 = "Size12";
            public static readonly string Size13 = "Size13";
            public static readonly string Size14 = "Size14";
            public static readonly string Size15 = "Size15";
            public static readonly string Size16 = "Size16";
            public static readonly string Size17 = "Size17";
            public static readonly string Size18 = "Size18";
            public static readonly string Size19 = "Size19";
            public static readonly string Size20 = "Size20";

            // Size user define  Fileds
            public static readonly string Userdefine = "Userdefine";

            public static readonly string Userdefine1 = "Userdefine1";
            public static readonly string Userdefine2 = "Userdefine2";
            public static readonly string Userdefine3 = "Userdefine3";
            public static readonly string Userdefine4 = "Userdefine4";
            public static readonly string Userdefine5 = "Userdefine5";
            public static readonly string Userdefine6 = "Userdefine6";
            public static readonly string Userdefine7 = "Userdefine7";
            public static readonly string Userdefine8 = "Userdefine8";
            public static readonly string Userdefine9 = "Userdefine9";
            public static readonly string Userdefine10 = "Userdefine10";
        }

        public class GridProductColorVendorCost
        {
            public static readonly string ProductColorVendorCostGrid = "ProductColorVendorCostGrid";
            public static readonly string Color = "Color";
            public static readonly string Vendor = "Vendor";
            public static readonly string VendorProductCode = "VendorProductCode";
            public static readonly string VendorProductColor = "VendorProductColor";
            public static readonly string Manufacturer = "Manufacturer";

            // CheckBox
            public static readonly string PreferedVendor = "PreferedVendor";
            public static readonly string WaitDays = "WaitDays";
            public static readonly string AdditionalInfo = "AdditionalInfo";
            public static readonly string MinOrderQty = "MinOrderQty";
            public static readonly string UnitOfPackage = "UnitOfPackage";
            public static readonly string FirstCurrency = "FirstCurrency";
            public static readonly string ExchangeRate = "ExchangeRate";
            public static readonly string FirstCost = "FirstCost";
            public static readonly string CostType = "CostType";

            public static readonly string AdditionalCost = "AdditionalCost";

            // column nams is 'Standard Cost'
            public static readonly string TotalCost = "TotalCost";
            public static readonly string Status = "Status";
        }

        public class GridERPProductVendorCost
        {
            public static readonly string ERPProductVendorCostGrid = "ERPProductVendorCostGrid";

            // need hide this column   for vendor quote
            // public static readonly string Vendor = "Vendor";

            // get from  vendor quote warehouse ( need to override
            /// public static readonly string WareHouse = "WareHouse";

            // get from     target reference    Category
            // public static readonly string Category = "Category";

            //Dimension drop down list  get from     target reference    Category

            // get from target reference

            //// Vendor

            public static readonly string Vendor = "Vendor";

            // whs

            public static readonly string Whs = "Whs";

            // Cat
            public static readonly string Cat = "Cat";

            public static readonly string Color = "Color";

            public static readonly string Dimension = "Dim";
            // Size drop downlist
            public static readonly string Size = "Size";
            // dropdown list     from    taget ref
            public static readonly string UnitOfPackage = "UnitOfPackage";

            public static readonly string FirstCost = "FirstCost";
            public static readonly string CostType = "CostType";

            //
            public static readonly string StandCost = "StandCost";

            //
            public static readonly string CurrentCost = "CurrentCost";

            public static readonly string Comments = "Comments";

            public static readonly string FirstCurrency = "FirstCurrency";

            public static readonly string IsDefault = "IsDefault";

            public static readonly string IsApproved = "IsApproved";

            //Color
            //Dim
            //Size
            //UnitOfPackage
            //FirstCost
            //CostType
            //StandCost
            //CurrentCost
            //Comments
        }

        public class GridBomComponent
        {
            //GridBomComponent
            public static readonly string BomGridComponent = "BomGridComponent";
            public static readonly string Component = "Component"; // ddl
            public static readonly string RefNo = "RefNo";           // text

            // TextType, the value could be PDMRefereceID, or ERP ProductID( manully load Entity by ResoucType)
            public static readonly string BomItemID = "BomItemID";
            public static readonly string BomItemSourceFrom = "BomItemSourceFrom"; //valueid: 1:ERP 2:PDM
            public static readonly string Division = "Division";
            public static readonly string ProductClass = "ProductClass";
            public static readonly string ProductType = "ProductType";
            public static readonly string WareHouse = "WareHouse";
            public static readonly string Category = "Category";
            public static readonly string ProductCode = "ProductCode";
            public static readonly string Description = "Description";// virtual
            public static readonly string BomItemColor = "BomItemColor";
            public static readonly string FinishGoodsColor = "FinishGoodsColor"; // hidden this Color
            public static readonly string Vendor = "Vendor"; // hidden this Color
            public static readonly string Dimension = "Dimension"; // drop  Demension detail

            public static readonly string Size = "Size";            // size roate id    detail

            public static readonly string UnitoOfPackage = "UnitoOfPackage";  // drop
            public static readonly string Qty = "Qty";
            public static readonly string UnitCost = "UnitCost";
            public static readonly string Cost = "Cost";
            public static readonly string Placement = "Placement";

            public static readonly string ProductNotes = "ProductNotes";

            public static readonly string Composition = "Composition";

            public static readonly string Comments = "Comments";

            // new added size...
            public static readonly string ProductSize = "ProductSize"; // finished goods size...
            public static readonly string CalQty = "CalQty"; // finished goods size...
            //public static readonly string ExtensionCost = "ExtensionCost"; // finished goods size...

            //need to add com
        }

        public class GridHTSCodes//HTSCodesGrid
        {
            public static readonly string HTSGrid = "HTSCodesGrid";
            public static readonly string HTSCode = "HTSCode";
            public static readonly string Desc1 = "Desc1";
            public static readonly string CountryID = "CountryID";
            public static readonly string CountryExportID = "CountryExportID";
            public static readonly string Composition = "Composition";
            public static readonly string IsDefaultImport = "IsDefaultImport";
            public static readonly string Rate = "Rate";
        }

        public class V2kERPProductAttribute//HTSCodesGrid
        {
            public static readonly string V2kERPProductAttributeGrid = "V2kERPProductAttributeGrid";
            public static readonly string ColorID = "ColorID"; // link CurrentFK Ref ProductColor drop
            public static readonly string Code = "Code"; // DDL binding to TemplateAttributeDetail
            public static readonly string Name = "Name"; // Entity DepdenColumn
            public static readonly string Description = "Description"; //Free Text ;
            public static readonly string SketchID = "SketchID"; // Image Type
        }

        //aProductAttributeDetail.ColorID = aRow["ColorID"] == System.DBNull.Value ? SqlInt32.Null : (Int32)aRow["ColorID"];
        //  aProductAttributeDetail.Name = aRow["Name"] == System.DBNull.Value ? SqlString.Null : (string)aRow["Name"];
        //  aProductAttributeDetail.Code = aRow["Code"] == System.DBNull.Value ? SqlString.Null : (string)aRow["Code"];
        //  aProductAttributeDetail.Description = aRow["Description"] == System.DBNull.Value ? SqlString.Null : (string)aRow["Description"];
        //  aProductAttributeDetail.SketchID = aRow["SketchID"] == System.DBNull.Value ? SqlInt32.Null : (Int32)aRow["SketchID"];

        public class ProductBasedGrid
        {
            public static readonly string ProductReferenceIDColumn = "ProductReferenceID";
            public static readonly string ProductReferenceCodeColumn = "ProductReferenceCode";
        }

        public class ProductStyleColorSizePrice
        {
            public static readonly string ProductStyleColorSizePriceGrid = "ProductStyleColorSizePriceGrid";
            public static readonly string ProductColorID = "ProductColorID";
            public static readonly string ProductSizeID = "ProductSizeID";
            public static readonly string Price = "Price";
        }


        public class RangePLanningAssortmentGrid
        {


            // concanation id string 11 |22 |33 | 44
            public static readonly string AssortmentConceptReferenceColumn = "AssortmentConceptReference";

            // concanation id string 1000 |1000 |1000 | 1000
            public static readonly string AssortmentCarryOverReferenceColumn = "AssortmentCarryOverReference";

            public static readonly string AssortmentCurrentReferenceColumn = "AssortmentCurrentReference";


            public static readonly string AssortmentConceptReferenceCountColumn = "AssortmentConceptReferenceCount";

            // concanation id string 1000 |1000 |1000 | 1000
            public static readonly string AssortmentCarryOverReferenceCountColumn = "AssortmentCarryOverReferenceCount";

            public static readonly string AssortmentCurrentReferenceCountColumn = "AssortmentCurrentReferenceCount";

        }


        public class GirdSizeRunDetailGrid
        {
            public static readonly string SizeRunDetailGrid = "SizeRunDetailGrid";

            public static readonly string SizeRunDetail = "ProductSizeRunDetail";

        }


        public class GridDimensionDetailGrid
        {
            public static readonly string DimensionDetailGrid = "DimensionDetailGrid";

            public static readonly string DimensionDetail = "ProductDimensionDetail";

        }

        public class SystemDefinePrintGrid
        {
            public static readonly string SystemDefinePrintSelectedRow = "SystemDefinePrintSelectedRow";
            public static readonly string SystemDefinePrintRequestBy = "SystemDefinePrintRequestBy";
            public static readonly string SystemDefinePrintRequestDate = "SystemDefinePrintRequestDate";

        }
    }
}
