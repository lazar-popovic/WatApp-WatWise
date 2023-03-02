using Microsoft.EntityFrameworkCore;

namespace CRUD_test.Data {
    public class DataContext : DbContext {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Player> Player { get; set; }
    }
}
