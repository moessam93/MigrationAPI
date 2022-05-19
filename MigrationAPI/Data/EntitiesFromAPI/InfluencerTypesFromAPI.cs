using MigrationAPI.Data.Entities;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MigrationAPI.Data.EntitiesFromAPI
{
    public class InfluencerTypesFromAPI
    {
        [JsonPropertyName("response")]
        public List<InfluencerTypeFromAPI> InfluencerTypes { get; set; }
    }
    public class InfluencerTypeFromAPI
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("followers")]
        public int Followers { get; set; }
        [JsonPropertyName ("key")]
        public string Key { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

}
