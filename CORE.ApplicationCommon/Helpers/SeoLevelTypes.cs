using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.Helpers
{
    public enum SeoLevelTypes
    {
        skorYok,
        kotu,
        ortalama, 
        iyi, 
        cokIyi
    }

    public static class SeoCheckerLevel
    {
        public static int GetValue(this SeoLevelTypes seoLevel)
        {
            switch (seoLevel)
            {
                case SeoLevelTypes.skorYok:
                    return 0;
                case SeoLevelTypes.kotu:
                    return 1;
                case SeoLevelTypes.ortalama:
                    return 2;
                case SeoLevelTypes.iyi:
                    return 3;
                case SeoLevelTypes.cokIyi:
                    return 4;
                default:
                    return -1;
            }
        }
    }

}
