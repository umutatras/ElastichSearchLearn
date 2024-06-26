using ElasticSearch.API.Models.ECommerce;
using ElasticSearch.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

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
           var result= await _repository.TermQuery(customerfirstName);
            return result;
        }
    }
}
