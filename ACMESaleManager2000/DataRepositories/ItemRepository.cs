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
            throw new NotImplementedException();
        }
    }
}
