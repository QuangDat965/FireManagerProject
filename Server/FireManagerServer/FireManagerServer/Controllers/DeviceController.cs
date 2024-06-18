using FireManagerServer.Common;
using FireManagerServer.Database.Entity;
using FireManagerServer.Model.HistoryModel;
using FireManagerServer.Service.JwtService;
using FireManagerServer.Services.DeviceServices;
using FireManagerServer.Services.HistoryServices;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FireManagerServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        public IDeviceService deviceService { get; }

        private readonly IJwtService _jwtService;
        private readonly IHistoryService _historyService;

        public DeviceController(IDeviceService deviceService, IJwtService jwtService, IHistoryService historyService)
        {
            this.deviceService = deviceService;
            _jwtService = jwtService;
            _historyService = historyService;
        }
        // GET: api/<DeviceController>
        [HttpGet]
        public async Task<List<DeviceEntity>> GetAll()
        {
            return await deviceService.GetAll();
        }
        [HttpGet("history")]
        public async Task<List<HistoryDisplayDto>> GetAllHistory()
        {
            return await _historyService.GetAll();
        }

        // GET api/<DeviceController>/5
        [HttpGet("{moduleId}")]
        public async Task<List<DeviceEntity>> Get(string moduleId)
        {
            return await deviceService.GetByModuleId(moduleId);
        }

        // POST api/<DeviceController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPost("on/{id}")]
        public async Task<bool> On( string id)
        {
            
            return await deviceService.OnDevice(id, _jwtService.GetId(CommomFuncition.GetTokenBear(HttpContext)));
        }

        [HttpPost("off/{id}")]
        public async Task<bool> Off( string id)
        {
            return await deviceService.OffDevice(id, _jwtService.GetId(CommomFuncition.GetTokenBear(HttpContext)));
        }


        // PUT api/<DeviceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DeviceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
