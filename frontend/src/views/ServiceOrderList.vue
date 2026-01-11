<script setup lang="ts">
import { useQuery } from '@tanstack/vue-query';
import { useRouter } from 'vue-router';
import { ordersService } from '@/api/modules/ordersService';
import BaseTable, { type TableHeader } from '@/components/BaseTable.vue';

const router = useRouter();

const headers: TableHeader[] = [
  { title: '# ID', key: 'id', width: '80px', align: 'start' },
  { title: 'Título da OS', key: 'title', align: 'start' },
  { title: 'Data de Criação', key: 'createdAt', align: 'end' },
  { title: 'Ações', key: 'actions', width: '100px', align: 'end', sortable: false },
];

const { data: orders, isLoading, isError } = useQuery({
  queryKey: ['service-orders'],
  queryFn: ordersService.getAll,
  staleTime: 1000 * 60 * 5,
});

const formatDate = (dateString: string) => {
  if (!dateString) return '-';
  try {
    return new Date(dateString).toLocaleDateString('pt-BR', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  } catch (e) {
    return dateString;
  }
};

const handleCreateNew = () => {
  router.push('/orders/new');
};

const handleDelete = (id: number) => {
  console.log('Deletar OS ID:', id);
  alert(`Deletar a ordem ${id}? (Funcionalidade futura)`);
};
</script>

<template>
  <v-container>
    <div class="d-flex justify-space-between align-center mb-6">
      <div>
        <h1 class="text-h5 text-primary font-weight-bold">Ordens de Serviço</h1>
        <p class="text-subtitle-2 text-grey">Gerencie suas demandas aqui</p>
      </div>
      
      <v-btn 
        color="primary" 
        prepend-icon="mdi-plus" 
        elevation="2"
        @click="handleCreateNew"
      >
        Nova OS
      </v-btn>
    </div>

    <v-alert
      v-if="isError"
      type="error"
      title="Erro ao carregar"
      text="Não foi possível buscar as ordens de serviço. Verifique sua conexão."
      variant="tonal"
      class="mb-4"
    ></v-alert>

    <BaseTable 
      :headers="headers" 
      :items="orders || []" 
      :loading="isLoading"
    >
      
      <template #item.createdAt="{ item }">
        <span class="text-grey-darken-2 font-weight-medium">
          {{ formatDate(item.createdAt) }}
        </span>
      </template>

      <template #item.actions="{ item }">
        <v-tooltip text="Excluir OS" location="top">
          <template #activator="{ props }">
            <v-btn 
              v-bind="props"
              icon="mdi-trash-can-outline" 
              variant="text" 
              color="error" 
              density="comfortable"
              @click="handleDelete(item.id)"
            ></v-btn>
          </template>
        </v-tooltip>
      </template>

    </BaseTable>
  </v-container>
</template>