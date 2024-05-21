using FireManagerServer.Database.Entity;

namespace FireManagerServer.Services.ModuleServices
{
    public interface IModuleService
    {
        Task<List<Module>> GetAll();
        Task<List<Module>> GetbyUserId(string userId);
        Task<List<Module>> GetbyUnitId(string unitId);
        Task<Module> GetbyId(string id);
        Task<bool> AddToRoom(string unitId, string moduleId);
        Task<bool> Update(Module request);
        Task<bool> Active(string id);
        Task<bool> DeActive(string id);
        Task<bool> OnFireRule(string id);
        Task<bool> OffFireRule(string id);
    }
}
