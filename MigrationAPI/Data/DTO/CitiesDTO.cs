namespace MigrationAPI.Data.DTO
{
    public class CitiesDTO
    {
        public int CountryId { get; set; }
        public int DisplayOrder { get; set; }
        public bool Blocked { get; set; }
        public bool Deleted { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
