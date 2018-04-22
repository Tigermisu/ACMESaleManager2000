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
using AutoMapper;
using ACMESaleManager2000.ViewModels;

namespace ACMESaleManager2000.Controllers
{
    [Produces("application/json")]
    [Route("api/PurchaseOrders")]
    public class PurchaseOrdersController : Controller
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        // GET: api/PurchaseOrders
        [HttpGet]
        public IEnumerable<PurchaseOrderViewModel> GetPurchaseOrders()
        {
            return _purchaseOrderService.GetAll().Select(p => Mapper.Map<PurchaseOrderViewModel>(p));
        }

        // GET: api/PurchaseOrders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseOrderEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var purchaseOrderEntity = await _context.PurchaseOrders.SingleOrDefaultAsync(m => m.Id == id);

            if (purchaseOrderEntity == null)
            {
                return NotFound();
            }

            return Ok(purchaseOrderEntity);
        }

        // PUT: api/PurchaseOrders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaseOrderEntity([FromRoute] int id, [FromBody] PurchaseOrderEntity purchaseOrderEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != purchaseOrderEntity.Id)
            {
                return BadRequest();
            }

            _context.Entry(purchaseOrderEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseOrderEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PurchaseOrders
        [HttpPost]
        public async Task<IActionResult> PostPurchaseOrderEntity([FromBody] PurchaseOrderEntity purchaseOrderEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PurchaseOrders.Add(purchaseOrderEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchaseOrderEntity", new { id = purchaseOrderEntity.Id }, purchaseOrderEntity);
        }

        // DELETE: api/PurchaseOrders/5
        [HttpDelete("{id}")]
        public IActionResult DeletePurchaseOrderEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_purchaseOrderService.DeleteEntity(id))
            {
                return Ok();
            }

            return NotFound();
        }

        private bool PurchaseOrderEntityExists(int id)
        {
            return _purchaseOrderService.EntityExists(id);
        }
    }
}