import React, { useEffect, useState } from 'react'
import BackgroundTop from '../components/BackgroundTop'
import {
    View, StyleSheet, Text, TouchableOpacity, ScrollView, Button
    , Modal, Alert
} from 'react-native';
import { theme } from '../core/theme'
import Icon from 'react-native-vector-icons/FontAwesome';
import Icon2 from 'react-native-vector-icons/FontAwesome5';
import { getData, postData } from '../api/Api';
import TextInput from '../components/TextInput';
import ButtonC from '../components/Button';
import CustomAlert from '../components/CustomAlert';



export default function ApartmentScreen({ navigation }) {
    const [data, setData] = useState([])
    const [name, setName] = useState('')
    const [desc, setDesc] = useState('')
    const [searchKey, setSearchKey] = useState('')
    const [screen, setScreen] = useState(0)
    const [sortScreen, setSortscreen] = useState(false)
    const [ordervalue, setOrdervalue] = useState(1)
    const [isModalVisible, setModalVisible] = useState(false);
    const [modalRepair, setModaRepair] = useState(false);
    const [apartment, setApartment] = useState({});
    const [alert, setAlert] = useState(false);
    const [idApartmentRemove, setIdApartmentRemove] = useState('');


    useEffect(() => {
        getApartment();
    }, [])
    const testFun = () => {

    }
    const getApartment = async () => {
        const dt = await postData('Apartment/getlist', {
            "searchKey": searchKey,
            "orderBy": ordervalue
        });
        setData(dt);
    }
    const onPressAddCancel = () => {
        setScreen(0);
    }
    const onPressAdd = async () => {
        // add apartment
        const rs = await postData('Apartment/add', {
            "name": name,
            "desc": desc
        });
        if (rs != null && rs === true) {
            console.log(rs);
            getApartment()
            setScreen(0)
        }
    }
    const handleChangeSearch = async (text) => {
        setSearchKey(text)
        if (text.length >= 1) {
            const dt = await postData(`Apartment/getlist`, {
                "searchKey": text,
                "orderBy": ordervalue
            });
            setData(dt);
        }
        if (text.length == 0) {
            const dt = await postData(`Apartment/getlist`, {
                "searchKey": "",
                "orderBy": ordervalue

            });
            setData(dt);
        }
    }
    const handleSort = async (sort) => {
        const dt = await postData('Apartment/getlist', {
            "searchKey": searchKey,
            "orderBy": sort
        });
        setData(dt)
        setSortscreen(false)
    }
    const onPressAddCancelRepair = () => {
        setModaRepair(false)
    }
    const updateApartment = async () => {
        const rs = await postData('Apartment/update', {
            "id": apartment.id,
            "name": name,
            "desc": desc
        })
        console.log(rs);
        if (rs === true) {
            setName("");
            setDesc("");
            setModaRepair(false)
        }
        getApartment()
    }
    const onPressAddRepair = async () => {
        updateApartment();
    }
    const handleRepair = (e) => {
        setModaRepair(true);
        setName(e.name);
        setDesc(e.desc)
        setApartment(e);
    }
    const handleRemove = (id) => {
        setIdApartmentRemove(id)
       setAlert(true)
    }
    const onOkAlert = async ()=> {
        const rs = await postData('Apartment/delete', {
            "id": idApartmentRemove
        })
        setAlert(false)
        getApartment()
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
                {/* alert */}
                <CustomAlert onOk={onOkAlert} onClose={onCloseAlert} visible={alert} title="Xóa tòa nhà" message="Xác nhận xóa"></CustomAlert>
                {/* header */}
                <View style={styles.header}>
                    <View style={[styles.box, { width: '30%', }]}>
                        <TouchableOpacity onPress={() => navigation.navigate('Dashboard')}>
                            <Icon name="angle-double-left" size={30} color="#fff" />
                        </TouchableOpacity>
                    </View>
                    <View style={[styles.box, { width: '40%' }]}>
                        <Text style={{ fontWeight: '700', color: '#fff' }}> Quản lý tòa nhà</Text>
                    </View>
                    <View style={[styles.box, { width: '30%' }]}>
                        <TouchableOpacity onPress={() => setScreen(1)}>
                            <Icon name="plus" size={30} color="#fff" />
                        </TouchableOpacity>
                    </View>
                </View>
                {/* searchKey */}
                <View style={{ justifyContent: 'center', alignItems: 'center', flexDirection: 'row', padding: 10 }}>
                    <View style={{ width: '80%' }}>
                        <TextInput
                            label="Tim kiem"
                            value={searchKey}
                            onChangeText={(text) => handleChangeSearch(text)}
                            autoCapitalize="none"

                        />
                    </View>
                    <View style={{ width: '20%', justifyContent: 'center', alignItems: 'center', position: 'relative', zIndex: 2 }}>
                        <Icon2 onPress={() => { sortScreen == false ? setSortscreen(true) : setSortscreen(false) }} size={30} color={theme.colors.mainColor} name='sort'></Icon2>
                        {/* Sort modal */}
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
                <ScrollView style={{ height: 500 }} >

                    <View style={{ padding: 10 }}>
                        {data != null && data.length > 0 ? data.map((e, i) => {
                            return <View key={i} style={styles.item}>
                                <Icon2 onPress={() => handleRepair(e)} name='tools' size={20} color='blue' style={{ position: 'absolute', right: 5, top: 5 }}></Icon2>
                                <Icon2 onPress={() => handleRemove(e.id)} name='trash-alt' size={20} color='red' style={{ position: 'absolute', right: 35, top: 5 }}></Icon2>
                                <View style={styles.itemleft}>
                                    <Icon name="building" size={80} color={theme.colors.mainColor} />
                                </View>
                                <View style={styles.itemright}>
                                    <View style={{ flexDirection: 'row', }}>
                                        <Text style={{ fontWeight: '500' }}>Tên tòa: </Text>
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
        position: 'relative',
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
    modalContainer: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: 'rgba(0, 0, 0, 0.5)',
    },
    alertContainer: {
        backgroundColor: 'white',
        padding: 20,
        borderRadius: 10,
        elevation: 5,
    },
    alertTitle: {
        fontSize: 18,
        fontWeight: 'bold',
        marginBottom: 10,
    },
    alertMessage: {
        fontSize: 16,
        marginBottom: 20,
    },
    okButton: {
        backgroundColor: 'lightblue',
        paddingVertical: 10,
        paddingHorizontal: 20,
        borderRadius: 5,
        alignSelf: 'flex-end',
    },
    okButtonText: {
        fontSize: 16,
        fontWeight: 'bold',
    },

})
