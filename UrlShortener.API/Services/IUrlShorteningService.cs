namespace UrlShortener.API.Services
{
    public interface IUrlShorteningService
    {
        Task<string> ShortenUrl(string url, string? alias);
        Task<string?> FindUrl(string code);
    }
}
