
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace FireManagerServer.BackgroundServices
{
    public class ListeningService : BackgroundService
    {
        private readonly IConfiguration configuration;
        MqttClient client = new MqttClient("broker.emqx.io");
        public ListeningService(IConfiguration configuration)
        {

            this.configuration = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            client.MqttMsgPublishReceived += async (s, e) =>
            {
                await ProcessEventAsync(s, e);
            };
            string[] topic = new string[] { configuration.GetValue<string>("SystemId") + "/#" };
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

        private async Task ProcessEventAsync(object sender, MqttMsgPublishEventArgs e)
        {

            var processcer = ProcessData.GetInstance(e, configuration);
            await processcer.TestLog();
            await processcer.SyncModule();
        }
    }
}
