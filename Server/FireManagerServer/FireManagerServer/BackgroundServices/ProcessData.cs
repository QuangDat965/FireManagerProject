using FireManagerServer.Common;
using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace FireManagerServer.BackgroundServices
{
    public class ProcessData
    {
        MqttMsgPublishEventArgs data;
        IConfiguration configuration;
        public ProcessData(MqttMsgPublishEventArgs e, IConfiguration configuration)
        {
            data = e;
            this.configuration = configuration;
        }
        private DbContextOptions<FireDbContext> GetBuilder()
        {
            var option = new DbContextOptionsBuilder<FireDbContext>().UseMySql(configuration["ConnectionStrings:MySqlConnection"], new MySqlServerVersion(new Version(5, 7, 0))).Options;
            return option;
        }
        public void TestLog()
        {
            Console.WriteLine("Topic: " + data.Topic);
            Console.WriteLine("Payload: " + Encoding.UTF8.GetString(data.Message));

        }
        public void SyncModuleAndDevice()
        {
            var arrgs = data.Topic.Split("/");
            var moduleId = arrgs[1];
            var moduleName = arrgs[2];
            string message = System.Text.Encoding.UTF8.GetString(data.Message);
            var devices = new List<SensorModel>();

            using (var dbcontext = new FireDbContext(GetBuilder()))
            {
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
                string sub = "";
                try
                {
                    sub = arrgs[3];               
                }
                catch (Exception)
                {
                    
                }
                try
                {
                    devices = message.ToSensorModel();
                }
                catch (Exception) { };
                var deviceEntities = new List<DeviceEntity>();
                if(devices?.Count>0 && string.IsNullOrEmpty(sub))
                {
                    foreach (var device in devices)
                    {
                        var deviceDb = dbcontext.Devices.FirstOrDefault(p => p.Topic == device.Name);
                        if (deviceDb == null && string.IsNullOrEmpty(sub))
                        {
                            deviceEntities.Add(new DeviceEntity()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Topic = device.Name,
                                Port = device.Port,
                                Type = device.Type == "R" ? Common.DeviceType.R : Common.DeviceType.W,
                                ModuleId = moduleId,
                                Unit = device.Unit,
                            });
                        }
                    }
                }          
                if(deviceEntities.Count>0)
                {
                    dbcontext.Devices.AddRange(deviceEntities);
                    dbcontext.SaveChanges();
                }
            }
        }
        public void ProcessAutoThresh()
        {
          
        }

        private void SendAutoThresh(string topicWrite, string status)
        {
            var client = new MqttClient("broker.emqx.io");
            client.Connect(Guid.NewGuid().ToString());
            client.Publish(topicWrite+"/Sub", Encoding.UTF8.GetBytes(status));
        }
    }
}

public static class CustomMapper
{
    public static List<SensorModel> ToSensorModel(this string messsages)
    {
        var rs = new List<SensorModel>();
        var arrMessages = messsages.Split(",");

        foreach (var messsage in arrMessages)
        {
            var model = new SensorModel();
            if (!string.IsNullOrEmpty(messsage))
            {
                var keyvalues = messsage.Trim('{', '}').Split(";");
                var list = new List<SensorModel>();
                foreach (var keyvalue in keyvalues)
                {
                    var key = keyvalue.Split(':')[0];
                    var value = keyvalue.Split(':')[1];
                    if (key == "Name")
                    {
                        model.Name = value;
                    }
                    if (key == "Value")
                    {
                        model.Value = value;
                    }
                    if (key == "Port")
                    {
                        model.Port = value;
                    }
                    if (key == "Type")
                    {
                        model.Type = value;
                    }
                    if (key == "Unit")
                    {
                        model.Unit = value;
                    }
                }
                rs.Add(model);
            }
        }
        return rs;
    }
}
public class SensorModel
{
    public string Name { get; set; }
    public string Value { get; set; }
    public string Type { get; set; }
    public string Port { get; set; }
    public string Unit { get; set; }
}
