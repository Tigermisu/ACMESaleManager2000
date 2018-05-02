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

        public bool VerifyStock(SaleOrder saleOrder)
        {
            List<ItemSaleOrder> itemSales = saleOrder.SoldItems.ToList();
            List<Item> items = _itemRepository.GetItems(itemSales.Select(i => i.ItemEntityId).ToArray());

            foreach (ItemSaleOrder itemSale in itemSales) {
                Item item = items.Where(i => i.Id == itemSale.ItemEntityId).First();
                if (itemSale.SoldQuantity > item.QuantityAvailable) {
                    return false;
                }
            }

            return true;
        }
    }
}
