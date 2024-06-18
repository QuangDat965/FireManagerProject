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
using FireManagerServer.Common;
using FireManagerServer.Services.DeviceServices;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace FireManagerServer.BackgroundServices
{
    public class AutoService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly MqttClient _mqttClient;
        private readonly ILoggerService<AutoService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public AutoService(IConfiguration configuration, IServiceScopeFactory scopeFactory, ILoggerService<AutoService> logger)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
            _mqttClient = new MqttClient(configuration.GetValue<string>("BrokerHost"));
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _mqttClient.MqttMsgPublishReceived += async (sender, e) =>
            {
                _logger.WillLog($"retrieve topic e: {e.Topic}");
                await ProcessEventAsync(sender, e);
            };
            string[] topic = new string[] { Constance.TOPIC_ASYNC + "/#" };
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
                _logger.WillLog($"Process topic e: {e.Topic}");
                var message = new MessageRawModel()
                {
                    Topic = e.Topic,
                    Payload = Encoding.UTF8.GetString(e.Message)
                };

                var arrgs = message.Topic.Split("/");
                var moduleId = arrgs[2];
                var moduleName = arrgs[3];
                var rules = new List<RuleDisplayDto>();
                using (var scope = _scopeFactory.CreateScope())
                {
                    var ruleService = scope.ServiceProvider.GetRequiredService<IRuleService>();

                    var results = await ruleService.GetByModuleId(moduleId);
                    rules = results.Where(x => x.isActive == true).ToList();
                }
                _logger.WillLog($"get rule: {JsonConvert.SerializeObject(rules)}");

                Console.WriteLine("Start HandleRuleRire");
                _logger.WillLog($"Start handleRuleService: {e.Topic}");
                await HandleRuleFire(rules, message, moduleId, moduleName);
                _logger.WillLog($"Finished handleRuleService: {e.Topic}");
                Console.WriteLine("End HandleRuleRire");

            }
            catch { }
            finally
            {
                _semaphore.Release();
            }
        }


        private async Task HandleRuleFire(List<RuleDisplayDto> rules, MessageRawModel message, string moduleId, string moduleName)
        {
            foreach (var rule in rules)
            {
                _logger.WillLog($"HadleruleId: {rule.Id}");

                var deviceSensors = message.Payload.ToSensorModel();
                var sensorDbs = rule.TopicThreshholds.Where(x => x.DeviceType == Common.DeviceType.R).ToList();
                Console.WriteLine("sensor:" + sensorDbs.Count);
                var sensorDeviceCheckmapping = deviceSensors.ToDictionary(x => x.Id, x => x.Value);
                var implDeviceCheckmapping = deviceSensors.ToDictionary(x => x.Name, x => x.Name);
                if (rule.TypeRule == Common.TypeRule.And)
                {
                    Console.WriteLine("Start Check Rule condition");

                    var results = new List<bool>();
                    foreach (var x in sensorDbs)
                    {
                        _logger.WillLog($"sensorDb:: {JsonConvert.SerializeObject(x)}");

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
                    _logger.WillLog($"Rule status: {results.Contains(false)}");
                    Console.WriteLine("Rule Status: " + results.Contains(false));
                    if (results.Contains(false))
                    {
                        continue;
                    }

                    var deviceImplements = rule.TopicThreshholds.Where(x => x.DeviceType == Common.DeviceType.W).ToList();
                    Console.WriteLine($"Device Type W: {deviceImplements.Count}");

                    foreach (var deviceImplement in deviceImplements)
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var _deviceService = scope.ServiceProvider.GetRequiredService<IDeviceService>();
                            var _moduleService = scope.ServiceProvider.GetRequiredService<IModuleService>();
                            var _apartmentService = scope.ServiceProvider.GetRequiredService<IApartmentService>();
                            var _ruleService = scope.ServiceProvider.GetRequiredService<IRuleService>();
                            _logger.WillLog($"Start On/Off: {deviceImplement.DeviceId}");

                            if (deviceImplement.ThreshHold == 0)
                            {
                                await _deviceService.OffDevice(deviceImplement.DeviceId,"System");

                            }
                            else
                            {
                                await _deviceService.OnDevice(deviceImplement.DeviceId, "System");

                            }
                            _logger.WillLog($"Fnish On/Off: {deviceImplement.DeviceId}");

                            var module = await _moduleService.GetbyId(rule.ModuleId);
                            var apartmentId = module.ApartmentId;
                            var neighbours = await _apartmentService.GetNeighBour(apartmentId);
                            if (neighbours?.Count > 0)
                            {
                                foreach (var neigh in neighbours)
                                {
                                    var nModules = await _moduleService.GetbyUnitId(neigh.Id);
                                    foreach (var nModule in nModules)
                                    {
                                        var nRules = await _ruleService.GetByModuleId(nModule.Id);
                                        nRules = nRules.Where(x => x.isFireRule == true && x.isActive == true).ToList();
                                        foreach (var nRule in nRules)
                                        {
                                            if (nRule.TopicThreshholds?.Count > 0)
                                            {
                                                var nDeviceImplemetns = new List<TopicThreshholdDisplayDto>();
                                                foreach (var deviceI in nRule.TopicThreshholds)
                                                {
                                                    if (deviceI.DeviceType == DeviceType.W)
                                                    {
                                                        nDeviceImplemetns.Add(deviceI);
                                                    }
                                                }
                                                if (nDeviceImplemetns.Count > 0)
                                                {
                                                    foreach (var nDeviceImplenment in nDeviceImplemetns)
                                                    {
                                                        if (nDeviceImplenment.ThreshHold == 0)
                                                        {
                                                            await _deviceService.OffDevice(nDeviceImplenment.DeviceId, "System");
                                                        }
                                                        else
                                                        {
                                                            await _deviceService.OnDevice(nDeviceImplenment.DeviceId, "System");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                         

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
                            using (var scope = _scopeFactory.CreateScope())
                            {
                                var _deviceService = scope.ServiceProvider.GetRequiredService<IDeviceService>();
                                var _moduleService = scope.ServiceProvider.GetRequiredService<IModuleService>();
                                var _apartmentService = scope.ServiceProvider.GetRequiredService<IApartmentService>();
                                var _ruleService = scope.ServiceProvider.GetRequiredService<IRuleService>();
                                if (deviceImplement.ThreshHold == 0)
                                {
                                    await _deviceService.OffDevice(deviceImplement.DeviceId, "System");

                                }
                                else
                                {
                                    await _deviceService.OnDevice(deviceImplement.DeviceId, "System");

                                }
                                var module = await _moduleService.GetbyId(rule.ModuleId);
                                var apartmentId = module.ApartmentId;
                                var neighbours = await _apartmentService.GetNeighBour(apartmentId);
                                if (neighbours?.Count > 0)
                                {
                                    foreach (var neigh in neighbours)
                                    {
                                        var nModules = await _moduleService.GetbyUnitId(neigh.BuldingId);
                                        foreach (var nModule in nModules)
                                        {
                                            var nRules = await _ruleService.GetByModuleId(nModule.Id);
                                            nRules = nRules.Where(x => x.isFireRule == true && x.isActive == true).ToList();
                                            foreach (var nRule in nRules)
                                            {
                                                if (nRule.TopicThreshholds?.Count > 0)
                                                {
                                                    var nDeviceImplemetns = new List<TopicThreshholdDisplayDto>();
                                                    foreach (var deviceI in nRule.TopicThreshholds)
                                                    {
                                                        if (deviceI.DeviceType == DeviceType.W)
                                                        {
                                                            nDeviceImplemetns.Add(deviceI);
                                                        }
                                                    }
                                                    if (nDeviceImplemetns.Count > 0)
                                                    {
                                                        foreach (var nDeviceImplenment in nDeviceImplemetns)
                                                        {
                                                            if (nDeviceImplenment.ThreshHold == 0)
                                                            {
                                                                await _deviceService.OffDevice(nDeviceImplenment.DeviceId, "System");
                                                            }
                                                            else
                                                            {
                                                                await _deviceService.OnDevice(nDeviceImplenment.DeviceId, "System");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                _logger.WillLog($"Finish ruleId: {rule.Id}");

            }

        }
    }
}
