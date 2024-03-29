import React, { useEffect, useState } from 'react'
import BackgroundTop from '../components/BackgroundTop'
import { View, StyleSheet, Text, TouchableOpacity, ScrollView, Button } from 'react-native';
import { theme } from '../core/theme'
import Icon from 'react-native-vector-icons/FontAwesome';
import { getData, postData } from '../api/Api';
import TextInput from '../components/TextInput';
import ButtonC from '../components/Button';



export default function ApartmentScreen({ navigation }) {
    const [data, setData] = useState([])
    const [name, setName] = useState('')
    const [desc, setDesc] = useState('')
    const [searchKey, setSearchKey] = useState('')
    const [screen, setScreen] = useState(0)
    useEffect(() => {
        getApartment();
    }, [])
    const testFun = () => {
        const getApartment = async () => {
            const dt = await getData('Apartment/getlist');
            setData(dt);
        }
        getApartment();
    }
    const getApartment = async () => {
        const dt = await getData('Apartment/getlist');
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

                    <View style={{ padding: 10 }}>
                        {data != null && data.length > 0 ? data.map(e => {
                            return <View style={styles.item}>
                                <View style={{ position: 'relative' }}><Icon color='#fff' name='rocket' size={30}></Icon></View>
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
