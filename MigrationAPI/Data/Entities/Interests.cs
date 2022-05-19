namespace MigrationAPI.Data.Entities
{
    public class Interests
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Key { get; set; }
        public bool Blocked { get; set; }
        public bool Deleted { get; set; }
        public int DisplayOrder { get; set; }
    }
}
