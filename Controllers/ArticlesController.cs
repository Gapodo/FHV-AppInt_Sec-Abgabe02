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
    [Route("api/shop/")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ShopsystemContext _context;
        private static Object ShopNotFoundContent = new { status = StatusCodes.Status404NotFound, title = "Not found", error = "No shop with the given id found." };

        public ArticlesController(ShopsystemContext context)
        {
            _context = context;
        }

        // GET: api/shops/1/articles
        [HttpGet("{shopId:int}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetArticles([FromRoute] int shopId)
        {
            //TODO funktion für check auslagern?!
            if ((await _context.Shops.FindAsync(shopId)) != null)
            {
                return await _context.Articles.Where(a => a.ShopId == shopId).Select(a => new ArticleDTO()
                {
                    Id = a.Id,
                    Name = a.Name,
                    EuroPrice = a.EuroPrice
                }).ToListAsync();
            }
            else
            {
                return NotFound(ShopNotFoundContent);
            }

        }

        // GET: api/shops/1/Articles/2
        [HttpGet("{shopId:int}/[controller]/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ArticleDTO>> GetArticle([FromRoute] int shopId, [FromRoute] int id)
        {
            // TODO Auslagern?
            if ((await _context.Shops.FindAsync(shopId)) != null)
            {
                // var Article = await _context.Articles.FindAsync(id);
                var Article = await _context.Articles.Where(a => a.ShopId == shopId && a.Id == id).Select(a => new ArticleDTO()
                {
                    Id = a.Id,
                    Name = a.Name,
                    EuroPrice = a.EuroPrice
                }).FirstAsync();

                if (Article == null)
                {
                    return NotFound();
                    // TODO nicer NotFound message (i.e. descriptive error)
                    // proves more difficult than anticipated
                }

                return Article;
            }
            return NotFound();
        }

        // PUT: api/shops/1/Articles/2
        [HttpPut("{shopId}/[controller]/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutArticle([FromRoute] int shopId, [FromRoute] int id, [FromBody] ArticleDTO ArticleDTO)
        {
            // TODO Auslagern?
            if ((await _context.Shops.FindAsync(shopId)) != null)
            {
                // compare url against body
                if (id != ArticleDTO.Id)
                {
                    return BadRequest();
                }

                var Article = await _context.Articles.Where(a => a.ShopId == shopId).SingleOrDefaultAsync(b => b.Id == id);

                // if search returned null there is either no item with that id or no item with that id within the shop
                if (Article == null)
                {
                    return NotFound();
                }

                Article.Name = ArticleDTO.Name;
                Article.EuroPrice = ArticleDTO.EuroPrice;

                _context.Entry(Article).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(id))
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
            else
            {
                return NotFound(ShopNotFoundContent);
            }

        }

        // POST: api/Articles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{shopId}/[controller]")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Article>> PostArticle(Article Article)
        {
            _context.Articles.Add(Article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = Article.Id }, Article);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{shopId}/[controller]/{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var Article = await _context.Articles.FindAsync(id);
            if (Article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(Article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }

        private bool ArticleIdIsInShop(int shopId, int id)
        {
            return _context.Articles.Where(a => a.ShopId == shopId && a.Id == id).ToList().Count == 1;
        }
    }
}
