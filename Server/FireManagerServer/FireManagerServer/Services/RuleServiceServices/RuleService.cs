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
                Desc = rule.Desc,
                isActive = rule.isActive,
                isFireRule = rule.isFire,
                TypeRule = rule.TypeRule
            };
            foreach(var e in rule.TopicThreshholds)
            {
                e.RuleId = ruleEntity.Id;
            };
            dbContext.Add(ruleEntity);
            dbContext.AddRange(rule.TopicThreshholds);
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
