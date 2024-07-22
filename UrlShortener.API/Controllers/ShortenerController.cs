using Microsoft.AspNetCore.Mvc;
using UrlShortener.API.Models.DTOs;
using UrlShortener.API.Services;

namespace UrlShortener.API.Controllers
{
    [ApiController]
    public class ShortenerController(IUrlShorteningService _urlShorteningService) : ControllerBase
    {

        [HttpPost("shorten")]
        public async Task<IActionResult> Shorten(ShortenUrlRequest req)
        {
            if (!Uri.TryCreate(req.Url, UriKind.Absolute, out _))
                return BadRequest("The specified URL is invalid.");

            try
            {
                var shortenedUrl = await _urlShorteningService.ShortenUrl(req.Url, req.Alias);
                return Ok(shortenedUrl);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetAndRedirect(string code)
        {
            var longUrl = await _urlShorteningService.FindUrl(code);

            if (longUrl == null)
                return NotFound();

            return Redirect(longUrl);
        }
    }
}
