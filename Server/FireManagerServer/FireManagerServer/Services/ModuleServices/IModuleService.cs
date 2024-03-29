using FireManagerServer.Database.Entity;

namespace FireManagerServer.Services.ModuleServices
{
    public interface IModuleService
    {
        Task<List<Module>> GetAll();
        Task<List<Module>> GetbyUserId(string userId);
        Task<List<Module>> GetbyUnitId(string unitId);
        Task<bool> AddToRoom(string unitId, string moduleId);
        Task<bool> Update(Module request);
    }
}
