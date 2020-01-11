using GreenShade.Blog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreenShade.Blog.DataAccess.Data
{
    public class BlogSysContext:DbContext
    {
        
        public BlogSysContext(DbContextOptions<BlogSysContext> options):base(options)
        {
        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ChatGroup> Groups { get; set; }
        public DbSet<ChatMassage> ChatMassages { get; set; }
        public DbSet<WnsPushUrl> WnsUrls { get; set; }
        
    }
}
