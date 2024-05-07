using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;
using FireManagerServer.Services.UnitServices;
using Microsoft.AspNetCore.Mvc;

namespace FireManagerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApartmentController:ControllerBase
    {
        private readonly IUnitService unitService;

        public ApartmentController(IUnitService unitService)
        {
            this.unitService = unitService;
        }
        [HttpPost, Route("getbyapartment")]
        public async Task<List<Apartment>> GetByApartment([FromBody] UnitFilter filter)
        {
            return await unitService.GetList(filter);
        }
        [HttpGet, Route("getall")]
        public async Task<List<Apartment>> GetAll()
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
        public async Task<bool> Update([FromBody] UnitUpdateDto request)
        {
            return await unitService.Update(request);
        }
        [HttpPost, Route("neighbour")]
        public async Task<bool> AddNeighBour([FromBody] NeighBourDto request)
        {
            return await unitService.AddUpdateNeighBour(request);
        }
        [HttpPut, Route("neighbour")]
        public async Task<bool> UpdateNeighBour([FromBody] NeighBourDto request)
        {
            return await unitService.UpdateNeighBour(request);
        }
        [HttpGet("neighbour/{id}")]
        public async Task<List<Apartment>> UpdateNeighBour(string id)
        {
            return await unitService.GetNeighBour(id);
        }

    }
    public class NeighBourDto
    {
        public string CurrentApartmentId { get; set; }
        public List<string> NeighboudIds { get; set; }
    }
}
