using System.ComponentModel.DataAnnotations;

namespace MigrationAPI.Data.Entities
{
    public class Countries
    {
        [Key]
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string IsoTwoCode { get; set; }
        public string IsoThreeCode { get; set; }
        public int DisplayOrder { get; set; }
        public bool Blocked { get; set; }
        public bool Deleted { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

    }
}
