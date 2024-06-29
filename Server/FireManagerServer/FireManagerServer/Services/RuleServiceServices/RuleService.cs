using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;
using FireManagerServer.Model.RuleModel;
using FireManagerServer.Services.DeviceServices;
using Microsoft.EntityFrameworkCore;

namespace FireManagerServer.Services.RuleServiceServices
{
    public class RuleService : IRuleService
    {
        private readonly FireDbContext dbContext;
        private readonly IDeviceService _deviceService;

        public RuleService(FireDbContext dbContext,IDeviceService deviceService)
        {
            this.dbContext = dbContext;
            _deviceService = deviceService;
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
                var deviceImps = await _deviceService.GetByModuleId(entity.ModuleId);
                foreach (var di in deviceImps.Where(x => x.Type == Common.DeviceType.W).ToList())
                {
                    if (di.InitValue == "0" || di.InitValue == null)
                    {
                        await _deviceService.OffDevice(di.Id, "System");
                    }
                    else
                    {
                        await _deviceService.OnDevice(di.Id, "System");
                    }
                }
                return true;
            }
        }

        public async Task<bool> Delete(string Id)
        {
            var entity = await dbContext.Rules.FirstOrDefaultAsync(x => x.Id == Id);
            dbContext.Rules.Remove(entity);
            await dbContext.SaveChangesAsync();
            var deviceImps = await _deviceService.GetByModuleId(entity.ModuleId);
            foreach (var di in deviceImps.Where(x => x.Type == Common.DeviceType.W).ToList())
            {
                if (di.InitValue == "0")
                {
                    await _deviceService.OnDevice(di.Id, "System");
                }
                else
                {
                    await _deviceService.OffDevice(di.Id, "System");
                }
            }
            return true;
        }

        public async Task<bool> FireActive(string id)
        {
            var entity = await dbContext.RuleEntities.FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
            {
                return false;
            }
            else
            {
                entity.isFireRule = true;
                dbContext.Update(entity);
                await dbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> FireDeActive(string id)
        {
            var entity = await dbContext.RuleEntities.FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null)
            {
                return false;
            }
            else
            {
                entity.isFireRule = false;
                dbContext.Update(entity);
                await dbContext.SaveChangesAsync();
                return true;
            }
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
                var deviceTypeMapings = await dbContext.Devices.Where(p => entity.TopicThreshholds.Select(x => x.DeviceId).Contains(p.Id)).Select(p => new
                {
                    Id = p.Id,
                    Type = p.Type
                }).ToListAsync();
                var dicmaping = deviceTypeMapings.ToDictionary(x => x.Id, x => x.Type);
                var topicthresholds = entity.TopicThreshholds.Select(x => new TopicThreshholdDisplayDto()
                {
                    DeviceId = x.DeviceId,
                    RuleId = x.RuleId,
                    ThreshHold = x.ThreshHold,
                    TypeCompare = x.TypeCompare,
                    Value = x.Value,
                    DeviceType = dicmaping[x.DeviceId]
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
                var devices = await dbContext.Devices.Where(x => entity.TopicThreshholds.Select(y => y.DeviceId).Contains(x.Id)).ToListAsync();
                var deMaping = devices.ToDictionary(x=>x.Id, x=>x);
                var topicthresholds = entity.TopicThreshholds.Select(x => new TopicThreshholdDisplayDto()
                {
                    DeviceId = x.DeviceId,
                    RuleId = x.RuleId,
                    ThreshHold = x.ThreshHold,
                    TypeCompare = x.TypeCompare,
                    Value = x.Value,
                    DeviceType = deMaping[x.DeviceId].Type,
                    Name = deMaping[x.DeviceId].Topic,
                    InitialValue = deMaping[x.DeviceId].InitValue,

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
