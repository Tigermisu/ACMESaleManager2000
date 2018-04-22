using ACMESaleManager2000.DataRepositories;
using ACMESaleManager2000.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainServices
{
    public class SaleOrderService : Service<SaleOrder>, ISaleOrderService
    {
        public SaleOrderService(IRepository<SaleOrder> repository) : base(repository)
        {
        }
    }
}
