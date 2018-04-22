using ACMESaleManager2000.Data;
using ACMESaleManager2000.DataEntities;
using ACMESaleManager2000.DomainObjects;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DataRepositories
{
    public class ItemRepository : Repository<Item, ItemEntity>, IItemRepository
    {
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        protected override DbSet<ItemEntity> DbSet { get {
                return _context.Items;
            }
        }

        public List<Item> GetItems()
        {
            return GetAll();
        }

        public List<Item> GetLowStockItems(int stockThreshold)
        {
            return Map(DbSet.Where(i => i.QuantityAvailable <= stockThreshold).ToList());
        }

        public List<Item> GetPopularItems(int saleThreshold)
        {
            return _context.SaleOrders.GroupBy(s => s.SoldItem)
                .Where(sale => sale.Sum(s => s.SoldQuantity) >= saleThreshold)
                .Select(s => new Item
                {
                    Name = s.First().SoldItem.Name,
                    SalePrice = s.Average(i => i.SoldItem.SalePrice),
                    QuantityAvailable = s.Sum(i => i.SoldQuantity)
                }).ToList();
        }
    }
}
