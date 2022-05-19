using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MigrationAPI.Data.EntitiesFromAPI
{
    public class InterestsFromAPI
    {
        [JsonPropertyName("response")]
        public List<InterestFromAPI> Interests { get; set; }
    }
    public class InterestFromAPI
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

}
