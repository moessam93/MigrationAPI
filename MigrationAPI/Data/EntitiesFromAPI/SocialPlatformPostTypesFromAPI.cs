using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MigrationAPI.Data.EntitiesFromAPI
{
    public class SocialPlatformPostTypesFromAPI
    {
        [JsonPropertyName("response")]
        public List<SocialPlatformPostTypeFromAPI> SocialPlatformPostTypes { get; set; }
    }
    public class SocialPlatformPostTypeFromAPI
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("displayOrder")]
        public int DisplayOrder { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("socialPlatformId")]
        public int SocialPlatformId { get; set; }
    }
}
