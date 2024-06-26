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

        public async Task<ImmutableList<ECommerce>> TermsQuery(List<string> customerFirstNameList)
        {
            List<FieldValue> terms = new List<FieldValue>();
            customerFirstNameList.ForEach(x =>
            {
                terms.Add(x);
            });

            var termsQuery = new TermsQuery()
            {
                Field = "customer_first_name.keyword",
                Terms = new TermsQueryField(terms.AsReadOnly())
            };
            var result=await _client.SearchAsync<ECommerce>(s=>s.Index(indexName).Query(termsQuery));

            var ikinciYol = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Terms(t => t.Field(f => f.CustomerFirstName.Suffix("keyword")).Terms(new TermsQueryField(terms.AsReadOnly())))));
            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;
            }
            return result.Documents.ToImmutableList();
        }

        public async Task<ImmutableList<ECommerce>> PrefixQuery(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Prefix(p=>p.Field(field=>field.CustomerFullName.Suffix("keyword")).Value(customerFullName))));
            return result.Documents.ToImmutableList();

        }
    }
}
