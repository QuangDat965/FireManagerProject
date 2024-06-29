using FireManagerServer.Common;
using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using FireManagerServer.Model.HistoryModel;
using FireManagerServer.Services.HistoryServices;
using Microsoft.EntityFrameworkCore;
using System.Text;
using uPLibrary.Networking.M2Mqtt;

namespace FireManagerServer.Services.DeviceServices
{
    public class DeviceService : IDeviceService
    {
        private readonly FireDbContext dbContext;
        private readonly IConfiguration _configuration;
        private readonly IHistoryService _historyService;
        private readonly JwtService _jwtService;

        public DeviceService(
            FireDbContext dbContext, IConfiguration configuration, IHistoryService historyService)
        {
            this.dbContext = dbContext;
            _configuration = configuration;
            _historyService = historyService;
        }
        public async Task<List<DeviceEntity>> GetAll()
        {
            return await dbContext.Devices.ToListAsync();
        }

        public async Task<List<DeviceEntity>> GetByModuleId(string moduleId)
        {
            return await dbContext.Devices.Where(p => p.ModuleId == moduleId).ToListAsync();
        }

        public async Task<bool> OffDevice(string deviceId, string userid, bool? timeout = true)
        {
            MqttClient client;
            string? responseFromDevice = null;
            client = new MqttClient(_configuration.GetValue<string>("BrokerHost"));
            client.MqttMsgPublishReceived += (sender, e) =>
            {
                responseFromDevice = Encoding.UTF8.GetString(e.Message); // Lưu trữ câu trả lời từ Client B
            };
            client.Connect(Guid.NewGuid().ToString());
            var device = await dbContext.Devices.Where(x => x.Id == deviceId).FirstOrDefaultAsync();
            var htr = new HistoryCreateDto()
            {
                DeviceId = deviceId,
                DeviceName = device.Topic,
                IsSuccess = false,
                DeviceType = device.Type,
                UserId = userid,
                Value = "0"
            };
            var systemId = _configuration.GetValue<string>("SystemId");
            //var deviceId = device.Id;
            client.Subscribe(new string[] { $"{Constance.TOPIC_RESPONSE}/{device.ModuleId}/{deviceId}" }, new byte[] { 0 });
            var topic = $"{Constance.TOPIC_WAIT}/{device.ModuleId}/{deviceId}";
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes("0"));
            if (timeout == null)
            {
                for (int i = 0; i < 15; i++)
                {
                    if (!string.IsNullOrEmpty(responseFromDevice))
                    {
                        htr.IsSuccess = true;
                        await _historyService.Create(htr);
                        return true;
                    }
                    ++i;
                    Thread.Sleep(1000);
                }
            }
            return false;
        }

        public async Task<bool> OnDevice(string deviceId, string userid, bool? timeout = true)
        {
            MqttClient client;
            string? responseFromDevice = null;
            client = new MqttClient(_configuration.GetValue<string>("BrokerHost"));
            client.MqttMsgPublishReceived += (sender, e) =>
            {
                responseFromDevice = Encoding.UTF8.GetString(e.Message); // Lưu trữ câu trả lời từ Client B
            };
            client.Connect(Guid.NewGuid().ToString());
            var device = await dbContext.Devices.Where(x => x.Id == deviceId).FirstOrDefaultAsync();
            var htr = new HistoryCreateDto()
            {
                DeviceId = deviceId,
                DeviceName = device.Topic,
                IsSuccess = false,
                DeviceType = device.Type,
                UserId = userid,
                Value = "1"
            };
            var systemId = _configuration.GetValue<string>("SystemId");
            //var deviceId = device.Id;
            client.Subscribe(new string[] { $"{Constance.TOPIC_RESPONSE}/{device.ModuleId}/{deviceId}" }, new byte[] { 0 });
            var topic = $"{Constance.TOPIC_WAIT}/{device.ModuleId}/{deviceId}";
            client.Publish(topic, System.Text.Encoding.UTF8.GetBytes("1"));
            if (timeout==true)
            {
                for (int i = 0; i < 15; i++)
                {
                    if (!string.IsNullOrEmpty(responseFromDevice))
                    {
                        htr.IsSuccess = true;
                        await _historyService.Create(htr);
                        return true;
                    }
                    ++i;
                    Thread.Sleep(1000);
                }
            }
            return false;
        }

    }
}
