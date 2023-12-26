<template>
  <b-navbar toggleable="lg" type="dark" variant="info" >
  <b-navbar-brand to="/" style="font-size: 32px; color: blueviolet;">Net.IT</b-navbar-brand>

  <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

  <b-collapse id="nav-collapse" is-nav>
    <b-navbar-nav class="ml-auto">
      <b-nav-item v-if="auth.isAuthenticated.value" @click="toProfile">{{ auth.username }}</b-nav-item>
      <b-nav-item v-if="auth.isAuthenticated.value" @click="logout">Logout</b-nav-item>
      <b-nav-item v-else>
        <router-link to="/login">Login</router-link>
        <router-link to="/register">Register</router-link>
      </b-nav-item>
      <router-link to="/dolbaebiki">Пользователи</router-link>
      <router-link to="/allChat" v-if="auth.isAuthenticated">Общий чат</router-link>
    </b-navbar-nav>
  </b-collapse>
</b-navbar>
</template>


<script setup lang="ts">
import { onMounted, ref } from 'vue';
import SessionService from '@/services/SessionService';
import { useRouter } from 'vue-router';
import auth from '@/state/auth';
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'

const router = useRouter();
const displayUsername = ref<string | null>(null);

const outputCookieAndUserData = () => {
  const token = SessionService.getToken() || null;
  const userData = SessionService.getUserData() || null;
  if (token && userData) {
    auth.isAuthenticated.value = true;
    auth.username = userData.login;
    displayUsername.value = userData.login;
  } else {
    auth.isAuthenticated.value = false;
    auth.username.value = null;
  }
};

const toProfile = () => {
  router.push(`/${auth.username}`)
};

const logout = () => {
  SessionService.removeToken();
  SessionService.removeUserData();
  outputCookieAndUserData();
  auth.isAuthenticated.value = false;
  auth.username.value = '';
  // Перенаправьте пользователя на страницу логина
  router.push('/login');
};

onMounted(() => {
  outputCookieAndUserData();
  console.log(auth.isAuthenticated)
});
</script>

<style scoped>
/* Добавьте дополнительные стили, если необходимо */
</style>
