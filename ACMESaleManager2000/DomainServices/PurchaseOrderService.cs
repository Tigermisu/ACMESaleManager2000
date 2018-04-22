using ACMESaleManager2000.DataRepositories;
using ACMESaleManager2000.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainServices
{
    public class PurchaseOrderService : Service<PurchaseOrder>, IPurchaseOrderService
    {
        public PurchaseOrderService(IRepository<PurchaseOrder> repository) : base(repository)
        {
        }
    }
}