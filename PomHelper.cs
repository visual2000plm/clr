
using System;
using System.Collections.Generic;
using System.Text;

namespace PLMCLRTools
{
    public static class PomHelper
    {
        public static List<decimal> CaculateSizeValueWithGradingValue(List<decimal> oneRowGradingValueList, decimal baseSizeValue, int baseIndex)
        {
            List<decimal> toRetrunSizeValueList = new List<decimal>();
            if (baseIndex == -1)
                return toRetrunSizeValueList;

            for (int i = 0; i < oneRowGradingValueList.Count; i++)
            {
                toRetrunSizeValueList.Add(0);
            }
            toRetrunSizeValueList[baseIndex] = baseSizeValue;

            //left
            for (int i = baseIndex; i >= 1; i--)
            {
                toRetrunSizeValueList[i - 1] = toRetrunSizeValueList[i] - oneRowGradingValueList[i - 1];
            } // right side
            for (int i = baseIndex; i < oneRowGradingValueList.Count - 1; i++)
            {
                toRetrunSizeValueList[i + 1] = toRetrunSizeValueList[i] + oneRowGradingValueList[i + 1];
            }

            return toRetrunSizeValueList;
        }

    }
  
}
