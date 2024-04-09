using System.Text;
using uPLibrary.Networking.M2Mqtt;

internal class Program
{
    private static void Main(string[] args)
    {
        
        MqttClient client = new MqttClient("broker.emqx.io");
        client.MqttMsgPublishReceived += (s, e) =>
        {
            Console.WriteLine("Topic: " + e.Topic);
            Console.WriteLine("Payload: " + Encoding.UTF8.GetString(e.Message));
        };
        client.Connect(Guid.NewGuid().ToString());
        string systemid = "quangthanhdat250501";
        client.Subscribe(new string[] { $"{systemid}/#" },new byte[] {0});

        var gaspush = $"{systemid}/ESP32-1/D2/R/Gas";
        var tempaturepush = $"{systemid}/ESP32-1/D3/R/Tempature";
        var window = $"{systemid}/ESP32-1/D4/W/Window";

        var gaspush2 = $"{systemid}/ESP32-2/D2/R/Gas";
        var tempaturepush2 = $"{systemid}/ESP32-2/D3/R/Tempature";
        var window2 = $"{systemid}/ESP32-2/D4/W/Window";


        while (true)
        {
            var gaspay = new Random().Next(1023, 2000);
            var temppay = new Random().Next(30, 40);
            // Gửi dữ liệu từ module 2
            var gaspay2 = new Random().Next(1023, 2000);
            var temppay2 = new Random().Next(30, 40);
            Console.Write("Gui du lieu: ");
            var send = Console.ReadLine();

            // Gửi dữ liệu từ module 1
            if (send == "q")
            {
               
                client.Publish(gaspush, Encoding.UTF8.GetBytes(gaspay.ToString()));
                client.Publish(tempaturepush, Encoding.UTF8.GetBytes(temppay.ToString()));
                client.Publish(window, Encoding.UTF8.GetBytes(0.ToString()));
                Console.WriteLine("Send module 1 value");
            }

            if (send == "e")
            {

                client.Publish(gaspush2, Encoding.UTF8.GetBytes(gaspay2.ToString()));
                client.Publish(tempaturepush2, Encoding.UTF8.GetBytes(temppay2.ToString()));
                client.Publish(window2, Encoding.UTF8.GetBytes(0.ToString()));
                Console.WriteLine("Send module 2 value");
            }


        }

        Console.ReadKey();
    }
}