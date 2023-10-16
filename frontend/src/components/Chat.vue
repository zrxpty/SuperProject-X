<template>
  <div>
    <input v-model="tokenInput" placeholder="Введите токен..." />
    <button @click="connectToSignalR" :disabled="isConnected">Подключиться к SignalR</button>
    <input v-model="messageInput" placeholder="Введите сообщение..." :disabled="!isConnected" />
    <button @click="sendMessage" :disabled="!isConnected || !messageInput">Отправить</button>
    <div v-for="(message, index) in messages" :key="index">
      <p>Пользователь: {{ message.user }}</p>
      <p>Сообщение: {{ message.text }}</p>
      <p>Статус: {{ message.status }}</p>
      <hr />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onBeforeUnmount } from 'vue';
import * as signalR from '@microsoft/signalr';

const tokenInput = ref('');
const messageInput = ref('');
const messages = ref([]);
let connection: signalR.HubConnection | null = null;
const isConnected = ref(false);

const connectToSignalR = () => {
  if (tokenInput.value.trim() === '') {
    console.warn('Введите токен для подключения к SignalR.');
    return;
  }

  connection = new signalR.HubConnectionBuilder()
    .withUrl(`http://localhost:5002/hello?access_token=${tokenInput.value}`)
    .build();

  connection.start()
    .then(() => {
      console.log('Соединение с SignalR установлено');
      isConnected.value = true;

      connection.on('ReceiveMessage', (message) => {
        messages.value.push(message);
      });
    })
    .catch(error => {
      console.error('Ошибка подключения к SignalR:', error);
    });
};

const sendMessage = () => {
  if (!tokenInput.value || !messageInput.value) {
    console.warn('Введите токен и сообщение для отправки.');
    return;
  }

  const newMessage = {
    user: tokenInput.value,
    text: messageInput.value,
    status: 'Sending...'
  };
  messages.value.push(newMessage);

  connection.send('SendMessage', messageInput.value)
    .then(() => {
      newMessage.status = 'Ok';
      messageInput.value = ''; 
    })
    .catch(error => {
      console.error('Ошибка отправки сообщения:', error);
      newMessage.status = 'Error';
    });
};

onBeforeUnmount(() => {
  if (connection && connection.state === signalR.HubConnectionState.Connected) {
    connection.stop();
  }
});
</script>
