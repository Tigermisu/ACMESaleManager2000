using ACMESaleManager2000.Data;
using ACMESaleManager2000.DataEntities;
using ACMESaleManager2000.DomainObjects;
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

        public override bool EntityExists(int Id)
        {
            return _context.PurchaseOrders.Any(e => e.Id == Id);
        }

        public override List<PurchaseOrder> GetAll()
        {
            return GetPurchaseOrders();
        }

        public List<PurchaseOrder> GetPurchaseOrders()
        {
            return Map(_context.PurchaseOrders.ToList());
        }
    }
}
