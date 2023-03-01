using Microsoft.EntityFrameworkCore;
using rad.Models;

namespace rad.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { } 

        public DbSet<Todo> todos => Set<Todo>();
    }
}
