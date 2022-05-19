using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MigrationAPI.Data;
using MigrationAPI.Data.Entities;
using MigrationAPI.Data.EntitiesFromAPI;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MigrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        public InterestsController(DataContext context, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _context = context;
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<List<Interests>> MigrateInterests()
        {
            var urlInterests = _configuration.GetSection("APIURI").GetSection("Lookups").Value + "/Interest";
            var clientAr = _clientFactory.CreateClient();
            clientAr.DefaultRequestHeaders.Add("language", "ar");
            var clientEn = _clientFactory.CreateClient();
            clientEn.DefaultRequestHeaders.Add("language", "en");

            var responseInterestsAr = await clientAr.GetAsync(urlInterests);
            var responseInterestsArBody = await responseInterestsAr.Content.ReadAsStringAsync();
            var serializedInterestsAr = JsonSerializer.Deserialize<InterestsFromAPI>(responseInterestsArBody);
            var interestsFromAPIAr = serializedInterestsAr.Interests;

            var responseInterestsEn = await clientEn.GetAsync(urlInterests);
            var responseInterestsEnBody = await responseInterestsEn.Content.ReadAsStringAsync();
            var serializedInterestsEn = JsonSerializer.Deserialize<InterestsFromAPI>(responseInterestsEnBody);
            var interestsFromAPIEn = serializedInterestsEn.Interests;

            //map interests names
            var interests = new List<Interests>();
            foreach (var interest in interestsFromAPIAr)
            {
                var _interest = new Interests();
                _interest.Key = interest.Key;
                _interest.NameAr = interest.Name;
                interests.Add(_interest);
            }

            foreach (var interest in interests)
            {
                foreach (var _interest in interestsFromAPIEn)
                {
                    if (interest.Key == _interest.Key)
                    {
                        interest.NameEn = _interest.Name;
                    }
                }
            }

            //Add to DB
            var interestsFromDB = _context.Interests.ToList();
            foreach (var interest in interests)
            {
                if (!interestsFromDB.Any(x => x.Key == interest.Key) && interest.NameEn != null && interest.NameAr != null)
                {
                    _context.Interests.Add(interest);
                }
            }
            await _context.SaveChangesAsync();

            return _context.Interests.ToList();

        }
    }
}
