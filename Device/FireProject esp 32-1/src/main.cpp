

#include <Arduino.h>
#include <PubSubClient.h> // Thư viện MQTT
#include <WiFi.h>
#include <DHT.h>
#include <Adafruit_Sensor.h>

#pragma region declare variable
#define MQ2pin 35
#define Dhtpin 18
#define BellPin 16
#define SubBellPin 12
#define LedPin 17
#define SystemId "datqt192755"
#define Synsc "sync"
#define Wait "wait"    
#define Response "response"
#define EspId "esp32id-2"
#define EspName "ESP32-2"
 
struct MyPacket
{
    String id;
    String name;
    String value;
    String type;
    String unit;
    String port;
};

const char *ssid = "DatBeoDz";               // Tên của mạng WiFi
const char *password = "25312001";           // Mật khẩu WiFi
const char *mqtt_server = "103.176.25.7"; // Địa chỉ IP của MQTT broker   // Địa chỉ IP của MQTT broker

String TOPIC_SYNC = String(SystemId) + "/" + String(Synsc) + "/" + String(EspId)+ "/" + String(EspName);
String TOPIC_WAIT = String(SystemId) + "/" + String(Wait) + "/" + String(EspId);
String TOPIC_RESPONSE = String(SystemId) + "/" + String(Response) + "/" + String(EspId);

String idTemp = String(EspId) +"id_Temp";
String idGas = String(EspId) +"id_Gas";
String idBell = String(EspId) +"id_Bell";
String idWin = String(EspId) +"id_Win";

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
    pinMode(SubBellPin, OUTPUT);

    pinMode(Dhtpin, INPUT);
    pinMode(MQ2pin, INPUT);

    digitalWrite(LedPin, LOW);
    digitalWrite(BellPin, LOW);
    digitalWrite(SubBellPin, LOW);
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
    sendStatusTempature(message);
    sendStatusBell(message);
    sendValueMQ2(message);
    sendStatusWindow(message);
    const char *payload = message.c_str();
    client.publish(TOPIC_SYNC.c_str(), payload);
    delay(5000);
}
void reconnect()
{
    while (!client.connected())
    {
        Serial.print("Attempting MQTT connection...");
        // Tạo tên ngẫu nhiên cho client
        String clientId = "ClientDeive-";
        clientId += String(random(0xffff), HEX);
        if (client.connect(clientId.c_str()))
        {
            String topic = TOPIC_WAIT + "/#";
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
    char *result = strrchr(topic, '/');

    if (result != NULL)
    {
        ++result;
        String finalString = String(result);
        String message;
        for (unsigned int i = 0; i < length; i++)
        {
            message += (char)payload[i];
        }
        Serial.println(message);
        Serial.println(finalString);
        if(finalString == idBell) {
            handleBell(message);
        }
        else if(finalString == idWin) {
            handleWindow(message);
        }
    }
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
{                                           // Chú ý sử dụng tham chiếu (&) cho client
    float sensorValue = analogRead(MQ2pin); // Đọc giá trị analog từ chân cảm biến, giả sử nó là giá trị float
    Serial.print("Sensor Value: ");
    Serial.println(sensorValue);
    MyPacket packet;
    packet.id = idGas;
    packet.name = "Khí gas";
    packet.type = "R";
    packet.unit = "A";
    packet.port = "D35";
    packet.value = String(sensorValue);
    PushMes(message, packet);
}
void sendStatusTempature(String &message)
{
    float temperature = dht.readTemperature();
     Serial.print(temperature);
    MyPacket packet;
    packet.id = idTemp;
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
    packet.id = idBell;
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
    packet.id = idWin;
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
        digitalWrite(SubBellPin, HIGH);
        String rs = TOPIC_RESPONSE +"/"+ idBell;
        client.publish(rs.c_str(),"1");
    }
    else
    {
        digitalWrite(BellPin, LOW);
        digitalWrite(SubBellPin, LOW);
        String rs = TOPIC_RESPONSE +"/"+ idBell;
        client.publish(rs.c_str(),"0");
    }
}
void handleWindow(String value)
{
    if (value == "1")
    {
        digitalWrite(LedPin, HIGH);
        String rs = TOPIC_RESPONSE + "/"+idWin;
        Serial.println(rs);
        client.publish(rs.c_str(),"1");
    }
    else
    {
        digitalWrite(LedPin, LOW);
        String rs = TOPIC_RESPONSE + "/"+idWin;
           Serial.println(rs);
        client.publish(rs.c_str(),"0");
    }
}
void PushMes(String &messs, MyPacket packet)
{
    messs +=
        "{Name:" + packet.name + ";" + "Id:" + packet.id + ";"  + "Value:" + packet.value + ";" + "Type:" + packet.type + ";" + "Port:" + packet.port + ";" + "Unit:" + packet.unit + "},";
}
