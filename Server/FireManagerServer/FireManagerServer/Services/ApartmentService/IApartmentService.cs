using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;

namespace FireManagerServer.Services.ApartmentService
{
    public interface IApartmentService
    {
        Task<bool> Add(ApartmentRequest request);
        Task<List<Apartment>> Get(string userId, ApartmentFilter filter);
        Task<List<Apartment>> GetAll();
        Task<bool> Update(ApartmentUpdateDto request);
        Task<bool> Delete(string id);
    }
}
