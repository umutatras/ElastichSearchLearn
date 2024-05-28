using ElasticSearch.API.Enums;

namespace ElasticSearch.API.Models
{
    public class ProductFeature
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public ColorEnum Color { get; set; }
    }
}
