import React, { useEffect, useState } from 'react'
import BackgroundTop from '../components/BackgroundTop'
import {
    View, StyleSheet, Text,
    TouchableOpacity, ScrollView, Button, Modal

} from 'react-native';
import { theme } from '../core/theme'
import Icon from 'react-native-vector-icons/FontAwesome';
import Icon2 from 'react-native-vector-icons/FontAwesome5';
import TextInput from '../components/TextInput';
import ButtonC from '../components/Button';
import { Picker } from '@react-native-picker/picker';
import CustomAlert from '../components/CustomAlert';
import { postData } from '../api/Api';


export default function UnitScreen({ navigation }) {
    const [data, setData] = useState([])
    const [name, setName] = useState('')
    const [desc, setDesc] = useState('')
    const [aparments, setApartment] = useState([]);
    const [aparmenId, setApartmentId] = useState('');
    const [units, setUnit] = useState('');
    const [unit, setUnitDto] = useState({});
    const [searchKey, setSearchKey] = useState('')
    const [screen, setScreen] = useState(0)
    const [sortScreen, setSortscreen] = useState(false)
    const [ordervalue, setOrdervalue] = useState(1)
    const [modalRepair, setmodalRepair] = useState(false)
    const [idUnitRemove, setIdUnitRemove] = useState('')
    const [alert, setAlert] = useState(false)
    useEffect(() => {

        initial()
    }, [])
    const fetchdata = async () => {
        const dt = await postData('Apartment/getlist', {

        });
        setApartment(dt);
    }
    const initial = async () => {
        const dt = await postData('Apartment/getlist', {

        });
        fetchUnit(dt[0].id);
        setApartmentId(dt[0].id)
        setApartment(dt);

    }
    const fetchUnit = async (apartmentId) => {
        const dt = await postData('Unit/getbyapartment', {
            "id": apartmentId,
            "searchKey": searchKey,
            "orderBy": ordervalue
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
    const onPressAdd = async () => {
        const rs = await postData('Unit/add', {
            "name": name,
            "apartmentId": aparmenId,
            "desc": desc
        });
        if (rs != null && rs === true) {
            setUnit(rs)
            fetchdata()
            fetchUnit(aparmenId)
            setScreen(0)
        }
    }
    const handleSort = async (sort) => {
        const dt = await postData('Unit/getbyapartment', {
            "id": aparmenId,
            "searchKey": searchKey,
            "orderBy": sort
        });
        setUnit(dt);
        setOrdervalue(sort)
        setSortscreen(false)
    }
    const handleChangeSearch = async (value) => {
        setSearchKey(value)
        const dt = await postData('Unit/getbyapartment', {
            "id": aparmenId,
            "searchKey": value,
            "orderBy": ordervalue
        });
        setUnit(dt);
    }
    const handleRepair = (e) => {
        setUnitDto(e)
        setName(e.name)
        setDesc(e.desc)
        setmodalRepair(true)
    }
    const onPressAddCancelRepair = () => {
        setmodalRepair(false)
    }
    const onPressAddRepair = async () => {
        const dt = await postData('Unit/update', {
            "id": unit.id,
            "name": name,
            "desc": desc
        });
        setName("")
        setUnit("")
        fetchUnit(aparmenId)
    }
    const handleRemove = (id) => {
        setIdUnitRemove(id)
        setAlert(true)
    }
    const onOkAlert = async () => {
        const dt = await postData('Unit/delete', {
            "id": idUnitRemove
           
        });
        setAlert(false)
        fetchUnit(aparmenId)
    }
    const onCloseAlert = () => {
        setAlert(false)
    }
    return (
        <BackgroundTop>

            <View style={{ flex: 1, position: 'relative' }}>
                {/* modal */}
                <View style={screen == 1 ? { position: 'absolute', top: 0, width: "100%", height: '100%', backgroundColor: 'rgba(0,0,0,0.6)', zIndex: 4, justifyContent: 'center', alignItems: 'center', padding: 10, } : { display: 'none' }}>
                    <View style={{ width: '80%', backgroundColor: '#fff', padding: 10, borderRadius: 10 }}>
                        <TextInput
                            label="Name"
                            returnKeyType="next"
                            value={name}
                            onChangeText={(text) => setName(text)}
                            autoCapitalize="none"
                        />

                        <TextInput
                            label="Desc"
                            returnKeyType="next"
                            value={desc}
                            onChangeText={(text) => setDesc(text)}
                            autoCapitalize="none"
                        />
                        <TextInput
                            label={aparmenId}
                            returnKeyType="next"
                            value={aparmenId}
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
                {/* modal repair */}
                <View style={modalRepair == true ? { position: 'absolute', top: 0, width: "100%", height: '100%', backgroundColor: 'rgba(0,0,0,0.6)', zIndex: 4, justifyContent: 'center', alignItems: 'center', padding: 10, } : { display: 'none' }}>
                    <View style={{ width: '80%', backgroundColor: '#fff', padding: 10, borderRadius: 10 }}>
                        <TextInput
                            label="Name"
                            returnKeyType="next"
                            value={name}
                            onChangeText={(text) => setName(text)}
                            autoCapitalize="none"
                        />

                        <TextInput
                            label="Desc"
                            returnKeyType="next"
                            value={desc}
                            onChangeText={(text) => setDesc(text)}
                            autoCapitalize="none"
                        />


                        <ButtonC onPress={onPressAddRepair} mode="contained" >
                            Submit
                        </ButtonC>
                        <ButtonC onPress={onPressAddCancelRepair} mode="contained" style={{ backgroundColor: '#ccc', color: '#000' }} >
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
                        <Text style={{ fontWeight: '700', color: '#fff' }}> Quản lý căn hộ</Text>
                    </View>
                    <View style={[styles.box, { width: '30%' }]}>
                        <TouchableOpacity onPress={() => setScreen(1)}>
                            <Icon name="plus" size={30} color="#fff" />
                        </TouchableOpacity>
                    </View>
                </View>
                {/* alert */}
                <CustomAlert onOk={onOkAlert} onClose={onCloseAlert} visible={alert} title="Xóa căn hộ" message="Xác nhận xóa"></CustomAlert>
                {/* searrch */}
                <View style={{ justifyContent: 'center', alignItems: 'center', flexDirection: 'row', padding: 10 }}>
                    <View style={{ width: '80%' }}>
                        <TextInput
                            label="Tim kiem"
                            value={searchKey}
                            onChangeText={(text) => handleChangeSearch(text)}
                            autoCapitalize="none"

                        />
                    </View>
                    <View style={{ width: '20%', justifyContent: 'center', alignItems: 'center', position: 'relative', zIndex: 10 }}>
                        <Icon2 onPress={() => { sortScreen == false ? setSortscreen(true) : setSortscreen(false) }} size={30} color={theme.colors.mainColor} name='sort'></Icon2>
                        {/* Sort modal */}
                        {/* <View style={sortScreen == false ? { display: 'none' } : { width: 100, height: 120, backgroundColor: '#fff', position: 'absolute',borderWidth:2, bottom: -120, right: 0, zIndex: 2, borderRadius: 10 }}>
                            <TouchableOpacity onPress={() => handleSort(1)} style={{borderBottomWidth:2, height: 30, justifyContent: 'center', alignItems:'center', padding: 5, }}>
                                <Text>Theo tên </Text>
                            </TouchableOpacity>
                            <TouchableOpacity onPress={() => handleSort(0)} style={{borderBottomWidth:2, height: 36, justifyContent: 'center', alignItems:'center', padding: 5, }} >
                                <Text>Theo ngày <Icon2 name="long-arrow-alt-up"></Icon2> </Text>
                            </TouchableOpacity>
                            <TouchableOpacity onPress={() => handleSort(2)} style={{borderBottomWidth:2, height: 36, justifyContent: 'center', alignItems:'center', padding: 5, }}>
                                <Text>Theo ngày <Icon2 name="long-arrow-alt-down"></Icon2> </Text>
                            </TouchableOpacity>
                        </View> */}
                        <Modal
                            animationType="slide"
                            transparent={true}
                            visible={sortScreen}
                            onRequestClose={() => {

                            }}
                        >
                            <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center', backgroundColor: 'rgba(0,0,0,0.5)' }}>
                                <View style={{ backgroundColor: 'white', padding: 20 }}>
                                    <TouchableOpacity onPress={() => handleSort(1)} style={{ borderBottomWidth: 2, height: 30, justifyContent: 'center', alignItems: 'center', padding: 5, }}>
                                        <Text>Theo tên </Text>
                                    </TouchableOpacity>
                                    <TouchableOpacity onPress={() => handleSort(0)} style={{ borderBottomWidth: 2, height: 36, justifyContent: 'center', alignItems: 'center', padding: 5, }} >
                                        <Text>Theo ngày <Icon2 name="long-arrow-alt-up"></Icon2> </Text>
                                    </TouchableOpacity>
                                    <TouchableOpacity onPress={() => handleSort(2)} style={{ borderBottomWidth: 2, height: 36, justifyContent: 'center', alignItems: 'center', padding: 5, }}>
                                        <Text>Theo ngày <Icon2 name="long-arrow-alt-down"></Icon2> </Text>
                                    </TouchableOpacity>
                                </View>
                            </View>
                        </Modal>
                    </View>
                </View>
                <ScrollView>
                    {/* picker */}
                    {aparments != null && aparments.length > 0 ?
                        <View style={{ marginLeft: 13, flexDirection: 'row', flexWrap: 'nowrap', width: '100%', height: 50, alignItems: 'center' }}>
                            <Text style={{ fontWeight: '500' }}>Chọn tòa: </Text>
                            <Picker
                                style={{ width: 150, height: 40 }}
                                selectedValue={aparmenId}
                                onValueChange={(itemValue) =>
                                    handlePickApartment(itemValue)
                                }>
                                {aparments.map((e, i) => {
                                    return (<Picker.Item key={i} label={e.name} value={e.id} />)
                                })}
                            </Picker>
                        </View>
                        : ""}

                    <View style={{ padding: 10 }}>
                        {units != null && units.length > 0 ? units.map((e, i) => {
                            return <View key={i} style={styles.item}>
                                <Icon2 onPress={() => handleRepair(e)} name='tools' size={20} color='blue' style={{ position: 'absolute', right: 5, top: 5 }}></Icon2>
                                <Icon2 onPress={() => handleRemove(e.id)} name='trash-alt' size={20} color='red' style={{ position: 'absolute', right: 35, top: 5 }}></Icon2>

                                <View style={{ position: 'relative' }}><Icon color='#fff' name='rocket' size={30}></Icon></View>
                                <View style={styles.itemleft}>
                                    <Icon name="home" size={80} color={theme.colors.mainColor} />
                                </View>
                                <View style={styles.itemright}>
                                    <View style={{ flexDirection: 'row', }}>
                                        <Text style={{ fontWeight: '500' }}>Tên căn: </Text>
                                        <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.name}</Text>
                                    </View>

                                    <View style={{ flexDirection: 'row' }}>
                                        <Text style={{ fontWeight: '500' }}>Mô tả: </Text>
                                        <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.desc}</Text>
                                    </View>

                                    <View style={{ flexDirection: 'row' }}>
                                        <Text style={{ fontWeight: '500' }}>Ngày tạo: </Text>
                                        <Text style={{ fontWeight: '500', opacity: 0.7 }}>{e.dateCreate}</Text>
                                    </View>
                                    <View style={{ flexDirection: 'row' }}>
                                        <Text onPress={() => navigation.navigate('UnitDetailScreen', { unit: e })} style={{ fontWeight: '500', color: 'blue', fontStyle: 'italic', textDecorationLine: 'underline' }}>Xem thông số</Text>

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
