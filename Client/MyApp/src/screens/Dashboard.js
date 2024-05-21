import React, { useState ,useEffect} from 'react'

import { View, Text, StyleSheet } from 'react-native'


const funcitions = [
  {
    id: 1,
    title: "Quản lý tòa nhà",
    icon: "building"

  },
  {
    id: 2,
    title: "Quản lý căn hộ",
    icon: "home"
  },
  {
    id: 3,
    title: "Quản lý thiết bị",
    icon: "microchip"
  },
  {
    id: 4,
    title: "Quản lý tự động",
    icon: "cubes"
  },
]

export default function Dashboard({ navigation }) {
  
  return (
    <View style={{flex:1,backgroundColor:'red'}}><Text>Dashboard</Text></View>
  )
}


const styles = StyleSheet.create({
  profile: {
    width: 390,
    height: 114,
    flexDirection: 'row',
    alignItems: 'center',
    padding: 10
  },
  footer: {
    width: "95%",
    height: 111,
    backgroundColor: '#fff',
    borderRadius: 41,
    borderWidth: 1,
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',


  },
  fItem: {
    width: 120,
    // backgroundColor:'red',
    justifyContent: 'center',
    alignItems: 'center'
  },
  content: {
    padding: 20,
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'center'
  },
  item: {
    margin: 5,
    backgroundColor: '#fff',
    width: 120,
    height: 130,
    justifyContent: 'center',
    alignItems: 'center',
    borderRadius: 30,
    shadowColor: '#000',
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.5,
    shadowRadius: 3.84,
    elevation: 5, // Độ cao của đổ bóng (chỉ áp dụng trên Android)

  }
})