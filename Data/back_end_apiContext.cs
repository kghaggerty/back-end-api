using Microsoft.EntityFrameworkCore;
using back_end_api;

namespace back_end_api.Data
{
    public class back_end_apiContext : DbContext
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

        }
    }

}