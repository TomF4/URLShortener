using Microsoft.AspNetCore.Mvc;
using System;
using URLShortener.Models;
using URLShortener.Services;

namespace URLShortener.Controllers
{
    /// <summary>
    /// URL Shortening ->
    ///     1. Take url and generate a id
    ///     2. store them as a pair
    ///     3. look up id to find pair
    ///     4. redirect to url
    /// </summary>
    

    public class UrlController : Controller
    {
        private readonly IUrlService _urlService;

        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        public IActionResult Index(string shortUrl)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            ViewData["ShortUrl"] = shortUrl ?? string.Empty;

            var displayMappings = _urlService.GetAllMappings()
                .Select(mapping => new DisplayUrlMapping
                {
                    LongUrl = mapping.LongUrl.Length > 64 ? $"{mapping.LongUrl[..64]}..." : mapping.LongUrl, // max 64 characters for viewing
                    FullShortUrl = $"{baseUrl}/{mapping.ShortUrl}",
                    Clicks = mapping.Clicks
                }).ToList();
            ViewData["Urls"] = displayMappings;
            
            return View();
        }

        [HttpPost]
        public IActionResult Create(string longUrl)
        {
            Uri result;
            if (!Uri.TryCreate(longUrl, UriKind.Absolute, out result) ||
                (result.Scheme != Uri.UriSchemeHttp && result.Scheme != Uri.UriSchemeHttps))
            {
                ModelState.AddModelError("longUrl", "Invalid URL");
                return View("Index");
            }

            var uniqueId = _urlService.GenerateUniqueShortUrl();
            var url = new UrlMapping
            {
                ShortUrl = $"{uniqueId}",
                LongUrl = longUrl,
                Clicks = 0
            };

            _urlService.AddMapping(url);
            return RedirectToAction("Index", new { shortUrl = url.ShortUrl });
        }

        [HttpGet]
        [Route("{shortUrlId}")]
        public IActionResult RedirectToLongUrl(string shortUrlId)
        {
            var url = _urlService.GetUrlMappingByShortUrl(shortUrlId);
            if (url == null)
            {
                return NotFound();
            }

            url.Clicks++;
            _urlService.UpdateMapping(url);
            return Redirect(url.LongUrl);
        }
    }
}
