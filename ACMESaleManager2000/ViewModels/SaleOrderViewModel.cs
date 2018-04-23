using ACMESaleManager2000.DataEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.ViewModels
{
    public class SaleOrderViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateOfSale { get; set; } = DateTime.Now;

        [Required]
        public virtual ICollection<ItemSaleOrderViewModel> SoldItems { get; set; }


        public string ClientName { get; set; }
    }
}
