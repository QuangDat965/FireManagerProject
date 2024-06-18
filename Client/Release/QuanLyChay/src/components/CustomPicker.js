import React, { useState } from 'react';
import { View, Text, TouchableOpacity, StyleSheet, FlatList, Modal } from 'react-native';
import { Button, RadioButton } from 'react-native-paper';

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
      <Text>{or==null?item.label:item.moduleName}</Text>
      <RadioButton
        value={item.id}
        status={selectedItem?.id === item.id ? 'checked' : 'unchecked'}
        onPress={() => handleSelection(item)}
      />
    </TouchableOpacity>
  );

  return (
    <View>
      <Button onPress={() => setModalVisible(true)}>
        {selectedItem ? selectedItem.label : title}
      </Button>
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

export default CustomPicker;
