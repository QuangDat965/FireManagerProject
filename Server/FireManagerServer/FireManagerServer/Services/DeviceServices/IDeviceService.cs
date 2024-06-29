using FireManagerServer.Database.Entity;

namespace FireManagerServer.Services.DeviceServices
{
    public interface IDeviceService
    {
        Task<List<DeviceEntity>> GetAll();
        Task<List<DeviceEntity>> GetByModuleId(string moduleId);
        Task<bool> OnDevice(string deviceId,string userid, bool? timeout=true);
        Task<bool> OffDevice(string deviceId, string userid, bool? timeout=true);

    }
}
