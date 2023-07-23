namespace URLShortener.Models
{
    public class UrlMapping
    {
        public Guid Id { get; set; }
        public string LongUrl { get; set; }
        public string ShortUrl { get; set; }
        public int Clicks { get; set; }
    }
}