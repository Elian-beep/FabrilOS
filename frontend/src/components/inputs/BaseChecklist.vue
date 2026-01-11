<script setup lang="ts">
import { useField } from 'vee-validate';
import { toRef } from 'vue';

export interface ChecklistItem {
  id: number | string;
  label: string;
}

interface Props {
  name: string;
  label?: string;
  items: ChecklistItem[];
  color?: string;
}

const props = withDefaults(defineProps<Props>(), {
  color: 'primary',
  items: () => [],
});

const nameRef = toRef(props, 'name');
const { value, errorMessage } = useField<any[]>(nameRef);

</script>

<template>
  <div class="mb-4">
    <label v-if="label" class="v-label text-body-2 font-weight-medium mb-1 text-grey-darken-1">
      {{ label }}
    </label>

    <div class="d-flex flex-wrap ga-4 py-2 px-1 border rounded" :class="{'border-error': !!errorMessage}">
      
      <v-checkbox
        v-for="item in items"
        :key="item.id"
        v-model="value"
        :label="item.label"
        :value="item.id"
        :color="color"
        hide-details
        density="compact"
        class="flex-grow-0 mr-4"
      ></v-checkbox>

      <div v-if="items.length === 0" class="text-caption text-grey pa-2">
        Nenhuma opção disponível.
      </div>
    </div>

    <div v-if="errorMessage" class="v-input__details mt-1">
      <div class="v-messages" role="alert">
        <div class="v-messages__message text-error">{{ errorMessage }}</div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.border-error {
  border-color: rgb(var(--v-theme-error)) !important;
}

:deep(.v-selection-control) {
  min-height: auto !important;
}
</style>