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

        public int ItemEntityId { get; set; }

        public Item SoldItem { get; set; }

        public int SoldQuantity { get; set; }

        public string ClientName { get; set; }
    }
}
