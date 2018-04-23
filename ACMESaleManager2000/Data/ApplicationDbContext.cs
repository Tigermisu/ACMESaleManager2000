using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ACMESaleManager2000.Models;
using ACMESaleManager2000.DataEntities;

namespace ACMESaleManager2000.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<SaleOrderEntity> SaleOrders { get; set; }
        public DbSet<PurchaseOrderEntity> PurchaseOrders { get; set; }
        public DbSet<ItemEntity> Items { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ItemPurchaseOrder>()
                .HasKey(ip => new { ip.ItemEntityId, ip.PurchaseOrderEntityId });

            builder.Entity<ItemPurchaseOrder>()
                .HasOne(ip => ip.Item)
                .WithMany(i => i.Purchases)
                .HasForeignKey(ip => ip.ItemEntityId);

            builder.Entity<ItemPurchaseOrder>()
                .HasOne(ip => ip.PurchaseOrder)
                .WithMany(p => p.PurchasedItems)
                .HasForeignKey(ip => ip.PurchaseOrderEntityId);


            builder.Entity<ItemSaleOrder>()
                .HasKey(ip => new { ip.ItemEntityId, ip.SaleOrderEntityId });

            builder.Entity<ItemSaleOrder>()
                .HasOne(ip => ip.Item)
                .WithMany(i => i.Sales)
                .HasForeignKey(ip => ip.ItemEntityId);

            builder.Entity<ItemSaleOrder>()
                .HasOne(ip => ip.SaleOrder)
                .WithMany(p => p.SoldItems)
                .HasForeignKey(ip => ip.SaleOrderEntityId);
        }
    }
}
