using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MigrationAPI.Data.EntitiesFromAPI
{
    public class CitiesFromAPI
    {
        [JsonPropertyName("response")]
        public List<CityFromAPI> Cities { get; set; }
    }
    public class CityFromAPI
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("countryId")]
        public int CountryId { get; set; }
    }
}
