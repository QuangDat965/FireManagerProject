using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace FireManagerServer.Services.UnitServices
{
    public class UnitService : IUnitService
    {
        private readonly FireDbContext dbContext;

        public UnitService(FireDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> Add(UnitRequest request)
        {
            var valueAdd = new Unit()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                ApartmentId = request.ApartmentId,
                Desc = request.Desc,
            };
            dbContext.Add(valueAdd);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(string unitId)
        {
            var data = await dbContext.Units.FindAsync(unitId);
            dbContext.Remove(data);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Unit>> GetAll()
        {
            return await dbContext.Units.ToListAsync();
        }

        public async Task<List<Unit>> GetList(string apartmentId, string? search)
        {
            var list = await dbContext.Units.Where(p=>p.ApartmentId ==apartmentId).ToListAsync();
            if(!string.IsNullOrEmpty(search))
            {
                list = list.Where(p => p.Name.Contains(search)).ToList();
            }
            return list;
        }

        public async Task<bool> Update(Unit unit)
        {
            dbContext.Units.Update(unit);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
