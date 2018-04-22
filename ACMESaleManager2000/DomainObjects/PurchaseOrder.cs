using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainObjects
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        
        public DateTime DateOfPurchase { get; set; } = DateTime.Now;

        public int ItemEntityId { get; set; }

        public Item PurchasedItem { get; set; }

        public int PurchasedQuantity { get; set; }

        public string Description { get; set; }
    }
}
