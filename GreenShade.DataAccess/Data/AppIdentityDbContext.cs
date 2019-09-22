using GreenShade.Blog.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreenShade.Blog.DataAccess.Data
{
    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options)
        {
        }
    }
}
