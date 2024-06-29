import React from 'react'
import { StyleSheet } from 'react-native'
import { Text } from 'react-native-paper'
import { theme } from '../core/theme'
import { View } from 'react-native'
import Logo from './Logo'
import Icon from 'react-native-vector-icons/FontAwesome';

export default function HeaderTopD({text}) {
  return <View style={styles.header}>
    <View style={[styles.box, { width: '30%',}]}>
      <View style={{width:'60', height:'60', backgroundColor:'#fff', borderRadius:50}}>
      <Logo/>
      </View>
    
    </View>
    <View style={[styles.box, { width: '40%'}]}>
      <Text style={{fontWeight:"700", color:'#fff'}}>{text}</Text>
    </View>
    <View style={[styles.box, { width: '30%'}]}>
    <Icon name="bell-o" size={30} color="#fff" />
    </View>
  </View>
}

const styles = StyleSheet.create({
  header: {
    backgroundColor: theme.colors.mainColor,
    width: "100%",
    height: 117,
    flexDirection: 'row',
    justifyContent: 'space-between', // Canh lề giữa các phần tử
    alignItems:'center'
  },
  box: {
    // backgroundColor:'#ccc',
      height:"100%",
      alignItems:'center',
      justifyContent:'center',
  },
  logo: {
    width:"90",
    height:"84",
    justifyContent:'center',
    alignItems:'center',
    alignSelf:'center',
  }
})
