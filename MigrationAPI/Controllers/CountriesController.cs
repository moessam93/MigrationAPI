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
    public class CountriesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        public CountriesController(DataContext context, IHttpClientFactory clientFactory, IConfiguration _confgiuration)
        {
            _context = context;
            _clientFactory = clientFactory;
            _configuration = _confgiuration;
        }
        [HttpPost]
        public async Task<List<Countries>> MigrateCountries()
        {
            var urlCountries = _configuration.GetSection("APIURI").GetSection("Lookups").Value + "/Country";
            var clientAr = _clientFactory.CreateClient();
            clientAr.DefaultRequestHeaders.Add("language", "ar");
            var clientEn = _clientFactory.CreateClient();
            clientEn.DefaultRequestHeaders.Add("language", "en");
            var responseCountriesAr = await clientAr.GetAsync(urlCountries); ;
            var responseCountriesArBody = await responseCountriesAr.Content.ReadAsStringAsync();
            var serializedCountriesAr = JsonSerializer.Deserialize<CountriesFromAPI>(responseCountriesArBody);
            var countriesFromAPIAr = serializedCountriesAr.Countries;

            var responseCountriesEn = await clientEn.GetAsync(urlCountries);
            var responseCountriesEnBody = await responseCountriesEn.Content.ReadAsStringAsync();
            var serializedCountriesEn = JsonSerializer.Deserialize<CountriesFromAPI>(responseCountriesEnBody);
            var countriesFromAPIEn = serializedCountriesEn.Countries;

            //map countries names
            var countries = new List<Countries>();
            foreach (var country in countriesFromAPIAr)
            {
                var _country = new Countries();
                _country.CountryCode = country.CountryCode;
                _country.IsoTwoCode = country.IsoTwoCode;
                _country.NameAr = country.Name;
                countries.Add(_country);
            }

            foreach (var country in countries)
            {
                foreach (var _country in countriesFromAPIEn)
                {
                    if (country.IsoTwoCode == _country.IsoTwoCode)
                    {
                        country.NameEn = _country.Name;
                    }
                }
            }

            //add to table Countries
            var countriesFromDB = _context.Countries.ToList();
            foreach (var country in countries)
            {
                if (!countriesFromDB.Any(x => x.IsoTwoCode == country.IsoTwoCode) && country.NameEn!=null && country.NameAr!=null)
                {
                    _context.Countries.Add(country);
                }
            }
            await _context.SaveChangesAsync();

            return _context.Countries.ToList();
        }
    }
}
