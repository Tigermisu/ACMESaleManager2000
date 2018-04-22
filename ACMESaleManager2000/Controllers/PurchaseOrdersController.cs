using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ACMESaleManager2000.Data;
using ACMESaleManager2000.DataEntities;

namespace ACMESaleManager2000.Controllers
{
    [Produces("application/json")]
    [Route("api/PurchaseOrders")]
    public class PurchaseOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseOrders
        [HttpGet]
        public IEnumerable<PurchaseOrderEntity> GetPurchaseOrders()
        {
            return _context.PurchaseOrders;
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
        public async Task<IActionResult> DeletePurchaseOrderEntity([FromRoute] int id)
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

            _context.PurchaseOrders.Remove(purchaseOrderEntity);
            await _context.SaveChangesAsync();

            return Ok(purchaseOrderEntity);
        }

        private bool PurchaseOrderEntityExists(int id)
        {
            return _context.PurchaseOrders.Any(e => e.Id == id);
        }
    }
}