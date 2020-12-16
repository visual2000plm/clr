using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;

namespace PLMCLRTools
{
    public class PLMSGetGridSpecialColumn
    {
        public static void GetAllGridSpecailColumnIds(PdmGridClrDto blockPdmGridDto, List<int> allGridRefGridBlockIds, List<int> allGridRefGridColumnIds, List<int> allGridRefSimpleDcuIds)
        {

           // List<PdmGridMetaColumnClrDto> selectedBlockColumnListDto = blockPdmGridDto.PdmGridMetaColumnList;

            CollectOneGridSimpleDcuAndColumnBlockIdColumnId(blockPdmGridDto, allGridRefGridBlockIds, allGridRefGridColumnIds, allGridRefSimpleDcuIds);

            // need to add dependency  Grid key column !
            List<int> allDistinctGridids = PdmCacheManager.GetMutiplePdmGridMetaColumnEntityFromCache(allGridRefGridColumnIds).Select(o => o.GridId).Distinct().ToList();

            foreach (int gridId in allDistinctGridids)
            {
                PdmGridClrDto agridEntity = PdmCacheManager.DictGridCache[gridId]; ;

                // product grid key column
                foreach (var column in agridEntity.ProductGridForeignKeyColumns)
                {
                    allGridRefGridColumnIds.Add(column.GridColumnId);
                    
                }

                // current ref grid column
                foreach (var column in agridEntity.CurrentRefRegularColumnKeyAndDynamicMatrixKeyDCUColumn)
                {
                   allGridRefGridColumnIds.Add(column.GridColumnId);

                }

                // current grid simple dcu
                foreach (var column in agridEntity.CurrentRefSimpleDCUColumns)
                {
                    allGridRefSimpleDcuIds.Add(column.Dcuid.Value);
                }

                foreach (var column in agridEntity.ProductGridSimpleDCUColumn)
                {
                    allGridRefSimpleDcuIds.Add(column.Dcuid.Value);
                }

                // need to add prodcut grid column and Display column as well

                allGridRefGridColumnIds.AddRange(agridEntity.KeyMetaColumns.Select(o => o.GridColumnId)); 

                List<int> productGridColumnAndAndDislayColumnIds = GetProductGridColumnAndAndDislayFields(agridEntity);
                allGridRefGridColumnIds.AddRange(productGridColumnAndAndDislayColumnIds);
            }

          
          


        }

        private static void CollectOneGridSimpleDcuAndColumnBlockIdColumnId(PdmGridClrDto blockPdmGridDto, List<int> allGridRefGridBlockIds, List<int> allGridRefGridColumnIds, List<int> allGridRefSimpleDcuIds)
        {
            foreach (PdmGridMetaColumnClrDto column in blockPdmGridDto.PdmGridMetaColumnList)
            {
                // Grid Block Id and Grid column Id
                if (column.DcucolumnBlockId.HasValue)
                {
                    allGridRefGridBlockIds.Add(column.DcucolumnBlockId.Value);
                }

                if (column.DcucolumnId.HasValue)
                {
                    allGridRefGridColumnIds.Add(column.DcucolumnId.Value);
                    GetAllParentDcuBlockIdsAndDcuColumnIds(allGridRefGridBlockIds, allGridRefGridColumnIds, column);
                }

                // Simple DcuIDs
                if (column.Dcuid.HasValue)
                {
                    allGridRefSimpleDcuIds.Add(column.Dcuid.Value);
                }
            }
        }

        private static void GetAllParentDcuBlockIdsAndDcuColumnIds(List<int> allGridRefGridBlockIds, List<int> allGridRefGridColumnIds,      PdmGridMetaColumnClrDto column)
        {
           
            PdmGridMetaColumnClrDto dcuColumn = PdmCacheManager.DictAllPdmGridMetaColumnCache[column.DcucolumnId.Value];

            //  int loopLevel = 0;
            while (dcuColumn.DcucolumnId.HasValue)
            {
                if (dcuColumn.DcucolumnBlockId.HasValue)
                {
                    allGridRefGridBlockIds.Add(dcuColumn.DcucolumnBlockId.Value);
                }

                if (dcuColumn.DcucolumnId.HasValue)
                {
                    allGridRefGridColumnIds.Add(dcuColumn.DcucolumnId.Value);
                }

                if (dcuColumn.MasterDcucolumnId.HasValue)
                {


                    PdmGridMetaColumnClrDto masterDcucolumn = PdmCacheManager.DictAllPdmGridMetaColumnCache[dcuColumn.MasterDcucolumnId.Value];
                    allGridRefGridBlockIds.Add(masterDcucolumn.DcucolumnBlockId.Value);

                }

                PdmGridMetaColumnClrDto nextDcuColumn = PdmCacheManager.DictAllPdmGridMetaColumnCache[dcuColumn.DcucolumnId.Value];
                if (nextDcuColumn.GridColumnId == dcuColumn.GridColumnId)// need to break up  dead lock
                {
                    break;
                }

                else
                {
                    dcuColumn = nextDcuColumn;


                }

            }
        }


        private static List<int>  GetProductGridColumnAndAndDislayFields( PdmGridClrDto agridEntity )
        {
                List<int> allKeyDisplaySortedColumnIdList = new List<int>();
 
                if (agridEntity.GridType == (int)EmGridType.ProductBasedGrid)
                {
                    allKeyDisplaySortedColumnIdList.Add(agridEntity.ProductGridProductReferenceColumn.GridColumnId);
                }

                var displyacolumnList = agridEntity.PdmGridMetaColumnList.Where(o => o.IsUsedToDisplayProductGridRowInfo.HasValue && o.IsUsedToDisplayProductGridRowInfo.Value).ToList();
                if (displyacolumnList.Count > 0)
                {
                    var sortIdList = displyacolumnList.OrderBy(o => o.DisplayProductGridRowInforOrder).Select(o => o.GridColumnId).ToList();

                    allKeyDisplaySortedColumnIdList.AddRange(sortIdList);
                }
                 return allKeyDisplaySortedColumnIdList;


        }



    }
}