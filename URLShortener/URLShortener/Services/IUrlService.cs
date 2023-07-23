using URLShortener.Models;

namespace URLShortener.Services
{
    public interface IUrlService
    {
        IEnumerable<UrlMapping> GetAllMappings();
        UrlMapping GetUrlMappingByShortUrl(string shortUrl);
        void AddMapping(UrlMapping mapping);
        void UpdateMapping(UrlMapping mapping);
        string GenerateUniqueShortUrl();
    }
}
