using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MigrationAPI.Data.EntitiesFromAPI
{
    public class SocialPlatformsFromAPI
    {
        [JsonPropertyName("response")]
        public List<SocialPlatformFromAPI> SocialPlatforms { get; set; }
    }
    public class SocialPlatformFromAPI
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("displayOrder")]
        public int DisplayOrder { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("isLinkingEnabled")]
        public bool IsLinkingEnabled { get; set; }
    }
}
