using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using ElasticSearch.API.Models.ECommerce;
using System.Collections.Immutable;

namespace ElasticSearch.API.Repositories
{
    public class ECommerceRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "kibana_sample_data_ecommerce";

        public ECommerceRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<ImmutableList<ECommerce>> TermQuery(string customerFirstName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Term(t => t.Field("customer_first_name.keyword").Value(customerFirstName))));
            var ikinciYol=await _client.SearchAsync<ECommerce>(s=>s.Index(indexName).Query(q=>q.Term(t=>t.CustomerFirstName.Suffix("keyword"),customerFirstName)));
            var ucuncuYol = new TermQuery("customer_first_name.keyword") { Value = customerFirstName, CaseInsensitive = true };
            var ucuncuYolBitis=await _client.SearchAsync<ECommerce>(s=>s.Index(indexName).Query(ucuncuYol));
            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;
            }
            return  result.Documents.ToImmutableList();
        }
       
    }
}
