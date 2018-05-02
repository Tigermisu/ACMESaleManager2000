using ACMESaleManager2000.DataEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.ViewModels
{
    public class ItemViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
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

        public string ImagePath { get; set; }

        public string ProductCode {
            get {
                if (Name == null || Description == null) {
                    return "BAD-ITEM";
                }
                string name = Name.Length > 3 ? Name.Substring(0, 3) : Name,
                    description = Description.Length > 3 ? Description.Substring(0, 3) : Description;

                return $"ACME-{name}-{description}";
            }
        }
    }
}
