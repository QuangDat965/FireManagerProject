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
            Console.WriteLine("MOdule name: " + moduleId);

            using (var dbcontext = new FireDbContext(GetBuilder()))
            {
                var module =  dbcontext.Modules.FirstOrDefault(p => p.Id == moduleId);
                if (module == null)
                {
                     dbcontext.Add(new Module()
                    {
                        Id = moduleId,
                        ModuleName = moduleId
                    });
                //synsc device 
                var devices = dbcontext
            }
        }
        public void ProcessAutoThresh()
        {
            var arrgs = data.Topic.Split("/");
            var payload = Encoding.UTF8.GetString(data.Message);
            using (var dbcontext = new FireDbContext(GetBuilder()))
            {
               var rules = dbcontext.Rules.Where(p=>p.isActive==true).ToList();
                rules.ForEach(r =>
                {
                    if (r.NameCompare == data.Topic)
                    {
                        if (int.Parse(payload) >= int.Parse(r.Threshold))
                        {
                            SendAutoThresh(r.TopicWrite, r.Status);
                        }
                    }
                });
            }
        }

        private void SendAutoThresh(string topicWrite, string status)
        {
            var client = new MqttClient("broker.emqx.io");
            client.Connect(Guid.NewGuid().ToString());
            client.Publish(topicWrite,Encoding.UTF8.GetBytes(status));
        }
    }
}
