import React from 'react'
import { StyleSheet } from 'react-native'
import { Button as PaperButton } from 'react-native-paper'
import { theme } from '../core/theme'

export default function ButtonC({ mode, style,colorc , ...props }) {
  return (
    <PaperButton
      style={[
        styles.button,
        mode === 'outlined' && { backgroundColor:theme.colors.surface },
        mode === 'contained' && { backgroundColor:theme.colors.mainColor },
        style,
        
      
      ]}
      labelStyle={mode === 'contained'?styles.text:mode === 'outlined'?styles.text2:styles.text}
      mode={mode}
      {...props}
    />
  )
}

const styles = StyleSheet.create({
  button: {
    width: '100%',
    marginVertical: 10,
    paddingVertical: 2,
  },
  text: {
    fontWeight: 'bold',
    fontSize: 15,
    lineHeight: 26,
  },
  text2: {
    fontWeight: 'bold',
    fontSize: 15,
    lineHeight: 26,
    color:theme.colors.mainColor
  },
})
