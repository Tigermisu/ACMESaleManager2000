using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACMESaleManager2000.Data;
using ACMESaleManager2000.DataEntities;
using ACMESaleManager2000.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace ACMESaleManager2000.DataRepositories
{
    public class SaleOrderRepository : Repository<SaleOrder, SaleOrderEntity>, ISaleOrderRepository
    {
        public SaleOrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        protected override DbSet<SaleOrderEntity> DbSet {
            get {
                return _context.SaleOrders;
            }
        }

        public override List<SaleOrder> GetAll()
        {
            var m = DbSet.Include(p => p.SoldItems).ThenInclude(s => s.Item).ToList();

            return Map(m);
        }

        public List<SaleOrder> GetSaleOrders()
        {
            return GetAll();
        }
    }
}
