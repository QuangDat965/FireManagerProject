using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using uPLibrary.Networking.M2Mqtt;

namespace FireManagerServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MqttController : ControllerBase
    {
        private readonly FireDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public MqttController(FireDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        [HttpPost,Route("send")]
        public async Task<bool> SendToTopic([FromBody] RequestMqtt request)
        {
            var result = false;
            try
            {
                MqttClient client;
                client = new MqttClient("broker.emqx.io");
                client.Connect(Guid.NewGuid().ToString());
                client.Publish(request.Topic,System.Text.Encoding.UTF8.GetBytes(request.Payload));            
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return await Task.FromResult(result);
        }
        [HttpPost, Route("send/device")]
        public async Task<bool> SendDevice([FromBody] RequestMqtt request)
        {
            var result = false;
            try
            {
                MqttClient client;
                client = new MqttClient("broker.emqx.io");
                client.Connect(Guid.NewGuid().ToString());
                var module = _dbContext.Modules.FirstOrDefault(p => p.Id == request.ModuleId);
                var device = new DeviceEntity();
                if(request.DeviceId!=null)
                {
                    device = _dbContext.Devices.FirstOrDefault(p => p.Id == request.DeviceId);
                }
                else if(request.Topic!=null)
                {
                    device = _dbContext.Devices.FirstOrDefault(p => p.Topic == request.Topic);
                }
                var systemId = _configuration.GetValue<string>("SystemId");
                var moduleId = module.Id;
                var moduleName =module.ModuleName;
                var deviceName =device.Topic;
                var topic =$"{systemId}/{moduleId}/{moduleName}/sub/{deviceName}";
                client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(request.Payload));
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return await Task.FromResult(result);
        }

    }

    public class RequestMqtt
    {
        public string? Topic { get; set; }
        public string? Payload { get; set; }
        public string? DeviceId { get; set; }
        public string? ModuleId { get; set; }
    }

}
