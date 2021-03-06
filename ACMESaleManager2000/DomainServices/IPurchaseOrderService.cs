﻿using ACMESaleManager2000.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainServices
{
    public interface IPurchaseOrderService : IService<PurchaseOrder>
    {
        void AddToItemInventory(int itemId, int quantity);
    }
}
