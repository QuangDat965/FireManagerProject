import React, { useEffect, useState } from 'react'
import BackgroundTop from '../components/BackgroundTop'
import {
    View, StyleSheet, Text,
    TouchableOpacity, ScrollView, Button, Alert, TextInput as Input

} from 'react-native';
import { theme } from '../core/theme'
import Icon from 'react-native-vector-icons/FontAwesome';
import { deleteData, getData, postData, postDataNobody } from '../api/Api';
import TextInput from '../components/TextInput';
import ButtonC from '../components/Button';
import { Picker } from '@react-native-picker/picker';
import Icon2 from 'react-native-vector-icons/FontAwesome5';
import CheckMultiple from '../components/CheckMultipe';
import { useNavigate } from 'react-router-native';
import CustomPicker from '../components/CustomPicker';

const items = [
    { id: '1', label: 'Football' },
    { id: '2', label: 'Baseball' },
    { id: '3', label: 'Hockey' },
    { id: '4', label: 'Basketball' }
];


export default function AutoScreen() {
    const navigate = useNavigate();

    const [aparments, setApartment] = useState([]);
    const [aparmenId, setApartmentId] = useState('');
    const [unitId, setUnitId] = useState('');
    const [moduleId, setModuleId] = useState('');
    const [units, setUnit] = useState([]);
    const [modules, setModule] = useState([]);
    const [screen, setScreen] = useState(0)
    const [devices, setDevice] = useState([])
    const [rules, setRule] = useState({})
    const [deviceRId, setDeviceRId] = useState("")
    const [deviceWId, setDeviceWId] = useState("")
    const [ruleDesc, setRuleDesc] = useState("")
    const [threshold, setThreshold] = useState("")
    const [onOF, setOnof] = useState("")
    const [typeRule, setTypeRule] = useState(0)

    const [sensors, setSensor] = useState([])
    const [controls, setControl] = useState([])
    useEffect(() => {

        initial()
    }, [])

    const initial = async () => {
        const dt = await postData('Building/getlist', {
            "searchKey": ""
        });
        setApartment(dt);
    }
    const fetchUnit = async (apartmentId) => {
        const dt = await postData('Apartment/getbyapartment', {
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
        const rs = await getData('Rule/' + moduleId);
        setRule(rs);
    }
    const onAddSensor = (adds) => {

        adds.map(e => {
            e["typeCompare"] = 0;
            e["threshold"] = 0;
        })
        setSensor(adds)
    }
    const setCompareDisplay = (e) => {
        sensors.map(item => {
            if (item.topic == e.topic) {
                item.isShow = item.isShow == "0" ? "1" : "0"
            }
        })
        setSensor([...sensors])
    }
    const handlePickThrehold = (id, value) => {
        sensors.map(item => {
            if (item.topic == id) {
                item.threshold = value
            }
        })
        setSensor([...sensors])
    }
    const handlePickThreholdDevice = (id, value) => {
        devices.map(item => {
            if (item.topic == id) {
                item.threshold = value
            }
        })
        setSensor([...devices])
    }
    const handleRemoveRuleSet = (e) => {
        console.log(e);
        const temps = sensors.filter(x => x.topic == e.topic);
        console.log(temps);
        setSensor(temps)
    }
    const OnPressSubmitRule = async () => {
       
        var list = [];

        sensors.forEach(p => {
            var obj = {}
            obj.deviceId= p.id,
            obj.threshHold= p.threshold,
            obj.typeCompare= p.typeCompare
            list.push(obj)
        })
        controls.forEach(p => {
            var obj = {}
            obj.deviceId= p.id,
            obj.threshHold= p.threshold,
            obj.typeCompare= p.typeCompare
            list.push(obj);
        })
        const rs = await postData('Rule', {

            "desc": ruleDesc,
            "isActive": false,
            "isFire": false,
            "typeRule": typeRule,
            "moduleId": moduleId,
            "topicThreshholds": list
        })
        setScreen(0)
        
    }
    const onActiveModule = async (e) => {
        if(e.isActive==true) {
           await postDataNobody('Rule/deactive/'+e.id);

        }
        else {
           await postDataNobody('Rule/active/'+e.id);

        }
         await getListRule();
    }
    const onRemoveRule = async (id) => {
        console.log(id);
        const rs = await postDataNobody('Rule/'+id);
        console.log(rs);
        await getListRule();
    }
    return (
        <BackgroundTop>

            <View style={{ flex: 1, position: 'relative' }}>
                {/* modal */}
                <View style={screen == 1 ? { position: 'absolute', flex: 1, top: 0, width: "100%", height: '100%', backgroundColor: 'rgba(0,0,0,0.6)', zIndex: 4, justifyContent: 'center', alignItems: 'center', padding: 10, } : { display: 'none' }}>
                    <View style={{ width: '100%', backgroundColor: '#fff', padding: 10, borderRadius: 10 }}>
                        <TextInput
                            label="Mô tả luật"
                            returnKeyType="next"
                            value={ruleDesc}
                            onChangeText={(text) => setRuleDesc(text)}
                            autoCapitalize="none"
                        />
                        <View style={{ flexDirection: 'row', alignItems: 'center' }}>
                            <Text style={{ fontWeight: '500' }}>Kiểu luật: </Text>
                            <CustomPicker
                                items={[{ id: 0, label: "AND" }, { id: 1, label: "OR" }]}
                                onSelectionChange={(e) => { setTypeRule(e.id) }}
                                title="chọn kiểu"
                            />
                        </View>
                        <ScrollView style={{ height: 300 }}>
                            <View style={{ flexDirection: 'row', alignItems: 'center' }}>
                                <CheckMultiple items={devices.filter(x => x.type == 0)}
                                    onSelectionChange={(adds) => { onAddSensor(adds) }}
                                    labelSet="topic"
                                    title={"Chọn cảm biến"}
                                />
                            </View >


                            {/* list sensor */}
                            <View style={styles.shadow}>
                                {sensors != null && sensors.length > 0 ? sensors.map((e, i) => {
                                    e.isShow == null ? e["isShow"] = "0" : e.isShow
                                    e.threshold == null ? e["threshold"] = "0" : e.threshold
                                    if (e.type == 0) {
                                        return (<View key={i} style={{ position: 'relative', flexDirection: 'row', padding: 0, alignContent: 'center', marginBottom: 4 }}>
                                            <View style={{ justifyContent: 'center', alignItems: 'center' }}>
                                                <Text style={{ justifyContent: 'center', alignItems: 'center' }}>{e.topic}</Text>
                                            </View>
                                            <CustomPicker items={[{ id: 0, label: '>' }, { id: 1, label: '<' }, { id: 2, label: '=' }]}
                                                onSelectionChange={(select) => { e.typeCompare = select.id }}
                                                title="chọn kiểu"
                                            />


                                            <Input style={{ borderWidth: 1, borderColor: '#ccc', marginLeft: 10, paddingHorizontal: 3, borderRadius: 4 }}
                                                placeholder="ngưỡng"
                                                value={e.threshold}
                                                returnKeyType='next'
                                                onChangeText={(value) => { handlePickThrehold(e.topic, value) }}
                                                keyboardType="numeric" />

                                        </View>)
                                    }

                                }) : <View></View>}
                            </View>

                            {/* device */}
                            <View style={{ flexDirection: 'row', alignItems: 'center' }}>
                                <CheckMultiple items={devices.filter(x => x.type === 1)}
                                    onSelectionChange={(adds) => setControl(adds)}
                                    labelSet="topic"
                                    title="Chọn thiết bị điều khiển"
                                />
                            </View >
                            {/* listdevice */}
                            <View style={styles.shadow}>
                                {controls != null && controls.length > 0 ? controls.map((e, i) => {
                                    e.isShow == null ? e["isShow"] = "0" : e.isShow
                                    e.threshold == null ? e["threshold"] = "0" : e.threshold
                                    if (e.type == 1) {
                                        return (<View key={i} style={{ position: 'relative', flexDirection: 'row', padding: 0, alignItems: 'center', marginBottom: 4 }}>
                                            <View style={{ justifyContent: 'center', alignItems: 'center' }}>
                                                <Text style={{ justifyContent: 'center', alignItems: 'center' }}>{e.topic}</Text>
                                            </View>


                                            <Text> =</Text>

                                            <CustomPicker
                                                title={e.threshold == 0 ? "OFF" : e.threshold == 1 ? "OFF" : "Chọn giá trị"}
                                                items={[{ id: 0, label: "ON" }, { id: 1, label: "OFF" }]}
                                                onSelectionChange={(item) => { e.threshold = item.id }}

                                            />

                                        </View>)
                                    }

                                }) : <View></View>}
                            </View>
                        </ScrollView>
                        <ButtonC onPress={()=> OnPressSubmitRule()} mode="contained" >
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
                        <TouchableOpacity onPress={() => navigate(-1)}>
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
                            <Icon2 onPress={() => handleRepair(e)} name='eye' size={20} color='blue' style={{ position: 'absolute', right: 5, top: 5 }}></Icon2>
                            <Icon2 onPress={() => {
                                onRemoveRule(e.id)
                            }} name='trash-alt' size={20} color='red' style={{ position: 'absolute', right: 35, top: 5 }}></Icon2>

                            <View style={styles.itemleft}>
                                <Icon2 name="balance-scale" size={70} color={theme.colors.mainColor} />
                            </View>
                            <View style={styles.itemright}>
                                <View style={{ flexDirection: 'row' }}>
                                    <Text style={{ fontWeight: '500' }}>Mô tả: </Text>
                                    <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.desc}</Text>
                                </View>

                                <View style={{ flexDirection: 'row' }}>
                                    <Text style={{ fontWeight: '500' }}>Kiểu so sánh: </Text>
                                    <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.typeRule==0?"And":"Or"}</Text>
                                </View>
                                <View style={{ flexDirection: 'row', alignItems:'center' }}>
                                    <Text style={{ fontWeight: '500' }}>Kích hoạt </Text>
                                    <Icon2 onPress={()=>{
                                        onActiveModule(e)
                                    }} name={e.isActive==true?'toggle-on':'toggle-off'}
                                    size={30}
                                    />
                                    
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
    shadow: {
        padding: 20,
        backgroundColor: '#fff',
        borderRadius: 10,
        shadowColor: '#000',
        shadowOffset: {
            width: 0,
            height: 2,
        },
        shadowOpacity: 0.25,
        shadowRadius: 3.84,
        elevation: 5,
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
    inputAndroid: {
        fontSize: 16,
        paddingHorizontal: 10,
        paddingVertical: 8,
        borderWidth: 0.5,
        borderColor: 'gray',
        borderRadius: 8,
        color: 'black',
        paddingRight: 10, // Đảm bảo khoảng trống cho icon
    },

})
const pickerSelectStyles = {
    inputIOS: {
        fontSize: 16,
        paddingVertical: 12,
        paddingHorizontal: 10,
        borderWidth: 1,
        borderColor: 'gray',
        borderRadius: 4,
        color: 'black',
        paddingRight: 30, // Đảm bảo khoảng trống cho icon
    },

};