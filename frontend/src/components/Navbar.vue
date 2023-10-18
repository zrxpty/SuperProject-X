  <template>
    <nav class="navbar">
      <div class="navbar-menu">
        <div class="navbar-start">
          <a v-if="auth.isAuthenticated.value" @click="toProfile" class="navbar-item">{{ auth.username }}</a>
          <a v-if="auth.isAuthenticated.value" @click="logout" class="navbar-item">Logout</a>
          <template v-else>
            <router-link to="/login" class="navbar-item">Login</router-link>
            <router-link to="/register" class="navbar-item">Register</router-link>
          </template>
          <router-link to="/dolbaebiki" class="navbar-item">Dolbaebiki</router-link>
        </div>
      </div>
    </nav>
  </template>

  <script setup lang="ts">
  import { onMounted, ref } from 'vue';
  import SessionService from '@/services/SessionService';
  import { useRouter } from 'vue-router';
  import auth from '@/state/auth';

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
  }
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
  .navbar {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    padding: 1rem;
    z-index: 1000; /* Устанавливаем z-index, чтобы компонент Navbar был поверх других элементов */
  }

  .navbar-menu {
    display: flex;
  }

  .navbar-item {
    color: white;
    margin-right: 1rem;
    text-decoration: none;
  }

  .navbar-item:hover {
    text-decoration: underline;
  }

  /* Добавьте остальные стили, если необходимо */
  </style>
