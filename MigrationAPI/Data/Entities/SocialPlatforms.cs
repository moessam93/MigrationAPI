using System.ComponentModel.DataAnnotations;

namespace MigrationAPI.Data.Entities
{
    public class SocialPlatforms
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsLinkingEnabled { get; set; }
        public bool Deleted { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
