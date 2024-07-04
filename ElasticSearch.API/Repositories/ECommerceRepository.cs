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
        public async Task<ImmutableList<ECommerce>> RangeQuery(double fromPrice, double toPrice)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName)
            .Query(q => q.
            Range(r => r.
            NumberRange(nr => nr.
            Field(f => f.TaxfulTotalPrice).
            Gt(fromPrice).
            Lte(toPrice)))));
            return result.Documents.ToImmutableList();

        }

        public async Task<ImmutableList<ECommerce>> MathcAllAsync()
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Size(100).Query(q => q.MatchAll()));

            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;

            }
            return result.Documents.ToImmutableList();

        }
        public async Task<ImmutableList<ECommerce>> PaginationQuery(int page,int pageSize)
        {
            var pageFrom = (page - 1) * pageSize;
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Size(pageSize).From(pageFrom).Query(q => q.MatchAll()));

            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;

            }
            return result.Documents.ToImmutableList();

        }

        public async Task<ImmutableList<ECommerce>> WildCardQuery(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Wildcard(w => w.Field(f => f.CustomerFullName.Suffix("keyword")).Wildcard(customerFullName))));
            foreach (var hit in result.Hits)
            {
                hit.Source.Id=hit.Id;

            }
            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> FuzzyQuery(string customerName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Fuzzy(fu=>fu.Field(f=>f.CustomerFirstName.Suffix("keyword")).Value(customerName).Fuzziness(new Fuzziness(1)))).Sort(sort=>sort.Field(field=>field.TaxfulTotalPrice,new FieldSort() { Order=SortOrder.Desc})));
            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;

            }
            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> MatchQueryFullTextAsync(string categoryName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Match(m => m.Field(f => f.Category).Query(categoryName).Operator(Operator.And))));
            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;

            }
            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> MatchBoolPrefixFullTextAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.MatchBoolPrefix(m => m.Field(f => f.CustomerFullName).Query(customerFullName))));
            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;

            }
            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> MatchPhraseFullTextAsync(string customerFullName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.MatchPhrase(m => m.Field(f => f.CustomerFullName).Query(customerFullName))));
            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;

            }
            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> CompoundQueryAsync(string customerFullName,string cityName,string categoryName)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Bool(b=>b.Must(m=>m.Term(t=>t.Field("geoip.city_name").Value(cityName))).MustNot(mn=>mn.Range(r=>r.NumberRange(nr=>nr.Field(f=>f.TaxfulTotalPrice).Lt(100)))).Should(s=>s.Term(t=>t.Field(f=>f.Category.Suffix("keyword")).Value(categoryName)))
            .Filter(f=>f.Term(t=>t.Field("manufacturer.keyword").Value("Tigress Enterprises"))))));
            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;

            }
            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> CompundQueryTwo(string customerFullName)
        {
            //var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Size(100).Query(q => q.MatchPhrasePrefix(m => m.Field(f => f.CustomerFullName).Query(customerFullName))));
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.Bool(b=>b.Should(m=>m.Match(ma=>ma.Field(f=>f.CustomerFullName).Query(customerFullName)).Prefix(p=>p.Field(f=>f.CustomerFullName.Suffix("keyword")).Value(customerFullName))))));
            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;

            }
            return result.Documents.ToImmutableList();
        }
        public async Task<ImmutableList<ECommerce>> MultiMatchQueryFullTextAsync(string name)
        {
            var result = await _client.SearchAsync<ECommerce>(s => s.Index(indexName).Query(q => q.MultiMatch(mm=>mm.Fields(new Field("customer_firs_name").And(new Field("customer_last_name")).And(new Field("customer_fullname"))).Query(name))));
            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;

            }
            return result.Documents.ToImmutableList();
        }



    }
}
