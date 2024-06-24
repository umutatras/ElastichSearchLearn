using ElasticSearch.API.Enums;

namespace ElasticSearch.API.DTOs
{
    public record ProductFeatureDto(int Width, int Height, ColorEnum Color);
}
