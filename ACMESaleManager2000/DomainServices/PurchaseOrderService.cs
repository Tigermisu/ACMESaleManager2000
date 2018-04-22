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
        protected readonly IRepository<Item> _itemRepository;

        public PurchaseOrderService(IRepository<PurchaseOrder> repository, IRepository<Item> itemRepository) : base(repository)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }
    }
}