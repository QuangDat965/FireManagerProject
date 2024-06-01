using FireManagerServer.Controllers;
using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace FireManagerServer.Services.UnitServices
{
    public class ApartmentService : IApartmentService
    {
        private readonly FireDbContext dbContext;

        public ApartmentService(FireDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> Add(UnitRequest request)
        {
            var valueAdd = new Apartment()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                BuldingId = request.ApartmentId,
                Desc = request.Desc,
            };
            dbContext.Add(valueAdd);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddUpdateNeighBour(NeighBourDto unit)
        {
            var aps = unit.NeighboudIds.Select(e => new ApartmentNeighbour
            {
                ApartmentId = unit.CurrentApartmentId,
                NeighbourId = e
            }).ToList();

            var olds = await dbContext.ApartmentNeighbours
                .Where(p => p.ApartmentId == unit.CurrentApartmentId)
                .ToListAsync();

            var comparer = new ApartmentNeighbourComparer();
            var addNeigh = aps.Except(olds, comparer).ToList();
            var deleteNeigh = olds.Except(aps, comparer).ToList();

            if (addNeigh.Any())
            {
                dbContext.ApartmentNeighbours.AddRange(addNeigh);
                await dbContext.SaveChangesAsync();
            }

            if (deleteNeigh.Any())
            {
                dbContext.ApartmentNeighbours.RemoveRange(deleteNeigh);
                await dbContext.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> Delete(string unitId)
        {
            var data = await dbContext.Apartments.FindAsync(unitId);
            dbContext.Remove(data);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Apartment>> GetAll()
        {
            return await dbContext.Apartments.ToListAsync();
        }

        public async Task<Apartment> GetById(string id)
        {
            return await dbContext.Apartments.FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<List<Apartment>> GetList(UnitFilter filter)
        {
            var list = await dbContext.Apartments.Where(p => p.BuldingId == filter.Id).ToListAsync();
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

        public async Task<List<Apartment>> GetNeighBour(string id)
        {
            var rs = await dbContext.ApartmentNeighbours.Where(p => p.ApartmentId == id).Select(p=>p.NeighbourId).ToListAsync();
            var apm = await dbContext.Apartments.Where(p => rs.Contains(p.Id)).ToListAsync();
            return apm;
        }

        public async Task<bool> Update(UnitUpdateDto unit)
        {
            var rs = await dbContext.Apartments.FirstOrDefaultAsync(p=>p.Id == unit.Id);
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

        public async Task<bool> UpdateNeighBour(NeighBourDto unit)
        {
            var olds = await dbContext.ApartmentNeighbours.Where(p => p.ApartmentId == unit.CurrentApartmentId).ToListAsync();
            if(olds!=null)
            {
                dbContext.RemoveRange(olds);
                dbContext.SaveChanges();
            }
            var apmUpdate = new List<ApartmentNeighbour>();
            foreach(var e in unit.NeighboudIds)
            {
                var dtUpdate = new ApartmentNeighbour()
                {
                    ApartmentId = unit.CurrentApartmentId,
                    NeighbourId = e
                
                };
                apmUpdate.Add(dtUpdate);
            }
            dbContext.AddRange(apmUpdate);
            await dbContext.SaveChangesAsync();
            return true;

        }
    }
    public class ApartmentNeighbourComparer : IEqualityComparer<ApartmentNeighbour>
    {
        // So sánh hai đối tượng ApartmentNeighbour
        public bool Equals(ApartmentNeighbour x, ApartmentNeighbour y)
        {
            // Kiểm tra xem hai đối tượng có cùng ApartmentId và NeighbourId không
            return x.ApartmentId == y.ApartmentId && x.NeighbourId == y.NeighbourId;
        }

        // Lấy mã băm của đối tượng ApartmentNeighbour
        public int GetHashCode(ApartmentNeighbour obj)
        {
            // Trả về mã băm của ApartmentId kết hợp với mã băm của NeighbourId
            return obj.ApartmentId.GetHashCode() ^ obj.NeighbourId.GetHashCode();
        }
    }
}
