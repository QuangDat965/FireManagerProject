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
using FireManagerServer.Services.UnitServices;
using System.Reflection;

namespace FireManagerServer.BackgroundServices
{
    public class AutoService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly MqttClient _mqttClient;
        private readonly IServiceScopeFactory _scopeFactory;

        public AutoService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
            _mqttClient = new MqttClient(configuration.GetValue<string>("BrokerHost"));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _mqttClient.MqttMsgPublishReceived += async (sender, e) =>
            {
                await ProcessEventAsync(sender, e);
            };
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
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        private async Task ProcessEventAsync(object sender, MqttMsgPublishEventArgs e)
        {
            await _semaphore.WaitAsync();
            try
            {
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
                if (string.IsNullOrEmpty(sub))
                {
                    var rules = new List<RuleDisplayDto>();
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var ruleService = scope.ServiceProvider.GetRequiredService<IRuleService>();
                        var results = await ruleService.GetByModuleId(moduleId);
                        rules = results.Where(x => x.isActive == true).ToList();
                    }
                    Console.WriteLine("Start HandleRuleRire");
                    await HandleRuleFire(rules, message, moduleId, moduleName, true);
                    Console.WriteLine("End HandleRuleRire");
                }
            }
            catch { }
            finally
            {
                _semaphore.Release();
            }
        }


        private async Task HandleRuleFire(List<RuleDisplayDto> rules, MessageRawModel message, string moduleId, string moduleName, bool notify)
        {
            foreach (var rule in rules)
            {

                var deviceSensors = message.Payload.ToSensorModel();
                var sensorDbs = rule.TopicThreshholds.Where(x => x.DeviceType == Common.DeviceType.R).ToList();
                Console.WriteLine("sensor:" + sensorDbs.Count);
                var sensorDeviceCheckmapping = deviceSensors.ToDictionary(x => x.Name, x => x.Value);
                var implDeviceCheckmapping = deviceSensors.ToDictionary(x => x.Name, x => x.Name);
                if (rule.TypeRule == Common.TypeRule.And)
                {
                    Console.WriteLine("Start Check Rule condition");

                    var results = new List<bool>();
                    foreach (var x in sensorDbs)
                    {

                        if (x.TypeCompare == Common.TypeCompare.Bigger)
                        {
                            string value = sensorDeviceCheckmapping[x.DeviceId];
                            double raw = double.Parse(value);
                            var a = (int)Math.Round(raw);
           
                            bool result = x.ThreshHold < (int)Math.Round(raw);
                            Console.WriteLine("Device:" + a);
                            Console.WriteLine("Db:" + x.ThreshHold);
                            results.Add(result);
                        }
                        else if (x.TypeCompare == Common.TypeCompare.Smaller)
                        {
                            string value = sensorDeviceCheckmapping[x.DeviceId];
                            double raw = double.Parse(value);
                            bool result = x.ThreshHold > (int)Math.Round(raw);
                            results.Add(result);
                        }
                    }
                    Console.WriteLine("Rule Status: "+ results.Contains(false));
                    if (results.Contains(false))
                    {
                        continue;
                    }
                   
                    var deviceImplements = rule.TopicThreshholds.Where(x => x.DeviceType == Common.DeviceType.W).ToList();
                    Console.WriteLine($"Device Type W: {deviceImplements.Count}");

                    foreach (var deviceImplement in deviceImplements)
                    {
                        var systemId = _configuration.GetValue<string>("SystemId");
                        var topic = $"{systemId}/{moduleId}/{moduleName}/sub/{implDeviceCheckmapping[deviceImplement.DeviceId]}";
                        _mqttClient.Publish(topic, Encoding.UTF8.GetBytes(deviceImplement.ThreshHold.ToString()));
                        if (notify)
                        {
                            Console.WriteLine("Start Notify");
                            await NotifyNeightBour(moduleId, message, moduleId, moduleName);
                            Console.WriteLine("End Notify");


                        }
                    }



                }
                else if (rule.TypeRule == Common.TypeRule.Or)
                {
                    var results = new List<bool>();
                    sensorDbs.ForEach(x =>
                    {

                        if (x.TypeCompare == Common.TypeCompare.Bigger)
                        {
                            string value = sensorDeviceCheckmapping[x.DeviceId];
                            double raw = double.Parse(value);
                            bool result = x.ThreshHold < (int)Math.Round(raw);
                            results.Add(result);
                        }
                        else if (x.TypeCompare == Common.TypeCompare.Smaller)
                        {
                            string value = sensorDeviceCheckmapping[x.DeviceId];
                            double raw = double.Parse(value);
                            bool result = x.ThreshHold > (int)Math.Round(raw);
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
                            if (notify)
                            {
                                await NotifyNeightBour(moduleId, message, moduleId, moduleName);

                            }
                        }
                    }
                }
            }

        }

        private async Task NotifyNeightBour(string moduleId, MessageRawModel message, string moduleIdDevice, string moduleName)
        {
            try
            {
                var scope = _scopeFactory.CreateScope();
                var _moduleService = scope.ServiceProvider.GetService<IModuleService>();
                var _apartmentService = scope.ServiceProvider.GetService<IApartmentService>();
                var _ruleService = scope.ServiceProvider.GetService<IRuleService>();

                var module = await _moduleService.GetbyId(moduleId);
                var neightbours = await _apartmentService.GetNeighBour(module.ApartmentId);
                foreach (var neighbour in neightbours)
                {
                    var modules = await _moduleService.GetbyUnitId(neighbour.Id);
                    if (modules?.Count > 0)
                    {
                        foreach (var moduleNeighb in modules)
                        {
                            if (moduleNeighb != null)
                            {
                                var rules = await _ruleService.GetByModuleId(moduleNeighb.Id);
                                rules = rules.Where(x => x.isFireRule == true).ToList();
                                await HandleRuleFire(rules, message, moduleId, moduleName, false);
                            }
                        }
                    }
                }

            }
            catch { };

        }
    }
}
