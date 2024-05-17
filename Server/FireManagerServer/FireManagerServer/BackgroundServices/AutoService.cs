using FireManagerServer.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using FireManagerServer.Services.ApartmentService;

namespace FireManagerServer.BackgroundServices
{
    public class AutoService : BackgroundService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ScopedServiceFactory<IApartmentService> _apartmentService;

        public AutoService(IDbContextFactory dbContextFactory, ScopedServiceFactory<IApartmentService> apartmentService)
        {
            _dbContextFactory = dbContextFactory;
            _apartmentService = apartmentService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                   //var db = _dbContextFactory.CreateDbContext();
                   // var aaa = db.Users.ToList();
                    //var db2 = _apartmentService.CreateScopedService();
                    //var rs = db2.GetAll();
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                await Task.Delay(TimeSpan.FromMilliseconds(1000), stoppingToken);
            }
        }
    }
}
