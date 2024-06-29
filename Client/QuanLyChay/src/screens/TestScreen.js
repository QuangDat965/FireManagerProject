import React, { useEffect } from 'react'
import Background from '../components/Background'
import Logo from '../components/Logo'
import Header from '../components/Header'
import Button from '../components/Button'
import Paragraph from '../components/Paragraph'
import { View, Text } from 'react-native'
import MqttService from '../helpers/mqttService'

export default function TestScreen({ navigation }) {
    const service = new MqttService();
    const succcess = (eee) => {
        console.log(eee);
        console.log('connecd to server');
        
    }
    service.connect(succcess);
  return (
    <Background>
      <View><Text>Test Page</Text></View>
    </Background>
  )
}
