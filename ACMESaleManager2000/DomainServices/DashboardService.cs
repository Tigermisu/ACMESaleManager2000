using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ACMESaleManager2000.DomainObjects;
using ACMESaleManager2000.Models;

namespace ACMESaleManager2000.DomainServices
{
    public class DashboardService : IDashboardService
    {
        private readonly IItemService _itemService;
        private readonly ISaleOrderService _saleOrderService;
        private readonly IPurchaseOrderService _purchaseOrderService;

        public DashboardService(IItemService itemService, IPurchaseOrderService purchaseOrderService, ISaleOrderService saleOrderService) {
            _itemService = itemService;
            _saleOrderService = saleOrderService;
            _purchaseOrderService = purchaseOrderService;
        }

        public bool CanUserAdministrate(ClaimsPrincipal user)
        {
            if (user.IsInRole("Admin") || user.IsInRole("Supervisor")) {
                return true;
            }

            return false;
        }

        public List<Item> GetLowStockItems(int threshold)
        {
            return _itemService.GetLowStockItems(threshold);
        }

        public List<ItemSaleReport> GetPopularItems(int deltaDays)
        {
            return _itemService.GetPopularItems(deltaDays)
                .Select(i => new ItemSaleReport{
                Item = i.Item,
                TotalSold = i.SoldQuantity,
                AveragePrice = i.SoldPrice
            }).ToList();
        }

        public ProfitReport GetProfits(int deltaDays) {
            return new ProfitReport
            {
                Incomes = _saleOrderService.GetAll()
                .Where(s => s.DateOfSale > DateTime.Now.AddDays(-deltaDays))
                .Select(p => new KeyValuePair<DateTime, decimal>(p.DateOfSale,
                    p.SoldItems.Aggregate<ItemSaleOrder, decimal>(0, (a, b) => {
                        return a + b.SoldPrice * b.SoldQuantity;
                    })))
                .ToList(),

                Expenses = _purchaseOrderService.GetAll()
                .Where(p => p.DateOfPurchase > DateTime.Now.AddDays(-deltaDays))
                .Select(p => new KeyValuePair<DateTime, decimal>(p.DateOfPurchase,
                    p.PurchasedItems.Aggregate<ItemPurchaseOrder, decimal>(0, (a, b) => {
                        return a + b.PurchasedPrice * b.PurchasedQuantity;
                    })))
                .ToList()
            };
        }
    }
}
