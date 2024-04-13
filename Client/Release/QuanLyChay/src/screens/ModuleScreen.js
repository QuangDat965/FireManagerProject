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


export default function ModuleScreen({ navigation }) {
    const [aparments, setApartment] = useState([]);
    const [aparmenId, setApartmentId] = useState('');
    const [unitId, setUnitId] = useState('');
    const [moduleId, setModuleId] = useState('');
    const [units, setUnit] = useState([]);
    const [modules, setModule] = useState([]);
    const [searchKey, setSearchKey] = useState('')
    const [screen, setScreen] = useState(0)
    useEffect(() => {

        initial()
    }, [])
    const testFun = () => {
        const fetchdata = async () => {
            const dt = await postData('Apartment/getlist',{
                "searchKey": ""
            });
            setApartment(dt);
        }
        fetchdata();
    }
    const initial = async () => {
        const dt = await postData('Apartment/getlist',{
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
    const fetchModule = async () => {
        const dt = await postData('Module/getbyunit', {
            "unitId": unitId
        });
        setModule(dt);
    }
    const onPressAddCancel = () => {
        setScreen(0);
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
        const rs = await postData('Module/addtoUnit', {
            "unitId": unitId,
            "moduleId": moduleId
        });
        if (rs != null && rs === true) {
            setScreen(0);
        }
        else {
            Alert.alert(
                'Lỗi', // Tiêu đề của thông báo
                'Không tồn tại module', // Nội dung của thông báo
                [
                    { text: 'Cancel', onPress: () => setScreen(0), style: 'cancel' },
                    { text: 'OK', onPress: () => setScreen(0) },
                ],
                { cancelable: false }
            );
        }
    }
    return (
        <BackgroundTop>

            <View style={{ flex: 1, position: 'relative' }}>
                {/* modal */}
                <View style={screen == 1 ? { position: 'absolute', top: 0, width: "100%", height: '100%', backgroundColor: 'rgba(0,0,0,0.6)', zIndex: 4, justifyContent: 'center', alignItems: 'center', padding: 10, } : { display: 'none' }}>
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
                        <TouchableOpacity onPress={() => navigation.navigate('Dashboard')}>
                            <Icon name="angle-double-left" size={30} color="#fff" />
                        </TouchableOpacity>
                    </View>
                    <View style={[styles.box, { width: '40%' }]}>
                        <Text style={{ fontWeight: '700', color: '#fff' }}> Quản lý Module</Text>
                    </View>
                    <View style={[styles.box, { width: '30%' }]}>
                        <TouchableOpacity onPress={() => setScreen(1)}>
                            <Icon name="plus" size={30} color="#fff" />
                        </TouchableOpacity>
                    </View>
                </View>
                <ScrollView>
                    <View style={{ justifyContent: 'center', alignItems: 'center' }}>
                        <View style={{ width: '80%' }}>
                            <TextInput
                                label="Tim kiem"
                                returnKeyType="next"
                                value={searchKey.value}
                                onChangeText={(text) => console.log('hi')}
                                autoCapitalize="none"
                            /></View>
                    </View>
                    <Button onPress={() => testFun()} title="test"></Button>
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
                                    aparments!=null && aparments.length>0?aparments.map((e,i) => {
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
                            {units != null && units.length > 0 ? units.map((e,i) => {
                                return (<Picker.Item key={i} label={e.name} value={e.id} />)
                            }) : <Picker.Item  label="None" value={0} />}
                        </Picker>
                    </View>
                    <Button onPress={() => fetchModule()} title="danh sách module"></Button>
                    <View style={{ padding: 10 }}>
                        {modules != null && modules.length > 0 ? modules.map((e,i) => {
                            return <View key={i} style={styles.item}>
                                <View style={{ position: 'relative' }}><Icon color='#fff' name='rocket' size={30}></Icon></View>
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
