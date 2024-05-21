using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;
using FireManagerServer.Model.RuleModel;
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

        public async Task<bool> Active(string id)
        {
            var entity = await dbContext.RuleEntities.FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
            {
                return false;
            }
            else
            {
                entity.isActive = true;
                dbContext.Update(entity);
                await dbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> Create(RuleAddDto rule)
        {

            var ruleEntity = new RuleEntity()
            {
                Id = Guid.NewGuid().ToString(),
                Desc = rule.Desc,
                isActive = rule.isActive,
                isFireRule = rule.isFire,
                TypeRule = rule.TypeRule,
                ModuleId = rule.ModuleId
            };
            var topicThreshholds = new List<TopicThreshhold>();
            foreach (var tth in rule.TopicThreshholds)
            {
                var topicth = new TopicThreshhold();
                topicth.DeviceId = tth.DeviceId;
                topicth.RuleId = ruleEntity.Id;
                topicth.ThreshHold = tth.ThreshHold;
                topicth.TypeCompare = tth.TypeCompare;
                topicThreshholds.Add(topicth);
            }

            await dbContext.RuleEntities.AddAsync(ruleEntity);
            await dbContext.TopicThreshholds.AddRangeAsync(topicThreshholds);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeActive(string id)
        {
            var entity = await dbContext.RuleEntities.FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
            {
                return false;
            }
            else
            {
                entity.isActive = false;
                dbContext.Update(entity);
                await dbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> Delete(string Id)
        {
            var entity = await dbContext.Rules.FirstOrDefaultAsync(x => x.Id == Id);
            dbContext.Rules.Remove(entity);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<RuleDisplayDto>> GetAll()
        {
            var entities = await dbContext.Rules.Include(x => x.TopicThreshholds).ToListAsync();
            var rs = new List<RuleDisplayDto>();

            foreach (var entity in entities)
            {
                var display = new RuleDisplayDto();
                display.Id = entity.Id;
                display.isFireRule = entity.isFireRule;
                display.ModuleId = entity.ModuleId;
                display.isActive = entity.isActive;
                display.Desc = entity.Desc;
                display.TypeRule = entity.TypeRule;
                display.TopicThreshholds = new();
                var topicthresholds = entity.TopicThreshholds.Select(x => new TopicThreshholdDisplayDto()
                {
                    DeviceId = x.DeviceId,
                    RuleId = x.RuleId,
                    ThreshHold = x.ThreshHold,
                    TypeCompare = x.TypeCompare,
                    Value = x.Value
                }).ToList();
                display.TopicThreshholds.AddRange(topicthresholds);
                rs.Add(display);
            }

            return rs;
        }

        public async Task<List<RuleDisplayDto>> GetByModuleId(string moduleId)
        {
            var entities = await dbContext.Rules.Include(x => x.TopicThreshholds).Where(p => p.ModuleId == moduleId).ToListAsync();
            var rs = new List<RuleDisplayDto>();

            foreach (var entity in entities)
            {
                var display = new RuleDisplayDto();
                display.Id = entity.Id;
                display.isFireRule = entity.isFireRule;
                display.ModuleId = entity.ModuleId;
                display.isActive = entity.isActive;
                display.Desc = entity.Desc;
                display.TypeRule = entity.TypeRule;
                display.TopicThreshholds = new();
                var topicthresholds = entity.TopicThreshholds.Select(x => new TopicThreshholdDisplayDto()
                {
                    DeviceId = x.DeviceId,
                    RuleId = x.RuleId,
                    ThreshHold = x.ThreshHold,
                    TypeCompare = x.TypeCompare,
                    Value = x.Value
                }).ToList();
                display.TopicThreshholds.AddRange(topicthresholds);
                rs.Add(display);
            }

            return rs;
        }

        public async Task<RuleEntity> Update(RuleEntity ruleEntity)
        {
            dbContext.Rules.Update(ruleEntity);
            await dbContext.SaveChangesAsync();
            return ruleEntity;
        }
    }
}
