import React, { useState } from 'react';
import { View, Text, TouchableOpacity, StyleSheet, FlatList, Modal } from 'react-native';
import { Checkbox, Button } from 'react-native-paper';

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
      <Text>{item[labelSet]}</Text>
      <Checkbox
        status={selectedItems.includes(item.id) ? 'checked' : 'unchecked'}
        onPress={() => toggleSelection(item.id)}
      />
    </TouchableOpacity>
  );

  return (
    <View>
      <Button onPress={() => setModalVisible(true)}>{title}</Button>
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
            <Button mode="contained" onPress={handleConfirm}>Confirm</Button>
            <Button onPress={() => setModalVisible(false)}>Cancel</Button>
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
