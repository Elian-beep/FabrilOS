<template>
  <v-app-bar title="Fabril OS" color="primary"></v-app-bar>

  <v-main class="bg-grey-lighten-5">
    <v-container class="py-6 page-container">
      <slot />
    </v-container>
  </v-main>
</template>

<style scoped>
.page-container {
  max-width: 1200px; 
}
</style>