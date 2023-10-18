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
  Подключите компонент к вашему маршрутизатору:
  vue
  Copy code
  // В файле, где у вас настроен маршрутизатор (например, router/index.ts)
  import { createRouter, createWebHistory } from 'vue-router';
  import UserList from '@/views/UserList.vue'; // Путь к вашему компоненту UserList.vue
  
  const routes = [
    {
      path: '/users',
      name: 'UserList',
      component: UserList,
    },
    // Добавьте другие маршруты, если это необходимо
  ];
  
  const router = createRouter({
    history: createWebHistory(),
    routes,
  });
  
  export default router;
  В этом примере компонент UserList получает данные о пользователях с вашего бэкенда после монтирования компонента (onMounted), используя функцию fetch. Затем он отображает список пользователей в пользовательском интерфейсе с использованием директивы v-for.
  
  Не забудьте настроить путь к вашему бэкенду в URL-адресе запроса (`'ссылка
  
  
  