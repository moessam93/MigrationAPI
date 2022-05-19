using Microsoft.AspNetCore.Mvc;
using MigrationAPI.Data;
using MigrationAPI.Data.Entities;
using MigrationAPI.Data.EntitiesFromAPI;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace MigrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialPlatformsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        public SocialPlatformsController(DataContext context, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _context = context;
            _clientFactory = clientFactory;
            _configuration = configuration;

        }
        //Migrate Social Platforms
        [HttpPost]
        public async Task<List<SocialPlatforms>> MigrateSocialPlatforms()
        {
            var urlSocialPlatforms = _configuration.GetSection("APIURI").GetSection("Lookups").Value + "/SocialPlatform";
            var clientAr = _clientFactory.CreateClient();
            clientAr.DefaultRequestHeaders.Add("language", "ar");
            var clientEn = _clientFactory.CreateClient();
            clientEn.DefaultRequestHeaders.Add("language", "en");
            var responseSocialPlatformsAr = await clientAr.GetAsync(urlSocialPlatforms);
            var responseSocialPlatformsArBody = await responseSocialPlatformsAr.Content.ReadAsStringAsync();
            var serializedSocialPlatformsAr = JsonSerializer.Deserialize<SocialPlatformsFromAPI>(responseSocialPlatformsArBody);
            var socialPlatformsFromAPIAr = serializedSocialPlatformsAr.SocialPlatforms;

            var responseSocialPlatformsEn = await clientEn.GetAsync(urlSocialPlatforms);
            var responseSocialPlatformsEnBody = await responseSocialPlatformsEn.Content.ReadAsStringAsync();
            var serializedSocialPlatformsEn = JsonSerializer.Deserialize<SocialPlatformsFromAPI>(responseSocialPlatformsEnBody);
            var socialPlatformsFromAPIEn = serializedSocialPlatformsEn.SocialPlatforms;
            
            //map social platforms
            var socialPlatforms = new List<SocialPlatforms>();
            foreach (var socialPlatform in socialPlatformsFromAPIAr)
            {
                var _socialPlatform = new SocialPlatforms();
                _socialPlatform.Key = socialPlatform.Key;
                _socialPlatform.Deleted = false;
                _socialPlatform.IsLinkingEnabled = socialPlatform.IsLinkingEnabled;
                _socialPlatform.NameAr = socialPlatform.Name;
                socialPlatforms.Add(_socialPlatform);
            }
            
            foreach (var socialPlatform in socialPlatforms)
            {
                foreach (var _socialPlatform in socialPlatformsFromAPIEn)
                {
                    if (socialPlatform.Key == _socialPlatform.Key)
                    {
                        socialPlatform.NameEn = _socialPlatform.Name;
                    }
                }
            }

            //add to table SocialPlatforms
            var socialPlatformsFromDB = _context.SocialPlatforms.ToList();
            foreach (var socialPlatform in socialPlatforms)
            {
                    if (!socialPlatformsFromDB.Any(x=> x.Key == socialPlatform.Key))
                    {
                        _context.SocialPlatforms.Add(socialPlatform);
                    }
            }
            await _context.SaveChangesAsync();
            return _context.SocialPlatforms.ToList();
        }
    }
}
