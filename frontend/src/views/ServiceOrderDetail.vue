<script setup lang="ts">
import { useRoute, useRouter } from 'vue-router';
import { useQuery } from '@tanstack/vue-query';
import { ordersService } from '@/api/modules/ordersService';

const route = useRoute();
const router = useRouter();

const orderId = Number(route.params.id);

const { data: order, isLoading, isError } = useQuery({
  queryKey: ['service-order', orderId],
  queryFn: () => ordersService.getById(orderId),
  enabled: !!orderId,
  staleTime: 0,
});

const formatDate = (dateString?: string) => {
  if (!dateString) return '-';
  return new Date(dateString).toLocaleDateString('pt-BR', {
    day: '2-digit', month: '2-digit', year: 'numeric',
    hour: '2-digit', minute: '2-digit'
  });
};
</script>

<template>
  <v-container>
    <v-btn 
      variant="text" 
      prepend-icon="mdi-arrow-left" 
      class="mb-4"
      @click="router.back()"
    >
      Voltar
    </v-btn>

    <div v-if="isLoading" class="d-flex justify-center pa-8">
      <v-progress-circular indeterminate color="primary"></v-progress-circular>
    </div>

    <v-alert
      v-else-if="isError || !order"
      type="error"
      title="OS não encontrada"
      text="Não foi possível carregar os detalhes desta ordem de serviço."
    ></v-alert>

    <v-card v-else class="rounded-lg pa-6" elevation="2">
      
      <div class="d-flex justify-space-between align-start mb-6">
        <div>
          <div class="text-overline text-grey-darken-1">Ordem de Serviço N° {{ order.id }}</div>
          <h1 class="text-h5 font-weight-bold text-primary">{{ order.title }}</h1>
          <div class="text-caption text-grey mt-1">
            Criado em: {{ formatDate(order.createdAt) }} 
            <span v-if="order.userName"> - Por: {{ order.userName }}</span>
          </div>
        </div>
      </div>

      <v-divider class="mb-6"></v-divider>

      <div class="mb-6">
        <label class="text-subtitle-2 font-weight-bold text-grey-darken-2">Descrição do Problema</label>
        <div class="text-body-1 mt-2 bg-grey-lighten-5 pa-4 rounded border-thin">
          {{ order.description }}
        </div>
      </div>

      <div v-if="order.imageUrls && order.imageUrls.length > 0" class="mb-6">
        <label class="text-subtitle-2 font-weight-bold text-grey-darken-2 mb-2 d-block">
          Foto do Equipamento
        </label>
        <v-card class="d-inline-block rounded-lg overflow-hidden border">
          <v-img
            :src="order.imageUrls[0]" 
            width="100%"
            max-width="400"
            max-height="300"
            cover
            class="bg-grey-lighten-2"
          >
            <template #placeholder>
              <div class="d-flex align-center justify-center fill-height">
                <v-progress-circular indeterminate color="grey-lighten-4"></v-progress-circular>
              </div>
            </template>
          </v-img>
        </v-card>
      </div>

      <div>
        <label class="text-subtitle-2 font-weight-bold text-grey-darken-2 mb-2 d-block">
          Checklist de Serviços
        </label>
        
        <div class="border rounded pa-4">
          <v-row dense>
            <v-col 
              v-for="item in order.checklists" 
              :key="item.id" 
              cols="12" 
              md="6"
            >
              <v-checkbox
                :model-value="item.isChecked"
                :label="item.label"
                density="compact"
                hide-details
                readonly
                color="primary"
                class="pointer-events-none" 
              >
                <template #label>
                  <span :class="item.isChecked ? 'text-high-emphasis font-weight-medium' : 'text-grey'">
                    {{ item.label }}
                  </span>
                </template>
              </v-checkbox>
            </v-col>
          </v-row>
        </div>
      </div>

    </v-card>
  </v-container>
</template>

<style scoped>
.pointer-events-none {
  pointer-events: none;
}
</style>