using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenShadow.Blog.DataAccess.Data;
using GreenShadow.Blog.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GreenShadow.Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly BlogContext _context;

        public ArticlesController(BlogContext context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            try
            {
                var artList = await _context.Articles.Include(x => x.User).ToListAsync();
                return Ok(artList);
            }
            catch (Exception ex)
            {

            }
            return BadRequest();
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(string id)
        {
            var user = HttpContext.User;
            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        // PUT: api/Articles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(string id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

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

        // POST: api/Articles
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle([FromForm]Article article)
        {
            if (HttpContext.User.Identity.IsAuthenticated && HttpContext.User.Claims != null)
            {
                foreach (var item in HttpContext.User.Claims)
                {
                    if (item.Type == ClaimTypes.NameIdentifier)
                    {
                        article.UserId = item.Value;
                    }
                }
            }

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Article>> DeleteArticle(string id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return article;
        }

        private bool ArticleExists(string id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
