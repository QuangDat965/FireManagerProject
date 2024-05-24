import React from 'react'
import Background from '../components/Background'
import Logo from '../components/Logo'
import Header from '../components/Header'
import Button from '../components/Button'
import Paragraph from '../components/Paragraph'
import { useNavigate } from 'react-router-native';

export default function StartScreen() {
  const navigate = useNavigate();
  return (
    <Background>
      <Logo />
      <Header>Xin Chào</Header>
      <Paragraph>
        Quản lý phòng cháy
      </Paragraph>
      <Button
        mode="contained"
        onPress={() => navigate('Login')}
      >
        Đăng Nhập
      </Button>
      <Button
        mode="outlined"
        onPress={() => navigate('RegisterScreen')}
      >
        Đăng Kí
      </Button>
    </Background>
  )
}
