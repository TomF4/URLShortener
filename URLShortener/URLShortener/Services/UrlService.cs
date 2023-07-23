using URLShortener.Data;
using URLShortener.Models;

namespace URLShortener.Services
{
    public class UrlService : IUrlService
    {
        private readonly UrlContext _context;

        public UrlService(UrlContext context)
        {
            _context = context;
        }

        public IEnumerable<UrlMapping> GetAllMappings()
        {
            return _context.UrlMappings.ToList();
        }

        public UrlMapping GetUrlMappingByShortUrl(string shortUrl)
        {
            return _context.UrlMappings.SingleOrDefault(urlMapping => urlMapping.ShortUrl == shortUrl);
        }

        public void AddMapping(UrlMapping mapping)
        {
            _context.UrlMappings.Add(mapping);
            _context.SaveChanges();
        }

        public void UpdateMapping(UrlMapping mapping)
        {
            _context.UrlMappings.Update(mapping);
            _context.SaveChanges();
        }

        public string GenerateUniqueShortUrl()
        {
            string shortUrl;
            do
            {
                shortUrl = Guid.NewGuid().ToString().Substring(0, 5);
            } while (_context.UrlMappings.Any(m => m.ShortUrl == shortUrl));

            return shortUrl;
        }
    }
}