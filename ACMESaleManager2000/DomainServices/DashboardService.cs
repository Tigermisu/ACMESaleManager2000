using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ACMESaleManager2000.Models;

namespace ACMESaleManager2000.DomainServices
{
    public class DashboardService : IDashboardService
    {
        public bool CanUserAdministrate(ClaimsPrincipal user)
        {
            if (user.IsInRole("Admin") || user.IsInRole("Supervisor")) {
                return true;
            }

            return false;
        }
    }
}
