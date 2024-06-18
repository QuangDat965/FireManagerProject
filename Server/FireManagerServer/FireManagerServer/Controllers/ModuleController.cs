using FireManagerServer.Common;
using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;
using FireManagerServer.Service.JwtService;
using FireManagerServer.Services.ModuleServices;
using Microsoft.AspNetCore.Mvc;

namespace FireManagerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService moduleService;
        private readonly IJwtService jwtService;

        public ModuleController(IModuleService moduleService, IJwtService jwtService)
        {
            this.moduleService = moduleService;
            this.jwtService = jwtService;
        }
        [HttpPost, Route("active/{id}")]
        public async Task<bool> Active(string id)
        {
            return await moduleService.Active(id);
        }
        [HttpPost, Route("deactive/{id}")]
        public async Task<bool> Deactive(string id)
        {
            return await moduleService.DeActive(id);
        }
        [HttpPost, Route("getall")]
        public async Task<List<Module>> GetAll()
        {
            return await moduleService.GetAll();
        }
        [HttpPost, Route("getbyunit")]
        public async Task<List<Module>> GetByUnit([FromBody] CommonRequest request)
        {
            return await moduleService.GetbyUnitId(request.UnitId);
        }
        [HttpPost, Route("addtoUnit")]
        public async Task<bool> AddToUnit([FromBody] CommonRequest request)
        {
            return await moduleService.AddToRoom(request.UnitId,request.ModuleId);
        }
        [HttpPost, Route("addtouser/{moduleid}")]
        public async Task<bool> AddToUser(string moduleid)
        {
            var token = CommomFuncition.GetTokenBear(HttpContext);
            var userId = jwtService.GetId(token);
            return await moduleService.AddToUser(userId, moduleid);
        }
        [HttpGet, Route("getbyuser")]
        public async Task<List<Module>> GetbyUserId()
        {
            var userid = jwtService.GetId(CommomFuncition.GetTokenBear(HttpContext));
            return await moduleService.GetbyUserId(userid);
        }
        [HttpGet("{id}")]
        public async Task<Module> GetbyId( string id)
        {
            return await moduleService.GetbyId(id);
        }
        [HttpPost, Route("update")]
        public async Task<bool> Update(Module request)
        {
            return await moduleService.Update(request);
        }
        [HttpPost, Route("setnullunit/{id}")]
        public async Task<bool> SetNullUnit(string id)
        {
            return await moduleService.SetNullUnit(id);
        }
    }
}
