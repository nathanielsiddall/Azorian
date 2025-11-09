using Microsoft.EntityFrameworkCore;

namespace Azorian.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Schoolhouse> Schoolhouses { get; set; }
        public DbSet<SchoolhouseStaff> SchoolhouseStaff { get; set; }
        public DbSet<InstructorProfile> InstructorProfiles { get; set; }

        public DbSet<MediaAsset> MediaAssets { get; set; }
        public DbSet<SchoolhouseMedia> SchoolhouseMedia { get; set; }
        public DbSet<InstructorMedia> InstructorMedia { get; set; }
        public DbSet<ClassMedia> ClassMedia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
