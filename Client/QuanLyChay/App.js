import React from 'react'
import { View, StyleSheet } from 'react-native';
import { NativeRouter, Routes, Route } from 'react-router-native';
import StartScreen from './src/screens/StartScreen';
import LoginScreen from './src/screens/LoginScreen';
import RegisterScreen from './src/screens/RegisterScreen';
import ResetPasswordScreen from './src/screens/ResetPasswordScreen';
import Dashboard from './src/screens/Dashboard';
import ApartmentScreen from './src/screens/ApartmentScreen';
import AutoScreen from './src/screens/AutoScreen';
import ModuleScreen from './src/screens/ModuleScreen';
import UnitDetailScreen from './src/screens/UnitDetailScreen';
import UnitScreen from './src/screens/UnitScreen';
import Home from './src/screens/Admin/Home'
import Module from './src/screens/Admin/Module';
// import HistoryScreen from './src/screens/HistoryScreen';
export default function App() {
  return (
    <View style = {styles.container}>
      <NativeRouter>
        <Routes>
          <Route path="/" element={<StartScreen />} />
          <Route path="/login" element={<LoginScreen />} />
          <Route path="/RegisterScreen" element={<RegisterScreen />} />
          <Route path="/ResetPasswordScreen" element={<ResetPasswordScreen />} />
          <Route path="/ApartmentScreen" element={<ApartmentScreen />} />
          <Route path="/AutoScreen" element={<AutoScreen />} />
          <Route path="/ModuleScreen" element={<ModuleScreen />} />
          <Route path="/UnitDetailScreen" element={<UnitDetailScreen />} />
          <Route path="/UnitScreen" element={<UnitScreen />} />
          <Route path="/Dashboard" element={<Dashboard />} />
          <Route path="/Home" element={<Home />} />
          <Route path="/Module" element={<Module />} />
          {/* <Route path="/" element={<HistoryScreen />} /> */}
          {/* <Route path="/HistoryScreen" element={<HistoryScreen />} /> */}
        </Routes>
      </NativeRouter>
    </View>

  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
});