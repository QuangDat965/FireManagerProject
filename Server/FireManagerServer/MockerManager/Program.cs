using MockerManager;
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
        client.Subscribe(new string[] { $"{systemid}/#" }, new byte[] { 0 });
        var topicSend = systemid + "/espmockid/espmock";

        var messsage1 = @"{Name:Chuông báo;Value:0;Type:W;Port:D16;Unit:On/Off},{Name:Khí gas;Value:2306.00;Type:R;Port:D35;Unit:A},{Name:Nhiệt độ;Value:31.80;Type:R;Port:D18;Unit:C},{Name:Cửa sổ;Value:0;Type:W;Port:D17;Unit:On/Off},";
        var rs = messsage1.ToSensorModel();

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

                client.Publish("quangthanhdat250501/espId-1723842-1qz2wsxE/ESP32-1/sub/Chuông báo", Encoding.UTF8.GetBytes("0"));
                //client.Publish(tempaturepush, Encoding.UTF8.GetBytes(temppay.ToString()));

                //client.Publish(window, Encoding.UTF8.GetBytes(0.ToString()));
                Console.WriteLine("Send module 1 value");
            }

            if (send == "e")
            {

                //client.Publish(gaspush2, Encoding.UTF8.GetBytes(gaspay2.ToString()));
                //client.Publish(tempaturepush2, Encoding.UTF8.GetBytes(temppay2.ToString()));
                //client.Publish(window2, Encoding.UTF8.GetBytes(0.ToString()));
                Console.WriteLine("Send module 2 value");
            }


        }

        Console.ReadKey();
    }
   
}

