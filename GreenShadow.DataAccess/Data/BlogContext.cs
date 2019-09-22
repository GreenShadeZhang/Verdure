using GreenShade.Blog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreenShade.Blog.DataAccess.Data
{
    public class BlogContext:DbContext
    {
        
        public BlogContext(DbContextOptions<BlogContext> options):base(options)
        {
        }
        public DbSet<Article> Articles { get; set; }
    }
}
