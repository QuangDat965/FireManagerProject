import AsyncStorage from '@react-native-async-storage/async-storage';
import init from 'react_native_mqtt';
import UUIDGenerator from 'react-native-uuid';


init({
  size: 10000,
  storageBackend: AsyncStorage,
  defaultExpires: 1000 * 3600 * 24,
  enableCache: true,
  sync : {}
});
const options = {
  host: 'broker.emqx.io',
  port: 8083,
};
class MqttService  {
  client = new  Paho.MQTT.Client(options.host, options.port, UUIDGenerator.v4());
  constructor() {
    
  }
   connect(callBack) {
    this.client.connect({
      onSuccess: callBack,
      useSSL: false,
      timeout: 3,
      onFailure: this.onFailure
    });

  }
  
  

  checkConnect = () => {
    console.log(this.client.isConnected());
  }

  onFailure = (err) => {
    console.log('Connect failed!');
    console.log(err);
  }
  onConnectionLost=(responseObject)=>{
    
    while(responseObject.errorCode !==0){
      console.log('onConnectionLost:' + responseObject.errorMessage);
      
    }
  }
  subscribeTopic = (topic) => {
    this.client.subscribe(topic, { qos: 0 });
  }
  sendMessage = (topic, payload) =>{
    var message = new Paho.MQTT.Message(payload);
    message.destinationName = topic;
    this.client.send(message);
  }
  getCLient(){
    return this.client;
  }
}

export default MqttService;