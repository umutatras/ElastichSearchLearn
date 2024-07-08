using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Elasticsearch.WEB.ViewModel
{
    public class BlogCreateViewModel
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Content { get; set; } = null!;
        [Required]
        public List<string> Tags { get; set; } = new();
    }
}
