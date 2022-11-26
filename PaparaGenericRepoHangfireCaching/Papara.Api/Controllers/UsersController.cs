using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Papara.Api.Models;
using Papara.Core.Enums;
using Papara.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Papara.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly Func<CacheTech, ICacheService> _cacheService;

        public UsersController(IUserRepository repository, Func<CacheTech, ICacheService> cacheService)
        {
            _repository = repository;
            _cacheService = cacheService;

    }


    //private readonly IUserRepository _repo;

    [HttpGet("GetUserDataFromAPI")]

        public async Task<IActionResult> GetUserDataFromAPI()
        {
            using var client = new HttpClient();
            var responseTask = await client.GetAsync(new Uri("https://jsonplaceholder.typicode.com/posts"));
            if (responseTask.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseString = await responseTask.Content.ReadAsStringAsync();
                var modelResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(responseString);
                foreach (var item in modelResponse)
                {
                    Papara.Core.Entites.User element = new Papara.Core.Entites.User();
                    element.userId = item.userId;
                    //element.id = element.id;
                    element.title = item.title;
                    element.body = item.body;
                    await _repository.AddAsync(element);
                }
                return Ok(responseString);

            }
            return BadRequest();   

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await _repository.GetAllAsync();
            return Ok(customers);
        }




        [HttpPost]
        [Route("BgW")]
        public IActionResult BgW()
        {
            RecurringJob.AddOrUpdate(() => GetUserDataFromAPI(), "*/5 * * * * *");
            return Ok($"5Sec");
        }
    }
}
