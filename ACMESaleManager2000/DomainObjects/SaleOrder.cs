using ACMESaleManager2000.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainObjects
{
    public class SaleOrder
    {
        public int Id { get; set; }
        
        public DateTime DateOfSale { get; set; } = DateTime.Now;

        public string ClientName { get; set; }

        public virtual ICollection<ItemSaleOrder> SoldItems { get; set; }
    }
}
