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


  <div>
  <b-navbar toggleable="lg" type="dark" variant="info">
    <b-navbar-brand href="#">NavBar</b-navbar-brand>

    <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

    <b-collapse id="nav-collapse" is-nav>
      <b-navbar-nav>
        <b-nav-item href="#">Link</b-nav-item>
        <b-nav-item href="#" disabled>Disabled</b-nav-item>
      </b-navbar-nav>

      <!-- Right aligned nav items -->
      <b-navbar-nav class="ml-auto">
        <b-nav-form>
          <b-form-input size="sm" class="mr-sm-2" placeholder="Search"></b-form-input>
          <b-button size="sm" class="my-2 my-sm-0" type="submit">Search</b-button>
        </b-nav-form>

        <b-nav-item-dropdown text="Lang" right>
          <b-dropdown-item href="#">EN</b-dropdown-item>
          <b-dropdown-item href="#">ES</b-dropdown-item>
          <b-dropdown-item href="#">RU</b-dropdown-item>
          <b-dropdown-item href="#">FA</b-dropdown-item>
        </b-nav-item-dropdown>

        <b-nav-item-dropdown right>
          <!-- Using 'button-content' slot -->
          <template #button-content>
            <em>User</em>
          </template>
          <b-dropdown-item href="#">Profile</b-dropdown-item>
          <b-dropdown-item href="#">Sign Out</b-dropdown-item>
        </b-nav-item-dropdown>
      </b-navbar-nav>
    </b-collapse>
  </b-navbar>
</div>
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
