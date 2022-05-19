using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MigrationAPI.Data.EntitiesFromAPI
{
    public class CountriesFromAPI
    {
        [JsonPropertyName("response")]
        public List<CountryFromAPI> Countries { get; set; }
    }
    public class CountryFromAPI
    {
        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("key")]
        public string IsoTwoCode { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
