using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;

namespace FireManagerServer.Services.RuleServiceServices
{
    public interface IRuleService
    {
        Task<List<RuleEntity>> GetAll();
        Task<List<RuleEntity>> GetByModuleId(string moduleId);
        Task<bool> Delete(string Id);
        Task<RuleEntity> Update(RuleEntity ruleEntity);
        Task<RuleEntity> Create(RuleAddDto rule);
    }
}
