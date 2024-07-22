namespace UrlShortener.API.Models.DTOs
{
    public class ShortenUrlRequest
    {
        public string Url { get; set; } = string.Empty;
        public string? Alias { get; set; }

    }
}
