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
        protected readonly IRepository<Item> _itemRepository;

        public SaleOrderService(IRepository<SaleOrder> repository, IRepository<Item> itemRepository) : base(repository)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }
    }
}
