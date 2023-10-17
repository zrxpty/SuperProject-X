<template>
  <div class="login">
    <div class="form-group">
      <label for="username">Имя пользователя:</label>
      <input v-model="username" type="text" id="username" class="form-control" />
    </div>
    <div class="form-group">
      <label for="password">Пароль:</label>
      <input v-model="password" type="password" id="password" class="form-control" />
    </div>
    <button @click="login">Войти</button>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { AuthResponseModel } from '../../models/account/AuthResponseModel';
import CookieService from '../../services/CookieService.ts';
import auth from '../../state/auth';

const username = ref<string>('');
const password = ref<string>('');
const baseURL = import.meta.env.VITE_BASE_URL;

const login = async () => {
  try {
    const response = await fetch(`${baseURL}/api/Identity/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        login: username.value,
        password: password.value,
      }),
    });

    if (response.ok) {
      // Получите ответ и преобразуйте его в объект AuthResponseModel
      const responseBody: AuthResponseModel = await response.json();
      if (responseBody.code !== 401) {

        CookieService.saveToken(responseBody.data.token);
        CookieService.saveUserData({
          email: responseBody.data.email,
          login: responseBody.data.login,
          role: responseBody.data.role,
        });

        console.log('Токен:', CookieService.getToken());
        console.log('Данные пользователя:', CookieService.getUserData());

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
.login {
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
