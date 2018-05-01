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
    }
}