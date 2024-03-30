using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;
using FireManagerServer.Services.UnitServices;
using Microsoft.AspNetCore.Mvc;

namespace FireManagerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UnitController:ControllerBase
    {
        private readonly IUnitService unitService;

        public UnitController(IUnitService unitService)
        {
            this.unitService = unitService;
        }
        [HttpPost, Route("getbyapartment")]
        public async Task<List<Unit>> GetByApartment([FromBody] CommonRequest request)
        {
            return await unitService.GetList(request.Id, request.SearchKey);
        }
        [HttpGet, Route("getall")]
        public async Task<List<Unit>> GetAll()
        {
            return await unitService.GetAll();
        }
        [HttpPost, Route("delete")]
        public async Task<bool> Delete([FromBody] CommonRequest request)
        {
            return await unitService.Delete(request.Id);
        }
        [HttpPost, Route("add")]
        public async Task<bool> Add([FromBody] UnitRequest request)
        {
            return await unitService.Add(request);
        }
        [HttpPost, Route("update")]
        public async Task<bool> Update([FromBody] Unit request)
        {
            return await unitService.Update(request);
        }
    }
}
