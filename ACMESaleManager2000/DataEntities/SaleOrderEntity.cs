﻿using System;
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
        public int ItemEntityId { get; set; }

        [ForeignKey("ItemEntityId")]
        public ItemEntity SoldItem { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int SoldQuantity { get; set; }

        public string ClientName { get; set; }
    }
}
