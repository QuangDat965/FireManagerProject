using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace FireManagerServer.Services.RuleServiceServices
{
    public class RuleService : IRuleService
    {
        private readonly FireDbContext dbContext;

        public RuleService(FireDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<RuleEntity> Create(RuleAddDto rule)
        {
            var ruleEntity = new RuleEntity()
            {
                Id = Guid.NewGuid().ToString(),
                NameCompare = rule.NameCompare,
                Threshold = rule.Threshold,
                TopicWrite = rule.TopicWrite,
                Desc = rule.Desc,
                isActive = rule.isActive,
                ModuleId = rule.ModuleId,
                Port = "0",
                Status = rule.Status,
            };
            dbContext.Add(ruleEntity);
            await dbContext.SaveChangesAsync();
            return ruleEntity;
        }

        public async Task<bool> Delete(string Id)
        {
            var entity = await dbContext.Rules.FirstOrDefaultAsync(x => x.Id == Id);
            dbContext.Rules.Remove(entity);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<RuleEntity>> GetAll()
        {
            return await dbContext.Rules.ToListAsync();
        }

        public async Task<List<RuleEntity>> GetByModuleId(string moduleId)
        {
            return await dbContext.Rules.Where(p=>p.ModuleId==moduleId).ToListAsync();
        }

        public async Task<RuleEntity> Update(RuleEntity ruleEntity)
        {
            dbContext.Rules.Update(ruleEntity);
            await dbContext.SaveChangesAsync();
            return ruleEntity;
        }
    }
}
