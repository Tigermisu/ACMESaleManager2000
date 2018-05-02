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

        public List<Item> GetItems(int[] ids)
        {
            return Map(DbSet.Where(item => ids.Contains(item.Id)).ToList());
        }

        public List<Item> GetLowStockItems(int stockThreshold)
        {
            return Map(DbSet.Where(i => i.QuantityAvailable <= stockThreshold).ToList());
        }

        public List<ItemSaleOrder> GetPopularItems(int deltaDays)
        {
            var items = _context.ItemSaleOrderEntity
                .Where(i => i.SaleOrder.DateOfSale > DateTime.Now.AddDays(-deltaDays))
                .GroupBy(i => i.ItemEntityId)
                .Select(e => new ItemSaleOrderEntity
            {
                Item = e.First().Item,
                SoldPrice = e.Average(a => a.SoldPrice),
                SoldQuantity = e.Sum(a => a.SoldQuantity)
            }).OrderByDescending(s => s.SoldQuantity)
            .Take(3);

            return items.Select(e => Mapper.Map<ItemSaleOrder>(e)).ToList();
        }

        public void ModifyStock(int itemId, int delta)
        {
            var item = GetEntityRaw(itemId) ?? throw new ArgumentNullException(nameof(itemId));

            item.QuantityAvailable += delta;

            _context.Entry(item).State = EntityState.Modified;

            _context.SaveChanges();
        }
    }
}
