using ElasticSearch.API.Models.ECommerce;
using ElasticSearch.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.Globalization;

namespace ElasticSearch.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ECommerceController : ControllerBase
    {
        private readonly ECommerceRepository _repository;

        public ECommerceController(ECommerceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ImmutableList<ECommerce>> TermQuery(string customerfirstName)
        {
            var result = await _repository.TermQuery(customerfirstName);
            return result;
        }
        [HttpPost]
        public async Task<ImmutableList<ECommerce>> TermsQuery(List<string> customerfirstNameList)
        {
            var result = await _repository.TermsQuery(customerfirstNameList);
            return result;
        }
        [HttpGet]
        public async Task<ImmutableList<ECommerce>> TermsQuery(string customerFullName)
        {
            var result = await _repository.PrefixQuery(customerFullName);
            return result;
        }
        [HttpGet]
        public async Task<ImmutableList<ECommerce>> RangeQuery(double from, double to)
        {
            var result = await _repository.RangeQuery(from, to);
            return result;
        }
        [HttpGet]
        public async Task<ImmutableList<ECommerce>> MatchAll()
        {
            var result = await _repository.MathcAllAsync();
            return result;
        }
        [HttpGet]
        public async Task<ImmutableList<ECommerce>> Pagination(int page, int pageSize)
        {
            var result = await _repository.PaginationQuery(page, pageSize);
            return result;
        }

        [HttpGet]
        public async Task<ImmutableList<ECommerce>> WildCard(string customerFullName)
        {
            var result = await _repository.WildCardQuery(customerFullName);
            return result;
        }
        [HttpGet]
        public async Task<ImmutableList<ECommerce>> FuzzyQuery(string customerName)
        {
            var result = await _repository.FuzzyQuery(customerName);
            return result;
        }
        [HttpGet]
        public async Task<ImmutableList<ECommerce>> MatchQueryFullText(string categoryName)
        {
            var result = await _repository.MatchQueryFullTextAsync(categoryName);
            return result;
        }
        [HttpGet]
        public async Task<ImmutableList<ECommerce>> MatchBoolPrefixFullText(string customerFullName)
        {
            var result = await _repository.MatchBoolPrefixFullTextAsync(customerFullName);
            return result;
        }
        [HttpGet]
        public async Task<ImmutableList<ECommerce>> MatchPhraseFullText(string customerFullName)
        {
            var result = await _repository.MatchPhraseFullTextAsync(customerFullName);
            return result;
        }     [HttpGet]
        public async Task<ImmutableList<ECommerce>> CompoundQuery(string customerFullName, string cityName, string categoryName)
        {
            var result = await _repository.CompoundQueryAsync(customerFullName, cityName, categoryName);
            return result;
        }
        public async Task<ImmutableList<ECommerce>> CompoundTwoQuery(string customerFullName, string cityName, string categoryName)
        {
            var result = await _repository.CompundQueryTwo(customerFullName);
            return result;
        }
    }
}
