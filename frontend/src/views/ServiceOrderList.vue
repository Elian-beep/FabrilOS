<script setup lang="ts">
import { useMutation, useQuery, useQueryClient } from '@tanstack/vue-query';
import { useRouter } from 'vue-router';
import { ordersService } from '@/api/modules/ordersService';
import BaseTable, { type TableHeader } from '@/components/BaseTable.vue';


const router = useRouter();

const queryClient = useQueryClient();

const headers: TableHeader[] = [
  { title: '# ID', key: 'id', width: '80px', align: 'start' },
  { title: 'Título da OS', key: 'title', align: 'start' },
  { title: 'Data de Criação', key: 'createdAt', align: 'start' },
  { title: 'Ações', key: 'actions', width: '100px', align: 'end', sortable: false },
];

const { data: orders, isLoading, isError } = useQuery({
  queryKey: ['service-orders'],
  queryFn: ordersService.getAll,
  staleTime: 1000 * 60 * 5,
});

const { mutate: deleteOrder, isPending: isDeleting } = useMutation({
  mutationFn: (id: number) => ordersService.delete(id),
  
  onSuccess: () => {
    queryClient.invalidateQueries({ queryKey: ['service-orders'] });
  },
  
  onError: (error) => {
    console.error(error);
    alert('Erro ao excluir. Tente novamente.');
  }
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
  const confirmacao = window.confirm("Tem certeza que deseja excluir");
  
  if (confirmacao) {
    deleteOrder(id);
  }
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
        <div class="d-flex flex-row">
          <v-tooltip text="Visualizar Detalhes" location="top">
            <template #activator="{ props }">
              <v-btn 
                v-bind="props"
                icon="mdi-eye-outline" 
                variant="text" 
                color="primary" 
                density="comfortable"
                class="mr-1"
                @click="router.push(`/orders/${item.id}`)"
              ></v-btn>
            </template>
          </v-tooltip>
  
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
        </div>
      </template>

    </BaseTable>
  </v-container>
</template>