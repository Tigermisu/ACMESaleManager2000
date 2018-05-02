using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.ViewModels
{
    public class ItemSaleReportViewModel
    {
        public ItemViewModel Item { get; set; }
        public int TotalSold { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal Income {
            get {
                return TotalSold * AveragePrice;
            }
        }
    }
}
