using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;

namespace FireManagerServer.Services.ApartmentService
{
    public interface IApartmentService
    {
        Task<bool> Add(ApartmentRequest request);
        Task<List<Building>> Get(string userId, ApartmentFilter filter);
        Task<List<Building>> GetAll();
        Task<bool> Update(ApartmentUpdateDto request);
        Task<bool> Delete(string id);
    }
}
