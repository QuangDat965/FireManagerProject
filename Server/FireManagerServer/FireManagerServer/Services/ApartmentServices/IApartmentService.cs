using FireManagerServer.Controllers;
using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;

namespace FireManagerServer.Services.UnitServices
{
    public interface IApartmentService
    {
        Task<List<Apartment>> GetList(UnitFilter filter);
        Task<List<Apartment>> GetAll();
        Task<List<Apartment>> GetNeighBour(string id);
        Task<Apartment> GetById(string id);
        Task<bool> Add(UnitRequest request);
        Task<bool> Delete(string unitId);
        Task<bool> Update(UnitUpdateDto unit);
        Task<bool> AddUpdateNeighBour(NeighBourDto unit);
        Task<bool> UpdateNeighBour(NeighBourDto unit);
    }
}
