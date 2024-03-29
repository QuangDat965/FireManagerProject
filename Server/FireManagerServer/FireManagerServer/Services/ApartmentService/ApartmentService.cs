using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace FireManagerServer.Services.ApartmentService
{
    public class ApartmentService : IApartmentService
    {
        private readonly FireDbContext dbContext;

        public ApartmentService(FireDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> Add(ApartmentRequest request)
        {
           
            await dbContext.AddAsync(new Apartment()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Desc = request.Desc,
                UserId = request.UserId,
            });
            await dbContext.SaveChangesAsync();
            return true;
            
        }

        public async Task<bool> Delete(string id)
        {
            var user = await dbContext.Apartments.FirstOrDefaultAsync(p=>p.Id == id);
             dbContext.Remove(user);
            dbContext.SaveChanges();
            return true;
        }

        public async Task<List<Apartment>> Get(string userId)
        {
            var list = await(from data in dbContext.Apartments
                             where data.UserId == userId
                             select data).ToListAsync();
            return list;
        }

        public async Task<List<Apartment>> GetAll()
        {
            return await dbContext.Apartments.ToListAsync();
        }

        public async Task<bool> Update(Apartment request)
        {
            dbContext.Update(request);
            dbContext.SaveChanges();
            return true;
        }
    }
}
