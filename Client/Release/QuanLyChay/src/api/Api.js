import AsyncStorage from "@react-native-async-storage/async-storage";

const API_URL = 'http://192.168.1.248';


const getData = async (t) => {
  try {
    const token = await AsyncStorage.getItem('token');
    const response = await fetch(`${API_URL}/${t}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`
      }
    });
    console.log('response: ',response);
    
    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Error fetching data:', error);
    return null;
  }
};

const getDataNo = async (t) => {
  try {

    const response = await fetch(`${API_URL}/${t}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        
      }
    });
    console.log('response: ',response);
    
    const data = await response.text();
    return data;
  } catch (error) {
    console.error('Error fetching data:', error);
    return null;
  }
};

// Hàm gửi yêu cầu POST
const postData = async (end,postData) => {
  try {
    const token = await AsyncStorage.getItem('token');
    const response = await fetch(`${API_URL}/${end}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`
      },
      body: JSON.stringify(postData)
    });
    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Error posting data:', error);
    throw error;
  }
};
const postDataNobody = async (end) => {
  try {
    const token = await AsyncStorage.getItem('token');
    const response = await fetch(`${API_URL}/${end}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`
      },
    });
    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Error posting data:', error);
    throw error;
  }
};

// Hàm gửi yêu cầu PUT
const putData = async (id, putData) => {
  try {
    const token = await AsyncStorage.getItem('token');
    const response = await fetch(`${API_URL}/data/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`
      },
      body: JSON.stringify(putData)
    });
    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Error updating data:', error);
    return null;
  }
};

// Hàm gửi yêu cầu DELETE
const deleteData = async (end) => {
  try {
    const token = await AsyncStorage.getItem('token');
    const response = await fetch(`${API_URL}/${end}`, {
      method: 'DELETE',
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Error deleting data:', error);
    throw error;
  }
};

export { getData, postData, putData, deleteData, getDataNo, postDataNobody };