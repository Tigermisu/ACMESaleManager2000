using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ACMESaleManager2000.Data;
using ACMESaleManager2000.DataEntities;
using ACMESaleManager2000.DomainServices;
using ACMESaleManager2000.ViewModels;
using AutoMapper;
using ACMESaleManager2000.DomainObjects;
using Microsoft.AspNetCore.Authorization;

namespace ACMESaleManager2000.Controllers
{
    [Produces("application/json")]
    [Route("api/Dashboard")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("authorize")]
        public bool CanAccessAdmin() {
            return _dashboardService.CanUserAdministrate(User);
        }

        [HttpGet("lowstock/{threshold}")]
        public List<ItemViewModel> GetLowStockItems([FromRoute] int threshold) {
            return _dashboardService.GetLowStockItems(threshold).Select(e => Mapper.Map<ItemViewModel>(e)).ToList();
        }

        [HttpGet("popular/{deltaDays}")]
        public List<ItemSaleReportViewModel> GetPopularItems([FromRoute] int deltaDays) {
            return _dashboardService.GetPopularItems(deltaDays).Select(e => Mapper.Map<ItemSaleReportViewModel>(e)).ToList();
        }

        [HttpGet("profits/{deltaDays}")]
        public ProfitReportViewModel GetProfits([FromRoute] int deltaDays)
        {
            return Mapper.Map<ProfitReportViewModel>(_dashboardService.GetProfits(deltaDays));
        }
    }
}