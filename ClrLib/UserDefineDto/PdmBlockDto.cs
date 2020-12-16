using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;

namespace PLMCLRTools
{ 
    public partial class PdmBlockClrDto 
    {

        public PdmGridClrDto BlockPdmGridDto
        {
            get
            {
                var gridSubitem = this.PdmBlockSubItemList.FirstOrDefault(o => o.GridId.HasValue);
                if (gridSubitem != null)
                {
                    int gridid = gridSubitem.GridId.Value;
                    return PdmCacheManager.DictGridCache[gridid];  
                }

                return null;
            
                
            }
        }
		
	
		        
    }
}

