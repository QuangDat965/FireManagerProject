import React, { useEffect, useState } from 'react'
import BackgroundTop from '../components/BackgroundTop'
import { View, StyleSheet, Text, TouchableOpacity, ScrollView, Button } from 'react-native';
import { theme } from '../core/theme'
import Icon from 'react-native-vector-icons/FontAwesome';
import Icon2 from 'react-native-vector-icons/FontAwesome6'
import { getData, getDataNo, postData } from '../api/Api';
import MqttService from '../helpers/mqttService';



export default function UnitDetailScreen(router) {
    const [dataModule, setDataModule] = useState([])
    const [screenControl, setScreenControl] = useState(0)
    const unit = router.route.params.unit;

    useEffect(() => {

        const initial = async () => {
            const modules = await postData('Module/getbyunit', {
                "unitId": unit.id
            });
            const systemId = await getDataNo('System/id');
            modules.map(e => {
                MqttProcess(e.id, systemId)
            })
        }


        initial()
    }, [])
    const MqttProcess = (moduleId, s) => {
        const service = new MqttService();
        const succcess = async (eee) => {
            console.log('conneted to sever');
            const topic = `${s}/${moduleId}`
            console.log('topic:', topic);
            service.subscribeTopic(`${topic}/#`);
        }
        service.connect(succcess);
        const client = service.getCLient();
        client.onMessageArrived = (message) => {

            console.log("Message received on topic: " + message.destinationName);
            console.log("Message content: " + message.payloadString);
            const topic = message.destinationName;
            const payload = message.payloadString;
            const parts = topic.split('/')
            var obj = {
                id: topic,
                payload: payload,
                module: parts[1],
                port: parts[2],
                type: parts[3],
                name: parts[4]
            }
            var oldData = dataModule;
            console.log(oldData);
            if (oldData.length <= 0) {
                oldData.push(obj)
                setDataModule([...oldData])
            }
            else {
                var index = oldData.findIndex(p => p.id === obj.id)
                if (index != -1) {
                    oldData[index] = obj;
                    setDataModule([...oldData])
                }
                else {
                    oldData.push(obj)
                    setDataModule([...oldData])
                }
            }
        };
    }
    const getSystemId = async () => {
        return await getData('System/id');
    }
    const testFun = async () => {
        const fetchdata = async () => {
            const dt = await getData('Apartment/getlist');
        }
        fetchdata();
        console.log(await getSystemId());
    }
    return (
        <BackgroundTop>
            {/* header */}
            <View style={styles.header}>
                <View style={[styles.box, { width: '30%', }]}>
                    <TouchableOpacity onPress={() => router.navigation.navigate('Dashboard')}>
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
                <View style={{ position: 'relative' }}><Icon color='#fff' name='rocket' size={30}></Icon></View>
                <View style={styles.itemleft}>
                    <Icon name="home" size={80} color={theme.colors.mainColor} />
                </View>
                <View style={styles.itemright}>
                    <View style={{ flexDirection: 'row', }}>
                        <Text style={{ fontWeight: '500' }}>Tên tòa: </Text>
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

            <Button onPress={() => testFun()} title="Thông số thiết bị"></Button>
            <ScrollView>

                <View style={screenControl == 0 ? { padding: 10 } : { display: 'none' }}>
                    {dataModule.length > 0 ? dataModule.map(e => {
                       return (e.type === 'R'?<View style={styles.item}>
                       <View style={{ position: 'relative' }}><Icon color='#fff' name='rocket' size={30}></Icon></View>
                       <View style={styles.itemleft}>
                           <Icon name="eye" size={80} color={theme.colors.mainColor} />
                       </View>
                       <View style={styles.itemright}>
                           <View style={{ flexDirection: 'row', }}>
                               <Text style={{ fontWeight: '500' }}>Tên sensor: {e.name} </Text>
                               <Text style={{ fontWeight: '500', opacity: 0.7 }}></Text>
                           </View>

                           <View style={{ flexDirection: 'row' }}>
                               <Text style={{ fontWeight: '500' }}>Giá trị: {e.payload} </Text>
                               <Text style={{ fontWeight: '500', opacity: 0.7 }}></Text>
                           </View>

                           <View style={{ flexDirection: 'row' }}>
                               <Text style={{ fontWeight: '500' }}>Module: </Text>
                               <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.module}</Text>
                           </View>

                           <View style={{ flexDirection: 'row' }}>
                               <Text style={{ fontWeight: '500' }}>Chân: </Text>
                               <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.port}</Text>
                           </View>

                           <View style={{ flexDirection: 'row' }}>
                               <Text style={{ fontWeight: '500' }}>Đơn vị: </Text>
                               <Text style={{ fontWeight: '500', opacity: 0.7 }}>Ampe</Text>
                           </View>
                       </View>

                   </View>:<View></View>)
                    }) :
                    <View></View>
                    }
                </View>

                <View style={screenControl == 1 ? { padding: 10 } : { display: 'none' }}>
                    {dataModule.length > 0 ? dataModule.map(e => {
                       return (e.type === 'W'?<View style={styles.item}>
                       <View style={{ position: 'relative' }}><Icon color='#fff' name='rocket' size={30}></Icon></View>
                       <View style={styles.itemleft}>
                           <Icon2 name="power-off" size={80} color={theme.colors.mainColor} />
                       </View>
                       <View style={styles.itemright}>
                           <View style={{ flexDirection: 'row', }}>
                               <Text style={{ fontWeight: '500' }}>Chức năng: {e.name} </Text>
                               <Text style={{ fontWeight: '500', opacity: 0.7 }}></Text>
                           </View>

                           <View style={{ flexDirection: 'row' }}>
                               <Text style={{ fontWeight: '500' }}>Trạng thái: {e.payload} </Text>
                               <Text style={{ fontWeight: '500', opacity: 0.7 }}></Text>
                           </View>

                           <View style={{ flexDirection: 'row' }}>
                               <Text style={{ fontWeight: '500' }}>Module: </Text>
                               <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.module}</Text>
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

                   </View>:<View></View>)
                    }) :
                    <View></View>
                    }
                </View>
            </ScrollView>

            <View style={{ width: '100%', justifyContent: 'center', alignItems: 'center', padding: 5, position: 'absolute', bottom: 0 }}>
                <View style={styles.footer}>
                    <TouchableOpacity onPress={() => setScreenControl(0)} style={styles.fItem}>
                        <Icon2 name="eye" size={50} color={screenControl == 0 ? theme.colors.mainColor : "#ccc"} />
                        <Text>Thiết bị cảm biến</Text>
                    </TouchableOpacity>
                    <TouchableOpacity onPress={() => setScreenControl(1)} style={styles.fItem}>
                        <Icon2 name="hand-paper" size={50} color={screenControl == 1 ? theme.colors.mainColor : "#ccc"} />
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
        height: 117,
        flexDirection: 'row',
        justifyContent: 'space-between', // Canh lề giữa các phần tử
        alignItems: 'center'
    },
    item: {
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
        marginRight: 10
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
        height: 111,
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
