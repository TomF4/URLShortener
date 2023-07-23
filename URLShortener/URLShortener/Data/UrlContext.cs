using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using URLShortener.Models;

namespace URLShortener.Data
{
    public class UrlContext : DbContext
    {
        public UrlContext(DbContextOptions<UrlContext> options)
            : base(options)
        {
        }

        public DbSet<UrlMapping> UrlMappings { get; set; }
    }
}
