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
        protected readonly IItemRepository _itemRepository;

        public PurchaseOrderService(IRepository<PurchaseOrder> repository, IItemRepository itemRepository) : base(repository)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }

        public void AddToItemInventory(int itemId, int quantity) {
            _itemRepository.ModifyStock(itemId, quantity);
        }
    }
}