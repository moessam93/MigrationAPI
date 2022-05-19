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

    public class SocialPlatformPostTypesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        public SocialPlatformPostTypesController(DataContext context, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _context = context;
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<List<SocialPlatformPostTypes>> MigrateSocialPlatformPostTypes()
        {
            var urlSocialPlatformPostTypes = _configuration.GetSection("APIURI").GetSection("Lookups").Value + "/SocialPlatformPostType";
            var urlSocialPlatforms = _configuration.GetSection("APIURI").GetSection("Lookups").Value + "/SocialPlatform";
            var clientAr = _clientFactory.CreateClient();
            clientAr.DefaultRequestHeaders.Add("language", "ar");
            var clientEn = _clientFactory.CreateClient();
            clientEn.DefaultRequestHeaders.Add("language", "en");
            var responseSocialPlatformPostTypesAr = await clientAr.GetAsync(urlSocialPlatformPostTypes);
            var responseSocialPlatformPostTypesArBody = await responseSocialPlatformPostTypesAr.Content.ReadAsStringAsync();
            var serializedSocialPlatformPostTypesAr = JsonSerializer.Deserialize<SocialPlatformPostTypesFromAPI>(responseSocialPlatformPostTypesArBody);
            var socialPlatformPostTypesFromAPIAr = serializedSocialPlatformPostTypesAr.SocialPlatformPostTypes;

            var responseSocialPlatformPostTypesEn = await clientEn.GetAsync(urlSocialPlatformPostTypes);
            var responseSocialPlatformPostTypesEnBody = await responseSocialPlatformPostTypesEn.Content.ReadAsStringAsync();
            var serializedSocialPlatformPostTypesEn = JsonSerializer.Deserialize<SocialPlatformPostTypesFromAPI>(responseSocialPlatformPostTypesEnBody);
            var socialPlatformPostTypesFromAPIEn = serializedSocialPlatformPostTypesEn.SocialPlatformPostTypes;

            var responseSocialPlatformsEn = await clientEn.GetAsync(urlSocialPlatforms);
            var responseSocialPlatformsEnBody = await responseSocialPlatformsEn.Content.ReadAsStringAsync();
            var serializedSocialPlatformsEn = JsonSerializer.Deserialize<SocialPlatformsFromAPI>(responseSocialPlatformsEnBody);
            var socialPlatformsFromAPIEn = serializedSocialPlatformsEn.SocialPlatforms;

            //map social platforms arabic and english names
            var socialPlatformPostTypes = new List<SocialPlatformPostTypes>();
            foreach (var socialPlatformPostType in socialPlatformPostTypesFromAPIAr)
            {
                var _socialPlatformPostType = new SocialPlatformPostTypes();
                _socialPlatformPostType.SocialPlatformId = socialPlatformPostType.SocialPlatformId;
                _socialPlatformPostType.NameAr = socialPlatformPostType.Name;
                _socialPlatformPostType.Key = socialPlatformPostType.Key;
                socialPlatformPostTypes.Add(_socialPlatformPostType);
            }

            foreach (var socialPlatformPostType in socialPlatformPostTypes)
            {
                foreach (var _socialPlatformPostType in socialPlatformPostTypesFromAPIAr)
                {
                    if (socialPlatformPostType.Key == _socialPlatformPostType.Key)
                    {
                        socialPlatformPostType.NameAr = _socialPlatformPostType.Name;
                    }
                }
            }
            foreach (var socialPlatformPostType in socialPlatformPostTypes)
            {
                foreach (var _socialPlatformPostType in socialPlatformPostTypesFromAPIEn)
                {
                    if (socialPlatformPostType.Key == _socialPlatformPostType.Key)
                    {
                        socialPlatformPostType.NameEn = _socialPlatformPostType.Name;
                    }
                }
            }

            //map socialPlatformId
            var socialPlatformsFromDB = _context.SocialPlatforms.ToList();
            foreach (var socialPlatformPostType in socialPlatformPostTypes)
            {
                var _socialPlatformFromAPI = socialPlatformsFromAPIEn.Find(x => x.Id == socialPlatformPostType.SocialPlatformId);

                var _socialPlatformFromDB = socialPlatformsFromDB.Find(x => x.Key == _socialPlatformFromAPI.Key);

                socialPlatformPostType.SocialPlatformId = _socialPlatformFromDB.Id;
                socialPlatformPostType.Id = 0;
            }
            //Add socialPlatform post types to DB
            var socialPlatformPostTypesFromDB = _context.SocialPlatformPostTypes.ToList();

            foreach (var socialPlatformPostType in socialPlatformPostTypes)
            {
                if (!socialPlatformPostTypesFromDB.Any(x => x.SocialPlatformId == socialPlatformPostType.SocialPlatformId) &&
                    socialPlatformPostType.NameEn != null && socialPlatformPostType.NameAr != null)
                {
                    _context.SocialPlatformPostTypes.Add(socialPlatformPostType);
                }
            }
            await _context.SaveChangesAsync();

            return _context.SocialPlatformPostTypes.ToList();
        }

    }
}
