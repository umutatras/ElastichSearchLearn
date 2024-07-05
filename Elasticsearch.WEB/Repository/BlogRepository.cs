using Elastic.Clients.Elasticsearch;
using Elasticsearch.WEB.Models;

namespace Elasticsearch.WEB.Repository
{
    public class BlogRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "blog";
        public BlogRepository(ElasticsearchClient client)
        {
            _client = client;
        }
        public async Task<Blog?> SaveAsync(Blog newBlog)
        {
            newBlog.Created = DateTime.Now;
            var response = await _client.IndexAsync(newBlog, x => x.Index(indexName).Id(Guid.NewGuid().ToString()));
            if (!response.IsValidResponse)
            {
                return null;
            }
            newBlog.Id=response.Id;
            return newBlog;
        }
    }
}
