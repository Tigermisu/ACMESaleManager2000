using ACMESaleManager2000.Data;
using ACMESaleManager2000.DataEntities;
using ACMESaleManager2000.DomainObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DataRepositories
{
    public class PurchaseOrderRepository : Repository<PurchaseOrder, PurchaseOrderEntity>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        protected override DbSet<PurchaseOrderEntity> DbSet {
            get {
                return _context.PurchaseOrders;
            }
        }

        public override List<PurchaseOrder> GetAll()
        {
            var m = DbSet.Include(p => p.PurchasedItems).ThenInclude(s => s.Item).ToList();

            return Map(m);
        }

        public List<PurchaseOrder> GetPurchaseOrders()
        {
            return GetAll();
        }
    }
}
