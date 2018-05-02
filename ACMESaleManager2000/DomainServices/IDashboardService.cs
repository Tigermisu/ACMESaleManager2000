using ACMESaleManager2000.DomainObjects;
using System.Collections.Generic;
using System.Security.Claims;

namespace ACMESaleManager2000.DomainServices
{
    public interface IDashboardService
    {
        bool CanUserAdministrate(ClaimsPrincipal user);

        List<ItemSaleReport> GetPopularItems(int deltaDays);

        List<Item> GetLowStockItems(int threshold);

        ProfitReport GetProfits(int deltaDays);
    }
}
