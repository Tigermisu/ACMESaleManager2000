using ACMESaleManager2000.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DataRepositories
{
    public interface IItemRepository
    {
        List<Item> GetItems();

        List<Item> GetItems(int[] ids);

        List<Item> GetLowStockItems(int stockThreshold);

        List<ItemSaleOrder> GetPopularItems(int deltaDays);

        void ModifyStock(int itemId, int delta);
    }
}
