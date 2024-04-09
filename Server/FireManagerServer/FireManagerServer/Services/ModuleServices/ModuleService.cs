using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace FireManagerServer.Services.ModuleServices
{
    public class ModuleService : IModuleService
    {
        private readonly FireDbContext dbContext;

        public ModuleService(FireDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddToRoom(string unitId, string moduleId)
        {
            var module = await dbContext.Modules.FirstOrDefaultAsync(m => m.Id == moduleId );
            if (module == null)
            {
                return false;
            }
            module.UnitId = unitId;
            dbContext.Update(module);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Module>> GetAll()
        {
            return await dbContext.Modules.ToListAsync();
        }

        public async Task<List<Module>> GetbyUnitId(string unitId)
        {
            return await dbContext.Modules.Where(p=>p.UnitId==unitId).ToListAsync();
        }

        public async Task<List<Module>> GetbyUserId(string userId)
        {
            return await dbContext.Modules.Where(p=>p.UserId == userId).ToListAsync();
        }

        public async Task<bool> Update(Module request)
        {
            dbContext.Modules.Update(request);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
