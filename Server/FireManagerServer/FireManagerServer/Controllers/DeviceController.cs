using FireManagerServer.Database.Entity;
using FireManagerServer.Services.DeviceServices;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FireManagerServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        public IDeviceService deviceService { get; }

        public DeviceController(IDeviceService deviceService)
        {
            this.deviceService = deviceService;
        }
        // GET: api/<DeviceController>
        [HttpGet]
        public async Task<List<DeviceEntity>> GetAll()
        {
            return await deviceService.GetAll();
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
