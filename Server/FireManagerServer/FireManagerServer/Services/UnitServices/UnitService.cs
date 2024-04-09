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

        public async Task<List<Unit>> GetList(UnitFilter filter)
        {
            var list = await dbContext.Units.Where(p => p.ApartmentId == filter.Id).ToListAsync();
            if (!string.IsNullOrEmpty(filter.SearchKey))
            {
                list = list.Where(p => p.Name.Contains(filter.SearchKey)).ToList();
            }
            if (filter.OrderBy == Common.OrderType.ByName)
            {
                list = list.OrderBy(p => p.Name).ToList();
            }
            else if (filter.OrderBy == Common.OrderType.ByDateNear)
            {
                list = list.OrderBy(p => p.DateCreate).ToList();
            }
            else if (filter.OrderBy == Common.OrderType.ByDateFar)
            {
                list = list.OrderByDescending(p => p.DateCreate).ToList();
            }
            return list;
        }

        public async Task<bool> Update(UnitUpdateDto unit)
        {
            var rs = await dbContext.Units.FirstOrDefaultAsync(p=>p.Id == unit.Id);
            if(rs == null)
            {
                return false;
            }
            rs.Name = unit.Name;
            rs.Desc = unit.Desc;
            dbContext.Update(rs);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
