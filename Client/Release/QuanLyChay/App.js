import React from 'react'
import { Provider } from 'react-native-paper'
import { NavigationContainer } from '@react-navigation/native'
import { createStackNavigator } from '@react-navigation/stack'
import { theme } from './src/core/theme'
import {
  StartScreen,
  LoginScreen,
  RegisterScreen,
  ResetPasswordScreen,
  Dashboard,
  UnitScreen,
  ModuleScreen,
  UnitDetailScreen,
  AutoScreen,
} from './src/screens'
import ApartmentScreen from './src/screens/ApartmentScreen'

const Stack = createStackNavigator()

export default function App() {
  return (
    <Provider theme={theme}>
      <NavigationContainer>
        <Stack.Navigator
          initialRouteName="StartScreen"
          screenOptions={{
            headerShown: false,
          }}
        >
          <Stack.Screen name="StartScreen" component={StartScreen} />
          <Stack.Screen name="LoginScreen" component={LoginScreen} />
          <Stack.Screen name="RegisterScreen" component={RegisterScreen} />
          <Stack.Screen name="Dashboard" component={Dashboard} />
          <Stack.Screen name="ApartmentScreen" component={ApartmentScreen} />
          <Stack.Screen name="UnitScreen" component={UnitScreen} />
          <Stack.Screen name="ModuleScreen" component={ModuleScreen} />
          <Stack.Screen name="UnitDetailScreen" component={UnitDetailScreen} />
          <Stack.Screen name="AutoScreen" component={AutoScreen} />
          <Stack.Screen
            name="ResetPasswordScreen"
            component={ResetPasswordScreen}
          />
        </Stack.Navigator>
      </NavigationContainer>
    </Provider>
  )
}
