using Elasticsearch.Net;
using Nest;

namespace ElasticSearch.API.Extensions
{
    public static class ElasticSearch
    {
        public static void AddElastic(this IServiceCollection services,IConfiguration configuration)
        {
            var pool = new SingleNodeConnectionPool(new Uri(configuration.GetSection("Elastic")["Url"]!));
            var settings = new ConnectionSettings(pool);
            //settings.BasicAuthentication(); username,password burada belirtilebilir.
            var client = new ElasticClient(settings);
            services.AddSingleton(client);
        }
    }
}
