using FireManagerServer.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace FireManagerServer.Database
{
    public class FireDbContext:DbContext
    {
        public FireDbContext(DbContextOptions<FireDbContext> options) : base(options)
        {

        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
    }
}
