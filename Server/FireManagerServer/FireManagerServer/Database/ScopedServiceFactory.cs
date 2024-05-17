using Microsoft.Extensions.DependencyInjection;

namespace FireManagerServer.Database
{
    public class ScopedServiceFactory<T>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ScopedServiceFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public T CreateScopedService()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                return scope.ServiceProvider.GetRequiredService<T>();
            }
        }
    }

}
