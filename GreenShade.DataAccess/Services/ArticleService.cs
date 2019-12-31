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
        private readonly BlogSysContext _context;

        public ArticleService(BlogSysContext context)
        {
            this._context = context;
        }
        public async Task<List<Article>> GetArticles(int status=0,int pi = 1, int ps = 10)
        {
            try
            {
                List<int> statuss = new List<int>();
                if (status == 0)
                {
                    statuss.Add(0);
                    statuss.Add(1);
                }
                else if (status == 1)
                {
                    statuss.Add(1);
                }
                var artList = await _context.Articles.Include(x => x.User).OrderByDescending(a=>a.Status).OrderByDescending(a=>a.ArticleDate).Where(a => statuss.Contains(a.Status)).Skip((pi - 1) * ps).Take(ps).ToListAsync();
                return artList;
            }
            catch (Exception ex)
            {

            }
            return null;
        }     
        public async Task<int> GetArticlesNum(int status)
        {
            int ret = 0;
            try
            {
                List<int> statuss = new List<int>();
                if (status == 0)
                {
                    statuss.Add(0);
                    statuss.Add(1);
                }
                else if (status == 1)
                {
                    statuss.Add(1);
                }
                ret = await _context.Articles.Where(a => statuss.Contains(a.Status)).CountAsync();
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
                ret = await _context.Articles.Where(x=>x.Status==1).CountAsync();
            }
            catch (Exception ex)
            {

            }
            return ret;
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
