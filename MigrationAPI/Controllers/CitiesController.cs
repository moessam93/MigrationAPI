using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MigrationAPI.Data.DTO;
using MigrationAPI.Data.EntitiesFromAPI;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MigrationAPI.Data.Entities
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        public CitiesController(DataContext context, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _context = context;
            _clientFactory = clientFactory;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<List<Cities>> MigrateCities()
        {
            var urlCities = _configuration.GetSection("APIURI").GetSection("Lookups").Value + "/City";
            var urlCountries = _configuration.GetSection("APIURI").GetSection("Lookups").Value + "/Country";
            var clientAr = _clientFactory.CreateClient();
            clientAr.DefaultRequestHeaders.Add("language", "ar");
            var clientEn = _clientFactory.CreateClient();
            clientEn.DefaultRequestHeaders.Add("language", "en");
            var responseCitiesAr = await clientAr.GetAsync(urlCities);
            var responseCitiesArBody = await responseCitiesAr.Content.ReadAsStringAsync();
            var serializedCitiesAr = JsonSerializer.Deserialize<CitiesFromAPI>(responseCitiesArBody);
            var citiesFromAPIAr = serializedCitiesAr.Cities;

            var responseCitiesEn = await clientEn.GetAsync(urlCities);
            var responseCitiesEnBody = await responseCitiesEn.Content.ReadAsStringAsync();
            var serializedCitiesEn = JsonSerializer.Deserialize<CitiesFromAPI>(responseCitiesEnBody);
            var citiesFromAPIEn = serializedCitiesEn.Cities;

            var responseCountriesEn = await clientEn.GetAsync(urlCountries);
            var responseCountriesEnBody = await responseCountriesEn.Content.ReadAsStringAsync();
            var serializedCountriesEn = JsonSerializer.Deserialize<CountriesFromAPI>(responseCountriesEnBody);
            var countriesFromAPIEn = serializedCountriesEn.Countries;

            //map cities arabic and english names
            var cities = new List<Cities>();
            foreach (var city in citiesFromAPIAr)
            {
                var _city = new Cities();
                _city.CountryId = city.CountryId;
                _city.Id = city.Id;
                _city.NameAr = city.Name;
                cities.Add(_city);
            }

            foreach (var city in cities)
            {
                foreach (var _city in citiesFromAPIEn)
                {
                    if (city.Id == _city.Id)
                    {
                        city.NameEn = _city.Name;
                    }
                }
            }

            //map countryId and add to DB
            var countriesFromDb = _context.Countries.ToList();
            foreach (var city in cities)
            {
                var _countryFromAPI = countriesFromAPIEn.Find(x => x.Id == city.CountryId);
                if (_countryFromAPI != null)
                {
                    var _countryFromDB = countriesFromDb.Find(x => x.IsoTwoCode == _countryFromAPI.IsoTwoCode);
                    city.CountryId = _countryFromDB.Id;
                    city.Id = 0;
                    _context.Cities.Add(city);
                }
                else
                {
                    continue;
                }
            }

            await _context.SaveChangesAsync();

            return _context.Cities.ToList();
        }
    }
}
