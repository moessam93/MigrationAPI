using Microsoft.EntityFrameworkCore;
using MigrationAPI.Data.Entities;

namespace MigrationAPI.Data
{
    public class DataContext:DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<SocialPlatforms> SocialPlatforms { get; set; }
        public DbSet<SocialPlatformPostTypes> SocialPlatformPostTypes { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<InfluencerTypes> InfluencerTypes { get; set; }
        public DbSet<Interests> Interests { get; set; }

    }
}
