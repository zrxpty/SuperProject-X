<template>
  <div style="display: fle;">
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
    <div class="container-fluid">
      <router-link to="/" class="navbar-brand" style="font-size: 32px; color: blueviolet;">Social Network</router-link>
      <div class="collapse navbar-collapse" id="navbarNav">
        <div class="navbar-brand">
          <a v-if="auth.isAuthenticated.value" @click="toProfile" class="nav-link" style="margin-right: 10px;">{{ auth.username }}</a>
          <a v-if="auth.isAuthenticated.value" @click="logout" class="nav-link" style="margin-right: 10px;">Logout</a>
          <template v-else>
            <router-link to="/login" class="nav-link" style="margin-right: 10px;">Login</router-link>
            <router-link to="/register" class="nav-link" style="margin-right: 10px;">Register</router-link>
          </template>
          <router-link to="/dolbaebiki" class="nav-link" style="margin-right: 10px;">Пользователи</router-link>
        </div>
      </div>
    </div>
  </nav>
  </div>
</template>


<script setup lang="ts">
import { onMounted, ref } from 'vue';
import SessionService from '@/services/SessionService';
import { useRouter } from 'vue-router';
import auth from '@/state/auth';
import { CAlert } from '@coreui/bootstrap-vue';
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
