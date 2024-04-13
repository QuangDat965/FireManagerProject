import React from 'react'
import { Image, StyleSheet } from 'react-native'

export default function LogoMain() {
  return <Image source={require('../assets/logo.png')} style={styles.image} />
}

const styles = StyleSheet.create({
  image: {
    width: 60,
    height: 60,
  },
})
