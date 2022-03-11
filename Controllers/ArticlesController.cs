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

        public ArticlesController(ShopsystemContext context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet("{shopId:int}/articles")]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetArticles(int shopId)
        {
            if ((await _context.Shops.FindAsync(shopId)) != null)
            {
                var articleDTOQuery = from article in _context.Articles
                                      where article.ShopId == shopId
                                      select new ArticleDTO()
                                      {
                                          Id = article.Id,
                                          Name = article.Name,
                                          EuroPrice = article.EuroPrice
                                      };

                return await articleDTOQuery.ToListAsync();
            } else {
                return NotFound(new { message = "Not found", error = "No shop with the given id found."});
            }

        }

        // GET: api/Articles/5
        [HttpGet("{shopId:int}/[controller]/{id:int}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var Article = await _context.Articles.FindAsync(id);

            if (Article == null)
            {
                return NotFound();
                // TODO nicer NotFound message (i.e. descriptive error)
                // proves more difficult than anticipated
            }

            return Article;
        }

        // PUT: api/Articles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{shopId}/[controller]/{id}")]
        public async Task<IActionResult> PutArticle(int id, Article Article)
        {
            if (id != Article.Id)
            {
                return BadRequest();
            }

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
    }
}
