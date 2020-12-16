
using System;
using System.Collections.Generic;
using System.Text;

namespace PLMCLRTools
{
    public static class BlockRegister
    {
        public class PorductCodeDescriptionBlock
        {
            public static readonly string PorductCodeDescriptionBlockName = "ucProductCode";
            public static readonly string ProductCode = "PorductCode";
            public static readonly string ProductDescription = "ProductDescription";
            public static readonly string ProductDescription2 = ERPOptionalPushField.Prefix + "ProductDescription2";
        }

        public class ProductQuoteCode
        {
            public static readonly string ProductQuoteCodeBlockName = "ProductQuoteCode";
            public static readonly string QuoteCode = "QuoteCode";
            public static readonly string QuoteDesc = "QuoteDesc";
            public static readonly string QuoteVendor = "QuoteVendor";
        }


        public class DivisionBlock
        {
            public static readonly string DivisionBlockName = "DivisionBlock";
            public static readonly string CompanyDivisionEntity = "CompanyDivision";
        }

        public class VendorRequestTechpackTypeBlock
        {
            public static readonly string VendorRequestTechPackTypeBlockName = "VendorRequestTechPackType";
            public static readonly string TechpackTypeEntity = "TechPackType"; // linked to TeckPackEntity


        }

        public class ProductReferenceIdBlock
        {
            public static readonly string ProductReferenceIdBlockBlockName = "ProductReferenceIdBlock";
            public static readonly string ProductReferenceId = "ProductReferenceId";

        }

        // 197	92	ProductReferenceId	2	2	NULL	NULL	NULL	NULL	ProductReferenceId	NULL	NULL	NULL	NULL	2	NULL	NULL	NULL	NULL	NULL	NULL	NULL	NULL	NULL	NULL	NULL	NULL	NULL	NULL	NULL	NULL	ProductReferenceId	NULL	NULL	NULL	0x0000000009D64D66	NULL	0	NULL	NULL	NULL	0	NULL	NULL	NULL	NULL

        // one is for

        public class ProductClassTypeBlock
        {
            public static readonly string ProductClassTypeBlockName = "ProductClassType";
            public static readonly string CompanyDivisionEntity = "CompanyDivision";
            public static readonly string ProductClassEntity = "ProductClass";
            public static readonly string ProductTypeEntity = ERPOptionalPushField.Prefix + "ProductType";
            //public static readonly string  DefaultWareHouse = "DefaultWareHouse";
            public static readonly string ProductQuoteManagerUser = "ProductQuoteManagerUser";

            public static readonly string ProductTypeGroup = "ProductTypeGroup"; //  Com.Visual2000.Business.BusinessEntity.V2k.Product.ProductClassGroup
        }

        // new v2k  block

        public class ProductWareHouseBlock
        {
            public static readonly string ProductWareHouseBlockName = "ProductWareHouse";
            public static readonly string ProductWareHouse = "ProductWareHouse";
        }

        public class ProductCategoryBlock
        {
            public static readonly string ProductCategoryBlockName = "ProductCategory";
            public static readonly string ProductCategory = "ProductCategory";
        }

        public class ProductStatusBlock
        {
            public static readonly string ProductStatusBlockName = "ucProductStatus";
            public static readonly string ProductStatusEntity = "ucProductStatus";
        }

        public class ProductDesignColorBlock
        {
            public static readonly string ProductDesignColor = "ProductDesignColor";
            public static readonly string ProductDesignColorGrid = "ProductDesignColorGrid";
        }

        public class ProductPriceTypeBlock
        {
            public static readonly string ProductPriceTypeBlockName = "ProductPriceType";

            public static readonly string ProductPriceType = "ProductPriceType";
        }

        public class SellingPeriodCollectionGroupBlock
        {
            public static readonly string SellingPeriodCollectionGroupBlockName = "ucSellingPeriodCollectionGroup";
            public static readonly string SellingPeriodEntity = ERPOptionalPushField.Prefix + "SellingPeriod";
            public static readonly string CollectionEntity = ERPOptionalPushField.Prefix + "Collection";
            public static readonly string GroupEntity = ERPOptionalPushField.Prefix + "Group";
        }

        public class SecurityGroupUserBlock
        {
            public static readonly string SecurityGroupUserBlockName = "SecurityGroupUserBlock";
            public static readonly string SecurityGroupEntity = "SecurityGroupEntity";  // DDL link to  PDMSecurityGroup Entity
            public static readonly string SecurityUserEntity = "SecurityUserEntity";  //  DDL Link to PDMUser
        }

        public class SizeRunBlock
        {
            public static readonly string SizeRunBlockName = "SizeRun";
            public static readonly string SizeRunEntity = "SizeRun";
            public static readonly string SizeDetailDispaly = "SizeDetailDispaly";  // only for display
            public static readonly string SampleSizeDetail = "SampleSizeDetail";  // link to SizeRangeDetail entity
            public static readonly string SizeRunDetailGrid = "SizeRunDetailGrid";  // link to SizeRangeDetail entity
        }

        public class DimensionBlock
        {
            public static readonly string DimensionBlockName = "Dimension";
            public static readonly string DimensionEntity = "Dimension";
            public static readonly string ProductDim = "ProductDim";
            public static readonly string DimensionDetailGrid = "DimensionDetailGrid";  // link to SizeRangeDetail entity
        }

        public class SketchBlock
        {
            public static readonly string SketchBlockName = "ucProductSketch";
            public static readonly string SketchEntity = "Sketch";
        }

        public class SpecFitSampleBlock
        {
            public static readonly string SpecFitSampleBlockName = "SpecFitSampleBlock";

            // new block subitem
            public static readonly string SpecFitMeasureUnit = "SpecFitMeasureUnit";

            public static readonly string SpecFitSizeRun = "SpecFitSizeRun";
            public static readonly string SpecFitBaseSize = "SpecFitBaseSize";
            public static readonly string SpecSelectedSize = "SpecSelectedSize";
            public static readonly string SpecFitGrid = "SpecFitGrid";

            // new added
            public static readonly string SpecSampleSizeRun = "SpecSampleSize"; // will link to tblsizeRunRoate
            //public static readonly string SecondSpecSampleSizeRun = "SecondSpecSampleSize"; // will link to tblsizeRunRoate
            //public static readonly string ThirdSpecSampleSizeRun = "ThirdSpecSampleSize"; // will link to tblsizeRunRoate
            public static readonly string SpecSampleColor = "SpecSampleColor";
            public static readonly string SpecFitStatus = "EmSpecFitStatus";   // need to add CheckBox
        }

        public class SpecFitPPBlock
        {
            public static readonly string SpecFitPPBlockName = "SpecFitPPBlock";

            //  // 160	EmPomUnitType		EmPomUnitType
            public static readonly string SpecFitMeasureUnit = "SpecFitMeasureUnit";

            public static readonly string SpecFitSizeRun = "SpecFitSizeRun";
            public static readonly string SpecFitBaseSize = "SpecFitBaseSize";
            public static readonly string SpecSelectedSize = "SpecSelectedSize";
            public static readonly string SpecFitGrid = "SpecFitGrid";

            // new added
            public static readonly string SpecSampleSizeRun = "SpecSampleSize"; // will link to tblsizeRunRoate
            //public static readonly string SecondSpecSampleSizeRun = "SecondSpecSampleSize"; // will link to tblsizeRunRoate
            //public static readonly string ThirdSpecSampleSizeRun = "ThirdSpecSampleSize"; // will link to tblsizeRunRoate

            public static readonly string SpecSampleColor = "SpecSampleColor";
            // jan 10, 2010
            public static readonly string SpecFitStatus = "EmSpecFitStatus";
        }

        public class SpecFitTopBlock
        {
            public static readonly string SpecFitTopBlockName = "SpecFitTopBlock";

            // new block subitem
            public static readonly string SpecFitMeasureUnit = "SpecFitMeasureUnit";

            public static readonly string SpecFitSizeRun = "SpecFitSizeRun";
            public static readonly string SpecFitBaseSize = "SpecFitBaseSize";
            public static readonly string SpecSelectedSize = "SpecSelectedSize";
            public static readonly string SpecFitGrid = "SpecFitGrid";

            // new added
            public static readonly string SpecSampleSizeRun = "SpecSampleSize"; // will link to tblsizeRunRoate

            //public static readonly string SecondSpecSampleSizeRun = "SecondSpecSampleSize"; // will link to tblsizeRunRoate
            //public static readonly string ThirdSpecSampleSizeRun = "ThirdSpecSampleSize"; // will link to tblsizeRunRoate

            public static readonly string SpecSampleColor = "SpecSampleColor";
            public static readonly string SpecFitStatus = "EmSpecFitStatus";
        }


        // new added blocks

        public class ProductVendorInviteBlock
        {
            public static readonly string ProductVendorInviteBlockName = "ProductVendorInviteBlock";
            public static readonly string ProductVendorInviteGrid = " ProductVendorInviteGrid";

        }

        public class ProductCustomerInviteBlock
        {
            public static readonly string ProductCustomerInviteBlockName = "ProductCustomerInviteBlock";
            public static readonly string ProductCustomerInviteGrid = " ProductCustomerInviteGrid";

        }


        public class ProductAgentInviteBlock
        {
            public static readonly string ProductAgentInviteBlockName = "ProductAgentInviteBlock";
            public static readonly string ProductAgentInviteGrid = " ProductAgentInviteGrid";

        }

        //-----------------

        public class ProductRevisedDateBlock
        {
            public static readonly string ProductRevisedDateBlockName = "ProductRevisedDateBlockName";
            public static readonly string ProductCreatedDate = "ProductCreatedDate";
            public static readonly string ProductRevisedDate = "ProductRevisedDate";
            public static readonly string ProductCreatedBy = "ProductCreatedBy";
            public static readonly string ProductLastRevisedBy = "ProductLastRevisedBy";
        }

        public class SpecGradingBlock
        {
            public static readonly string SpecGradingBlockName = "SpecGradingBlockName";
            public static readonly string SpecGradingQcMeasureUnit = "SpecFitMeasureUnit";
            public static readonly string SpecFitSizeRun = "SpecFitSizeRun";
            public static readonly string SpecFitBaseSize = "SpecFitBaseSize";
            public static readonly string SpecSelectedSize = "SpecSelectedSize";
            public static readonly string SpecGradingGrid = "SpecGradingGrid";
            //public static readonly string SpecFitQCColor = "SpecFitQCColor";
            //will remove
            //  public static readonly string IsGradingBlock = "IsGradingBlock";           //
        }

        public class SpecQCBlock
        {
            public static readonly string SpecQCBlockName = "SpecQCBlockName";
            public static readonly string SpecGradingQcMeasureUnit = "SpecFitMeasureUnit";
            public static readonly string SpecFitSizeRun = "SpecFitSizeRun";
            public static readonly string SpecFitBaseSize = "SpecFitBaseSize";
            public static readonly string SpecSelectedSize = "SpecSelectedSize";
            public static readonly string SpecQCGrid = "SpecQCGrid";
            public static readonly string SpecFitQCColor = "SpecFitQCColor";

            //
        }

        public class ProductUnitOfPackageBlock
        {
            public static readonly string ProductUnitOfPackage = "ProductUnitOfPackage";
            public static readonly string ProductPackageGrid = "ProductPackageGrid";
        }

        public class CompositionBlock
        {
            public static readonly string CompositionBlockName = "ucComposition";
            public static readonly string CompositionEntity = ERPOptionalPushField.Prefix + "ucComposition";
        }

        public class ProductCareInstructionBlock
        {
            public static readonly string ProductCareInstructionBlockName = "ProductCareInstruction";
            public static readonly string ProductCareInstructionEntity = ERPOptionalPushField.Prefix + "ProductCareInstruction";
            public static readonly string ProductCareInstructionDisplay = "ProductCareInstructionDisplay";
        }

        public class ProductNotesBlock
        {
            public static readonly string ProductNotesBlockName = "ProductNotes";
            public static readonly string ProductNotesEntity = ERPOptionalPushField.Prefix + "ProductNotes";
        }

        // made in country
        public class MadeInCountry
        {
            public static readonly string CountryBlockName = "MadeInCountry";
            public static readonly string CountryEntity = ERPOptionalPushField.Prefix + "MadeInCountry";
        }

        // not implemt yet
        public class CountryOfOriginal
        {
            public static readonly string CountryBlockName = "CountryOfOriginal";
            public static readonly string CountryEntity = ERPOptionalPushField.Prefix + "CountryOfOriginal";
        }

        public class CustomerBlock
        {
            public static readonly string CustomerBlockName = "CustomerBlock";
            public static readonly string CustomerEntity = "Customer";
        }

        public class ProductWidthBlock
        {
            public static readonly string ProductWidthBlockName = "ProductWidth";
            public static readonly string RefWidth = "RefWidth";
            public static readonly string RefWidthUnit = "RefWidthUnit"; // drop down list
            public static readonly string Width = ERPOptionalPushField.Prefix + "Width";
            public static readonly string WidthUnit = ERPOptionalPushField.Prefix + "WidthUnit";       // drop down list
        }

        public class ProductHeightBlock
        {
            public static readonly string ProductHeightBlockName = "ProductHeight";
            public static readonly string RefHeight = "RefHeight";
            public static readonly string RefHeightUnit = "RefHeightUnit"; // drop down list
            public static readonly string Height = "Height";
            public static readonly string HeightUnit = "HeightUnit";       // drop down list
        }

        public class ProductLengthBlock
        {
            public static readonly string ProductLengthBlockName = "ProductLength";
            public static readonly string RefLength = "RefLength";
            public static readonly string RefLengthUnit = "RefLengthUnit"; // drop down list
            public static readonly string Length = "Length";
            public static readonly string LengthUnit = "LengthUnit";       // drop down list
        }

        public class ProductWeightBlock
        {
            public static readonly string ProductWeightBlockName = "ProductWeight";
            public static readonly string RefWeight = "RefWeight";
            public static readonly string RefWeightUnit = "RefWeightUnit"; // drop down list
            public static readonly string Weight = ERPOptionalPushField.Prefix + "Weight";
            public static readonly string WeightUnit = ERPOptionalPushField.Prefix + "WeightUnit";       // drop down list
        }

        public class ProductCostBlock
        {
            public static readonly string ProductCostBlockName = "ProductCost";
            public static readonly string RefCost = "RefCost";
            public static readonly string RefCostCurrency = "RefCostCurrency"; // drop down list
            public static readonly string Cost = "Cost";
            public static readonly string CostCurrency = "CostCurrency";       // drop down list
        }

        public class ProductPriceBlock
        {
            public static readonly string ProductPriceBlockName = "ProductPrice";
            public static readonly string RefPrice = "RefPrice";
            public static readonly string RefPriceCurrency = "RefPriceCurrency"; // drop down list
            public static readonly string Price = "Price";
            public static readonly string PriceCurrency = "PriceCurrency";       // drop down list
        }

        public class ProductSuggestRetailBlock
        {
            public static readonly string ProductSuggestRetailBlockName = "ProductSuggestRetail";
            public static readonly string RefSuggestRetail = "RefSuggestRetail";
            public static readonly string RefSuggestRetailCurrency = "RefSuggestRetailCurrency"; // drop down list
            public static readonly string SuggestRetail = "SuggestRetail";
            public static readonly string SuggestRetailCurrency = "SuggestRetailCurrency";       // drop down list
        }

        public class BomComponentBlock
        {
            // blockName

            //public enum EmColorInfor  { Dependent=1, Independent=2 } ;
            public static readonly string BomComponentBlockName = "BomComponent";
            // Bom Subitem
            public static readonly string Description = "Description";
            public static readonly string ComponentTemplate = "ComponentTemplate"; // drop down list

            public static readonly string GridBomComponent = "GridBomComponent";

            public static readonly string ProductColor = "ProductColor";
            public static readonly string ProductDim = "ProductDim";

            public static readonly string ProductSize = "ProductSize";

            public static readonly string ProductPackage = "ProductPackage";
        }

        public class ColorCurrencySettingBlock
        {
            // blockName

            public static readonly string ColorCurrencySettingBlockName = "ColorCurrencySetting";
            public static readonly string ColorInfor = "ColorInfor";    //
            public static readonly string Currency = "Currency"; // drop down list
        }

        public class ProductColorVendorCostBlock
        {
            public static readonly string ProductColorVendorCost = "ProductColorVendorCost";
            public static readonly string ProductColorVendorCostGrid = "ProductColorVendorCostGrid";
        }

        public class ERPProductVendorCostBlock
        {
            public static readonly string ERPProductVendorCost = "ERPProductVendorCost";
            public static readonly string ERPProductVendorCostGrid = "ERPProductVendorCostGrid";
        }

        public class HTSCodesBlock
        {
            public static readonly string HTSCodesBlockName = "HTSCodes";
            public static readonly string HTSCodesGrid = "HTSCodesGrid";
        }

        public class V2kERPProductAttributeBlock
        {
            public static readonly string V2kERPProductAttributeBlockName = "V2kERPProductAttributeBlock";
            public static readonly string V2kERPProductAttributeGrid = "V2kERPProductAttributeGrid";
        }

        public class ProductStyleColorSizePriceBlock
        {
            public static readonly string ProductStyleColorSizePriceBlockName = "ProductStyleColorSizePrice";
            public static readonly string ProductPriceType = "ProductPriceType";
            public static readonly string ProductStyleColorSizePriceGrid = "ProductStyleColorSizePriceGrid";
        }

        public class ProductVendorBlock
        {
            public static readonly string ProductVendorBlockName = "VendorBlock";
            public static readonly string VendorEntity = "Vendor";
        }

        public class LinePlanningStatusBlock
        {
            public static readonly string LinePlanningStatusBlockName = "LinePlanningStatus";
            public static readonly string LinePlanningStatus = "LinePlanningStatus"; // drop down list
        }

        public class LinePlanningItemStatusBlock
        {
            public static readonly string LinePlanningItemStatusBlockName = "LinePlanningItemStatus";
            public static readonly string LinePlanningItemStatus = "LinePlanningItemStatus"; // drop down list
        }

        public class LinePlanningOrigalFromBlock
        {
            public static readonly string LinePlanningOrigalFromBlockName = "LinePlanningOrigalFrom";
            public static readonly string LinePlanningOrigalFrom = "LinePlanningOrigalFrom"; // drop down list
        }

        public class RefStaticFiled
        {
            public static readonly string RefStaticFiledCreatedBy = "RefStaticFiledCreatedBy";
            public static readonly string RefStaticFiledDivRefNumber = "RefStaticFiledDivRefNumber";
            public static readonly string RefStaticFiledLastRevisedBy = "RefStaticFiledLastRevisedBy";
            public static readonly string RefStaticFiledMasterReferenceID = "RefStaticFiledMasterReferenceID";
            public static readonly string RefStaticFiledMerchPlan = "RefStaticFiledMerchPlan";

            public static readonly string RefStaticFiledMerchPlanStatus = "RefStaticFiledMerchPlanStatus";
            public static readonly string RefStaticFiledProductGroup = "RefStaticFiledProductGroup";
            public static readonly string RefStaticFiledProductQuoteCount = "RefStaticFiledProductQuoteCount";
            public static readonly string RefStaticFiledQuoteGroup = "RefStaticFiledQuoteGroup";
            public static readonly string RefStaticFiledQuoteStatus = "RefStaticFiledQuoteStatus";

            public static readonly string RefStaticFiledQuoteTargetReferenceID = "RefStaticFiledQuoteTargetReferenceID";
            public static readonly string RefStaticFiledRefTxType = "RefStaticFiledRefTxType";
            public static readonly string RefStaticFiledSampleGroup = "RefStaticFiledSampleGroup";
            public static readonly string RefStaticFiledSketchGroup = "RefStaticFiledSketchGroup";
            public static readonly string RefStaticFiledTechPackTemplate = "RefStaticFiledTechPackTemplate";

            public static readonly string RefStaticFiledVendorGroup = "RefStaticFiledVendorGroup";
        }

        public class ERPOptionalPushField
        {
            public const string Prefix = "ERPOptionalPushField_";

            public static readonly string BrandLabel = Prefix + "BrandLabel";
            public static readonly string ProductCatalog = Prefix + "ProductCatalog";
            public static readonly string CareInstruction = Prefix + "ProductCareInstruction";

            public static readonly string LabelNote = Prefix + "LabelNote";
            public static readonly string WebDescription1 = Prefix + "WebDescription1";
            public static readonly string WebDescription2 = Prefix + "WebDescription2";

            public static readonly string ShippingDate1 = Prefix + "ShippingDate1";
            public static readonly string ShippingDate2 = Prefix + "ShippingDate2";
            public static readonly string CancelByDate = Prefix + "CancelByDate";

            public static readonly string Weight = Prefix + "Weight";
            public static readonly string WeightUnit = Prefix + "WeightUnit";
            public static readonly string Width = Prefix + "Width";
            public static readonly string WidthUnit = Prefix + "WidthUnit";

            public static readonly string ProductType = Prefix + "ProductType";
            public static readonly string SellingPeriod = Prefix + "SellingPeriod";
            public static readonly string Collection = Prefix + "Collection";
            public static readonly string Group = Prefix + "Group";
            public static readonly string ProductDescription2 = Prefix + "ProductDescription2";

            public static readonly string ProductNotes = Prefix + "ProductNotes";
            public static readonly string Composition = Prefix + "ucComposition";
            public static readonly string CountryOfOriginal = Prefix + "CountryOfOriginal";
            public static readonly string MadeInCountry = Prefix + "MadeInCountry";
        }

        public class ProductFirstStandardCostSellingReailBlock
        {
            public static readonly string ProductStyleColorSizePriceBlockName = "ProductFirstStandardCostSellingReailBlock";
            public static readonly string FirstCost = "FirstCost";
            public static readonly string FirstCostCurrency = "FirstCostCurrency";
            public static readonly string StandardCost = "StandardCost";
            public static readonly string SellingPrice = "SellingPrice";
            public static readonly string Retail = "Retail";
        }


        public class ContentSearchBlock
        {
            public static readonly string ContentSearchBlockName = "ContentSearchBlock";
            public static readonly string Content = "Content";
            public static readonly string FileSize = "FileSize";
        }
    }
}
