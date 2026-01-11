<script setup lang="ts">
import { useField } from 'vee-validate';
import { toRef } from 'vue';

interface Props {
  name: string;
  label?: string;
  placeholder?: string;
  rows?: string | number;
  maxLength?: number;
}

const props = withDefaults(defineProps<Props>(), {
  rows: 3,
  maxLength: 250,
});

const nameRef = toRef(props, 'name');
const { value, errorMessage, handleBlur } = useField(nameRef);
</script>

<template>
  <div class="mb-4">
    <v-textarea
      v-model="value"
      :id="name"
      :error-messages="errorMessage"
      :label="label"
      :placeholder="placeholder"
      :rows="rows"
      :maxlength="maxLength"
      counter
      variant="outlined"
      color="primary"
      density="comfortable"
      hide-details="auto"
      no-resize 
      @blur="handleBlur"
    ></v-textarea>
  </div>
</template>

<style scoped>
:deep(.v-field__input) {
  /* resize: none; // Já garantido pela prop no-resize */
}
</style>