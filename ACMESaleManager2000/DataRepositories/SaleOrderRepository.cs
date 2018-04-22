using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ACMESaleManager2000.Data;
using ACMESaleManager2000.DataEntities;
using ACMESaleManager2000.DomainObjects;

namespace ACMESaleManager2000.DataRepositories
{
    public class SaleOrderRepository : Repository<SaleOrder, SaleOrderEntity>, ISaleOrderRepository
    {
        public SaleOrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override List<SaleOrder> GetAll()
        {
            return GetSaleOrders();
        }

        public List<SaleOrder> GetSaleOrders()
        {
            return Map(_context.SaleOrders.ToList());
        }
    }
}
