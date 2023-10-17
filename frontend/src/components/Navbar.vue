<template>
  <nav class="navbar">
    <div class="navbar-menu">
      <div class="navbar-start">
        <router-link v-if="auth.isAuthenticated.value" :to="`/${auth.username}`" class="navbar-item">{{ auth.username }}</router-link>
        <a v-if="auth.isAuthenticated.value" @click="logout" class="navbar-item">Logout</a>
        <template v-else>
          <router-link to="/login" class="navbar-item">Login</router-link>
          <router-link to="/register" class="navbar-item">Register</router-link>
        </template>
      </div>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import CookieService from '../services/CookieService.ts';
import { useRouter } from 'vue-router';
import auth from '../state/auth';

const router = useRouter();

const outputCookieAndUserData = () => {
  const token = CookieService.getToken() || null;
  const userData = CookieService.getUserData() || null;
  if (token && userData) {
    auth.isAuthenticated.value = true;
    auth.username = userData.login;
  } else {
    auth.isAuthenticated.value = false;
    auth.username.value = null;
  }
};

const logout = () => {
  CookieService.removeToken();
  CookieService.removeUserData();
  outputCookieAndUserData();
  auth.isAuthenticated.value = false;
  auth.username.value = '';
  // Перенаправьте пользователя на страницу логина
  router.push('/login');
};

onMounted(() => {
  outputCookieAndUserData();
});
</script>

<style scoped>
/* Добавьте стили для вашего компонента навигационной панели, если необходимо */
.navbar {
  padding: 1rem;
}

.navbar-menu {
  display: flex;
  justify-content: flex-end;
}

.navbar-item {
  color: white;
  margin-right: 1rem;
  text-decoration: none;
}

.navbar-item:hover {
  text-decoration: underline;
}
</style>
