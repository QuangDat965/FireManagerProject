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
           
            await dbContext.AddAsync(new Building()
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
            var user = await dbContext.Buildings.FirstOrDefaultAsync(p=>p.Id == id);
             dbContext.Remove(user);
            dbContext.SaveChanges();
            return true;
        }

        public async Task<List<Building>> Get(string userId, ApartmentFilter filter)
        {
            var list = await(from data in dbContext.Buildings
                             where data.UserId== userId
                             select data).ToListAsync();
            if(!string.IsNullOrEmpty(filter.SearchKey))
            {
                list = list.Where(_ => _.Name.Contains(filter.SearchKey)).ToList();
            }
            if(filter.OrderBy == Common.OrderType.ByName)
            {
                list = list.OrderBy(p=>p.Name).ToList();
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

        public async Task<List<Building>> GetAll()
        {
            return await dbContext.Buildings.ToListAsync();
        }

        public async Task<bool> Update(ApartmentUpdateDto request)
        {
            var rs = await dbContext.Buildings.FirstOrDefaultAsync(p=>p.Id == request.Id);
            if(rs==null)
            {
                return false;
            }
            rs.Desc = request.Desc;
            rs.Name = request.Name;
            rs.DateUpdate = DateTime.Now;
            dbContext.Update(rs);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
