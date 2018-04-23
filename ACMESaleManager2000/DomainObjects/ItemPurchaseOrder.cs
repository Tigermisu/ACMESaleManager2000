using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainObjects
{
    public class ItemPurchaseOrder
    {
        public int ItemEntityId { get; set; }
        
        public Item Item { get; set; }

        public int PurchaseOrderEntityId { get; set; }
        
        public PurchaseOrder PurchaseOrder { get; set; }
        
        public int PurchasedQuantity { get; set; }
        
        public decimal PurchasedPrice { get; set; }
    }
}
