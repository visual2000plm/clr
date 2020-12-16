
using System;
using System.Collections.Generic;
using System.Text;

namespace PLMCLRTools
{
    public enum EmSpecialColumnType
    {
        RegularColumn = 1,

        PointToExternalDCUDataSourceKeyColumn = 2,

        ProductGridSimpleDCU = 3,
        ProductGridKeyDCUColumn = 4,

        CurrentRefSimpleDCU = 5,

        CurrentRefKeyDCUColumn = 6,

        DynamicMatrixKeyColumn = 7,

        ForeignKeyDependentColumn = 8,

        UnknownTypeColumn = 20,
    }
    public enum EmExChangeMode
    {
        OneWay = 1,
        TwoWay = 2,
    }

    public enum EmExChangeActionType { New = 1, Modified = 2, Deleted = 3, NoChange = 4 };

    public enum EmControlType
    {
        CheckBox = 13,

        Date = 7,

        DDL = 1,

        Empty = 17,

        File = 9,

        Grid = 6,

        Image = 5,

        Label = 10,

        Memo = 4,

        TextBox = 2,

        Numeric = 20,

        AutoGeneration = 23,

        RGBColorDisplay = 24,

        InvalidControlType = 99,

        Video = 25,
    }

    public enum EmGridType
    {
        RegularGrid = 1,
        ProductBasedGrid = 2,
        DynamicMatrixGrid = 3,
        LinePlanningGrid = 4,
        RangePLanningGrid = 5,
    }

    public enum EmEntityType
    {
        SystemDefineTable = 1,

        //  PDMtable = 2,
        PDMEnum = 3,

        UserDefineTable = 4,
        RelationFKEntity = 5,
        RelationAssociation = 6
    }

    public enum EmSortDirection
    {
        Asc = 1,
        Desc = 2,  
     
    }

    public enum EmEntityCode
    {
         
        ProductClass = 1,

         
        ProductType = 2,

         
        CompanyDivision = 3,

         
        Collection = 4,

         
        Group = 5,

         
        SellingPeriod = 6,

         
        Dimension = 7,

         
        SizeRun = 8,

         
        Sketch = 9,

         
        Composition = 10,

        //[EnumMember]
        //Color,
        //[EnumMember]
        //BomTemplate,
         
        BodyPart = 11,

         
        BodyType = 12,

         
        Vendor = 13,

         
        SystemProductStatusType = 14,

        //[EnumMember]
        //Fabric,
        //[EnumMember]
        //V2kProduct,
        //[EnumMember]
        //StitchPoint,
        //[EnumMember]
        //SewInstruction,
         
        Employee = 15,

         
        Agent = 16,

         
        Country = 17,

        //[EnumMember]
        //SewStitchTemplate,
        //[EnumMember]
        //CustomerStandard,
         
        BodyTypeGroup = 18,

         
        SizeRunDetail = 19,

         
        Customer = 20,

        //[EnumMember]
        //CareInstruction,
         
        ContentLabel = 21,

         
        UnitOfMeasure = 22,

         
        Currency = 23,

         
        CompositionType = 24,

         
        Fiber = 25,

         
        CieWarehouse = 26,

         
        DimentionDetail = 27,

         
        Package = 28,

        //[EnumMember]
        //SystemSubjectDetail,
         
        CompanySetup = 29,

         
        HTS = 30,

         
        PdmProductColor = 31,

         
        RGBColor = 32,

         
        PDMUser = 33,

         
        ProductManagerUser = 34,

         
        ProductSize = 35,

         
        ProductDim = 36,

        //[EnumMember]
        //EmPriceByType,
         
        ProductPackage = 37,

         
        Component = 38,

         
        ProductWarehouse = 39,

         
        ProductCategory = 40,

         
        ReferenceTemplate = 41,

        //[EnumMember]
        //EmSpecFitStatus,
         
        TemplateAttributeDetail = 42,
         
        V2kLabel = 43,

         
        PDMTab = 44,

         
        ProductClassGroup = 45,

         
        PDMSecurityGroup = 46,

         
        MassUpdateView = 47,

         
        PDMBlock = 48,

         
        PDMDocumentView = 49,

         
        PDMEntity = 50,

         
        PDMGrid = 51,

         
        PDMItem = 52,

         
        PDMLanguage = 53,

         
        PDMListMenu = 54,

         
        PDMLPTemplate = 55,

         
        PDMNotification = 56,

         
        PDMPrintJob = 57,

         
        PDMProduct = 58,

         
        PDMReferenceView = 59,

         
        PDMReportWebPublish = 60,

         
        PDMSearchTemplate = 61,

         
        PDMSecurityControlorList = 62,

         
        PDMSecurityPermission = 63,

         
        PDMSecurityRegDomain = 64,

         
        PDMSEFolder = 65,

         
        PDMSetup = 66,

         
        PDMTemplateTabLibReferenceSetting = 67,

         
        PDMV2kPrefixType = 68,

         
        PDMBlockSubItem = 69,

         
        PDMGridMetaColumn = 70,

         
        PDMTAActivity = 71,

         
        PDMTAPhase = 72,

         
        PDMTASeverity = 73,

         
        PDMTACalendar = 74,

         
        PDMTAProject = 75,

         
        PDMBLQuery = 76,

         
        TechPackType = 77,

         
        PdmBLMethod = 78,

         
        PDMOCRFileType = 79,

         
        RGBColorFamily = 90,

         
        RGBColorReferenceType = 91,

         
        ColorNRF = 92,

         
        CareInstruction = 93,

         
        CareInstructionLabel = 94,

         
        ProductCatalog = 95,

         
        PdmStoreProcSetting = 96,
    }

}
