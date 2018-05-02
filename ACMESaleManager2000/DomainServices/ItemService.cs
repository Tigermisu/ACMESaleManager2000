using ACMESaleManager2000.DataRepositories;
using ACMESaleManager2000.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainServices
{
    public class ItemService : Service<Item>, IItemService
    {
        protected readonly IItemRepository _itemRepository;

        public ItemService(IRepository<Item> repository, IItemRepository itemRepository) : base(repository)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }

        public List<Item> GetLowStockItems(int threshold)
        {
            return _itemRepository.GetLowStockItems(threshold);
        }

        public List<ItemSaleOrder> GetPopularItems(int deltaDays)
        {
            return _itemRepository.GetPopularItems(deltaDays);
        }
    }
}
