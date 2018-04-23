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
        protected readonly IItemRepository _itemRepository;

        public SaleOrderService(IRepository<SaleOrder> repository, IItemRepository itemRepository) : base(repository)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }

        public void SubtractFromItemInventory(int itemId, int quantity)
        {
            _itemRepository.ModifyStock(itemId, -quantity);
        }
    }
}
