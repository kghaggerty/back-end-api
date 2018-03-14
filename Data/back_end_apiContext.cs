using Microsoft.EntityFrameworkCore;
using back_end_api;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace back_end_api.Data
{
    public class back_end_apiContext : IdentityDbContext<User>
    {
        public back_end_apiContext(DbContextOptions<back_end_apiContext> options)
            : base(options)
        { }

        public DbSet<User> User { get; set; }
        public DbSet<Goals> Goals { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<DailyCheck> DailyCheck { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyCheck>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

            modelBuilder.Entity<Goals>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");
                
                modelBuilder.Entity<Post>()
                .Property(b => b.DateCreated)
                .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

                base.OnModelCreating(modelBuilder);
        }
    }

}