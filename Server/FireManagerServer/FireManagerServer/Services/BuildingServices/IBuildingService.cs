using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;

namespace FireManagerServer.Services.ApartmentService
{
    public interface IBuildingService
    {
        Task<bool> Add(ApartmentRequest request);
        Task<List<Building>> Get(string userId, ApartmentFilter filter);
        Task<List<Building>> GetAll();
        Task<Building> GetById(string apartmentId);

        Task<bool> Update(ApartmentUpdateDto request);
        Task<bool> Delete(string id);
    }
}
