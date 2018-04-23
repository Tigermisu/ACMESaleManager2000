using ACMESaleManager2000.DataEntities;
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

        public string Description { get; set; }

        public virtual ICollection<ItemPurchaseOrder> PurchasedItems { get; } = new List<ItemPurchaseOrder>();
    }
}
