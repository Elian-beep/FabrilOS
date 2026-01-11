<script setup lang="ts">
import { useRoute } from 'vue-router';
import DefaultLayout from './layouts/DefaultLayout.vue';
import AuthLayout from './layouts/AuthLayout.vue';
import { computed } from 'vue';

const route = useRoute();

const layouts = {
default: DefaultLayout ,
auth: AuthLayout,
};

const currentLayout = computed(() => {
  const layoutName = (route.meta.layout as string) || 'default';
  return layouts[layoutName as keyof typeof layouts];
});
</script>

<template>
  <v-app>
    <component :is="currentLayout">
      <router-view />
    </component>
  </v-app>
</template>

<style scoped></style>
