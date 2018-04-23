using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DataEntities
{
    public class ItemEntity : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int QuantityAvailable { get; set; }

        [Required]
        [Range(0, Double.MaxValue)]
        public decimal PurchasePrice { get; set; }

        [Required]
        [Range(0, Double.MaxValue)]
        public decimal SalePrice { get; set; }
        
        public virtual ICollection<ItemPurchaseOrderEntity> Purchases { get; set; }
        
        public virtual ICollection<ItemSaleOrderEntity> Sales { get; set; }

        public string ImagePath { get; set; }
    }
}
