using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;

namespace FireManagerServer.Services.UnitServices
{
    public interface IUnitService
    {
        Task<List<Unit>> GetList(string apartmentId);
        Task<List<Unit>> GetAll();
        Task<bool> Add(UnitRequest request);
        Task<bool> Delete(string unitId);
    }
}
