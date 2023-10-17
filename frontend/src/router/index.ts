import { createRouter, createWebHistory } from 'vue-router';
import Login from '../views/Account/Login.vue';
import Register from '../views/Account/Register.vue';

const routes = [
  {
    path: '/login',
    name: 'login',
    component: Login,
  }, 
  {
    path: '/register',
    name: 'register',
    component: Register
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
