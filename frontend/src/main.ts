import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import { QueryClient, VueQueryPlugin } from '@tanstack/vue-query';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
      staleTime: 1000 * 60 * 5,
      retry: 2,
    },
  },
});

const app = createApp(App);

app.use(createPinia());
app.use(VueQueryPlugin, { queryClient });

app.mount('#app');
