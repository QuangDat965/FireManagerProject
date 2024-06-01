

#include <Arduino.h>
#include <PubSubClient.h> // Thư viện MQTT
#include <WiFi.h>
#include <DHT.h>
#include <Adafruit_Sensor.h>

#pragma region declare variable
#define MQ2pin 35
#define Dhtpin 18
#define BellPin 16
#define LedPin 17
#define SystemId "quangthanhdat250501"
#define EspId "espId-1723842-1qz2wsxE"
#define EspName "ESP32-1"

struct MyPacket
{
  String name;
  String value;
  String type;
  String unit;
  String port;
};

const char *ssid = "DatBeoDz";              // Tên của mạng WiFi
const char *password = "25312001";          // Mật khẩu WiFi
const char *mqtt_server = "103.195.239.175"; // Địa chỉ IP của MQTT broker   // Địa chỉ IP của MQTT broker

String TOPIC = String(SystemId) + "/" + String(EspId) + "/" + String(EspName);

WiFiClient espClient;
PubSubClient client(espClient);
DHT dht(Dhtpin, DHT11);
#pragma endregion
#pragma region delacre funcition
void PushMes(String &messs, MyPacket packet);
void setup_wifi();
void callback(char *topic, byte *payload, unsigned int length);
void sendValueMQ2(String &message);
void sendStatusBell(String &message);
void sendStatusTempature(String &message);
void sendStatusWindow(String &message);
void handleBell(String value);
void handleWindow(String value);
void reconnect();
#pragma endregion

void setup()
{
  Serial.begin(115200);
  setup_wifi();
  dht.begin();
  client.setServer(mqtt_server, 1883);
  client.setCallback(callback);
  Serial.println("MQ2 warming up!");
  pinMode(BellPin, OUTPUT);
  pinMode(LedPin, OUTPUT);
  delay(5000);
}

void loop()
{
  String message = "";
  if (!client.connected())
  {
    reconnect();
  }
  client.loop();
  sendStatusBell(message);
  sendValueMQ2(message);
  sendStatusTempature(message);
  sendStatusWindow(message);
  Serial.print(TOPIC.c_str());
  Serial.print(message);
  const char *payload = message.c_str();
  client.publish(TOPIC.c_str(), payload);
  delay(5000);
}
void reconnect()
{
  while (!client.connected())
  {
    Serial.print("Attempting MQTT connection...");
    // Tạo tên ngẫu nhiên cho client
    String clientId = "ESP8266Client-";
    clientId += String(random(0xffff), HEX);
    if (client.connect(clientId.c_str()))
    {
      String topic = TOPIC + "/" + "sub/#";
      client.subscribe(topic.c_str());
    }
    else
    {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      delay(5000);
    }
  }
}
void callback(char *topic, byte *payload, unsigned int length)
{
  Serial.print("Message arrived [");
  Serial.print(topic);
  Serial.print("] ");
  char *lastSlash = strrchr(topic, '/');
  // Kiểm tra nếu không tìm thấy dấu '/'
  String payloadRetrieve = "";
  if (lastSlash != NULL)
  {
    // Trích xuất phần tử sau dấu '/'
    char *lastElement = lastSlash + 1;

    // In ra phần tử cuối cùng
    payloadRetrieve = String(lastElement);
    
    
  }
  Serial.println("payloadRetrieve");
  Serial.println(payloadRetrieve);

  String payloadStr = "";
    for (int i = 0; i < length; i++)
    {
      payloadStr += (char)payload[i];
    }
      Serial.println("payloadStr");
  Serial.println(payloadStr);
    if(payloadRetrieve=="Cửa sổ") {
      handleWindow(payloadStr);
    }
    else if(payloadRetrieve=="Chuông báo") {
      handleBell(payloadStr);
    }

  // String payloadStr = "";
  // for (int i = 0; i < length; i++)
  // {
  //   payloadStr += (char)payload[i];
  // }
  // Serial.print("payload: ");
  // Serial.println(payloadStr);
}
void setup_wifi()
{
  delay(10);
  // Kết nối WiFi
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
}
void sendValueMQ2(String &message)
{                                         // Chú ý sử dụng tham chiếu (&) cho client
  float sensorValue = analogRead(MQ2pin); // Đọc giá trị analog từ chân cảm biến, giả sử nó là giá trị float
  Serial.print("Sensor Value: ");
  Serial.println(sensorValue);
  MyPacket packet;
  packet.name = "Khí gas";
  packet.type = "R";
  packet.unit = "A";
  packet.port = "D35";
  packet.value = String(sensorValue);
  PushMes(message, packet);
}
void sendStatusTempature(String &message){
  float temperature = dht.readTemperature();
  MyPacket packet;
  packet.name = "Nhiệt độ";
  packet.type = "R";
  packet.unit = "C";
  packet.port = "D18";
  packet.value = String(temperature);
  PushMes(message, packet);
}
void sendStatusBell(String &message)
{
  int value = digitalRead(BellPin);
  MyPacket packet;
  packet.name = "Chuông báo";
  packet.type = "W";
  packet.unit = "On/Off";
  packet.port = "D16";
  packet.value = String(value);
  PushMes(message, packet);
}
void sendStatusWindow(String &message)
{
  int value = digitalRead(LedPin);
  MyPacket packet;
  packet.name = "Cửa sổ";
  packet.type = "W";
  packet.unit = "On/Off";
  packet.port = "D17";
  packet.value = String(value);
  PushMes(message, packet);
}
void handleBell(String value)
{
  if (value == "1")
  {
    digitalWrite(BellPin, HIGH);
  }
  else
  {
    digitalWrite(BellPin, LOW);
  }
}
void handleWindow(String value)
{
  if (value == "1")
  {
    digitalWrite(LedPin, HIGH);
  }
  else
  {
    digitalWrite(LedPin, LOW);
  }
}
void PushMes(String &messs, MyPacket packet)
{
  messs +=
      "{Name:" + packet.name + ";" + "Value:" + packet.value + ";" + "Type:" + packet.type + ";" + "Port:" + packet.port + ";" + "Unit:" + packet.unit + "},";
}