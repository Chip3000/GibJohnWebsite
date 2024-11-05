using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GibJohnWebsite.Models;

namespace GibJohnWebsite.Data
{
    public class GibJohnWebsiteContext : DbContext
    {
        public GibJohnWebsiteContext (DbContextOptions<GibJohnWebsiteContext> options)
            : base(options)
        {
        }

        public DbSet<GibJohnWebsite.Models.AddLessonClass> AddLessonClass { get; set; } = default!;
    }
}
