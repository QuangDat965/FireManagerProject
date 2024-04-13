using FireManagerServer.Database.Entity;

namespace FireManagerServer.Services.DeviceServices
{
    public interface IDeviceService
    {
        Task<List<DeviceEntity>> GetAll();
        Task<List<DeviceEntity>> GetByModuleId(string moduleId);

    }
}
