import React, { useState ,useEffect} from 'react'
import BackgroundTop from '../components/BackgroundTop'
import { View, Text, Button, TouchableOpacity, SafeAreaView, StyleSheet, Image, ScrollView } from 'react-native'
import HeaderTopD from '../components/HeaderTopD'
import Icon from 'react-native-vector-icons/FontAwesome';
import { theme } from '../core/theme'
import ButtonC from '../components/Button'
import { useNavigate } from 'react-router-native';

const funcitions = [
  {
    id: 1,
    title: "Quản lý tòa nhà",
    icon: "building"

  },
  {
    id: 2,
    title: "Quản lý căn hộ",
    icon: "home"
  },
  {
    id: 3,
    title: "Quản lý module",
    icon: "microchip"
  },
  {
    id: 4,
    title: "Quản lý tự động",
    icon: "cubes"
  },
]

export default function Dashboard() {
  const navigate = useNavigate();

  const [screen, setSceen] = useState(1)
  const [test, setTest] = useState('')
  const handleSwtich = (id) => {
    if (id == 1) {
      navigate('/ApartmentScreen');
    }
    else if(id ==2) {
      navigate('/UnitScreen');
    }
    else if(id ==3) {
      navigate('/ModuleScreen');
    }
    else if(id ==4) {
      navigate('/AutoScreen');
    }
  }
  useEffect(() => {
  setSceen(1)
  }, [])
  return (
    <BackgroundTop>
      <HeaderTopD text={'Quản lý cháy'}></HeaderTopD>
      <ScrollView style = {screen==1?{marginBottom:111}:{display:'none'}}>
        <View style={styles.profile}>
          <Image style={{ width: 60, height: 60, borderRadius: 50, marginRight: 10 }} source={require('../assets/logo2.jpg')}></Image>
          <Text style={{ fontWeight: '700' }}>Hubert</Text>
        </View>
        <View style={styles.content}>
          {funcitions.map((e,i) => {
            return (<TouchableOpacity key={i} onPress={() => handleSwtich(e.id)} style={{ justifyContent: 'center', alignItems: 'center', marginBottom: 10 }}>
              <View style={styles.item}>
                <Icon name={e.icon} size={96} color={theme.colors.mainColor} />

              </View>

              <Text style={{ fontWeight: '700', justifyContent: 'center', alignItems: 'center', alignContent: 'center' }}>{e.title}</Text>
            </TouchableOpacity>)
          })}
        </View>
        <Text>{test}</Text>
      </ScrollView>
      <ScrollView style= {screen==2?{marginBottom:111}:{display:'none'}}>
      <ButtonC mode="contained" onPress={()=> navigate('/login')}>
        Đăng xuất
      </ButtonC>
      </ScrollView>
      <View style={{ width: '100%', justifyContent: 'center', alignItems: 'center', padding: 5, position:'absolute', bottom:0 }}>
        <View style={styles.footer}>
          <TouchableOpacity onPress={() => setSceen(1)} style={styles.fItem}>
            <Icon name="home" size={50} color={screen == 1 ? theme.colors.mainColor : "#ccc"} />
            <Text>Trang chủ</Text>
          </TouchableOpacity>
          <TouchableOpacity onPress={() => setSceen(2)} style={styles.fItem}>
            <Icon name="user" size={50} color={screen == 2 ? theme.colors.mainColor : "#ccc"} />
            <Text>Người dùng</Text>
          </TouchableOpacity>
        </View>
      </View>
    </BackgroundTop>
  )
}


const styles = StyleSheet.create({
  profile: {
    width: 390,
    height: 114,
    flexDirection: 'row',
    alignItems: 'center',
    padding: 10
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
  content: {
    padding: 20,
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'center'
  },
  item: {
    margin: 5,
    backgroundColor: '#fff',
    width: 120,
    height: 130,
    justifyContent: 'center',
    alignItems: 'center',
    borderRadius: 30,
    shadowColor: '#000',
    shadowOffset: {
      width: 0,
      height: 2,
    },
    shadowOpacity: 0.5,
    shadowRadius: 3.84,
    elevation: 5, // Độ cao của đổ bóng (chỉ áp dụng trên Android)

  }
})