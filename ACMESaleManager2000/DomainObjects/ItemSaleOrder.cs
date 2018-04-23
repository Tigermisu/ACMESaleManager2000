using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainObjects
{
    public class ItemSaleOrder
    {
        public int ItemEntityId { get; set; }
        
        public Item Item { get; set; }

        public int SaleOrderEntityId { get; set; }
        
        public SaleOrder SaleOrder { get; set; }
        
        public int SoldQuantity { get; set; }
        
        public decimal SoldPrice { get; set; }
    }
}
