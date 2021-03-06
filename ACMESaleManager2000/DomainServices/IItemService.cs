﻿using ACMESaleManager2000.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainServices
{
    public interface IItemService : IService<Item>
    {
        List<Item> GetLowStockItems(int threshold);

        List<ItemSaleOrder> GetPopularItems(int deltaDays);
    }
}
