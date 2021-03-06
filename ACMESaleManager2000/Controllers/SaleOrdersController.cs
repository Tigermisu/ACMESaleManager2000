﻿using System;
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
    [Route("api/SaleOrders")]
    [Authorize]
    public class SaleOrdersController : Controller
    {
        private readonly ISaleOrderService _saleOrderService;

        public SaleOrdersController(ISaleOrderService sale)
        {
            _saleOrderService = sale;
        }

        // GET: api/SaleOrders
        [HttpGet]
        [Authorize(Roles = "Admin, Supervisor")]
        public IEnumerable<SaleOrderViewModel> GetSaleOrders()
        {
            return _saleOrderService.GetAll().Select(s => Mapper.Map<SaleOrderViewModel>(s));
        }

        // GET: api/SaleOrders/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Supervisor")]
        public IActionResult GetSaleOrderEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var saleOrderEntity = _saleOrderService.GetEntity(id);

            if (saleOrderEntity == null)
            {
                return NotFound();
            }

            return Ok(saleOrderEntity);
        }

        // PUT: api/SaleOrders/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Supervisor")]
        public IActionResult PutSaleOrderEntity([FromRoute] int id, [FromBody] SaleOrderViewModel saleOrderEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != saleOrderEntity.Id)
            {
                return BadRequest();
            }

            if (_saleOrderService.SaveModifiedEntity(Mapper.Map<SaleOrderEntity>(saleOrderEntity)))
            {
                return NoContent();
            }

            return NotFound();
        }

        // POST: api/SaleOrders
        [HttpPost]
        public IActionResult PostSaleOrderEntity([FromBody] SaleOrderViewModel saleOrderEntity)
        {
            if (!ModelState.IsValid || !_saleOrderService.VerifyStock(Mapper.Map<SaleOrder>(saleOrderEntity)))
            {
                return BadRequest(ModelState);
            }

            _saleOrderService.CreateEntity(Mapper.Map<SaleOrder>(saleOrderEntity));

            foreach (var s in saleOrderEntity.SoldItems)
            {
                _saleOrderService.SubtractFromItemInventory(s.ItemEntityId, s.SoldQuantity);
            }

            return CreatedAtAction("GetSaleOrderEntity", new { id = saleOrderEntity.Id }, saleOrderEntity);
        }

        // DELETE: api/SaleOrders/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Supervisor")]
        public IActionResult DeleteSaleOrderEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_saleOrderService.DeleteEntity(id))
            {
                return Ok();
            }

            return NotFound();
        }

        private bool SaleOrderEntityExists(int id)
        {
            return _saleOrderService.EntityExists(id);
        }
    }
}