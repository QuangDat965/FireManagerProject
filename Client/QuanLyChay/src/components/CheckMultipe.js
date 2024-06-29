import React, { useState } from 'react';
import { View,Button, Text, TouchableOpacity, StyleSheet, FlatList, Modal } from 'react-native';
import { Checkbox } from 'react-native-paper';
import { theme } from '../core/theme';
import ButtonC from './Button';

const CheckMultipe = ({ items, onSelectionChange, labelSet , title}) => {
  const [selectedItems, setSelectedItems] = useState([]);
  const [modalVisible, setModalVisible] = useState(false);

  const toggleSelection = (id) => {
    setSelectedItems((prevSelectedItems) => {
      if (prevSelectedItems.includes(id)) {
        return prevSelectedItems.filter(item => item !== id);
      } else {
        return [...prevSelectedItems, id];
      }
    });
  };

  const handleConfirm = () => {
    const selectedObjects = items.filter(item => selectedItems.includes(item.id));
    onSelectionChange(selectedObjects);
    setModalVisible(false);
  };

  const renderItem = ({ item }) => (
    <TouchableOpacity onPress={() => toggleSelection(item.id)} style={styles.item}>
      <Text style={{color:theme.colors.mainColor}}>{item[labelSet]}</Text>
      <Checkbox
        status={selectedItems.includes(item.id) ? 'checked' : 'unchecked'}
        onPress={() => toggleSelection(item.id)}
      />
    </TouchableOpacity>
  );

  return (
    <View>
      <Button color={theme.colors.mainColor} onPress={() => setModalVisible(true)} title={title}/>
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
              extraData={selectedItems}
            />
            <ButtonC mode="contained" onPress={handleConfirm}>Confirm</ButtonC>
            <ButtonC mode = 'outlined' onPress={() => setModalVisible(false)}>Cancel</ButtonC>
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

export default CheckMultipe;
