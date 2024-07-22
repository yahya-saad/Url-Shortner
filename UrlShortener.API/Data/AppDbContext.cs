using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Models.Entities;

namespace UrlShortener.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    }
}
