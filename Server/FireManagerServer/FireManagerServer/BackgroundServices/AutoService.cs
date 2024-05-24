using FireManagerServer.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using FireManagerServer.Services.ApartmentService;
using FireManagerServer.Services.ModuleServices;
using FireManagerServer.Services.RuleServiceServices;
using uPLibrary.Networking.M2Mqtt;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using uPLibrary.Networking.M2Mqtt.Messages;
using FireManagerServer.Database.Entity;
using FireManagerServer.Model.RuleModel;
using System.Text;
using System.Net.Sockets;

namespace FireManagerServer.BackgroundServices
{
    public class AutoService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ScopedServiceFactory<IRuleService> _ruleService;
        private readonly MqttClient _mqttClient = new MqttClient("broker.emqx.io");
        private readonly IServiceScopeFactory _scopeFactory;

        public AutoService(ScopedServiceFactory<IRuleService> ruleService,IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _ruleService = ruleService;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _mqttClient.MqttMsgPublishReceived += ProcessEventAsync;
            string[] topic = new string[] { _configuration.GetValue<string>("SystemId") + "/#" };
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                   
                    if (!_mqttClient.IsConnected)
                    {
                        _mqttClient.Connect(Guid.NewGuid().ToString());
                        _mqttClient.Subscribe(topic, new byte[] { 0 });
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

        private async void ProcessEventAsync(object sender, MqttMsgPublishEventArgs e)
        {
            //var db = _dbContextFactory.CreateDbContext();
            // var aaa = db.Users.ToList();
            //var db2 = _apartmentService.CreateScopedService();
            //var rs = db2.GetAll();

            //flow 1. get data from rule service
            // 2. get typeRule, dataChecks , dataImplements,
            // 3. map to model compare
            var message = new MessageRawModel()
            {
                Topic = e.Topic,
                Payload = Encoding.UTF8.GetString(e.Message)
            };
            
            var arrgs = message.Topic.Split("/");
            var moduleId = arrgs[1];
            var moduleName = arrgs[2];
            var sub = "";
            try
            {
                sub = arrgs[3];
            }
            catch { };
            if(string.IsNullOrEmpty(sub))
            {

                var rules = new List<RuleDisplayDto>();
                using (var scope = _scopeFactory.CreateScope())
                {
                    var ruleService = scope.ServiceProvider.GetRequiredService<IRuleService>();
                    var results = await ruleService.GetAll();
                    rules = results.Where(x => x.isActive == true ).ToList();

                }
                

                foreach (var rule in rules)
                {
                    var deviceSensors = message.Payload.ToSensorModel();
                    var sensorDbs = rule.TopicThreshholds.Where(x => x.DeviceType == Common.DeviceType.R).ToList();
                    var sensorDeviceCheckmapping = deviceSensors.ToDictionary(x => x.Name, x => x.Value);
                    var implDeviceCheckmapping = deviceSensors.ToDictionary(x => x.Name, x => x.Name);

                    if (rule.TypeRule == Common.TypeRule.And)
                    {
                        var results = new List<bool>();
                        sensorDbs.ForEach(x =>
                        {

                            if (x.TypeCompare == Common.TypeCompare.Bigger)
                            {
                                bool result = x.ThreshHold > int.Parse(sensorDeviceCheckmapping[x.DeviceId]);
                                results.Add(result);
                            }
                            else if (x.TypeCompare == Common.TypeCompare.Smaller)
                            {
                                bool result = x.ThreshHold < int.Parse(sensorDeviceCheckmapping[x.DeviceId]);
                                results.Add(result);
                            }
                        });
                        if(results.Contains(false))
                        {
                            continue;
                        }
                        var deviceImplements = rule.TopicThreshholds.Where(x => x.DeviceType == Common.DeviceType.W).ToList();
                        foreach(var deviceImplement in deviceImplements)
                        {
                            var systemId = _configuration.GetValue<string>("SystemId");
                            var topic = $"{systemId}/{moduleId}/{moduleName}/sub/{implDeviceCheckmapping[deviceImplement.DeviceId]}";
                            _mqttClient.Publish(topic, Encoding.UTF8.GetBytes(deviceImplement.ThreshHold.ToString()));
                        }

                    }
                    else if(rule.TypeRule == Common.TypeRule.Or)
                    {
                        var results = new List<bool>();
                        sensorDbs.ForEach(x =>
                        {

                            if (x.TypeCompare == Common.TypeCompare.Bigger)
                            {
                                bool result = x.ThreshHold > int.Parse(sensorDeviceCheckmapping[x.DeviceId]);
                                results.Add(result);
                            }
                            else if (x.TypeCompare == Common.TypeCompare.Smaller)
                            {
                                bool result = x.ThreshHold < int.Parse(sensorDeviceCheckmapping[x.DeviceId]);
                                results.Add(result);
                            }
                        });
                        if (results.Contains(true))
                        {
                            var deviceImplements = rule.TopicThreshholds.Where(x => x.DeviceType == Common.DeviceType.W).ToList();
                            foreach (var deviceImplement in deviceImplements)
                            {
                                var systemId = _configuration.GetValue<string>("SystemId");
                                var topic = $"{systemId}/{moduleId}/{moduleName}/sub/{implDeviceCheckmapping[deviceImplement.DeviceId]}";
                                _mqttClient.Publish(topic, Encoding.UTF8.GetBytes(deviceImplement.ThreshHold.ToString()));
                            }
                        }
                    }
                }
            }
            
            
        }
    }
}
