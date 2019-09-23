using GreenShade.Blog.DataAccess.Data;
using GreenShade.Blog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenShade.Blog.DataAccess.Services
{
    public class ArticleService
    {
        private readonly BlogContext _context;

        public ArticleService(BlogContext context)
        {
            this._context = context;
        }
        public async Task<List<Article>> GetArticles(int pi = 1, int ps = 10)
        {
            try
            {
                var artList = await _context.Articles.Include(x => x.User).Skip((pi - 1) * ps).Take(ps).ToListAsync();
                return artList;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<Article> GetArticle(string id)
        {
            var article = await _context.Articles.FindAsync(id);
            _context.Entry(article)
        .Reference(b => b.User)
        .Load();
            if (article == null)
            {
                return null;
            }

            return article;
        }

        public async Task PostArticle(Article article)
        {
            article.ArticleDate = DateTime.UtcNow;
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
        }

        public async Task<Article> DeleteArticle(string id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return null;
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return article;
        }

        public bool ArticleExists(string id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
