namespace MigrationAPI.Data.Entities
{
    public class Cities
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int DisplayOrder { get; set; }
        public bool Blocked { get; set; }
        public bool Deleted { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

    }
}
