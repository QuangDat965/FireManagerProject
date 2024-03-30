using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;

namespace FireManagerServer.Services.ApartmentService
{
    public interface IApartmentService
    {
        Task<bool> Add(ApartmentRequest request);
        Task<List<Apartment>> Get(string userId, string? searchKey);
        Task<List<Apartment>> GetAll();
        Task<bool> Update(Apartment request);
        Task<bool> Delete(string id);
    }
}
