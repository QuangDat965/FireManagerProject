import React, { useState } from 'react';
import { View, Text, Modal, TouchableOpacity, StyleSheet } from 'react-native';

const CustomAlert = ({ visible, onClose ,onOk, title, message}) => {
  return (
    <Modal
      animationType="fade"
      transparent={true}
      visible={visible}
 
    >
      <View style={styles.modalContainer}>
        <View style={styles.alertContainer}>
          <Text style={styles.alertTitle}>{title}</Text>
          <Text style={styles.alertMessage}>{message}</Text>
          <View style={{flexDirection:'row', justifyContent:'space-between'}}>
          <TouchableOpacity style={styles.cancleButton} onPress={onClose}>
            <Text style={styles.okButtonText}>Close</Text>
          </TouchableOpacity>
          <TouchableOpacity style={styles.okButton} onPress={onOk}>
            <Text style={styles.okButtonText}>OK</Text>
          </TouchableOpacity>
          </View>
        </View>
      </View>
    </Modal>
  );
};

const styles = StyleSheet.create({
    container: {
      flex: 1,
      justifyContent: 'center',
      alignItems: 'center',
    },
    button: {
      padding: 10,
      backgroundColor: 'lightblue',
      borderRadius: 5,
    },
    modalContainer: {
      flex: 1,
      justifyContent: 'center',
      alignItems: 'center',
      backgroundColor: 'rgba(0, 0, 0, 0.5)',
    },
    alertContainer: {
      backgroundColor: 'white',
      padding: 20,
      borderRadius: 10,
      elevation: 5,
    },
    alertTitle: {
      fontSize: 18,
      fontWeight: 'bold',
      marginBottom: 10,
    },
    alertMessage: {
      fontSize: 16,
      marginBottom: 20,
    },
    okButton: {
      backgroundColor: 'red',
      paddingVertical: 10,
      paddingHorizontal: 20,
      borderRadius: 5,
      alignSelf: 'flex-end',

    },
    cancleButton: {
        backgroundColor: 'lightblue',
        paddingVertical: 10,
        paddingHorizontal: 20,
        borderRadius: 5,
        alignSelf: 'flex-end',
 
      },
    okButtonText: {
      fontSize: 16,
      fontWeight: 'bold',
    },
  });

export default CustomAlert