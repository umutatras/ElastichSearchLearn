
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace ElasticSearch.API.Extensions
{
    public static class ElasticSearchExt
    {
        public static void AddElastic(this IServiceCollection services,IConfiguration configuration)
        {
            var userName = configuration.GetSection("Elastic")["Username"];
            var password = configuration.GetSection("Elastic")["Password"];
            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"]!)).Authentication(new BasicAuthentication(userName!,password!));
           
            var client=new ElasticsearchClient(settings);

            //nest için config
            //var pool = new SingleNodeConnectionPool(new Uri(configuration.GetSection("Elastic")["Url"]!));
            //var settings = new ConnectionSettings(pool);
            ////settings.BasicAuthentication(); username,password burada belirtilebilir.
            //var client = new ElasticClient(settings);
            services.AddSingleton(client);
        }
    }
}
