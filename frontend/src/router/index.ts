// @ts-ignore
import { createRouter, createWebHistory } from 'vue-router';
import Login from '@/views/Account/Login.vue';
import Register from '@/views/Account/Register.vue';
import Users from '@/views/Account/Users.vue';
import Profile from '@/views/Account/Profile.vue';

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
  },
  {
    path: '/dolbaebiki',
    name: 'users',
    component: Users
  },
  {
    path: '/:id',
    name: 'profile',
    component: Profile
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
