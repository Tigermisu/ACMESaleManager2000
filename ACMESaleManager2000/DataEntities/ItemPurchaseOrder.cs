using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DataEntities
{
    public class ItemPurchaseOrder
    {
        public int ItemEntityId { get; set; }

        [ForeignKey("ItemEntityId")]
        public ItemEntity Item { get; set; }

        public int PurchaseOrderEntityId { get; set; }

        [ForeignKey("PurchaseOrderEntityId")]
        public PurchaseOrderEntity PurchaseOrder { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int PurchasedQuantity { get; set; }

        [Required]
        [Range(0, Double.MaxValue)]
        public decimal PurchasedPrice { get; set; }
    }
}
