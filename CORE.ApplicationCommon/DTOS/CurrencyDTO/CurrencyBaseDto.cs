using System;
using System.Collections.Generic;
using System.Text;

namespace CORE.ApplicationCommon.DTOS.CurrencyDTO
{
    public class CurrencyBaseDto
    {
        public string code { get; set; }
        public string crossorder { get; set; }
        public string currencyCode { get; set; }
        public string unit { get; set; }
        public string name { get; set; }
        public string currencyName { get; set; }
        public string ForexBuying { get; set; }
        public string isRateOrDown { get; set; }

        public string ForexSelling { get; set; }
        public string BanknoteBuying { get; set; }
        public string BanknoteSelling { get; set; }
        public string CrossRateOther { get; set; }
        public string CrossRateUSD { get; set; }
    }
}
