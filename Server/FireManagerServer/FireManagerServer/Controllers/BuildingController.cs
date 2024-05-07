using FireManagerServer.Common;
using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;
using FireManagerServer.Model.Response;
using FireManagerServer.Service.JwtService;
using FireManagerServer.Services.ApartmentService;
using FireManagerServer.Services.AuthenService;
using Microsoft.AspNetCore.Mvc;

namespace FireManagerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuildingController : ControllerBase
    {
        private readonly IApartmentService apartmentService;
        private readonly IJwtService jwtService;

        public BuildingController(IApartmentService apartmentService, IJwtService jwtService)
        {
            this.apartmentService = apartmentService;
            this.jwtService = jwtService;
        }
        [HttpPost("getlist")]
        public async Task<List<Building>> GetList([FromBody] ApartmentFilter filter)
        {
            var rs = CommomFuncition.GetTokenBear(HttpContext);
            return await apartmentService.Get(jwtService.GetId(rs), filter);
        }
        [HttpGet("getall")]
        public async Task<List<Building>> GetAll()
        {
            return await apartmentService.GetAll();
        }
        [HttpPost("add")]
        public async Task<bool> Add([FromBody] ApartmentRequest request)
        {
            var rs = CommomFuncition.GetTokenBear(HttpContext);
            var id = jwtService.GetId(rs);
            request.UserId = id;
            return await apartmentService.Add(request);
        }
        [HttpPost("delete")]
        public async Task<bool> Delete([FromBody] CommonRequest request)
        {
            return await apartmentService.Delete(request.Id);
        }
        [HttpPost("update")]
        public async Task<bool> Update([FromBody] ApartmentUpdateDto request)
        {
            return await apartmentService.Update(request);
        }
    }
   
}
