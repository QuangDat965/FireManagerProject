using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace FireManagerServer.Services.DeviceServices
{
    public class DeviceService : IDeviceService
    {
        private readonly FireDbContext dbContext;

        public DeviceService(
            FireDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<DeviceEntity>> GetAll()
        {
            return await dbContext.Devices.ToListAsync();
        }

        public async Task<List<DeviceEntity>> GetByModuleId(string moduleId)
        {
            return await dbContext.Devices.Where(p => p.ModuleId == moduleId).ToListAsync();
        }
    }
}
