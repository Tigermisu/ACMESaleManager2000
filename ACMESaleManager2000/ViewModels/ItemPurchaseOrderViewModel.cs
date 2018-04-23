using ACMESaleManager2000.DataEntities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACMESaleManager2000.ViewModels
{
    public class ItemPurchaseOrderViewModel
    {
        public int ItemEntityId { get; set; }

        [ForeignKey("ItemEntityId")]
        public ItemViewModel Item { get; set; }

        public int PurchaseOrderEntityId { get; set; }

        [ForeignKey("PurchaseOrderEntityId")]
        public ItemPurchaseOrderViewModel PurchaseOrder { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int PurchasedQuantity { get; set; }

        [Required]
        [Range(0, Double.MaxValue)]
        public decimal PurchasedPrice { get; set; }
    }
}