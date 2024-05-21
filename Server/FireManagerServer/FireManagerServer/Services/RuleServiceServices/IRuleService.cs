using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;
using FireManagerServer.Model.RuleModel;

namespace FireManagerServer.Services.RuleServiceServices
{
    public interface IRuleService
    {
        Task<List<RuleDisplayDto>> GetAll();
        Task<List<RuleDisplayDto>> GetByModuleId(string moduleId);
        Task<bool> Delete(string Id);
        Task<RuleEntity> Update(RuleEntity ruleEntity);
        Task<bool> Create(RuleAddDto rule);
        Task<bool> Active(string id);
        Task<bool> DeActive(string id);
    }
}
