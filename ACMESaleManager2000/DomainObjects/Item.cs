using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainObjects
{
    public class Item
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int QuantityAvailable { get; set; }
        
        public decimal PurchasePrice { get; set; }
        
        public decimal SalePrice { get; set; }

        public string ImagePath { get; set; }
    }
}
