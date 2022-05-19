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
    public class InfluencerTypesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        public InfluencerTypesController(DataContext context, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _context = context;
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<List<InfluencerTypes>> MigrateInfluencerTypes()
        {
            var urlInfluencerTypes = _configuration.GetSection("APIURI").GetSection("Lookups").Value + "/InfluencerType";
            var clientAr = _clientFactory.CreateClient();
            clientAr.DefaultRequestHeaders.Add("language", "ar");
            var clientEn = _clientFactory.CreateClient();
            clientEn.DefaultRequestHeaders.Add("language", "en");

            var responseInfluencerTypesAr = await clientAr.GetAsync(urlInfluencerTypes);
            var responseInfluencerTypesArBody = await responseInfluencerTypesAr.Content.ReadAsStringAsync();
            var serializedInfluencerTypesAr = JsonSerializer.Deserialize<InfluencerTypesFromAPI>(responseInfluencerTypesArBody);
            var influencerTypesFromAPIAr = serializedInfluencerTypesAr.InfluencerTypes;

            var responseInfluencerTypesEn = await clientEn.GetAsync(urlInfluencerTypes);
            var responseInfluencerTypesEnBody = await responseInfluencerTypesEn.Content.ReadAsStringAsync();
            var serializedInfluencerTypesEn = JsonSerializer.Deserialize<InfluencerTypesFromAPI>(responseInfluencerTypesEnBody);
            var influencerTypesFromAPIEn = serializedInfluencerTypesEn.InfluencerTypes;

            //map InfluencerTypes names
            var influencerTypes = new List<InfluencerTypes>();
            foreach (var influencerType in influencerTypesFromAPIAr)
            {
                var _influencerType = new InfluencerTypes();
                _influencerType.Id= influencerType.Id;
                _influencerType.Key= influencerType.Key;
                _influencerType.NameAr = influencerType.Name;
                influencerTypes.Add(_influencerType);
            }

            foreach (var influencerType in influencerTypes)
            {
                foreach (var _influencerType in influencerTypesFromAPIEn)
                {
                    if (influencerType.Id == _influencerType.Id)
                    {
                        influencerType.NameEn = _influencerType.Name;
                    }
                }
            }

            foreach (var influencerType in influencerTypes)
            {
                influencerType.Id = 0;
            }
            
            //Add influencer types to DB
            var influencerTypesFromDB = _context.InfluencerTypes.ToList();


            foreach (var influencerType in influencerTypes)
            {
                if (!influencerTypesFromDB.Any(x => x.Key == influencerType.Key) && influencerType.NameEn != null && influencerType.NameAr != null)
                {
                    _context.InfluencerTypes.Add(influencerType);
                }
            }
            await _context.SaveChangesAsync();

            return _context.InfluencerTypes.ToList();


        }
    }
}
