using Microsoft.EntityFrameworkCore;

namespace FireManagerServer.Database
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DbContextFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public FireDbContext CreateDbContext()
        {
            var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FireDbContext>();
            return dbContext;
        }
    }

    public interface IDbContextFactory
    {
        FireDbContext CreateDbContext();
    }
}
