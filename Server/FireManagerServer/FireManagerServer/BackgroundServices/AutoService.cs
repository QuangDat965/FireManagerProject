﻿using FireManagerServer.Database;
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
using Microsoft.AspNetCore.Rewrite;

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
                var scope = _scopeFactory.CreateScope();
                var _deviceService = scope.ServiceProvider.GetRequiredService<IDeviceService>();
                var _moduleService = scope.ServiceProvider.GetRequiredService<IModuleService>();
                var _apartmentService = scope.ServiceProvider.GetRequiredService<IApartmentService>();
                var _ruleService = scope.ServiceProvider.GetRequiredService<IRuleService>();
                var module = await _moduleService.GetbyId(rule.ModuleId);
                var apartmentId = module.ApartmentId;
                var deviceSensors = message.Payload.ToSensorModel();
                var sensorDbs = rule.TopicThreshholds.Where(x => x.DeviceType == Common.DeviceType.R).ToList();
                Console.WriteLine("sensor:" + sensorDbs.Count);
                var sensorDeviceCheckmapping = deviceSensors.ToDictionary(x => x.Id, x => x.Value);
                var implDeviceCheckmapping = deviceSensors.ToDictionary(x => x.Name, x => x.Name);
                #region And Rule
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
                    //no fire
                    if (results.Contains(false))
                    {
                        await _apartmentService.SetIsFireOrNot(apartmentId, false);
                        #region check neigbour                     
                        var neighbours = await _apartmentService.GetNeighBour(apartmentId);
                        var fireNeighbourExists = neighbours.Where(x => x.IsFire == true).ToList();
                        if(fireNeighbourExists?.Count>0)
                        {
                            await NotifyFire(rule, true);
                        }
                        else
                        {

                            await NotifyFire(rule, false);
                        }
                        
                        #endregion
                    }
                    //fire
                    else
                    {
                        await _apartmentService.SetIsFireOrNot(apartmentId, true);
                        await NotifyFire(rule, true);
                    }                            
                }
                #endregion
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
                        await _apartmentService.SetIsFireOrNot(apartmentId, true);
                        await NotifyFire(rule, true);
                    }
                    else
                    {
                        await _apartmentService.SetIsFireOrNot(apartmentId, false);
                        #region check neigbour                     
                        var neighbours = await _apartmentService.GetNeighBour(apartmentId);
                        var fireNeighbourExists = neighbours.Where(x => x.IsFire == true).ToList();
                        if (fireNeighbourExists?.Count > 0)
                        {
                            await NotifyFire(rule, true);
                        }
                        else
                        {
                            
                            await NotifyFire(rule, false);
                        }

                        #endregion
                    }
                }
                _logger.WillLog($"Finish ruleId: {rule.Id}");

            }

        }

        private async Task NotifyFire(RuleDisplayDto rule, bool isFire)
        {
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

                    if(isFire)
                    {
                        if (deviceImplement.ThreshHold == 0)
                        {
                            await _deviceService.OffDevice(deviceImplement.DeviceId, "System", false);
                        }
                        else if (deviceImplement.ThreshHold == 1)
                        {
                            await _deviceService.OnDevice(deviceImplement.DeviceId, "System", false);

                        }
                    }
                    else
                    {
                        if(deviceImplement.InitialValue =="0")
                        {
                            await _deviceService.OffDevice(deviceImplement.DeviceId, "System", false);
                        }
                        else
                        {
                            await _deviceService.OnDevice(deviceImplement.DeviceId, "System", false);
                        }
                    }
                    _logger.WillLog($"Fnish On/Off: {deviceImplement.DeviceId}");

                }
            }
        }
    }
}
