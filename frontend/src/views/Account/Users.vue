<template>
    <div>
      <h1>Список пользователей</h1>
      <ul>
        <li v-for="user in users" :key="user.login">
          {{ user.login }}
        </li>
      </ul>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref, onMounted } from 'vue';
  interface User {
  login: string;
}

const baseURL = import.meta.env.VITE_BASE_URL;
const users = ref<User[]>([]);
  
  onMounted(async () => {
    try {
      const response = await fetch(`${baseURL}/api/User/getUsers`);
      if (response.ok) {
        const data = await response.json();
        users.value = data.data;
      } else {
        console.error('Ошибка при получении данных с сервера');
      }
    } catch (error) {
      console.error('Ошибка при выполнении запроса:', error);
    }
  });
  </script>
  
  <style scoped>
  /* Стилизуйте ваш компонент здесь, если это необходимо */
  </style>
 
  
  
  