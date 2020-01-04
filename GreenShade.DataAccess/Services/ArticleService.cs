using GreenShade.Blog.DataAccess.Data;
using GreenShade.Blog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenShade.Blog.DataAccess.Services
{
    public class ArticleService
    {
        private readonly BlogSysContext _context;
        private ILogger<ArticleService> _logger;
        public ArticleService(BlogSysContext context,
            ILogger<ArticleService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<List<Article>> GetArticlesByType(int type = 0, int pi = 1, int ps = 10)
        {
            try
            {
                List<int> types = new List<int>();
                if (type == 0)
                {
                    types.Add(0);
                    types.Add(1);
                }
                else if (type == 1)
                {
                    types.Add(1);
                }
                else if (type == 3)
                {
                    types.Add(0);
                    types.Add(1);
                    types.Add(-1);
                }
                var artList = await _context.Articles.Include(x => x.User)
                    .OrderByDescending(a => a.Type)
                    .OrderByDescending(a => a.ArticleDate)
                    .Where(a => types.Contains(a.Type) && a.Status != -1)
                    .Skip((pi - 1) * ps).Take(ps).ToListAsync();
                return artList;
            }
            catch (Exception ex)
            {
                _logger.LogError("ef get Arts", ex);
            }
            return null;
        }
        public async Task<int> GetArticlesNumByType(int type)
        {
            int ret = 0;
            try
            {
                List<int> types = new List<int>();
                if (type == 0)
                {
                    types.Add(0);
                    types.Add(1);
                }
                else if (type == 1)
                {
                    types.Add(1);
                }
                else if (type == 3)
                {
                    types.Add(0);
                    types.Add(1);
                    types.Add(-1);
                }
                ret = await _context.Articles.Where(a => types.Contains(a.Type) && a.Status != -1).CountAsync();
            }
            catch (Exception ex)
            {

            }
            return ret;
        }

        public async Task<int> GetHotArticlesNum()
        {
            int ret = 0;
            try
            {
                ret = await _context.Articles.Where(x => x.Status == 1).CountAsync();
            }
            catch (Exception ex)
            {

            }
            return ret;
        }

        public async Task<Article> GetArticle(string id)
        {
            var article = await _context.Articles.FindAsync(id);
            try
            {
                if (article == null)
                {
                    return null;
                }
                _context.Entry(article)
             .Reference(b => b.User)
             .Load();
            }
            catch (Exception ex)
            {

            }
            return article;
        }

        public async Task PostArticle(Article article)
        {
            article.ArticleDate = DateTime.Now;
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateArticle(Article article)
        {
            _context.Articles.Update(article);
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
