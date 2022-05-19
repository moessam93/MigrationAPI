namespace MigrationAPI.Data.Entities
{
    public class SocialPlatformPostTypes
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public int DisplayOrder { get; set; }
        public bool Deleted { get; set; }
        public int SocialPlatformId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
