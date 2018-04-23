using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DataEntities
{
    public class SaleOrderEntity : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateOfSale { get; set; } = DateTime.Now;

        [Required]
        public virtual ICollection<ItemSaleOrder> SoldItems { get; } = new List<ItemSaleOrder>();

        public string ClientName { get; set; }
    }
}
