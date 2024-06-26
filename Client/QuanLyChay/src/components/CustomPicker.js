import React, { useState } from 'react';
import { View,Button, Text, TouchableOpacity, StyleSheet, FlatList, Modal } from 'react-native';
import {  RadioButton } from 'react-native-paper';
import { theme } from '../core/theme';
import ButtonC from './Button';

const CustomPicker = ({ items, onSelectionChange, title, or }) => {
  const [selectedItem, setSelectedItem] = useState(null);
  const [modalVisible, setModalVisible] = useState(false);

  const handleSelection = (item) => {
    setSelectedItem(item);
    setModalVisible(false);
    if (onSelectionChange) {
      onSelectionChange(item);
    }
  };

  const renderItem = ({ item }) => (
    <TouchableOpacity onPress={() => handleSelection(item)} style={styles.item}>
      <Text style={{color:theme.colors.mainColor}}>{or==null?item.label:item.moduleName}</Text>
      <RadioButton
        value={item.id}
        status={selectedItem?.id === item.id ? 'checked' : 'unchecked'}
        onPress={() => handleSelection(item)}
      />
    </TouchableOpacity>
  );

  return (
    <View>
      <Button color={theme.colors.mainColor} onPress={() => setModalVisible(true)} title= {selectedItem ? selectedItem.label : title}/>
      
      <Modal
        animationType="slide"
        transparent={true}
        visible={modalVisible}
        onRequestClose={() => setModalVisible(false)}
      >
        <View style={styles.modalContainer}>
          <View style={styles.modalContent}>
            <FlatList
              data={items}
              renderItem={renderItem}
              keyExtractor={item => item.id}
              extraData={selectedItem}
            />
            <ButtonC mode='contained' onPress={() => setModalVisible(false)}>Cancel</ButtonC>
          </View>
        </View>
      </Modal>
    </View>
  );
};

const styles = StyleSheet.create({
  item: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: 12,
    borderBottomWidth: 1,
    borderColor: '#ccc',
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'center',
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
  },
  modalContent: {
    backgroundColor: 'white',
    margin: 20,
    padding: 20,
    borderRadius: 10,
    elevation: 10,
  },
});

export default CustomPicker;
