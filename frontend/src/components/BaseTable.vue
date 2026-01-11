<script setup lang="ts">
export interface TableHeader {
  title: string;
  key: string;
  align?: 'start' | 'end' | 'center';
  sortable?: boolean;
  width?: string | number;
}

interface Props {
  headers: TableHeader[];
  items: any[];
  loading?: boolean;
}

withDefaults(defineProps<Props>(), {});
</script>

<template>
  <v-card elevation="2" class="rounded-lg border-thin">
    <v-data-table
      :headers="headers"
      :items="items"
      :loading="loading"
      :items-per-page="-1"
      :hide-default-footer="true"
      hover
      density="comfortable"
    >
      <template v-for="(_, name) in $slots" #[name]="slotData">
        <slot :name="name" v-bind="slotData || {}"></slot>
      </template>
      
      <template #no-data>
        <div class="pa-8 text-center text-grey-darken-1">
          <v-icon icon="mdi-file-remove-outline" size="40" class="mb-2"></v-icon>
          <div class="text-body-1">Nenhum registro encontrado.</div>
        </div>
      </template>

      <template #loading>
        <v-skeleton-loader type="table-row@6"></v-skeleton-loader>
      </template>

    </v-data-table>
  </v-card>
</template>