using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;
using FireManagerServer.Services.ModuleServices;
using Microsoft.AspNetCore.Mvc;

namespace FireManagerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModuleController:ControllerBase
    {
        private readonly IModuleService moduleService;

        public ModuleController(IModuleService moduleService)
        {
            this.moduleService = moduleService;
        }
        [HttpPost,Route("getall")]
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
        [HttpPost, Route("getbyuser")]
        public async Task<List<Module>> GetbyUserId()
        {

            return await moduleService.GetAll();
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
    }
}
