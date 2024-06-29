// import React, { useState, useEffect } from 'react';
// import { View, Text, TouchableOpacity, StyleSheet, ScrollView } from 'react-native';
// import { Table, Row, Rows } from 'react-native-table-component';
// import { getData } from '../api/Api';

// const HistoryScreen = () => {
//   const [activeTab, setActiveTab] = useState('SensorHistory');
//     const [sensor, setSensor] = useState([]);
//     const [control, setControl] = useState([]);
//     useEffect(() => {
    
//         const fetchHistory = async () => {
//             const rs = await getData('Device/history');
//             console.log(rs);
//             setSensor(rs.filter(x=>x.deviceType ==0))
//             setControl(rs.filter(x=>x.deviceType ==1))
//         }
//         fetchHistory();

//     }, [])
//   const sensorData = [
//     {
//       DeviceName: 'Sensor 1',
//       Value: 'Value 1',
//       IsSuccess: true,
//       DateRetrieve: new Date(),
//       DeviceId: 'DeviceId1',
//       UserId: 'UserId1',
//       DeviceType: 0
//     },
//     {
//       DeviceName: 'Sensor 2',
//       Value: 'Value 2',
//       IsSuccess: false,
//       DateRetrieve: new Date(),
//       DeviceId: 'DeviceId2',
//       UserId: 'UserId2',
//       DeviceType: 1
//     },
//   ];

//   const controlData = [
//     {
//       DeviceName: 'Control 1',
//       Value: 'Value 1',
//       IsSuccess: true,
//       DateRetrieve: new Date(),
//       DeviceId: 'DeviceId1',
//       UserId: 'UserId1',
//       DeviceType: 0
//     },
//     {
//       DeviceName: 'Control 2',
//       Value: 'Value 2',
//       IsSuccess: false,
//       DateRetrieve: new Date(),
//       DeviceId: 'DeviceId2',
//       UserId: 'UserId2',
//       DeviceType: 1
//     },
//   ];

//   const TableScreen = ({ data, title }) => {
//     const tableHead = ['Device Name', 'Value', 'Is Success', 'Date Retrieve', 'Device Id', 'User Id', 'Device Type'];

//     const formatDate = (date) => {
//       return new Date(date).toLocaleString();
//     };

//     const formatIsSuccess = (isSuccess) => {
//       if (isSuccess === null) return '';
//       return isSuccess ? 'Yes' : 'No';
//     };

//     const formatDeviceType = (deviceType) => {
//       switch (deviceType) {
//         case 0:
//           return 'Type A';
//         case 1:
//           return 'Type B';
//         default:
//           return 'Unknown';
//       }
//     };

//     const tableData = data.map((item) => [
//       item.DeviceName,
//       item.Value,
//       formatIsSuccess(item.IsSuccess),
//       formatDate(item.DateRetrieve),
//       item.DeviceId,
//       item.UserId ?? '',
//       formatDeviceType(item.DeviceType)
//     ]);

//     return (
//       <View style={styles.contentContainer}>
//         <View style={styles.header}>
//           <Text style={styles.headerText}>{title}</Text>
//         </View>
//         <ScrollView horizontal={true}>
//           <View>
//             <Table borderStyle={{ borderWidth: 1, borderColor: '#C1C0B9' }}>
//               <Row data={tableHead} style={styles.head} textStyle={styles.text} />
//               <Rows data={tableData} textStyle={styles.text} />
//             </Table>
//           </View>
//         </ScrollView>
//       </View>
//     );
//   };

//   return (
//     <View style={styles.mainContainer}>
     
//       {activeTab === 'SensorHistory' ? (
//         <TableScreen data={sensor} title="Lịch sử cảm biến" />
//       ) : (
//         <TableScreen data={control} title="Lịch sử điều khiển" />
//       )}
//        <View style={styles.tabContainer}>
//         <TouchableOpacity
//           style={[styles.tab, activeTab === 'SensorHistory' && styles.activeTab]}
//           onPress={() => setActiveTab('SensorHistory')}
//         >
//           <Text style={styles.tabText}>Lịch sử cảm biến</Text>
//         </TouchableOpacity>
//         <TouchableOpacity
//           style={[styles.tab, activeTab === 'ControlHistory' && styles.activeTab]}
//           onPress={() => setActiveTab('ControlHistory')}
//         >
//           <Text style={styles.tabText}>Lịch sử điều khiển</Text>
//         </TouchableOpacity>
//       </View>
//     </View>
//   );
// };

// const styles = StyleSheet.create({
//   mainContainer: {
//     flex: 1,
//     backgroundColor: '#fff',
//   },
//   tabContainer: {
//     flexDirection: 'row',
//     backgroundColor: '#f1f8ff',
//   },
//   tab: {
//     flex: 1,
//     padding: 16,
//     alignItems: 'center',
//     justifyContent: 'center',
//   },
//   activeTab: {
//     borderBottomWidth: 2,
//     borderBottomColor: '#007bff',
//   },
//   tabText: {
//     fontSize: 16,
//     fontWeight: 'bold',
//     color: '#007bff',
//   },
//   contentContainer: {
//     flex: 1,
//     padding: 10,
//   },
//   header: {
//     height: 60,
//     marginTop:20,
//     justifyContent: 'center',
//     alignItems: 'center',
//     backgroundColor: '#f8f9fa',
//     borderBottomWidth: 1,
//     borderBottomColor: '#e0e0e0',
//     marginBottom: 10,
//   },
//   headerText: {
//     fontSize: 20,
//     fontWeight: 'bold',
//   },
//   head: {
//     height: 40,
//     backgroundColor: '#f1f8ff',
//   },
//   text: {
//     margin: 6,
//     textAlign: 'center',
//   },
//   footer: {
//     height: 60,
//     justifyContent: 'center',
//     alignItems: 'center',
//     backgroundColor: '#f8f9fa',
//     borderTopWidth: 1,
//     borderTopColor: '#e0e0e0',
//     marginTop: 10,
//   },
//   footerText: {
//     fontSize: 16,
//     color: '#6c757d',
//   },
// });

// export default HistoryScreen;
