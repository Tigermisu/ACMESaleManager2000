using System.Security.Claims;

namespace ACMESaleManager2000.DomainServices
{
    public interface IDashboardService
    {
        bool CanUserAdministrate(ClaimsPrincipal user);
    }
}
