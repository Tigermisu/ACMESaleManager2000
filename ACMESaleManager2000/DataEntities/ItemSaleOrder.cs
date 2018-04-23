using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DataEntities
{
    public class ItemSaleOrder
    {
        public int ItemEntityId { get; set; }

        [ForeignKey("ItemEntityId")]
        public ItemEntity Item { get; set; }

        public int SaleOrderEntityId { get; set; }

        [ForeignKey("SaleOrderEntityId")]
        public SaleOrderEntity SaleOrder { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int SoldQuantity { get; set; }

        [Required]
        [Range(0, Double.MaxValue)]
        public decimal SoldPrice { get; set; }
    }
}
