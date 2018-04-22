using ACMESaleManager2000.Data;
using ACMESaleManager2000.DataEntities;
using ACMESaleManager2000.DomainObjects;
using AutoMapper;
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

        public override List<Item> GetAll() {
            return GetItems();
        }

        public List<Item> GetItems()
        {
            return Map(_context.Items.ToList());
        }

        public List<Item> GetLowStockItems(int stockThreshold)
        {
            return Map(_context.Items.Where(i => i.QuantityAvailable <= stockThreshold).ToList());
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
