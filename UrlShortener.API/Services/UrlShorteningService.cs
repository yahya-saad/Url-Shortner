using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Configs;
using UrlShortener.API.Data;
using UrlShortener.API.Models.Entities;

namespace UrlShortener.API.Services
{
    public class UrlShorteningService : IUrlShorteningService
    {
        private readonly Random _random = new Random();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpRequest _request;
        private readonly AppDbContext _context;

        public UrlShorteningService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _request = _httpContextAccessor.HttpContext.Request;
        }

        public async Task<string?> FindUrl(string code)
        {
            var shortenedUrl = await _context.ShortenedUrls.SingleOrDefaultAsync(s => s.Code == code);
            return shortenedUrl?.LongUrl;
        }

        public async Task<string> ShortenUrl(string url, string? alias)
        {
            string code;

            if (string.IsNullOrEmpty(alias))
            {
                code = await GenerateCode();
            }
            else
            {
                bool aliasExists = await _context.ShortenedUrls.AnyAsync(c => c.Code == alias);
                if (aliasExists)
                    throw new ArgumentException("The alias is already in use.");
                code = alias;
            }

            var shortenedUrl = new ShortenedUrl
            {
                Id = Guid.NewGuid(),
                LongUrl = url,
                Code = code,
                ShortUrl = $"{_request.Scheme}://{_request.Host}{_request.PathBase}/{code}",
                CreatedOnUtc = DateTime.UtcNow
            };

            await _context.ShortenedUrls.AddAsync(shortenedUrl);
            await _context.SaveChangesAsync();

            return shortenedUrl.ShortUrl;
        }

        private async Task<string> GenerateCode()
        {
            var codeChars = new char[ShortLinkSettings.Length];
            int maxValue = ShortLinkSettings.Alphabet.Length;

            while (true)
            {
                for (var i = 0; i < ShortLinkSettings.Length; i++)
                {
                    var randomIndex = _random.Next(maxValue);
                    codeChars[i] = ShortLinkSettings.Alphabet[randomIndex];
                }
                var code = new string(codeChars);

                bool isCodeInUse = await _context.ShortenedUrls.AnyAsync(c => c.Code == code);
                if (!isCodeInUse)
                    return code;
            }
        }
    }
}
