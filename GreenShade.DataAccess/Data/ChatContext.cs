using GreenShade.Blog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreenShade.Blog.DataAccess.Data
{
   public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options)
        {
        }
        public DbSet<ChatGroup> Groups { get; set; }
    }
}
