import React, { useEffect, useState } from 'react'
import BackgroundTop from '../components/BackgroundTop'
import {
    View, StyleSheet, Text,
    TouchableOpacity, ScrollView, Button, Alert

} from 'react-native';
import { theme } from '../core/theme'
import Icon from 'react-native-vector-icons/FontAwesome';
import { getData, postData } from '../api/Api';
import TextInput from '../components/TextInput';
import ButtonC from '../components/Button';
import { Picker } from '@react-native-picker/picker';
import Icon2 from 'react-native-vector-icons/FontAwesome5';



export default function AutoScreen({ navigation }) {
    const [aparments, setApartment] = useState([]);
    const [aparmenId, setApartmentId] = useState('');
    const [unitId, setUnitId] = useState('');
    const [moduleId, setModuleId] = useState('');
    const [units, setUnit] = useState([]);
    const [modules, setModule] = useState([]);
    const [screen, setScreen] = useState(0)
    const [devices, setDevice] = useState(0)
    const [rules, setRule] = useState({})
    const [deviceRId, setDeviceRId] = useState("")
    const [deviceWId, setDeviceWId] = useState("")
    const [ruleDesc, setRuleDesc] = useState("")
    const [threshold, setThreshold] = useState("")
    const [onOF, setOnof] = useState("")
    useEffect(() => {

        initial()
    }, [])

    const initial = async () => {
        const dt = await postData('Apartment/getlist', {
            "searchKey": ""
        });
        setApartment(dt);
    }
    const fetchUnit = async (apartmentId) => {
        const dt = await postData('Unit/getbyapartment', {
            "id": apartmentId
        });
        setUnit(dt);
    }

    const onPressAddCancel = () => {
        setScreen(0);
    }
    const handlePickApartment = (aparmenId) => {
        setApartmentId(aparmenId)
        fetchUnit(aparmenId);
    }
    const handlePickUnit = async (valueUnit) => {
        setUnitId(valueUnit);
        const dt = await postData('Module/getbyunit', {
            "unitId": valueUnit
        });
        setModule(dt);
    }
    const onPressAdd = async () => {

        const rs = await postData('Rule', {
            "desc": ruleDesc,
            "isActive": false,
            "nameCompare": deviceRId,
            "threshold": threshold,
            "topicWrite": deviceWId,
            "status": onOF,
            "port": "0",
            "moduleId": moduleId
        });

       setScreen(0)
    }
    const handlePickModule = async (value) => {
        setModuleId(value)
        const deviceModules = await getData(`Device/${value}`);
        setDevice(deviceModules);
        console.log(deviceModules);
    }
    const getListRule = async () => {
        const rs = await getData('Rule/'+moduleId);
        setRule(rs);
    }
    return (
        <BackgroundTop>

            <View style={{ flex: 1, position: 'relative' }}>
                {/* modal */}
                <View style={screen == 1 ? { position: 'absolute', top: 0, width: "100%", height: '100%', backgroundColor: 'rgba(0,0,0,0.6)', zIndex: 4, justifyContent: 'center', alignItems: 'center', padding: 10, } : { display: 'none' }}>
                    <View style={{ width: '100%', backgroundColor: '#fff', padding: 10, borderRadius: 10 }}>
                        <TextInput
                            label="Mô tả rule"
                            returnKeyType="next"
                            value={ruleDesc}
                            onChangeText={(text) => setRuleDesc(text)}
                            autoCapitalize="none"
                        />
                        <Text style={{ fontWeight: '500' }}>Chọn sensor: </Text>
                        <Picker
                            style={{ width: 200, height: 40 }}
                            selectedValue={deviceRId}
                            onValueChange={(itemValue) =>
                                setDeviceRId(itemValue)
                            }>
                            <Picker.Item label="" value={0} />
                            {
                                devices != null && devices.length > 0 ? devices.map((e, i) => {
                                    if (e.type == 0) {
                                        return (<Picker.Item key={i} label={e.topic} value={e.id} />)
                                    }

                                })
                                    : <Picker.Item label="None" value={0} />
                            }
                        </Picker>
                        <TextInput
                            label="Ngưỡng"
                            returnKeyType="next"
                            value={threshold}
                            onChangeText={(text) => setThreshold(text)}
                            autoCapitalize="none"
                        />
                        <Text style={{ fontWeight: '500' }}>Chọn device: </Text>
                        <Picker
                            style={{ width: 200, height: 40 }}
                            selectedValue={deviceWId}
                            onValueChange={(itemValue) =>
                                setDeviceWId(itemValue)
                            }>
                            <Picker.Item label="" value={0} />
                            {
                                devices != null && devices.length > 0 ? devices.map((e, i) => {
                                    if (e.type == 1) {
                                        return (<Picker.Item key={i} label={e.topic} value={e.id} />)
                                    }

                                })
                                    : <Picker.Item label="None" value={0} />
                            }
                        </Picker>
                        <TextInput
                            label="Bật/Tắt(1/0)"
                            returnKeyType="next"
                            value={onOF}
                            onChangeText={(text) => setOnof(text)}
                            autoCapitalize="none"
                        />
                        <ButtonC onPress={onPressAdd} mode="contained" >
                            Submit
                        </ButtonC>
                        <ButtonC onPress={onPressAddCancel} mode="contained" style={{ backgroundColor: '#ccc', color: '#000' }} >
                            Cancel
                        </ButtonC>
                    </View>
                </View>
                {/* header */}
                <View style={styles.header}>
                    <View style={[styles.box, { width: '30%', }]}>
                        <TouchableOpacity onPress={() => navigation.navigate('Dashboard')}>
                            <Icon name="angle-double-left" size={30} color="#fff" />
                        </TouchableOpacity>
                    </View>
                    <View style={[styles.box, { width: '40%' }]}>
                        <Text style={{ fontWeight: '700', color: '#fff' }}> Quản lý tự động</Text>
                    </View>
                    <View style={[styles.box, { width: '30%' }]}>
                        <TouchableOpacity onPress={() => setScreen(1)}>
                            <Icon name="plus" size={30} color="#fff" />
                        </TouchableOpacity>
                    </View>
                </View>
                {/* picker */}

                <View style={{ marginLeft: 13, flexDirection: 'row', flexWrap: 'nowrap', width: '100%', height: 50, alignItems: 'center' }}>
                    <Text style={{ fontWeight: '500' }}>Chọn tòa: </Text>
                    <Picker
                        style={{ width: 200, height: 40 }}
                        selectedValue={aparmenId}
                        onValueChange={(itemValue) =>
                            handlePickApartment(itemValue)
                        }>
                        <Picker.Item label="" value={0} />
                        {
                            aparments != null && aparments.length > 0 ? aparments.map((e, i) => {
                                return (<Picker.Item key={i} label={e.name} value={e.id} />)
                            })
                                : <Picker.Item label="None" value={0} />
                        }
                    </Picker>
                </View>
                <View style={{ marginLeft: 13, flexDirection: 'row', flexWrap: 'nowrap', width: '100%', height: 50, alignItems: 'center' }}>
                    <Text style={{ fontWeight: '500' }}>Chọn căn hộ: </Text>
                    <Picker
                        style={{ width: 200, height: 40 }}
                        selectedValue={unitId}
                        onValueChange={(itemValue) =>
                            handlePickUnit(itemValue)
                        }>
                        <Picker.Item label="" value={0} />
                        {units != null && units.length > 0 ? units.map((e, i) => {
                            return (<Picker.Item key={i} label={e.name} value={e.id} />)
                        }) : <Picker.Item label="None" value={0} />}
                    </Picker>
                </View>
                {/* module */}
                <View style={{ marginLeft: 13, flexDirection: 'row', flexWrap: 'nowrap', width: '100%', height: 50, alignItems: 'center' }}>
                    <Text style={{ fontWeight: '500' }}>Chọn module: </Text>
                    <Picker
                        style={{ width: 200, height: 40 }}
                        selectedValue={moduleId}
                        onValueChange={(itemValue) =>
                            handlePickModule(itemValue)
                        }>
                        <Picker.Item label="" value={0} />
                        {modules != null && modules.length > 0 ? modules.map((e, i) => {
                            return (<Picker.Item key={i} label={e.moduleName} value={e.id} />)
                        }) : <Picker.Item label="None" value={0} />}
                    </Picker>
                </View>
                <Button onPress={() => getListRule()} title="danh sách Rule"></Button>                 
                <ScrollView>
                {rules != null && rules.length > 0 ? rules.map((e, i) => {
                            return <View key={i} style={styles.item}>
                                <Icon2 onPress={() => handleRepair(e)} name='tools' size={20} color='blue' style={{ position: 'absolute', right: 5, top: 5 }}></Icon2>
                                <Icon2 onPress={() => handleRemove(e.id)} name='trash-alt' size={20} color='red' style={{ position: 'absolute', right: 35, top: 5 }}></Icon2>

                                <View style={{ position: 'relative' }}><Icon color='#fff' name='rocket' size={30}></Icon></View>
                                <View style={styles.itemleft}>
                                    <Icon name="home" size={80} color={theme.colors.mainColor} />
                                </View>
                                <View style={styles.itemright}>
                                    <View style={{ flexDirection: 'row' }}>
                                        <Text style={{ fontWeight: '500' }}>Mô tả: </Text>
                                        <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.desc}</Text>
                                    </View>

                                    <View style={{ flexDirection: 'row' }}>
                                        <Text style={{ fontWeight: '500' }}>sensor: </Text>
                                        <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.nameCompare.split('/')[4]}</Text>
                                    </View>

                                    <View style={{ flexDirection: 'row' }}>
                                        <Text style={{ fontWeight: '500' }}>Ngưỡng: </Text>
                                        <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.threshold}</Text>
                                    </View>
                                    <View style={{ flexDirection: 'row' }}>
                                        <Text style={{ fontWeight: '500' }}>Ngoại vi: </Text>
                                        <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.topicWrite.split('/')[4]}</Text>
                                    </View>
                                    <View style={{ flexDirection: 'row' }}>
                                        <Text style={{ fontWeight: '500' }}>Trạng thái: </Text>
                                        <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.status}</Text>
                                    </View>
                                </View>

                            </View>
                        }) : ""
                        }

                </ScrollView>
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

})
