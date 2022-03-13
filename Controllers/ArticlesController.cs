using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using a02_shopsystem.Model;
using a02_shopsystem.DTO;

namespace a02_shopsystem.Controllers
{
    [Route("api/shops/{shopId:int}/[controller]/")]
    [ApiController]
    [Produces("application/json")]
    public class ArticlesController : ControllerBase
    {
        private readonly ShopsystemContext _context;
        private static Object ShopNotFoundContent = new { status = StatusCodes.Status404NotFound, title = "Not found", error = "No shop with the given id found." };

        public ArticlesController(ShopsystemContext context)
        {
            _context = context;
        }

        // GET: api/shops/1/articles
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles([FromRoute] int shopId)
        {
            if ((await _context.Shops.FindAsync(shopId)) != null)
            {
                return await _context.Articles.Where(a => a.ShopId == shopId).ToListAsync();
            }
            else
            {
                return NotFound(ShopNotFoundContent);
            }
        }

        // GET: api/shops/1/Articles/2
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Article>> GetArticle([FromRoute] int shopId, [FromRoute] int id)
        {
            if ((await _context.Shops.FindAsync(shopId)) != null)
            {
                var article = await _context.Articles.Where(a => a.ShopId == shopId && a.Id == id).FirstOrDefaultAsync();

                if (article == null)
                {
                    return NotFound();
                }

                return article;
            }
            return NotFound();
        }

        // PUT: api/shops/1/Articles/2
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutArticle([FromRoute] int shopId, [FromRoute] int id, [FromBody] ArticleUpdateDTO articleUpdateDTO)
        {
            if ((await _context.Shops.FindAsync(shopId)) != null)
            {
                // compare url against body
                if (id != articleUpdateDTO.Id)
                {
                    return BadRequest();
                }

                if (articleUpdateDTO.Name.Trim().Length == 0)
                {
                    return BadRequest();
                }

                var article = await _context.Articles.Where(a => a.ShopId == shopId).SingleOrDefaultAsync(b => b.Id == id);

                // if search returned null there is either no item with that id or no item with that id within the shop
                if (article == null)
                {
                    return NotFound();
                }

                article.Name = articleUpdateDTO.Name.Trim();
                article.EuroPrice = articleUpdateDTO.EuroPrice;

                _context.Entry(article).State = EntityState.Modified;

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

        // POST: api/shops/1/Articles
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Article>> PostArticle([FromRoute] int shopId, [FromBody] ArticleCreationDTO articleCreationDTO)
        {
            if ((await _context.Shops.FindAsync(shopId)) != null)
            {

                if (articleCreationDTO.Name.Trim().Length == 0)
                {
                    return BadRequest();
                }

                Article article = new Article()
                {
                    Name = articleCreationDTO.Name.Trim(),
                    EuroPrice = articleCreationDTO.EuroPrice,
                    ShopId = shopId
                };

                _context.Articles.Add(article);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetArticle), new { id = article.Id, shopId = article.ShopId }, article);
            }
            return NotFound(ShopNotFoundContent);
        }

        // DELETE: api/shops/1/Articles/2
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteArticle([FromRoute] int shopId, [FromRoute] int id)
        {
            if ((await _context.Shops.FindAsync(shopId)) != null)
            {

                var article = await _context.Articles.Where(a => a.ShopId == shopId && a.Id == id).FirstOrDefaultAsync();

                if (article == null)
                {
                    return NotFound();
                }

                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            return NotFound(ShopNotFoundContent);
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
