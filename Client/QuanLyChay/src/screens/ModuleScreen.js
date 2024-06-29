import React, { useEffect, useState } from 'react'
import BackgroundTop from '../components/BackgroundTop'
import {
    View, StyleSheet, Text,
    TouchableOpacity, ScrollView, Button, Alert

} from 'react-native';
import { theme } from '../core/theme'
import Icon from 'react-native-vector-icons/FontAwesome';
import Icon2 from 'react-native-vector-icons/FontAwesome5';
import { getData, postData, postDataNobody } from '../api/Api';
import TextInput from '../components/TextInput';
import ButtonC from '../components/Button';
import { Picker } from '@react-native-picker/picker';
import { useNavigate } from 'react-router-native';



export default function ModuleScreen() {
    const navigate = useNavigate();

    const [aparments, setApartment] = useState([]);
    const [aparmenId, setApartmentId] = useState('');
    const [unitId, setUnitId] = useState('');
    const [moduleId, setModuleId] = useState('');
    const [units, setUnit] = useState([]);
    const [modules, setModule] = useState([]);
    const [searchKey, setSearchKey] = useState('')
    const [screen, setScreen] = useState(2)
    const [modal, setModal] = useState(false)
    //
    const [uModules, setUmodule] = useState([]);
    const [uModuleId, setUmoduleId] = useState('');
    const [udevices, setUdevice] = useState([]);

    useEffect(() => {

        initial()
    }, [])
    const initial = async () => {
        const dt = await postData('Building/getlist', {
            "searchKey": ""
        });
        setApartment(dt);
        var md = await getData('Module/getbyuser');
        setUmodule(md);
    }
    const fetchUnit = async (apartmentId) => {
        const dt = await postData('Apartment/getbyapartment', {
            "id": apartmentId
        });
        setUnit(dt);
    }
    const fetchModule = async () => {
        const dt = await postData('Module/getbyunit', {
            "unitId": unitId
        });
        setUmodule(dt);
    }
    const onPressAddCancel = () => {
        setModal(false)
    }
    const handlePickApartment = (aparmenId) => {
        setApartmentId(aparmenId)
        fetchUnit(aparmenId);
        console.log('apart', aparmenId);
    }
    const handlePickUnit = (valueUnit) => {
        setUnitId(valueUnit)
        console.log('unit', valueUnit);
    }
    const onPressAdd = async () => {
        // const rs = await postData('Module/addtoUnit', {
        //     "unitId": unitId,
        //     "moduleId": moduleId
        // });
        const rs = await postDataNobody(`Module/addtouser/${moduleId}`);
        if (rs != null && rs === true) {
            setModal(false);
            const rs = await getData(`Device/${moduleId}`);
            setUdevice(rs);
            var md = await getData('Module/getbyuser');
            setUmodule(md);
            setUmoduleId(moduleId)
         
        }
        else {
            Alert.alert(
                'Lỗi', // Tiêu đề của thông báo
                'Không tồn tại module', // Nội dung của thông báo
                [
                    { text: 'Cancel', onPress: () => setModal(false), style: 'cancel' },
                    { text: 'OK', onPress: () => setModal(false) },
                ],
                { cancelable: false }
            );
        }
        

    }
    const onRemoveRule = (id) => {
        const rs = postDataNobody(`Module/setnullunit/${id}`);
        
    } 
    const handlePickModuleId = async (id) => {
        const rs = await getData(`Device/${uModuleId}`);
        setUdevice(rs);
        setUmoduleId(id)
    }
    const handlePressMatchModule = (value) => {
        setUmoduleId(value)
    }
    const synscModule = async () => {
        const dt = await postData('Module/getbyunit', {
            "unitId": unitId
        });
        setModule(dt);
    }
    const addModuleToApartment = async () => {
        const rs = await postData('Module/addtoUnit', {
            "unitId": unitId,
            "moduleId": uModuleId
        })
        const dt = await postData('Module/getbyunit', {
            "unitId": unitId
        });
        setModule(dt);
    }
    return (
        <BackgroundTop>

            <View style={{ flex: 1, position: 'relative' }}>
                {/* modal */}
                <View style={modal == true ? { position: 'absolute', top: 0, width: "100%", height: '100%', backgroundColor: 'rgba(0,0,0,0.6)', zIndex: 4, justifyContent: 'center', alignItems: 'center', padding: 10, } : { display: 'none' }}>
                    <View style={{ width: '80%', backgroundColor: '#fff', padding: 10, borderRadius: 10 }}>
                        <TextInput
                            label="ModuleId"
                            returnKeyType="next"
                            value={moduleId}
                            onChangeText={(text) => setModuleId(text)}
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
                        <TouchableOpacity onPress={() => navigate(-1)}>
                            <Icon name="angle-double-left" size={30} color="#fff" />
                        </TouchableOpacity>
                    </View>
                    <View style={[styles.box, { width: '40%' }]}>
                        <Text style={{ fontWeight: '700', color: '#fff' }}> Quản lý Module</Text>
                    </View>
                    <View style={[styles.box, { width: '30%' }]}>
                        <TouchableOpacity onPress={() => setModal(true)}>
                            <Icon name="plus" size={30} color="#fff" />
                        </TouchableOpacity>
                    </View>
                </View>
                <ScrollView >
                    {/* picker */}
                    <View style={screen == 2 ? "" : { display: 'none' }}>
                        <View style={{ padding: 10 }}>
                            <View style={{ marginLeft: 13, flexDirection: 'row', flexWrap: 'nowrap', width: '100%', height: 50, alignItems: 'center' }}>
                                <Text style={{ fontWeight: '500' }}>Chọn mô đun: </Text>
                                <Picker
                                    style={{ width: 200, height: 40 }}
                                    selectedValue={uModuleId}
                                    onValueChange={(itemValue) =>
                                        handlePickModuleId(itemValue)
                                    }>
                                    <Picker.Item label="" value={0} />
                                    {
                                        uModules != null && uModules.length > 0 ? uModules.map((e, i) => {
                                            return (<Picker.Item key={i} label={e.moduleName} value={e.id} />)
                                        })
                                            : <Picker.Item label="None" value={0} />
                                    }
                                </Picker>
                            </View>
                            {udevices.length > 0 ? udevices.map((e, i) => {
                                return (true ? <View key={i} style={styles.item2}>
                                    <View style={styles.itemleft}>
                                        <Icon name="eye" size={60} color={theme.colors.mainColor} />
                                    </View>
                                    <View style={styles.itemright}>
                                        <View style={{ flexDirection: 'row', }}>
                                            <Text style={{ fontWeight: '500' }}>Tên thiết bị: {e.topic} </Text>
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
                    </View>
                    <View style={screen == 3 ? "" : { display: 'none' }}>
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
                        <View style={{ marginLeft: 13, flexDirection: 'row', flexWrap: 'nowrap', width: '100%', height: 50, alignItems: 'center' }}>
                            <Text style={{ fontWeight: '500' }}>Chọn mô đun: </Text>
                            <Picker
                                style={{ width: 200, height: 40 }}
                                selectedValue={uModuleId}
                                onValueChange={(itemValue) =>
                                    handlePressMatchModule(itemValue)
                                }>
                                <Picker.Item label="" value={0} />
                                {uModules != null && uModules.length > 0 ? uModules.map((e, i) => {
                                    return (<Picker.Item key={i} label={e.moduleName} value={e.id} />)
                                }) : <Picker.Item label="None" value={0} />}
                            </Picker>
                        </View>
                        <View style={{ flexDirection: 'row', justifyContent: 'space-between', padding: 10 }}>
                            <Button color={theme.colors.mainColor} onPress={() => addModuleToApartment()} title="Thêm mô đun"></Button>
                            <Button color={theme.colors.mainColor} onPress={() => synscModule()} title="Xem mô đun"></Button>
                        </View>
                        <View style={{ padding: 10 }}>
                            {modules != null && modules.length > 0 ? modules.map((e, i) => {
                                return <View key={i} style={styles.item}>

                                    <Icon2 onPress={() => {
                                        onRemoveRule(e.id)
                                    }} name='trash-alt' size={20} color='red' style={{ position: 'absolute', right: 20, top: 5 }}></Icon2>

                                    <View style={styles.itemleft}>
                                        <Icon name="building" size={80} color={theme.colors.mainColor} />
                                    </View>
                                    <View style={styles.itemright}>
                                        <View style={{ flexDirection: 'row', }}>
                                            <Text style={{ fontWeight: '500' }}>Tên module: </Text>
                                            <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.moduleName}</Text>
                                        </View>

                                        <View style={{ flexDirection: 'row' }}>
                                            <Text style={{ fontWeight: '500' }}>Mô tả: </Text>
                                            <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.desc}</Text>
                                        </View>

                                        <View style={{ flexDirection: 'row' }}>
                                            <Text style={{ fontWeight: '500' }}>Ngày tạo: </Text>
                                            <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.dateCreate}</Text>
                                        </View>
                                    </View>

                                </View>
                            }) : ""
                            }
                        </View>
                    </View>
                </ScrollView>
            </View>
            <View style={{ width: '100%', justifyContent: 'center', alignItems: 'center', padding: 5, position: 'absolute', bottom: 0 }}>
                <View style={styles.footer}>
                    <TouchableOpacity onPress={() => setScreen(2)} style={styles.fItem}>
                        <Text style={screen == 2 ? { fontWeight: '600', color: 'red' } : styles.text}>Thiết bị hiện có</Text>
                    </TouchableOpacity>
                    <TouchableOpacity onPress={() => setScreen(3)} style={styles.fItem}>
                        <Text style={screen == 3 ? { fontWeight: '600', color: 'red' } : styles.text}>Thêm vào căn hộ</Text>
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
    text: {
        fontWeight: '500'
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
        justifyContent: 'center',
        alignItems: 'center'
    },
})
