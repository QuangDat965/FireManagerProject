
using FireManagerServer.Common;
using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using FireManagerServer.Services.DeviceServices;
using FireManagerServer.Services.ModuleServices;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace FireManagerServer.BackgroundServices
{
    public class ListeningService : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly IDbContextFactory _dbContextFactory;
        MqttClient client;

        public ListeningService(IConfiguration configuration, IDbContextFactory dbContextFactory)
        {

            this.configuration = configuration;
            _dbContextFactory = dbContextFactory;
            client = new MqttClient(configuration.GetValue<string>("BrokerHost"));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            client.MqttMsgPublishReceived += ProcessEventAsync;
            string[] topic = new string[] { Constance.TOPIC_ASYNC + "/#" };
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (!client.IsConnected)
                    {
                        client.Connect(Guid.NewGuid().ToString());
                        client.Subscribe(topic, new byte[] { 0 });
                        Console.WriteLine("Connected Mqtt");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                await Task.Delay(TimeSpan.FromMilliseconds(1000), stoppingToken);
            }
        }
        private void ProcessEventAsync(object sender, MqttMsgPublishEventArgs e)
        {

            //var processcer = new ProcessData(e, configuration);
            //processcer.TestLog();
            //processcer.SyncModuleAndDevice();

            var message = new MessageRawModel()
            {
                Topic = e.Topic,
                Payload = Encoding.UTF8.GetString(e.Message)
            };

            this.SyncModuleAndDevice(message);


        }
        private void SyncModuleAndDevice(MessageRawModel packet)
        {

            var arrgs = packet.Topic.Split("/");
            var moduleId = arrgs[2];
            var moduleName = arrgs[3];
            string message = packet.Payload;
            var devices = message.ToSensorModel();
            var dbcontext = _dbContextFactory.CreateDbContext();
            //synsc module
            var module = dbcontext.Modules.FirstOrDefault(p => p.Id == moduleId);
            if (module == null)
            {
                dbcontext.Add(new Module()
                {
                    Id = moduleId,
                    ModuleName = moduleName
                });
                dbcontext.SaveChanges();
            }
            //synsc device 
            var deviceEntities = new List<DeviceEntity>();
            if (devices?.Count>0)
            {
                foreach (var device in devices)
                {
                    var deviceDb = dbcontext.Devices.FirstOrDefault(p => p.Id == device.Id);
                    if (deviceDb == null)
                    {
                        deviceEntities.Add(new DeviceEntity()
                        {
                            Id = device.Id,
                            Topic = device.Name,
                            Port = device.Port,
                            Type = device.Type == "R" ? Common.DeviceType.R : Common.DeviceType.W,
                            ModuleId = moduleId,
                            Unit = device.Unit,
                            InitValue = device.Value
                        });
                    }
                }
            }
            if (deviceEntities.Count > 0)
            {
                dbcontext.Devices.AddRange(deviceEntities);
                dbcontext.SaveChanges();
            }
            //syncs History
            //Task.Run(()=>SaveDataDevice(packet));
        }
        private void SaveDataDevice(MessageRawModel packet)
        {
            try
            {
                var _dbcontext = _dbContextFactory.CreateDbContext();
                var sensors = packet.Payload.ToSensorModel();
                foreach (var sensor in sensors)
                {
                    if (sensor.Type == "R")
                    {
                        _dbcontext.HistoryDatas.Add(new HistoryData()
                        {
                            Id = Guid.NewGuid().ToString(),
                            DeviceName = sensor.Name,
                            DateRetrieve = DateTime.Now,
                            Value = sensor.Value,
                            DeviceId = sensor.Id,
                            DeviceType = DeviceType.R
                        });
                         _dbcontext.SaveChanges();
                    }
                }

            }
            catch { }


        }
    }
}
public class MessageRawModel
{
    public string? Topic { get; set; }
    public string? Payload { get; set; }
}

