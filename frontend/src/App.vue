<template>
  <div>
    <Navbar />
  </div>
  <div>
    <RouterView />
  </div>
</template>

<style scoped>
</style>
<script setup lang="ts">
import { ref, onMounted } from 'vue';
import * as signalR from '@microsoft/signalr';
import Navbar from '@/components/Navbar.vue';
import SessionService from '@/services/SessionService';

let connectionChat: signalR.HubConnection | null = null;
let connectionNotificate: signalR.HubConnection | null = null;
const authService = new SessionService(); // Пример создания экземпляра authService

onMounted(() => {
  initializeSignalR();
});

async function initializeSignalR() {
  const token = await authService.getAuthInfo();
  connectionChat = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7109/chat', {
        accessTokenFactory: async () => {
          return token!.token;
        }
      })
      .build();

  connectionChat.start()
      .catch(err => console.error(err));

  connectionChat.on('feedback', (message, user) => {
    console.log('Получено сообщение:', JSON.stringify(message));
  });

  connectionNotificate = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7109/notification', {
        accessTokenFactory: async () => {
          return token!.token;
        }
      })
      .build();

  connectionNotificate.start()
      .catch(err => console.error(err));

  connectionNotificate.on('ReceiveNotification', (message) => {
    console.log(JSON.stringify(message));
  });
}
</script>
