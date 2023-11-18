import { createApp } from 'vue'
import '@/style.css'
import App from '@/App.vue'
import router from '@/router'
import BootstrapVue3 from 'bootstrap-vue-3'
import Vue from 'vue'

createApp(App).use(BootstrapVue3).use(router).mount('#app')
