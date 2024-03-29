using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace FireManagerServer.BackgroundServices
{
    public class ProcessData
    {
        MqttMsgPublishEventArgs data;
        IConfiguration configuration;
        static ProcessData instance = null;
        private ProcessData(MqttMsgPublishEventArgs e, IConfiguration configuration)
        {
            data = e;
            this.configuration = configuration;
        }
        public static ProcessData GetInstance(MqttMsgPublishEventArgs e, IConfiguration configuration)
        {
            if (instance == null)
            {
                instance = new ProcessData(e, configuration);
            }
            return instance;
        }
        private DbContextOptions<FireDbContext> GetBuilder()
        {
            var option = new DbContextOptionsBuilder<FireDbContext>().UseMySql(configuration["ConnectionStrings:MySqlConnection"], new MySqlServerVersion(new Version(5, 7, 0))).Options;
            return option;
        }
        public Task TestLog()
        {
            Console.WriteLine("Topic: " + data.Topic);
            Console.WriteLine("Payload: " + Encoding.UTF8.GetString(data.Message));
            return Task.CompletedTask;
        }
        public async Task SyncModule()
        {
            var arrgs = data.Topic.Split("/");
            var moduleId = arrgs[1];

            using (var dbcontext = new FireDbContext(GetBuilder()))
            {
                var module = await dbcontext.Modules.FirstOrDefaultAsync(p => p.Id == moduleId);
                if (module == null)
                {
                    await dbcontext.AddAsync(new Module()
                    {
                        Id = moduleId,
                        ModuleName = moduleId
                    });
                    await dbcontext.SaveChangesAsync();
                }
            }
        }

    }
}
