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

namespace ACMESaleManager2000.Controllers
{
    [Produces("application/json")]
    [Route("api/Items")]
    public class ItemsController : Controller
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        // GET: api/Items
        [HttpGet]
        public IEnumerable<ItemViewModel> GetItems()
        {
            return _itemService.GetAll().Select(i => Mapper.Map<ItemViewModel>(i));
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var itemEntity = await _context.Items.SingleOrDefaultAsync(m => m.Id == id);

            if (itemEntity == null)
            {
                return NotFound();
            }

            return Ok(itemEntity);
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemEntity([FromRoute] int id, [FromBody] ItemEntity itemEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != itemEntity.Id)
            {
                return BadRequest();
            }

            _context.Entry(itemEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemEntityExists(id))
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

        // POST: api/Items
        [HttpPost]
        public async Task<IActionResult> PostItemEntity([FromBody] ItemEntity itemEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Items.Add(itemEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemEntity", new { id = itemEntity.Id }, itemEntity);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var itemEntity = await _context.Items.SingleOrDefaultAsync(m => m.Id == id);
            if (itemEntity == null)
            {
                return NotFound();
            }

            _context.Items.Remove(itemEntity);
            await _context.SaveChangesAsync();

            return Ok(itemEntity);
        }

        private bool ItemEntityExists(int id)
        {
            return _itemService.EntityExists(id);
        }
    }
}