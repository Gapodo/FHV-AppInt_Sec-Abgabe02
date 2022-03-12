#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using a02_shopsystem.Model;
using a02_shopsystem.DTO;

namespace a02_shopsystem.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly ShopsystemContext _context;
        private static Object ShopNotFoundContent = new { status = StatusCodes.Status404NotFound, title = "Not found", error = "No shop with the given id found." };

        public ShopsController(ShopsystemContext context)
        {
            _context = context;
        }

        // GET: api/shops/1/shops
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ShopDTO>>> GetShops()
        {
            return await _context.Shops.Select(a => new ShopDTO()
            {
                Id = a.Id,
                Name = a.Name
            }).ToListAsync();

        }

        // GET: api/shops/1/
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShopDTO>> GetShop([FromRoute] int id)
        {
            var shop = await _context.Shops.Select(a => new ShopDTO()
            {
                Id = a.Id,
                Name = a.Name,
            }).FirstOrDefaultAsync(s => s.Id == id);

            if (shop == null)
            {
                return NotFound();
            }

            return shop;
        }

        // PUT: api/shops/1
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutShop([FromRoute] int id, [FromBody] ShopDTO shopDTO)
        {
            // compare url against body
            if (id != shopDTO.Id)
            {
                return BadRequest();
            }

            if (shopDTO.Name.Trim().Length == 0)
            {
                return BadRequest();
            }

            var shop = await _context.Shops.SingleOrDefaultAsync(b => b.Id == id);

            // if search returned null there is either no item with that id or no item with that id within the shop
            if (shop == null)
            {
                return NotFound();
            }

            shop.Name = shopDTO.Name;

            _context.Entry(shop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShopExists(id))
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

        // POST: api/shops/
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Shop>> PostShop([FromBody] ShopDTO shopDTO)
        {
            if (shopDTO.Name.Trim().Length == 0)
            {
                return BadRequest();
            }

            Shop shop = new Shop()
            {
                Name = shopDTO.Name
            };

            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();
            // update the dto for returning
            shopDTO.Id = shop.Id;
            return CreatedAtAction(nameof(GetShop), new { id = shop.Id }, shopDTO);
        }

        // DELETE: api/shops/1
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteShop([FromRoute] int id)
        {
            var shop = await _context.Shops.FirstOrDefaultAsync(a => a.Id == id);

            if (shop == null)
            {
                return NotFound();
            }

            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShopExists(int id)
        {
            return _context.Shops.Any(e => e.Id == id);
        }
    }
}
