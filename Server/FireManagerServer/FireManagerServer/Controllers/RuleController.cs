using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;
using FireManagerServer.Model.RuleModel;
using FireManagerServer.Services.RuleServiceServices;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FireManagerServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        private readonly IRuleService ruleService;

        public RuleController(IRuleService ruleService)
        {
            this.ruleService = ruleService;
        }
        // GET: api/<RuleController>
        [HttpGet("all")]
        public async Task<List<RuleDisplayDto>> GetAll()
        {
            return await ruleService.GetAll();
        }

        // GET api/<RuleController>/5
        [HttpGet("{moduleId}")]
        public async Task<List<RuleDisplayDto>> Get(string moduleId)
        {
            return await ruleService.GetByModuleId(moduleId);
        }

        // POST api/<RuleController>
        [HttpPost]
        public async Task<bool> Post([FromBody] RuleAddDto rule)
        {
            return await ruleService.Create(rule);
        }
        [HttpPost("active/{id}")]
        public async Task<bool> Active(string id)
        {
            return await ruleService.Active(id);
        }
        [HttpPost("deactive/{id}")]
        public async Task<bool> DeActive(string id)
        {
            return await ruleService.DeActive(id);
        }

        // PUT api/<RuleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RuleController>/5
        [HttpPost("{id}")]
        public async Task<bool> Delete(string id)
        {
            return await ruleService.Delete(id);
        }
    }
}
