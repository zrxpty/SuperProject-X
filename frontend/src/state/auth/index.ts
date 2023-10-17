import { ref, Ref } from 'vue';

const isAuthenticated = ref(false);
const username: Ref<string | null> = ref(null);

export default {
  isAuthenticated,
  username,
};