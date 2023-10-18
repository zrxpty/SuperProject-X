<template>
    <div class="register">
      <h1>Register</h1>
      <div class="form-group">
        <label>Email:</label>
        <input v-model="email" type="email" class="form-control" required>
      </div>
      <div class="form-group">
        <label>Login:</label>
        <input v-model="login" type="text" class="form-control" required>
      </div>
      <div class="form-group">
        <label>Password:</label>
        <input v-model="password" type="password" class="form-control" required>
      </div>
      <button @click="register">Register</button>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue';
  import { AuthResponseModel } from '@/models/account/AuthResponseModel';
  import SessionService from '@/services/SessionService';
  import auth from '@/state/auth';
  
  const email = ref('');
  const login = ref('');
  const password = ref('');
  
  const baseURL = import.meta.env.VITE_BASE_URL;
  
  const register = async () => {
    try {
      const response = await fetch(`${baseURL}/api/Identity/register`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          email: email.value,
          login: login.value,
          password: password.value,
        }),
      });
  
      if (response.ok) {
      // Получите ответ и преобразуйте его в объект AuthResponseModel
      const responseBody: AuthResponseModel = await response.json();
      if (responseBody.code !== 401) {

        SessionService.saveToken(responseBody.data.token);
        SessionService.saveUserData({
          email: responseBody.data.email,
          login: responseBody.data.login,
          role: responseBody.data.role,
        });

        console.log('Токен:', SessionService.getToken());
        console.log('Данные пользователя:', SessionService.getUserData());

        auth.isAuthenticated.value = true;
        auth.username.value = responseBody.data.login;
      } else {
        alert(responseBody.message);
      }
    }
    } catch (error) {
      // Обработка сетевых ошибок
      console.error('Ошибка сети:', error);
    }
  };
  </script>
  
  <style scoped>
  .register {
    max-width: 300px;
    margin: 0 auto;
    padding: 20px;
    border: 1px solid #ccc;
    border-radius: 5px;
  }
  
  .form-group {
    margin-bottom: 10px;
  }
  
  button {
    cursor: pointer;
  }
  </style>
  @/services/SessionService