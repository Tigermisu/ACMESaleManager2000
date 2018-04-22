﻿using ACMESaleManager2000.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACMESaleManager2000.DataRepositories
{
    interface ISaleOrderRepository
    {
        List<SaleOrder> GetSaleOrders();
    }
}
