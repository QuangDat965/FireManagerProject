import React, { useEffect, useState } from 'react'
import BackgroundTop from '../components/BackgroundTop'
import { View, StyleSheet, Text, TouchableOpacity, ScrollView, Button } from 'react-native';
import { theme } from '../core/theme'
import Icon from 'react-native-vector-icons/FontAwesome';
import Icon2 from 'react-native-vector-icons/FontAwesome6'
import { getData, getDataNo, postData, postDataNobody } from '../api/Api';
import MqttService from '../helpers/mqttService';
import { useNavigate, useLocation } from 'react-router-native';
import Loading from '../components/Loading';




export default function UnitDetailScreen() {
    const navigate = useNavigate();
    const location = useLocation();
    const state = location.state;
    const { unit } = state;
    const [dataModule, setDataModule] = useState([])
    const [screenControl, setScreenControl] = useState(0)
    const [devices, setDevice] = useState([])
    const [isloading, setIsloading] = useState(false)
    useEffect(() => {

        const initial = async () => {
            const modules = await postData('Module/getbyunit', {
                "unitId": unit.id
            });
            const systemId = await getDataNo('System/id');
            modules.forEach(async e => {
                const deviceModules = await getData(`Device/${e.id}`);
                if (deviceModules != null && deviceModules.length > 0) {
                    deviceModules.forEach(element => {
                        devices.push(element)
                        setDevice([...devices])
                    });
                }
                MqttProcess(e.id, systemId, e.moduleName)
            });
        }


        initial()
    }, [])
    const MqttProcess = (moduleId, s, name) => {
        const service = new MqttService();
        const succcess = async (eee) => {
            const topic = `${s}/sync/${moduleId}/${name}`
            service.subscribeTopic(`${topic}/#`);
        }
        service.connect(succcess);
        const client = service.getCLient();
        client.onMessageArrived = (message) => {

            // console.log("Message received on topic: " + message.destinationName);
            // console.log("Message content: " + message.payloadString);
            // const topic = message.destinationName;
            const payload = message.payloadString;
           
            const listDevice = GetdataFromPayload(payload);
     
            
            listDevice.forEach(fModule => {
               
                devices.forEach(fDb => {
                    if(fModule.Id == fDb.id) {
                        fDb.payload = fModule.Value
                    }
                })
                
            });
            setDevice([...devices]);
        };
    }
    const GetdataFromPayload= (payload) => {
        const dataString = payload;

        // Sử dụng biểu thức chính quy để tách các đối tượng từ chuỗi
        const regex = /{([^}]+)}/g;
        const matches = [...dataString.matchAll(regex)];
        
        // Tạo mảng chứa các đối tượng từ các kết quả tìm kiếm
        const dataArray = matches.map(match => {
            const objectString = match[1];
            const objectData = objectString.split(';').reduce((obj, pair) => {
                const [key, value] = pair.split(':');
                obj[key.trim()] = isNaN(value) ? value.trim() : parseFloat(value.trim());
                return obj;
            }, {});
            return objectData;
        });
        return dataArray;
    }
    const getSystemId = async () => {
        return await getData('System/id');
    }
    const testFun = async () => {
        const fetchdata = async () => {
            const dt = await getData('Building/getlist');
        }
        fetchdata();
    }
    const handleToggle = async (e) => {
    setIsloading(true);
    console.log(e);
    if(e.payload == null || e.payload == 0)
        {
            var rs = await postData(`Device/on/${e.id}`, {
                "id":e.id
            })
        }
    else {
        var rs = await postDataNobody(`Device/off/${e.id}`)
    }
    setIsloading(false);    
    
    }
    return (
        <BackgroundTop>
            {/* header */}
            <Loading visible = {isloading}/>
            <View style={styles.header}>
                <View style={[styles.box, { width: '30%', }]}>
                    <TouchableOpacity onPress={() => navigate(-1)}>
                        <Icon name="angle-double-left" size={30} color="#fff" />
                    </TouchableOpacity>
                </View>
                <View style={[styles.box, { width: '40%' }]}>
                    <Text style={{ fontWeight: '700', color: '#fff' }}>{unit.name}</Text>
                </View>
                <View style={[styles.box, { width: '30%' }]}>
                    <TouchableOpacity onPress={() => setScreen(1)}>
                        <Icon name="plus" size={30} color="#fff" />
                    </TouchableOpacity>
                </View>
            </View>

            {/* item */}
            <View style={styles.item}>
                <View style={styles.itemleft}>
                    <Icon name="home" size={80} color={theme.colors.mainColor} />
                </View>
                <View style={styles.itemright}>
                    <View style={{ flexDirection: 'row', marginTop:5 }}>
                        <Text style={{ fontWeight: '500' }}>Tên căn hộ: </Text>
                        <Text style={{ fontWeight: '500', opacity: 0.7 }}>{unit.name}</Text>
                    </View>

                    <View style={{ flexDirection: 'row' }}>
                        <Text style={{ fontWeight: '500' }}>Mô tả: </Text>
                        <Text style={{ fontWeight: '500', opacity: 0.7 }}>{unit.desc}</Text>
                    </View>

                    <View style={{ flexDirection: 'row' }}>
                        <Text style={{ fontWeight: '500' }}>Ngày tạo: </Text>
                        <Text style={{ fontWeight: '500', opacity: 0.7 }}>{unit.dateCreate}</Text>
                    </View>
                </View>

            </View>

            <Button color={theme.colors.mainColor} onPress={() => testFun()} title="Thông số thiết bị"></Button>
            <ScrollView style={{ marginBottom: 100 }}>

                <View style={screenControl == 0 ? { padding: 10 } : { display: 'none' }}>
                    {devices.length > 0 ? devices.map((e,i) => {
                        return (e.type === 0 ? <View key={i} style={styles.item2}>
                            <View style={styles.itemleft}>
                                <Icon name="eye" size={60} color={theme.colors.mainColor} />
                            </View>
                            <View style={styles.itemright}>
                                <View style={{ flexDirection: 'row', }}>
                                    <Text style={{ fontWeight: '500' }}>Tên sensor: {e.topic} </Text>
                                    <Text style={{ fontWeight: '500', opacity: 0.7 }}></Text>
                                </View>

                                <View style={{ flexDirection: 'row' }}>
                                    <Text style={{ fontWeight: '500' }}>Giá trị: {e.payload} </Text>
                                    <Text style={{ fontWeight: '500', opacity: 0.7 }}></Text>
                                </View>

                                <View style={{ flexDirection: 'row' }}>
                                    <Text style={{ fontWeight: '500' }}>Module: </Text>
                                    <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.moduleId}</Text>
                                </View>

                                <View style={{ flexDirection: 'row' }}>
                                    <Text style={{ fontWeight: '500' }}>Chân: </Text>
                                    <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.port}</Text>
                                </View>

                                <View style={{ flexDirection: 'row' }}>
                                    <Text style={{ fontWeight: '500' }}>Đơn vị: </Text>
                                    <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.unit}</Text>
                                </View>
                            </View>

                        </View> : <View></View>)
                    }) :
                        <View></View>
                    }
                </View>

                <View style={screenControl == 1 ? { padding: 10 } : { display: 'none' }}>
                    {devices.length > 0 ? devices.map((e,i) => {
                        return (e.type === 1 ? <View key={i} style={styles.item2}>
                            <View style={styles.itemleft}>
                                <TouchableOpacity onPress={() => handleToggle(e)}>
                                    <Icon2 name={e.payload == 1 ? 'toggle-on' : 'toggle-off'} size={60} color={theme.colors.mainColor} />
                                </TouchableOpacity>
                            </View>
                            <View style={styles.itemright}>
                                <View style={{ flexDirection: 'row', }}>
                                    <Text style={{ fontWeight: '500' }}>Chức năng: {e.topic} </Text>
                                    <Text style={{ fontWeight: '500', opacity: 0.7 }}></Text>
                                </View>

                                <View style={{ flexDirection: 'row' }}>
                                    <Text style={{ fontWeight: '500' }}>Trạng thái: {e.payload} </Text>
                                    <Text style={{ fontWeight: '500', opacity: 0.7 }}></Text>
                                </View>

                                <View style={{ flexDirection: 'row' }}>
                                    <Text style={{ fontWeight: '500' }}>Module: </Text>
                                    <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.moduleId}</Text>
                                </View>

                                <View style={{ flexDirection: 'row' }}>
                                    <Text style={{ fontWeight: '500' }}>Chân: </Text>
                                    <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.port}</Text>
                                </View>

                                <View style={{ flexDirection: 'row' }}>
                                    <Text style={{ fontWeight: '500' }}>Đơn vị: </Text>
                                    <Text style={{ fontWeight: '500', opacity: 0.7 }}>On/OFF</Text>
                                </View>
                            </View>

                        </View> : <View></View>)
                    }) :
                        <View></View>
                    }
                </View>
            </ScrollView>

            <View style={{ width: '100%', justifyContent: 'center', alignItems: 'center', padding: 5, position: 'absolute', bottom: 0 }}>
                <View style={styles.footer}>
                    <TouchableOpacity onPress={() => setScreenControl(0)} style={styles.fItem}>
                        <Icon2 name="eye" size={38} color={screenControl == 0 ? theme.colors.mainColor : "#ccc"} />
                        <Text>Thiết bị cảm biến</Text>
                    </TouchableOpacity>
                    <TouchableOpacity onPress={() => setScreenControl(1)} style={styles.fItem}>
                        <Icon2 name="hand-paper" size={38} color={screenControl == 1 ? theme.colors.mainColor : "#ccc"} />
                        <Text>Thiết bị điều khiển</Text>
                    </TouchableOpacity>
                </View>
            </View>

        </BackgroundTop>
    )

}

const styles = StyleSheet.create({

    header: {
        backgroundColor: theme.colors.mainColor,
        width: '100%',
        height: 80,
        flexDirection: 'row',
        justifyContent: 'space-between', // Canh lề giữa các phần tử
        alignItems: 'center'
    },
    item: {
        height: 90,
        overflow: 'hidden',
        marginBottom: 10,
        width: "100%",
        flexDirection: 'row',
        padding: 20,
        alignItems: 'center',
        borderColor: '#ccc',
        borderWidth: 2,
        borderRadius: 20,

    },
    item2: {
        height: 127,
        overflow: 'hidden',
        marginBottom: 10,
        width: "100%",
        flexDirection: 'row',
        padding: 20,
        alignItems: 'center',
        borderColor: '#ccc',
        borderWidth: 2,
        borderRadius: 20,

    },
    itemright: {
        height: 80,
        width: "100%",


    },
    itemleft: {
        height: 80,
        width: 80,
        justifyContent: 'center',
        alignItems: 'center'

    },
    box: {
        // backgroundColor:'#ccc',
        height: "100%",
        alignItems: 'center',
        justifyContent: 'center',
    },
    logo: {
        width: "90",
        height: "84",
        justifyContent: 'center',
        alignItems: 'center',
        alignSelf: 'center',
    },
    footer: {
        width: "95%",
        height: 70,
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

})
