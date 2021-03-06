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
    [Route("api/Items")]
    [Authorize]
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
        public IActionResult GetItemEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var itemEntity = _itemService.GetEntity(id);

            if (itemEntity == null)
            {
                return NotFound();
            }

            return Ok(itemEntity);
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        [Authorize(Roles="Admin, Supervisor")]
        public IActionResult PutItemEntity([FromRoute] int id, [FromBody] ItemViewModel itemEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != itemEntity.Id)
            {
                return BadRequest();
            }

            if (_itemService.SaveModifiedEntity(Mapper.Map<ItemEntity>(itemEntity)))
            {
                return NoContent();
            }

            return NotFound();
        }

        // POST: api/Items
        [HttpPost]
        [Authorize(Roles = "Admin, Supervisor")]
        public IActionResult PostItemEntity([FromBody] ItemViewModel itemEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _itemService.CreateEntity(Mapper.Map<Item>(itemEntity));

            return CreatedAtAction("GetItemEntity", new { id = itemEntity.Id }, itemEntity);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Supervisor")]
        public IActionResult DeleteItemEntity([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_itemService.DeleteEntity(id))
            {
                return Ok();
            }

            return NotFound();
        }

        private bool ItemEntityExists(int id)
        {
            return _itemService.EntityExists(id);
        }
    }
}