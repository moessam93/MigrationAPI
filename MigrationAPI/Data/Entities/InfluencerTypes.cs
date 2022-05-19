namespace MigrationAPI.Data.Entities
{
    public class InfluencerTypes
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public decimal EstimatePrice { get; set; }
        public int DisplayOrder { get; set; }
        public int Followers { get; set; }
        public bool Blocked { get; set; }
        public bool Deleted { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
