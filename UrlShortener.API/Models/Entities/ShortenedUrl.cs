using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UrlShortener.API.Configs;

namespace UrlShortener.API.Models.Entities
{
    [Index(nameof(Code))]
    public class ShortenedUrl
    {
        public Guid Id { get; set; }

        public string LongUrl { get; set; } = string.Empty;

        public string ShortUrl { get; set; } = string.Empty;
        [MaxLength(ShortLinkSettings.Length)]
        public string Code { get; set; } = string.Empty;

        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    }
}
