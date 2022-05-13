using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.SeoDTO
{
    public class SeoScoreBaseDto
    {
        public string UniqeCode { get; set; }
        public string Note { get; set; }
        public int Amount { get; set; }
        public int Level { get; set; }
        public int NewsId { get; set; }

    }
}
