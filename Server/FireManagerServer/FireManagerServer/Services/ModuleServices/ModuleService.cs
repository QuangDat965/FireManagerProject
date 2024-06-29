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

        public async Task<bool> Active(string id)
        {
            var entity = await dbContext.Modules.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (entity != null)
            {
                entity.Status = true;
                dbContext.Update(entity);

            }
            await dbContext.SaveChangesAsync(); 
            return true;
        }

        public async Task<bool> AddToRoom(string unitId, string moduleId)
        {
           try
            {
                var module = await dbContext.Modules.FirstOrDefaultAsync(m => m.Id == moduleId);
                if (module == null)
                {
                    return false;
                }
                module.ApartmentId = unitId;
                dbContext.Update(module);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception)
            {
                return false;

            }
        }

        public async Task<bool> AddToUser(string userid, string moduleId)
        {
            var module = dbContext.Modules.FirstOrDefault(x=>x.Id == moduleId && x.Status ==true);
            module.UserId = userid;
            dbContext.Update(module);
            return await dbContext.SaveChangesAsync()>0;
        }

        public async Task<bool> DeActive(string id)
        {
            var entity = await dbContext.Modules.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (entity != null)
            {
                entity.Status = false;
                dbContext.Update(entity);

            }
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Module>> GetAll()
        {
            return await dbContext.Modules.ToListAsync();
        }

        public async Task<Module> GetbyId(string id)
        {
            return await dbContext.Modules.FirstOrDefaultAsync(p=>p.Id==id);
        }

        public async Task<List<Module>> GetbyUnitId(string unitId)
        {
            return await dbContext.Modules.Where(p=>p.ApartmentId==unitId).ToListAsync();
        }

        public async Task<List<Module>> GetbyUserId(string userId)
        {
            return await dbContext.Modules.Where(p=>p.UserId == userId && p.Status ==true).ToListAsync();
        }

        public async Task<bool> OffFireRule(string id)
        {
           
            return true;
        }

        public async Task<bool> OnFireRule(string id)
        {
           
            return true;
        }

        public async Task<bool> SetNullUnit(string id)
        {
            var module = await dbContext.Modules.Where(x=>x.Id == id).FirstOrDefaultAsync();
            module.ApartmentId = null;
            dbContext.Update(module);
            return await dbContext.SaveChangesAsync()> 0;
        }

        public async Task<bool> Update(Module request)
        {
            dbContext.Modules.Update(request);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
