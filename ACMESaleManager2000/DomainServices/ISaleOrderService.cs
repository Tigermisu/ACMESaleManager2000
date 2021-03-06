﻿using ACMESaleManager2000.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DomainServices
{
    public interface ISaleOrderService : IService<SaleOrder>
    {
        void SubtractFromItemInventory(int itemId, int quantity);
        bool VerifyStock(SaleOrder saleOrder);
    }
}
