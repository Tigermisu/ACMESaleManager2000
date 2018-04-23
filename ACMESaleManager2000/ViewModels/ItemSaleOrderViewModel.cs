using ACMESaleManager2000.DataEntities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACMESaleManager2000.ViewModels
{
    public class ItemSaleOrderViewModel
    {
        public int ItemEntityId { get; set; }

        [ForeignKey("ItemEntityId")]
        public ItemViewModel Item { get; set; }

        public int SaleOrderEntityId { get; set; }

        [ForeignKey("SaleOrderEntityId")]
        public SaleOrderViewModel SaleOrder { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int SoldQuantity { get; set; }

        [Required]
        [Range(0, Double.MaxValue)]
        public decimal SoldPrice { get; set; }
    }
}