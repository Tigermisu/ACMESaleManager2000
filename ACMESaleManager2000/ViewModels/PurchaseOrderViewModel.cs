using ACMESaleManager2000.DataEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.ViewModels
{
    public class PurchaseOrderViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateOfPurchase { get; set; } = DateTime.Now;

        [Required]
        public virtual ICollection<ItemPurchaseOrderViewModel> PurchasedItems { get; set; }

        public string Description { get; set; }

        public decimal Total {
            get {
                decimal total = 0;

                foreach (ItemPurchaseOrderViewModel itempurchase in PurchasedItems) {
                    total += itempurchase.PurchasedPrice * itempurchase.PurchasedQuantity;
                }

                return total;
            }
        }
    }
}
